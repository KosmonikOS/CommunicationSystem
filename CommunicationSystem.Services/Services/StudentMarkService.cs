using AutoMapper;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Services
{
    public class StudentMarkService : IStudentMarkService
    {
        private readonly IOptionRepository optionRepository;
        private readonly IMapper mapper;

        public StudentMarkService(IOptionRepository optionRepository, IMapper mapper)
        {
            this.optionRepository = optionRepository;
            this.mapper = mapper;
        }
        public async Task<IContentResponse<int>> CalculateStudentMarkAsync(StudentFullTestAnswerDto dto)
        {
            var testRightOptions = await mapper.ProjectTo<RightOptionDto>(optionRepository
                .GetRightOptions(dto.TestId)).ToListAsync();
            if (!testRightOptions.Any())
                return new ContentResponse<int>(ResponseStatus.NotFound);
            dto.Questions = dto.Questions.Where(x => x.QuestionType != QuestionType.Open).ToList();
            var totalPoints = dto.Questions.Sum(x => x.Points);
            double userPoints = 0;
            foreach (var question in dto.Questions)
            {
                var rightOptions = testRightOptions.Where(x => x.QuestionId == question.Id).ToList();
                if (question.Answers.Count() <= rightOptions.Count())
                {
                    foreach (var studentAnswer in question.Answers)
                    {
                        if (rightOptions.Any(x => x.Value ==
                            studentAnswer.ToLower()))
                        {
                            userPoints += question.Points / (double)rightOptions.Count();
                        }
                    }

                }
            }
            double precentage = (userPoints / totalPoints) * 100;
            var mark = precentage >= 90 ? 5 : precentage >= 70 ? 4 : precentage >= 60 ? 3 : 2;
            return new ContentResponse<int>(ResponseStatus.Ok) { Content = mark };
        }
    }
}
