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
        public string Name { get; set; }
#nullable enable
        public string? ClassesCodes { get; set; }
#nullable disable
        public string Gender { get; init; }
        public HashSet<Class> Classes { get; set; } = new();
        public DateTime DateOfBirth { get; init; }
        public HashSet<Grade> Grades { get; set; } = new();
        public string GetAllClassesCode()
        {
            if (ClassesCodes is not null)
            {
                return ClassesCodes;
            }
            var res = "";
            var n = Classes.Count;
            var i = 0;
            foreach (var cls in Classes)
            {
                res += (i == n-1)?$"{cls.Code}": $"{cls.Code}, ";
            }
            ClassesCodes = res;
            return res;
        }

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
