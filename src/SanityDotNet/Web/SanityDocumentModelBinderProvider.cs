using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using SanityDotNet.Models;

namespace SanityDotNet.Web
{
    public class SanityDocumentModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (!typeof(ISanityDocument).IsAssignableFrom(context.Metadata.ModelType))
            {
                return null;
            }

            var complexTypeModelBinderProvider = context.Services.GetRequiredService<ComplexTypeModelBinderProvider>();
            var complexTypeModelBinder = complexTypeModelBinderProvider.GetBinder(context);
            return new SanityDocumentModelBinder(complexTypeModelBinder);
        }
    }
}