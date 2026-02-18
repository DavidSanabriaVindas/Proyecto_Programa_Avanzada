using PYME.Models;

namespace PYME.Services
{
    public interface IUsuariosService
    {
        List<Usuario> ObtenerTodos(); 
        Usuario? ObtenerDetalle(int id);

        bool CrearUsuario(Usuario usuario);
        bool ActualizarUsuario(Usuario usuario);
        bool EliminarUsuario(int id);

    }
}
