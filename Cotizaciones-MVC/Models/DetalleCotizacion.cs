namespace Cotizaciones_MVC.Models
{
    public class DetalleCotizacion
    {
	    public int id { get; set; }	
	    public string bar_code { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public double cantidad { get; set; }
        public int id_partida { get; set; }
        public int cotizacion { get; set; }

        public double total { get; set; }
    }
}
