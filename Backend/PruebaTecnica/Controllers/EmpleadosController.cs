using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Models;
using PruebaTecnica.Repositories;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoRepository _empleadoRepository;

        public EmpleadosController(EmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEmpleados()
        {
            var empleados = await _empleadoRepository.GetAllEmpleadosWithDepartamento();
            return Ok(empleados);
        }

        [HttpPost]
        public ActionResult<Empleado> CreateEmpleado(Empleado empleado)
        {
            int empleadoId = _empleadoRepository.CreateEmpleado(empleado);
            Empleado? newEmpleado = _empleadoRepository.GetEmpleadoById(empleadoId);
            if (newEmpleado != null)
            {
                return Ok(newEmpleado);
            }
            else
            {
                return BadRequest("No puedo crear al usuario");
            }
            
        }

        [HttpPut]
        public ActionResult<Empleado> UpdateEmpleado(Empleado empleado)
        {
            _empleadoRepository.UpdateEmpleado(empleado);
            Empleado? updatedEmpleado = _empleadoRepository.GetEmpleadoById(empleado.Id);
            if (updatedEmpleado != null)
            {
                return Ok(updatedEmpleado);
            }
            else
            {
                return BadRequest("No puedo crear al usuario");
            }
        }
    }
}
