using JMCarsWeb.DTOs;
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

        public async Task<bool> Registrar(ClienteDTO cliente, bool aceptaTerminos)
        {
            var request = new { Cliente = cliente, AceptaTerminos = aceptaTerminos };
            var respuesta = await _httpClient.PostAsJsonAsync("api/cliente/registrar", request);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<ClienteDTO?> ObtenerPorId(int id)
        {
            return await _httpClient.GetFromJsonAsync<ClienteDTO>($"api/cliente/{id}");
        }

        public async Task ActualizarPerfil(ClienteDTO cliente)
        {
            await _httpClient.PutAsJsonAsync($"api/cliente/{cliente.IdUsuario}", cliente);
        }

        public async Task Inactivar(int id)
        {
            await _httpClient.PutAsJsonAsync($"api/cliente/{id}/inactivar", new { });
        }

    }
}
