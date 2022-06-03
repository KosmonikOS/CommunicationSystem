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
    public class GetTestStudentsQueryHandler : IRequestHandler<GetTestStudentsQuery, IContentResponse<List<TestStudentDto>>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetTestStudentsQueryHandler> logger;

        public GetTestStudentsQueryHandler(IStudentRepository studentRepository
            ,IMapper mapper,ILogger<GetTestStudentsQueryHandler> logger)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IContentResponse<List<TestStudentDto>>> Handle(GetTestStudentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dtos = await mapper.ProjectTo<TestStudentDto>(studentRepository
                    .GetStudents(request.TestId)).ToListAsync();
                return new ContentResponse<List<TestStudentDto>>(ResponseStatus.Ok) { Content = dtos };
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new ContentResponse<List<TestStudentDto>>(ResponseStatus.InternalServerError);
            }
        }
    }
}
