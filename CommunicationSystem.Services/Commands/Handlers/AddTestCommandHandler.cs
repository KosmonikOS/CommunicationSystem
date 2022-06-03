using AutoMapper;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Commands.Handlers
{
    public class AddTestCommandHandler : IRequestHandler<AddTestCommand, IResponse>
    {
        private readonly ICreateTestRepository testRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly ILogger<AddTestCommandHandler> logger;

        public AddTestCommandHandler(ICreateTestRepository testRepository, IStudentRepository studentRepository
            , IMapper mapper, ILogger<AddTestCommandHandler> logger)
        {
            this.testRepository = testRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IResponse> Handle(AddTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var test = mapper.Map<Test>(request.Dto);
                test.Id = Guid.NewGuid();
                test.Date = DateTime.UtcNow;
                testRepository.AddTest(test);
                studentRepository.UpdateTestStudents(request.Dto.Students,test.Id);
                await testRepository.SaveChangesAsync();
                return new BaseResponse(ResponseStatus.Ok);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new BaseResponse(ResponseStatus.InternalServerError);
            }
        }
    }
}
