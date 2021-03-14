using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace budget4home.App.Groups.Validators
{
    public interface IGetByIdValidator
    {
        Task<bool> ValidateAsync(string userId, long groupId);
    }

    public class GetByIdValidator : IGetByIdValidator
    {
        private readonly IGroupValidator _groupValidator;

        public GetByIdValidator(IGroupValidator groupValidator)
        {
            _groupValidator = groupValidator;
        }

        public async Task<bool> ValidateAsync(string userId, long groupId)
        {
            await _groupValidator.ValidateAsync(userId, groupId);

            return true;
        }
    }
}
