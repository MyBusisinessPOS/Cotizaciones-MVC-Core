using Cotizaciones_MVC.Models;
using Dapper;
using System.Data.SqlClient;

namespace Cotizaciones_MVC.Servicios
{

    public interface IRepositorioClientes {
        Task Crear(Cliente cliente);

        Task<bool> Existe(string email);
    }

    public class RepositorioClientes : IRepositorioClientes
    {


        private string table_name = "clientes";
        private readonly string connectionString;

        public RepositorioClientes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //usamos el metodo asyncrono para realizar el Insert a la base de datos
        public async Task Crear(Cliente cliente) {

            using var connection = new SqlConnection(connectionString);

            //Me permite obtener el id que se inserto en la base de datos
            var id = await  connection.QuerySingleAsync<int>($@"INSERT INTO clientes (name, email, phone) VALUES  (@name, @email, @phone);
                                                                        SELECT SCOPE_IDENTITY();", cliente); //SELECT SCOPE OBTIENE EL ID DEL REGISTRO
            //Contiene el ID del cliente insertado
            cliente.id = id;
        }

        public async Task<bool> Existe(string email)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>($"SELECT 1 FROM clientes WHERE email = @email ", new { email } );
            return existe == 1;
        }
    }
}
