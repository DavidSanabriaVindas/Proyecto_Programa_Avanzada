using PYME.Models;

namespace PYME.Services
{
    public interface IMovimientoService
    {
        List<MovimientoInventario> ObtenerTodos();
        List<MovimientoInventario> ObtenerPorProducto(int idProducto);
        MovimientoInventario? ObtenerDetalle(int id);

        (bool success, string mensaje) RegistrarEntrada(MovimientoInventario movimiento);
        (bool success, string mensaje) RegistrarSalida(MovimientoInventario movimiento);
    }
}