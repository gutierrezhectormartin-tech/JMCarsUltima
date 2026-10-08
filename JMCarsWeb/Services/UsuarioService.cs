using JMCarsWeb.DTOs;
using System.Net.Http.Json;

namespace JMCarsWeb.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;

        public UsuarioService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("JMCarsAPI");
        }

        public async Task<UsuarioDTO?> Login (string email, string contrasena)
        {
            try
            {
                var request = new { Email = email, Contrasena = contrasena };
                var respuesta = await _httpClient.PostAsJsonAsync("api/usuario/login", request);

                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<UsuarioDTO>();
                }

                if (respuesta.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }

                string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo iniciar sesión, intentelo nuevamente.");
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private class ExisteMailResponse
        {
            public bool Existe { get; set; }
        }

        public async Task<bool> ExisteMail(string email)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"api/usuario/existe-mail/{email}");

                if(!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(await ErrorHelper.LeerMensajeError(respuesta, "No se pudo verificar el correo"));
                }
                var resultado = await respuesta.Content.ReadFromJsonAsync<ExisteMailResponse>();
                return resultado?.Existe ?? false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RecuperarContrasena(string email)
        {
            try
            {
                var respuesta = await _httpClient.PostAsJsonAsync("api/usuario/recuperar-contrasena", email);
                if(!respuesta.IsSuccessStatusCode)
                {
                    throw new Exception(); //A drede no ponemos mas info de porque.
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ResetearContrasena(string token, string NuevaContrasena)
        {
            try
            {
                var request = new { Token = token, NuevaContrasena = NuevaContrasena };
                var respuesta = await _httpClient.PostAsJsonAsync("api/usuario/resetear-contrasena", request);
                if (!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "El enlace no es válido o ya expiró");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ResetearContrasena(string email, string contrasenaActual, string contrasenaNueva)
        {
            try
            {
                var request = new { Email = email, ContrasenaActual = contrasenaActual, ContrasenaNueva = contrasenaNueva};
                var respuesta = await _httpClient.PostAsJsonAsync("api/usuario/cambiar-contrasena", request);

                if(!respuesta.IsSuccessStatusCode)
                {
                    string error = await ErrorHelper.LeerMensajeError(respuesta, "No se pudo cambiar la contraseña");
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> EstadoVerificadorFuncionandoCorrectamente()
        {
            try
            {
                var respuesta = await _httpClient.GetFromJsonAsync<EstadoVerificadorResponse>("api/usuario/estado-verificador");
                return respuesta?.FuncionandoCorrectamente ?? true;
            }
            catch (Exception)
            {
                return true; // si no se puede consultar, es que paso algo que esta mas alla del admin, y como es un aviso secundario no vale la pena notificacion
            }
        }
        public class EstadoVerificadorResponse
        {
            public bool FuncionandoCorrectamente { get; set; }
        }
    }
}