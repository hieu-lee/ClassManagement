using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement.Models
{
    public class Grade : IComparable<Grade>
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public double GradeinNum { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Classroom")]
        public string ClassCode { get; set; }
        public Class Classroom { get; set; }

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
    }
}
