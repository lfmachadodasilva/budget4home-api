using System.Linq;
using AutoMapper;
using budget4home.Models.Dtos;

namespace budget4home.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserModel, UserModel>().ReverseMap();
            CreateMap<LabelModel, LabelModel>().ReverseMap();
            CreateMap<GroupModel, GroupModel>().ReverseMap();
            CreateMap<ExpenseModel, ExpenseModel>().ReverseMap();

            CreateMap<LabelDto, LabelModel>().ReverseMap();
            CreateMap<LabelManageDto, LabelModel>().ReverseMap();
            CreateMap<LabelFullDto, LabelFullModel>().ReverseMap();
            CreateMap<UserDto, UserModel>().ReverseMap();
            CreateMap<ExpenseDto, ExpenseModel>().ReverseMap();
            CreateMap<ExpenseModel, ExpenseManageDto>().ReverseMap();
            CreateMap<GroupDto, GroupModel>().ReverseMap();
            CreateMap<GroupFullDto, GroupFullModel>().ReverseMap();

            CreateMap<GroupManageDto, GroupModel>().ForMember(
                dest =>
                    dest.Users,
                    opt => opt.MapFrom(src =>
                        src.Users.Select(x => new GroupUserModel { GroupId = src.Id, UserId = x })));
            CreateMap<GroupModel, GroupManageDto>().ForMember(
                dest =>
                    dest.Users,
                    opt => opt.MapFrom(src => src.Users.Select(x => x.UserId)));
        }
    }
}