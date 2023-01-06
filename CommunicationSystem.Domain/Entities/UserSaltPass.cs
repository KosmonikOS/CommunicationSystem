
namespace CommunicationSystem.Domain.Entities
{
    public class UserSaltPass
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
