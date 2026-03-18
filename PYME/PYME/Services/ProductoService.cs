using PYME.Models;
using PYME.Repositories;

namespace PYME.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public List<Producto> ObtenerTodos()
            => _repository.ObtenerTodos();

        public Producto? ObtenerDetalle(int id)
            => _repository.ObtenerPorId(id);

        public bool CrearProducto(Producto producto)
        {
            if (_repository.ExisteSKU(producto.SKU))
                return false;

            producto.Stock_Actual = 0;
            producto.Fecha_Creacion = DateTime.Now;

            _repository.Agregar(producto);
            return true;
        }

        public bool ActualizarProducto(int id, Producto producto)
        {
            var productoExistente = _repository.ObtenerPorId(id);
            if (productoExistente == null)
                return false;

            if (productoExistente.SKU != producto.SKU && _repository.ExisteSKU(producto.SKU))
                return false;

            productoExistente.SKU = producto.SKU;
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio_Costo = producto.Precio_Costo;
            productoExistente.Precio_Venta = producto.Precio_Venta;
            productoExistente.Stock_Minimo = producto.Stock_Minimo;
            productoExistente.Estado = producto.Estado;
            productoExistente.Fecha_Actualizacion = DateTime.Now;

            _repository.Actualizar(productoExistente);
            return true;
        }

        public bool EliminarProducto(int id)
        {
            var producto = _repository.ObtenerPorId(id);
            if (producto == null)
                return false;

            _repository.Eliminar(id);
            return true;
        }
    }
}