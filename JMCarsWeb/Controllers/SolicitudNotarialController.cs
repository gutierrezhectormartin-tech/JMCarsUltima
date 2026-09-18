using Microsoft.AspNetCore.Mvc;
using JMCarsWeb.DTOs;
using JMCarsWeb.Services;

namespace JMCarsWeb.Controllers
{
    public class SolicitudNotarialController : Controller
    {
        private readonly SolicitudNotarialService _solicitudService;
        private readonly EscribanoService _escribanoService;
        private readonly VehiculoService _vehiculoService;

        public SolicitudNotarialController(SolicitudNotarialService solicitudService, EscribanoService escribanoService, VehiculoService vehiculoService)
        {
            _solicitudService = solicitudService;
            _escribanoService = escribanoService;
            _vehiculoService = vehiculoService;
        }

        [HttpGet]
        public async Task<IActionResult> Solicitar(int idVehiculo)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debe iniciar sesión como cliente para solicitar un escribano.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                VehiculoDTO? vehiculo = await _vehiculoService.DetalleVehiculo(idVehiculo);

                if (vehiculo == null)
                {
                    TempData["Error"] = "No se encontró el vehículo.";
                    return RedirectToAction("Index", "Home");
                }

                List<EscribanoDTO>? escribanos = await _escribanoService.ListarActivos();

                if (escribanos == null || !escribanos.Any())
                {
                    TempData["Error"] = "No hay escribanos disponibles en este momento.";
                    return RedirectToAction("Detalle", "Vehiculo", new { id = idVehiculo });
                }

                ViewBag.Escribanos = escribanos;
                return View(vehiculo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error: " + ex.Message;
                return RedirectToAction("Detalle", "Vehiculo", new { id = idVehiculo });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Solicitar(int idVehiculo, int idEscribano)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debe iniciar sesión como cliente para solicitar un escribano.";
                return RedirectToAction("Index", "Login");
            }

            if (idEscribano <= 0)
            {
                TempData["Error"] = "Debe seleccionar un escribano.";
                return RedirectToAction("Solicitar", new { idVehiculo });
            }

            try
            {
                await _solicitudService.Crear(idUsuario.Value, idVehiculo, idEscribano);
                TempData["Mensaje"] = "Solicitud enviada con éxito. El escribano la revisará a la brevedad.";
                return RedirectToAction("MisSolicitudes");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Solicitar", new { idVehiculo });
            }
        }

        [HttpGet]
        public async Task<IActionResult> MisSolicitudes()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 3)
            {
                TempData["Error"] = "Debe iniciar sesión como cliente para ver sus solicitudes.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                List<SolicitudEscribanoDTO>? solicitudes = await _solicitudService.ListarPorCliente(idUsuario.Value);

                if (solicitudes == null)
                {
                    TempData["Error"] = "Ha ocurrido un error al cargar sus solicitudes.";
                    return View(new List<SolicitudEscribanoDTO>());
                }

                return View(solicitudes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error: " + ex.Message;
                return View(new List<SolicitudEscribanoDTO>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> SolicitudesPendientes()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 2)
            {
                TempData["Error"] = "Debe iniciar sesión como escribano para ver las solicitudes.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                List<SolicitudEscribanoDTO>? solicitudes = await _solicitudService.ListarPorEscribano(idUsuario.Value);

                if (solicitudes == null)
                {
                    TempData["Error"] = "Ha ocurrido un error al cargar las solicitudes.";
                    return View(new List<SolicitudEscribanoDTO>());
                }

                return View(solicitudes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ha ocurrido un error: " + ex.Message;
                return View(new List<SolicitudEscribanoDTO>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Aceptar(int idSolicitud)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 2)
            {
                TempData["Error"] = "Debe iniciar sesión como escribano para gestionar solicitudes.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _solicitudService.Aceptar(idSolicitud, idUsuario.Value);
                TempData["Mensaje"] = "Solicitud aceptada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("SolicitudesPendientes");
        }

        [HttpPost]
        public async Task<IActionResult> Rechazar(int idSolicitud)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 2)
            {
                TempData["Error"] = "Debe iniciar sesión como escribano para gestionar solicitudes.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _solicitudService.Rechazar(idSolicitud, idUsuario.Value);
                TempData["Mensaje"] = "Solicitud rechazada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("SolicitudesPendientes");
        }

        [HttpPost]
        public async Task<IActionResult> Finalizar(int idSolicitud)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol != 2)
            {
                TempData["Error"] = "Debe iniciar sesión como escribano para gestionar solicitudes.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _solicitudService.Finalizar(idSolicitud, idUsuario.Value);
                TempData["Mensaje"] = "Venta finalizada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("SolicitudesPendientes");
        }
    }
}
