using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget4home.Models
{
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