namespace WebAPI.Services
{
    public interface IEmailService
    {
        Task EnviarCorreoRecuperacion(string pDestinatario, string pNombreUsuario, string pLinkRecuperacion);
    }
}
