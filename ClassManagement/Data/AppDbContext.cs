using ClassManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ClassNote> ClassNotes { get; set; }

        public DbSet<Grade> Grades { get; set; }
        
        public DbSet<Account> Accounts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
    }
}
