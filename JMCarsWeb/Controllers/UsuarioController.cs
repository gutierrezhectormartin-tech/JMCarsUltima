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
            bool enviado = await _usuarioService.RecuperarContrasena(email);

            if (enviado)
                ViewBag.Mensaje = "Si el email existe, se enviaron instrucciones a tu correo.";
            else
                ViewBag.Error = "Ocurrió un error. Intentá nuevamente.";

            return View();
        }
    }
}