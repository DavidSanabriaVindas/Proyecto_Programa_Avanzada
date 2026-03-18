using Microsoft.EntityFrameworkCore;
using PYME.Models;
using System.Reflection.Emit;

namespace PYME.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<MovimientoInventario> Movimientos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.Id_Rol)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(m => m.Producto)
                .WithMany(p => p.MovimientosInvetario)
                .HasForeignKey(m => m.Id_Producto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.MovimientosInvetario)
                .HasForeignKey(m => m.Id_Usuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}