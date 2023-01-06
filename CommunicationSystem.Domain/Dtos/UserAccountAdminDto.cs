
namespace CommunicationSystem.Domain.Dtos
{
    public class UserAccountAdminDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int? Grade { get; set; }
        public string? GradeLetter { get; set; }
        public string AccountImage { get; set; }
        public string? Phone { get; set; }
        public int Role { get; set; }
        public string RoleName { get; set; }
    }
}
