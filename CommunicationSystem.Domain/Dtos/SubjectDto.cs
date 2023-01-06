using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Dtos
{
    public class SubjectDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Name { get; set; }
    }
}
