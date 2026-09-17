namespace WebAPI.Services
{
    public interface IEmailService
    {
        Task EnviarCorreoRecuperacion(string pDestinatario, string pNombreUsuario, string pLinkRecuperacion);
        Task EnviarCorreo(string pDestinatario, string pNombreUsuario, string pAsunto, string pCuerpo);
    }
}
