using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Queries.Handlers
{
    public class GetTestStudentsQueryHandler : IRequestHandler<GetTestStudentsQuery, IContentResponse<List<TestStudentDto>>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public GetTestStudentsQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<List<TestStudentDto>>> Handle(GetTestStudentsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await mapper.ProjectTo<TestStudentDto>(studentRepository
                .GetStudents(request.TestId)).ToListAsync();
            return new ContentResponse<List<TestStudentDto>>(ResponseStatus.Ok) { Content = dtos };
        }
    }
}
