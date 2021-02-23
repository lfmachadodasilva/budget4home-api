using System.Collections.Generic;

namespace budget4home.Models.Dtos
{
    public class GroupDto : BaseDto { }

    public class GroupFullDto : GroupDto
    {
        public ICollection<UserDto> Users { get; set; }
    }

    public class GroupManageDto : GroupDto
    {
        public ICollection<string> Users { get; set; }
    }
}