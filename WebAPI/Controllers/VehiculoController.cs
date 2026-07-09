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

        [HttpPost("registrar")]
        public IActionResult RegistrarVehiculo([FromBody] Vehiculo pVehiculo)
        {
            try
            {
                if (pVehiculo == null)
                {
                    return BadRequest(new { mensaje = "Los datos del vehículo no son válidos o vienen vacíos." });
                }

                _logicaVehiculo.Registrar(pVehiculo);

                return Ok(new { mensaje = "Vehículo registrado con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult BuscarGeneral(decimal latCli, decimal lonCli, int radioKM, int? idMarca = null, decimal? precioMax = null)
        {
            try
            {
                List<Vehiculo> vehiculos = _logicaVehiculo.BuscarGeneral(latCli, lonCli, radioKM, idMarca, precioMax);

                if(vehiculos == null || !vehiculos.Any())
                {
                    return NotFound(new { mensaje = "No se encontraron vehiculos con las condiciones de busqueda ingresadas" });
                }

                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
