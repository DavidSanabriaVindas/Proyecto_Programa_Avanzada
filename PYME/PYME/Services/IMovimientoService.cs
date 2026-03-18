using PYME.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PYME.Services
{
    public interface IMovimientoService
    {
        List<MovimientoInventario> ObtenerTodos();
        List<MovimientoInventario> ObtenerPorProducto(int idProducto);
        MovimientoInventario? ObtenerDetalle(int id);

        List<SelectListItem> ObtenerDescripcionesEntrada();
        List<SelectListItem> ObtenerDescripcionesSalida();

        (bool success, string mensaje) RegistrarEntrada(MovimientoInventario movimiento);
        (bool success, string mensaje) RegistrarSalida(MovimientoInventario movimiento);
    }
}