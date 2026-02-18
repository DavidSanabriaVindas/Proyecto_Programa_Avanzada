using PYME.Data;
using PYME.Models;
using Microsoft.EntityFrameworkCore;

namespace PYME.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly AppDbContext _context;

        public RolRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Rol> ObtenerTodos()
            => _context.Roles
                .OrderBy(r => r.Nombre)
                .ToList();

        public List<Rol> ObtenerActivos()
            => _context.Roles
                .Where(r => r.Estado)
                .OrderBy(r => r.Nombre)
                .ToList();

        public Rol? ObtenerPorId(int id)
            => _context.Roles
                .Include(r => r.Usuarios)
                .FirstOrDefault(r => r.Id_Rol == id);

        public bool ExisteId(int id)
            => _context.Roles.Any(r => r.Id_Rol == id);
        public void Agregar(Rol rol)
        {
            _context.Roles.Add(rol);
            _context.SaveChanges();
        }
        public void Actualizar(Rol rol)
        {
            _context.Roles.Update(rol);
            _context.SaveChanges();
        }
        public void Eliminar(int id)
        {
            var rol = _context.Roles.Find(id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                _context.SaveChanges();
            }
        }
    }
}