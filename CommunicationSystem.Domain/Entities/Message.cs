using CommunicationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ToGroup { get; set; } = 0;
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public ViewStatus ViewStatus { get; set; } = ViewStatus.isntViewed;
        public MessageTypes Type { get; set; } = MessageTypes.Text;

        public int FromId { get; set; }
        public User From { get; set; }
        public int ToId { get; set; }
        public User To { get; set; }
    }
}
