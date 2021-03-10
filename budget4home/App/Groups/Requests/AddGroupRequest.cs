using System.Collections.Generic;

namespace budget4home.App.Groups.Requests
{
    public class AddGroupRequest
    {
        public string Name { get; set; }
        public ICollection<string> Users { get; set; }
    }
}