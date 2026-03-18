using System.ComponentModel.DataAnnotations;

namespace PYME.Models
{
    public class MovimientoInventarioViewModel
    {
        [Required]
        [Range(1, 100000, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        public string Tipo_Movimiento { get; set; }
    }
}
