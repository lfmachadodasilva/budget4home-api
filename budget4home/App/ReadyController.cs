using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace budget4home.App
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadyController : Controller
    {
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
        public OkObjectResult Index()
        {
            return Ok("The service is ready");
        }
    }
}
