using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenController : ControllerBase
    {
        private const int MaximoFotos = 3;
        private const long MaximoBytesPorFoto = 8 * 1024 * 1024;
        private readonly string _rutaCarpeta;
        public ImagenController(IConfiguration configuration)
        {
            _rutaCarpeta = configuration["Imagenes:Ruta"]!;
        }

        [HttpPost("subir")]
        public async Task<IActionResult> Subir(List<IFormFile> fotos)
        {
            if(fotos.Count == 0)
            {
                return BadRequest(new { error = "No se ha recibido ninguna foto" });
            }

            if(fotos.Count > MaximoFotos)
            {
                return BadRequest(new { error = $"Maximo {MaximoFotos} fotos por publicacion" });
            }
            List<string> extensiones = new List<string>();

            foreach(IFormFile foto in fotos)
            {
                if(foto.Length > MaximoBytesPorFoto)
                {
                    return BadRequest(new { error = "Cada foto debe tener 8MB como maaximo" });
                }
                    
                string? extension = await DetectarExtension(foto);
                if(extension == null)
                {
                    return BadRequest(new { error = "Solo se permiten formatos .jpg y .png" });
                }
                extensiones.Add(extension);
            }

            List<string> rutas = new List<string>();

            for(int i = 0; i < fotos.Count; i++)
            {
                string nombre = $"{Guid.NewGuid():N}{extensiones[i]}";
                string rutaCompleta = Path.Combine(_rutaCarpeta, nombre);

                using (FileStream destino = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await fotos[i].CopyToAsync(destino);
                }
                rutas.Add("images/" + nombre);
            }
            return Ok(new { rutas });

        }

        private static async Task<string?> DetectarExtension(IFormFile archivo)
        {
            byte[] cabecera = new byte[4];

            using (Stream stream = archivo.OpenReadStream())
            {
                int leidos = await stream.ReadAsync(cabecera, 0, 4);
                if(leidos < 4)
                {
                    return null;
                }

            }
            if (cabecera[0] == 0xFF && cabecera[1] == 0xD8 && cabecera[2] == 0xFF)
            {
                return ".jpg";
            }
            if (cabecera[0] == 0x89 && cabecera[1] == 0x50 && cabecera[2] == 0x4E && cabecera[3] == 0x47)
            {
                return ".png";
            }

            return null;
        }

    }
}
