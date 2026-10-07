using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Logica;
using Modelo;
using Logica.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasController : ControllerBase
    {
        private readonly ILogicaEstadisticas _logicaEstadisticas;

        public EstadisticasController()
        {
            _logicaEstadisticas = FabricaLogica.GetInstancia().GetLogicaEstadisticas();
        }

        [HttpGet("conteos")]
        public IActionResult ObtenerConteos()
        {
            try
            {
                Estadisticas estadisticas = _logicaEstadisticas.ObtenerConteos();
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("compras-por-mes")]
        public IActionResult ObtenerComprasPorMes()
        {
            try
            {
                List<CompraVentaPorMes> lista = _logicaEstadisticas.ObtenerComprasPorMes();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("marcas-publicadas")]
        public IActionResult ObtenerMarcasPublicadas()
        {
            try
            {
                List<MarcaPublicada> lista = _logicaEstadisticas.ObtenerMarcasMasPublicadas();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("modelos-vendidos-por-marca")]
        public IActionResult ObtenerModelosVendidosPorMarca()
        {
            try
            {
                List<ModeloVendido> lista = _logicaEstadisticas.ObtenerModelosVendidosPorMarca();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
