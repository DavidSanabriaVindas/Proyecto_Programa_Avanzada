using Microsoft.AspNetCore.Identity;
using PYME.Constants;
using PYME.Models;
using PYME.Repositories;

namespace PYME.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly UserManager<Usuario> _userManager;

        public UsuarioService(IUsuarioRepository repository, UserManager<Usuario> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            var usuarios = _repository.ObtenerTodos();

            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RolNombre = roles.FirstOrDefault();
            }

            return usuarios;
        }
        public async Task<Usuario?> ObtenerDetalleAsync(int id)
        {
            var user = _repository.ObtenerPorId(id);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RolNombre = roles.FirstOrDefault();
            }

            return user;
        }
        public async Task<(bool success, string? error)> CrearUsuarioAsync(
            Usuario usuario, string password, string rol)
        {
            var existe = await _userManager.FindByNameAsync(usuario.UserName);

            if (existe != null)
                return (false, "Ya existe un usuario con ese username");

            var result = await _userManager.CreateAsync(usuario, password);

            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(usuario, rol);

            return (true, null);
        }
        public async Task<(bool success, string? error)> ActualizarUsuarioAsync(
           Usuario usuario, string rol, string? nuevaPassword)
        {
            var existente = await _userManager.FindByIdAsync(usuario.Id.ToString());

            if (existente == null)
                return (false, "Usuario no encontrado");

            if (existente.UserName != usuario.UserName)
            {
                var existe = await _userManager.FindByNameAsync(usuario.UserName);
                if (existe != null)
                    return (false, "Ya existe ese username");
            }

            existente.UserName = usuario.UserName;
            existente.Email = usuario.Email;
            existente.Nombre = usuario.Nombre;
            existente.Primer_Apellido = usuario.Primer_Apellido;
            existente.Segundo_Apellido = usuario.Segundo_Apellido;
            existente.Direccion_Exacta = usuario.Direccion_Exacta;
            existente.Telefono = usuario.Telefono;
            existente.Estado = usuario.Estado;

            var result = await _userManager.UpdateAsync(existente);

            if (!result.Succeeded)
                return (false, "Error al actualizar usuario");

            var rolesActuales = await _userManager.GetRolesAsync(existente);

            if (!rolesActuales.Contains(rol))
            {
                await _userManager.RemoveFromRolesAsync(existente, rolesActuales);
                await _userManager.AddToRoleAsync(existente, rol);
            }

            if (!string.IsNullOrWhiteSpace(nuevaPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existente);
                var passResult = await _userManager.ResetPasswordAsync(existente, token, nuevaPassword);

                if (!passResult.Succeeded)
                    return (false, "Error al cambiar la contraseña");
            }

            return (true, null);
        }
        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var usuario = await _userManager.FindByIdAsync(id.ToString());

            if (usuario == null)
                return false;

            var result = await _userManager.DeleteAsync(usuario);

            return result.Succeeded;
        }

        public List<string> ObtenerRoles()
        {
            return new List<string>
        {
        Roles.Admin,
        Roles.Gerente,
        Roles.Vendedor,
        Roles.Almacenista
        };
        }
    }
}