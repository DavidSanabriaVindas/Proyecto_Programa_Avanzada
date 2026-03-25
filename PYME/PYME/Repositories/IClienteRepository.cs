using PYME.Models;

namespace PYME.Repositories
{
    public interface IClienteRepository
    {
        List<Cliente> ObtenerTodos();
        Cliente? ObtenerPorId(int id);
        void Agregar(Cliente cliente);
        void Actualizar(Cliente cliente);
        void Eliminar(int id);
    }
}