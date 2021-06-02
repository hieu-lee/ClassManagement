using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassManagement.Models
{
	public class ClassSchedule : IComparable<ClassSchedule>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public DayOfWeek Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        [ForeignKey("Classroom")]
        public string ClassroomId{ get; set; }
        public string ClassCode { get; set; }
        public Class Classroom { get; set; }

        [ForeignKey("Owner")]
        public string OwnerUsername { get; set; }
        public Account Owner { get; set; }

        public override bool Equals(object obj)
        {
            var other = (ClassSchedule)obj;
            return ClassCode.Equals(other.ClassCode) && OwnerUsername.Equals(other.OwnerUsername) && Day.Equals(other.Day) && StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
        }

        public override int GetHashCode()
        {
            var tuple = new Tuple<string, string, DayOfWeek, TimeSpan, TimeSpan>(ClassCode, OwnerUsername, Day, StartTime.Value, EndTime.Value);
            return tuple.GetHashCode();
        }

        public int CompareTo(ClassSchedule other)
        {
            var r = Day.CompareTo(other.Day);
            if (r != 0) return r;
            r = StartTime.Value.CompareTo(other.StartTime.Value);
            if (r != 0) return r;
            r = EndTime.Value.CompareTo(other.EndTime.Value);
            if (r != 0) return r;
            r = Id.CompareTo(other.Id);
            return r;
        }
    }
}
