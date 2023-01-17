using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cotizaciones_MVC.Models
{
    public class Cotizacion
    {

        public int id { get; set; }

        [Display(Name = "Cliente")]
        public int cliente { get; set; }
        public string datos { get; set; }
        public double importe { get; set; }
        public double impuesto { get; set; }

        [Display(Name = "Fecha Cotización")]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; } = DateTime.Today;
        public string hora { get; set; }
        public bool estado { get; set; }    

    }
}
