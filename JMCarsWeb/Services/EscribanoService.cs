using JMCarsWeb.DTOs;
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

        public async Task<bool> Registrar(EscribanoDTO escribano, bool aceptaTerminos)
        {
            var request = new { Escribano = escribano, AceptaTerminos = aceptaTerminos };
            var respuesta = await _httpClient.PostAsJsonAsync("api/escribano/registrar", request);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<List<EscribanoDTO>?> ListarActivos()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync("api/escribano/activos");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<EscribanoDTO>>();
                }
                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<EscribanoDTO>();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EscribanoDTO?> ObtenerPorId(int id)
        {
            return await _httpClient.GetFromJsonAsync<EscribanoDTO>($"api/escribano/{id}");
        }

        public async Task ActualizarPerfil(EscribanoDTO escribano)
        {
            await _httpClient.PutAsJsonAsync($"api/escribano/{escribano.IdUsuario}", escribano);
        }

        public async Task Inactivar(int id)
        {
            await _httpClient.DeleteAsync($"api/escribano/{id}/inactivar");
        }
    }
}
