using JMCarsWeb.DTOs;
using System.Globalization;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class VehiculoService
    {
        private readonly HttpClient _httpClient;

        public VehiculoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task<List<VehiculoDTO>> ListarVehiculos()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<VehiculoDTO>>("api/vehiculo/listar") ?? new List<VehiculoDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido listar los vehiculos: " + ex.Message);
            }
        }

        public async Task<List<VehiculoDTO>> ListarMisVehiculos(string idUsuario)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<VehiculoDTO>>($"api/vehiculo/mis-vehiculos/{idUsuario}") ?? new List<VehiculoDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido listar sus vehiculos: " + ex.Message);
            }
        }

        public async Task<VehiculoDTO?> DetalleVehiculo(int idVehiculo)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<VehiculoDTO>($"api/vehiculo/detalle/{idVehiculo}");
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo obtener el detalle del vehiculo: " + ex.Message);
            }
        }
        public async Task RegistrarVehiculo(VehiculoDTO pVehiculo)
        {
            try
            {
                HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/vehiculo/registrar", pVehiculo);

                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo registrar su vehiculo");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo registrar su vehiculo: " + ex.Message);
            }
        }

        public async Task ModificarVehiculo(VehiculoDTO pVehiculo)
        {
            try
            {
                HttpResponseMessage respuesta = await _httpClient.PutAsJsonAsync("api/vehiculo/modificar", pVehiculo);

                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo modificar el vehiculo");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo modificar el vehiculo:" + ex.Message);
            }
        }

        public async Task<List<VehiculoDTO>> BuscarGeneral(decimal latCli, decimal lonCli, int radioKM, int? idMarca = null, decimal? precioMax = null)
        {
            try
            {
                var url = $"api/vehiculo/buscar?latCli={latCli.ToString(CultureInfo.InvariantCulture)}&lonCli={lonCli.ToString(CultureInfo.InvariantCulture)}&radioKM={radioKM}";

                if (idMarca.HasValue)
                {
                    url += $"&idMarca={idMarca.Value}";
                }

                if (precioMax.HasValue)
                {
                    url += $"&precioMax={precioMax.Value}";
                }

                var respuesta = await _httpClient.GetAsync(url);

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<List<VehiculoDTO>>() ?? new List<VehiculoDTO>();
                }

                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<VehiculoDTO>();
                }
                    
                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo realizar la busqueda");
                throw new Exception(error);

            }
            catch (Exception ex)
            {
                throw new Exception("no se pudo realizar la busqueda: " + ex.Message);
            }

        }
        public async Task CambiarEstadoVehiculo(int idVehiculo, int idEstado)
        {

            try
            {

                var respuesta = await _httpClient.PutAsJsonAsync($"api/vehiculo/estado/{idVehiculo}", new { IdEstado = idEstado });
                if (!respuesta.IsSuccessStatusCode)
                {
                    var error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo cambiar el estado a la publicacion");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo cambiar el estado a la publicacion: " + ex.Message);
            }
           
        }

    }
}
