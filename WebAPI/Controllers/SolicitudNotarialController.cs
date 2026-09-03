using Logica;
using Logica.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelo;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudNotarialController : ControllerBase
    {
        private readonly ILogicaSolicitudNotarial _logicaSolicitud;

        public SolicitudNotarialController()
        {
            _logicaSolicitud = FabricaLogica.GetInstancia().GetLogicaSolicitudNotarial();
        }

        [HttpPost("crear")]
        public IActionResult Crear([FromBody] SolicitudNotarialRequest solicitud)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (solicitud.IdCliente <= 0 || solicitud.IdVehiculo <= 0 || solicitud.IdEscribano <= 0)
            {
                return BadRequest(new { mensaje = "Debe indicar el cliente, el vehículo y el escribano." });
            }

            try
            {
                _logicaSolicitud.Crear(solicitud.IdCliente, solicitud.IdVehiculo, solicitud.IdEscribano);
                return Ok(new { mensaje = "Solicitud enviada con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}/aceptar/{idEscribano}")]
        public IActionResult Aceptar(int id, int idEscribano)
        {
            try
            {
                _logicaSolicitud.Aceptar(id, idEscribano);
                return Ok(new { mensaje = "Solicitud aceptada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}/rechazar/{idEscribano}")]
        public IActionResult Rechazar(int id, int idEscribano)
        {
            try
            {
                _logicaSolicitud.Rechazar(id, idEscribano);
                return Ok(new { mensaje = "Solicitud rechazada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}/finalizar/{idEscribano}")]
        public IActionResult Finalizar(int id, int idEscribano)
        {
            try
            {
                _logicaSolicitud.Finalizar(id, idEscribano);
                return Ok(new { mensaje = "Venta finalizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            try
            {
                SolicitudEscribano solicitud = _logicaSolicitud.ObtenerPorId(id);

                if (solicitud == null)
                {
                    return NotFound(new { mensaje = "Solicitud no encontrada" });
                }

                return Ok(solicitud);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("por-cliente/{idCliente}")]
        public IActionResult ListarPorCliente(int idCliente)
        {
            try
            {
                List<SolicitudEscribano> solicitudes = _logicaSolicitud.ListarPorCliente(idCliente);

                if (solicitudes == null || !solicitudes.Any())
                {
                    return NotFound(new { mensaje = "No se han encontrado solicitudes para este cliente" });
                }

                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("por-escribano/{idEscribano}")]
        public IActionResult ListarPorEscribano(int idEscribano)
        {
            try
            {
                List<SolicitudEscribano> solicitudes = _logicaSolicitud.ListarPorEscribano(idEscribano);

                if (solicitudes == null || !solicitudes.Any())
                {
                    return NotFound(new { mensaje = "No se han encontrado solicitudes para este escribano" });
                }

                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class SolicitudNotarialRequest
    {
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }
        public int IdEscribano { get; set; }
    }
}
