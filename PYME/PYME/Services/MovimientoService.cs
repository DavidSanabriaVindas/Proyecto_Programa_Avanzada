using PYME.Models;
using PYME.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PYME.Services
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository _repository;
        private readonly IProductoRepository _productoRepository;

        public MovimientoService(IMovimientoRepository repository, IProductoRepository productoRepository)
        {
            _repository = repository;
            _productoRepository = productoRepository;
        }

        public List<MovimientoInventario> ObtenerTodos()
            => _repository.ObtenerTodos();

        public List<MovimientoInventario> ObtenerPorProducto(int idProducto)
            => _repository.ObtenerPorProducto(idProducto);

        public MovimientoInventario? ObtenerDetalle(int id)
            => _repository.ObtenerPorId(id);

        public List<SelectListItem> ObtenerDescripcionesEntrada() => new List<SelectListItem>
        {
            new("Compra a proveedor", "Compra a proveedor"),
            new("Otra entrada",       "Otra entrada")
        };

        public List<SelectListItem> ObtenerDescripcionesSalida() => new List<SelectListItem>
        {
            new("Venta",  "Venta"),
            new("Merma",  "Merma")
        };

        public (bool success, string mensaje) RegistrarEntrada(MovimientoInventario movimiento)
        {
            var producto = _productoRepository.ObtenerPorId(movimiento.Id_Producto);
            if (producto == null)
                return (false, "Producto no encontrado.");

            producto.Stock_Actual += movimiento.Cantidad;
            producto.Fecha_Actualizacion = DateTime.Now;
            _productoRepository.Actualizar(producto);

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

            movimiento.Tipo_Movimiento = "SALIDA";
            movimiento.Fecha_Movimiento = DateTime.Now;
            _repository.Agregar(movimiento);

            return (true, "Salida registrada.");
        }
    }
}