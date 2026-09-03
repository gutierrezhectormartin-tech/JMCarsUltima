using JMCarsWeb.DTOs;
using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace JMCarsWeb.Controllers
{
    public class AdminController : Controller
    {
        private VehiculoService _vehiculoService;

        public AdminController(VehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
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
                List<VehiculoDTO>? vehiculos = await _vehiculoService.ListarVehiculos();

                if(vehiculos == null)
                {
                    TempData["Error"] = "Ha ocurrido un error. No se han encontrado vehiculos";
                    return View(new List<VehiculoDTO>());
                }

                return View(vehiculos);
            }

            catch (Exception ex )
            {
                TempData["Error"] = "Ha ocurrido un error." + ex.Message;
                return View(new List<VehiculoDTO>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, bool publicado)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningun usuario con permisos de adminitrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                if(publicado)
                {
                    await _vehiculoService.InactivarVehiculo(id);
                }
                else
                {
                    await _vehiculoService.ActivarVehiculo(id);
                }

                TempData["Mensaje"] = "El estado del vehiculo se ha cambiado exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error al cambiar el estado del vehiculo" + ex.Message;
                throw;
            }

            return RedirectToAction("Vehiculos");
        }

    }
}
