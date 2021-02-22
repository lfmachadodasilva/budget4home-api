using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.Helpers;
using budget4home.Models;
using budget4home.Models.Configurations;
using budget4home.Repositories;
using Microsoft.EntityFrameworkCore;

namespace budget4home.Services
{
    public interface IGroupService
    {
        Task<List<GroupModel>> GetAll(string userId);
        Task<List<GroupFullModel>> GetAllFullAsync(string userId);
        Task<GroupModel> AddAsync(string userId, GroupModel model);
        Task<GroupModel> UpdateAsync(string userId, GroupModel model);
        Task<bool> DeleteAsync(string userId, long id);
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IValidateHelper _validateHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Context _context;

        public GroupService(
            IGroupRepository groupRepository,
            IValidateHelper validateHelper,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _validateHelper = validateHelper;
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

        public async Task<GroupModel> AddAsync(string userId, GroupModel model)
        {
            if (model.Users.All(x => x.UserId != userId))
            {
                throw new ForbidException();
            }

            var ret = await _groupRepository.AddAsync(model);
            if (ret != null && await _unitOfWork.CommitAsync() > 0)
                return ret;
            return null;
        }

        public async Task<GroupModel> UpdateAsync(string userId, GroupModel model)
        {
            if (model.Users.All(x => x.UserId != userId))
            {
                throw new ForbidException();
            }
            await _validateHelper.CheckGroupAsync(model.Id, userId);

            var ret = await _groupRepository.UpdateAsync(model);
            if (ret != null && await _unitOfWork.CommitAsync() > 0)
                return ret;
            return null;
        }

        public async Task<bool> DeleteAsync(string userId, long id)
        {
            await _validateHelper.CheckGroupAsync(id, userId);

            var ret = await _groupRepository.DeleteAsync(id);
            if (ret && await _unitOfWork.CommitAsync() > 0)
                return ret;
            return false;
        }
    }
}