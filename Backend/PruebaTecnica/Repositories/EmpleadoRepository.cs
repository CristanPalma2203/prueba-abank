using Dapper;
using Npgsql;
using PruebaTecnica.Data;
using PruebaTecnica.DTO;
using PruebaTecnica.Models;
using System.Data;

namespace PruebaTecnica.Repositories
{
    public class EmpleadoRepository
    {
        private readonly DatabaseContext _db;

        public EmpleadoRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<EmpleadoDepartamentoDTO>> GetAllEmpleadosWithDepartamento()
        {
            var sql = @"
                SELECT e.Id, e.Nombres, e.Apellidos, e.Telefono, e.Correo, d.Nombre AS NombreDepartamento
                FROM Empleado e
                JOIN Departamento d ON e.IdArea = d.Id";
            using var connection = _db.Connection;
            connection.Open();
            var empleados = await connection.QueryAsync<EmpleadoDepartamentoDTO>(sql);
            return empleados;
        }

        public Empleado? GetEmpleadoById(int id)
        {
            var sql = @"SELECT * FROM Empleado WHERE Id = @Id";
            using var connection = _db.Connection;
            connection.Open();
            var empleado = connection.QueryFirstOrDefault<Empleado>(sql, new { Id = id});
            return empleado;
        }

        public int CreateEmpleado(Empleado empleado)
        {
            using var connection = _db.Connection;
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@p_Nombres", empleado.Nombres);
            parameters.Add("@p_Apellidos", empleado.Apellidos);
            parameters.Add("@p_Telefono", empleado.Telefono);
            parameters.Add("@p_Correo", empleado.Correo);
            //parameters.Add("@p_FechaContratacion", empleado.FechaContratacion);
            parameters.Add("@p_IdArea", empleado.IdArea);

            // Definir el parámetro de salida
            parameters.Add("@p_EmpleadoId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("CrearEmpleado", parameters, commandType: CommandType.StoredProcedure);

            int empleadoId = parameters.Get<int>("@p_EmpleadoId");
            return empleadoId;
        }

        public void UpdateEmpleado(Empleado empleado)
        {
            using var connection = _db.Connection;
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@p_Id", empleado.Id);
            parameters.Add("@p_Nombres", empleado.Nombres);
            parameters.Add("@p_Apellidos", empleado.Apellidos);
            parameters.Add("@p_Telefono", empleado.Telefono);
            parameters.Add("@p_Correo", empleado.Correo);
            //parameters.Add("@p_FechaContratacion", empleado.FechaContratacion);
            parameters.Add("@p_IdArea", empleado.IdArea);
            connection.Query("public.EditarEmpleado", parameters, commandType: CommandType.StoredProcedure);

        }
    }
}
