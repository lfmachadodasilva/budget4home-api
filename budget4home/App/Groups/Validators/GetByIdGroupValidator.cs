using System.Threading.Tasks;

namespace budget4home.App.Groups.Validators
{
    public interface IGetByIdGroupValidator
    {
        Task<bool> ValidateAsync(string userId, long groupId);
    }

    public class GetByIdGroupValidator : IGetByIdGroupValidator
    {
        private readonly IGroupValidator _groupValidator;

        public GetByIdGroupValidator(IGroupValidator groupValidator)
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
