using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [Email(ErrorMessage = "Некорректный формат почты")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string NickName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string? FirstName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string? MiddleName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Введите от 2 до 50 символов")]
        public string? LastName { get; set; }
        public int? Grade { get; set; }
        public string? GradeLetter { get; set; }
        public string IsConfirmed { get; set; }
        public string AccountImage { get; set; }
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        public string? Phone { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? EnterTime { get; set; }
        public DateTime? LeaveTime { get; set; }

        public Role Role { get; set; }
        public int RoleId { get; set; }
        public UserSaltPass PassHash { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Test> Tests { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; } 

        public string GetFullName => LastName + " " + FirstName + " " + MiddleName;
        public string GetFullGrade => Grade + " " + GradeLetter;
    }
}
