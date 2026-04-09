using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PYME.Models
{
    public class Venta
    {
        [Key]
        public int Id_Venta { get; set; }

        public DateTime? Fecha_Venta { get; set; } = DateTime.Now;

        public int Total { get; set; }

        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } = "Pendiente";

        [ForeignKey(nameof(Cliente))]
        public int Id_Cliente { get; set; }
        [ValidateNever]
        public Cliente? Cliente { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int Id_Usuario { get; set; }
        [ValidateNever]
        public Usuario? Usuario { get; set; }

        public List<Detalle_Venta> Detalles { get; set; } = new();
    }
}