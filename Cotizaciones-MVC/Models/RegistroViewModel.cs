using System.ComponentModel.DataAnnotations;

namespace Cotizaciones_MVC.Models
{
    public class RegistroViewModel
    {

        [Required(ErrorMessage = "Campo {0} requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        [EmailAddress(ErrorMessage = "El campo debe de ser un correo electrónico válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        public string Password { get; set; }    
    }
}
