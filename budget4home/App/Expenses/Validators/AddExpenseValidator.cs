using System;
using System.Threading.Tasks;
using budget4home.App.Expenses.Requests;
using budget4home.App.Groups.Validators;
using budget4home.App.Labels.Validators;

namespace budget4home.App.Expenses.Validators
{
    public interface IAddExpenseValidator
    {
        Task<bool> ValidateAsync(string userId, AddExpenseRequest request);
    }

    public class AddExpenseValidator : IAddExpenseValidator
    {
        private readonly IExpenseValidator _expenseValidator;
        private readonly IGroupValidator _groupValidator;
        private readonly ILabelValidator _labelValidator;

        public AddExpenseValidator(
            IExpenseValidator expenseValidator,
            IGroupValidator groupValidator,
            ILabelValidator labelValidator)
        {
            _expenseValidator = expenseValidator;
            _groupValidator = groupValidator;
            _labelValidator = labelValidator;
        }

        public async Task<bool> ValidateAsync(string userId, AddExpenseRequest request)
        {
            _expenseValidator.Validate(request.Name, request.Value, request.Schedule);
            await _groupValidator.ValidateAsync(userId, request.GroupId);
            await _labelValidator.ValidateAsync(request.LabelId, request.GroupId);
            return true;
        }
    }
}