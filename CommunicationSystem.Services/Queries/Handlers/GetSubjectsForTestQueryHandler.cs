using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetSubjectsForTestQueryHandler : IRequestHandler<GetSubjectsForTestQuery, IContentResponse<List<Subject>>>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly ILogger<GetSubjectsForTestQueryHandler> logger;

        public GetSubjectsForTestQueryHandler(ISubjectRepository subjectRepository,ILogger<GetSubjectsForTestQueryHandler> logger)
        {
            this.subjectRepository = subjectRepository;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<Subject>>> Handle(GetSubjectsForTestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subjects = await subjectRepository.GetSubjects().ToListAsync();
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
