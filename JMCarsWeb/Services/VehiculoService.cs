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

        public async Task<List<Vehiculo>> ListarMisVehiculos(string idUsuario)
        {
            return await _httpClient.GetFromJsonAsync<List<Vehiculo>>($"api/vehiculo/mis-vehiculos/{idUsuario}") ?? new List<Vehiculo>();
        }
        public async Task<bool> RegistrarVehiculo(Vehiculo pVehiculo)
        {
            HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/vehiculo/registrar", pVehiculo);

            if (respuesta.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                string error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

    }
}
