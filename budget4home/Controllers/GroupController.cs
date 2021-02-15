using System.Threading.Tasks;
using budget4home.Helpers;
using budget4home.Models;
using budget4home.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IValidateHelper _validateHelper;

        public GroupController(IGroupService groupService, IValidateHelper validateHelper)
        {
            _groupService = groupService;
            _validateHelper = validateHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long groupId)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var objs = await _groupService.GetAll(userId);
            return Ok(objs);
        }

        [HttpGet("full")]
        public async Task<IActionResult> GetAll(long groupId, int year, int month)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var objs = await _groupService.GetAllFullAsync(userId);
            return Ok(objs);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GroupModel obj)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var objs = await _groupService.AddAsync(userId, obj);
                return Ok(objs);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GroupModel obj)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var objs = await _groupService.UpdateAsync(userId, obj);
                return Ok(objs);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var objs = await _groupService.DeleteAsync(userId, id);
                return Ok(objs);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}