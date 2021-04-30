using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.Models
{
	public class Class
    {
        [Key]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string DeleteHeight { get; set; } = "0";
        public string Description { get; set; } = string.Empty;
#nullable enable
        public string? Address { get; set; }
#nullable disable
        public HashSet<Student> Students { get; set; } = new();
        public HashSet<ClassSchedule> Schedules { get; set; } = new();
        public int NumberOfStudent => Students.Count;
        private Dictionary<string, string> ImgSourceLookUp = new()
        {
            ["0"] = "down_arrow.png",
            ["30px"] = "up_arrow.png"
        };
        public string ImgSource => ImgSourceLookUp[DeleteHeight];
        public override bool Equals(object obj)
        {
            var other = (Class)obj;
            return Code.Equals(other.Code);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
