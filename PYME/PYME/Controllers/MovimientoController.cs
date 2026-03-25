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
        private readonly IProductoService _productoService;
        private readonly IUsuarioService _usuarioService;

        public MovimientoController(
            IMovimientoService movimientoService,
            IProductoService productoService,
            IUsuarioService usuarioService)
        {
            _movimientoService = movimientoService;
            _productoService = productoService;
            _usuarioService = usuarioService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var movimientos = _movimientoService.ObtenerTodos();
            return View(movimientos);
        }

        [HttpGet("detalle/{id:int}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var movimiento = _movimientoService.ObtenerDetalle(id);
            if (movimiento == null)
                return NotFound();
            ViewBag.Productos = _productoService.ObtenerTodos();
            ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();

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
        public async Task<IActionResult> Entrada()
        {
            ViewBag.Productos = _productoService.ObtenerTodos();
            ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
            ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();

            return View(new MovimientoInventario());
        }

   
        [HttpPost("entrada")]
        public async Task<IActionResult> Entrada(MovimientoInventario movimiento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();
                return View(movimiento);
            }

            var (success, mensaje) = _movimientoService.RegistrarEntrada(movimiento);

            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesEntrada();
                return View(movimiento);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet("salida")]
        public async Task<IActionResult> Salida()
        {
            ViewBag.Productos = _productoService.ObtenerTodos();
            ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
            ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();

            return View(new MovimientoInventario());
        }

      
        [HttpPost("salida")]
        public async Task<IActionResult> Salida(MovimientoInventario movimiento)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();
                return View(movimiento);
            }

            var (success, mensaje) = _movimientoService.RegistrarSalida(movimiento);

            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
                ViewBag.Descripciones = _movimientoService.ObtenerDescripcionesSalida();
                return View(movimiento);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }
    }
}