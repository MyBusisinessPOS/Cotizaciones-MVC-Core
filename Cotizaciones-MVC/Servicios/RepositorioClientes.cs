using Cotizaciones_MVC.Models;
using Dapper;
using System.Data.SqlClient;

namespace Cotizaciones_MVC.Servicios
{

    public interface IRepositorioClientes {
        Task Crear(Cliente cliente);
        Task<bool> Existe(string email);
        Task<IEnumerable<Cliente>> Obtener();
        Task Actualizar(Cliente cliente);
        Task<Cliente> ObtenerPorId(int id);
        Task Borrar(int id);
    }

    public class RepositorioClientes : IRepositorioClientes
    {

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



        public async Task<IEnumerable<Cliente>> Obtener() {
            using var connection = new SqlConnection(connectionString);

            //Me permite obtener el id que se inserto en la base de datos
            return await connection.QueryAsync<Cliente>($@"SELECT id, name, email, phone FROM clientes "); //SELECT SCOPE OBTIENE EL ID DEL REGISTRO

        }


        public async Task Actualizar(Cliente cliente)
        {

            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE clientes SET name = @name, email = @email, phone = @phone WHERE id = @id ", cliente);
        }


        //Ontiene por id el modelo
        public async Task<Cliente> ObtenerPorId(int id)
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cliente>($@"SELECT id, name, email, phone FROM clientes WhERE id = @id", new { id });

        }


        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.QueryFirstOrDefaultAsync<Cliente>($@"DELETE FROM clientes WHERE id = @id", new { id });
        }

    }
}
