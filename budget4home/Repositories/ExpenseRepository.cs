using AutoMapper;
using budget4home.Models;

namespace budget4home.Repositories
{
    public interface IExpenseRepository : IRepository<ExpenseModel, long>
    {
    }

    public class ExpenseRepository : RepositoryBase<ExpenseModel, long>, IExpenseRepository
    {
        public ExpenseRepository(Context context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}