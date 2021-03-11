using System.Threading.Tasks;
using budget4home.App.Groups.Validators;
using budget4home.App.Labels.Requests;

namespace budget4home.App.Labels.Validators
{
    public interface IAddLabelValidator
    {
        Task<bool> ValidateAsync(string userId, AddLabelRequest request);
    }

    public class AddLabelValidator : IAddLabelValidator
    {
        private readonly ILabelValidator _labelValidator;
        private readonly IGroupValidator _groupValidator;

        public AddLabelValidator(ILabelValidator labelValidator, IGroupValidator groupValidator)
        {
            _labelValidator = labelValidator;
            _groupValidator = groupValidator;
        }

        public async Task<bool> ValidateAsync(string userId, AddLabelRequest request)
        {
            _labelValidator.Validate(request.Name);
            await _groupValidator.ValidateAsync(userId, request.GroupId);

            return true;
        }
    }
}