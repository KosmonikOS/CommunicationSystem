using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class TestAnswer
    {
        public int TestId { get; set; }
        public int UserId { get; set; }
        public List<Question> Questions { get; set; }
    }
}
