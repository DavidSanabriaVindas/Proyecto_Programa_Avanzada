using PYME.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PYME.Services
{
    public interface IMovimientoService
    {
        Task<List<MovimientoInventario>> ObtenerTodosAsync();
        Task<MovimientoInventario?> ObtenerDetalleAsync(int id);
        Task<List<MovimientoInventario>> ObtenerPorProductoAsync(int idProducto);
        Task<List<Producto>> ObtenerProductosAsync();
        Task<List<Usuario>> ObtenerUsuariosAsync();

        List<SelectListItem> ObtenerDescripcionesEntrada();
        List<SelectListItem> ObtenerDescripcionesSalida();

        (bool success, string mensaje) RegistrarEntrada(MovimientoInventario movimiento);
        (bool success, string mensaje) RegistrarSalida(MovimientoInventario movimiento);
    }
}