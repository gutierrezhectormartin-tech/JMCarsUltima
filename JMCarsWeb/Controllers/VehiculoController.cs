using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Modelo;
using System.Security.Cryptography.X509Certificates;

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
            int? idUsuarioSession = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuarioSession == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (fotoInput == null || fotoInput.Length == 0)
            {
                ViewBag.Error = "La fotografía del vehículo es obligatoria.";
                return View(vehiculo);
            }

            string latRaw = Request.Form["Latitud"];
            string lngRaw = Request.Form["Longitud"];

            if (string.IsNullOrEmpty(latRaw) || string.IsNullOrEmpty(lngRaw))
            {
                ViewBag.Error = "Debe seleccionar una ubicación válida en el mapa.";
                return View(vehiculo);
            }

            try
            {
                // Guardado físico de la foto en tu carpeta "images" existente
                string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Como la carpeta ya existe, no hace falta crearla, pero esto no molesta si lo dejás
                if (!Directory.Exists(rutaCarpeta))
                {
                    Directory.CreateDirectory(rutaCarpeta);
                }

                string nombreArchivo = Path.GetFileName(fotoInput.FileName);
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await fotoInput.CopyToAsync(stream);
                }

                // Importante: al guardar en la DB, guardamos la ruta relativa que el HTML va a entender
                vehiculo.Fotografia = new List<string> { "images/" + nombreArchivo };

                // Armado del Vendedor
                vehiculo.Vendedor = new Cliente
                {
                    IdUsuario = idUsuarioSession.Value,
                    NombreCompleto = HttpContext.Session.GetString("NombreUsuario") ?? "Vendedor Test",
                    Email = HttpContext.Session.GetString("EmailUsuario") ?? "test@vendedor.com",
                    Cedula = HttpContext.Session.GetString("CedulaUsuario") ?? "12345678",
                    Telefono = HttpContext.Session.GetString("TelefonoUsuario") ?? "099123456"
                };

                // Armado del Modelo y Marca
                string nombreModeloEscrito = Request.Form["Modelo.Nombre"];
                string nombreMarcaEscrito = Request.Form["Marca.Nombre"];

                vehiculo.Modelo = new Modelos
                {
                    IdModelo = 1,
                    Modelo = !string.IsNullOrEmpty(nombreModeloEscrito) ? nombreModeloEscrito : "No especificado",
                    Marca = new Marcas
                    {
                        IdMarca = 1,
                        NombreMarca = !string.IsNullOrEmpty(nombreMarcaEscrito) ? nombreMarcaEscrito : "No especificada"
                    }
                };

                // Parseo de coordenadas unificado con estilo Buscar
                vehiculo.Latitud = Math.Round(decimal.Parse(latRaw, System.Globalization.CultureInfo.InvariantCulture), 6);
                vehiculo.Longitud = Math.Round(decimal.Parse(lngRaw, System.Globalization.CultureInfo.InvariantCulture), 6);

                bool exitoLlamadaApi = await _vehiculoService.RegistrarVehiculo(vehiculo);

                if (!exitoLlamadaApi)
                {
                    ViewBag.Error = "La API rechazó los datos. Revisa la consola o los logs de la API.";
                    return View(vehiculo);
                }

                return RedirectToAction("MisVehiculos");
            }
            catch (Exception)
            {
                ViewBag.Error = "Ocurrio un error inesperado intente nuevamente";
                return View(vehiculo);
            }
        }


        [HttpGet]
        public IActionResult Buscar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(string latCli, string lonCli, int radioKM, string? direccion = null, int? idMarca = null, decimal? precioMax = null)
        {
            if(latCli == null || lonCli == null)
            {
                ViewBag.Error = "Debe de seleccionar una direccion correcta.";
                return View();
            }
            decimal lat = decimal.Parse(latCli, System.Globalization.CultureInfo.InvariantCulture);
            decimal lon = decimal.Parse(lonCli, System.Globalization.CultureInfo.InvariantCulture);

            ViewBag.LatCli = lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
            ViewBag.LonCli = lon.ToString(System.Globalization.CultureInfo.InvariantCulture);
            ViewBag.RadioKM = radioKM;
            ViewBag.Direccion = direccion;

            try
            {
                List<Vehiculo> vehiculos = await _vehiculoService.BuscarGeneral(lat, lon, radioKM, idMarca, precioMax);

                if (vehiculos == null)
                {
                    ViewBag.Error = "Ocurrió un error al realizar la busqueda. Intente nuevamnte";
                    return View();
                }

                if (!vehiculos!.Any())
                {
                    ViewBag.Mensaje = "No se encontraron vehiculos en este radio";
                    return View(new List<Vehiculo>());
                }

                return View(vehiculos);
            }
            catch (Exception)
            {
                ViewBag.Error = "Ocurrio un error inesperado intente nuevamente";
                return View();
            }
        }
    }
}