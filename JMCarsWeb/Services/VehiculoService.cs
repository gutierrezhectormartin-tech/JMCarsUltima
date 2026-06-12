using Modelo;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class VehiculoService
    {
        private readonly HttpClient _httpClient;

        public VehiculoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task<List<Vehiculo>> ListarVehiculos()
        {
            return await _httpClient.GetFromJsonAsync<List<Vehiculo>>("api/vehiculo/listar") ?? new List<Vehiculo>();
        }
    }
}
