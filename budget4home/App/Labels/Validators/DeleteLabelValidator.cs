using System.Threading.Tasks;
using budget4home.App.Groups.Validators;

namespace budget4home.App.Labels.Validators
{
    public interface IDeleteLabelValidator
    {
        Task<bool> ValidateAsync(string userId, long groupId);
    }

    public class DeleteLabelValidator : IDeleteLabelValidator
    {
        private readonly IGroupValidator _groupValidator;

        public DeleteLabelValidator(IGroupValidator groupValidator)
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