using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.Helpers;
using budget4home.Util;

namespace budget4home.App.Expenses
{
    public interface IExpenseService
    {
        Task<List<ExpenseModel>> GetAllAsync(string userId, long groupId, int? year, int? month);
        Task<List<int>> GetYearsAsync(string userId);
        Task<ExpenseModel> GetByIdAsync(long id);
        Task<List<KeyValuePair<long, decimal>>> GetValueByLabelAsync(long groupId, DateTime from, DateTime to, bool sum);
        Task<ExpenseModel> AddAsync(string userId, ExpenseModel model);
        Task<ExpenseModel> UpdateAsync(string userId, ExpenseModel model);
        Task<bool> DeleteAsync(string userId, long id, bool includeSchedule);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<List<ExpenseModel>> GetAllAsync(string userId, long groupId, int? year, int? month)
        {
            return _expenseRepository.GetAllAsync(userId, groupId, year, month);
        }

        public Task<List<int>> GetYearsAsync(string userId)
        {
            return _expenseRepository.GetYearsAsync(userId);
        }

        public Task<ExpenseModel> GetByIdAsync(long id)
        {
            return _expenseRepository.GetByIdAsync(id, true);
        }

        public async Task<List<KeyValuePair<long, decimal>>> GetValueByLabelAsync(
            long groupId,
            DateTime from,
            DateTime to,
            bool sum)
        {
            return await _expenseRepository.GetValueByLabelAsync(groupId, from, to, sum);
        }

        public async Task<ExpenseModel> AddAsync(string userId, ExpenseModel model)
        {
            _unitOfWork.BeginTransaction();

            var ret = await _expenseRepository.AddAsync(model);
            if (!(ret != null && await _unitOfWork.CommitAsync() > 0))
            {
                throw new DbException("ERROR_EXPENSE_ADD");
            }

            if (model.ScheduleTotal > 1 && model.ParentId == null)
            {
                for (var i = 0; i < model.ScheduleTotal - 1; i++)
                {
                    var lastModel = _mapper.Map<ExpenseModel>(model);

                    lastModel.Id = 0;
                    lastModel.Label = null;
                    lastModel.Group = null;
                    lastModel.Date = lastModel.Date.AddMonths(i + 1);
                    lastModel.ScheduleBy = i + 2;
                    lastModel.ParentId = model.Id;

                    await AddAsync(userId, lastModel);
                }
                await _unitOfWork.CommitAsync();
            }

            return ret;
        }

        public async Task<ExpenseModel> UpdateAsync(string userId, ExpenseModel model)
        {
            _unitOfWork.BeginTransaction();

            var expense = await _expenseRepository.GetByIdAsync(model.Id);

            // update non-update fields
            model.GroupId = expense.GroupId;
            model.ParentId = expense.ParentId;
            model.ScheduleBy = expense.ScheduleBy;
            model.ScheduleTotal = expense.ScheduleTotal;

            var ret = await _expenseRepository.UpdateAsync(model);
            var commitedItems = await _unitOfWork.CommitAsync();
            if (commitedItems <= 0)
            {
                throw new DbException("ERROR_EXPENSE_DELETE");
            }
            return ret;
        }

        public async Task<bool> DeleteAsync(string userId, long id, bool includeSchedule)
        {
            if (includeSchedule)
            {
                var expenseToDelete = await _expenseRepository.GetByIdAsync(id);

                // check and delete schedule
                var parentId = expenseToDelete.ParentId ?? expenseToDelete.Id;
                var lstToDelete = new List<long>(await _expenseRepository.GetAllByParentAsync(parentId));
                // delete all children
                foreach (var idToDelete in lstToDelete)
                    await _expenseRepository.DeleteAsync(idToDelete);
                // mark to delete parent
                id = parentId;
            }

            await _expenseRepository.DeleteAsync(id);
            var commitedItems = await _unitOfWork.CommitAsync();
            if(commitedItems <= 0)
            {
                throw new DbException("ERROR_EXPENSE_DELETE");
            }

            return true;
        }
    }
}
