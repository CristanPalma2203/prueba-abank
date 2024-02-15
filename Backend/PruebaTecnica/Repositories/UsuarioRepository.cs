using Dapper;
using PruebaTecnica.Data;
using PruebaTecnica.DTO;
using PruebaTecnica.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PruebaTecnica.Repositories
{
    public class UsuarioRepository
    {
        private readonly DatabaseContext _db;
        private readonly string _jwtKey;
        private readonly int _jwtExpirationDays;

        public UsuarioRepository(DatabaseContext db, IConfiguration configuration)
        {
            _db = db;
            _jwtKey = configuration.GetValue<string>("JwtSecret");
            _jwtExpirationDays = configuration.GetValue<int>("JwtExpirationDays");
        }

        public async Task<bool> CreateUsuario(string nombreUsuario, string password)
        {
            var passwordHash = GetPasswordHash(password);

            string sql = @"
                INSERT INTO Usuario (Usuario, Password)
                VALUES (@Usuario, @Password)";

            using var connection = _db.Connection;
            connection.Open();
            int rowsAffected = await connection.ExecuteAsync(sql, new { Usuario = nombreUsuario, Password = Convert.ToBase64String(passwordHash) });
            return rowsAffected > 0;
        }

        public async Task<Usuario?> Authenticate(string nombreUsuario, string password)
        {
            string sql = @"
                SELECT Id, Usuario as NombreUsuario, Password, IdEmpleado FROM Usuario
                WHERE Usuario = @Usuario";


            using var connection = _db.Connection;
            connection.Open();
            var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Usuario = nombreUsuario });
            if (usuario != null && VerificarPasswordHash(password, Convert.FromBase64String(usuario.Password)))
            {
                return usuario;
            }

            return null;
        }

        public async Task<Usuario?> GetUsuarioById(int usuarioId)
        {
            string sql = @"
                SELECT Id, Usuario as NombreUsuario, Password, IdEmpleado FROM Usuario
                WHERE Id = @Id";


            using var connection = _db.Connection;
            connection.Open();
            var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = usuarioId });

            return usuario;
        }

        public string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(_jwtExpirationDays), // Tiempo de expiración del token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var usuarioId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                return usuarioId;
            }
            catch
            {
                return null;
            }
        }


        private static bool VerificarPasswordHash(string password, byte[] hash)
        {
            var computedHash = GetPasswordHash(password);
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static byte[] GetPasswordHash(string password)
        {
            using var sha512 = SHA512.Create();
            var encoded = Encoding.UTF8.GetBytes(password);
            var computedHash = sha512.ComputeHash(encoded);
            return computedHash;
        }
    }
}
