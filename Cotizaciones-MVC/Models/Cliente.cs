using System.ComponentModel.DataAnnotations;

namespace Cotizaciones_MVC.Models
{
    public class Cliente
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        public string name { get; set; }
        [Required(ErrorMessage = "Campo {0} requerido")]
        [EmailAddress(ErrorMessage = "El campo debe de ser un correo electrónico válido")]
        public string email { get; set; }
        [Required(ErrorMessage = "Campo {0} requerido")]
        public string phone { get;set; }

    }
}
