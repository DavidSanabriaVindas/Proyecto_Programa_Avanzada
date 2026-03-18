using PYME.Models;
using PYME.Services;
using Microsoft.AspNetCore.Mvc;

namespace PYME.Controllers
{
    [Route("movimiento")]
    public class MovimientoController : Controller
    {
        private readonly IMovimientoService _movimientoService;
        private readonly IProductoService _productoService;

        public MovimientoController(
            IMovimientoService movimientoService,
            IProductoService productoService)
        {
            _movimientoService = movimientoService;
            _productoService = productoService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var movimientos = _movimientoService.ObtenerTodos();
            return View(movimientos);
        }

        [HttpGet("detalle/{id:int}")]
        public IActionResult Detalle(int id)
        {
            var movimiento = _movimientoService.ObtenerDetalle(id);
            if (movimiento == null)
                return NotFound();
            return View(movimiento);
        }

        [HttpGet("porproducto/{idProducto:int}")]
        public IActionResult PorProducto(int idProducto)
        {
            var producto = _productoService.ObtenerDetalle(idProducto);
            if (producto == null)
                return NotFound();

            var movimientos = _movimientoService.ObtenerPorProducto(idProducto);
            ViewBag.Producto = producto;
            return View(movimientos);
        }

        [HttpGet("entrada")]
        public IActionResult Entrada()
        {
            ViewBag.Productos = _productoService.ObtenerTodos();
            ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();
            return View(new MovimientoInventario());
        }

        [HttpPost("entrada")]
        public IActionResult Entrada(MovimientoInventario movimiento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();
                return View(movimiento);
            }

            var (success, mensaje) = _movimientoService.RegistrarEntrada(movimiento);
            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();
                return View(movimiento);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet("salida")]
        public IActionResult Salida()
        {
            ViewBag.Productos = _productoService.ObtenerTodos();
            ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();
            return View(new MovimientoInventario());
        }

        [HttpPost("salida")]
        public IActionResult Salida(MovimientoInventario movimiento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();
                return View(movimiento);
            }

            var (success, mensaje) = _movimientoService.RegistrarSalida(movimiento);
            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();
                return View(movimiento);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }
    }
}