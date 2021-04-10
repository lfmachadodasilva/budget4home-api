using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Expenses.Requests;
using budget4home.App.Expenses.Responses;
using budget4home.Helpers;
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

        public ExpenseController(
            IExpenseService expenseService,
            IMapper mapper)
        {
            _expenseService = expenseService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetExpenseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetExpensesRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var models = await _expenseService.GetAllAsync(
                    userId,
                    request.Group,
                    request.Year,
                    request.Month);
                var dtos = _mapper.Map<ICollection<GetExpenseResponse>>(models);
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
        [ProducesResponseType(typeof(GetExpenseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([ExpenseValidation] long id)
        {
            try
            {
                var obj = await _expenseService.GetByIdAsync(id);
                return Ok(_mapper.Map<GetExpenseResponse>(obj));
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

        [HttpGet("years")]
        [ProducesResponseType(typeof(GetExpenseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears()
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var result = await _expenseService.GetYearsAsync(userId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
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
        public async Task<IActionResult> Put([FromBody] UpdateExpenseRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                var model = _mapper.Map<ExpenseModel>(request);
                var obj = await _expenseService.UpdateAsync(userId, model);
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
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([ExpenseValidation] long id, [FromQuery] bool includeSchedule = false)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _expenseService.DeleteAsync(userId, id, includeSchedule);
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