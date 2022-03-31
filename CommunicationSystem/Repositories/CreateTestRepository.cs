using CommunicationSystem.Models;
using CommunicationSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Repositories
{
    public class CreateTestRepository : ICreateTestRepository
    {
        private readonly CommunicationContext db;

        public CreateTestRepository(CommunicationContext db)
        {
            this.db = db;
        }

        public void DeleteOptionWithoutSaving(int id)
        {
            var option = db.Options.SingleOrDefault(o => o.Id == id);
            db.Options.Remove(option);
        }

        public async Task DeleteQuestionWithoutSavingAsync(int id)
        {
            var question = db.Questions.SingleOrDefault(q => q.Id == id);
            var options = db.Options.Where(o => o.QuestionId == question.Id).AsNoTracking().ToListAsync();
            var answers = db.StudentAnswers.Where(a => a.QuestionId == question.Id).AsNoTracking().ToListAsync();
            await Task.WhenAll(options, answers);
            db.Options.RemoveRange(options.Result);
            db.StudentAnswers.RemoveRange(answers.Result);
            db.Questions.Remove(question);
        }

        public async Task DeleteTestEntity(string type, int id)
        {
            switch (type)
            {
                case "test":
                    await DeleteTestWithoutSavingAsync(id);
                    break;
                case "question":
                    await DeleteQuestionWithoutSavingAsync(id);
                    break;
                case "option":
                    DeleteOptionWithoutSaving(id);
                    break;
            }
            await db.SaveChangesAsync();
        }

        public async Task DeleteTestWithoutSavingAsync(int id)
        {
            var test = db.Tests.AsNoTracking().SingleOrDefault(t => t.Id == id);
            var questions = await db.Questions.Where(q => q.TestId == test.Id).AsNoTracking().ToListAsync();
            foreach (var q in questions)
            {
                var opts = await db.Options.Where(o => o.QuestionId == q.Id).AsNoTracking().ToListAsync();
                var answrs = await db.StudentAnswers.Where(a => a.QuestionId == q.Id).AsNoTracking().ToListAsync();
                db.Options.RemoveRange(opts);
                db.StudentAnswers.RemoveRange(answrs);
            }
            db.Questions.RemoveRange(questions);
            db.Tests.Remove(test);
        }

        public async Task<List<UsersToTests>> GetStudentsByParamAsync(string param)
        {
            param = param.ToLower();
            int grade;
            var isGrade = Int32.TryParse(param, out grade);
            var students = await (from u in db.Users
                                  where ((isGrade && grade == u.Grade) || (u.LastName + " " + u.FirstName + " " + u.MiddleName).ToLower().Contains(param))
                                  && u.Role == 1
                                  select new UsersToTests()
                                  {
                                      UserId = u.Id,
                                      Name = u.GetFullName,
                                      Grade = u.GetFullGrade

                                  }
                    ).AsNoTracking().ToListAsync();
            return students;
        }

        public async Task<List<Question>> GetUsersAnswersAsync(int id, int testId)
        {
            var answers = await (from q in db.Questions
                                 where q.TestId == testId
                                 select new Question()
                                 {
                                     Id = q.Id,
                                     Image = q.Image,
                                     Points = q.Points,
                                     QuestionType = q.QuestionType,
                                     TestId = q.TestId,
                                     Text = q.Text,
                                     OpenAnswer = db.StudentAnswers.FirstOrDefault(a => a.UserId == id && a.QuestionId == q.Id).Answer ?? "",
                                     Options = (from o in db.Options
                                                where o.QuestionId == q.Id
                                                select new Option()
                                                {
                                                    Id = o.Id,
                                                    Text = o.Text,
                                                    IsRightOption = o.IsRightOption,
                                                    QuestionId = o.QuestionId,
                                                    IsSelected = db.StudentAnswers.FirstOrDefault(a => a.Answer.ToLower() == (q.QuestionType == QuestionType.OpenWithCheck ? o.Text.ToLower() : o.Id.ToString()) && a.UserId == id && a.QuestionId == q.Id) != null,
                                                }
                                     ).AsNoTracking().ToList()
                                 }
                ).AsNoTracking().ToListAsync();
            return answers;
        }

        public async Task<List<Test>> GetUsersTestsAsync(int id)
        {
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            var tests = await (from t in db.Tests
                               join s in db.Subjects on t.Subject equals s.Id
                               join u in db.Users on t.Creator equals u.Id
                               where t.Creator.ToString().Contains(user.Role == 3 ? "" : user.Id.ToString())
                               select new Test()
                               {
                                   Id = t.Id,
                                   Date = t.Date,
                                   Creator = t.Creator,
                                   CreatorName = u.NickName,
                                   Grade = t.Grade,
                                   Name = t.Name,
                                   Questions = t.Questions,
                                   Time = t.Time,
                                   Subject = t.Subject,
                                   Students = (from s in db.UsersToTests
                                               join u in db.Users on s.UserId equals u.Id
                                               where s.TestId == t.Id
                                               select new UsersToTests()
                                               {
                                                   Name = u.GetFullName,
                                                   Grade = u.GetFullGrade,
                                                   UserId = u.Id,
                                                   IsSelected = true,
                                                   IsCompleted = s.IsCompleted,
                                                   Mark = s.Mark
                                               }).AsNoTracking().ToList(),
                                   SubjectName = s.Name,
                                   QuestionsList = (from q in db.Questions
                                                    where q.TestId == t.Id
                                                    select new Question()
                                                    {
                                                        Id = q.Id,
                                                        TestId = q.Id,
                                                        Text = q.Text,
                                                        Image = q.Image,
                                                        Points = q.Points,
                                                        QuestionType = q.QuestionType,
                                                        Options = (from o in db.Options
                                                                   where o.QuestionId == q.Id
                                                                   select new Option()
                                                                   {
                                                                       Id = o.Id,
                                                                       IsRightOption = o.IsRightOption,
                                                                       QuestionId = o.QuestionId,
                                                                       Text = o.Text

                                                                   }).AsNoTracking().ToList()
                                                    }).AsNoTracking().ToList()
                               }).AsNoTracking().ToListAsync();
            return tests;
        }

        public async Task SaveTestAsync(Test test)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                if (test != null)
                {
                    if (test.Id > 0)
                    {
                        db.Tests.Update(test);
                        var prestudents = await db.UsersToTests.Where(s => s.TestId == test.Id).AsNoTracking().ToListAsync();
                        db.UsersToTests.RemoveRange(prestudents);
                    }
                    else
                    {
                        test.Date = DateTime.Now;
                        test.Id = 0;
                        db.Add(test);
                        await db.SaveChangesAsync();
                    }
                    if (test.QuestionsList != null)
                    {
                        foreach (var question in test.QuestionsList)
                        {
                            question.TestId = (int)test.Id;
                            if (question.Id > 0)
                            {
                                db.Questions.Update(question);
                            }
                            else
                            {
                                question.Id = 0;
                                db.Questions.Add(question);
                                await db.SaveChangesAsync();
                            }
                            if (question.Options != null)
                            {
                                foreach (var option in question.Options)
                                {
                                    if (option.Id > 0)
                                    {
                                        db.Options.Update(option);
                                    }
                                    else
                                    {
                                        option.QuestionId = question.Id;
                                        option.Id = 0;
                                        db.Options.Add(option);
                                    }
                                }
                            }
                        }
                    }
                    if (test.Students != null)
                    {
                        foreach (var student in test.Students)
                        {
                            student.TestId = (int)test.Id;
                            db.UsersToTests.Add(student);
                        }
                    }
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task UpdateStudentMarkAsync(int studentId, int testId, int mark)
        {
            var utt = db.UsersToTests.SingleOrDefault(u => u.TestId == testId && u.UserId == studentId);
            utt.Mark = mark;
            await db.SaveChangesAsync();
        }
    }
}
