using JMCarsWeb.DTOs;
using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace JMCarsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly VehiculoService _vehiculoService;

        public HomeController(VehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        public async Task<IActionResult> Index(string marca)
        {
            List<VehiculoDTO> lista = await _vehiculoService.ListarVehiculos();

            lista = lista.Where(v => v.Publicado).ToList();

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
