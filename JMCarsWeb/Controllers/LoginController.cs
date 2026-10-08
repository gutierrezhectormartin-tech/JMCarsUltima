using JMCarsWeb.Services;
using JMCarsWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using JMCarsWeb.DTOs;

namespace JMCarsWeb.Controllers
{
    public class LoginController : Controller
    {
        private UsuarioService _usuarioService;
        private EstadisticasService _estadisticasService;

        public LoginController(UsuarioService usuarioService, EstadisticasService estadisticasService)
        {
            _usuarioService = usuarioService;
            _estadisticasService = estadisticasService;
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
            UsuarioDTO? usuarioLogueado;
            try
            {
                usuarioLogueado = await _usuarioService.Login(model.Email, model.Contrasena);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index", model);
            }

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
                case 1:
                    List<string> avisos = new List<string>();
                    bool funcionandoCorrectamente = await _usuarioService.EstadoVerificadorFuncionandoCorrectamente();
                    if(!funcionandoCorrectamente)
                    {
                        avisos.Add("El servicio de notificacion de Solicitudes Vencidas no funciona correctamente");
                    }
                    try
                    {
                        EstadisticasDTO conteos = await _estadisticasService.ObtenerConteos();
                        if(conteos.CantSolicitudesPendientesVencidas > 0)
                        {
                            avisos.Add("Hay " + conteos.CantSolicitudesPendientesVencidas + " solicitudes vencidas");
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if(avisos.Count > 0)
                    {
                        TempData["Advertencia"] = string.Join("|", avisos);
                    }
                    return RedirectToAction("Index", "Home");
                case 2:
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
