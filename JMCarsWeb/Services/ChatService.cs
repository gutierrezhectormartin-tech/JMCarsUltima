using JMCarsWeb.DTOs;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task<List<ChatDTO>?> ListarChatsPorUsuario (int idUsuario)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/chat/listar/{idUsuario}");

                if(respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<ChatDTO>>();
                }
                if(respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<ChatDTO>();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<MensajeDTO>?> ObtenerMensajes(int idChat, int idUsuario)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/chat/{idChat}/mensajes/{idUsuario}");

                if(respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<MensajeDTO>>();
                }
                if(respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<MensajeDTO>();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int?> ObtenerOCrearChat(int idVehiculo, int idComprador, int idVendedor)
        {
            try
            {
                var request = new { IdVehiculo = idVehiculo, IdComprador = idComprador, IdVendedor = idVendedor };
                var respuesta = await _httpClient.PostAsJsonAsync("api/chat/obtener-o-crear", request);
                if(respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadFromJsonAsync<IdChatResponse>();
                    return resultado?.IdChat;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> EnviarMensaje(int idChat, int idEmisor, string contenido)
        {
            try
            {
                var request = new { IdEmisor = idEmisor, Contenido = contenido };
                var respuesta = await _httpClient.PostAsJsonAsync($"api/chat/{idChat}/mensajes", request);
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ChatDTO>?> ListarChatsPorVehiculo(int idVehiculo)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/chat/vehiculo/{idVehiculo}");

                if(respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<ChatDTO>>();
                }
                if(respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<ChatDTO>();
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class IdChatResponse
    {
        public int IdChat { get; set; }
    }
}
