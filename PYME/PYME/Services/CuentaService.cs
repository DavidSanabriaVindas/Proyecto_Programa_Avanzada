using PYME.Constants;
using PYME.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PYME.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public CuentaService(
                UserManager<Usuario> userManager,
                SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool Succeeded, string? ErrorMessage)> LoginAsync(string correo, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(
                correo, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return (true, null);

            return (false, "Correo o contraseña incorrectos.");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}