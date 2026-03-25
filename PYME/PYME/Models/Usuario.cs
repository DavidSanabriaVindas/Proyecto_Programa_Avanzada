using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PYME.Models
{
    public class Usuario : IdentityUser<int>
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Nombre { get; set; } 

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        public string Primer_Apellido { get; set; } 

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        public string Segundo_Apellido { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Estado { get; set; } = true;

        public string? Direccion_Exacta { get; set; }

        public int? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string? Correo { get; set; }

        public List<MovimientoInventario> MovimientosInvetario { get; set; } = new();

    }
}
