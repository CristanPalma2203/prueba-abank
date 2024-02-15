using Microsoft.Extensions.Configuration;
using System.Data;
using Npgsql;

namespace PruebaTecnica.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(name: "DefaultConnection");
        }

        public IDbConnection Connection => new NpgsqlConnection(_connectionString);
    }
}
