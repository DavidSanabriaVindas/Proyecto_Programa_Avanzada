namespace PYME.Models
{
    public class ProductoMasVendidoVM
    {
        public string Nombre { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal IngresoGenerado { get; set; }
    }
}