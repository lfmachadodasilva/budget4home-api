using AutoMapper;
using budget4home.App.Users.Responses;

namespace budget4home.App.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, UserModel>().ReverseMap();
            CreateMap<GetUserResponse, UserModel>().ReverseMap();
        }
    }
}