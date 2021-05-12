using ClassManagement.Data;
using ClassManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Services
{
    public class GradesService
    {
        private readonly AppDbContext dbContext;
        public GradesService (AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Grade> GetGradeAsync(string id)
        {
            var res = await dbContext.Grades.Where(s => s.Id == id).Include(s => s.Student).FirstOrDefaultAsync();
            return res;
        }

        public SortedSet<Grade> GetAllGrades()
        {
            return new(dbContext.Grades.Include(s => s.Id).ToArray());
        }

        public ServiceResult GetGradesFromStudent(Student std)
        {
            var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.StudentId == std.Id).ToArray());
            return new() { success = true, Grades = grades };
        }

        public async Task<ServiceResult> CreateNewGrade(Grade NewGrade)
        {
            var Grade = dbContext.Grades.Find(NewGrade.Id);
            if (Grade is not null)
            {
                return new() { success = false, err = "Grade already existed" };
            }
            dbContext.Grades.Add(NewGrade);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

    }
}
