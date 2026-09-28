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

        public async Task Registrar(EscribanoDTO escribano, bool aceptaTerminos)
        {
            try
            {
                var request = new { Escribano = escribano, AceptaTerminos = aceptaTerminos };
                var respuesta = await _httpClient.PostAsJsonAsync("api/escribano/registrar", request);
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido registar, intentelo nuevamente.");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido registar: " + ex.Message);
            }
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
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido obtener la lista de escribanos activos, intentelo nuevamente");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener la lista de escribanos activos, intentelo nuevamente: " + ex.Message);
            }
        }

        public async Task<EscribanoDTO?> ObtenerPorId(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<EscribanoDTO>($"api/escribano/{id}");
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

        public async Task Inactivar(int id)
        {
            try
            {
                var respuesta = await _httpClient.DeleteAsync($"api/escribano/{id}/inactivar");
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido inactivar el escribano, intentelo nuevamente.");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido inactivar el escribano: " + ex.Message);
            }
        }

        public async Task<List<EscribanoDTO>> ListarInactivos()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync("api/escribano/inactivos");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<EscribanoDTO>>() ?? new List<EscribanoDTO>();
                }
                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<EscribanoDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido obtener la lista de escribanos pendientes, intentelo nuevamente");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener la lista de escribanos pendientes: " + ex.Message);
            }
        }

        public async Task Activar(int id)
        {
            try
            {
                var respuesta = await _httpClient.PutAsync($"api/escribano/{id}/activar", null);
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido activar el escribano, intentelo nuevamente.");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido activar el escribano: " + ex.Message);
            }
        }
    }
}
