using JMCarsWeb.Services;
using JMCarsWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Modelo;

namespace WebApi.Controllers
{
    public class LoginController : Controller
    {
        private UsuarioService _usuarioService;

        public LoginController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Email y contraseña son obligatorios.";
                return View("Index", model);
            }

            // Martin cambiamos la llamaada a la logica, para desacoplar por el servicio de usuarios
            Usuario usuarioLogueado = await _usuarioService.Login(model.Email, model.Contrasena);

            if (usuarioLogueado == null)
            {
                // credenciales invalidas, vuelvo a mostrar el formulario con error
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View("Index", model);
            }

            //guardamos los datos en el sesion
            HttpContext.Session.SetInt32("IdUsuario", usuarioLogueado.IdUsuario);
            HttpContext.Session.SetInt32("IdRol", (int)usuarioLogueado.RolUsu);
            HttpContext.Session.SetString("NombreCompleto", usuarioLogueado.NombreCompleto);

            // redirijo segun el rol (todos a Home por ahora)
            switch (usuarioLogueado.RolUsu)
            {
                case Rol.Administrador:
                    return RedirectToAction("Index", "Home");
                case Rol.Escribano:
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // limpio la sesion del usuario
            HttpContext.Session.Clear();
            TempData["Mensaje"] = "Se ha cerrado correctamente tu sesión";
            return RedirectToAction("Index", "Home");
        }
    }
}
