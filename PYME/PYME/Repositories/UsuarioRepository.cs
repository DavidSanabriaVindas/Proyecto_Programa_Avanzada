using PYME.Data;
using PYME.Models;
using Microsoft.EntityFrameworkCore;

namespace PYME.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context; 
        }
        public List<Usuario> ObtenerTodos()
            => _context.Usuarios
            .Include(u => u.Rol)
            .OrderBy(u => u.Nombre)
            .ToList();
        public Usuario? ObtenerPorId(int id)
            => _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefault(u => u.Id_Usuario == id);
        public Usuario? ObtenerPorUsername(string username)
            => _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefault(u => u.Username == username);
        public bool ExisteUsername(string username)
            => _context.Usuarios.Any(u => u.Username == username);
        public void Agregar(Usuario usuario)
            {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }
        public void Actualizar(Usuario usuario)
        {
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