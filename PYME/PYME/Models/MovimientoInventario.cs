using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PYME.Models
{
    public class MovimientoInventario
    {
        [Key]
        public int Id_Movimiento { get; set; }

        [ForeignKey(nameof(Producto))]
        public int Id_Producto { get; set; }

        [ValidateNever]
        public Producto? Producto { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int Id_Usuario { get; set; }

        [ValidateNever]
        public Usuario? Usuario { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        public DateTime? Fecha_Movimiento { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }

        [ValidateNever]
        public string Tipo_Movimiento { get; set; }
    }
}