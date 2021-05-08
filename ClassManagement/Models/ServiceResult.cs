using System.Collections.Generic;

namespace ClassManagement.Models
{
	public class ServiceResult
    {
        public bool success { get; set; }
        public string err { get; set; }
        public SortedSet<Class> Classes { get; set; }
        public SortedSet<Student> Students { get; set; }
        public SortedSet<ClassNote> Notes { get; set; }
        public HashSet<ClassNote> DayNotes { get; set; }
        public HashSet<ClassSchedule> Schedules { get; set; }

        public SortedSet<Grade> Grades { get; set; }
    }
}
