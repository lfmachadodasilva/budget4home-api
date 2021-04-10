using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Labels.Requests;
using budget4home.App.Labels.Responses;
using budget4home.Helpers;
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

        public LabelController(ILabelService labelService, IMapper mapper)
        {
            _labelService = labelService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetLabelResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long groupId)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var models = await _labelService.GetAllAsync(userId, groupId);
            return Ok(_mapper.Map<ICollection<GetLabelResponse>>(models));
        }

        [HttpGet("/api/full/[controller]")]
        [ProducesResponseType(typeof(ICollection<GetFullLabelResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetFullLabelsRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var models = await _labelService.GetAllFullAsync(
                    userId,
                    request.Group,
                    request.Year,
                    request.Month);
                var dtos = _mapper.Map<ICollection<GetFullLabelResponse>>(models);
                return Ok(dtos);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetLabelResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([LabelValidation] long id)
        {
            try
            {
                var obj = await _labelService.GetByIdAsync(id);
                return Ok(_mapper.Map<GetLabelResponse>(obj));
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
        public async Task<IActionResult> Post([FromBody] AddLabelRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var models = _mapper.Map<LabelModel>(request);
                var obj = await _labelService.AddAsync(userId, models);
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
        public async Task<IActionResult> Put([FromBody] UpdateLabelRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var models = _mapper.Map<LabelModel>(request);
                var obj = await _labelService.UpdateAsync(userId, models);
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

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([LabelValidation] long id)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _labelService.DeleteAsync(userId, id);
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