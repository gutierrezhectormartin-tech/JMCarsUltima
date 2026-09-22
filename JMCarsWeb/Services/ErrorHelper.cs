using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    internal static class ErrorHelper
    {
        internal static async Task<string> LeerMensajeError(HttpResponseMessage pRespuesta, string pMensajePorDefecto)
        {
			try
			{
				var contenido = await pRespuesta.Content.ReadFromJsonAsync <Dictionary<string, string>>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				return contenido?.GetValueOrDefault("error") ?? contenido.GetValueOrDefault("mensaje") ?? pMensajePorDefecto;
			}
			catch (Exception)
			{
				return pMensajePorDefecto;
			}
        }
    }
}
