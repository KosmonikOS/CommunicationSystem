using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class UpdateTestCommandHandler : IRequestHandler<UpdateTestCommand, IResponse>
    {
        private readonly ITestRepository testRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public UpdateTestCommandHandler(ITestRepository testRepository, IStudentRepository studentRepository
            , IMapper mapper)
        {
            this.testRepository = testRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }
        public async Task<IResponse> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
        {
            var test = mapper.Map<Test>(request.Dto);
            testRepository.UpdateTest(test);
            studentRepository.UpdateTestStudents(request.Dto.Students, test.Id);
            await testRepository.SaveChangesAsync();
            return new BaseResponse(ResponseStatus.Ok);
        }
    }
}
