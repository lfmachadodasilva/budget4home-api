using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Groups.Requests;
using budget4home.App.Groups.Responses;
using budget4home.Helpers;
using budget4home.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.App.Groups
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        private readonly ICache _cache;

        public GroupController(
            IGroupService groupService,
            IMapper mapper,
            ICache cache)
        {
            _groupService = groupService;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetGroupResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var result = await _cache.GetOrCreateAsync(
                new CacheKey(userId, "group"),
                async () =>
                {
                    var objs = await _groupService.GetAll(userId);
                    return _mapper.Map<ICollection<GetGroupResponse>>(objs);
                }
            );
            return Ok(result);
        }

        [HttpGet("/api/full/[controller]")]
        [ProducesResponseType(typeof(ICollection<GetFullGroupResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var result = await _cache.GetOrCreateAsync(
                new CacheKey(userId, "group", "full"),
                async () =>
                {
                    var objs = await _groupService.GetAllFullAsync(userId);
                    return _mapper.Map<ICollection<GetFullGroupResponse>>(objs);
                }
            );
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdGroupResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([GroupValidation] long id)
        {
            try
            {
                var obj = await _groupService.GetByIdAsync(id);
                return Ok(_mapper.Map<GetByIdGroupResponse>(obj));
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] AddGroupRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            try
            {
                var model = _mapper.Map<GroupModel>(request);
                var obj = await _groupService.AddAsync(userId, model);

                // clear user cache
                _cache.Delete(userId);

                return Ok(obj.Id);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] UpdateGroupRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            try
            {
                var model = _mapper.Map<GroupModel>(request);
                var obj = await _groupService.UpdateAsync(userId, model);

                // clear user cache
                _cache.Delete(userId);

                return Ok(obj.Id);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([GroupValidation] long id)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            try
            {
                await _groupService.DeleteAsync(userId, id);

                // clear user cache
                _cache.Delete(userId);

                return Ok(id);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}