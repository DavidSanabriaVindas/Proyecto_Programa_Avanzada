using PYME.Models;
using PYME.Services;
using PYME.Constants;
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
            ViewBag.Roles = _usuarioService.ObtenerRoles();
            return View(new Usuario());
        }


        [HttpPost("crear")]
        public async Task<IActionResult> Crear(Usuario usuario, string password, string rol)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _usuarioService.ObtenerRoles();
                return View(usuario);
            }

            var resultado = await _usuarioService.CrearUsuarioAsync(usuario, password, rol);

            if (!resultado.success)
            {
                ModelState.AddModelError("", resultado.error);
                ViewBag.Roles = _usuarioService.ObtenerRoles();
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

            ViewBag.Roles = _usuarioService.ObtenerRoles();

            return View(usuario);
        }

        [HttpPost("editar/{id:int}")]
        public async Task<IActionResult> Editar(int id, Usuario usuario, string rol, string? password)
        {
            if (id != usuario.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _usuarioService.ObtenerRoles();
                return View(usuario);
            }

            var resultado = await _usuarioService.ActualizarUsuarioAsync(usuario, rol, password);

            if (!resultado.success)
            {
                ModelState.AddModelError("", resultado.error);
                ViewBag.Roles = _usuarioService.ObtenerRoles();
                return View(usuario);
            }

            return RedirectToAction("Index");
        }

        [HttpPost("eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _usuarioService.EliminarUsuarioAsync(id);
            return RedirectToAction("Index");
        }
    }
}