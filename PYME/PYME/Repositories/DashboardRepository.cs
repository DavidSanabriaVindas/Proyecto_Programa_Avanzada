using Microsoft.EntityFrameworkCore;
using PYME.Data;
using PYME.Models;

namespace PYME.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<MovimientoInventario> ObtenerMovimientosPorPeriodo(DateTime fechaInicio, DateTime fechaFin)
            => _context.Movimientos
                .Include(m => m.Producto)
                .Where(m => m.Fecha_Movimiento >= fechaInicio && m.Fecha_Movimiento <= fechaFin.AddDays(1))
                .ToList();

        public List<MovimientoInventario> ObtenerMovimientosPorAnio(int anio)
            => _context.Movimientos
                .Include(m => m.Producto)
                .Where(m => m.Fecha_Movimiento.HasValue && m.Fecha_Movimiento.Value.Year == anio)
                .ToList();

        public List<MovimientoInventario> ObtenerMovimientosPorProducto(int idProducto, DateTime? fechaInicio, DateTime? fechaFin, string? tipoFiltro)
        {
            var query = _context.Movimientos
                .Include(m => m.Usuario)
                .Where(m => m.Id_Producto == idProducto)
                .AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(m => m.Fecha_Movimiento >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(m => m.Fecha_Movimiento <= fechaFin.Value.AddDays(1));

            if (!string.IsNullOrEmpty(tipoFiltro) && tipoFiltro != "TODOS")
                query = query.Where(m => m.Tipo_Movimiento == tipoFiltro);

            return query.OrderByDescending(m => m.Fecha_Movimiento).ToList();
        }

        public List<ProductoStockBajoVM> ObtenerProductosStockBajo()
            => _context.Productos
                .Where(p => p.Estado && p.Stock_Actual <= p.Stock_Minimo)
                .Select(p => new ProductoStockBajoVM
                {
                    Id_Producto = p.Id_Producto,
                    Nombre = p.Nombre,
                    SKU = p.SKU,
                    StockActual = p.Stock_Actual ?? 0,
                    StockMinimo = p.Stock_Minimo
                })
                .OrderBy(p => p.StockActual)
                .ToList();

        public List<Producto> ObtenerTodosLosProductosActivos()
            => _context.Productos
                .Where(p => p.Estado)
                .OrderBy(p => p.Nombre)
                .ToList();

        public Producto? ObtenerProductoPorId(int idProducto)
            => _context.Productos.FirstOrDefault(p => p.Id_Producto == idProducto);
    }
}