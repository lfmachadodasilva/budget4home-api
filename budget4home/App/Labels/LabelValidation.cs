using budget4home.App.Groups;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace budget4home.App.Labels
{
    public class LabelValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var labelId = (long)value;
            var labelRepository = (ILabelRepository)validationContext.GetService(typeof(ILabelRepository));

            var getByIdTask = labelRepository.GetByIdAsync(labelId, true);
            Task.WaitAll(getByIdTask);

            var label = getByIdTask.Result;
            if (label == null)
            {
                // label does not exist
                return new ValidationResult("INVALID_LABEL");
            }

            long groupId;

            var parent = validationContext.ObjectInstance;
            var groupIdProp = parent.GetType().GetProperty("GroupId");
            if (groupIdProp != null)
            {
                groupId = (long)groupIdProp.GetValue(parent, null);
                if (!groupId.Equals(label.GroupId))
                {
                    // label group does not match with parent group
                    return new ValidationResult("INVALID_LABEL_GROUP");
                }
            }
            else
            {
                groupId = label.GroupId;
            }

            var groupValidation = new GroupValidationAttribute();
            try
            {
                groupValidation.Validate(groupId, validationContext);
            }
            catch (ValidationException ex)
            {
                // invalid group
                return ex.ValidationResult;
            }

            return ValidationResult.Success;
        }
    }
}
