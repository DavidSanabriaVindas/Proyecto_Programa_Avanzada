namespace PYME.Models
{
    public class ProductoStockBajoVM
    {
        public int Id_Producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
    }
}