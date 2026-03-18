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

        public bool CrearProducto(ProductoViewModel modelo)
        {
            var productos = _repository.ObtenerTodos();

            if (productos.Any(p => p.SKU == modelo.SKU))
                return false;

            var producto = new Producto
            {
                SKU = modelo.SKU,
                Nombre = modelo.Nombre,
                Descripcion = modelo.Descripcion,
                Precio_Costo = modelo.Precio_Costo,
                Precio_Venta = modelo.Precio_Venta,
                Stock_Actual = modelo.Stock_Actual,
                Stock_Minimo = modelo.Stock_Minimo,
                Estado = modelo.Estado,
                Fecha_Creacion = DateTime.Now
            };

            _repository.Agregar(producto);
            return true;
        }

        public bool ActualizarProducto(int id, ProductoViewModel modelo)
        {
            var productoExistente = _repository.ObtenerPorId(id);
            if (productoExistente == null)
                return false;

            var productos = _repository.ObtenerTodos();
            if (productoExistente.SKU != modelo.SKU
                && productos.Any(p => p.SKU == modelo.SKU))
                return false;

            productoExistente.SKU = modelo.SKU;
            productoExistente.Nombre = modelo.Nombre;
            productoExistente.Descripcion = modelo.Descripcion;
            productoExistente.Precio_Costo = modelo.Precio_Costo;
            productoExistente.Precio_Venta = modelo.Precio_Venta;
            productoExistente.Stock_Actual = modelo.Stock_Actual;
            productoExistente.Stock_Minimo = modelo.Stock_Minimo;
            productoExistente.Estado = modelo.Estado;
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