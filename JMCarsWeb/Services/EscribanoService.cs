using Modelo;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class EscribanoService
    {
        private readonly HttpClient _httpClient;

        public EscribanoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task Registrar(Escribano escribano)
        {
            await _httpClient.PostAsJsonAsync("api/escribano/registrar", escribano);
        }

        public async Task<Escribano> ObtenerPorId(int id)
        {
            return await _httpClient.GetFromJsonAsync<Escribano>($"api/escribano/{id}");
        }

        public async Task ActualizarPerfil(Escribano escribano)
        {
            await _httpClient.PutAsJsonAsync($"api/escribano/{escribano.IdUsuario}", escribano);
        }

        public async Task Inactivar(int id)
        {
            await _httpClient.DeleteAsync($"api/escribano/{id}/inactivar");
        }
    }
}
