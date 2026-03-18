using PYME.Models;

namespace PYME.Services
{
    public interface IProductoService
    {
        List<Producto> ObtenerTodos();
        Producto? ObtenerDetalle(int id);

        bool CrearProducto(ProductoViewModel modelo);
        bool ActualizarProducto(int id, ProductoViewModel modelo);
        bool EliminarProducto(int id);
    }
}