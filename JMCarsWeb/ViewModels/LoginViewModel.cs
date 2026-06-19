using System.ComponentModel.DataAnnotations;

namespace JMCarsWeb.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El mail es obligatorio.")]
        [EmailAddress(ErrorMessage = "Es Email no tiene un formato valido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es Obligatoria")]
        public string Contrasena { get; set; }
    }
}
