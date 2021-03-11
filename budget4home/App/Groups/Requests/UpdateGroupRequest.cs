using System.Collections.Generic;

namespace budget4home.App.Groups.Requests
{
    public class UpdateGroupRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Users { get; set; }
    }
}