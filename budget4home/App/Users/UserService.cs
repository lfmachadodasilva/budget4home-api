using System.Collections.Generic;
using System.Threading.Tasks;

namespace budget4home.App.Users
{
    public interface IUserService
    {
        Task<IList<UserModel>> GetAllAsync();
    }

    public class UserService : IUserService
    {
        private readonly IFirebaseRepository _firebaseRepository;

        public UserService(IFirebaseRepository firebaseRepository)
        {
            _firebaseRepository = firebaseRepository;
        }

        public Task<IList<UserModel>> GetAllAsync()
        {
            return _firebaseRepository.GetAllUsersAsync();
        }
    }
}