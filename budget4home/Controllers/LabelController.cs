using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.Helpers;
using budget4home.Models;
using budget4home.Models.Dtos;
using budget4home.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IValidateHelper _validateHelper;
        private readonly IMapper _mapper;

        public LabelController(ILabelService labelService, IValidateHelper validateHelper, IMapper mapper)
        {
            _labelService = labelService;
            _validateHelper = validateHelper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long groupId)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var objs = await _labelService.GetAll(userId, groupId);
            return Ok(_mapper.Map<List<LabelModel>, ICollection<LabelDto>>(objs));
        }

        [HttpGet("full")]
        public async Task<IActionResult> GetAll(long groupId, int year, int month)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var objs = await _labelService.GetAllFullAsync(userId, groupId, year, month);
            return Ok(objs);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LabelModel obj)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var objs = await _labelService.AddAsync(userId, obj);
                return Ok(objs);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] LabelModel obj)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var objs = await _labelService.UpdateAsync(userId, obj);
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