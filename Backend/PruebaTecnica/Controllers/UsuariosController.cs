using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.DTO;
using PruebaTecnica.Models;
using PruebaTecnica.Repositories;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuariosController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUsuario([FromBody] NuevoUsuarioDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var resultado = await _usuarioRepository.CreateUsuario(request.Usuario, request.Password);

            if (resultado)
            {
                return Ok("Usuario creado exitosamente.");
            }
            else
            {
                return BadRequest(String.Format("No puede crear usuario {0}", request.Usuario));
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Autenticate([FromBody] AuthenticateDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var usuario = await _usuarioRepository.Authenticate(request.Usuario, request.Password);
            if (usuario != null)
            {
                var token = _usuarioRepository.GenerateJwtToken(usuario);
                return Ok(new { token = token, usuario = new { usuarioId = usuario.Id, nombreUsuario = usuario.NombreUsuario } });
            }
            else
            {
                return BadRequest(String.Format("No puede autenticar usuario {0}", request.Usuario));
            }
        }
    }
}
