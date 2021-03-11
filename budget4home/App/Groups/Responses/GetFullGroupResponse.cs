using System.Collections.Generic;
using budget4home.App.Users.Responses;

namespace budget4home.App.Groups.Responses
{

    public class GetFullGroupResponse : GetGroupResponse
    {
        public ICollection<GetUserResponse> Users { get; set; }
    }
}