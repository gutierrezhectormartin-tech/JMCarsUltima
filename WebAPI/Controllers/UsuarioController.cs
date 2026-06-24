using Logica;
using Logica.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modelo;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

        public class UsuarioController : ControllerBase
    {
        private readonly ILogicaUsuario _logicaUsuario;
        private readonly IEmailService _servicioEmail;
        private readonly IConfiguration _configuracion;

        public UsuarioController(IEmailService servicioEmail, IConfiguration configuracion)
        {
            _logicaUsuario = FabricaLogica.GetInstancia().GetLogicaUsuario();
            _servicioEmail = servicioEmail;
            _configuracion = configuracion;
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

        [HttpPost("recuperar-contrasena")]
        public async Task<IActionResult> RecuperarContrasena([FromBody] string pEmail)
        {
            try
            {
                TokenRecuperacion token = _logicaUsuario.RecuperarContrasena(pEmail);

                if (token == null)
                {
                    return Ok(new { mensaje = "De existir el mail ingresado se enviarán instrucciones a su casilla." });
                }

                string link = $"https://localhost:7242/Usuario/ResetearContrasena?token={token.Token}";

                await _servicioEmail.EnviarCorreoRecuperacion(pEmail, "Usuario JMCars", link);

                return Ok(new { mensaje = "De existir el mail ingresado se enviarán instrucciones a su casilla." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("resetear-contrasena")]
        public IActionResult ResetearContrasena([FromBody] ResetearContrasenaRequest request)
        {
            try
            {
                bool exito = _logicaUsuario.ResetearContrasena(request.Token, request.NuevaContrasena);

                if(!exito)
                {
                    return BadRequest(new { mensaje = "El enlace no es válido o esta vencido" });
                }

                return Ok(new { mensaje = "Contraseña actualizada correctamente" });
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
    public class ResetearContrasenaRequest
    {
        public string Token { get; set; }
        public string NuevaContrasena { get; set; }
    }

}
