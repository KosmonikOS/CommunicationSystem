using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Domain.MapperProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<User, SearchStudentDto>()
                .ForMember(dest => dest.Name, x =>
                x.MapFrom(src => src.LastName + " " + src.FirstName + " " + src.MiddleName))
                .ForMember(dest => dest.Grade, x =>
                x.MapFrom(src => src.Grade + " " + src.GradeLetter))
                .ForMember(dest => dest.UserId, x => x.MapFrom(src => src.Id));
            CreateMap<TestUser, TestStudentDto>()
                .ForMember(dest => dest.Name, x =>
                x.MapFrom(src => src.User.LastName + " " + src.User.FirstName + " " + src.User.MiddleName))
                .ForMember(dest => dest.Grade, x =>
                x.MapFrom(src => src.User.Grade + " " + src.User.GradeLetter))
                .ForMember(dest => dest.IsSelected, x => x.MapFrom(src => true));
        }
    }
}
