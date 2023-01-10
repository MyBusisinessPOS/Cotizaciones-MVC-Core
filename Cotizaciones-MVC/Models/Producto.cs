using System.ComponentModel.DataAnnotations;

namespace Cotizaciones_MVC.Models
{
    public class Producto
    {

        public int id { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")] //Valida y establece el mensaje de validacion
        public string bar_code { get; set; }


        [Required(ErrorMessage = "Campo {0} requerido")] //Valida y establece el mensaje de validacion
        public string descrip { get; set; }


        [Required(ErrorMessage = "Campo {0} requerido")] //Valida y establece el mensaje de validacion
        public double price { get; set; }


        [Required(ErrorMessage = "Campo {0} requerido")] //Valida y establece el mensaje de validacion
        public string unit { get; set; }
    }
}
