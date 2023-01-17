using Cotizaciones_MVC.Models;
using Dapper;
using System.Data.SqlClient;

namespace Cotizaciones_MVC.Servicios
{

    public interface IRepositorioCotizaciones {
        Task ActualizarImportes(double importe, int id);
        Task<Cotizacion> CotizacionPorId(int id);
        Task Crear(Cotizacion modelo);
        Task CrearDetalle(DetalleCotizacion modelo);
        Task<IEnumerable<DetalleCotizacion>> ObtienePartidasPorIdCotizacion(int cotizacion);
        Task<IEnumerable<Cotizacion>> OrdenesGeneradas();
    }

    public class RepositorioCotizaciones : IRepositorioCotizaciones
    {

        //Contiene la lista de productos
        private readonly string connectionString;

        public RepositorioCotizaciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Cotizacion> CotizacionPorId(int id)
        {

            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Cotizacion>($@"SELECT id, cliente, datos, importe, impuesto, fecha, hora FROM cotizacion WhERE id = @id", new { id });

        }

        public async Task<IEnumerable<DetalleCotizacion>> ObtienePartidasPorIdCotizacion(int cotizacion) {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<DetalleCotizacion>($@"SELECT id, bar_code, descripcion, precio, cantidad, cotizacion, total  FROM detalle_cotizacion WHERE cotizacion = @cotizacion", new { cotizacion });
        }


        //usamos el metodo asyncrono para realizar el Insert a la base de datos
        public async Task Crear(Cotizacion modelo)
        {

            using var connection = new SqlConnection(connectionString);

            //Me permite obtener el id que se inserto en la base de datos
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO cotizacion (cliente ,datos,importe, impuesto, fecha, hora) VALUES (@cliente, @datos, @importe, @impuesto, @fecha, @hora);
                                                                        SELECT SCOPE_IDENTITY();", modelo); //SELECT SCOPE OBTIENE EL ID DEL REGISTRO
            //Contiene el ID del cliente insertado
            modelo.id = id;
        }


        public async Task CrearDetalle(DetalleCotizacion modelo)
        {

            using var connection = new SqlConnection(connectionString);

            //Me permite obtener el id que se inserto en la base de datos
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO detalle_cotizacion (bar_code ,descripcion,precio, cantidad, cotizacion, total) VALUES (@bar_code ,@descripcion, @precio, @cantidad, @cotizacion, @total);
                                                                        SELECT SCOPE_IDENTITY();", modelo); //SELECT SCOPE OBTIENE EL ID DEL REGISTRO
            //Contiene el ID del cliente insertado
            modelo.id = id;
        }



        public async Task ActualizarImportes(double importe, int id)
        {

            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE cotizacion SET importe = @importe WHERE id = @id ", new { importe, id });
        }



        public async Task<IEnumerable<Cotizacion>> OrdenesGeneradas()
        {

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cotizacion>($@"SELECT id, cliente ,datos,importe, impuesto, fecha, hora  FROM cotizacion ");
        }



    }
}
