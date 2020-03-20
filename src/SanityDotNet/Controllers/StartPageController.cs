using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SanityDotNet.Client;
using SanityDotNet.Extensions;
using SanityDotNet.Models;

namespace SanityDotNet.Controllers
{
    public class StartPageController : Controller
    {
        private readonly ISanityClient _sanityClient;

        public StartPageController(ISanityClient sanityClient)
        {
            _sanityClient = sanityClient;
        }

        public IActionResult Index()
        {
            var stringToMatch = "hej";
            var search = _sanityClient.Query<Product>()
                .Filter(m => m.Title.Is(stringToMatch))
                .Filter(m => m.Variant.Id.Is(1))
                .Filter(m => m.Title.In(new [] { "foo", "bar"}))
                .Filter(m => m.IsPublished.Is(true))
                .Filter(m => m.Variant.IsPrimary.Is(false))
                .OrFilter(m => m.IsNew.Is(true))
                .GetQueryResult();

            return Content("Hello world!");
        }
    }
}