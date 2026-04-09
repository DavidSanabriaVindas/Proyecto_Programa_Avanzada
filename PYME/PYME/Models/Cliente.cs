using System.ComponentModel.DataAnnotations;

namespace PYME.Models
{
    public class Cliente
    {
        [Key]
        public int Id_Cliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        public string Primer_Apellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        public string Segundo_Apellido { get; set; }

        public int? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string? Correo { get; set; }

        public string? Direccion_Exacta { get; set; }

        public List<Venta> Ventas { get; set; } = new();
    }
}