using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.Helpers;
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
        private readonly IValidateHelper _validateHelper;
        private readonly Context _context;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            IValidateHelper validateHelper,
            Context context)
        {
            _expenseRepository = expenseRepository;
            _validateHelper = validateHelper;
            _context = context;
        }

        public Task<List<ExpenseModel>> GetAll(string userId, long groupId, int year, int month)
        {
            return _expenseRepository
                .GetAll()
                .Include(x => x.Label)
                .Include(x => x.Group)
                .Where(x => x.Date.Year == year && x.Date.Month == month && x.GroupId == groupId)
                .ToListAsync();
        }

        public Task<List<KeyValuePair<long, decimal>>> GetAllByLabel(
            string userId,
            long groupId,
            DateTime from,
            DateTime to,
            Func<IEnumerable<decimal>, decimal> func)
        {
            return Task.Run(() =>
            {
                return _context.Expenses
                    .Include(x => x.Group)
                        .ThenInclude(x => x.Users)
                    .Where(x =>
                        // filter by group and user
                        x.Group.Users.Any(x => x.UserId.Equals(userId) && x.GroupId.Equals(groupId)) &&
                        // filter by date
                        x.Date.CompareTo(from) >= 0 && x.Date.CompareTo(to) <= 0)
                    .Select(x => new { Id = x.LabelId, Value = x.Value })
                    // TODO refactor to not get this data
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .Select(x => new KeyValuePair<long, decimal>(x.Key, func(x.Select(y => y.Value))))
                    .ToList();
            });
        }

        public async Task<ExpenseModel> AddAsync(string userId, ExpenseModel model)
        {
            var group = await _validateHelper.CheckGroupAsync(model.GroupId, userId);
            await _validateHelper.CheckLabelAsync(model.LabelId, group);

            var ret = await _expenseRepository.AddAsync(model);
            if (ret != null && await _context.SaveChangesAsync() > 0)
                return ret;
            return null;
        }

        public async Task<ExpenseModel> UpdateAsync(string userId, ExpenseModel model)
        {
            var expense = await _validateHelper.CheckExpenseAsync(model.Id);

            var group = await _validateHelper.CheckGroupAsync(expense.GroupId, userId);
            await _validateHelper.CheckLabelAsync(expense.LabelId, group);

            group = await _validateHelper.CheckGroupAsync(model.GroupId, userId);
            await _validateHelper.CheckLabelAsync(model.LabelId, group);

            var ret = await _expenseRepository.UpdateAsync(model);
            if (ret != null && await _context.SaveChangesAsync() > 0)
                return ret;
            return null;
        }

        public async Task<bool> DeleteAsync(string userId, long id)
        {
            var expense = await _validateHelper.CheckExpenseAsync(id);
            await _validateHelper.CheckGroupAsync(expense.GroupId, userId);

            var ret = await _expenseRepository.DeleteAsync(id);
            if (ret && await _context.SaveChangesAsync() > 0)
                return ret;
            return false;
        }
    }
}