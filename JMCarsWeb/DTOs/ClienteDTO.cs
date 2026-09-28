using System.ComponentModel.DataAnnotations;

namespace JMCarsWeb.DTOs
{
    public class ClienteDTO : UsuarioDTO
    {
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [RegularExpression(@"^[0-9]{7,8}$", ErrorMessage = "La cédula debe tener 7 u 8 dígitos numéricos.")]
        public string Cedula { get; set; }
    }
}
