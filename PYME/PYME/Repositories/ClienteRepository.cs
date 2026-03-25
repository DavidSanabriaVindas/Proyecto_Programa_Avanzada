using PYME.Data;
using PYME.Models;
using Microsoft.EntityFrameworkCore;

namespace PYME.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Cliente> ObtenerTodos()
            => _context.Clientes
                .OrderBy(c => c.Nombre)
                .ToList();

        public Cliente? ObtenerPorId(int id)
            => _context.Clientes
                .FirstOrDefault(c => c.Id_Cliente == id);

        public void Agregar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public void Actualizar(Cliente cliente)
        {
            var existingEntity = _context.Clientes.Local
                .FirstOrDefault(c => c.Id_Cliente == cliente.Id_Cliente);

            if (existingEntity != null)
                _context.Entry(existingEntity).State = EntityState.Detached;

            _context.Clientes.Update(cliente);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
        }
    }
}