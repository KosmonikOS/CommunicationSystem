using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Domain.MapperProfiles
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<Test, CreateTestShowDto>()
                .ForMember(dest => dest.Subject, x => x.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.Creator, x => x.MapFrom(src => src.CreatorId))
                .ForMember(dest => dest.CreatorName, x => x.MapFrom(src => src.Creator.NickName));
            CreateMap<CreateTestDto, Test>()
                .ForMember(dest => dest.Students, x => x.Ignore())
                .ForMember(dest => dest.Creator, x => x.Ignore())
                .ForMember(dest => dest.CreatorId, x => x.MapFrom(src => src.Creator))
                .ForMember(dest => dest.QuestionsQuantity, x => x.MapFrom(src => src.Questions.Count))
                .ForMember(dest => dest.Subject, x => x.Ignore())
                .ForMember(dest => dest.SubjectId, x => x.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Questions, x => x.MapFrom(src => src.Questions));
        }
    }
}
