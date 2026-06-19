using Logica;
using Logica.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelo;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

        public class UsuarioController : ControllerBase
    {
        private readonly ILogicaUsuario _logicaUsuario;

        public UsuarioController()
        {
            _logicaUsuario = FabricaLogica.GetInstancia().GetLogicaUsuario();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { mensaje = "Datos Invalidos" });
            }
                
            try
            {
                Usuario usuario = _logicaUsuario.Login(request.Email, request.Contrasena);
                if(usuario == null)
                    {
                        return Unauthorized(new { mensaje = "Email o Contraseña incorrectos" });
                    }
                
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("existe-mail/{email}")]
        public IActionResult ExisteMail(string mail)
        {
            try
            {
                bool existe = _logicaUsuario.ExisteEmail(mail);
                return Ok(new { existe });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Contrasena { get; set; }

    }

}
