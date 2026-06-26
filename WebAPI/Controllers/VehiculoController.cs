using Logica;
using Logica.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelo;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly ILogicaVehiculo _logicaVehiculo;

        public VehiculoController()
        {
            _logicaVehiculo = FabricaLogica.GetLogicaVehiculo();
        }


        [HttpGet("listar")]
        public IActionResult ListarVehiculos()
        {
            try
            {
                List<Vehiculo> vehiculos = _logicaVehiculo.ListarVehiculos();
                if(vehiculos == null || !vehiculos.Any())
                {
                    return NotFound(new { mensaje = "No se han encontrado vehiculos a listar" });
                }
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("mis-vehiculos/{idUsuario}")]
        public IActionResult ListarMisVehiculos(string idUsuario)
        {
            try
            {
                // Validación básica de que nos manden un ID
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return BadRequest(new { mensaje = "El ID de usuario es requerido." });
                }

                List<Vehiculo> misVehiculos = _logicaVehiculo.ListarMisVehiculos(idUsuario);

                if (misVehiculos == null || !misVehiculos.Any())
                {
                    return NotFound(new { mensaje = "No se han encontrado vehículos para este usuario" });
                }

                return Ok(misVehiculos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
