using CommunicationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreatetestsController : ControllerBase
    {
        private readonly CommunicationContext db;
        public CreatetestsController(CommunicationContext context)
        {
            db = context;
        }
        [HttpGet("{id}")]
        public List<Test> Get(int id)
        {
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            return (from t in db.Tests
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
                                    select new UsersToTests() {
                                       Name = u.FirstName + " " +  u.LastName + " " + u.MiddleName,
                                       Grade = u.Grade + " " + u.GradeLetter,
                                       UserId = u.Id,
                                       IsSelected = true,
                                       IsCompleted = s.IsCompleted,
                                       Mark = s.Mark
                                    }).ToList(),
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

                                                        }).ToList()
                                         }).ToList()
                    }).ToList();
        }
        [HttpGet("getusers/{param}")]
        public List<UsersToTests> GetUsers(string param)
        {
            if (param != "")
            {
                var grade = 0;
                return (from u in db.Users
                        where ((Int32.TryParse(param, out grade) && grade == u.Grade) || (u.FirstName + " " + u.MiddleName + " " + u.LastName).ToLower().Contains(param.ToLower()))
                        && u.Role == 1
                        select new UsersToTests()
                        {
                            UserId = u.Id,
                            Name = u.LastName + " " + u.FirstName + " " + u.MiddleName,
                            Grade = u.Grade + " " + u.GradeLetter

                        }
                        ).ToList();
            }
            return new List<UsersToTests>() { };
        }
        [HttpPost]
        public IActionResult Post(Test test)
        {
            if (test != null)
            {
                try
                {
                    test.Date = DateTime.Now;
                    if (test.Id > 0)
                    {
                        db.Tests.Update(test);
                        var prestudents = db.UsersToTests.Where(s => s.TestId == test.Id).ToList();
                        db.UsersToTests.RemoveRange(prestudents);
                    }
                    else
                    {
                        test.Id = 0;
                        db.Tests.Add(test);
                        db.SaveChanges();
                    }
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
                            db.SaveChanges();
                        }
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
                    foreach(var student in test.Students)
                    {
                        student.TestId = (int)test.Id;
                        db.UsersToTests.Add(student);
                    }
                    db.SaveChanges();
                    return Ok();
                }
                catch(Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpDelete("{type}/{id}")]
        public IActionResult Delete(string type, int id)
        {
            if (type != null && id != 0)
            {
                try
                {
                    switch (type)
                    {
                        case "test":
                            var test = db.Tests.SingleOrDefault(t => t.Id == id);
                            var questions = db.Questions.Where(q => q.TestId == test.Id).ToList();
                            foreach(var q in questions)
                            {
                                var opts = db.Options.Where(o => o.QuestionId == q.Id).ToList();
                                var answrs = db.StudentAnswers.Where(a => a.QuestionId == q.Id).ToList();
                                db.Options.RemoveRange(opts);
                                db.StudentAnswers.RemoveRange(answrs);
                            }
                            db.Questions.RemoveRange(questions);
                            db.Tests.Remove(test);
                            break;
                        case "question":
                            var question = db.Questions.SingleOrDefault(q => q.Id == id);
                            var options = db.Options.Where(o => o.QuestionId == question.Id).ToList();
                            var answers = db.StudentAnswers.Where(a => a.QuestionId == question.Id).ToList();
                            db.Options.RemoveRange(options);
                            db.StudentAnswers.RemoveRange(answers);
                            db.Questions.Remove(question);
                            break;
                        case "option":
                            var option = db.Options.SingleOrDefault(o => o.Id == id);
                            db.Options.Remove(option);
                            break;
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
    }
}
