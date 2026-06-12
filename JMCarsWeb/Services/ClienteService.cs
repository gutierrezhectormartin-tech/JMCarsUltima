using Modelo;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class ClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task Registrar(Cliente cliente)
        {
            await _httpClient.PostAsJsonAsync("api/cliente/registro", cliente);
        }

        public async Task<Cliente?> ObtenerPorId(int id)
        {
            return await _httpClient.GetFromJsonAsync<Cliente>($"api/cliente/{id}");
        }

        public async Task ActualizarPerfil(Cliente cliente)
        {
            await _httpClient.PutAsJsonAsync($"api/cliente/{cliente.IdUsuario}", cliente);
        }

        public async Task Inactivar(int id)
        {
            await _httpClient.PutAsJsonAsync($"api/cliente/{id}/inactivar", new { });
        }

    }
}
