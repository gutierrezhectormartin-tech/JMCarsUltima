using Logica;
using Modelo;

namespace WebAPI.Services
{
    public class VerificadorSolicitudesVencidasService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<VerificadorSolicitudesVencidasService> _logger;

        public static bool UltimaEjecucionExitosa { get; private set; } = true;

        public VerificadorSolicitudesVencidasService(IServiceScopeFactory scopeFactory, ILogger<VerificadorSolicitudesVencidasService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var logicaSolicitud = FabricaLogica.GetInstancia().GetLogicaSolicitudNotarial();
                    var logicaUsuario = FabricaLogica.GetInstancia().GetLogicaUsuario();

                    List<SolicitudVencida> vencidas = logicaSolicitud.ListarPendientesVencidas();

                    if(vencidas.Any())
                    {
                        List<Usuario> administradores = logicaUsuario.ListarAdministradoresActivos();

                        using (var  scope = _scopeFactory.CreateScope())
                        {
                            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                            string asunto = "JMCars - Solicitudes Notariales Vencidas";
                            string cuerpo = "Las siguientes solicitudes tienen mas de 24 hrs sin respuesta<br><ul>";

                            foreach(var v in vencidas)
                            {
                                cuerpo += $"<li>Solicitud #{v.IdSolicitud} - {v.NombreMarca} {v.NombreModelo} - Cliente: {v.NombreCliente} - Escribano: {v.NombreEscribano} - Fecha: {v.FechaSolicitud:dd//MM/yyyy HH:mm}</li>";
                            }
                            cuerpo += "</ul>";

                            foreach(var admin in administradores)
                            {
                                await emailService.EnviarCorreo(admin.Email, admin.NombreCompleto, asunto, cuerpo);
                            }
                        }
                    }
                    UltimaEjecucionExitosa = true;
                }
                catch (Exception ex)
                {
                    UltimaEjecucionExitosa = false;
                    _logger.LogError(ex, "Error al verificar solititudes vencidas");
                }
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
