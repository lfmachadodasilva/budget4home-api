using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.App.Base;
using budget4home.Util;
using Microsoft.EntityFrameworkCore;

namespace budget4home.App.Expenses
{
    public interface IExpenseRepository : IRepository<ExpenseModel, long>
    {
        Task<List<ExpenseModel>> GetAllAsync(string userId, long groupId, int? year, int? month);
        Task<List<KeyValuePair<long, decimal>>> GetValueByLabelAsync(
            long groupId,
            DateTime from,
            DateTime to,
            bool sum);
        Task<List<long>> GetAllByParentAsync(long expenseId);
    }

    public class ExpenseRepository : RepositoryBase<ExpenseModel, long>, IExpenseRepository
    {
        private readonly Context _context;

        public ExpenseRepository(Context context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
        }

        public override Task<ExpenseModel> GetByIdAsync(long id, bool include = false)
        {
            if(include)
            {
                return _context.Expenses
                    .Include(e => e.Label)
                    .FirstOrDefaultAsync(e => e.Id.Equals(id));
            }

            return base.GetByIdAsync(id, include);
        }

        public Task<List<ExpenseModel>> GetAllAsync(string userId, long groupId, int? year, int? month)
        {
            if (year.HasValue && month.HasValue)
            {
                return
                GetAll()
                .Include(x => x.Label)
                .Include(x => x.Group)
                .Where(x => x.Date.Year == year && x.Date.Month == month && x.GroupId == groupId)
                .OrderBy(x => x.Date).ThenBy(x => x.Name)
                .ToListAsync();
            }
            return
                GetAll()
                .Include(x => x.Label)
                .Include(x => x.Group)
                .Where(x => x.GroupId == groupId)
                .OrderBy(x => x.Date).ThenBy(x => x.Name)
                .ToListAsync();
        }

        public Task<List<KeyValuePair<long, decimal>>> GetValueByLabelAsync(
            long groupId,
            DateTime from,
            DateTime to,
            bool sum)
        {
            var tmp1 =
                GetAll()
                .Where(x =>
                    // filter by group
                    x.GroupId.Equals(groupId) &&
                    // filter by date
                    x.Date.CompareTo(from) >= 0 && x.Date.CompareTo(to) <= 0)
                .GroupBy(x => x.LabelId);

            var tmp2 = sum ?
                tmp1.Select(x => new KeyValuePair<long, decimal>(x.Key, x.Select(x => x.Value).Sum())) :
                tmp1.Select(x => new KeyValuePair<long, decimal>(x.Key, x.Select(x => x.Value).Average()));

            return tmp2.ToListAsync();
        }

        public Task<List<long>> GetAllByParentAsync(long expenseId)
        {
            return GetAll()
                .Where(x => x.ParentId.Equals(expenseId))
                .Select(x => x.Id)
                .ToListAsync();
        }
    }
}