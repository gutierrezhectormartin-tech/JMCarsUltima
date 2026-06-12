using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Modelo;

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
            List<Vehiculo> lista = await _vehiculoService.ListarVehiculos();

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
    }
}
