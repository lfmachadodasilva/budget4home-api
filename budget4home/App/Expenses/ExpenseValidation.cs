using budget4home.App.Groups;
using budget4home.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Expenses
{
    public class ExpenseValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var userId = UserHelper.GetUserId(httpContextAccessor.HttpContext);

            var expenseId = (long)value;
            var expenseRepository = (IExpenseRepository)validationContext.GetService(typeof(IExpenseRepository));

            var getByIdTask = expenseRepository.GetByIdAsync(expenseId);
            getByIdTask.Wait();

            var expense = getByIdTask.Result;
            if (expense == null)
            {
                // expense does not exist
                return new ValidationResult("INVALID_EXPENSE");
            }

            var groupId = expense.GroupId;

            var groupValidation = new GroupValidationAttribute();
            try
            {
                groupValidation.Validate(groupId, validationContext);
            }
            catch (ValidationException ex)
            {
                // invalid group
                return ex.ValidationResult;
            }

            return ValidationResult.Success;
        }
    }
}
