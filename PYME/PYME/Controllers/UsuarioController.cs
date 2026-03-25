using PYME.Models;
using PYME.Services;
using Microsoft.AspNetCore.Mvc;
namespace PYME.Controllers
{
    [Route("usuario")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
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
            return View(new Usuario());
        }
        [HttpPost("crear")]
        public IActionResult Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);
            if (!_usuarioService.CrearUsuario(usuario))
            {
                ModelState.AddModelError("UserName", "Ya existe un usuario con este username");
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
            return View(usuario);
        }
        [HttpPost("editar/{id:int}")]
        public IActionResult Editar(int id, Usuario usuario)
        {
            if (id != usuario.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(usuario);
            if (!_usuarioService.ActualizarUsuario(usuario))
            {
                ModelState.AddModelError("UserName", "Ya existe un usuario con este username");
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