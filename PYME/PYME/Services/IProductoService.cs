using PYME.Models;

namespace PYME.Services
{
    public interface IProductoService
    {
        List<Producto> ObtenerTodos();
        Producto? ObtenerDetalle(int id);
        bool CrearProducto(Producto producto);
        bool ActualizarProducto(int id, Producto producto);
        bool EliminarProducto(int id);
    }
}