using Modelo;
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

        public async Task<List<Vehiculo>> ListarVehiculos()
        {
            return await _httpClient.GetFromJsonAsync<List<Vehiculo>>("api/vehiculo/listar") ?? new List<Vehiculo>();
        }

        public async Task<List<Vehiculo>> ListarMisVehiculos(string idUsuario)
        {
            return await _httpClient.GetFromJsonAsync<List<Vehiculo>>($"api/vehiculo/mis-vehiculos/{idUsuario}") ?? new List<Vehiculo>();
        }
        public async Task<bool> RegistrarVehiculo(Vehiculo pVehiculo)
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

        public async Task<List<Vehiculo>> BuscarGeneral(decimal latCli, decimal lonCli, int radioKM, int? idMarca = null, decimal? precioMax = null)
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
                    return await respuesta.Content.ReadFromJsonAsync<List<Vehiculo>>() ?? new List<Vehiculo>();
                }

                if (respuesta.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<Vehiculo>();
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
