using Cotizaciones_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cotizaciones_MVC.Controllers
{
    public class ProductosController : Controller
    {

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Producto producto) {
            return View();
        } 

       

    }
}
