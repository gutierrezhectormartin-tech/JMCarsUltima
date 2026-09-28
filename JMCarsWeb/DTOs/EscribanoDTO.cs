using System.ComponentModel.DataAnnotations;

namespace JMCarsWeb.DTOs
{
    public class EscribanoDTO : UsuarioDTO
    {
        [Required(ErrorMessage = "El número de caja profesional es obligatorio.")]
        [StringLength(50, ErrorMessage = "El número de caja no puede superar los 50 caracteres.")]
        public string NumeroCaja { get; set; }

        [Required(ErrorMessage = "La dirección del estudio es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres.")]
        public string Direccion { get; set; }

    }
}
