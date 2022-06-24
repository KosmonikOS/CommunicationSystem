using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddSubjectCommandHandler : IRequestHandler<AddSubjectCommand, IResponse>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;

        public AddSubjectCommandHandler(ISubjectRepository subjectRepository, IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
        }
        public async Task<IResponse> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = mapper.Map<Subject>(request.Dto);
            subjectRepository.AddSubject(subject);
            await subjectRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
