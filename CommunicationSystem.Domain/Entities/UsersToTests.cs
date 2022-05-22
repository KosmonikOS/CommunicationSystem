using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Entities
{
    public class UsersToTests
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public bool IsCompleted { get; set; }
        public int Mark { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string Grade { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
