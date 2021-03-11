using System;
using System.Linq;
using System.Threading.Tasks;
using budget4home.App.Groups.Requests;
using budget4home.App.Users.Validators;

namespace budget4home.App.Groups.Validators
{
    public interface IAddGroupValidator
    {
        Task<bool> ValidateAsync(string userId, AddGroupRequest request);
    }

    public class AddGroupValidator : IAddGroupValidator
    {
        private readonly IGroupValidator _groupValidator;
        private readonly IUserValidator _userValidator;

        public AddGroupValidator(IGroupValidator groupValidator, IUserValidator userValidator)
        {
            _groupValidator = groupValidator;
            _userValidator = userValidator;
        }

        public async Task<bool> ValidateAsync(string userId, AddGroupRequest request)
        {
            if (!request.Users.Any(u => u.Equals(userId)))
            {
                throw new ArgumentException("INVALID_CURRENT_USER");
            }

            _groupValidator.Validate(request.Name);
            await _userValidator.ValidateAsync(request.Users);

            return true;
        }
    }
}