using System;
using System.Threading.Tasks;
using budget4home.App.Expenses.Requests;
using budget4home.App.Groups.Validators;
using budget4home.App.Labels.Validators;

namespace budget4home.App.Expenses.Validators
{
    public interface IUpdateExpenseValidator
    {
        Task<bool> ValidateAsync(string userId, UpdateExpenseRequest request);
    }

    public class UpdateExpenseValidator : IUpdateExpenseValidator
    {
        private readonly IExpenseValidator _expenseValidator;
        private readonly IGroupValidator _groupValidator;
        private readonly ILabelValidator _labelValidator;

        public UpdateExpenseValidator(
            IExpenseValidator expenseValidator,
            IGroupValidator groupValidator,
            ILabelValidator labelValidator)
        {
            _expenseValidator = expenseValidator;
            _groupValidator = groupValidator;
            _labelValidator = labelValidator;
        }

        public async Task<bool> ValidateAsync(string userId, UpdateExpenseRequest request)
        {
            _expenseValidator.Validate(request.Name, request.Value);
            var expense = await _expenseValidator.ValidateAsync(request.Id);
            await _groupValidator.ValidateAsync(userId, expense.GroupId);
            await _labelValidator.ValidateAsync(request.LabelId, expense.GroupId);

            return true;
        }
    }
}