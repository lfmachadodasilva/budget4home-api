using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget4home.Models
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
}