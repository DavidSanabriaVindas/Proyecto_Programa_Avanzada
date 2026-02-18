using System.ComponentModel.DataAnnotations;

namespace PYME.Models
{
    public class Rol
    {
        [Key]
        public int Id_Rol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Estado { get; set; } = true;

        public List<Usuario> Usuarios { get; set; } = new();
    }
}

