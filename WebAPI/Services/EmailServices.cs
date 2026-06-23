using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;

namespace WebAPI.Services
{
    public class EmailServices : IEmailService
    {
        private readonly IConfiguration _configuracion;

        public EmailServices(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task EnviarCorreoRecuperacion(string pDestinatario, string pNombreUsuario, string pLinkRecuperacion)
        {
            var mensaje = new MimeMessage();

            mensaje.From.Add(new MailboxAddress("JMCars", _configuracion["EmailSettings:Usuario"]));
            mensaje.To.Add(new MailboxAddress(pNombreUsuario, pDestinatario));
            mensaje.Subject = "Recuperacion de Contraseña - JMCars";

            mensaje.Body = new TextPart("html")
            {
                Text = $@"
                    <h3>Hola {pNombreUsuario}, </h3>
                    <p>Recibimos una solicitud para reestablecer tu contraeña. </p>
                    <p>Hacé clic  en el siguiente enlace para continuar (validez 30 minutos)</p>
                    <p><a href='{pLinkRecuperacion}'>Reestablecer mi Contraseña</a></p>
                    <p>Si no solicitaste este cambio, ignorá este mail.</p>
                "
            };

            using var cliente = new SmtpClient();

            await cliente.ConnectAsync(_configuracion["EmailSettings:Host"]!, int.Parse(_configuracion["EmailSettings:Port"]!), SecureSocketOptions.StartTls);

            await cliente.AuthenticateAsync(_configuracion["EmailSettings:Usuario"]!, _configuracion["EmailSettings:ContrasenaApp"]!);

            await cliente.SendAsync(mensaje);
            await cliente.DisconnectAsync(true);
        }


    }
}
