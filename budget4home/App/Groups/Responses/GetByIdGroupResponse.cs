using System.Collections.Generic;

namespace budget4home.App.Groups.Responses
{
    public class GetByIdGroupResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Users { get; set; }

    }
}