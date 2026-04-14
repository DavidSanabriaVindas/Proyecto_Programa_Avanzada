namespace PYME.Models
{
    public class HistorialProductoVM
    {
        public Producto Producto { get; set; } = new();
        public List<MovimientoInventario> Movimientos { get; set; } = new();
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? TipoFiltro { get; set; }
        public List<Producto> TodosLosProductos { get; set; } = new();
        public int TotalEntradas { get; set; }
        public int TotalSalidas { get; set; }
        public int TotalVentas { get; set; }
    }
}