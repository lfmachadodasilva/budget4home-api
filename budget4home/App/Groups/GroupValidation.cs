using budget4home.Helpers;
using budget4home.Util;
using Microsoft.AspNetCore.Http;
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

            var cache = (ICache)validationContext.GetService(typeof(ICache));

            var task = cache.GetOrCreateAsync(new CacheKey(groupId, "user", userId), () =>
            {
                return groupRepository.MatchAsync(userId, groupId);
            });
            //var task = groupRepository.MatchAsync(userId, groupId);

            task.Wait();

            if (!task.Result)
            {
                return new ValidationResult("INVALID_GROUP");
            }

            return ValidationResult.Success;
        }
    }
}
