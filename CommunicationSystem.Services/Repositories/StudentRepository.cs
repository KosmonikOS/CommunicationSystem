using CommunicationSystem.Data;
using CommunicationSystem.Domain.Dtos;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using CommunicationSystem.Services.Infrastructure.Enums;
using CommunicationSystem.Services.Infrastructure.Responses;
using CommunicationSystem.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunicationSystem.Services.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CommunicationContext context;

        public StudentRepository(CommunicationContext context)
        {
            this.context = context;
        }
        public IQueryable<TestUser> GetStudents(Guid id)
        {
            return context.TestUser.Where(x => x.TestId == id);
        }

        public IQueryable<StudentAnswerShowDto> GetStudentAnswers(int userId, Guid testId)
        {
            return context.Questions
                .Where(x => x.TestId == testId)
                .Select(x => new StudentAnswerShowDto()
                {
                    Id = x.Id,
                    Image = x.Image,
                    Points = x.Points,
                    QuestionType = x.QuestionType,
                    Text = x.Text,
                    OpenAnswer = (int)x.QuestionType > 1 ?
                    x.StudentAnswers.FirstOrDefault(x => x.UserId == userId).Answer : "",
                    Options = context.Options.Where(y => y.QuestionId == x.Id)
                        .Select(y => new StudentAnswerOptionShowDto()
                        {
                            Id = y.Id,
                            Text = y.Text,
                            IsRightOption = y.IsRightOption,
                            IsSelected = x.StudentAnswers
                                .Any(z => z.UserId == userId && z.Answer.ToLower() ==
                                (x.QuestionType == QuestionType.OpenWithCheck
                                ? y.Text.ToLower() : y.Id.ToString()))
                        }).ToList()
                });
        }
        public IResponse UpdateStudentMark(UpdateStudentMarkDto dto)
        {
            var testUser = context.TestUser.Find(dto.UserId, dto.TestId);
            if (testUser == null)
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            testUser.Mark = dto.Mark;
            testUser.IsCompleted = true;
            return new BaseResponse(ResponseStatus.Ok);
        }

        public void AddStudentAnswers(StudentFullTestAnswerDto dto)
        {
            context.AddRange(dto.Questions.SelectMany(x => x.Answers, (x, y) => new StudentAnswer()
            {
                Answer = y,
                QuestionId = x.Id,
                TestId = dto.TestId,
                UserId = dto.UserId
            }));
        }

        public void UpdateTestStudents(IEnumerable<TestStudentStateDto> students, Guid testId)
        {
            context.AddRange(students
                .Where(x => x.State == DbEntityState.Added)
                .Select(x => new TestUser()
                {
                    UserId = x.UserId,
                    TestId = testId,
                    IsCompleted = x.IsCompleted
                }));
            context.UpdateRange(students
                .Where(x => x.State == DbEntityState.Modified)
                .Select(x => new TestUser()
                {
                    UserId = x.UserId,
                    TestId = testId,
                    IsCompleted = x.IsCompleted
                }));
            context.RemoveRange(students
                .Where(x => x.State == DbEntityState.Deleted)
                .Select(x => new TestUser()
                {
                    UserId = x.UserId,
                    TestId = testId,
                }));
            foreach (var student in students.Where(x => x.State == Domain.Enums.DbEntityState.Deleted ||
                (x.State == DbEntityState.Modified && !x.IsCompleted)))
            {
                var answers = context.StudentAnswers.Where(x => x.UserId == student.UserId &&
                    x.TestId == testId).AsEnumerable();
                context.RemoveRange(answers);
            }
        }
        public int SaveChanges()
        {
            return context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
