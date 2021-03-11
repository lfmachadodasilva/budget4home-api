using AutoMapper;
using budget4home.App.Labels.Requests;
using budget4home.App.Labels.Responses;

namespace budget4home.App.Labels
{
    public class LabelProfile : Profile
    {
        public LabelProfile()
        {
            CreateMap<LabelModel, LabelModel>()
                // Foreign object need to be ignored
                .ForMember(dest => dest.Group, act => act.Ignore());
            CreateMap<GetLabelsResponse, LabelModel>().ReverseMap();
            CreateMap<GetFullLabelsResponse, LabelFullModel>().ReverseMap();
            CreateMap<AddLabelRequest, LabelModel>().ReverseMap();
            CreateMap<UpdateLabelRequest, LabelModel>().ReverseMap();
        }
    }
}