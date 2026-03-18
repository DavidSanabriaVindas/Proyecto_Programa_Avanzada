using PYME.Data;
using PYME.Models;
using Microsoft.EntityFrameworkCore;

namespace PYME.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Producto> ObtenerTodos()
            => _context.Productos
                .OrderBy(p => p.Nombre)
                .ToList();

        public Producto? ObtenerPorId(int id)
            => _context.Productos
                .FirstOrDefault(p => p.Id_Producto == id);

        public void Agregar(Producto producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();
        }

        public void Actualizar(Producto producto)
        {
            var existingEntity = _context.Productos.Local
                .FirstOrDefault(p => p.Id_Producto== producto.Id_Producto);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Productos.Update(producto);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
        }
    }
}
