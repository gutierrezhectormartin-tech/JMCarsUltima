using JMCarsApp.DTOs;
using System.Net.Http.Json;

namespace JMCarsApp.Services
{
    public class EscribanoService
    {
        private readonly HttpClient _httpClient;

        public EscribanoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task Registrar(EscribanoDTO escribano, bool aceptaTerminos)
        {
            try
            {
                var request = new { Escribano = escribano, AceptaTerminos = aceptaTerminos };
                var respuesta = await _httpClient.PostAsJsonAsync("api/escribano/registrar", request);
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido registrar, intentelo nuevamente.");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido registrar: " + ex.Message);
            }
        }

        public async Task<EscribanoDTO?> ObtenerPorId(int id)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/escribano/{id}");
                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<EscribanoDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido obtener el escribano");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener el escribano: " + ex.Message);
            }
        }

        public async Task ActualizarPerfil(EscribanoDTO escribano)
        {
            try
            {
                var respuesta = await _httpClient.PutAsJsonAsync($"api/escribano/{escribano.IdUsuario}", escribano);
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido actualizar el perfil, intentelo nuevamente.");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido actualizar el perfil: " + ex.Message);
            }
        }
    }
}
