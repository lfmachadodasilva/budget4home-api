using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Labels.Requests;
using budget4home.App.Labels.Responses;
using budget4home.App.Labels.Validators;
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
        private readonly IGetFullLabelsValidator _getFullValidator;
        private readonly IAddLabelValidator _addValidator;
        private readonly IUpdateLabelValidator _updateValidator;
        private readonly IDeleteLabelValidator _deleteValidator;
        private readonly IMapper _mapper;

        public LabelController(
            ILabelService labelService,
            IGetFullLabelsValidator getFullValidator,
            IAddLabelValidator addValidator,
            IUpdateLabelValidator updateValidator,
            IDeleteLabelValidator deleteValidator,
            IMapper mapper)
        {
            _labelService = labelService;
            _getFullValidator = getFullValidator;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetLabelsResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long groupId)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var models = await _labelService.GetAllAsync(userId, groupId);
            return Ok(_mapper.Map<ICollection<GetLabelsResponse>>(models));
        }

        [HttpGet("/api/full/[controller]")]
        [ProducesResponseType(typeof(ICollection<GetFullLabelsResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetFullLabelsRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _getFullValidator.ValidateAsync(userId, request);
                var models = await _labelService.GetAllFullAsync(
                    userId,
                    request.Group,
                    request.Year,
                    request.Month);
                var dtos = _mapper.Map<ICollection<GetFullLabelsResponse>>(models);
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

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] AddLabelRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _addValidator.ValidateAsync(userId, request);
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
                await _updateValidator.ValidateAsync(userId, request);
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

        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _deleteValidator.ValidateAsync(userId, id);
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