using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;

namespace CommunicationSystem.Domain.MapperProfiles
{
    public class OptionProfile : Profile
    {
        public OptionProfile()
        {
            CreateMap<Option, CreateOptionDto>();
            CreateMap<CreateOptionDto, Option>();
            CreateMap<Option, OptionShowDto>();
            CreateMap<Option, RightOptionDto>()
                .ForMember(dest => dest.Value, x => x.MapFrom(src =>
                src.Question.QuestionType == QuestionType.OpenWithCheck
                ? src.Text.ToLower()
                : src.Id.ToString()));
        }
    }
}
