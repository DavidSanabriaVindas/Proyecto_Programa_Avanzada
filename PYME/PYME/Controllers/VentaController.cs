using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PYME.Constants;
using PYME.Models;
using PYME.Services;

namespace PYME.Controllers
{
    [Authorize]
    [Route("venta")]
    public class VentaController : Controller
    {
        private readonly IVentaService _ventaService;
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly UserManager<Usuario> _userManager;

        public VentaController(
            IVentaService ventaService,
            IClienteService clienteService,
            IProductoService productoService,
            UserManager<Usuario> userManager)
        {
            _ventaService = ventaService;
            _clienteService = clienteService;
            _productoService = productoService;
            _userManager = userManager;
        }

        [HttpGet("")]
        [Authorize(Roles = "Admin,Vendedor")]
        public async Task<IActionResult> Index()
        {
            var usuario = await _userManager.GetUserAsync(User);
            List<Venta> ventas;

            if (User.IsInRole(Roles.Admin) || User.IsInRole(Roles.Gerente))
            {
                ventas = _ventaService.ObtenerTodos();
            }
            else
            {
                ventas = _ventaService.ObtenerTodos()
                    .Where(v => v.Id_Usuario == usuario!.Id)
                    .ToList();
            }

            return View(ventas);
        }

        [HttpGet("detalle/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detalle(int id)
        {
            var venta = _ventaService.ObtenerDetalle(id);

            if (venta == null)
                return NotFound();

            if (User.IsInRole(Roles.Vendedor))
            {
                var usuario = await _userManager.GetUserAsync(User);
                if (venta.Id_Usuario != usuario!.Id)
                    return Forbid();
            }

            return View(venta);
        }

        [HttpGet("crear")]
        [Authorize(Roles = "Admin,Vendedor")]
        public IActionResult Crear()
        {
            ViewBag.Clientes = _clienteService.ObtenerTodos();
            ViewBag.Productos = _productoService.ObtenerTodos()
                .Where(p => p.Estado && p.Stock_Actual > 0)
                .ToList();
            return View(new Venta());
        }

        [HttpPost("crear")]
        [Authorize(Roles = "Admin,Vendedor")]
        public async Task<IActionResult> Crear(
    [FromForm] int Id_Cliente,
    [FromForm] string? Observaciones,
    List<int> ProductoIds,
    List<int> Cantidades)
        {
            var usuario = await _userManager.GetUserAsync(User);

            var venta = new Venta
            {
                Id_Cliente = Id_Cliente,
                Observaciones = Observaciones,
                Id_Usuario = usuario!.Id,
                Estado = "Pendiente"
            };

            var detalles = new List<Detalle_Venta>();
            for (int i = 0; i < ProductoIds.Count; i++)
            {
                if (Cantidades[i] > 0)
                {
                    detalles.Add(new Detalle_Venta
                    {
                        Id_Producto = ProductoIds[i],
                        Cantidad = Cantidades[i]
                    });
                }
            }

            var (success, mensaje) = _ventaService.CrearVenta(venta, detalles);

            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Clientes = _clienteService.ObtenerTodos();
                ViewBag.Productos = _productoService.ObtenerTodos()
                    .Where(p => p.Estado && p.Stock_Actual > 0)
                    .ToList();
                return View(new Venta());
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet("cambiar-estado/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult CambiarEstado(int id)
        {
            var venta = _ventaService.ObtenerDetalle(id);
            if (venta == null)
                return NotFound();

            return View(venta);
        }

        [HttpPost("cambiar-estado/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var (success, mensaje) = _ventaService.ActualizarEstado(id, nuevoEstado);

            if (!success)
            {
                TempData["Error"] = mensaje;
                return RedirectToAction("Detalle", new { id });
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost("eliminar/{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Eliminar(int id)
        {
            _ventaService.EliminarVenta(id);
            TempData["Mensaje"] = "Venta eliminada correctamente.";
            return RedirectToAction("Index");
        }
    }
}