using Microsoft.AspNetCore.Mvc;
using SanityDotNet.Models;

namespace SanityDotNet.Mvc
{
    public abstract class ContentController<T> : Controller where T : ISanityDocument
    {
    }
}