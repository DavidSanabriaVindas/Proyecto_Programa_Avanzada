using PYME.Models;

namespace PYME.Repositories
{
    public interface IUsuarioRepository
    {
        List<Usuario> ObtenerTodos();
        Usuario? ObtenerPorId(int id);
        Usuario? ObtenerPorUsername(string username);
        bool ExisteUsername(string username);
        void Agregar(Usuario usuario);
        void Actualizar(Usuario usuario);
        void Eliminar(int id);  
    }
}
