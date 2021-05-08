using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.Models
{
	public class Student : IComparable<Student>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; init; }
        public HashSet<Class> Classes { get; set; } = new();
        public DateTime DateOfBirth { get; init; }
        public HashSet<Grade> Grades { get; set; } = new();

        public int CompareTo(Student other)
        {
            return Name.CompareTo(other);
        }

        public override bool Equals(object obj)
        {
            var other = (Student)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
