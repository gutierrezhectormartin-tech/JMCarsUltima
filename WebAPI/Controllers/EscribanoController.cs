using Logica;
using Logica.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelo;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EscribanoController : ControllerBase
    {
        private readonly ILogicaEscribano _logicaEscribano;

        public EscribanoController()
        {
            _logicaEscribano = FabricaLogica.GetInstancia().GetLogicaEscribano();
        }

        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] RegistroEscribanoRequest escribano)
        {
            if(!escribano.AceptaTerminos)
            {
                return BadRequest(new { mensaje = "Debe aceptar los terminos y condiciones para registrarse" });
            }
            if (string.IsNullOrEmpty(escribano.Escribano.Contrasena))
            { 
                return BadRequest(new { mensaje = "La contraseña es obligatoria." }); 
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                escribano.Escribano.FechaAceptacionTerminos = DateTime.Now;
                _logicaEscribano.Registrar(escribano.Escribano);
                return Ok(new { mensaje = "Escribano resgitrado con éxito" });
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
                Escribano escribano = _logicaEscribano.ObtenerPorId(id);
                if(escribano == null)
                {
                    return NotFound(new { mensaje = "Escribano no encontrado" });
                }
                return Ok(escribano);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarPerfil(int id, [FromBody] Escribano escribano)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                escribano.IdUsuario = id;
                _logicaEscribano.ActualizarPerfil(escribano);

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
                _logicaEscribano.Inactivar(id);
                return Ok(new { mensaje = "La cuenta ha sido correctametne inactivada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }

    public class RegistroEscribanoRequest
    {
        public Escribano Escribano { get; set; }
        public bool AceptaTerminos { get; set; }
    }
}
