using JMCarsWeb.DTOs;
using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace JMCarsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly VehiculoService _vehiculoService;
        private readonly EstadisticasService _estadisticasService;

        public HomeController(VehiculoService vehiculoService, EstadisticasService estadisticasService)
        {
            _vehiculoService = vehiculoService;
            _estadisticasService = estadisticasService;
        }

        public async Task<IActionResult> Index(string marca)
        {
            List<VehiculoDTO> lista = await _vehiculoService.ListarVehiculos();

            lista = lista.Where(v => v.IdEstadoPublicacion == 2).ToList();

            ViewBag.Destacados = lista.Where(v => v.Fotografia != null && v.Fotografia.Any()).OrderByDescending(v => v.IdVehiculo)
                                            .Take(6)
                                            .ToList();

            try
            {
                ViewBag.Conteos = await _estadisticasService.ObtenerConteos();
            }
            catch (Exception)
            {
                ViewBag.Conteos = null;
            }
            if (!string.IsNullOrEmpty(marca))
            {
                lista = lista.Where(v =>
                    v.Modelo.Marca.NombreMarca
                    .ToLower()
                    .Contains(marca.ToLower()))
                    .ToList();
            }

            return View(lista);
        }

        [HttpGet]
        public IActionResult CuentaInactivada()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TerminosYCondiciones()
        {
            return View();
        }
    }
}
