using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Domain.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserAccountDto>()
                .ForMember(dest => dest.Role,x => x.MapFrom(src => src.Role.RoleId));
            CreateMap<UserAccountUpdateDto, User>();
        }
    }
}
