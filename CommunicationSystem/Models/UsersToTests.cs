using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class UsersToTests
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string Grade { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
