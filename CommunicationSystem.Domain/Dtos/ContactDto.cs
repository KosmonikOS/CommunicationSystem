using CommunicationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Dtos
{
    public class ContactDto
    {
        public int? ToId { get; set; }
        public Guid? ToGroup { get; set; }
        public bool IsGroup { get; set; }
        public string NickName { get; set; }
        public string AccountImage { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageDate { get; set; }
        public MessageType? LastMessageType { get; set; }
        public int NotViewedMessages { get; set; }
        public string? LastActivity { get; set; }
    }
}
