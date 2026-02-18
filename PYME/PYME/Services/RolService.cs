using PYME.Models;
using PYME.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PYME.Services
{
    public class RolService: IRolService
    {
        private readonly IRolRepository _repository;

        public RolService(IRolRepository repository)
        {
            _repository = repository;
        }

        public List<Rol> ObtenerTodos()
            => _repository.ObtenerTodos();

        public Rol? ObtenerDetalle(int id)
            => _repository.ObtenerPorId(id);

        public bool CrearRol(Rol rol)
        {
            if (_repository.ExisteId(rol.Id))
                return false;

            _repository.Agregar(rol);
            return true;
        }

        public bool ActualizarRol(Rol rol)
        {
            if (!_repository.ExisteId(rol.Id))
                return false;

            _repository.Actualizar(rol);
            return true;
        }

        public bool EliminarRol(int id)
        {
            if (!_repository.ExisteId(id))
                return false;

            _repository.Eliminar(id);
            return true;
        }
    }
}
