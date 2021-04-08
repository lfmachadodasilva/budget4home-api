using budget4home.App.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Groups.Requests
{
    public class UpdateGroupRequest
    {
        [GroupValidation]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [UserValidation]
        public ICollection<string> Users { get; set; }
    }
}