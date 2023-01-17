using Cotizaciones_MVC.Models;
using Cotizaciones_MVC.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using System.Security.Cryptography.Xml;

namespace Cotizaciones_MVC.Controllers
{
    public class CotizacionesController : Controller
    {

        //Clientes
        public readonly IRepositorioClientes repositorioClientes;

        //Prductos
        public readonly IRepositorioProductos repositorioProductos;

        //Cotizacion 
        public readonly IRepositorioCotizaciones repositorioCotizaciones;

        public CotizacionesController(
            IRepositorioClientes repositorioClientes,
            IRepositorioProductos repositorioProductos,
            IRepositorioCotizaciones repositorioCotizaciones)
        {
            this.repositorioClientes = repositorioClientes;
            this.repositorioProductos = repositorioProductos;
            this.repositorioCotizaciones = repositorioCotizaciones;
        }



        //Creamos la persistencia de datos en la vista
        [BindProperty]
        public CotizacionViewModel cotizacionVM { get; set; }


        [Authorize]
        public async Task<IActionResult> Index()
        {

            var cotizaciones = await repositorioCotizaciones.OrdenesGeneradas();
            return View(cotizaciones);
        }

        //Retorna la lista de clientes
        private async Task<IEnumerable<SelectListItem>> ListaClientes()
        {
            var cuentas = await repositorioClientes.Obtener();
            return cuentas.Select(x => new SelectListItem(x.name, x.id.ToString()));
        }

        //Retorna la lista de los productos
        private async Task<IEnumerable<SelectListItem>> ListaProductos()
        {
            var categorias = await repositorioProductos.Obtener();
            return categorias.Select(x => new SelectListItem(x.descrip, x.bar_code.ToString()));
        }


        public async Task<IActionResult> Crear(int? cotizacionId)
        {

            //Intanciamos el modelo
            cotizacionVM = new CotizacionViewModel();


            //Contiene la lista de productos
            cotizacionVM.Productos = await ListaProductos();


            //Contiene la lista de los clientes
            cotizacionVM.Clientes = await ListaClientes();

            //Contiene la lista de los productos de la cotizacion
            cotizacionVM.DetalleCotizaciones = new List<DetalleCotizacion>();

            //Si hay datos obtenemos elos datos de la cotizacion por el ID enviado por parametro
            if (cotizacionId != null)
            {
                //Contiene el encabezado
                var cotizacion = await repositorioCotizaciones.CotizacionPorId((int)cotizacionId);
                cotizacionVM.Cotizacion = cotizacion;

                //Contiene el desatalle
                var detalle = await repositorioCotizaciones.ObtienePartidasPorIdCotizacion(cotizacion.id);
                cotizacionVM.DetalleCotizaciones = detalle;
            }

            return View(cotizacionVM);
        }

       
        public async Task<IActionResult> emitirCotizacion(int cotizacionId) {

            //Intanciamos el modelo
            cotizacionVM = new CotizacionViewModel();

            cotizacionVM.DetalleCotizaciones = new List<DetalleCotizacion>();

            //Si hay datos obtenemos elos datos de la cotizacion por el ID enviado por parametro
            if (cotizacionId != null)
            {
                //Contiene el encabezado
                var cotizacion = await repositorioCotizaciones.CotizacionPorId((int)cotizacionId);
                cotizacionVM.Cotizacion = cotizacion;

                //Contiene el desatalle
                var detalle = await repositorioCotizaciones.ObtienePartidasPorIdCotizacion(cotizacion.id);
                cotizacionVM.DetalleCotizaciones = detalle;
            }

            return  new ViewAsPdf("emitirCotizacion", cotizacionVM)
            {
                FileName = $"Cotizacion#{cotizacionVM.Cotizacion.id}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
            };
        }


        public async Task<IActionResult> imprimirCotizacion(int id)
        {
            cotizacionVM = new CotizacionViewModel();

            cotizacionVM.DetalleCotizaciones = new List<DetalleCotizacion>();

            //Si hay datos obtenemos elos datos de la cotizacion por el ID enviado por parametro
            if (id != null)
            {
                //Contiene el encabezado
                var cotizacion = await repositorioCotizaciones.CotizacionPorId((int)id);
                cotizacionVM.Cotizacion = cotizacion;

                //Contiene el desatalle
                var detalle = await repositorioCotizaciones.ObtienePartidasPorIdCotizacion(cotizacion.id);
                cotizacionVM.DetalleCotizaciones = detalle;
            }

            return new ViewAsPdf("emitirCotizacion", cotizacionVM)
            {
                FileName = $"Cotizacion#{cotizacionVM.Cotizacion.id}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
            };

        }

        [HttpPost]
        public async Task<IActionResult> AgregarProductoPost(string bar_code, int cantidad, int cotizacionId, int cliente)
        {


            cotizacionVM = new CotizacionViewModel();
            cotizacionVM.Cotizacion = new Cotizacion();


            cotizacionVM.Cotizacion.id = cotizacionId;

            //Si el valor del id es 0 entonces no hay cotizacion 
            if (cotizacionVM.Cotizacion.id == 0)
            {

                cotizacionVM.Cotizacion.estado = false;
                cotizacionVM.Cotizacion.importe = 200;
               
                //Contiene el modelo del cliente por el id del cliente seleccionado
                var clienteDatos = await repositorioClientes.ObtenerPorId(cliente);
                cotizacionVM.Cotizacion.cliente = clienteDatos.id;
                cotizacionVM.Cotizacion.datos = clienteDatos.name;
               

                //Creamos la cotizacion en la base de datos
                await repositorioCotizaciones.Crear(cotizacionVM.Cotizacion);
               
                
                //Contiene el ID de la cotizacion insertada
                var cotizaIdentificador = cotizacionVM.Cotizacion.id;
                cotizacionId = cotizaIdentificador;

                cotizacionVM.DetalleCotizacion = new DetalleCotizacion();

                var productos = await repositorioProductos.ObtenerPorBarcode(bar_code);

                cotizacionVM.DetalleCotizacion.cotizacion = cotizacionId;
                cotizacionVM.DetalleCotizacion.bar_code = bar_code;
                cotizacionVM.DetalleCotizacion.cantidad = cantidad;
                cotizacionVM.DetalleCotizacion.precio = productos.price;
                cotizacionVM.DetalleCotizacion.total = productos.price * cantidad;
                cotizacionVM.DetalleCotizacion.descripcion = productos.descrip;

                await repositorioCotizaciones.CrearDetalle(cotizacionVM.DetalleCotizacion);


                //Actualizamos los importes en la tabla cotizacion
                await repositorioCotizaciones.ActualizarImportes((productos.price * cantidad), cotizacionId);



            }
            else {

                cotizacionVM.DetalleCotizacion = new DetalleCotizacion();
                var productos = await repositorioProductos.ObtenerPorBarcode(bar_code);
                cotizacionVM.DetalleCotizacion.cotizacion = cotizacionId;
                cotizacionVM.DetalleCotizacion.bar_code = bar_code;
                cotizacionVM.DetalleCotizacion.cantidad = cantidad;
                cotizacionVM.DetalleCotizacion.precio = productos.price;
                cotizacionVM.DetalleCotizacion.total = productos.price * cantidad;
                cotizacionVM.DetalleCotizacion.descripcion = productos.descrip;

                await repositorioCotizaciones.CrearDetalle(cotizacionVM.DetalleCotizacion);

            }
            
            return RedirectToAction("Crear", new { cotizacionId = cotizacionVM.Cotizacion.id });

        }



    }
}
