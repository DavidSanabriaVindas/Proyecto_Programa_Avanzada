using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PYME.Models
{
    public class Usuario : IdentityUser<int>
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Primer_Apellido { get; set; }

        [Required]
        public string Segundo_Apellido { get; set; }

        [Required]
        public bool Estado { get; set; } = true;

        public string? Direccion_Exacta { get; set; }

        public int? Telefono { get; set; }

        public int? Id_Rol { get; set; }
        public Rol? Rol { get; set; }

        public List<MovimientoInventario> MovimientosInvetario { get; set; } = new();
    }
}
