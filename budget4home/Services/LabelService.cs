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
    public interface ILabelService
    {
        Task<List<LabelModel>> GetAll(string userId, long groupId);
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
        private readonly Context _context;

        public LabelService(
            ILabelRepository labelRepository,
            IExpenseService expenseService,
            IGroupRepository groupRepository,
            IValidateHelper validateHelper,
            Context context)
        {
            _labelRepository = labelRepository;
            _expenseService = expenseService;
            _validateHelper = validateHelper;
            _context = context;
        }

        public Task<List<LabelModel>> GetAll(string userId, long groupId)
        {
            return _labelRepository
                .GetAll()
                .Include(x => x.Group).ThenInclude(x => x.Users)
                .Where(x =>
                    x.Group.Users.Any(x => x.UserId.Equals(userId) &&
                    x.GroupId.Equals(groupId)))
                .ToListAsync();
        }

        public async Task<List<LabelFullModel>> GetAllFullAsync(string userId, long groupId, int year, int month)
        {
            var labels = _labelRepository
                .GetAll()
                .Include(x => x.Group).ThenInclude(x => x.Users)
                .Where(x =>
                    x.Group.Users.Any(x => x.UserId.Equals(userId) &&
                    x.GroupId.Equals(groupId))).ToList();

            var date = new DateTime(year, month, 1);
            var currValue = await _expenseService.GetAllByLabel(userId, groupId, date, date.AddMonths(1), x => x.Sum());
            var lastValue = await _expenseService.GetAllByLabel(userId, groupId, date.AddMonths(-1), date.AddDays(-1), x => x.Sum());
            var avgValue = await _expenseService.GetAllByLabel(userId, groupId, date, date.AddDays(-1), x => x.Average());

            return (from label in labels
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
                    }).ToList();
        }

        public async Task<LabelModel> AddAsync(string userId, LabelModel model)
        {
            await _validateHelper.CheckGroupAsync(model.GroupId, userId);

            var ret = await _labelRepository.AddAsync(model);
            if (ret != null && await _context.SaveChangesAsync() > 0)
                return ret;
            return null;
        }

        public async Task<LabelModel> UpdateAsync(string userId, LabelModel model)
        {
            var group = await _validateHelper.CheckGroupAsync(model.GroupId, userId);
            await _validateHelper.CheckLabelAsync(model.Id, group);

            var ret = await _labelRepository.UpdateAsync(model);
            if (ret != null && await _context.SaveChangesAsync() > 0)
                return ret;
            return null;
        }

        public async Task<bool> DeleteAsync(string userId, long id)
        {
            await _validateHelper.CheckGroupAsync(id, userId);

            var ret = await _labelRepository.DeleteAsync(id);
            if (ret && await _context.SaveChangesAsync() > 0)
                return ret;
            return false;
        }
    }
}