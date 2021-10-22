using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupImage { get; set; }
        [NotMapped]
        public GroupUser[] Users { get; set; }
    }
}
