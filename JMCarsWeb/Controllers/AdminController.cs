using JMCarsWeb.DTOs;
using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace JMCarsWeb.Controllers
{
    public class AdminController : Controller
    {
        private VehiculoService _vehiculoService;
        private EscribanoService _escribanoService;

        public AdminController(VehiculoService vehiculoService, EscribanoService escribanoService)
        {
            _vehiculoService = vehiculoService;
            _escribanoService = escribanoService;
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
        public async Task<IActionResult> Escribanos()
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                List<EscribanoDTO> escribanos = await _escribanoService.ListarInactivos();
                return View(escribanos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<EscribanoDTO>());
            }
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
                TempData["Mensaje"] = "El escribano fue aprobado y ya puede iniciar sesión";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Escribanos");
        }

    }
}
