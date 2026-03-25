using PYME.Models;

namespace PYME.Services
{
    public interface IClienteService
    {
        List<Cliente> ObtenerTodos();
        Cliente? ObtenerDetalle(int id);
        bool CrearCliente(Cliente cliente);
        bool ActualizarCliente(Cliente cliente);
        bool EliminarCliente(int id);
    }
}