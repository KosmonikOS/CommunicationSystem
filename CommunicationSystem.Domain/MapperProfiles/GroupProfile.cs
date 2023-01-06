using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Domain.MapperProfiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupShowDto>()
                .ForMember(dest => dest.Members, x => x.MapFrom(src => src.Users));
            CreateMap<CreateGroupDto, Group>()
                .ForMember(dest => dest.Users, x => x.Ignore());
        }
    }
}
