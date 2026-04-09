using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PYME.Constants;
using PYME.Models;
using PYME.Services;
using System.Text.Json;

namespace PYME.Controllers
{
    [Route("venta")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Vendedor}")]
    public class VentaController : Controller
    {
        private readonly IVentaService _ventaService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly IUsuarioService _usuarioService;

        public VentaController(
            IVentaService ventaService,
            IClienteService clienteService,
            IProductoService productoService,
            IUsuarioService usuarioService)
        {
            _ventaService = ventaService;
            _clienteService = clienteService;
            _productoService = productoService;
            _usuarioService = usuarioService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var ventas = _ventaService.ObtenerTodos();
            return View(ventas);
        }

        [HttpGet("detalle/{id:int}")]
        public IActionResult Detalle(int id)
        {
            var venta = _ventaService.ObtenerDetalle(id);
            if (venta == null)
                return NotFound();
            return View(venta);
        }

        [HttpGet("crear")]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Clientes = _clienteService.ObtenerTodos();
            ViewBag.Productos = _productoService.ObtenerTodos();
            ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
            return View(new Venta());
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(Venta venta, string detallesJson)
        {
            List<Detalle_Venta>? detalles = null;

            try
            {
                detalles = JsonSerializer.Deserialize<List<Detalle_Venta>>(detallesJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                ModelState.AddModelError("", "Error al procesar los productos.");
            }

            if (detalles == null || !detalles.Any())
                ModelState.AddModelError("", "Debe agregar al menos un producto.");

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = _clienteService.ObtenerTodos();
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
                return View(venta);
            }

            var (success, mensaje) = _ventaService.CrearVenta(venta, detalles!);

            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Clientes = _clienteService.ObtenerTodos();
                ViewBag.Productos = _productoService.ObtenerTodos();
                ViewBag.Usuarios = await _usuarioService.ObtenerTodosAsync();
                return View(venta);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id:int}")]
        public IActionResult Editar(int id)
        {
            var venta = _ventaService.ObtenerDetalle(id);
            if (venta == null)
                return NotFound();

            ViewBag.Estados = new List<string> { "Pendiente", "Completada", "Cancelada" };
            return View(venta);
        }

        [HttpPost("editar/{id:int}")]
        public IActionResult Editar(int id, string estado)
        {
            var (success, mensaje) = _ventaService.ActualizarEstado(id, estado);

            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                var venta = _ventaService.ObtenerDetalle(id);
                ViewBag.Estados = new List<string> { "Pendiente", "Completada", "Cancelada" };
                return View(venta);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost("eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            _ventaService.EliminarVenta(id);
            return RedirectToAction("Index");
        }
    }
}