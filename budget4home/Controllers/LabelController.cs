using System.Threading.Tasks;
using budget4home.Models;
using budget4home.Services;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var labels = _labelService.GetAll();
            return Ok(labels);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LabelModel obj)
        {
            try
            {
                var objs = await _labelService.AddAsync(obj);
                return Ok(objs);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}