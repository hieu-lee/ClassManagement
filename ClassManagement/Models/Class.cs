using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Models
{
    public class Class
    {
        [Key]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
#nullable enable
        public string? Address { get; set; }
#nullable disable
        public HashSet<Student> Students { get; set; } = new();
        public HashSet<ClassSchedule> Schedules { get; set; } = new();
        public int NumberOfStudent => Students.Count;
        public override bool Equals(object obj)
        {
            var other = (Class)obj;
            return Code.Equals(other.Code);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
