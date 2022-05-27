using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddSubjectCommandHandler : IRequestHandler<AddSubjectCommand, IResponse>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;
        private readonly ILogger<AddSubjectCommandHandler> logger;

        public AddSubjectCommandHandler(ISubjectRepository subjectRepository
            ,IMapper mapper,ILogger<AddSubjectCommandHandler> logger)
        {
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subject = mapper.Map<Subject>(request.Dto);
                subjectRepository.AddSubject(subject);
                await subjectRepository.SaveChangesAsync();
                return new BaseResponse(ResponseStatus.Ok);
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError);
            }
        }
    }
}
