using Logica;
using Logica.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Persistencia;
using Persistencia.Interfaces;
using WebAPI.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmailService, EmailServices>();
builder.Services.AddHostedService<VerificadorSolicitudesVencidasService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string rutaImagenes = app.Configuration["Imagenes:Ruta"]!;
Directory.CreateDirectory(rutaImagenes);

FileExtensionContentTypeProvider proveedorTipoDeArchivo = new FileExtensionContentTypeProvider();
proveedorTipoDeArchivo.Mappings[".avif"] = "image/avif";
proveedorTipoDeArchivo.Mappings[".webp"] = "image/webp";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(rutaImagenes),
    RequestPath = "/images",
    ContentTypeProvider = proveedorTipoDeArchivo
});

app.UseAuthorization();

app.MapControllers();

app.Run();
