using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Models
{
    public class ClassNote
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public DateTime Day { get; init; }
        [Required]
        public Class Classroom { get; init; }
        public string Content { get; set; }
    }
}
