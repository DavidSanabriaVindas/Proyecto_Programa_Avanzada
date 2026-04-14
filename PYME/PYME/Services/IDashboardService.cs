using PYME.Models;

namespace PYME.Services
{
    public interface IDashboardService
    {
        DashboardViewModel ObtenerDashboard(DateTime fechaInicio, DateTime fechaFin);
        HistorialProductoVM ObtenerHistorialProducto(int idProducto, DateTime? fechaInicio, DateTime? fechaFin, string? tipoFiltro);
    }
}