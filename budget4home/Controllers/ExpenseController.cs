using System.Threading.Tasks;
using budget4home.Helpers;
using budget4home.Models;
using budget4home.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budget4home.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IValidateHelper _validateHelper;

        public ExpenseController(IExpenseService ExpenseService, IValidateHelper validateHelper)
        {
            _expenseService = ExpenseService;
            _validateHelper = validateHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long groupId, int year, int month)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var objs = await _expenseService.GetAll(userId, groupId, year, month);
            return Ok(objs);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseModel obj)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var objs = await _expenseService.AddAsync(userId, obj);
                return Ok(objs);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ExpenseModel obj)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var objs = await _expenseService.UpdateAsync(userId, obj);
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