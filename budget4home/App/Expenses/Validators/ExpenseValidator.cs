using System;
using System.Threading.Tasks;

namespace budget4home.App.Expenses.Validators
{
    public interface IExpenseValidator
    {
        bool Validate(string name, decimal value, int schedule = 1);

        Task<ExpenseModel> ValidateAsync(long expenseId, long? groupId = null);
    }

    public class ExpenseValidator : IExpenseValidator
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseValidator(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public bool Validate(string name, decimal value, int schedule = 1)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_EXPENSE_NAME");
            }

            if (value <= 0)
            {
                throw new ArgumentException("INVALID_EXPENSE_VALUE");
            }

            if (schedule < 1 || schedule > 12)
            {
                throw new ArgumentException("INVALID_EXPENSE_SCHEDULE");
            }

            return true;
        }

        public async Task<ExpenseModel> ValidateAsync(long expenseId, long? groupId = null)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseId);

            if (expense == null)
            {
                throw new ArgumentException("INVALID_EXPENSE");
            }

            if (groupId != null && !expense.GroupId.Equals(groupId))
            {
                throw new ArgumentException("INVALID_EXPENSE_GROUP");
            }

            return expense;
        }
    }
}