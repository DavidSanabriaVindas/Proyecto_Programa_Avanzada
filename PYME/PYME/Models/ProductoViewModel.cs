using System.ComponentModel.DataAnnotations;

namespace PYME.Models
{
    public class ProductoViewModel
    {

        [Required(ErrorMessage = "El SKU es obligatorio.")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio costo es obligatorio.")]
        public int? Precio_Costo { get; set; }

        [Required(ErrorMessage = "El precio venta es obligatorio.")]
        public int? Precio_Venta { get; set; }

        [Required(ErrorMessage = "El stock actual es obligatorio.")]
        public int? Stock_Actual { get; set; }

        [Required(ErrorMessage = "El stock minimo es obligatorio.")]
        public int? Stock_Minimo { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Estado { get; set; } = true;

       
    }
}
