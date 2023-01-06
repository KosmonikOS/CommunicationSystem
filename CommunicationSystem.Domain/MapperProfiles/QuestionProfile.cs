using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Domain.MapperProfiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, CreateQuestionDto>();
            CreateMap<CreateQuestionDto, Question>();
            CreateMap<Question, QuestionShowDto>();
        }
    }
}
