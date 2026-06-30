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

        [HttpGet]
        public async Task<IActionResult> MisVehiculos()
        {
            // 1. Lo leemos como INT porque así se guardó en el Login
            int? idUsuarioInt = HttpContext.Session.GetInt32("IdUsuario");

            // 2. Si es nulo (o sea, no hay sesión activa), al Login derecho viejo
            if (idUsuarioInt == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // 3. Lo pasamos a string para poder mandárselo a tu servicio de la API
            string idUsuarioStr = idUsuarioInt.Value.ToString();

            // 4. Llamamos a la API pasándole el ID correcto
            List<Vehiculo> misVehiculos = await _vehiculoService.ListarMisVehiculos(idUsuarioStr);

            return View(misVehiculos);
        }
    }
}