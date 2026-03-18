using PYME.Models;
using PYME.Services;
using Microsoft.AspNetCore.Mvc;

namespace PYME.Controllers
{
    [Route("producto")]
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var productos = _productoService.ObtenerTodos();
            return View(productos);
        }

        [HttpGet("detalle/{id:int}")]
        public IActionResult Detalle(int id)
        {
            var producto = _productoService.ObtenerDetalle(id);
           
            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpGet("crear")]
        public IActionResult Crear()
        {
            return View(new Producto());
        }

        [HttpPost("crear")]
        public IActionResult Crear(Producto producto)
        {
            if (!ModelState.IsValid)
                return View(producto);

            if (!_productoService.CrearProducto(producto))
            {
                ModelState.AddModelError("SKU", "Ya existe un producto con este SKU");
                return View(producto);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id:int}")]
        public IActionResult Editar(int id)
        {
            var producto = _productoService.ObtenerDetalle(id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost("editar/{id:int}")]
        public IActionResult Editar(int id, Producto producto)
        {
            if (id != producto.Id_Producto)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(producto);

            if (!_productoService.ActualizarProducto(id, producto))
            {
                ModelState.AddModelError("SKU", "Ya existe un producto con este SKU");
                return View(producto);
            }

            return RedirectToAction("Index");
        }

        [HttpPost("eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            _productoService.EliminarProducto(id);
            return RedirectToAction("Index");
        }

        [HttpGet("buscar")]
        public IActionResult Buscar(string texto)
        {
            return Json(_productoService.Buscar(texto));
        }
    }
}