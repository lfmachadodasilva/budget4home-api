using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Groups.Requests;
using budget4home.App.Groups.Responses;
using budget4home.App.Groups.Validators;
using budget4home.Helpers;
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
        private readonly IAddGroupValidator _addValidator;
        private readonly IGetByIdGroupValidator _getByIdValidator;
        private readonly IUpdateGroupValidator _updateValidator;
        private readonly IDeleteGroupValidator _deleteValidator;
        private readonly IMapper _mapper;

        public GroupController(
            IGroupService groupService,
            IAddGroupValidator addValidator,
            IGetByIdGroupValidator getByIdValidator,
            IUpdateGroupValidator updateValidator,
            IDeleteGroupValidator deleteValidator,
            IMapper mapper)
        {
            _groupService = groupService;
            _addValidator = addValidator;
            _getByIdValidator = getByIdValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetGroupResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var objs = await _groupService.GetAll(userId);
            return Ok(_mapper.Map<ICollection<GetGroupResponse>>(objs));
        }

        [HttpGet("/api/full/[controller]")]
        [ProducesResponseType(typeof(ICollection<GetFullGroupResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var objs = await _groupService.GetAllFullAsync(userId);
            return Ok(_mapper.Map<ICollection<GetFullGroupResponse>>(objs));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdGroupResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(long id)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _getByIdValidator.ValidateAsync(userId, id);
                var obj = await _groupService.GetByIdAsync(id);
                return Ok(_mapper.Map<GetByIdGroupResponse>(obj));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] AddGroupRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _addValidator.ValidateAsync(userId, request);
                var model = _mapper.Map<GroupModel>(request);
                var obj = await _groupService.AddAsync(userId, model);
                return Ok(obj.Id);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] UpdateGroupRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _updateValidator.ValidateAsync(userId, request);
                var model = _mapper.Map<GroupModel>(request);
                var obj = await _groupService.UpdateAsync(userId, model);
                return Ok(obj.Id);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _deleteValidator.ValidateAsync(userId, id);
                await _groupService.DeleteAsync(userId, id);
                return Ok(id);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}