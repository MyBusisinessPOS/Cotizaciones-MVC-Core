using Cotizaciones_MVC.Models;
using Dapper;
using System.Data.SqlClient;

namespace Cotizaciones_MVC.Servicios
{

    public interface IRepositorioProductos {

        Task Crear(Producto producto);

        Task<IEnumerable<Producto>> Obtener();

        Task Actualizar(Producto producto);

        Task<Producto> ObtenerPorId(int id);

        Task Borrar(int id);
    }
    public class RepositorioProductos : IRepositorioProductos
    {


        private readonly string connectionString;

        public RepositorioProductos(IConfiguration configuration) {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Producto producto)
        {
            using var connection = new SqlConnection(connectionString);

            //Me permite obtener el id que se inserto en la base de datos
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO productos (bar_code, descrip, price, unit) VALUES  (@bar_code, @descrip, @price, @unit);
                                                                        SELECT SCOPE_IDENTITY();", producto); //SELECT SCOPE OBTIENE EL ID DEL REGISTRO
            //Contiene el ID del PRODUCTO insertado
            producto.id = id;
        }

        public async Task<IEnumerable<Producto>> Obtener()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Producto>($@"SELECT id, bar_code, descrip, price, unit FROM productos");
        }


        public async Task Actualizar(Producto producto) {

            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE productos SET descrip = @descrip, price = @price, unit = @unit WHERE bar_code = @bar_code ", producto);
        }


        //Ontiene por id el modelo
        public async Task<Producto> ObtenerPorId(int id) {

            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Producto>($@"SELECT id, bar_code, descrip, price, unit FROM productos WhERE id = @id", new {id});

        }

        public async Task Borrar(int id) {
            using var connection = new SqlConnection(connectionString);

             await connection.QueryFirstOrDefaultAsync<Producto>($@"DELETE FROM productos WhERE id = @id", new { id });
        }
    }
}
