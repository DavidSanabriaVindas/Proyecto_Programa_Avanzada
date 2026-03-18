using PYME.Models;
using PYME.Repositories;

namespace PYME.Services
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository _repository;
        private readonly IProductoRepository _productoRepository;

        public MovimientoService(IMovimientoRepository repository, IProductoRepository productoRepository)
        {
            _repository = repository;
            _productoRepository = productoRepository;
        }

        public List<MovimientoInventario> ObtenerTodos()
            => _repository.ObtenerTodos();

        public List<MovimientoInventario> ObtenerPorProducto(int idProducto)
            => _repository.ObtenerPorProducto(idProducto);

        public MovimientoInventario? ObtenerDetalle(int id)
            => _repository.ObtenerPorId(id);

        public (bool success, string mensaje) RegistrarEntrada(MovimientoInventario movimiento)
        {
            var producto = _productoRepository.ObtenerPorId(movimiento.Id_Producto);
            if (producto == null)
                return (false, "Producto no encontrado.");

            producto.Stock_Actual += movimiento.Cantidad;
            producto.Fecha_Actualizacion = DateTime.Now;
            _productoRepository.Actualizar(producto);

            movimiento.Tipo_Movimiento = "ENTRADA";
            movimiento.Fecha_Movimiento = DateTime.Now;
            _repository.Agregar(movimiento);

            return (true, $"Entrada registrada. Stock actual: {producto.Stock_Actual}");
        }

        public (bool success, string mensaje) RegistrarSalida(MovimientoInventario movimiento)
        {
            var producto = _productoRepository.ObtenerPorId(movimiento.Id_Producto);
            if (producto == null)
                return (false, "Producto no encontrado.");

            if (producto.Stock_Actual < movimiento.Cantidad)
                return (false, $"Stock insuficiente. Stock actual: {producto.Stock_Actual}");

            producto.Stock_Actual -= movimiento.Cantidad;
            producto.Fecha_Actualizacion = DateTime.Now;
            _productoRepository.Actualizar(producto);

            movimiento.Tipo_Movimiento = "SALIDA";
            movimiento.Fecha_Movimiento = DateTime.Now;
            _repository.Agregar(movimiento);

            return (true, $"Salida registrada. Stock actual: {producto.Stock_Actual}");
        }
    }
}