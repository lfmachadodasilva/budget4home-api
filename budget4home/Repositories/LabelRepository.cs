using AutoMapper;
using budget4home.Models;

namespace budget4home.Repositories
{
    public interface ILabelRepository : IRepository<LabelModel, long>
    {
    }

    public class LabelRepository : RepositoryBase<LabelModel, long>, ILabelRepository
    {
        public LabelRepository(Context context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}