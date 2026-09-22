using JMCarsWeb.DTOs;
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

        public async Task<UsuarioDTO?> Login (string email, string contrasena)
        {
            try
            {
                var request = new { Email = email, Contrasena = contrasena };
                var respuesta = await _httpClient.PostAsJsonAsync("api/usuario/login", request);

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<UsuarioDTO>();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private class ExisteMailResponse
        {
            public bool Existe { get; set; }
        }

        public async Task<bool> ExisteMail(string email)
        {
            try
            {
                var respuesta = await _httpClient.GetFromJsonAsync<ExisteMailResponse>($"api/usuario/existe-mail/{email}");
                return respuesta?.Existe ?? false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RecuperarContrasena(string email)
        {
            try
            {
                var respuesta = await _httpClient.PostAsJsonAsync("api/usuario/recuperar-contrasena", email);
                var contenido = await respuesta.Content.ReadAsStringAsync();
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ResetearContrasena(string token, string NuevaContrasena)
        {
            try
            {
                var request = new { Token = token, NuevaContrasena = NuevaContrasena };
                var respuesta = await _httpClient.PostAsJsonAsync("api/usuario/resetear-contrasena", request);
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}