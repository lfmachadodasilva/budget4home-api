using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.Models;
using Microsoft.EntityFrameworkCore;

namespace budget4home.Repositories
{
    public interface IExpenseRepository : IRepository<ExpenseModel, long>
    {
        Task<List<ExpenseModel>> GetAllAsync(string userId, long groupId, int year, int month);
        Task<List<KeyValuePair<long, decimal>>> GetValueByLabelAsync(
            long groupId,
            DateTime from,
            DateTime to,
            bool sum);
    }

    public class ExpenseRepository : RepositoryBase<ExpenseModel, long>, IExpenseRepository
    {
        private readonly Context _context;

        public ExpenseRepository(Context context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
        }

        public Task<List<ExpenseModel>> GetAllAsync(string userId, long groupId, int year, int month)
        {
            return
                GetAll()
                .Include(x => x.Label)
                .Include(x => x.Group)
                .Where(x => x.Date.Year == year && x.Date.Month == month && x.GroupId == groupId)
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
    }
}