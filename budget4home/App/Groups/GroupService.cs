using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.App.Labels;
using budget4home.Helpers;
using budget4home.Util;
using Microsoft.EntityFrameworkCore;

namespace budget4home.App.Groups
{
    public interface IGroupService
    {
        Task<List<GroupModel>> GetAll(string userId);
        Task<List<GroupFullModel>> GetAllFullAsync(string userId);
        Task<GroupModel> GetByIdAsync(long id);
        Task<GroupModel> AddAsync(string userId, GroupModel model);
        Task<GroupModel> UpdateAsync(string userId, GroupModel model);
        Task<bool> DeleteAsync(string userId, long id);
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ILabelService _labelService;
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(
            IGroupRepository groupRepository,
            ILabelService labelService,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _labelService = labelService;
            _unitOfWork = unitOfWork;
        }

        public Task<List<GroupModel>> GetAll(string userId)
        {
            return _groupRepository.GetAll(userId).ToListAsync();
        }

        public Task<List<GroupFullModel>> GetAllFullAsync(string userId)
        {
            return _groupRepository.GetAllFullAsync(userId);
        }

        public Task<GroupModel> GetByIdAsync(long id)
        {
            return _groupRepository.GetByIdAsync(id);
        }

        public async Task<GroupModel> AddAsync(string userId, GroupModel model)
        {
            var ret = await _groupRepository.AddAsync(model);
            var commitedItems = await _unitOfWork.CommitAsync();
            if (commitedItems <= 0)
            {
                throw new DbException("ERROR_EXPENSE_DELETE");
            }
            return ret;
        }

        public async Task<GroupModel> UpdateAsync(string userId, GroupModel model)
        {
            var ret = await _groupRepository.UpdateAsync(model);
            var commitedItems = await _unitOfWork.CommitAsync();
            if (commitedItems <= 0)
            {
                throw new DbException("ERROR_EXPENSE_DELETE");
            }
            return ret;
        }

        public async Task<bool> DeleteAsync(string userId, long id)
        {
            // delete all labels
            await _labelService.DeleteByGroupAsync(userId, id);

            await _groupRepository.DeleteAsync(id);
            var commitedItems = await _unitOfWork.CommitAsync();
            if (commitedItems <= 0)
            {
                throw new DbException("ERROR_EXPENSE_DELETE");
            }
            return true;
        }
    }
}