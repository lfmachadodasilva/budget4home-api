using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.Helpers;
using budget4home.Models;
using budget4home.Models.Dtos;
using budget4home.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(
            IGroupService groupService,
            IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var objs = await _groupService.GetAll(userId);
            return Ok(_mapper.Map<ICollection<GroupDto>>(objs));
        }

        [HttpGet("/api/full/[controller]")]
        [ProducesResponseType(typeof(ICollection<GroupFullDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var objs = await _groupService.GetAllFullAsync(userId);
            return Ok(_mapper.Map<ICollection<GroupFullDto>>(objs));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GroupManageDto dto)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var model = _mapper.Map<GroupModel>(dto);
                var obj = await _groupService.AddAsync(userId, model);
                return Ok(_mapper.Map<GroupManageDto>(obj));
            }
            catch (ForbidException)
            {
                return Forbid();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GroupManageDto dto)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var model = _mapper.Map<GroupModel>(dto);
                var obj = await _groupService.UpdateAsync(userId, model);
                return Ok(_mapper.Map<GroupManageDto>(obj));
            }
            catch (ForbidException)
            {
                return Forbid();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var objs = await _groupService.DeleteAsync(userId, id);
                return Ok(objs);
            }
            catch (ForbidException)
            {
                return Forbid();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}