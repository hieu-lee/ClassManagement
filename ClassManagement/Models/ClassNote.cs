using System;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.Models
{
	public class ClassNote : IComparable<ClassNote>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public DateTime Day { get; init; }
        [Required]
        public Class Classroom { get; init; }
        public string Content { get; set; }

        public int CompareTo(ClassNote other)
        {
            return -Day.CompareTo(other.Day);
        }

        public override bool Equals(object obj)
        {
            var other = (ClassNote)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
