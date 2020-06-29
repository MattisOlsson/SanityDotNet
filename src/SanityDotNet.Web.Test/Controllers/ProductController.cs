using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanityDotNet.Mvc;
using SanityDotNet.Web.Test.Models;
using SanityDotNet.Web.Test.Models.ViewModels;

namespace SanityDotNet.Web.Test.Controllers
{
    public class ProductController : ContentController<Product>
    {
        private readonly IUrlResolver _urlResolver;

        public ProductController(IUrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Product currentDocument)
        {
            var model = new ProductViewModel
            {
                Product = currentDocument,
                Url = await _urlResolver.GetUrl(currentDocument)
            };

            return View(model);
        }

        public async Task<IActionResult> Test(Product currentDocument)
        {
            var model = new ProductViewModel
            {
                Product = currentDocument,
                Url = await _urlResolver.GetUrl(currentDocument)
            };

            return View("Index", model);
        }
    }
}