using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationSystem.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupImage { get; set; } = "/assets/group.png";
        [NotMapped]
        public GroupUser[] Users { get; set; }
    }
}
