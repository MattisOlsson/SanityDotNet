using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanityDotNet.Client;
using SanityDotNet.Mvc;
using SanityDotNet.QueryApi.Extensions;
using SanityDotNet.QueryApi.Filters;
using SanityDotNet.SampleWeb.Models;
using SanityDotNet.SampleWeb.Models.ViewModels;

namespace SanityDotNet.SampleWeb.Controllers
{
    public class CategoryController : ContentController<Category>
    {
        private readonly ISanityClient _client;

        public CategoryController(ISanityClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index(Category currentDocument)
        {
            var productsResponse = await _client
                .Query<Product>(CultureInfo.CurrentUICulture)
                .Filter(p => p.MainCategory.Id.Is(currentDocument.Id))
                .OrFilter(p => p.Categories.MatchContained(currentDocument.Id))
                .GetQueryResponse();

            var model = new CategoryViewModel
            {
                Category = currentDocument,
                Products = productsResponse.Result
            };

            return View(model);
        }
    }
}