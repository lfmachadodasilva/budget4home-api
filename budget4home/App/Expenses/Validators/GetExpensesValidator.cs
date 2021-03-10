using System;
using System.Threading.Tasks;
using budget4home.App.Expenses.Requests;
using budget4home.App.Groups.Validators;

namespace budget4home.App.Expenses.Validators
{
    public interface IGetExpensesValidator
    {
        Task<bool> ValidateAsync(string userId, GetExpensesRequest request);
    }

    public class GetExpensesValidator : IGetExpensesValidator
    {
        private readonly IGroupValidator _groupValidator;

        public GetExpensesValidator(IGroupValidator groupValidator)
        {
            _groupValidator = groupValidator;
        }

        public Task<bool> ValidateAsync(string userId, GetExpensesRequest request)
        {
            if (request.Month < 1 || request.Month > 12)
            {
                throw new ArgumentException("INVALID_MONTH");
            }
            if (request.Year < 1700)
            {
                throw new ArgumentException("INVALID_YEAR");
            }

            return _groupValidator.ValidateAsync(userId, request.Group);
        }
    }
}