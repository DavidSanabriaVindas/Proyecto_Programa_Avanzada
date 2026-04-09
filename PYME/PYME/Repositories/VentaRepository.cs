using Microsoft.EntityFrameworkCore;
using PYME.Data;
using PYME.Models;

namespace PYME.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly AppDbContext _context;

        public VentaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Venta> ObtenerTodos()
            => _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.Fecha_Venta)
                .ToList();

        public Venta? ObtenerPorId(int id)
            => _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefault(v => v.Id_Venta == id);

        public void Agregar(Venta venta)
        {
            _context.Ventas.Add(venta);
            _context.SaveChanges();
        }

        public void Actualizar(Venta venta)
        {
            var existing = _context.Ventas.Local
                .FirstOrDefault(v => v.Id_Venta == venta.Id_Venta);

            if (existing != null)
                _context.Entry(existing).State = EntityState.Detached;

            _context.Ventas.Update(venta);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var venta = _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefault(v => v.Id_Venta == id);

            if (venta != null)
            {
                _context.Detalles_Venta.RemoveRange(venta.Detalles);
                _context.Ventas.Remove(venta);
                _context.SaveChanges();
            }
        }
    }
}