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
            return query.Take(50);
        }

        public IQueryable<TestUser> GetStudents(Guid id)
        {
            return context.TestUser.Where(x => x.TestId == id);
        }

        public IQueryable<StudentAnswerShowDto> GetStudentAnswers(int userId, Guid testId)
        {
            //return (from q in context.Questions
            //        where q.TestId == testId
            //        join s in context.StudentAnswers
            //        on new { UserId = userId, QuestionId = q.Id } equals new { UserId = s.UserId, QuestionId = s.QuestionId }
            //        into Questions
            //        from a in Questions.DefaultIfEmpty()
            //        select new StudentAnswerShowDto()
            //        {
            //            Id = q.Id,
            //            Image = q.Image,
            //            Points = q.Points,
            //            QuestionType = q.QuestionType,
            //            Text = q.Text,
            //            OpenAnswer = (int)q.QuestionType > 1
            //            ? a.Answer : "",
            //            Options = (from o in context.Options
            //                       where q.Id == o.QuestionId
            //                       select
            //                       new StudentAnswerOptionShowDto()
            //                       {
            //                           Id = o.Id,
            //                           Text = o.Text,
            //                           IsRightOption = o.IsRightOption,
            //                           IsSelected = q.QuestionType == QuestionType.OpenWithCheck
            //                           ? a.Answer.ToLower() == o.Text.ToLower()
            //                           : a.Answer == o.Id.ToString()
            //                       })
            //        }).AsNoTracking();
            //return (from s in context.StudentAnswers
            //        where s.UserId == userId && s.TestId == testId
            //        join q in context.Questions
            //        on s.QuestionId equals q.Id
            //        into Questions
            //        from a in Questions.DefaultIfEmpty()
            //        select new StudentAnswerShowDto()
            //        {
            //            Id = a.Id,
            //            Image = a.Image,
            //            Points = a.Points,
            //            QuestionType = a.QuestionType,
            //            Text = a.Text,
            //            OpenAnswer = (int)a.QuestionType > 1
            //            ? s.Answer : "",
            //            Options = (from o in context.Options
            //                       where a.Id == o.QuestionId
            //                       select
            //                       new StudentAnswerOptionShowDto()
            //                       {
            //                           Id = o.Id,
            //                           Text = o.Text,
            //                           IsRightOption = o.IsRightOption,
            //                           IsSelected = a.QuestionType == QuestionType.OpenWithCheck
            //                           ? s.Answer.ToLower() == o.Text.ToLower()
            //                           : s.Answer == o.Id.ToString()
            //                       })
            //        }).AsNoTracking();
            //return (from q in context.Questions
            //        where q.TestId == testId
            //        select new StudentAnswerShowDto()
            //        {
            //            Id = q.Id,
            //            Image = q.Image,
            //            Points = q.Points,
            //            QuestionType = q.QuestionType,
            //            Text = q.Text,
            //            OpenAnswer = (int)q.QuestionType > 1
            //                ? context.StudentAnswers.FirstOrDefault(x => x.UserId == userId && x.QuestionId == q.Id).Answer
            //                : "",
            //            Options = (from o in context.Options
            //                       where q.Id == o.QuestionId
            //                       select
            //                       new StudentAnswerOptionShowDto()
            //                       {
            //                           Id = o.Id,
            //                           Text = o.Text,
            //                           IsRightOption = o.IsRightOption,
            //                           //IsSelected = q.QuestionType == QuestionType.OpenWithCheck
            //                           //? a.Answer.ToLower() == o.Text.ToLower()
            //                           //: a.Answer == o.Id.ToString()
            //                           IsSelected = context.StudentAnswers
            //                           .Where(x => x.UserId == userId && x.QuestionId == q.Id)
            //                           .Any(x => x.Answer.ToLower() ==
            //                           (q.QuestionType == QuestionType.OpenWithCheck
            //                           ? o.Text.ToLower()
            //                           : o.Id.ToString()))
            //                       }).ToList()
            //        }).AsNoTracking();
            return context.Questions
                .Where(x => x.TestId == testId)
                .Select(x => new StudentAnswerShowDto()
                {
                    Id = x.Id,
                    Image = x.Image,
                    Points = x.Points,
                    QuestionType = x.QuestionType,
                    Text = x.Text,
                    OpenAnswer = (int)x.QuestionType > 1?
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
            foreach (var student in students.Where(x => x.State == StudentState.Deleted ||
                (x.State == StudentState.Modified && !x.IsCompleted)))
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
