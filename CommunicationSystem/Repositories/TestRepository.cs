using CommunicationSystem.Models;
using CommunicationSystem.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CommunicationSystem.Services.Interfaces;

namespace CommunicationSystem.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly CommunicationContext db;
        private readonly ITest test;

        public TestRepository(CommunicationContext db, ITest test)
        {
            this.db = db;
            this.test = test;
        }
        public async Task<List<Test>> GetUserTestsAsync(int id)
        {
            var tests = await (from utt in db.UsersToTests
                               where utt.UserId == id && utt.IsCompleted == false
                               join t in db.Tests on utt.TestId equals t.Id
                               join s in db.Subjects on t.Subject equals s.Id
                               join u in db.Users on t.Creator equals u.Id
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
                                   SubjectName = s.Name,
                                   QuestionsList = (from q in db.Questions
                                                    where q.TestId == t.Id
                                                    select new Question()
                                                    {
                                                        Id = q.Id,
                                                        TestId = q.Id,
                                                        Text = q.Text,
                                                        Points = q.Points,
                                                        Image = q.Image,
                                                        QuestionType = q.QuestionType,
                                                        Options = (from o in db.Options
                                                                   where o.QuestionId == q.Id
                                                                   select new Option()
                                                                   {
                                                                       Id = o.Id,
                                                                       IsRightOption = o.IsRightOption,
                                                                       QuestionId = o.QuestionId,
                                                                       Text = o.Text

                                                                   }).ToList()
                                                    }).ToList()
                               }).ToListAsync();
            return tests;
        }

        public async Task SaveTestAnswerAsync(TestAnswer testAnswer)
        {
            if (testAnswer != null)
            {
                var currentAnswers = db.StudentAnswers.Where(u => u.UserId == testAnswer.UserId && u.TestId == testAnswer.TestId);
                db.StudentAnswers.RemoveRange(currentAnswers);
                var utt = db.UsersToTests.FirstOrDefault(u => u.UserId == testAnswer.UserId && u.TestId == testAnswer.TestId);
                utt.Mark = test.CalculateMark(testAnswer);
                utt.IsCompleted = true;
                foreach (var question in testAnswer.Questions)
                {
                    foreach (var answer in question.StudentAnswers)
                    {
                        db.StudentAnswers.Add(new StudentAnswer() { UserId = testAnswer.UserId, Answer = answer.ToString(), QuestionId = question.Id, TestId = testAnswer.TestId });
                    }
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
