using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Expenses.Requests;
using budget4home.App.Expenses.Responses;
using budget4home.App.Expenses.Validators;
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
        private readonly IGetExpensesValidator _getValidator;
        private readonly IGetByIdExpenseValidator _getByIdValidator;
        private readonly IAddExpenseValidator _addValidator;
        private readonly IUpdateExpenseValidator _updateValidator;
        private readonly IDeleteExpenseValidator _deleteValidator;
        private readonly IMapper _mapper;

        public ExpenseController(
            IExpenseService expenseService,
            IGetExpensesValidator getValidator,
            IGetByIdExpenseValidator getByIdValidator,
            IAddExpenseValidator addValidator,
            IUpdateExpenseValidator updateValidator,
            IDeleteExpenseValidator deleteValidator,
            IMapper mapper)
        {
            _expenseService = expenseService;
            _getValidator = getValidator;
            _getByIdValidator = getByIdValidator;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GetExpenseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery]GetExpensesRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _getValidator.ValidateAsync(userId, request);
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
        public async Task<IActionResult> GetById(long id)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _getByIdValidator.ValidateAsync(userId, id);
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

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] AddExpenseRequest request)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _addValidator.ValidateAsync(userId, request);
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
                await _updateValidator.ValidateAsync(userId, request);
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
        public async Task<IActionResult> Delete(long id, [FromQuery]bool includeSchedule = false)
        {
            var userId = UserHelper.GetUserId(HttpContext);

            try
            {
                await _deleteValidator.ValidateAsync(userId, id);
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