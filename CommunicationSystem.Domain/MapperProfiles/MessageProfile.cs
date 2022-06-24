using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Domain.MapperProfiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<AddMessageDto, Message>()
                .ForMember(dest => dest.Group, x => x.Ignore())
                .ForMember(dest => dest.From, x => x.Ignore())
                .ForMember(dest => dest.To, x => x.Ignore())
                .ForMember(dest => dest.FromId, x => x.MapFrom(src => src.From))
                .ForMember(dest => dest.ToId, x => x.MapFrom(src => src.To));
            CreateMap<AddFileMessageDto, Message>()
                .ForMember(dest => dest.Group, x => x.Ignore())
                .ForMember(dest => dest.From, x => x.Ignore())
                .ForMember(dest => dest.To, x => x.Ignore())
                .ForMember(dest => dest.FromId, x => x.MapFrom(src => src.From))
                .ForMember(dest => dest.ToId, x => x.MapFrom(src => src.To));
            CreateMap<Message, SendMessageDto>()
                .ForMember(dest => dest.From, x => x.MapFrom(src => src.FromId))
                .ForMember(dest => dest.To, x => x.MapFrom(src => src.ToId));
        }
    }
}
