using Cotizaciones_MVC.Models;
using Cotizaciones_MVC.Servicios;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Cotizaciones_MVC.Controllers
{
    public class ClientesController : Controller
    {

        private readonly IRepositorioClientes repository;

        public ClientesController(IRepositorioClientes repository) {
            this.repository = repository;
        }

        public async Task<IActionResult> Index() {

            var clientes = await repository.Obtener();
            return View(clientes);
        }


        public IActionResult Crear() {

            return View();
        }

        [HttpPost] //Metodo asyncrono
        public async Task<IActionResult> Crear(Cliente cliente)
        {

            //Validamos el modelo 
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            //EL SIGUIENTE METODO SE REMPLAZO PARA REALIZAR 
            //var exitEmail = await repository.Existe(cliente.email);
            //
            //
            ////Validamos a nivel controller y lanzamos el error
            //if (exitEmail)
            //{
            //    ModelState.AddModelError(nameof(cliente.email), $"El email {cliente.email} ya existe en la base de datos");
            //    return View(cliente);
            //}


            await repository.Crear(cliente);

            return RedirectToAction("Index");
        }


        //Metodo que nos permitira obtener por medio de JavaScrip si existe el email
        [HttpGet]
        public async Task<IActionResult> VerificarExiteEmail(string email) {

            var exitEmail = await repository.Existe(email);

            if (exitEmail)
            {
                return Json($"El email {email} ya existe en la base de datos");
            }

            return Json(true);
        }




        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await repository.ObtenerPorId(id);


            return View(cliente);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(Cliente cliente)
        {

            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            await repository.Actualizar(cliente);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Borrar(int id)
        {
            var cliente = await repository.ObtenerPorId(id);
            return View(cliente);

        }



        [HttpPost]
        public async Task<IActionResult> BorrarCliente(int id)
        {

            var cliente = await repository.ObtenerPorId(id);
            await repository.Borrar(cliente.id);

            return RedirectToAction("Index");
        }



    }
}
