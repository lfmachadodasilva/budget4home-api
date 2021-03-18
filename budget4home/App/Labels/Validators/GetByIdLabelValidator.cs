using System.Threading.Tasks;
using budget4home.App.Groups.Validators;

namespace budget4home.App.Labels.Validators
{
    public interface IGetByIdLabelValidator
    {
        Task<bool> ValidateAsync(string userId, long labelId);
    }

    public class GetByIdLabelValidator : IGetByIdLabelValidator
    {
        private readonly IGroupValidator _groupValidator;
        private readonly ILabelValidator _labelValidator;

        public GetByIdLabelValidator(IGroupValidator groupValidator, ILabelValidator labelValidator)
        {
            _groupValidator = groupValidator;
            _labelValidator = labelValidator;
        }

        public async Task<bool> ValidateAsync(string userId, long labelId)
        {
            var label = await _labelValidator.ValidateAsync(labelId);
            await _groupValidator.ValidateAsync(userId, label.GroupId);

            return true;
        }
    }
}