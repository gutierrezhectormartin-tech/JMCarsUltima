using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using JMCarsWeb.DTOs;

namespace WebApi.Controllers
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
                bool exito = await _clienteService.Registrar(clientePasado, aceptaTerminos);

                if(!exito)
                {
                    ViewBag.Error = "No se puede completar el registro. Verifique sus datos";
                    return View(clientePasado);
                }

                TempData ["Mensaje"] = "Registro realizado con éxito."; //le agregue aca el tempdata porque nunca iba a funcionarte con viewbag, luego de un redirect el viewbag se pierde te acordas martin?
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: No se pudo completar el registro. Verifica tus datos";
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


            ClienteDTO? cliente = await _clienteService.ObtenerPorId(idUsuario.Value);

            if (cliente == null)
            {
                return RedirectToAction("Index", "Login");
            }

            HttpContext.Session.SetString("EmailCliente", cliente.Email);
            HttpContext.Session.SetString("CedulaCliente", cliente.Cedula);

            return View(cliente);
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

            clientePasado.Email = HttpContext.Session.GetString("EmailCliente");
            clientePasado.Contrasena = HttpContext.Session.GetString("CedulaCliente");

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.Error = string.Join(" | ", errores);
                return View(clientePasado);
            }

            if (!ModelState.IsValid)
            {
                return View(clientePasado);
            }

            try
            {
                clientePasado.IdUsuario = idUsuario.Value;
                await _clienteService.ActualizarPerfil(clientePasado);
                HttpContext.Session.SetString("NombreCompleto", clientePasado.NombreCompleto);
                ViewBag.Mensaje = "El perfil se actualizó correctamente";
                return View(clientePasado);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se pudo actualizar el perfil.";
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


            await _clienteService.Inactivar(idUsuario.Value);

            HttpContext.Session.Clear();

            return RedirectToAction("CuentaInactivada", "Home");
        }
    }
}
