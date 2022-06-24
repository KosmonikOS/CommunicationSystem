using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddTestCommandHandler : IRequestHandler<AddTestCommand, IResponse>
    {
        private readonly ITestRepository testRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public AddTestCommandHandler(ITestRepository testRepository, IStudentRepository studentRepository
            , IMapper mapper)
        {
            this.testRepository = testRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }
        public async Task<IResponse> Handle(AddTestCommand request, CancellationToken cancellationToken)
        {
            var test = mapper.Map<Test>(request.Dto);
            test.Id = Guid.NewGuid();
            test.Date = DateTime.UtcNow;
            testRepository.AddTest(test);
            studentRepository.UpdateTestStudents(request.Dto.Students, test.Id);
            await testRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
