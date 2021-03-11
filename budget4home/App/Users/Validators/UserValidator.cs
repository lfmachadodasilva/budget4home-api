using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace budget4home.App.Users.Validators
{
    public interface IUserValidator
    {
        Task<bool> ValidateAsync(ICollection<string> userIds);
    }

    public class UserValidator : IUserValidator
    {
        private readonly IFirebaseRepository _firebaseRepository;

        public UserValidator(IFirebaseRepository firebaseRepository)
        {
            _firebaseRepository = firebaseRepository;
        }

        public async Task<bool> ValidateAsync(ICollection<string> userIds)
        {
            if (!await _firebaseRepository.ExistAsync(userIds))
            {
                throw new ArgumentException("INVALID_USER");
            }

            return true;
        }
    }
}