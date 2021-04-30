using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.Models
{
	public class Class : IComparable<Class>
    {
        [Key]
        [Required]
        [StringLength(6, ErrorMessage = "Code leng", MinimumLength = 6)]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string DeleteHeight { get; set; } = "0";
        public string Description { get; set; } = string.Empty;
#nullable enable
        public string? Address { get; set; }
#nullable disable
        public HashSet<Student> Students { get; set; } = new();
        public HashSet<ClassSchedule> Schedules { get; set; } = new();
        public int NumberOfStudent => Students.Count;
        public string ImgSource => StyleConsts.ImgSourceLookUp[DeleteHeight];

        public int CompareTo(Class other)
        {
            return Name.CompareTo(other.Name);
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
