using budget4home.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Threading.Tasks;

namespace budget4home.App
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadyController : Controller
    {
        private readonly IDistributedCache _cache;

        public ReadyController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
        public OkObjectResult Index()
        {
            return Ok("The service is ready");
        }

        [HttpGet("cache")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddCache(string key, string value)
        {
            return Ok(await _cache.GetOrCreateAsync(new CacheKey(key), () => {
                return Task.FromResult(value);
            }));
        }
    }
}
