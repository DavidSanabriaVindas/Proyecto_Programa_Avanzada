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
        private readonly UserManager<Usuario> _userManager;

        public VentaController(IVentaService ventaService, UserManager<Usuario> userManager)
        {
            _ventaService = ventaService;
            _userManager = userManager;
        }

        [HttpGet("")]
        [Authorize(Roles = "Admin,Vendedor")]
        public IActionResult Index()
        {
            var ventas = _ventaService.ObtenerTodos();
            return View(ventas);
        }

        [HttpGet("detalle/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Detalle(int id)
        {
            var venta = _ventaService.ObtenerDetalle(id);
            if (venta == null) return NotFound();
            return View(venta);
        }

        [HttpGet("crear")]
        [Authorize(Roles = "Admin,Vendedor")]
        public IActionResult Crear()
        {
            ViewBag.Clientes = _ventaService.ObtenerClientes();
            ViewBag.Productos = _ventaService.ObtenerProductosDisponibles();
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

            var detalles = ProductoIds
                .Select((idProducto, i) => new Detalle_Venta
                {
                    Id_Producto = idProducto,
                    Cantidad = Cantidades[i]
                })
                .Where(d => d.Cantidad > 0)
                .ToList();

            var venta = new Venta
            {
                Id_Cliente = Id_Cliente,
                Observaciones = Observaciones,
                Id_Usuario = usuario!.Id,
                Estado = "Pendiente"
            };

            var (success, mensaje) = _ventaService.CrearVenta(venta, detalles);

            if (!success)
            {
                ModelState.AddModelError("", mensaje);
                ViewBag.Clientes = _ventaService.ObtenerClientes();
                ViewBag.Productos = _ventaService.ObtenerProductosDisponibles();
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
            if (venta == null) return NotFound();
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