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

        public IQueryable<User> GetStudents(string search, StudentsSearchOption searchOption)
        {
            var query = context.Users.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                switch (searchOption)
                {
                    case StudentsSearchOption.FullName:
                        query = query.Where(x => EF.Functions.ILike(
                            x.LastName + " " + x.FirstName + " " + x.MiddleName, $"%{search}%"));
                        break;
                    case StudentsSearchOption.Grade:
                        query = query.Where(x => EF.Functions.ILike(
                            x.Grade + x.GradeLetter, $"%{search}%"));
                        break;
                }
            }
            return query;
        }

        public IQueryable<TestUser> GetStudents(Guid id)
        {
            return context.TestUser.Where(x => x.TestId == id);
        }

        public IQueryable<StudentAnswerDto> GetStudentAnswers(int userId, Guid testId)
        {
            return (from q in context.Questions
                    where q.TestId == testId
                    join s in context.StudentAnswers
                    on new { UserId = userId, QuestionId = q.Id } equals new { UserId = s.UserId, QuestionId = s.QuestionId }
                    into Questions from a in Questions.DefaultIfEmpty()
                    select new StudentAnswerDto()
                    {
                        Id = q.Id,
                        Image = q.Image,
                        Points = q.Points,
                        QuestionType = q.QuestionType,
                        Text = q.Text,
                        OpenAnswer = q.QuestionType == QuestionType.Open
                        ? a.Answer : "",
                        Options = (from o in context.Options
                                   where q.Id == o.QuestionId
                                   select
                                   new StudentAnswerOptionDto()
                                   {
                                       Id = o.Id,
                                       Text = o.Text,
                                       IsRightOption = o.IsRightOption,
                                       IsSelected = q.QuestionType == QuestionType.OpenWithCheck
                                       ? a.Answer.ToLower() == o.Text.ToLower()
                                       : a.Answer == o.Id.ToString()
                                   })
                    }).AsNoTracking();
        }
        public IResponse UpdateStudentMark(UpdateStudentMarkDto dto)
        {
            var testUser = context.TestUser.Find(dto.UserId, dto.TestId);
            if (testUser == null)
                return new BaseResponse(ResponseStatus.NotFound) { Message = "Пользователь не найден" };
            testUser.Mark = dto.Mark;
            return new BaseResponse(ResponseStatus.Ok);
        }

        public void UpdateTestStudents(IEnumerable<TestStudentStateDto> students,Guid testId)
        {
            context.AddRange(students
                .Where(x => x.State == StudentState.Added)
                .Select(x => new TestUser()
                {
                    UserId = x.UserId,
                    TestId = testId,
                    IsCompleted = x.IsCompleted
                }));
            context.UpdateRange(students
                .Where(x => x.State == StudentState.Modified)
                .Select(x => new TestUser()
                {
                    UserId = x.UserId,
                    TestId = testId,
                    IsCompleted = x.IsCompleted
                }));
            context.RemoveRange(students
                .Where(x => x.State == StudentState.Deleted)
                .Select(x => new TestUser()
                {
                    UserId = x.UserId,
                    TestId = testId,
                }));
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
