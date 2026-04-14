using PYME.Models;

namespace PYME.Repositories
{
    public interface IDashboardRepository
    {
        List<MovimientoInventario> ObtenerMovimientosPorPeriodo(DateTime fechaInicio, DateTime fechaFin);
        List<MovimientoInventario> ObtenerMovimientosPorAnio(int anio);
        List<MovimientoInventario> ObtenerMovimientosPorProducto(int idProducto, DateTime? fechaInicio, DateTime? fechaFin, string? tipoFiltro);
        List<ProductoStockBajoVM> ObtenerProductosStockBajo();
        List<Producto> ObtenerTodosLosProductosActivos();
        Producto? ObtenerProductoPorId(int idProducto);
    }
}