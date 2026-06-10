using Logica;
using Logica.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Modelo;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogicaCliente _logicaCliente;

        public ClientController()
        {
            _logicaCliente = FabricaLogica.GetInstancia().GetLogicaCliente();
        }



        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logicaCliente.Registrar(cliente);
                return Ok(new { mensaje = "Registro realizado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            try
            {
                Cliente cliente = _logicaCliente.ObtenerPorId(id);
                if (cliente == null)
                {
                    return NotFound(new { mensaje = "Cliente no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("{id}")]
        public IActionResult ActualizarPerfil(int id, [FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);                
            }
            try
            {
                cliente.IdUsuario = id;
                _logicaCliente.ActualizarPerfil(cliente);

                return Ok(new { mensaje = "Perfil actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}/inactivar")]
        public IActionResult Inactivar(int id)
        {
            try
            {
                _logicaCliente.Inactivar(id);
                return Ok(new { mensaje = "La cuenta ha sido correctametne inactivada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}

