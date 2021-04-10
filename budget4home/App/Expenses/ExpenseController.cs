using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Expenses.Requests;
using budget4home.App.Expenses.Responses;
using budget4home.Helpers;
using budget4home.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.App.Expenses
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;
        private readonly ICache _cache;

        public ExpenseController(
            IExpenseService expenseService,
            IMapper mapper,
            ICache cache)
        {
            _expenseService = expenseService;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetExpenseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetExpensesRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            var result = await _cache.GetOrCreateAsync(
                new CacheKey(request.Group, "expense", request.Month, request.Year), 
                async () =>
                {
                    var models = await _expenseService.GetAllAsync(
                        userId,
                        request.Group,
                        request.Year,
                        request.Month);
                    var dtos = _mapper.Map<ICollection<GetExpenseResponse>>(models);
                    return dtos;
                });
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetExpenseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([ExpenseValidation] long id)
        {
            try
            {
                var obj = await _expenseService.GetByIdAsync(id);
                return Ok(_mapper.Map<GetExpenseResponse>(obj));
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("years")]
        [ProducesResponseType(typeof(GetExpenseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears()
        {
            var userId = UserHelper.GetUserId(HttpContext);

            var result = await _expenseService.GetYearsAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] AddExpenseRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var model = _mapper.Map<ExpenseModel>(request);
                var obj = await _expenseService.AddAsync(userId, model);

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
        public async Task<IActionResult> Put([FromBody] UpdateExpenseRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var model = _mapper.Map<ExpenseModel>(request);
                var obj = await _expenseService.UpdateAsync(userId, model);

                // clear group cache
                _cache.Delete(obj.GroupId.ToString());

                return Ok(obj.Id);
            }
            catch (DbException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([ExpenseValidation] long id, [FromQuery] bool includeSchedule = false)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var obj = await _expenseService.DeleteAsync(userId, id, includeSchedule);

                // TODO delete group cache
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