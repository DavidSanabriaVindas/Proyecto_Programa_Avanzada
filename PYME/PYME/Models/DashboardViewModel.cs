namespace PYME.Models
{
    public class DashboardViewModel
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public long TotalUnidadesVendidas { get; set; }
        public decimal TotalIngresoVentas { get; set; }
        public int TotalMovimientosPeriodo { get; set; }

        public List<ProductoStockBajoVM> ProductosStockBajo { get; set; } = new();
        public ProductoMasVendidoVM? ProductoMasVendido { get; set; }
        public List<ResumenMensualVM> ResumenMensual { get; set; } = new();
        public List<ProductoMasVendidoVM> TopProductos { get; set; } = new();
        public List<VentaDiariaVM> VentasDiarias { get; set; } = new();
    }
}