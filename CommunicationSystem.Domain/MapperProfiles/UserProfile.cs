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
                .ForMember(dest => dest.Role, x => x.MapFrom(src => src.RoleId));
            CreateMap<User, UserAccountAdminDto>()
                .ForMember(dest => dest.Role, x => x.MapFrom(src => src.RoleId));
            CreateMap<UserAccountUpdateDto, User>();
            CreateMap<UserAccountAdminAddDto, User>()
                .ForMember(dest => dest.Role, x => x.Ignore())
                .ForMember(dest => dest.RoleId, x => x.MapFrom(src => src.Role));
            CreateMap<UserAccountAdminUpdateDto, User>()
                .ForMember(dest => dest.Role, x => x.Ignore())
                .ForMember(dest => dest.RoleId, x => x.MapFrom(src => src.Role));
            CreateMap<User, GroupMemberShowDto>()
                .ForMember(dest => dest.UserId, x => x.MapFrom(src => src.Id));
            CreateMap<User, GroupSearchMemberDto>()
                .ForMember(dest => dest.UserId, x => x.MapFrom(src => src.Id));
        }
    }
}
