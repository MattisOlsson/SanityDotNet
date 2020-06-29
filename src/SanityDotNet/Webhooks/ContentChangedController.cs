using Microsoft.AspNetCore.Mvc;
using SanityDotNet.Caching;
using SanityDotNet.Webhooks.Models;

namespace SanityDotNet.Webhooks
{
    public class ContentChangedController : ControllerBase
    {
        private readonly ISanityCacheHandler _cacheHandler;

        public ContentChangedController(ISanityCacheHandler cacheHandler)
        {
            _cacheHandler = cacheHandler;
        }

        [HttpPost]
        public IActionResult Index([FromBody] ContentChangedPayload payload)
        {
            _cacheHandler.Invalidate(payload.DocumentIds.All);
            return Ok();
        }
    }
}