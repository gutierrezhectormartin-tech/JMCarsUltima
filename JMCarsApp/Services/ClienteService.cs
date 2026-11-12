using JMCarsApp.DTOs;
using System.Net.Http.Json;

namespace JMCarsApp.Services
{
    public class ClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task Registrar(ClienteDTO cliente, bool aceptaTerminos)
        {
            try
            {
                var request = new { Cliente = cliente, AceptaTerminos = aceptaTerminos };
                var respuesta = await _httpClient.PostAsJsonAsync("api/cliente/registrar", request);
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo completar el registro");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo completar el registro: " + ex.Message);
            }
        }
    }
}
