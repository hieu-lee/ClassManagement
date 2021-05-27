using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement.Models
{
	public class ClassSchedule
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public DayOfWeek Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        [ForeignKey("Classroom")]
        public string ClassroomCode { get; set; }
        public Class Classroom { get; set; }

        [ForeignKey("Owner")]
        public string OwnerUsername { get; set; }
        public Account Owner { get; set; }

        public override bool Equals(object obj)
        {
            var other = (ClassSchedule)obj;
            return ClassroomCode.Equals(other.ClassroomCode) && Day.Equals(other.Day) && StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
        }

        public override int GetHashCode()
        {
            var tuple = new Tuple<string, DayOfWeek, TimeSpan, TimeSpan>(ClassroomCode, Day, StartTime.Value, EndTime.Value);
            return tuple.GetHashCode();
        }
    }
}
