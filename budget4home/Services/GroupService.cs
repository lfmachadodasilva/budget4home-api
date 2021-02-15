using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.Models;
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

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public Task<List<GroupModel>> GetAll(string userId)
        {
            return _groupRepository.GetAll(userId).ToListAsync();
        }

        public Task<List<GroupFullModel>> GetAllFullAsync(string userId)
        {
            return _groupRepository.GetAllFullAsync(userId);
        }

        public Task<GroupModel> AddAsync(string userId, GroupModel model)
        {
            // TODO check user id

            return _groupRepository.AddAsync(model);
        }

        public Task<GroupModel> UpdateAsync(string userId, GroupModel model)
        {
            // TODO check user id

            return _groupRepository.UpdateAsync(model);
        }

        public Task<bool> DeleteAsync(string userId, long id)
        {
            // TODO check user id

            return _groupRepository.DeleteAsync(id);
        }
    }
}