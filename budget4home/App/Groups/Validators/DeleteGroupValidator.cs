using System.Threading.Tasks;
using budget4home.App.Groups.Requests;

namespace budget4home.App.Groups.Validators
{
    public interface IDeleteGroupValidator
    {
        Task<bool> ValidateAsync(string userId, long groupId);
    }

    public class DeleteGroupValidator : IDeleteGroupValidator
    {
        private readonly IGroupValidator _groupValidator;

        public DeleteGroupValidator(IGroupValidator groupValidator)
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