using CommunicationSystem.Domain.Entities;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IPasswordHashService
    {
        public UserSaltPass GenerateSaltPass(string password);
        public bool ComparePasswords(string hash, string salt, string password);
    }
}
