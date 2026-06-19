using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Modelo;

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
        public async Task<IActionResult> Registro(Cliente clientePasado)
        {
           
            if (!ModelState.IsValid)
            {
                return View(clientePasado);
            }


            try
            {
                await _clienteService.Registrar(clientePasado);
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
            if (idUsuario == null || idRol != (int)Rol.Cliente)
            {
                return RedirectToAction("Index", "Login");
            }


            Cliente? cliente = await _clienteService.ObtenerPorId(idUsuario.Value);

            if (cliente == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Perfil(Cliente clientePasado)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");
            if (idUsuario == null || idRol != (int)Rol.Cliente)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelState.Remove(nameof(Cliente.Contrasena));

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
            if (idUsuario == null || idRol != (int)Rol.Cliente)
            {
                return RedirectToAction("Index", "Login");
            }


            await _clienteService.Inactivar(idUsuario.Value);

            HttpContext.Session.Clear();

            return RedirectToAction("CuentaInactivada", "Home");
        }
    }
}
