using CommunicationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TestsController : ControllerBase
    {
        private readonly CommunicationContext db;
        public TestsController(CommunicationContext context)
        {
            db = context;
        }
        [HttpGet("{id}")]
        public List<Test> Get(int id)
        {
            return (from utt in db.UsersToTests
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
                    }).ToList();
        }
        [HttpPost]
        public IActionResult Post(TestAnswer testAnswer)
        {
            if (testAnswer != null)
            {
                try
                {
                    var currentAnswers = db.StudentAnswers.Where(u => u.UserId == testAnswer.UserId);
                    db.StudentAnswers.RemoveRange(currentAnswers);
                    var utt = db.UsersToTests.FirstOrDefault(u => u.UserId == testAnswer.UserId && u.TestId == testAnswer.TestId);
                    utt.Mark = CalculateMark(testAnswer);
                    utt.IsCompleted = true;
                    foreach (var question in testAnswer.Questions)
                    {
                        foreach (var answer in question.StudentAnswers)
                        {
                            db.StudentAnswers.Add(new StudentAnswer() { UserId = testAnswer.UserId, Answer = answer.ToString(), QuestionId = question.Id,TestId = testAnswer.TestId });
                        }
                    }
                    db.SaveChanges();
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        private int CalculateMark(TestAnswer testAnswer)
        {
            int totatPoints = testAnswer.Questions.Where(q => q.QuestionType != QuestionType.Open).Sum(q => q.Points);
            double userPoints = 0;
            foreach (var question in testAnswer.Questions.Where(q => q.QuestionType != QuestionType.Open))
            {
                var answers = db.Options.Where(o => o.QuestionId == question.Id && o.IsRightOption == true).Select(o => question.QuestionType == QuestionType.OpenWithCheck ? o.Text : o.Id.ToString()).ToList();
                if (question.StudentAnswers.Count() <= answers.Count())
                {
                    foreach (var studentAnswer in question.StudentAnswers)
                    {
                        foreach (var answer in answers)
                        {
                            if (studentAnswer.ToLower() == answer.ToLower())
                            {
                                userPoints += question.Points / Convert.ToDouble(answers.Count());
                            }
                        }
                    }
                }
            }
            double result = (userPoints / totatPoints) * 100;
            return result >= 90 ? 5 : result >= 70 ? 4 : result >= 60 ? 3 : 2;
        }
    }
}
