using System;
using System.Threading.Tasks;
using budget4home.App.Groups.Validators;
using budget4home.App.Labels.Requests;

namespace budget4home.App.Labels.Validators
{
    public interface IGetFullLabelsValidator
    {
        Task<bool> ValidateAsync(string userId, GetFullLabelsRequest request);
    }

    public class GetFullLabelsValidator : IGetFullLabelsValidator
    {
        private readonly IGroupValidator _groupValidator;

        public GetFullLabelsValidator(IGroupValidator groupValidator)
        {
            _groupValidator = groupValidator;
        }

        public Task<bool> ValidateAsync(string userId, GetFullLabelsRequest request)
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