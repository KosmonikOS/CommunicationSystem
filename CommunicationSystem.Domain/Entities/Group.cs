
using System.ComponentModel.DataAnnotations;

namespace CommunicationSystem.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Введите от 2 до 30 символов")]
        public string Name { get; set; }
        public string GroupImage { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
