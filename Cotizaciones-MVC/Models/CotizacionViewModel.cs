using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cotizaciones_MVC.Models
{
    public class CotizacionViewModel
    {

        public Cotizacion Cotizacion;
        public Cliente Cliene { get; set; }
        public Producto Producto { get; set; }
        public DetalleCotizacion DetalleCotizacion { get; set; }  
        public IEnumerable<SelectListItem> Clientes { get; set; }
        public IEnumerable<SelectListItem> Productos { get; set; }
        public IEnumerable<Cotizacion> Cotizaciones { get; set; }
        public IEnumerable<DetalleCotizacion> DetalleCotizaciones { get; set; }

    }
}
