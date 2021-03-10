using System.Threading.Tasks;
using budget4home.App.Groups.Validators;
using budget4home.App.Labels.Requests;

namespace budget4home.App.Labels.Validators
{
    public interface IUpdateLabelValidator
    {
        Task<bool> ValidateAsync(string userId, UpdateLabelRequest request);
    }

    public class UpdateLabelValidator : IUpdateLabelValidator
    {
        private readonly IGroupValidator _groupValidator;
        private readonly ILabelValidator _labelValidator;

        public UpdateLabelValidator(IGroupValidator groupValidator, ILabelValidator labelValidator)
        {
            _groupValidator = groupValidator;
            _labelValidator = labelValidator;
        }

        public async Task<bool> ValidateAsync(string userId, UpdateLabelRequest request)
        {
            _labelValidator.Validate(request.Name);
            await _groupValidator.ValidateAsync(userId, request.Id);

            return true;
        }
    }
}