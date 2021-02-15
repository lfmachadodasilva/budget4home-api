using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.Models;
using budget4home.Repositories;
using Microsoft.EntityFrameworkCore;

namespace budget4home.Services
{
    public interface IExpenseService
    {
        Task<List<ExpenseModel>> GetAll(string userId, long groupId, int year, int month);
        Task<List<KeyValuePair<long, decimal>>> GetAllByLabel(string userId, long groupId, DateTime from, DateTime to, Func<IEnumerable<decimal>, decimal> func);
        Task<ExpenseModel> AddAsync(string userId, ExpenseModel model);
        Task<ExpenseModel> UpdateAsync(string userId, ExpenseModel model);
        Task<bool> DeleteAsync(string userId, long id);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public Task<List<ExpenseModel>> GetAll(string userId, long groupId, int year, int month)
        {
            return _expenseRepository
                .GetAll()
                .Include(x => x.Label)
                .Include(x => x.Group)
                .ToListAsync();
        }

        public Task<List<KeyValuePair<long, decimal>>> GetAllByLabel(
            string userId,
            long groupId,
            DateTime from,
            DateTime to,
            Func<IEnumerable<decimal>, decimal> func)
        {
            return _expenseRepository
                .GetAll()
                .Include(x => x.Group)
                    .ThenInclude(x => x.Users)
                // filter by group and user
                .Where(x => x.Group.Users.Any(x => x.UserId.Equals(userId) && x.GroupId.Equals(groupId)))
                .Where(x => x.Date.CompareTo(from) >= 0 && x.Date.CompareTo(to) <= 0)
                .GroupBy(x => x.Label.Id)
                .Select(x => new KeyValuePair<long, decimal>(x.Key, func(x.Select(y => y.Value))))
                .ToListAsync();
        }

        public Task<ExpenseModel> AddAsync(string userId, ExpenseModel model)
        {
            // TODO check group

            // TODO check label

            return _expenseRepository.AddAsync(model);
        }

        public Task<ExpenseModel> UpdateAsync(string userId, ExpenseModel model)
        {
            // TODO check group

            // TODO check label

            return _expenseRepository.UpdateAsync(model);
        }

        public Task<bool> DeleteAsync(string userId, long id)
        {
            // TODO check userID

            return _expenseRepository.DeleteAsync(id);
        }
    }
}