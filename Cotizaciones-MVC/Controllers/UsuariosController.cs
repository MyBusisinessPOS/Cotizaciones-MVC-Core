using Cotizaciones_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cotizaciones_MVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> userManager;


        //Contiene la Cookie para saber quien es el usuario logeado
        private readonly SignInManager<Usuario> signInManager;

        public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager) {

            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Registro() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel registroViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(registroViewModel);
            }

            var usuario = new Usuario()
            {
                email = registroViewModel.Email,
                nombre = registroViewModel.Nombre
            };

            var resultado = await userManager.CreateAsync(usuario, password: registroViewModel.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Cotizaciones");
            }
            else {

                foreach (var error in resultado.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(registroViewModel);
            }            
        }


        [HttpPost]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {

            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var resultado = await signInManager.PasswordSignInAsync(login.Email, login.Password, login.Recuerdame, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Cotizaciones");

            }
            else {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto");
                return View(login);
            }
             
        }

        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }

    }
}
