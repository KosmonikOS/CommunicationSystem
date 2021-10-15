using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class UserLastMessage
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string accountImage { get; set; }
        public long MessageId { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
    }
}
