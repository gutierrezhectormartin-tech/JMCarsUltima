using JMCarsWeb.Services;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("JMCarsAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
});

builder.Services.AddScoped<VehiculoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<EscribanoService>();


builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


var proveedorTiposDeArchivo = new FileExtensionContentTypeProvider();
proveedorTiposDeArchivo.Mappings[".avif"] = "image/avif";
proveedorTiposDeArchivo.Mappings[".webp"] = "image/webp";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = proveedorTiposDeArchivo
});

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
