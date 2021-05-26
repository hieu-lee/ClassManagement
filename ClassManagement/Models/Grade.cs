﻿using System;
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
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public int HashCode { get; init; }
        [Required]
        public double GradeinNum { get; set; }

        [Required]
        public double RelativeValue { get; set; } = 1;

        [Required]
        public string StdName { get; set; }

        [Required]
        public string ExamName { get; set; }

        [Required]
        public DateTime? ExamTime { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; } = string.Empty;
        public Student Student { get; set; } = new();


        [ForeignKey("Classroom")]
        public string ClassCode { get; set; }

        public string Description { get; set; }
        public Class Classroom { get; set; }

        public Grade()
        {
            HashCode = Id.GetHashCode();
        }

        public int CompareTo (Grade other)
        {
            var c = GradeinNum.CompareTo(other.GradeinNum);
            var res = (c == 0)?Id.CompareTo(other.Id):c;
            return res;
        }
        public override bool Equals(object obj)
        {
            var other = (Grade)obj;
            return Id.Equals(other.Id);
        }
        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}
