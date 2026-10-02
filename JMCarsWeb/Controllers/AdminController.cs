using JMCarsWeb.DTOs;
using JMCarsWeb.Services;
using JMCarsWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JMCarsWeb.Controllers
{
    public class AdminController : Controller
    {
        private VehiculoService _vehiculoService;
        private EscribanoService _escribanoService;
        private ClienteService _clienteService;

        public AdminController(VehiculoService vehiculoService, EscribanoService escribanoService, ClienteService clienteService)
        {
            _vehiculoService = vehiculoService;
            _escribanoService = escribanoService;
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Vehiculos()
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                List<VehiculoDTO> vehiculos = await _vehiculoService.ListarVehiculos();
                return View(vehiculos);
            }

            catch (Exception ex )
            {
                TempData["Error"] =  ex.Message;
                return View(new List<VehiculoDTO>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, int idEstado)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _vehiculoService.CambiarEstadoVehiculo(id, idEstado);
                TempData["Mensaje"] = "El estado del vehiculo se ha cambiado exitosamente";
               
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Vehiculos");
        }

        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            UsuariosViewModel modelo = new UsuariosViewModel();

            try
            {
                modelo.Clientes = await _clienteService.ListarTodos();
                modelo.Escribanos = await _escribanoService.ListarTodos();
                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActivarCliente(int id)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _clienteService.Activar(id);
                TempData["Mensaje"] = "La cuenta del cliente fue activada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        public async Task<IActionResult> InactivarCliente(int id)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _clienteService.Inactivar(id);
                TempData["Mensaje"] = "La cuenta del cliente fue inactivada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        public async Task<IActionResult> ActivarEscribano(int id)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _escribanoService.Activar(id);
                TempData["Mensaje"] = "La cuenta del escribano fue activada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        public async Task<IActionResult> InactivarEscribano(int id)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _escribanoService.Inactivar(id);
                TempData["Mensaje"] = "La cuenta del escribano fue inactivada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }

    }
}
