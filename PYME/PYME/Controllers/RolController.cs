using PYME.Models;
using PYME.Services;
using Microsoft.AspNetCore.Mvc;

namespace PYME.Controllers
{
    [Route("rol")]
    public class RolController : Controller
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var roles = _rolService.ObtenerTodos();
            return View(roles);
        }

        [HttpGet("detalle/{id:int}")]
        public IActionResult Detalle(int id)
        {
            var rol = _rolService.ObtenerPorId(id);

            if (rol == null)
                return NotFound();

            return View(rol);
        }

        [HttpGet("crear")]
        public IActionResult Crear()
        {
            return View(new Rol());
        }

        [HttpPost("crear")]
        public IActionResult Crear(Rol rol)
        {
            if (!ModelState.IsValid)
                return View(rol);

            _rolService.CrearRol(rol);

            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id:int}")]
        public IActionResult Editar(int id)
        {
            var rol = _rolService.ObtenerPorId(id);

            if (rol == null)
                return NotFound();

            return View(rol);
        }

        [HttpPost("editar/{id:int}")]
        public IActionResult Editar(int id, Rol rol)
        {
            if (id != rol.Id_Rol)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(rol);

            _rolService.ActualizarRol(rol);

            return RedirectToAction("Index");
        }

        [HttpPost("eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            _rolService.EliminarRol(id);
            return RedirectToAction("Index");
        }
    }
}
