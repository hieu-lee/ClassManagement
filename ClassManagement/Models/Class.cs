using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement.Models
{
	public class Class : IComparable<Class>
    {
        [Key]
        [Required]
        [StringLength(10, ErrorMessage = "Code length must be between 6 and 10 characters", MinimumLength = 6)]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
#nullable enable
        public string? Address { get; set; }
#nullable disable
        public HashSet<Student> Students { get; set; } = new();
        public HashSet<Grade> Grades { get; set; } = new();
        public HashSet<ClassSchedule> Schedules { get; set; } = new();
        public HashSet<ClassNote> Notes { get; set; } = new();

        [ForeignKey("Owner")]
        public string OwnerUsername { get; set; }
        public Account Owner { get; set; }
        public int NumberOfStudent => Students.Count;

        public int CompareTo(Class other)
        {
            var r = Name.CompareTo(other.Name);
            int res;
            res = (r != 0)?r:Code.CompareTo(other.Code);
            return res;
        }

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
