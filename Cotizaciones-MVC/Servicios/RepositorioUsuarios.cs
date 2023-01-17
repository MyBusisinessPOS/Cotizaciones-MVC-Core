using Cotizaciones_MVC.Models;
using Dapper;
using System.Data.SqlClient;

namespace Cotizaciones_MVC.Servicios
{

    public interface IRepositorioUsuarios{
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CreaUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {

        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration) {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreaUsuario(Usuario usuario) {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO usuarios (email, email_normalizado, password_hash, nombre) VALUES  (@email, @email_normalizado, @password_hash, @nombre);
                                                                        SELECT SCOPE_IDENTITY();", usuario); 
            return id;
        }

   
        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using var connection = new SqlConnection(connectionString);
            var usuario = await connection.QuerySingleOrDefaultAsync<Usuario>($@"SELECT id, email, email_normalizado, password_hash, nombre FROM usuarios WHERE email_normalizado = @emailNormalizado", new { emailNormalizado });
            return usuario;
        }


    }
}
