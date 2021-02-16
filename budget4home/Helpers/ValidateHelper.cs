using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget4home.Models;
using budget4home.Repositories;

namespace budget4home.Helpers
{
    public interface IValidateHelper
    {
        Task<GroupModel> CheckGroupAsync(long groupId, string userId);
        Task<LabelModel> CheckLabelAsync(long labelId, GroupModel group);
        Task<ExpenseModel> CheckExpenseAsync(long expenseId);
    }

    public class ValidateHelper : IValidateHelper
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ILabelRepository _labelRepository;
        private readonly IExpenseRepository _expenseRepository;

        public ValidateHelper(
            IGroupRepository groupRepository,
            ILabelRepository labelRepository,
            IExpenseRepository expenseRepository)
        {
            _groupRepository = groupRepository;
            _labelRepository = labelRepository;
            _expenseRepository = expenseRepository;
        }

        public async Task<GroupModel> CheckGroupAsync(long groupId, string userId)
        {
            // check if the user has access to this group
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null)
            {
                throw new ForbidException();
            }
            if (group.Users.All(x => x.UserId != userId))
            {
                throw new ForbidException();
            }
            return group;
        }

        public async Task<LabelModel> CheckLabelAsync(long labelId, GroupModel group)
        {
            // check if the user has access to this label
            var label = await _labelRepository.GetByIdAsync(labelId);
            if (label == null)
            {
                throw new ForbidException();
            }
            if (label.GroupId != group.Id)
            {
                throw new ForbidException();
            }
            return label;
        }

        public async Task<ExpenseModel> CheckExpenseAsync(long expenseId)
        {
            // check if this ID exist
            var expense = await _expenseRepository.GetByIdAsync(expenseId);
            if (expense == null)
            {
                throw new KeyNotFoundException();
            }
            return expense;
        }
    }
}