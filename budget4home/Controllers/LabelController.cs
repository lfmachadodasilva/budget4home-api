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
        [ProducesResponseType(typeof(ICollection<LabelDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long groupId)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var models = await _labelService.GetAllAsync(userId, groupId);
            return Ok(_mapper.Map<List<LabelModel>, ICollection<LabelDto>>(models));
        }

        [HttpGet("/api/full/[controller]")]
        [ProducesResponseType(typeof(ICollection<LabelFullDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(long groupId, int year, int month)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var models = await _labelService.GetAllFullAsync(userId, groupId, year, month);
            return Ok(_mapper.Map<ICollection<LabelFullDto>>(models));
        }

        [HttpPost]
        [ProducesResponseType(typeof(LabelManageDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] LabelManageDto dto)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var models = _mapper.Map<LabelModel>(dto);
                var obj = await _labelService.AddAsync(userId, models);
                return Ok(_mapper.Map<LabelManageDto>(obj));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(LabelManageDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] LabelManageDto dto)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var models = _mapper.Map<LabelModel>(dto);
                var obj = await _labelService.UpdateAsync(userId, models);
                return Ok(_mapper.Map<LabelManageDto>(obj));
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
                var objs = await _labelService.DeleteAsync(userId, id);
                return Ok(objs);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}