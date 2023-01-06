using CommunicationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Dtos
{
    public class AddMessageDto
    { 
        public bool IsGroup { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Content { get; set; }
        public int From { get; set; }
        public int? To { get; set; }
        public Guid? ToGroup { get; set; }

    }
}
