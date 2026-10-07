using JMCarsWeb.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class EstadisticasService
    {
        private readonly HttpClient _httpClient;
        public EstadisticasService (IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task<EstadisticasDTO> ObtenerConteos()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync("api/estadisticas/conteos");

                if(!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudieron obtener los indicadores"));
                }
                return await respuesta.Content.ReadFromJsonAsync<EstadisticasDTO>() ?? new EstadisticasDTO();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudieron obtener los indicadores: " + ex.Message);
            }
        }

        public async Task<List<CompraVentaPorMesDTO>> ObtenerComprasPorMes()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync("api/estadisticas/compras-por-mes");
                if(!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudieron obtener las compras por mes"));
                }
                return await respuesta.Content.ReadFromJsonAsync<List<CompraVentaPorMesDTO>>() ?? new List<CompraVentaPorMesDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudieron obtener las compras por mes: " + ex.Message);
            }
        }

        public async Task<List<MarcaPublicadaDTO>> ObtenerMarcasPublicadas()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync("api/estadisticas/marcas-publicadas");
                if(!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudieron obtener las marcas publicadas"));
                }
                return await respuesta.Content.ReadFromJsonAsync<List<MarcaPublicadaDTO>>() ?? new List<MarcaPublicadaDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudieron obtener las marcas publicadas: " + ex.Message);
            }
        }

        public async Task<List<ModeloVendidoDTO>> ObtenerModelosVendidosPorMarca()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync("api/estadisticas/modelos-vendidos-por-marca");
                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudieron obtener los modelos vendidos por marca"));
                }
                return await respuesta.Content.ReadFromJsonAsync<List<ModeloVendidoDTO>>() ?? new List<ModeloVendidoDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudieron obtener los modelos vendidos por marca: " + ex.Message);
            }
        }
    }
}
