using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanityDotNet.Client;
using SanityDotNet.QueryApi.Extensions;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Sorting;
using SanityDotNet.SampleWeb.Models;
using SanityDotNet.SampleWeb.Models.ViewModels;
using SanityDotNet.Web;

namespace SanityDotNet.SampleWeb.Controllers
{
    public class StartPageController : Controller
    {
        private readonly ISanityClient _sanityClient;
        private readonly IUrlResolver _urlResolver;

        public StartPageController(ISanityClient sanityClient, IUrlResolver urlResolver)
        {
            _sanityClient = sanityClient;
            _urlResolver = urlResolver;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var response = await _sanityClient.Query<Category>()
                .OrderBy(
                    p => p.UpdatedAt.Descending(),
                    p => p.CreatedAt.Descending(),
                    p => p.Title.Ascending()
                )
                .GetQueryResponse();

            var tasks = response.Result.Select(CreateCategoryViewModel);
            var viewModels = await Task.WhenAll(tasks);
            return View(viewModels);
        }

        private async Task<ProductViewModel> CreateProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Product = product,
                Url = await _urlResolver.GetUrl(product),
            };
        }

        private async Task<CategoryViewModel> CreateCategoryViewModel(Category category)
        {
            return new CategoryViewModel
            {
                Category = category,
                Url = await _urlResolver.GetUrl(category),
            };
        }
    }
}