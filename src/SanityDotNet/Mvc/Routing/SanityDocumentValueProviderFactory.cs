using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SanityDotNet.Mvc.Routing
{
    public class SanityDocumentValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            context.ValueProviders.Add(new SanityDocumentValueProvider());
            return Task.CompletedTask;
        }
    }
}