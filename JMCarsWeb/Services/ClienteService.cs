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

        public async Task<ClienteDTO?> ObtenerPorId(int id)
        {
            try
            {

                var respuesta =  await _httpClient.GetAsync($"api/cliente/{id}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<ClienteDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido obtener el cliente");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener el cliente: " + ex.Message);
            }
        }

        public async Task ActualizarPerfil(ClienteDTO cliente)
        {
            try
            {
                var respuesta = await _httpClient.PutAsJsonAsync($"api/cliente/{cliente.IdUsuario}", cliente);
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido actualizar el perfil");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo actualizar  el perfil: " + ex.Message);
            }
        }

        public async Task Inactivar(int id)
        {
            try
            {
                var respuesta = await _httpClient.DeleteAsync($"api/cliente/{id}/inactivar");
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido inactivar el cliente, intentelo nuevamente.");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido inactivar el cliente: " + ex.Message);
            }
        }

        public async Task<List<ClienteDTO>> ListarTodos()
        {
            try
            {
                var respuesta = await _httpClient.GetAsync("api/cliente/todos");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<ClienteDTO>>() ?? new List<ClienteDTO>();
                }
                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<ClienteDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido obtener la lista de clientes, intentelo nuevamente");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener la lista de clientes: " + ex.Message);
            }
        }

        public async Task Activar(int id)
        {
            try
            {
                var respuesta = await _httpClient.PutAsync($"api/cliente/{id}/activar", null);
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido activar el cliente, intentelo nuevamente.");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido activar el cliente: " + ex.Message);
            }
        }

    }
}
