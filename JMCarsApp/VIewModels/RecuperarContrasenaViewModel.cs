using System.ComponentModel.DataAnnotations;

namespace JMCarsApp.ViewModels
{
    public class RecuperarContrasenaViewModel
    {
        [Required(ErrorMessage = "El mail es obligatorio.")]
        [EmailAddress(ErrorMessage = "El Email no tiene un formato valido.")]
        public string Email { get; set; }
    }
}
