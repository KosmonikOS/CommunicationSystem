using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string Name { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
