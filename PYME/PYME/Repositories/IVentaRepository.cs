using PYME.Models;

namespace PYME.Repositories
{
    public interface IVentaRepository
    {
        List<Venta> ObtenerTodos();
        Venta? ObtenerPorId(int id);
        void Agregar(Venta venta);
        void Actualizar(Venta venta);
        void Eliminar(int id);
    }
}