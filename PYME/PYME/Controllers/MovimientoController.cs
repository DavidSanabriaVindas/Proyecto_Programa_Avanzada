using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PYME.Constants;
using PYME.Models;
using PYME.Services;

namespace PYME.Controllers
{
    [Route("movimiento")]
    [Authorize(Roles = "Admin,Almacenista")]
    public class MovimientoController : Controller
    {
        private readonly IMovimientoService _movimientoService;

        public MovimientoController(IMovimientoService movimientoService)
        {
            _movimientoService = movimientoService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var movimientos = await _movimientoService.ObtenerTodosAsync();
            return View(movimientos);
        }

        [HttpGet("detalle/{id:int}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var movimiento = await _movimientoService.ObtenerDetalleAsync(id);
            if (movimiento == null)
                return NotFound();

            ViewBag.Productos = await _movimientoService.ObtenerProductosAsync();
            ViewBag.Usuarios = await _movimientoService.ObtenerUsuariosAsync();

            return View(movimiento);
        }

        [HttpGet("porproducto/{idProducto:int}")]
        public async Task<IActionResult> PorProducto(int idProducto)
        {
            var movimientos = await _movimientoService.ObtenerPorProductoAsync(idProducto);
            if (movimientos == null)
                return NotFound();

            return View(movimientos);
        }

        [HttpGet("entrada")]
        public async Task<IActionResult> Entrada()
        {
            ViewBag.Productos = await _movimientoService.ObtenerProductosAsync();
            ViewBag.Usuarios = await _movimientoService.ObtenerUsuariosAsync();
            ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();

            return View(new MovimientoInventario());
        }

        [HttpPost("entrada")]
        public async Task<IActionResult> Entrada(MovimientoInventario movimiento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = await _movimientoService.ObtenerProductosAsync();
                ViewBag.Usuarios = await _movimientoService.ObtenerUsuariosAsync();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();
                return View(movimiento);
            }

            var (success, mensaje) = _movimientoService.RegistrarEntrada(movimiento);

            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Productos = await _movimientoService.ObtenerProductosAsync();
                ViewBag.Usuarios = await _movimientoService.ObtenerUsuariosAsync();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();
                return View(movimiento);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet("salida")]
        public async Task<IActionResult> Salida()
        {
            ViewBag.Productos = await _movimientoService.ObtenerProductosAsync();
            ViewBag.Usuarios = await _movimientoService.ObtenerUsuariosAsync();
            ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();

            return View(new MovimientoInventario());
        }

        [HttpPost("salida")]
        public async Task<IActionResult> Salida(MovimientoInventario movimiento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = await _movimientoService.ObtenerProductosAsync();
                ViewBag.Usuarios = await _movimientoService.ObtenerUsuariosAsync();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();
                return View(movimiento);
            }

            var (success, mensaje) = _movimientoService.RegistrarSalida(movimiento);

            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Productos = await _movimientoService.ObtenerProductosAsync();
                ViewBag.Usuarios = await _movimientoService.ObtenerUsuariosAsync();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();
                return View(movimiento);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }
    }
}