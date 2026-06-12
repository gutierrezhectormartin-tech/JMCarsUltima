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
            bool existe = await _usuarioService.ExisteMail(email);

            if (existe)
            {
                ViewBag.Mensaje = "Se enviaron instrucciones al correo.";
            }
            else
            {
                ViewBag.Error = "No existe una cuenta asociada a ese correo.";
            }

            return View();
        }
    }
}