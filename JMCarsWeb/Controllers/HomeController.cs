using JMCarsWeb.DTOs;
using JMCarsWeb.Services;
using JMCarsWeb.ViewModels;
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

        public async Task<IActionResult> Index(int? marca, int? modelo, int? anio, string? ordenPrecio)
        {
            const int tamanoPagina = 12;
            HomeViewModel modelo_vista = new HomeViewModel
            {
                Marca = marca,
                Modelo = modelo,
                Anio = anio,
                OrdenPrecio = ordenPrecio
            };

            ViewBag.Destacados = new List<VehiculoDTO>();

            try
            {
                modelo_vista.Vehiculos = await _vehiculoService.ListarPublicadosPaginado(marca, modelo, anio, ordenPrecio, 1, tamanoPagina);
                modelo_vista.Filtros = await _vehiculoService.ListarFiltrosPublicados();

                PaginaDTO<VehiculoDTO> ultimos = await _vehiculoService.ListarPublicadosPaginado(null, null, null, null, 1, tamanoPagina);
                List<VehiculoDTO> conFoto = ultimos.Items.Where(v => v.Fotografia != null && v.Fotografia.Any()).ToList();
                ViewBag.Destacados = conFoto.Take(6).ToList();

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            try
            {
                ViewBag.Conteos = await _estadisticasService.ObtenerConteos();
            }
            catch (Exception)
            {
                ViewBag.Conteos = null;
            }

            return View(modelo_vista);
        }

        [HttpGet]
        public async Task<IActionResult> CargarMas(int? marca, int? modelo, int? anio, string? ordenPrecio, int pagina = 2)
        {
            const int tamanoPagina = 12;
            try
            {
                PaginaDTO<VehiculoDTO> resultado = await _vehiculoService.ListarPublicadosPaginado(marca, modelo, anio, ordenPrecio, pagina, tamanoPagina);
                Response.Headers["X-Hay-Mas"] = resultado.HayMas ? "true" : "false";
                return PartialView("_TarjetasVehiculos", resultado.Items);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
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
