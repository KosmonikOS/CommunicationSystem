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
    }
}
