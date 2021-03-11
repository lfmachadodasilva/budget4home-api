using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Base;
using budget4home.App.Users;
using budget4home.Util;
using Microsoft.EntityFrameworkCore;

namespace budget4home.App.Groups
{
    public interface IGroupRepository : IRepository<GroupModel, long>
    {
        Task<bool> MatchAsync(string userId, long groupId);
        IQueryable<GroupModel> GetAll(string userId);
        Task<List<GroupFullModel>> GetAllFullAsync(string userId);
    }

    public class GroupRepository : RepositoryBase<GroupModel, long>, IGroupRepository
    {
        private readonly Context _context;
        private readonly IFirebaseRepository _firebaseRepository;

        public GroupRepository(
            Context context,
            IMapper mapper,
            IFirebaseRepository firebaseRepository)
            : base(context, mapper)
        {
            _context = context;
            _firebaseRepository = firebaseRepository;
        }

        public Task<bool> MatchAsync(string userId, long groupId)
        {
            return _context.GroupUser.AnyAsync(gu => gu.UserId.Equals(userId) && gu.GroupId.Equals(groupId));
        }

        public IQueryable<GroupModel> GetAll(string userId)
        {
            return base.GetAll()
                .Include(x => x.Users)
                .Where(x => x.Users.Any(y => y.UserId.Equals(userId)));
        }

        public async Task<List<GroupFullModel>> GetAllFullAsync(string userId)
        {
            var usersTask = _firebaseRepository.GetAllUsersAsync();
            var groups = GetAll(userId).ToList();
            var users = await usersTask;

            return groups.Select(group =>
            {
                var usersTmp = group.Users.Join(
                    users,
                    groupUser => groupUser.UserId,
                    user => user.Id,
                    (gu, u) => u).ToList();

                return new GroupFullModel
                {
                    Id = group.Id,
                    Name = group.Name,
                    Users = usersTmp
                };
            }).ToList();
        }

        public override Task<GroupModel> GetByIdAsync(long id, bool include)
        {
            //_logger.LogInformation("get all");
            return _context.Groups
                .Include(g => g.Users)
                    .ThenInclude(gu => gu.Group)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}