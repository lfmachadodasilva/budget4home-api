using System.Threading.Tasks;
using budget4home.App.Groups.Requests;
using budget4home.App.Users.Validators;

namespace budget4home.App.Groups.Validators
{
    public interface IUpdateGroupValidator
    {
        Task<bool> ValidateAsync(string userId, UpdateGroupRequest request);
    }

    public class UpdateGroupValidator : IUpdateGroupValidator
    {
        private readonly IGroupValidator _groupValidator;
        private readonly IUserValidator _userValidator;

        public UpdateGroupValidator(IGroupValidator groupValidator, IUserValidator userValidator)
        {
            _groupValidator = groupValidator;
            _userValidator = userValidator;
        }

        public async Task<bool> ValidateAsync(string userId, UpdateGroupRequest request)
        {
            _groupValidator.Validate(request.Name);
            await _groupValidator.ValidateAsync(userId, request.Id);
            await _userValidator.ValidateAsync(request.Users);

            return true;
        }
    }
}