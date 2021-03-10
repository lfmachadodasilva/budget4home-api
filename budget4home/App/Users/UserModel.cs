using budget4home.App.Base;

namespace budget4home.App.Users
{
    public class UserModel : BaseModelString
    {
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
    }
}