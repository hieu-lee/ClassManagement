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

        public async Task<Grade> GetGradeAsync(Student student)
        {
            var res = await dbContext.Grades.Where(s => s.StudentId == student.Id).FirstOrDefaultAsync();
            return res;
        }

        public SortedSet<Grade> GetAllGrades()
        {
            return new(dbContext.Grades.ToArray());
        }

        public Student GetStudentFromName (string studentName)
        {
            var students = dbContext.Students.Where (s => s.Name == studentName).ToArray();
            if (students.Length==0)
            {
                Student newStudent = new() { Name = studentName };
                return newStudent;
            }
            return students[0];
        }
        
        public ServiceResult GetGradesFromStudent(Student std)
        {
            var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.StudentId == std.Id).ToArray());
            return new() { success = true, Grades = grades };
        }

        public async Task<ServiceResult> CreateNewGradeAsync(Grade NewGrade)
        {
            Student student;
            Class classroom;
            var Grade = dbContext.Grades.Find(NewGrade.Id);
            if (Grade != null)
            {
                return new() { success = false, err = "Grade already existed" };
            }
            if (NewGrade.Student.Id != null)
            {
                student = dbContext.Students.Find(NewGrade.Student.Id);
                if (student is not null)
                {
                    NewGrade.Student = student;
                }
            }
            if (NewGrade.Classroom.Code != null)
            {
                classroom = dbContext.Classes.Find(NewGrade.Classroom.Code);
                if (classroom is not null)
                {
                    NewGrade.Classroom = classroom;
                }
            }
            dbContext.Grades.Add(NewGrade);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

    }
}
