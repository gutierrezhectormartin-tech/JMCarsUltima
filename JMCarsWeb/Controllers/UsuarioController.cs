using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace JMCarsWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult RecuperarContrasena()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarContrasena(string email)
        {
            try
            {
                await _usuarioService.RecuperarContrasena(email);
                TempData["Mensaje"] = "Si el email existe, se enviaron instrucciones a tu correo.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error. Intentá nuevamente.";
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetearContrasena(string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "El enlace no es válido.";
                return View("RecuperarContrasena");
            }

            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetearContrasena(string ptoken, string pNuevaContrasena)
        {
            try
            {
                await _usuarioService.ResetearContrasena(ptoken, pNuevaContrasena);
                TempData["Mensaje"] = "Tu contraseña fue reseteada correctamente. Inicia sesión con tu nueva contraseña.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                ViewBag.Token = ptoken;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(string contrasenaActual, string contrasenaNueva, string repetirContrasenaNueva)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idUsuario == null || (idRol != 2 && idRol != 3))
            {
                TempData["Error"] = "Ningún usuario Logueado";
                return RedirectToAction("Index", "Login");
            }

            string controladorPerfil = idRol == 2 ? "Escribano" : "Cliente";
            string? email = idRol == 2 ? HttpContext.Session.GetString("EmailEscribano") : HttpContext.Session.GetString("EmailCliente");
            if(string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "No se ha podido confirmar usuario, logeese nuevamente";
                return RedirectToAction("Index", "Login");
            }
            if(contrasenaNueva != repetirContrasenaNueva)
            {
                TempData["Error"] = "Las contraseña nueva y su verificacion no son iguales";
                return RedirectToAction("Perfil", controladorPerfil);
            }
            try
            {
                await _usuarioService.ResetearContrasena(email, contrasenaActual, contrasenaNueva);
                TempData["Mensaje"] = "Tu contraseña se cambio con exito";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Perfil", controladorPerfil);
        }

    }
}