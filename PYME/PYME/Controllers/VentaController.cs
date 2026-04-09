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

        // GET /venta
        [HttpGet("")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente},{Roles.Vendedor}")]
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

        // GET /venta/detalle/5
        [HttpGet("detalle/{id:int}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente},{Roles.Vendedor}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var venta = _ventaService.ObtenerDetalle(id);

            if (venta == null)
                return NotFound();

            // Vendedor solo puede ver sus propias ventas
            if (User.IsInRole(Roles.Vendedor))
            {
                var usuario = await _userManager.GetUserAsync(User);
                if (venta.Id_Usuario != usuario!.Id)
                    return Forbid();
            }

            return View(venta);
        }

        // GET /venta/crear
        [HttpGet("crear")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente},{Roles.Vendedor}")]
        public IActionResult Crear()
        {
            ViewBag.Clientes = _clienteService.ObtenerTodos();
            ViewBag.Productos = _productoService.ObtenerTodos()
                .Where(p => p.Estado && p.Stock_Actual > 0)
                .ToList();
            return View(new Venta());
        }

        // POST /venta/crear
        [HttpPost("crear")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente},{Roles.Vendedor}")]
        public async Task<IActionResult> Crear(
            Venta venta,
            List<int> ProductoIds,
            List<int> Cantidades)
        {
            var usuario = await _userManager.GetUserAsync(User);
            venta.Id_Usuario = usuario!.Id;

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
                return View(venta);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        // GET /venta/cambiar-estado/5
        [HttpGet("cambiar-estado/{id:int}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
        public IActionResult CambiarEstado(int id)
        {
            var venta = _ventaService.ObtenerDetalle(id);
            if (venta == null)
                return NotFound();

            return View(venta);
        }

        // POST /venta/cambiar-estado/5
        [HttpPost("cambiar-estado/{id:int}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
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

        // POST /venta/eliminar/5
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