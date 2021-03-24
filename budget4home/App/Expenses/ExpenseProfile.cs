using AutoMapper;
using budget4home.App.Expenses.Requests;
using budget4home.App.Expenses.Responses;

namespace budget4home.App.Expenses
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<ExpenseModel, ExpenseModel>()
                // Foreign object need to be ignored
                .ForMember(dest => dest.Group, act => act.Ignore())
                .ForMember(dest => dest.Label, act => act.Ignore())
                .ForMember(dest => dest.Parent, act => act.Ignore());
            CreateMap<AddExpenseRequest, ExpenseModel>()
                .ForMember(dest => dest.ScheduleTotal, act => act.MapFrom(x => x.Schedule))
                .ForMember(dest => dest.ScheduleBy, act => act.MapFrom(x => 1))
                .ReverseMap();
            CreateMap<UpdateExpenseRequest, ExpenseModel>().ReverseMap();
            CreateMap<GetExpenseResponse, ExpenseModel>().ReverseMap();
        }
    }
}
