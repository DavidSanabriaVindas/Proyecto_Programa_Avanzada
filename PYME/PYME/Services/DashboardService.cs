using PYME.Models;
using PYME.Repositories;

namespace PYME.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public DashboardViewModel ObtenerDashboard(DateTime fechaInicio, DateTime fechaFin)
        {
            var movimientos = _repository.ObtenerMovimientosPorPeriodo(fechaInicio, fechaFin);

            var ventas = movimientos
                .Where(m => m.Tipo_Movimiento == "SALIDA" && m.Descripcion == "Venta")
                .ToList();

            var totalUnidades = ventas.Sum(m => (long)m.Cantidad);

            var totalIngreso = ventas
                .Sum(m => (decimal)m.Cantidad * (m.Producto?.Precio_Venta ?? 0));

            var stockBajo = _repository.ObtenerProductosStockBajo();

            var topProductos = ventas
                .GroupBy(m => m.Producto?.Nombre ?? "Desconocido")
                .Select(g => new ProductoMasVendidoVM
                {
                    Nombre = g.Key,
                    CantidadVendida = g.Sum(x => x.Cantidad),
                    IngresoGenerado = g.Sum(x => (decimal)x.Cantidad * (x.Producto?.Precio_Venta ?? 0))
                })
                .OrderByDescending(x => x.CantidadVendida)
                .Take(5)
                .ToList();

            var movimientosAnio = _repository.ObtenerMovimientosPorAnio(DateTime.Today.Year);

            var meses = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun",
                                 "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

            var resumenMensual = Enumerable.Range(1, 12).Select(mes => new ResumenMensualVM
            {
                NumeroMes = mes,
                Mes = meses[mes - 1],
                UnidadesVendidas = movimientosAnio
                    .Where(m => m.Fecha_Movimiento!.Value.Month == mes
                             && m.Tipo_Movimiento == "SALIDA"
                             && m.Descripcion == "Venta")
                    .Sum(m => (long)m.Cantidad),
                UnidadesIngresadas = movimientosAnio
                    .Where(m => m.Fecha_Movimiento!.Value.Month == mes
                             && m.Tipo_Movimiento == "ENTRADA")
                    .Sum(m => (long)m.Cantidad)
            }).ToList();

            var ventasDiarias = ventas
                .GroupBy(m => m.Fecha_Movimiento!.Value.Date)
                .Select(g => new VentaDiariaVM
                {
                    Fecha = g.Key.ToString("dd/MM"),
                    Unidades = g.Sum(x => x.Cantidad)
                })
                .OrderBy(x => x.Fecha)
                .ToList();

            return new DashboardViewModel
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                TotalUnidadesVendidas = totalUnidades,
                TotalIngresoVentas = totalIngreso,
                TotalMovimientosPeriodo = movimientos.Count,
                ProductosStockBajo = stockBajo,
                ProductoMasVendido = topProductos.FirstOrDefault(),
                ResumenMensual = resumenMensual,
                TopProductos = topProductos,
                VentasDiarias = ventasDiarias
            };
        }

        public HistorialProductoVM ObtenerHistorialProducto(int idProducto, DateTime? fechaInicio, DateTime? fechaFin, string? tipoFiltro)
        {
            var producto = _repository.ObtenerProductoPorId(idProducto);
            if (producto == null)
                return new HistorialProductoVM
                {
                    TodosLosProductos = _repository.ObtenerTodosLosProductosActivos()
                };

            var movimientos = _repository.ObtenerMovimientosPorProducto(idProducto, fechaInicio, fechaFin, tipoFiltro);

            var totalEntradas = movimientos
                .Where(m => m.Tipo_Movimiento == "ENTRADA")
                .Sum(m => m.Cantidad);

            var totalSalidas = movimientos
                .Where(m => m.Tipo_Movimiento == "SALIDA")
                .Sum(m => m.Cantidad);

            var totalVentas = movimientos
                .Where(m => m.Tipo_Movimiento == "SALIDA" && m.Descripcion == "Venta")
                .Sum(m => m.Cantidad);

            return new HistorialProductoVM
            {
                Producto = producto,
                Movimientos = movimientos,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                TipoFiltro = tipoFiltro,
                TodosLosProductos = _repository.ObtenerTodosLosProductosActivos(),

                TotalEntradas = totalEntradas,
                TotalSalidas = totalSalidas,
                TotalVentas = totalVentas
            };
        }
    }
}