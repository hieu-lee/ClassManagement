using System;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.Models
{
	public class ClassSchedule
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public Class ClassRoom { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public override bool Equals(object obj)
        {
            var other = (ClassSchedule)obj;
            return ClassRoom.Code.Equals(other.ClassRoom.Code) && Day.Equals(other.Day) && StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
        }

        public override int GetHashCode()
        {
            var tuple = new Tuple<string, DayOfWeek, TimeSpan, TimeSpan>(ClassRoom.Code, Day, StartTime, EndTime);
            return tuple.GetHashCode();
        }
    }
}
