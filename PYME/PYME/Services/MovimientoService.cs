using PYME.Models;
using PYME.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PYME.Services
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository _repository;
        private readonly IProductoRepository _productoRepository;
        private readonly IUsuarioService _usuarioService;

        public MovimientoService(
            IMovimientoRepository repository,
            IProductoRepository productoRepository,
            IUsuarioService usuarioService)
        {
            _repository = repository;
            _productoRepository = productoRepository;
            _usuarioService = usuarioService;
        }

        public Task<List<MovimientoInventario>> ObtenerTodosAsync()
            => Task.FromResult(_repository.ObtenerTodos());

        public Task<MovimientoInventario?> ObtenerDetalleAsync(int id)
            => Task.FromResult(_repository.ObtenerPorId(id));

        public Task<List<MovimientoInventario>> ObtenerPorProductoAsync(int idProducto)
            => Task.FromResult(_repository.ObtenerPorProducto(idProducto));

        public Task<List<Producto>> ObtenerProductosAsync()
            => Task.FromResult(_productoRepository.ObtenerTodos());

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
            => await _usuarioService.ObtenerTodosAsync();

        public List<SelectListItem> ObtenerDescripcionesEntrada() => new List<SelectListItem>
        {
            new("Compra a proveedor", "Compra a proveedor"),
            new("Otra entrada",       "Otra entrada")
        };

        public List<SelectListItem> ObtenerDescripcionesSalida() => new List<SelectListItem>
        {
            new("Venta", "Venta"),
            new("Merma", "Merma")
        };

        public (bool success, string mensaje) RegistrarEntrada(MovimientoInventario movimiento)
        {
            var producto = _productoRepository.ObtenerPorId(movimiento.Id_Producto);
            if (producto == null)
                return (false, "Producto no encontrado.");

            producto.Stock_Actual += movimiento.Cantidad;
            producto.Fecha_Actualizacion = DateTime.Now;
            _productoRepository.Actualizar(producto);

            movimiento.Producto = null;
            movimiento.Usuario = null;
            movimiento.Tipo_Movimiento = "ENTRADA";
            movimiento.Fecha_Movimiento = DateTime.Now;
            _repository.Agregar(movimiento);

            return (true, "Entrada registrada.");
        }

        public (bool success, string mensaje) RegistrarSalida(MovimientoInventario movimiento)
        {
            var producto = _productoRepository.ObtenerPorId(movimiento.Id_Producto);
            if (producto == null)
                return (false, "Producto no encontrado.");

            if (producto.Stock_Actual < movimiento.Cantidad)
                return (false, "Stock insuficiente.");

            producto.Stock_Actual -= movimiento.Cantidad;
            producto.Fecha_Actualizacion = DateTime.Now;
            _productoRepository.Actualizar(producto);

            movimiento.Producto = null;
            movimiento.Usuario = null;
            movimiento.Tipo_Movimiento = "SALIDA";
            movimiento.Fecha_Movimiento = DateTime.Now;
            _repository.Agregar(movimiento);

            return (true, "Salida registrada.");
        }
    }
}