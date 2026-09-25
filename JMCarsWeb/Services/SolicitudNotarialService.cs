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
            try
            {
                var request = new { IdCliente = idCliente, IdVehiculo = idVehiculo, IdEscribano = idEscribano };
                var respuesta = await _httpClient.PostAsJsonAsync("api/solicitudnotarial/crear", request);

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudo enviar la solicitud"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo enviar la solicitud: " + ex.Message);
            }
        }

        public async Task Aceptar(int idSolicitud, int idEscribano)
        {
            try
            {
                var request = new { IdEscribano = idEscribano };
                var respuesta = await _httpClient.PutAsJsonAsync($"api/solicitudnotarial/{idSolicitud}/aceptar", request);

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudo aceptar la solicitud."));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo aceptar la solicitud: " + ex.Message);
            }
        }

        public async Task Rechazar(int idSolicitud, int idEscribano)
        {
            try
            {
                var request = new { IdEscribano = idEscribano };
                var respuesta = await _httpClient.PutAsJsonAsync($"api/solicitudnotarial/{idSolicitud}/rechazar", request);

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudo rechazar la solicitud."));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo rechazar la solicitud: " + ex.Message);
            }
        }

        public async Task Finalizar(int idSolicitud, int idEscribano)
        {
            try
            {
                var request = new { IdEscribano = idEscribano };
                var respuesta = await _httpClient.PutAsJsonAsync($"api/solicitudnotarial/{idSolicitud}/finalizar", request);

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudo finalizar la venta."));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo finalizar la venta: " + ex.Message);
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
            catch (Exception ex)
            {
                throw new Exception("No se pudo obtener la solicitud: " + ex.Message);
            }
        }

        public async Task<List<SolicitudEscribanoDTO>> ListarPorCliente(int idCliente)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/solicitudnotarial/por-cliente/{idCliente}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<SolicitudEscribanoDTO>>() ?? new List<SolicitudEscribanoDTO>();
                }
                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<SolicitudEscribanoDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo listar las solicitudes del cliente");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo listar las solicitudes del cliente: " + ex.Message);
            }
        }

        public async Task<List<SolicitudEscribanoDTO>> ListarPorEscribano(int idEscribano)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/solicitudnotarial/por-escribano/{idEscribano}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<SolicitudEscribanoDTO>>() ?? new List<SolicitudEscribanoDTO>();
                }
                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<SolicitudEscribanoDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo listar las solicitudes del escribano");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo listar las solicitudes del escribano: " + ex.Message);
            }
        }

        public async Task<List<CompraVentaDTO>> ListarComprasVentaPorEscribano(int idEscribano)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/solicitudnotarial/compraventa/por-escribano/{idEscribano}");

                if(respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<CompraVentaDTO>>() ?? new List<CompraVentaDTO>();
                }
                if(respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<CompraVentaDTO>();
                }
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo listar las compraventa de este escribano");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo listar las compraventas del escribano: " + ex.Message);
            }
        }

        public async Task CambiarEstadoCompraVenta(int idCompraVenta, int idEstadoCompraVenta, int idEscribano)
        {
            try
            {
                var request = new { IdEstadoCompraVenta = idEstadoCompraVenta, IdEscribano = idEscribano };
                var respuesta = await _httpClient.PutAsJsonAsync($"api/solicitudnotarial/compraventa/{idCompraVenta}/estado", request);
                if(!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se ha podido actualizar el estado del compraventa");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido actualizar el estado del compraventa: " + ex.Message);
            }
        }
        //private async Task<string> LeerMensajeError(HttpResponseMessage pRespuesta, string pMensajePorDefecto)
        //{
        //    try
        //    {
        //        var contenido = await pRespuesta.Content.ReadFromJsonAsync<MensajeResponse>();

        //        if (contenido != null && !string.IsNullOrWhiteSpace(contenido.Mensaje))
        //        {
        //            return contenido.Mensaje;
        //        }

        //        return pMensajePorDefecto;
        //    }
        //    catch (Exception e)
        //    {
        //        return pMensajePorDefecto;
        //    }
        //}
    }

    //public class MensajeResponse
    //{
    //    public string Mensaje { get; set; }
    //}
}
