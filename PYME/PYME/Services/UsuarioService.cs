using PYME.Models;
using PYME.Repositories;

namespace PYME.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public List<Usuario> ObtenerTodos()
            => _repository.ObtenerTodos();

        public Usuario? ObtenerDetalle(int id)
            => _repository.ObtenerPorId(id);

        public bool CrearUsuario(Usuario usuario)
        {
            if (_repository.ExisteUsername(usuario.Username))
                return false;

            _repository.Agregar(usuario);
            return true;
        }

        public bool ActualizarUsuario(Usuario usuario)
        {
            var usuarioExistente = _repository.ObtenerPorId(usuario.Id_Usuario);
            if (usuarioExistente == null)
                return false;

            if (usuarioExistente.Username != usuario.Username
                && _repository.ExisteUsername(usuario.Username))
                return false;

            _repository.Actualizar(usuario);
            return true;
        }

        public bool EliminarUsuario(int id)
        {
            var usuario = _repository.ObtenerPorId(id);
            if (usuario == null)
                return false;

            _repository.Eliminar(id);
            return true;
        }
    }
}