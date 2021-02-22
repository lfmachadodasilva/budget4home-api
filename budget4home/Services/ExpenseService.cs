using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using budget4home.Helpers;
using budget4home.Models;
using budget4home.Models.Configurations;
using budget4home.Repositories;
using Microsoft.EntityFrameworkCore;

namespace budget4home.Services
{
    public interface IExpenseService
    {
        Task<List<ExpenseModel>> GetAllAsync(string userId, long groupId, int year, int month);
        Task<List<KeyValuePair<long, decimal>>> GetValueByLabelAsync(long groupId, DateTime from, DateTime to, bool sum);
        Task<ExpenseModel> AddAsync(string userId, ExpenseModel model);
        Task<ExpenseModel> UpdateAsync(string userId, ExpenseModel model);
        Task<bool> DeleteAsync(string userId, long id);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IValidateHelper _validateHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            IValidateHelper validateHelper,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _validateHelper = validateHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<List<ExpenseModel>> GetAllAsync(string userId, long groupId, int year, int month)
        {
            return _expenseRepository.GetAllAsync(userId, groupId, year, month);
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

            var group = await _validateHelper.CheckGroupAsync(model.GroupId, userId);
            await _validateHelper.CheckLabelAsync(model.LabelId, group);

            var ret = await _expenseRepository.AddAsync(model);
            if (ret != null && await _unitOfWork.CommitAsync() > 0)
            {
                if (model.ScheduleTotal > 1 && model.ParentId == null)
                {
                    for (var i = 0; i < model.ScheduleTotal; i++)
                    {
                        var lastModel = _mapper.Map<ExpenseModel>(model);

                        lastModel.Id = 0;
                        lastModel.Label = null;
                        lastModel.Group = null;
                        lastModel.Date = lastModel.Date.AddMonths(i + 1);
                        lastModel.ScheduleBy = i + 1;
                        lastModel.ParentId = model.Id;

                        await AddAsync(userId, lastModel);
                    }
                    await _unitOfWork.CommitAsync();
                }

                return ret;
            }

            return null;
        }

        public async Task<ExpenseModel> UpdateAsync(string userId, ExpenseModel model)
        {
            // TODO update schedule
            _unitOfWork.BeginTransaction();

            var expense = await _validateHelper.CheckExpenseAsync(model.Id);
            if (expense.ParentId != null)
            {
                model.ParentId = expense.ParentId;
                model.ScheduleBy = expense.ScheduleBy;
                model.ScheduleTotal = expense.ScheduleTotal;
            }

            var group = await _validateHelper.CheckGroupAsync(expense.GroupId, userId);
            await _validateHelper.CheckLabelAsync(expense.LabelId, group);

            group = await _validateHelper.CheckGroupAsync(model.GroupId, userId);
            await _validateHelper.CheckLabelAsync(model.LabelId, group);

            var ret = await _expenseRepository.UpdateAsync(model);
            if (ret != null && await _unitOfWork.CommitAsync() > 0)
            {
                return ret;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(string userId, long id)
        {
            // TODO delete schedule

            var expense = await _validateHelper.CheckExpenseAsync(id);
            await _validateHelper.CheckGroupAsync(expense.GroupId, userId);

            var ret = await _expenseRepository.DeleteAsync(id);
            if (ret && await _unitOfWork.CommitAsync() > 0)
                return ret;
            return false;
        }
    }
}