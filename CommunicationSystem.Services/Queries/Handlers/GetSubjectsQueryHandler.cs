using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetSubjectsQueryHandler : IRequestHandler<GetSubjectsQuery, IContentResponse<List<Subject>>>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly ILogger<GetSubjectsQueryHandler> logger;

        public GetSubjectsQueryHandler(ISubjectRepository subjectRepository,ILogger<GetSubjectsQueryHandler> logger)
        {
            this.subjectRepository = subjectRepository;
            this.logger = logger;
        }

        public async Task<IContentResponse<List<Subject>>> Handle(GetSubjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subjects = await subjectRepository.GetSubjectsAsync();
                return new ContentResponse<List<Subject>>(ResponseStatus.Ok) { Content = subjects };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<Subject>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
