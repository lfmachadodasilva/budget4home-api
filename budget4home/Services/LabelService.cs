using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.Helpers;
using budget4home.Models;
using budget4home.Models.Configurations;
using budget4home.Repositories;

namespace budget4home.Services
{
    public interface ILabelService
    {
        Task<List<LabelModel>> GetAllAsync(string userId, long groupId);
        Task<List<LabelFullModel>> GetAllFullAsync(string userId, long groupId, int year, int month);
        Task<LabelModel> AddAsync(string userId, LabelModel model);
        Task<LabelModel> UpdateAsync(string userId, LabelModel model);
        Task<bool> DeleteAsync(string userId, long id);
    }

    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IExpenseService _expenseService;
        private readonly IValidateHelper _validateHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Context _context;

        public LabelService(
            ILabelRepository labelRepository,
            IExpenseService expenseService,
            IGroupRepository groupRepository,
            IValidateHelper validateHelper,
            IUnitOfWork unitOfWork)
        {
            _labelRepository = labelRepository;
            _expenseService = expenseService;
            _validateHelper = validateHelper;
            _unitOfWork = unitOfWork;
        }

        public Task<List<LabelModel>> GetAllAsync(string userId, long groupId)
        {
            return _labelRepository.GetAllAsync(userId, groupId);
        }

        public async Task<List<LabelFullModel>> GetAllFullAsync(string userId, long groupId, int year, int month)
        {
            await _validateHelper.CheckGroupAsync(groupId, userId);

            var labels = await GetAllAsync(userId, groupId);

            var date = new DateTime(year, month, 1);
            var currValue = await _expenseService.GetValueByLabelAsync(groupId, date, date.AddMonths(1), true);
            var lastValue = await _expenseService.GetValueByLabelAsync(groupId, date.AddMonths(-1), date.AddDays(-1), true);
            var avgValue = await _expenseService.GetValueByLabelAsync(groupId, new DateTime(1700, 1, 1), date.AddDays(-1), false);

            var ret = from label in labels
                      join curr in currValue on label.Id equals curr.Key into c
                      join last in lastValue on label.Id equals last.Key into l
                      join avg in avgValue on label.Id equals avg.Key into a
                      from curr in c.DefaultIfEmpty(new KeyValuePair<long, decimal>(label.Id, 0))
                      from last in l.DefaultIfEmpty(new KeyValuePair<long, decimal>(label.Id, 0))
                      from avg in a.DefaultIfEmpty(new KeyValuePair<long, decimal>(label.Id, 0))
                      select new LabelFullModel
                      {
                          Id = label.Id,
                          Name = label.Name,
                          Group = label.Group,
                          AvgValue = avg.Value,
                          CurrValue = curr.Value,
                          LastValue = last.Value
                      };
            return ret.ToList();
        }

        public async Task<LabelModel> AddAsync(string userId, LabelModel model)
        {
            await _validateHelper.CheckGroupAsync(model.GroupId, userId);

            var ret = await _labelRepository.AddAsync(model);
            if (ret != null && await _unitOfWork.CommitAsync() > 0)
                return ret;
            return null;
        }

        public async Task<LabelModel> UpdateAsync(string userId, LabelModel model)
        {
            var group = await _validateHelper.CheckGroupAsync(model.GroupId, userId);
            await _validateHelper.CheckLabelAsync(model.Id, group);

            var ret = await _labelRepository.UpdateAsync(model);
            if (ret != null && await _unitOfWork.CommitAsync() > 0)
                return ret;
            return null;
        }

        public async Task<bool> DeleteAsync(string userId, long id)
        {
            await _validateHelper.CheckGroupAsync(id, userId);

            var ret = await _labelRepository.DeleteAsync(id);
            if (ret && await _unitOfWork.CommitAsync() > 0)
                return ret;
            return false;
        }
    }
}