using System.ComponentModel.DataAnnotations;

namespace PYME.Models
{
    public class UsuarioViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string Primer_Apellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es obligatorio")]
        public string Segundo_Apellido { get; set; }

        [Required(ErrorMessage = "El username es obligatorio")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }

        public string? Direccion_Exacta { get; set; }

        public int? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        public int Id_Rol { get; set; }
    }
}