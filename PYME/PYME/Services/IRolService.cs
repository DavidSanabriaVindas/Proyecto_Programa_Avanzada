using PYME.Models;

namespace PYME.Services
{
    public interface IRolService
    {
        List<Rol> ObtenerTodos();
        List<Rol> ObtenerActivos();
        Rol? ObtenerDetalle(int id);
        bool CrearRol(Rol rol);
        bool ActualizarRol(Rol rol);
        bool EliminarRol(int id);
    }
}