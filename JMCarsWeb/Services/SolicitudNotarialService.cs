using JMCarsWeb.DTOs;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class SolicitudNotarialService
    {
        private readonly HttpClient _httpClient;

        public SolicitudNotarialService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task Crear(int idCliente, int idVehiculo, int idEscribano)
        {
            var request = new { IdCliente = idCliente, IdVehiculo = idVehiculo, IdEscribano = idEscribano };
            var respuesta = await _httpClient.PostAsJsonAsync("api/solicitudnotarial/crear", request);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception(await LeerMensajeError(respuesta, "No se pudo enviar la solicitud."));
            }
        }

        public async Task Aceptar(int idSolicitud, int idEscribano)
        {
            var respuesta = await _httpClient.PutAsync($"api/solicitudnotarial/{idSolicitud}/aceptar/{idEscribano}", null);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception(await LeerMensajeError(respuesta, "No se pudo aceptar la solicitud."));
            }
        }

        public async Task Rechazar(int idSolicitud, int idEscribano)
        {
            var respuesta = await _httpClient.PutAsync($"api/solicitudnotarial/{idSolicitud}/rechazar/{idEscribano}", null);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception(await LeerMensajeError(respuesta, "No se pudo rechazar la solicitud."));
            }
        }

        public async Task Finalizar(int idSolicitud, int idEscribano)
        {
            var respuesta = await _httpClient.PutAsync($"api/solicitudnotarial/{idSolicitud}/finalizar/{idEscribano}", null);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception(await LeerMensajeError(respuesta, "No se pudo finalizar la venta."));
            }
        }

        public async Task<SolicitudEscribanoDTO?> ObtenerPorId(int idSolicitud)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/solicitudnotarial/{idSolicitud}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<SolicitudEscribanoDTO>();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SolicitudEscribanoDTO>?> ListarPorCliente(int idCliente)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/solicitudnotarial/por-cliente/{idCliente}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<SolicitudEscribanoDTO>>();
                }
                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<SolicitudEscribanoDTO>();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SolicitudEscribanoDTO>?> ListarPorEscribano(int idEscribano)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/solicitudnotarial/por-escribano/{idEscribano}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<SolicitudEscribanoDTO>>();
                }
                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<SolicitudEscribanoDTO>();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<string> LeerMensajeError(HttpResponseMessage pRespuesta, string pMensajePorDefecto)
        {
            try
            {
                var contenido = await pRespuesta.Content.ReadFromJsonAsync<MensajeResponse>();

                if (contenido != null && !string.IsNullOrWhiteSpace(contenido.Mensaje))
                {
                    return contenido.Mensaje;
                }

                return pMensajePorDefecto;
            }
            catch (Exception)
            {
                return pMensajePorDefecto;
            }
        }
    }

    public class MensajeResponse
    {
        public string Mensaje { get; set; }
    }
}
