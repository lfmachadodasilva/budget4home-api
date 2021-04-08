using budget4home.App.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Groups.Requests
{
    public class AddGroupRequest
    {
        [Required]
        public string Name { get; set; }

        [UserValidation(true)]
        public ICollection<string> Users { get; set; }
    }
}