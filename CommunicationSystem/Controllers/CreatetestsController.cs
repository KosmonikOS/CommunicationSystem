using CommunicationSystem.Models;
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
                        SubjectName = s.Name,
                        QuestionsList = (from q in db.Questions
                                         where q.TestId == t.Id
                                         select new Question()
                                         {
                                             Id = q.Id,
                                             TestId = q.Id,
                                             Text = q.Text,
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
        public List<SelectableUser> GetUsers(string param)
        {
            var grade = 0;
            return (from u in db.Users
                    where (Int32.TryParse(param, out grade) && grade == u.Grade) || param.Contains(u.FirstName) || param.Contains(u.MiddleName) || param.Contains(u.LastName)
                    select new SelectableUser()
                    {
                        Id = u.Id,
                        Name = u.LastName + " " + u.FirstName + " " + u.MiddleName,
                        Grade = u.Grade +" " + u.GradeLetter

                    }
                    ).ToList();
        }
        [HttpDelete("{type}/{id}")]
        public IActionResult Delete(string type,int id)
        {
            if(type != null && id != 0) {
                try
                {
                    switch (type)
                    {
                        case "test":
                            var test = this.db.Tests.SingleOrDefault(t => t.Id == id);
                            this.db.Tests.Remove(test);
                            break;
                        case "question":
                            var question = this.db.Questions.SingleOrDefault(q => q.Id == id);
                            this.db.Questions.Remove(question);
                            break;
                        case "option":
                            var option= this.db.Options.SingleOrDefault(o => o.Id == id);
                            this.db.Options.Remove(option);
                            break;
                    }
                    return Ok();
                }
                catch(Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
    }
}
