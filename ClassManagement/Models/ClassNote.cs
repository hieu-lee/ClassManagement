using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement.Models
{
	public class ClassNote : IComparable<ClassNote>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public int HashCode { get; init; }
        [Required]
        public DateTime Day { get; init; }
        public string Content { get; set; }
        [ForeignKey("Classroom")]
        public string ClassroomCode { get; set; }
        [Required]
        public Class Classroom { get; init; }

        public ClassNote()
        {
            HashCode = Id.GetHashCode();
        }

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
            return HashCode;
        }
    }
}
