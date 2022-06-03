using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetStudentsWithSearchQueryHandler : IRequestHandler<GetStudentsWithSearchQuery, IContentResponse<List<SearchStudentDto>>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetStudentsWithSearchQueryHandler> logger;

        public GetStudentsWithSearchQueryHandler(IStudentRepository studentRepository
            ,IMapper mapper,ILogger<GetStudentsWithSearchQueryHandler> logger)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<SearchStudentDto>>> Handle(GetStudentsWithSearchQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dtos = await mapper.ProjectTo<SearchStudentDto>(studentRepository
                    .GetStudents(request.Search,request.SearchOption)).ToListAsync();
                return new ContentResponse<List<SearchStudentDto>>(ResponseStatus.Ok) { Content = dtos };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<SearchStudentDto>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
