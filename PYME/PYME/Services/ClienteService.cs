using PYME.Models;
using PYME.Repositories;

namespace PYME.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public List<Cliente> ObtenerTodos()
            => _repository.ObtenerTodos();

        public Cliente? ObtenerDetalle(int id)
            => _repository.ObtenerPorId(id);

        public bool CrearCliente(Cliente cliente)
        {
            _repository.Agregar(cliente);
            return true;
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            var existente = _repository.ObtenerPorId(cliente.Id_Cliente);
            if (existente == null)
                return false;

            _repository.Actualizar(cliente);
            return true;
        }

        public bool EliminarCliente(int id)
        {
            var cliente = _repository.ObtenerPorId(id);
            if (cliente == null)
                return false;

            _repository.Eliminar(id);
            return true;
        }
    }
}