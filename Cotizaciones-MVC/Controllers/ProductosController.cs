using Cotizaciones_MVC.Models;
using Cotizaciones_MVC.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Cotizaciones_MVC.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IRepositorioProductos repository;

        public ProductosController(IRepositorioProductos repository)
        {
            this.repository = repository;
        }


        public async Task<IActionResult> Index() {
            var producto = await repository.Obtener();
            return View(producto);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Producto producto) {

            if (!ModelState.IsValid)
            {
                return View(producto);
            }

    
            await repository.Crear(producto);
             
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var producto = await repository.ObtenerPorId(id);


            return View(producto);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(Producto producto)
        {

            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            await repository.Actualizar(producto);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Borrar(int id) {
            var producto = await repository.ObtenerPorId(id);
            return View(producto);

        }



        [HttpPost]
        public async Task<IActionResult> BorrarProducto(int id)
        {

            var producto = await repository.ObtenerPorId(id);
            await repository.Borrar(producto.id);

            return RedirectToAction("Index");
        }


    }
}
