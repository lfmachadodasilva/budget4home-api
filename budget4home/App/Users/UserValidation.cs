using budget4home.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Users
{
    public class UserValidationAttribute : ValidationAttribute
    {
        private readonly bool _checkYou;

        public UserValidationAttribute(bool checkYou = false)
        {
            _checkYou = checkYou;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var usersId = (ICollection<string>)value;

            if (_checkYou)
            {
                var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
                var userId = UserHelper.GetUserId(httpContextAccessor.HttpContext);

                if(!usersId.Contains(userId))
                {
                    return new ValidationResult("INVALID_CURRENT_USER");
                }
            }

            var firebaseRepository = (IFirebaseRepository)validationContext.GetService(typeof(IFirebaseRepository));

            var task = firebaseRepository.ExistAsync(usersId);
            task.Wait();

            if (!task.Result)
            {
                return new ValidationResult("INVALID_USER");
            }

            return ValidationResult.Success;
        }
    }
}
