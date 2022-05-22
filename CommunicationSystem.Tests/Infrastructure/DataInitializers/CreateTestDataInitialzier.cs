using AutoFixture;
using CommunicationSystem.Data;
using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CommunicationSystem.Tests.Infrastructure.DataInitializers
{
    public static class CreateTestDataInitialzier
    {
        public static void Initialize(CommunicationContext context)
        {
            var utt = new Fixture().Build<UsersToTests>()
                                   .With(utt => utt.TestId,1)
                                   .With(utt => utt.UserId,1).Create();
                
            
            var users = new List<User>(){
                new Fixture().Build<User>()
                             .With(u => u.Id,1).Create(),
                new Fixture().Build<User>()
                             .With(u => u.MiddleName,"Test0")
                             .With(u => u.Role,1).Create(),
                new Fixture().Build<User>()
                             .With(u => u.MiddleName,"Test1")
                             .With(u => u.Role,1).Create()
        };
            var subject = new Fixture().Create<Subject>();
            var test = new Fixture().Build<Test>()
                                    .With(T => T.Id,1)
                                    .With(t => t.Subject, subject.Id)
                                    .With(t => t.Creator, users[0].Id).Create();
            var questions = new List<Question>()
            {
                new Fixture().Build<Question>()
                                        .With(q => q.TestId,test.Id)
                                        .With(q => q.QuestionType,QuestionType.Single)
                                        .With(q => q.Points,10).Create(),
                new Fixture().Build<Question>()
                                        .With(q => q.TestId,test.Id)
                                        .With(q => q.QuestionType,QuestionType.OpenWithCheck)
                                        .With(q => q.OpenAnswer,"OpenAnswer")
                                        .With(q => q.Points,20).Create()
        };
            var options = new List<Option>()
            {
                new Fixture().Build<Option>()
                             .With(o => o.QuestionId,questions[0].Id)
                             .With(o => o.IsRightOption,true).Create(),
                new Fixture().Build<Option>()
                             .With(o => o.QuestionId,questions[1].Id)
                             .With(o => o.IsRightOption,true).Create()
            };
            var answers = new List<StudentAnswer>()
            {
                new Fixture().Build<StudentAnswer>()
                             .With(a => a.QuestionId,questions[0].Id)
                             .With(a => a.UserId,users[0].Id)
                             .With(a => a.TestId,test.Id)
                             .With(a =>a.Answer,options[0].Id.ToString())
                             .Create(),
                new Fixture().Build<StudentAnswer>()
                             .With(a => a.QuestionId,questions[0].Id)
                             .With(a => a.UserId,users[0].Id)
                             .With(a => a.TestId,test.Id)
                             .With(a =>a.Answer,questions[1].OpenAnswer)
                             .Create()

            };
            context.Users.AddRange(users);
            context.UsersToTests.Add(utt);
            context.Subjects.Add(subject);
            context.Tests.Add(test);
            context.Questions.AddRange(questions);
            context.Options.AddRange(options);
            context.StudentAnswers.AddRange(answers);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
