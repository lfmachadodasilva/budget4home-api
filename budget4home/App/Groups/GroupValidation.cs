using budget4home.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Groups
{
    public class GroupValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var userId = UserHelper.GetUserId(httpContextAccessor.HttpContext);

            var groupId = (long)value;
            var groupRepository = (IGroupRepository)validationContext.GetService(typeof(IGroupRepository));

            var task = groupRepository.MatchAsync(userId, groupId);
            task.Wait();

            if(!task.Result)
            {
                return new ValidationResult("INVALID_GROUP");
            }

            return ValidationResult.Success;
        }
    }
}
