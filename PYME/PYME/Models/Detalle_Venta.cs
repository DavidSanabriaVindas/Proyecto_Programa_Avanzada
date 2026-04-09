using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PYME.Models
{
    public class Detalle_Venta
    {
        [Key]
        public int Id_Detalle_Venta { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        public int Precio_Unitario { get; set; }

        public int Subtotal { get; set; }

        [ForeignKey(nameof(Venta))]
        public int Id_Venta { get; set; }
        [ValidateNever]
        public Venta? Venta { get; set; }

        [ForeignKey(nameof(Producto))]
        public int Id_Producto { get; set; }
        [ValidateNever]
        public Producto? Producto { get; set; }
    }
}
