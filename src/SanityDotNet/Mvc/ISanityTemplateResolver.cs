using SanityDotNet.Models;
using SanityDotNet.Mvc.Models;

namespace SanityDotNet.Mvc
{
    public interface ISanityTemplateResolver
    {
        ContentControllerDescriptor Resolve(ISanityDocument document);
    }
}