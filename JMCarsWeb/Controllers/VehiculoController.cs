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
            int? idUsuarioInt = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuarioInt == null)
            {
                return RedirectToAction("Index", "Login");
            }

            string idUsuarioStr = idUsuarioInt.Value.ToString();

            List<Vehiculo> misVehiculos = await _vehiculoService.ListarMisVehiculos(idUsuarioStr);

            return View(misVehiculos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            int? idUsuarioInt = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuarioInt == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Vehiculo vehiculo, IFormFile fotoInput)
        {
            // 1. Procesar la foto
            if (fotoInput != null && fotoInput.Length > 0)
            {
                vehiculo.Fotografia = new List<string> { fotoInput.FileName };
            }
            else
            {
                ViewBag.Error = "La fotografía del vehículo es obligatoria.";
                return View(vehiculo);
            }

            // 2. Intentar recuperar los datos de la Session
            int? idUsuarioSession = HttpContext.Session.GetInt32("IdUsuario");

            // Si no hay usuario logueado en la Session, mandamos al Login
            if (idUsuarioSession == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Leemos las strings de la Session (Si vienen null, les ponemos un valor hardcodeado para testear la API)
            string nombreSession = HttpContext.Session.GetString("NombreUsuario") ?? "Vendedor Test";
            string emailSession = HttpContext.Session.GetString("EmailUsuario") ?? "test@vendedor.com";
            string cedulaSession = HttpContext.Session.GetString("CedulaUsuario") ?? "12345678";
            string telefonoSession = HttpContext.Session.GetString("TelefonoUsuario") ?? "099123456";

            // 3. Armamos el objeto Vendedor sin que quede ningún campo obligatorio en null
            vehiculo.Vendedor = new Cliente
            {
                IdUsuario = idUsuarioSession.Value,
                NombreCompleto = nombreSession,
                Email = emailSession,
                Cedula = cedulaSession,
                Telefono = telefonoSession
            };

            // 4. Capturar el Modelo y la Marca reales de la pantalla
            vehiculo.Modelo = new Modelos();

            // 1. Leemos el texto del Modelo escrito por el usuario en la vista
            string nombreModeloEscrito = Request.Form["Modelo.Nombre"];
            vehiculo.Modelo.Modelo = !string.IsNullOrEmpty(nombreModeloEscrito) ? nombreModeloEscrito : "No especificado";

            // SALVAMOS VALIDACIÓN INTERNA: Le dejamos el ID 1 fijo para ganarle al "IdModelo <= 0"
            vehiculo.Modelo.IdModelo = 1;

            // 2. Leemos el texto de la Marca escrito por el usuario en la vista
            string nombreMarcaEscrito = Request.Form["Marca.Nombre"];

            vehiculo.Modelo.Marca = new Marcas
            {
                IdMarca = 1, // Mantenemos el 1 por las validaciones de IDs mínimos
                NombreMarca = !string.IsNullOrEmpty(nombreMarcaEscrito) ? nombreMarcaEscrito : "No especificada"
            };
            // 4.5. RECORTAR DECIMALES USANDO CULTURA INVARIABLE (Evita que el punto se borre)
            // Leemos el texto crudo que viene directo del HTML antes de que .NET lo intente convertir solo
            string latRaw = Request.Form["Latitud"];
            string lngRaw = Request.Form["Longitud"];

            // Usamos System.Globalization para entender el punto decimal del mapa
            if (decimal.TryParse(latRaw, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal latCorrecta))
            {
                vehiculo.Latitud = Math.Round(latCorrecta, 6);
            }
            else if (vehiculo.Latitud.HasValue)
            {
                // Si no lo pudo leer del Form, redondeamos lo que ya tenía
                vehiculo.Latitud = Math.Round(vehiculo.Latitud.Value, 6);
            }

            if (decimal.TryParse(lngRaw, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal lngCorrecta))
            {
                vehiculo.Longitud = Math.Round(lngCorrecta, 6);
            }
            else if (vehiculo.Longitud.HasValue)
            {
                vehiculo.Longitud = Math.Round(vehiculo.Longitud.Value, 6);
            }

            // 5. Envío a la API
            try
            {
                bool exitoLlamadaApi = await _vehiculoService.RegistrarVehiculo(vehiculo);

                if (exitoLlamadaApi)
                {
                    return RedirectToAction("MisVehiculos");
                }
                else
                {
                    ViewBag.Error = "La API rechazó los datos. Revisa la consola o los logs de la API.";
                    return View(vehiculo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Atención: Ocurrió un error al intentar comunicar con la API: " + ex.Message;
                return View(vehiculo);
            }
        }
    }
}