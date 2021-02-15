using AutoMapper;

namespace budget4home.Models
{
    public class MyExpensesProfile : Profile
    {
        public MyExpensesProfile()
        {
            CreateMap<UserModel, UserModel>().ReverseMap();
            CreateMap<LabelModel, LabelModel>().ReverseMap();
            CreateMap<GroupModel, GroupModel>().ReverseMap();
            CreateMap<ExpenseModel, ExpenseModel>().ReverseMap();
        }
    }
}