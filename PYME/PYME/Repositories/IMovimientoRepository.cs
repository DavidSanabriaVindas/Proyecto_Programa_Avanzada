using PYME.Models;

namespace PYME.Repositories
{
    public interface IMovimientoRepository
    {
        List<MovimientoInventario> ObtenerTodos();
        List<MovimientoInventario> ObtenerPorProducto(int idProducto);
        MovimientoInventario? ObtenerPorId(int id);
        void Agregar(MovimientoInventario movimiento);
    }
}
