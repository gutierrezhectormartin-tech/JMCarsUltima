using JMCarsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using JMCarsWeb.DTOs;
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
            List<VehiculoDTO> vehiculos = await _vehiculoService.ListarVehiculos();
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

            List<VehiculoDTO> misVehiculos = await _vehiculoService.ListarMisVehiculos(idUsuarioStr);


            return View(misVehiculos);
        }
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("MisVehiculos");
            }

            VehiculoDTO vehiculo = await _vehiculoService.DetalleVehiculo(id);

            if (vehiculo == null)
            {
                return RedirectToAction("MisVehiculos");
            }

            return View("DetalleVehiculo",vehiculo);
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
        public async Task<IActionResult> Crear(VehiculoDTO vehiculo, List<IFormFile> fotoInput)
        {
            int? idUsuarioSession = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuarioSession == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (fotoInput == null || !fotoInput.Any(f => f.Length > 0))
            {
                ViewBag.Error = "Debe subir al menos una fotografía del vehículo.";
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
                string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(rutaCarpeta))
                {
                    Directory.CreateDirectory(rutaCarpeta);
                }

                var rutasGuardadas = new List<string>();
                foreach (var archivo in fotoInput.Where(f => f.Length > 0))
                {
                    string nombreArchivo = $"{Guid.NewGuid():N}_{Path.GetFileName(archivo.FileName)}";
                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await archivo.CopyToAsync(stream);
                    }

                    rutasGuardadas.Add("images/" + nombreArchivo);
                }

                vehiculo.Fotografia = rutasGuardadas;

                vehiculo.Vendedor = new ClienteDTO
                {
                    IdUsuario = idUsuarioSession.Value,
                    NombreCompleto = HttpContext.Session.GetString("NombreUsuario") ?? "Vendedor Test",
                    Email = HttpContext.Session.GetString("EmailUsuario") ?? "test@vendedor.com",
                    Cedula = HttpContext.Session.GetString("CedulaUsuario") ?? "12345678",
                    Telefono = HttpContext.Session.GetString("TelefonoUsuario") ?? "099123456"
                };

                string nombreModeloEscrito = Request.Form["Modelo.Nombre"];
                string nombreMarcaEscrito = Request.Form["Marca.Nombre"];

                vehiculo.Modelo = new ModeloDTO
                {
                    IdModelo = 1,
                    Modelo = !string.IsNullOrEmpty(nombreModeloEscrito) ? nombreModeloEscrito : "No especificado",
                    Marca = new MarcaDTO
                    {
                        IdMarca = 1,
                        NombreMarca = !string.IsNullOrEmpty(nombreMarcaEscrito) ? nombreMarcaEscrito : "No especificada"
                    }
                };


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
        public async Task<IActionResult> Editar(int id)
        {
            int? idUsuarioSession = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuarioSession == null)
            {
                return RedirectToAction("Index", "Login");
            }

            VehiculoDTO vehiculo = await _vehiculoService.DetalleVehiculo(id);

            if (vehiculo == null || vehiculo.Vendedor?.IdUsuario != idUsuarioSession.Value)
            {
                return RedirectToAction("MisVehiculos");
            }

            return View(vehiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, VehiculoDTO vehiculo)
        {
            int? idUsuarioSession = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuarioSession == null)
            {
                return RedirectToAction("Index", "Login");
            }

            VehiculoDTO vehiculoActual = await _vehiculoService.DetalleVehiculo(id);

            if (vehiculoActual == null || vehiculoActual.Vendedor?.IdUsuario != idUsuarioSession.Value)
            {
                return RedirectToAction("MisVehiculos");
            }

            string nombreModeloEscrito = Request.Form["Modelo.Nombre"];
            string nombreMarcaEscrito = Request.Form["Marca.Nombre"];

            vehiculoActual.Precio = vehiculo.Precio;
            vehiculoActual.Km = vehiculo.Km;
            vehiculoActual.Anio = vehiculo.Anio;
            vehiculoActual.CajaCambios = vehiculo.CajaCambios;
            vehiculoActual.Motorizacion = vehiculo.Motorizacion;
            vehiculoActual.Descripcion = vehiculo.Descripcion;
            vehiculoActual.Modelo.Modelo = !string.IsNullOrEmpty(nombreModeloEscrito) ? nombreModeloEscrito : vehiculoActual.Modelo.Modelo;
            vehiculoActual.Modelo.Marca.NombreMarca = !string.IsNullOrEmpty(nombreMarcaEscrito) ? nombreMarcaEscrito : vehiculoActual.Modelo.Marca.NombreMarca;


            if (string.IsNullOrEmpty(vehiculoActual.Vendedor.Cedula))
            {
                vehiculoActual.Vendedor.Cedula = "12345678";
            }
            vehiculoActual.Vendedor.Contrasena = null;

            try
            {
                await _vehiculoService.ModificarVehiculo(vehiculoActual);
                return RedirectToAction("Detalle", new { id });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(vehiculoActual);
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
                List<VehiculoDTO> vehiculos = await _vehiculoService.BuscarGeneral(lat, lon, radioKM, idMarca, precioMax);

                if (vehiculos == null)
                {
                    ViewBag.Error = "Ocurrió un error al realizar la busqueda. Intente nuevamnte";
                    return View();
                }

                if (!vehiculos!.Any())
                {
                    TempData["Error"] = "No se encontraron vehiculos en este radio";
                    return View(new List<VehiculoDTO>());
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