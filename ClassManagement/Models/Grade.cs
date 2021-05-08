using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.Models
{
    public class Grade : IComparable<Grade>
    {
        [Key]
        [Required]
        [StringLength(10, ErrorMessage = "Code leng", MinimumLength = 6)]
        public string Id { get; set; }

        [Required]
        public double GradeinNum { get; set; }

        public HashSet<Student> Students { get; set; } = new();

        public HashSet<Class> Classes { get; set; } = new();

        public int CompareTo (Grade other)
        {
            return GradeinNum.CompareTo(other.GradeinNum);
        }
        public override bool Equals(object obj)
        {
            var other = (Grade)obj;
            return Id.Equals(other.Id);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        [Required]
        public string StudentName { get; set; }

        public HashSet<Grade> Grades { get; set; } = new();

    }
}
