using PYME.Models;

namespace PYME.Services
{
    public interface ICuentaService
    {
        Task<(bool Succeeded, string? ErrorMessage)> LoginAsync(string correo, string password, bool rememberMe);
        Task LogoutAsync();
    }
}
