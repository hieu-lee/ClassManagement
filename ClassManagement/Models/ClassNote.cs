using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement.Models
{
	public class ClassNote : IComparable<ClassNote>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public DateTime? Day { get; set; }
        public string Content { get; set; }
        [ForeignKey("Classroom")]
        public string ClassroomCode { get; set; }
        [Required]
        public Class Classroom { get; init; }

        [ForeignKey("Owner")]
        public string OwnerUsername { get; set; }
        public Account Owner { get; init; }

        public int CompareTo(ClassNote other)
        {
            return -Day.Value.CompareTo(other.Day.Value);
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
