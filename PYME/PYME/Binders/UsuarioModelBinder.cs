using PYME.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PYME.Binders
{
    public class UsuarioModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = bindingContext.HttpContext.Request;

            var nombre = request.Form["Nombre"].ToString();
            var primerApellido = request.Form["Primer_Apellido"].ToString();
            var segundoApellido = request.Form["Segundo_Apellido"].ToString();
            var password = request.Form["Password"].ToString();
            var direccionExacta = request.Form["Direccion_Exacta"].ToString();
            var telefonoTexto = request.Form["Telefono"].ToString();
            var correo = request.Form["Correo"].ToString();
            var idRolTexto = request.Form["Id_Rol"].ToString();
            var idUsuarioTexto = request.Form["Id_Usuario"].ToString();

            bool estado = request.Form["Estado"].Contains("true");

            int.TryParse(telefonoTexto, out int telefono);
            int.TryParse(idRolTexto, out int idRol);
            int.TryParse(idUsuarioTexto, out int idUsuario);

            string usernameGenerado;

            if (!string.IsNullOrWhiteSpace(nombre) &&
                !string.IsNullOrWhiteSpace(primerApellido))
            {
                usernameGenerado = $"{nombre}.{primerApellido}"
                    .Replace(" ", "")
                    .ToLower();
            }
            else
            {
                usernameGenerado = Guid.NewGuid().ToString("N");
            }

            var usuario = new Usuario
            {
                Id_Usuario = idUsuario,
                Nombre = nombre,
                Primer_Apellido = primerApellido,
                Segundo_Apellido = segundoApellido,
                Username = usernameGenerado,
                Password = password,
                Estado = estado,
                Direccion_Exacta = string.IsNullOrWhiteSpace(direccionExacta) ? null : direccionExacta,
                Telefono = telefono == 0 ? null : telefono,
                Correo = string.IsNullOrWhiteSpace(correo) ? null : correo,
                Id_Rol = idRol
            };

            bindingContext.Result = ModelBindingResult.Success(usuario);
            return Task.CompletedTask;
        }
    }
}
