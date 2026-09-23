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

        public async Task<List<ChatDTO>> ListarChatsPorUsuario (int idUsuario)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/chat/listar/{idUsuario}");

                if(respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<ChatDTO>>() ?? new List<ChatDTO>();
                }
                if(respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<ChatDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudieron listar los chats");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los chats del usuario: " + ex.Message);
            }
        }

        public async Task<List<MensajeDTO>> ObtenerMensajes(int idChat, int idUsuario)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/chat/{idChat}/mensajes/{idUsuario}");

                if(respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<MensajeDTO>>() ?? new List<MensajeDTO>();
                }
                if(respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<MensajeDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se han podido obtener los mensajes");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los mensajes: " + ex.Message);
            }
        }

        public async Task<int> ObtenerOCrearChat(int idVehiculo, int idComprador, int idVendedor)
        {
            try
            {
                var request = new { IdVehiculo = idVehiculo, IdComprador = idComprador, IdVendedor = idVendedor };
                var respuesta = await _httpClient.PostAsJsonAsync("api/chat/obtener-o-crear", request);
                if(respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadFromJsonAsync<IdChatResponse>();
                    return resultado!.IdChat;
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo obtener o crear chat");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("Error no se pudo crear u obtener chat: " + ex.Message);
            }
        }

        public async Task EnviarMensaje(int idChat, int idEmisor, string contenido)
        {
            try
            {
                var request = new { IdEmisor = idEmisor, Contenido = contenido };
                var respuesta = await _httpClient.PostAsJsonAsync($"api/chat/{idChat}/mensajes", request);
                if(!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo enviar el mensaje");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error no se ha podido enviar el mensaje: " + ex.Message);
            }
        }

        public async Task<List<ChatDTO>> ListarChatsPorVehiculo(int idVehiculo)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/chat/vehiculo/{idVehiculo}");

                if(respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<ChatDTO>>() ?? new List<ChatDTO>(); ;
                }
                if(respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<ChatDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudieron listar los chats");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("Error no se ha logrado listar los chats: " + ex.Message);
            }
        }
    }

    public class IdChatResponse
    {
        public int IdChat { get; set; }
    }
}
