using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using JMCarsApp.Services;

namespace JMCarsApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped<UsuarioService>();
            builder.Services.AddHttpClient("JMCarsAPI", cliente => cliente.BaseAddress = new Uri("https://10.0.2.2:7289"))
#if DEBUG
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (mensaje, certificado, cadena, errores) => true
})
#endif
;
            builder.Services.AddSingleton<SessionService>();

#if DEBUG

            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();

#endif

            return builder.Build();
        }
    }
}
