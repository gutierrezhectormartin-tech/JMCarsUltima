using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using JMCarsWeb.DTOs;

namespace JMCarsWeb.Controllers
{
    public class ClienteController : Controller
    {
        private ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(ClienteDTO clientePasado, bool aceptaTerminos)
        {
           
            if (!ModelState.IsValid)
            {
                return View(clientePasado);
            }

            if(!aceptaTerminos)
            {
                ViewBag.Error = "Debe aceptar los terminos y condiciones para registrarse";
                return View(clientePasado);
            }

            try
            {
                await _clienteService.Registrar(clientePasado, aceptaTerminos);
                TempData ["Mensaje"] = "Registro realizado con éxito."; //le agregue aca el tempdata porque nunca iba a funcionarte con viewbag, luego de un redirect el viewbag se pierde te acordas martin?
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(clientePasado);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            // chequeo de autorizacion: tiene que estar logueado y ser cliente (rol = 3)
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");
            if (idUsuario == null || idRol != 3)
            {
                return RedirectToAction("Index", "Login");
            }


            try
            {
                ClienteDTO? cliente = await _clienteService.ObtenerPorId(idUsuario.Value);

                if (cliente == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                HttpContext.Session.SetString("EmailCliente", cliente.Email);
                HttpContext.Session.SetString("CedulaCliente", cliente.Cedula);

                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Perfil(ClienteDTO clientePasado)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");
            if (idUsuario == null || idRol != 3)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelState.Remove("Contrasena");
            ModelState.Remove("Email");
            ModelState.Remove("Cedula");

            clientePasado.Email = HttpContext.Session.GetString("EmailCliente")!;
            clientePasado.Cedula = HttpContext.Session.GetString("CedulaCliente")!; //cambio aca.

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["Error"] = string.Join(" | ", errores);
                return View(clientePasado);
            }

            try
            {
                clientePasado.IdUsuario = idUsuario.Value;
                await _clienteService.ActualizarPerfil(clientePasado);
                HttpContext.Session.SetString("NombreCompleto", clientePasado.NombreCompleto);
                TempData["Mensaje"] = "El perfil se actualizó correctamente";
                return View(clientePasado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(clientePasado);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Inactivar()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");
            if (idUsuario == null || idRol != 3)
            {
                return RedirectToAction("Index", "Login");
            }


            try
            {
                await _clienteService.Inactivar(idUsuario.Value);

                HttpContext.Session.Clear();

                return RedirectToAction("CuentaInactivada", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Perfil");
            }
        }
    }
}
