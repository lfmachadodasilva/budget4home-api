using System;
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
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public ExpenseController(
            IExpenseService ExpenseService,
            IMapper mapper)
        {
            _expenseService = ExpenseService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ExpenseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long groupId, int year, int month)
        {
            var userId = UserHelper.GetUserId(HttpContext);
            var models = await _expenseService.GetAll(userId, groupId, year, month);
            var dtos = _mapper.Map<ICollection<ExpenseDto>>(models);
            return Ok(dtos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ExpenseManageDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] ExpenseManageDto dto)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var model = _mapper.Map<ExpenseModel>(dto);
                var obj = await _expenseService.AddAsync(userId, model);
                return Ok(_mapper.Map<ExpenseManageDto>(obj));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ExpenseManageDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] ExpenseManageDto dto)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var model = _mapper.Map<ExpenseModel>(dto);
                var obj = await _expenseService.UpdateAsync(userId, model);
                return Ok(_mapper.Map<ExpenseManageDto>(obj));
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
                var objs = await _expenseService.DeleteAsync(userId, id);
                return Ok(objs);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}