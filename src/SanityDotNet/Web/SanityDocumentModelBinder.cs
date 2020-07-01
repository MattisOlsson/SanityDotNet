using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SanityDotNet.Models;

namespace SanityDotNet.Web
{
    public class SanityDocumentModelBinder : IModelBinder
    {
        private readonly IModelBinder _inner;

        public SanityDocumentModelBinder(IModelBinder inner)
        {
            _inner = inner;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value == ValueProviderResult.None)
            {
                return _inner.BindModelAsync(bindingContext);
            }

            if (bindingContext.ActionContext.RouteData.Values.TryGetValue(value.FirstValue, out object model) &&
                model is ISanityDocument)
            {
                bindingContext.Result = ModelBindingResult.Success(model);
                return Task.CompletedTask;
            }

            return _inner.BindModelAsync(bindingContext);
        }
    }
}