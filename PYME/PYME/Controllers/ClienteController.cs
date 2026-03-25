using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PYME.Constants;
using PYME.Models;
using PYME.Services;

namespace PYME.Controllers
{
    [Route("cliente")]
    [Authorize(Roles = Roles.Admin)]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var clientes = _clienteService.ObtenerTodos();
            return View(clientes);
        }

        [HttpGet("detalle/{id:int}")]
        public IActionResult Detalle(int id)
        {
            var cliente = _clienteService.ObtenerDetalle(id);
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        [HttpGet("crear")]
        public IActionResult Crear()
        {
            return View(new Cliente());
        }

        [HttpPost("crear")]
        public IActionResult Crear(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            _clienteService.CrearCliente(cliente);
            return RedirectToAction("Index");
        }

        [HttpGet("editar/{id:int}")]
        public IActionResult Editar(int id)
        {
            var cliente = _clienteService.ObtenerDetalle(id);
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        [HttpPost("editar/{id:int}")]
        public IActionResult Editar(int id, Cliente cliente)
        {
            if (id != cliente.Id_Cliente)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(cliente);

            _clienteService.ActualizarCliente(cliente);
            return RedirectToAction("Index");
        }

        [HttpPost("eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            _clienteService.EliminarCliente(id);
            return RedirectToAction("Index");
        }
    }
}