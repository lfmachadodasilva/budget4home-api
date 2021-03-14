using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Base;
using budget4home.Util;
using Microsoft.EntityFrameworkCore;

namespace budget4home.App.Labels
{
    public interface ILabelRepository : IRepository<LabelModel, long>
    {
        Task<List<LabelModel>> GetAllAsync(string userId, long groupId);
    }

    public class LabelRepository : RepositoryBase<LabelModel, long>, ILabelRepository
    {
        private readonly Context _context;

        public LabelRepository(Context context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
        }

        public Task<List<LabelModel>> GetAllAsync(string userId, long groupId)
        {
            return
                GetAll()
                .Include(x => x.Group).ThenInclude(x => x.Users)
                .Where(x =>
                    x.Group.Users.Any(x => x.UserId.Equals(userId) &&
                    x.GroupId.Equals(groupId)))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}