using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using budget4home.App.Base;
using budget4home.App.Users;

namespace budget4home.App.Groups
{
    [Table("GroupUser")]
    public class GroupUserModel
    {
        [Required]
        public long GroupId { get; set; }

        [ForeignKey("GroupId")]
        public GroupModel Group { get; set; }

        [Required]
        public string UserId { get; set; }
    }

    [Table("Group")]
    public class GroupModel : BaseModel
    {
        public ICollection<GroupUserModel> Users { get; set; }
    }

    public class GroupFullModel : BaseModel
    {
        public ICollection<UserModel> Users { get; set; }
    }
}