using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement.Models
{
    public class Student : IComparable<Student>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
#nullable enable
        public string ClassesCodesToString => GetAllClassesCode();
        public string? Description { get; set; }
#nullable disable
        [Required]
        public string Gender { get; set; }
        public HashSet<Class> Classes { get; set; } = new();
        [Required]
        public DateTime? DateOfBirth { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Value.Year;
        public HashSet<Grade> Grades { get; set; } = new();

        [ForeignKey("Owner")]
        public string OwnerUsername { get; set; }
        public Account Owner { get; set; }
        public string GetAllClassesCode()
        {
            var res = "";
            var n = Classes.Count;
            var i = 0;
            foreach (var cls in Classes)
            {
                res += (i == n-1)?$"{cls.Code}": $"{cls.Code}, ";
                i++;
            }
            return res;
        }

        public int CompareTo(Student other)
        {
            return Name.CompareTo(other.Name);
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
