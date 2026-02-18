using PYME.Models;

namespace PYME.Repositories
{
    public interface IRolRepository
    {
        List<Rol> ObtenerTodos();
        List<Rol> ObtenerActivos();
        Rol? ObtenerPorId(int id);
        void Agregar(Rol rol);
        void Actualizar(Rol rol);
        void Eliminar(int id);
        bool ExisteId(int id_Rol);
    }
}
