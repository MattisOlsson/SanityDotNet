using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanityDotNet.Mvc;
using SanityDotNet.SampleWeb.Models;
using SanityDotNet.SampleWeb.Models.ViewModels;
using SanityDotNet.Web;

namespace SanityDotNet.SampleWeb.Controllers
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