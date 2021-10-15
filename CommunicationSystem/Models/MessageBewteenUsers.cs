using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class MessageBewteenUsers
    {
        public long Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public bool ToGroup { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public string NickName { get; set; }
        public string AccountImage { get; set; }
    }
}
