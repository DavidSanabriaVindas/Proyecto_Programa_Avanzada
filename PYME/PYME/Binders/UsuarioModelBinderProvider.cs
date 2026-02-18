using PYME.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PYME.Binders
{
    public class UsuarioModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(Usuario))
            {
                return new UsuarioModelBinder();
            }

            return null;
        }
    }
}
