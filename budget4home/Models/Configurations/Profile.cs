using System.Linq;
using AutoMapper;
using budget4home.Models.Dtos;

namespace budget4home.Models.Configurations
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            MapUser();
            MapLabel();
            MapGroup();
            MapExpense();
        }

        private void MapUser()
        {
            CreateMap<UserModel, UserModel>().ReverseMap();
            CreateMap<UserDto, UserModel>().ReverseMap();
        }

        private void MapLabel()
        {
            CreateMap<LabelModel, LabelModel>()
                // Foreign object need to be ignored
                .ForMember(dest => dest.Group, act => act.Ignore());
            CreateMap<LabelDto, LabelModel>().ReverseMap();
            CreateMap<LabelManageDto, LabelModel>().ReverseMap();
            CreateMap<LabelFullDto, LabelFullModel>().ReverseMap();
        }

        private void MapGroup()
        {
            CreateMap<GroupModel, GroupModel>().ReverseMap();
            CreateMap<GroupManageDto, GroupModel>().ForMember(
                dest =>
                    dest.Users,
                    opt => opt.MapFrom(src =>
                        src.Users.Select(x => new GroupUserModel { GroupId = src.Id, UserId = x })));
            CreateMap<GroupModel, GroupManageDto>().ForMember(
                dest =>
                    dest.Users,
                    opt => opt.MapFrom(src => src.Users.Select(x => x.UserId)));
            CreateMap<GroupDto, GroupModel>().ReverseMap();
            CreateMap<GroupFullDto, GroupFullModel>().ReverseMap();
        }

        private void MapExpense()
        {
            CreateMap<ExpenseModel, ExpenseModel>()
                // Foreign object need to be ignored
                .ForMember(dest => dest.Group, act => act.Ignore())
                .ForMember(dest => dest.Label, act => act.Ignore())
                .ForMember(dest => dest.Parent, act => act.Ignore());
            CreateMap<ExpenseDto, ExpenseModel>().ReverseMap();
            CreateMap<ExpenseModel, ExpenseAddDto>().ReverseMap();
            CreateMap<ExpenseModel, ExpenseUpdateDto>().ReverseMap();
        }
    }
}