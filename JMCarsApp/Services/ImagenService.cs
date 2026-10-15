using System.Collections.Concurrent;

namespace JMCarsApp.Services
{
    public class ImagenService
    {
        private readonly HttpClient _httpClient;
        private readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();

        public ImagenService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsWeb");
        }

        public async Task<string?> ObtenerDataUri(string pRuta)
        {
            string ruta = pRuta.Replace("\\", "/");

            if(_cache.TryGetValue(ruta, out string? guardada))
            {
                return guardada;
            }
            try
            {
                HttpResponseMessage respuesta = await _httpClient.GetAsync(ruta);
                if(!respuesta.IsSuccessStatusCode)
                {
                    return null;
                }
                byte[] bytes = await respuesta.Content.ReadAsByteArrayAsync();
                string tipo = respuesta.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
                string dataUri = $"data:{tipo};base64, {Convert.ToBase64String(bytes)}";
                _cache[ruta] = dataUri;
                return dataUri;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}
