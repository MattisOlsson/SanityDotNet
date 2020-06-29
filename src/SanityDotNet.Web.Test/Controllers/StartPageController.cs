using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanityDotNet.Client;
using SanityDotNet.Extensions;
using SanityDotNet.QueryApi.Extensions;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.QueryApi.Sorting;
using SanityDotNet.Web.Test.Models;
using SanityDotNet.Web.Test.Models.ViewModels;

namespace SanityDotNet.Web.Test.Controllers
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
            var response = await _sanityClient.Query<Product>(CultureInfo.GetCultureInfo("no"))
                .Filter(p => p.Title.IsDefined())
                .OrderBy(
                    p => p.UpdatedAt.Descending(),
                    p => p.CreatedAt.Descending(),
                    p => p.Title.Ascending()
                )
                .GetQueryResponse();

            var tasks = response.Result.Select(CreateProductViewModel);
            var viewModels = await Task.WhenAll(tasks);
            return View(viewModels);
        }

        private async Task<ProductViewModel> CreateProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Product = product,
                Url = await _urlResolver.GetUrl(product)
            };
        }
    }
}