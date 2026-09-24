using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
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
            catch (Exception)
            {
                TempData["Error"] = "El enlace no es válido o ya expiró";
                ViewBag.Token = ptoken;
                return View();
            }
        }

    }
}