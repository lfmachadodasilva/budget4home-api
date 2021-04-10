using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;

namespace budget4home.App.Users
{
    public interface IFirebaseRepository
    {
        Task<IList<UserModel>> GetAllUsersAsync();
        Task<bool> ExistAsync(ICollection<string> userIds);
    }

    public class FirebaseRepository : IFirebaseRepository
    {
        public FirebaseRepository()
        {
        }

        public async Task<IList<UserModel>> GetAllUsersAsync()
        {
            var ret = new List<UserModel>(100);
            var enumerator = FirebaseAuth.DefaultInstance.ListUsersAsync(null).GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                var user = enumerator.Current;
                ret.Add(new UserModel
                {
                    Id = user.Uid,
                    Name = string.IsNullOrEmpty(user.DisplayName) ? user.Email.Split("@")[0] : user.DisplayName,
                    Email = user.Email,
                    PhotoUrl = user.PhotoUrl
                });
            }

            return ret;
        }

        public async Task<bool> ExistAsync(ICollection<string> userIds)
        {
            var identifiers = new List<UserIdentifier>(userIds.Select(id => new UidIdentifier(id)));
            var result = await FirebaseAuth.DefaultInstance.GetUsersAsync(identifiers);
            return !result.NotFound.Any();
        }
    }
}
