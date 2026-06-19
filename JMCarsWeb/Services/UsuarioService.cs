using Modelo;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;

        public UsuarioService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task<Usuario> Login (string email, string contrasena)
        {
            var request = new { Email = email, Contrasena = contrasena };
            var respuesta = await _httpClient.PostAsJsonAsync("api/usuario/login", request);



            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadFromJsonAsync<Usuario>();
            }

            return null;
        }

        public async Task<bool> ExisteMail(string email)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"api/usuario/existe-mail/{email}");
        }
    }
}