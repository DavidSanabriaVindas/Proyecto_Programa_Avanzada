using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PYME.Models
{
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        public string Primer_Apellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        public string Segundo_Apellido { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Estado { get; set; } = true;
        public string? Direccion_Exacta { get; set; }

        public int? Telefono { get; set; }

        public string? Correo { get; set; }

        // FK con Rol
        [ForeignKey(nameof(Rol))]
        public int Id_Rol { get; set; }
        public Rol Rol { get; set; }
    }
}
