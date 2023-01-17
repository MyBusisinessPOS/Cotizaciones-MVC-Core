using Cotizaciones_MVC.Servicios;
using System.Security.Claims;

namespace Cotizaciones_MVC.Servicios
{

    public interface IServiciosServicios
    {
        int ObtenerUsuarioId();
    }

    public class ServicioUsuario : IServiciosServicios
    {
        private readonly HttpContext httpContext;

        public ServicioUsuario(IHttpContextAccessor httpContextAccessor) { 
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public int ObtenerUsuarioId()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User
                     .Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                var id = int.Parse(idClaim.Value);

                return id;

            }
            else
            {
                throw new ApplicationException("El usuario no esa autenticado");
            }
            
        }
    }
}
