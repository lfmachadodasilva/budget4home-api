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
        }
    }
}