using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public LabelService(ILabelRepository labelRepository, IExpenseService expenseService)
        {
            _labelRepository = labelRepository;
            _expenseService = expenseService;
        }

        public Task<List<LabelModel>> GetAll(string userId, long groupId)
        {
            return _labelRepository
                .GetAll()
                .Include(x => x.Group)
                    .ThenInclude(x => x.Users)
                .Where(x =>
                    x.Group.Users.Any(x => x.UserId.Equals(userId) &&
                    x.GroupId.Equals(groupId)))
                .ToListAsync();
        }

        public async Task<List<LabelFullModel>> GetAllFullAsync(string userId, long groupId, int year, int month)
        {
            var labels = _labelRepository
                .GetAll()
                .Include(x => x.Group)
                    .ThenInclude(x => x.Users)
                .Where(x =>
                    x.Group.Users.Any(x => x.UserId.Equals(userId) &&
                    x.GroupId.Equals(groupId)));

            var currValueTask = _expenseService.GetAllByLabel(
                userId,
                groupId,
                new DateTime(year, month, 1),
                new DateTime(year, month, 1).AddMonths(1),
                x => x.Sum());
            var lastValueTask = _expenseService.GetAllByLabel(
                userId,
                groupId,
                new DateTime(year, month, 1).AddMonths(-1),
                new DateTime(year, month, 1).AddDays(-1),
                x => x.Sum());
            var avgValueTask = _expenseService.GetAllByLabel(
                userId,
                groupId,
                new DateTime(year, month, 1).AddDays(-1),
                new DateTime(year, month, 1).AddYears(-1),
                x => x.Average());

            // Task.WaitAll(currValueTask, lastValueTask, avgValueTask);

            return (from l in labels
                    join curr in await currValueTask on l.Id equals curr.Key
                    join last in await lastValueTask on l.Id equals last.Key
                    join avg in await avgValueTask on l.Id equals avg.Key
                    select new LabelFullModel
                    {
                        Id = l.Id,
                        Name = l.Name,
                        Group = l.Group,
                        AvgValue = avg.Value,
                        CurrValue = curr.Value,
                        LastValue = last.Value
                    }).ToList();
        }

        public Task<LabelModel> AddAsync(string userId, LabelModel model)
        {
            // TODO check group

            return _labelRepository.AddAsync(model);
        }

        public Task<LabelModel> UpdateAsync(string userId, LabelModel model)
        {
            // TODO check group

            return _labelRepository.UpdateAsync(model);
        }

        public Task<bool> DeleteAsync(string userId, long id)
        {
            // TODO check group

            return _labelRepository.DeleteAsync(id);
        }
    }
}