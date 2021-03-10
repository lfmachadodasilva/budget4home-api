using System;
using System.Threading.Tasks;

namespace budget4home.App.Groups.Validators
{
    public interface IGroupValidator
    {
        bool Validate(string name);
        Task<bool> ValidateAsync(string userId, long groupId);
    }

    public class GroupValidator: IGroupValidator
    {
        private readonly IGroupRepository _groupRepository;

        public GroupValidator(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public bool Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_GROUP_NAME");
            }

            return true;
        }

        public async Task<bool> ValidateAsync(string userId, long groupId)
        {
            if (!await _groupRepository.MatchAsync(userId, groupId))
            {
                throw new ArgumentException("INVALID_GROUP");
            }

            return true;
        }
    }
}