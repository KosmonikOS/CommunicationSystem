using CommunicationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public bool IsGroup { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public ViewStatus ViewStatus { get; set; } = ViewStatus.isntViewed;
        public MessageType Type { get; set; } = MessageType.Text;

        public int FromId { get; set; }
        public User From { get; set; }
        public int? ToId { get; set; }
        public User? To { get; set; }
        public Guid? ToGroup { get; set; }
        public Group? Group { get; set; }
    }
}
