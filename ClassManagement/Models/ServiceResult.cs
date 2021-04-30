using System.Collections.Generic;

namespace ClassManagement.Models
{
	public class ServiceResult
    {
        public bool success { get; set; }
        public string err { get; set; }
        public HashSet<Class> Classes { get; set; }
        public HashSet<Student> Students { get; set; }
        public HashSet<ClassNote> Notes { get; set; }
        public HashSet<ClassSchedule> Schedules { get; set; }
    }
}
