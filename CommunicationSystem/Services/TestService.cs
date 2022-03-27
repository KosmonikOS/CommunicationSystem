using CommunicationSystem.Models;
using CommunicationSystem.Services.Interfaces;
using System;
using System.Linq;

namespace CommunicationSystem.Services
{
    public class TestService : ITest
    {
        private readonly CommunicationContext db;

        public TestService(CommunicationContext db)       
        {
            this.db = db;
        }
        public int CalculateMark(TestAnswer testAnswer)
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
                                break;
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
