using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Dtos
{
    public class UpdateStudentMarkDto
    {
        public int UserId { get; set; }
        public Guid TestId { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public int? Mark { get; set; }
    }
}
