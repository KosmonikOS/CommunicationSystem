
namespace CommunicationSystem.Domain.Entities
{
    public class UserSaltPass
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
