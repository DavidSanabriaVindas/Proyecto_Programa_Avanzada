using PYME.Models;

namespace PYME.Services
{
    public interface IVentaService
    {
        List<Venta> ObtenerTodos();
        Venta? ObtenerDetalle(int id);
        (bool success, string mensaje) CrearVenta(Venta venta, List<Detalle_Venta> detalles);
        (bool success, string mensaje) ActualizarEstado(int id, string nuevoEstado);
        bool EliminarVenta(int id);
    }
}