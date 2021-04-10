using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Groups;
using budget4home.App.Labels.Requests;
using budget4home.App.Labels.Responses;
using budget4home.Helpers;
using budget4home.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.App.Labels
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IMapper _mapper;
        private readonly ICache _cache;

        public LabelController(ILabelService labelService, IMapper mapper, ICache cache)
        {
            _labelService = labelService;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetLabelResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([GroupValidation]long groupId)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var result = await _cache.GetOrCreateAsync(new CacheKey(groupId), async () =>
            {
                var models = await _labelService.GetAllAsync(userId, groupId);
                return _mapper.Map<ICollection<GetLabelResponse>>(models);
            });
            return Ok(result);
        }

        [HttpGet("/api/full/[controller]")]
        [ProducesResponseType(typeof(ICollection<GetFullLabelResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetFullLabelsRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var result = await _cache.GetOrCreateAsync(
                new CacheKey(request.Group, "label", request.Month, request.Year),
                async () =>
                {
                    var models = await _labelService.GetAllFullAsync(
                        userId,
                        request.Group,
                        request.Year,
                        request.Month);
                    var dtos = _mapper.Map<ICollection<GetFullLabelResponse>>(models);
                    return dtos;
                }
            );
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetLabelResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([LabelValidation] long id)
        {
            try
            {
                var obj = await _labelService.GetByIdAsync(id);

                // clear group cache
                _cache.Delete(obj.GroupId.ToString());

                return Ok(_mapper.Map<GetLabelResponse>(obj));
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] AddLabelRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            try
            {
                var models = _mapper.Map<LabelModel>(request);
                var obj = await _labelService.AddAsync(userId, models);

                // clear group cache
                _cache.Delete(obj.GroupId.ToString());

                return Ok(obj.Id);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] UpdateLabelRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            try
            {
                var models = _mapper.Map<LabelModel>(request);
                var obj = await _labelService.UpdateAsync(userId, models);

                // clear group cache
                _cache.Delete(obj.GroupId.ToString());

                return Ok(obj.Id);
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

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([LabelValidation] long id)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            try
            {
                var obj = await _labelService.DeleteAsync(userId, id);

                // clear group cache
                _cache.Delete(obj.GroupId.ToString());

                return Ok(id);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}