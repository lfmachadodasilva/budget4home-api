using System.Threading.Tasks;
using budget4home.App.Groups.Validators;

namespace budget4home.App.Expenses.Validators
{
    public interface IDeleteExpenseValidator
    {
        Task<bool> ValidateAsync(string userId, long expenseId);
    }

    public class DeleteExpenseValidator : IDeleteExpenseValidator
    {
        private readonly IExpenseValidator _expenseValidator;
        private readonly IGroupValidator _groupValidator;

        public DeleteExpenseValidator(
            IExpenseValidator expenseValidator,
            IGroupValidator groupValidator)
        {
            _expenseValidator = expenseValidator;
            _groupValidator = groupValidator;
        }

        public async Task<bool> ValidateAsync(string userId, long expenseId)
        {
            var expense = await _expenseValidator.ValidateAsync(expenseId);
            await _groupValidator.ValidateAsync(userId, expense.GroupId);

            return true;
        }

    }
}