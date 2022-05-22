using CommunicationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
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
