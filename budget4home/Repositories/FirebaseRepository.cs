using System.Collections.Generic;
using System.Threading.Tasks;
using budget4home.Models;
using FirebaseAdmin.Auth;

namespace budget4home.Repositories
{
    public interface IFirebaseRepository
    {
        Task<IList<UserModel>> GetAllUsersAsync();
    }

    public class FirebaseRepository : IFirebaseRepository
    {
        public FirebaseRepository()
        {
        }

        public async Task<IList<UserModel>> GetAllUsersAsync()
        {
            var ret = new List<UserModel>();

            var enumerator = FirebaseAuth.DefaultInstance.ListUsersAsync(null).GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                var user = enumerator.Current;
                ret.Add(new UserModel
                {
                    Id = user.Uid,
                    Name = string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : user.Email.Split("@")[0],
                    Email = user.Email,
                    PhotoUrl = user.PhotoUrl
                });
            }

            return ret;
        }
    }
}