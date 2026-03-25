using PYME.Models;

namespace PYME.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerDetalleAsync(int id);
        Task<(bool success, string? error)> CrearUsuarioAsync(Usuario usuario, string password, string rol);
        Task<(bool success, string? error)> ActualizarUsuarioAsync(Usuario usuario, string rol, string? nuevaPassword);
        Task<bool> EliminarUsuarioAsync(int id);
        List<string> ObtenerRoles();
    }
}
