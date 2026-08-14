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
            return await _httpClient.GetFromJsonAsync<List<VehiculoDTO>>("api/vehiculo/listar") ?? new List<VehiculoDTO>();
        }

        public async Task<List<VehiculoDTO>> ListarMisVehiculos(string idUsuario)
        {
            return await _httpClient.GetFromJsonAsync<List<VehiculoDTO>>($"api/vehiculo/mis-vehiculos/{idUsuario}") ?? new List<VehiculoDTO>();
        }

        public async Task<VehiculoDTO?> DetalleVehiculo(int idVehiculo)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<VehiculoDTO>($"api/vehiculo/detalle/{idVehiculo}");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> RegistrarVehiculo(VehiculoDTO pVehiculo)
        {
            HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/vehiculo/registrar", pVehiculo);

            if (respuesta.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                string error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task<bool> ModificarVehiculo(VehiculoDTO pVehiculo)
        {
            HttpResponseMessage respuesta = await _httpClient.PutAsJsonAsync("api/vehiculo/modificar", pVehiculo);

            if (respuesta.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                string error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task<bool> InactivarVehiculo(int idVehiculo)
        {
            HttpResponseMessage respuesta = await _httpClient.PutAsync($"api/vehiculo/inactivar/{idVehiculo}", null);

            if (respuesta.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                string error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task<bool> ActivarVehiculo(int idVehiculo)
        {
            HttpResponseMessage respuesta = await _httpClient.PutAsync($"api/vehiculo/activar/{idVehiculo}", null);

            if (respuesta.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                string error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(error);
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

                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

    }
}
