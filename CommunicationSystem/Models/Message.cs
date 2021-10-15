using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class Message
    {
        public long Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public bool ToGroup { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
    }
}
