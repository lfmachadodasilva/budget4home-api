using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.App.Expenses;
using budget4home.Helpers;
using budget4home.Util;

namespace budget4home.App.Labels
{
    public interface ILabelService
    {
        Task<List<LabelModel>> GetAllAsync(string userId, long groupId);
        Task<LabelModel> GetByIdAsync(long id);
        Task<List<LabelFullModel>> GetAllFullAsync(string userId, long groupId, int year, int month);
        Task<LabelModel> AddAsync(string userId, LabelModel model);
        Task<LabelModel> UpdateAsync(string userId, LabelModel model);
        Task<bool> DeleteAsync(string userId, long id);
    }

    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IExpenseService _expenseService;
        private readonly IUnitOfWork _unitOfWork;

        public LabelService(
            ILabelRepository labelRepository,
            IExpenseService expenseService,
            IUnitOfWork unitOfWork)
        {
            _labelRepository = labelRepository;
            _expenseService = expenseService;
            _unitOfWork = unitOfWork;
        }

        public Task<List<LabelModel>> GetAllAsync(string userId, long groupId)
        {
            return _labelRepository.GetAllAsync(userId, groupId);
        }

        public Task<LabelModel> GetByIdAsync(long id)
        {
            return _labelRepository.GetByIdAsync(id);
        }

        public async Task<List<LabelFullModel>> GetAllFullAsync(string userId, long groupId, int year, int month)
        {
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
            return ret.OrderBy(x => x.Name).ToList();
        }

        public async Task<LabelModel> AddAsync(string userId, LabelModel model)
        {
            var ret = await _labelRepository.AddAsync(model);
            if (!(ret != null && await _unitOfWork.CommitAsync() > 0))
            {
                throw new DbException("ERROR_EXPENSE_UPDATE");
            }
            return ret;
        }

        public async Task<LabelModel> UpdateAsync(string userId, LabelModel model)
        {
            var label = await _labelRepository.GetByIdAsync(model.Id);
            model.GroupId = label.GroupId;
            
            var ret = await _labelRepository.UpdateAsync(model);
            if (!(ret != null && await _unitOfWork.CommitAsync() > 0))
            {
                throw new DbException("ERROR_EXPENSE_UPDATE");
            }
            return ret;
        }

        public async Task<bool> DeleteAsync(string userId, long id)
        {
            var ret = await _labelRepository.DeleteAsync(id);
            if (!(!ret && await _unitOfWork.CommitAsync() > 0))
            {
                throw new DbException("ERROR_EXPENSE_DELETE");
            }
            return true;
        }
    }
}
