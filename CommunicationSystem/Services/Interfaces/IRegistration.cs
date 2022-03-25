namespace CommunicationSystem.Services.Interfaces
{
    public interface IRegistration
    {
        public bool IsUniqueEmail(string email);
    }
}
