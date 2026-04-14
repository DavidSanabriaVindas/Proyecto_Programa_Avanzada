using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PYME.Services;
using System.Text;

namespace PYME.Controllers
{
    [Route("dashboard")]
    [Authorize(Roles = "Admin,Gerente")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("")]
        public IActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var hoy = DateTime.Today;
            var inicio = fechaInicio ?? new DateTime(hoy.Year, hoy.Month, 1);
            var fin = fechaFin ?? hoy;

            var vm = _dashboardService.ObtenerDashboard(inicio, fin);
            return View(vm);
        }

        [HttpGet("kpis")]
        public IActionResult GetKpis(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var hoy = DateTime.Today;
            var inicio = fechaInicio ?? new DateTime(hoy.Year, hoy.Month, 1);
            var fin = fechaFin ?? hoy;

            var vm = _dashboardService.ObtenerDashboard(inicio, fin);

            return Json(new
            {
                totalUnidadesVendidas = vm.TotalUnidadesVendidas,
                totalIngresoVentas = vm.TotalIngresoVentas.ToString("N0"),
                totalMovimientos = vm.TotalMovimientosPeriodo,
                stockBajoCount = vm.ProductosStockBajo.Count,
                productoTopNombre = vm.ProductoMasVendido?.Nombre ?? "Sin datos",
                productoTopCantidad = vm.ProductoMasVendido?.CantidadVendida ?? 0
            });
        }

        [HttpGet("historial")]
        public IActionResult Historial(int? idProducto, DateTime? fechaInicio, DateTime? fechaFin, string? tipoFiltro)
        {
            var vm = _dashboardService.ObtenerHistorialProducto(
                idProducto ?? 0, fechaInicio, fechaFin, tipoFiltro);
            return View(vm);
        }

        [HttpGet("exportar-csv")]
        public IActionResult ExportarCsv(int idProducto, DateTime? fechaInicio, DateTime? fechaFin, string? tipoFiltro)
        {
            var vm = _dashboardService.ObtenerHistorialProducto(idProducto, fechaInicio, fechaFin, tipoFiltro);

            var sb = new StringBuilder();
            sb.AppendLine("Fecha,Tipo,Cantidad,Descripcion,Usuario");

            foreach (var m in vm.Movimientos)
            {
                sb.AppendLine($"{m.Fecha_Movimiento:yyyy-MM-dd HH:mm},{m.Tipo_Movimiento},{m.Cantidad},\"{m.Descripcion}\",{m.Usuario?.UserName}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", $"historial_{vm.Producto.SKU}_{DateTime.Today:yyyyMMdd}.csv");
        }
    }
}