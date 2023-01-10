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

            //Validamos si existe el Email
            var exitEmail = await repository.Existe(cliente.email);


            //Validamos a nivel controller y lanzamos el error
            if (exitEmail)
            {
                ModelState.AddModelError(nameof(cliente.email), $"El email {cliente.email} ya existe en la base de datos");
                return View(cliente);
            }


            await  repository.Crear(cliente);

            return View();
        }
    }
}
