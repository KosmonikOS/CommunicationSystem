using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetSubjectsForTestQueryHandler : IRequestHandler<GetSubjectsForTestQuery, IContentResponse<List<Subject>>>
    {
        private readonly ISubjectRepository subjectRepository;

        public GetSubjectsForTestQueryHandler(ISubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
        }
        public async Task<IContentResponse<List<Subject>>> Handle(GetSubjectsForTestQuery request, CancellationToken cancellationToken)
        {
            var subjects = await subjectRepository.GetSubjects().ToListAsync(cancellationToken);
            return new ContentResponse<List<Subject>>(ResponseStatus.Ok) { Content = subjects };
        }
    }
}
