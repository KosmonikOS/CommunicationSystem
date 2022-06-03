using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Domain.MapperProfiles
{
    public class OptionProfile : Profile
    {
        public OptionProfile()
        {
            CreateMap<Option, CreateOptionDto>();
            CreateMap<CreateOptionDto, Option>();
        }
    }
}
