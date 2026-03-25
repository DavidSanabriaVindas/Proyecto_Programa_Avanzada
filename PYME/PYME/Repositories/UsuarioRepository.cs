using PYME.Data;
using PYME.Models;
using Microsoft.EntityFrameworkCore;
namespace PYME.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Usuario> ObtenerTodos()
            => _context.Usuarios
                .OrderBy(u => u.Nombre)
                .ToList();
        public Usuario? ObtenerPorId(int id)
            => _context.Usuarios
                .FirstOrDefault(u => u.Id == id);
        public Usuario? ObtenerPorUsername(string username)
            => _context.Usuarios
                .FirstOrDefault(u => u.UserName == username);
        public bool ExisteUsername(string username)
            => _context.Usuarios.Any(u => u.UserName == username);
        public void Agregar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }
        public void Actualizar(Usuario usuario)
        {
            var existingEntity = _context.Usuarios.Local
                .FirstOrDefault(u => u.Id == usuario.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }
        public void Eliminar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }
    }
}