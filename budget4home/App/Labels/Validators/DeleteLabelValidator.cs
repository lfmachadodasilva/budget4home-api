using System.Threading.Tasks;
using budget4home.App.Groups.Validators;

namespace budget4home.App.Labels.Validators
{
    public interface IDeleteLabelValidator
    {
        Task<bool> ValidateAsync(string userId, long labelId);
    }

    public class DeleteLabelValidator : IDeleteLabelValidator
    {
        private readonly ILabelValidator _labelValidator;
        private readonly IGroupValidator _groupValidator;

        public DeleteLabelValidator(
            ILabelValidator labelValidator,
            IGroupValidator groupValidator)
        {
            _labelValidator = labelValidator;
            _groupValidator = groupValidator;
        }

        public async Task<bool> ValidateAsync(string userId, long labelId)
        {
            var label = await _labelValidator.ValidateAsync(labelId);
            await _groupValidator.ValidateAsync(userId, label.GroupId);

            return true;
        }
    }
}