using System.ComponentModel.DataAnnotations;

namespace PYME.Models
{
    public class Producto
    {
        [Key]
        public int Id_Producto { get; set; }

        [Required(ErrorMessage = "El SKU es obligatorio.")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio costo es obligatorio.")]
        public int Precio_Costo { get; set; }

        [Required(ErrorMessage = "El precio venta es obligatorio.")]
        public int Precio_Venta { get; set; }

        public int? Stock_Actual { get; set; }

        [Required(ErrorMessage = "El stock minimo es obligatorio.")]
        public int Stock_Minimo { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Estado { get; set; } = true;

        public DateTime? Fecha_Creacion { get; set; }

        public DateTime? Fecha_Actualizacion { get; set; }

        public List<MovimientoInventario> MovimientosInvetario { get; set; } = new();
        public List<Detalle_Venta> Detalles_Venta { get; set; } = new();
    }
}