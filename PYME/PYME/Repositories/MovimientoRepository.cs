using PYME.Data;
using PYME.Models;
using Microsoft.EntityFrameworkCore;

namespace PYME.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly AppDbContext _context;

        public MovimientoRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<MovimientoInventario> ObtenerTodos()
            => _context.Movimientos
                .Include(m => m.Producto)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.Fecha_Movimiento)
                .ToList();

        public List<MovimientoInventario> ObtenerPorProducto(int idProducto)
            => _context.Movimientos
                .Include(m => m.Producto)
                .Include(m => m.Usuario)
                .Where(m => m.Id_Producto == idProducto)
                .OrderByDescending(m => m.Fecha_Movimiento)
                .ToList();

        public MovimientoInventario? ObtenerPorId(int id)
            => _context.Movimientos
                .Include(m => m.Producto)
                .Include(m => m.Usuario)
                .FirstOrDefault(m => m.Id_Movimiento == id);

        public void Agregar(MovimientoInventario movimiento)
        {
            _context.Movimientos.Add(movimiento);
            _context.SaveChanges();
        }
    }

}
