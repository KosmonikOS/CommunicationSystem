
namespace CommunicationSystem.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupImage { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
