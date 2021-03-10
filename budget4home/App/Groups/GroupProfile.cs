using System.Linq;
using AutoMapper;
using budget4home.App.Groups.Requests;
using budget4home.App.Groups.Responses;

namespace budget4home.App.Groups
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupModel, GroupModel>().ReverseMap();
            CreateMap<GetGroupResponse, GroupModel>().ReverseMap();
            CreateMap<GetFullGroupResponse, GroupModel>().ForMember(
                dest =>
                    dest.Users,
                opt => opt.MapFrom(src =>
                    src.Users.Select(x => new GroupUserModel { GroupId = src.Id, UserId = x.Id })));
            CreateMap<GroupFullModel, GetFullGroupResponse>().ForMember(
                dest =>
                    dest.Users,
                opt => opt.MapFrom(src => src.Users));
            CreateMap<AddGroupRequest, GroupModel>().ForMember(
                dest =>
                    dest.Users,
                opt => opt.MapFrom(src =>
                    src.Users.Select(x => new GroupUserModel { GroupId = 0, UserId = x })));
            CreateMap<UpdateGroupRequest, GroupModel>().ForMember(
                dest =>
                    dest.Users,
                opt => opt.MapFrom(src =>
                    src.Users.Select(x => new GroupUserModel { GroupId = src.Id, UserId = x })));
        }
    }
}