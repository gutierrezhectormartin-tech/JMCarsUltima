using System.ComponentModel.DataAnnotations;

namespace JMCarsWeb.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set;}

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        [StringLength(255, ErrorMessage = "El email no puede superar los 255 caracteres.")]
        public string Email { get; set; }

        [StringLength(255, MinimumLength = 3, ErrorMessage = "La contraseña debe tener entre 3 y 255 caracteres.")]
        public string? Contrasena { get; set; }
        public bool EstadoUsu { get; set; }
        public int RolUsu { get; set; }
        public DateTime? FechaAceptacionTerminos { get; set; }

    }
}
