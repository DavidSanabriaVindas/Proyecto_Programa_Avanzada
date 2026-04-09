using PYME.Models;
using PYME.Repositories;

namespace PYME.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IProductoRepository _productoRepository;

        public VentaService(IVentaRepository ventaRepository, IProductoRepository productoRepository)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
        }

        public List<Venta> ObtenerTodos()
            => _ventaRepository.ObtenerTodos();

        public Venta? ObtenerDetalle(int id)
            => _ventaRepository.ObtenerPorId(id);

        public (bool success, string mensaje) CrearVenta(Venta venta, List<Detalle_Venta> detalles)
        {
            if (detalles == null || !detalles.Any())
                return (false, "Debe agregar al menos un producto.");

            foreach (var detalle in detalles)
            {
                var producto = _productoRepository.ObtenerPorId(detalle.Id_Producto);

                if (producto == null)
                    return (false, $"Producto con ID {detalle.Id_Producto} no encontrado.");

                if (!producto.Stock_Actual.HasValue || producto.Stock_Actual < detalle.Cantidad)
                    return (false, $"Stock insuficiente para '{producto.Nombre}'. " +
                                   $"Disponible: {producto.Stock_Actual ?? 0}, solicitado: {detalle.Cantidad}.");
            }

            int total = 0;
            foreach (var detalle in detalles)
            {
                var producto = _productoRepository.ObtenerPorId(detalle.Id_Producto)!;
                detalle.Precio_Unitario = producto.Precio_Venta;
                detalle.Subtotal = detalle.Precio_Unitario * detalle.Cantidad;
                total += detalle.Subtotal;
            }

            venta.Total = total;
            venta.Fecha_Venta = DateTime.Now;
            venta.Detalles = detalles;

            _ventaRepository.Agregar(venta);

            foreach (var detalle in detalles)
            {
                var producto = _productoRepository.ObtenerPorId(detalle.Id_Producto)!;
                producto.Stock_Actual -= detalle.Cantidad;
                producto.Fecha_Actualizacion = DateTime.Now;
                _productoRepository.Actualizar(producto);
            }

            return (true, "Venta registrada exitosamente.");
        }

        public (bool success, string mensaje) ActualizarEstado(int id, string nuevoEstado)
        {
            var venta = _ventaRepository.ObtenerPorId(id);

            if (venta == null)
                return (false, "Venta no encontrada.");

            venta.Estado = nuevoEstado;
            _ventaRepository.Actualizar(venta);

            return (true, "Estado actualizado.");
        }

        public bool EliminarVenta(int id)
        {
            var venta = _ventaRepository.ObtenerPorId(id);
            if (venta == null) return false;

            _ventaRepository.Eliminar(id);
            return true;
        }
    }
}