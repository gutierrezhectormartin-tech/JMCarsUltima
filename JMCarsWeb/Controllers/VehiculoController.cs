using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Modelo;

namespace JMCarsWeb.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly VehiculoService _vehiculoService;

        public VehiculoController(VehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        public async Task<IActionResult> Listar()
        {
            List<Vehiculo> vehiculos = await _vehiculoService.ListarVehiculos();
            return View(vehiculos);
        }

        [HttpGet]
        public IActionResult PruebaMapa()
        {
            return View();
        }
    }
}