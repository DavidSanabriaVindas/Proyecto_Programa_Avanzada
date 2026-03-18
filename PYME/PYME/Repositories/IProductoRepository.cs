using PYME.Models;

namespace PYME.Repositories
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        Producto? ObtenerPorId(int id);
        bool ExisteSKU(string sku);
        void Agregar(Producto producto);
        void Actualizar(Producto producto);
        void Eliminar(int id);
        void ActualizarStock(int idProducto, int nuevoStock);
    }
}
