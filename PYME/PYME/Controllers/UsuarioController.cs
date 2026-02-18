using PYME.Models;
using PYME.Services;
using Microsoft.AspNetCore.Mvc;

namespace PYME.Controllers
{
    [Route("usuario")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;

        public UsuarioController(
            IUsuarioService usuarioService,
            IRolService rolService)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var usuarios = _usuarioService.ObtenerTodos();
            return View(usuarios);
        }

        [HttpGet("detalle/{id:int}")]
        public IActionResult Detalle(int id)
        {
            var usuario = _usuarioService.ObtenerDetalle(id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpGet("crear")]
        public IActionResult Crear()
        {
            ViewBag.Roles = _rolService.ObtenerActivos();
            return View(new Usuario());
        }

        [HttpPost("crear")]
        public IActionResult Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _rolService.ObtenerActivos();
                return View(usuario);
            }

            if (!_usuarioService.CrearUsuario(usuario))
            {
                ModelState.AddModelError("Username", "Ya existe un usuario con este username");
                ViewBag.Roles = _rolService.ObtenerActivos();
                return View(usuario);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id:int}")]
        public IActionResult Editar(int id)
        {
            var usuario = _usuarioService.ObtenerDetalle(id);

            if (usuario == null)
                return NotFound();

            ViewBag.Roles = _rolService.ObtenerActivos();
            return View(usuario);
        }

        [HttpPost("editar/{id:int}")]
        public IActionResult Editar(int id, Usuario usuario)
        {
            if (id != usuario.Id_Usuario)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _rolService.ObtenerActivos();
                return View(usuario);
            }

            if (!_usuarioService.ActualizarUsuario(usuario))
            {
                ModelState.AddModelError("Username", "Ya existe un usuario con este username");
                ViewBag.Roles = _rolService.ObtenerActivos();
                return View(usuario);
            }

            return RedirectToAction("Index");
        }

        [HttpPost("eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            _usuarioService.EliminarUsuario(id);
            return RedirectToAction("Index");
        }
    }
}