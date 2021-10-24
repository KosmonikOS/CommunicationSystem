using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public enum ViewStatus
    {
        isntViewed,
        isViewed,
    }
    public enum MessageTypes
    {
        Text,
        Image,
    }
    public class Message
    {
        public long Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int ToGroup { get; set; } = 0;
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        public string ToEmail {get;set;}
        public ViewStatus ViewStatus { get; set; } = ViewStatus.isntViewed;
        public MessageTypes Type { get; set; } = MessageTypes.Text;
    }
}
