using JMCarsWeb.DTOs;
using JMCarsWeb.Services;
using JMCarsWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using X.PagedList;
using X.PagedList.Extensions;

namespace JMCarsWeb.Controllers
{
    public class AdminController : Controller
    {
        private VehiculoService _vehiculoService;
        private EscribanoService _escribanoService;
        private ClienteService _clienteService;
        private EstadisticasService _estadisticasService;
        public AdminController(VehiculoService vehiculoService, EscribanoService escribanoService, ClienteService clienteService, EstadisticasService estadisticasService)
        {
            _vehiculoService = vehiculoService;
            _escribanoService = escribanoService;
            _clienteService = clienteService;
            _estadisticasService = estadisticasService;
        }

        [HttpGet]
        public async Task<IActionResult> Vehiculos(int? estado, string? marca, string? buscar, int pagina = 1)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningún usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }
            const int tamanoPagina = 20;
            try
            {

                List<VehiculoDTO> todos = await _vehiculoService.ListarVehiculos();

                List<string> marcas = todos.Select(v => v.Modelo?.Marca?.NombreMarca ?? "")
                                            .Where(m => m != "")
                                            .Distinct()
                                            .OrderBy(m => m)
                                            .ToList();

                IEnumerable<VehiculoDTO> filtrados = todos;

                if(estado.HasValue)
                {
                    filtrados = filtrados.Where(v => v.IdEstadoPublicacion == estado.Value);
                }
                if(!string.IsNullOrWhiteSpace(marca))
                {
                    filtrados = filtrados.Where(v => v.Modelo?.Marca?.NombreMarca == marca);
                }
                if(!string.IsNullOrWhiteSpace(buscar))
                {
                    string texto = buscar.Trim();
                    filtrados = filtrados.Where(v => (v.Modelo?.Modelo ?? "").Contains(texto, StringComparison.OrdinalIgnoreCase) || (v.Vendedor?.NombreCompleto ?? "").Contains(texto, StringComparison.OrdinalIgnoreCase));
                }

                List<VehiculoDTO> lista = filtrados.OrderByDescending(v => v.IdVehiculo).ToList();

                int totalPaginas = (int)Math.Ceiling(lista.Count / (double)tamanoPagina);
                if(pagina > totalPaginas)
                {
                    pagina = totalPaginas;
                }
                if(pagina < 1)
                {
                    pagina = 1;
                }

                VehiculosAdminViewModel modelo = new VehiculosAdminViewModel
                {
                    Vehiculos = lista.ToPagedList(pagina, tamanoPagina),
                    Marcas = marcas,
                    Estado = estado,
                    Marca = marca,
                    Buscar = buscar,
                    TotalFiltrados = lista.Count
                };
                return View(modelo);

            }

            catch (Exception ex )
            {
                TempData["Error"] =  ex.Message;
                return View(new VehiculosAdminViewModel{ Vehiculos = new List<VehiculoDTO>().ToPagedList(1, tamanoPagina)});
            }
        }

        [HttpGet]
        public async  Task<IActionResult> HistorialPublicaciones(int idCliente)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "No hay un usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }
            try
            {
                List<VehiculoDTO> vehiculos = await _vehiculoService.ListarMisVehiculos(idCliente.ToString());
                return View(vehiculos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<VehiculoDTO>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, int idEstado)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningún usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _vehiculoService.CambiarEstadoVehiculo(id, idEstado, idRol.Value);
                TempData["Mensaje"] = "El estado del vehiculo se ha cambiado exitosamente";
               
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Vehiculos");
        }

        [HttpGet]
        public async Task<IActionResult> Estadisticas()
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if (idRol != 1)
            {
                TempData["Error"] = "Ningún usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }

            EstadisticasViewModel modelo = new EstadisticasViewModel();

            try
            {
                modelo.Conteos = await _estadisticasService.ObtenerConteos();
                modelo.MarcasPublicadas = await _estadisticasService.ObtenerMarcasPublicadas();
                modelo.ModelosVendidos = await _estadisticasService.ObtenerModelosVendidosPorMarca();

                List<CompraVentaPorMesDTO> comprasPorMes = await _estadisticasService.ObtenerComprasPorMes();
                DateTime primerDiaMesActual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                for (int i = 11; i >= 0; i--)
                {
                    DateTime mes = primerDiaMesActual.AddMonths(-i);
                    CompraVentaPorMesDTO existente = comprasPorMes.FirstOrDefault(c => c.Anio == mes.Year && c.Mes == mes.Month);

                    modelo.ComprasPorMes.Add(new CompraVentaPorMesDTO{Anio = mes.Year, Mes = mes.Month, Cantidad = existente != null ? existente.Cantidad : 0});                
                }
                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(modelo);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningún usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }

            UsuariosViewModel modelo = new UsuariosViewModel();

            try
            {
                modelo.Clientes = await _clienteService.ListarTodos();
                modelo.Escribanos = await _escribanoService.ListarTodos();
                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActivarCliente(int id)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningún usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _clienteService.Activar(id);
                TempData["Mensaje"] = "La cuenta del cliente fue activada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        public async Task<IActionResult> InactivarCliente(int id)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningún usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _clienteService.Inactivar(id);
                TempData["Mensaje"] = "La cuenta del cliente fue inactivada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        public async Task<IActionResult> ActivarEscribano(int id)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningún usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _escribanoService.Activar(id);
                TempData["Mensaje"] = "La cuenta del escribano fue activada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        public async Task<IActionResult> InactivarEscribano(int id)
        {
            int? idRol = HttpContext.Session.GetInt32("IdRol");

            if(idRol != 1)
            {
                TempData["Error"] = "Ningún usuario con permisos de administrador logueado";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                await _escribanoService.Inactivar(id);
                TempData["Mensaje"] = "La cuenta del escribano fue inactivada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Usuarios");
        }

    }
}
