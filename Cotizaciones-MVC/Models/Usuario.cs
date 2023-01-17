using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cotizaciones_MVC.Models
{
    public class Usuario
    {
        public int id { get; set; }

        public string email { get; set; }

        public string email_normalizado { get; set; }

        public string password_hash { get; set; }

        public string nombre { get; set; }  
    }
}
