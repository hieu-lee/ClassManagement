using ClassManagement.Data;
using ClassManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
            var student = dbContext.Students.Where(s => s.Name == studentName).FirstOrDefault();
            if (student is null)
            {
                Student newStudent = new() { Name = studentName, Gender = "Male", DateOfBirth = DateTime.Now.Date};
                return newStudent;
            }
            return student;
        }

        public Class GetClassFromCode (string classCode)
        {
            var clss2 = dbContext.Classes.Where(s => s.Code == classCode).FirstOrDefault();
            if (clss2 is null)
            {
                Class newClass = new() { Code = classCode, Name = "NewCSClass" };
                return newClass;
            }
            return clss2;
            //var classes = dbContext.Classes.Where(s => s.Code == classCode).ToArray();
            //if (classes.Length == 0)
            //{
            //    Class newClass = new() { Code = classCode, Name = "CS" };
            //    return newClass;
            //}
            //return classes[0];
        }
        
        public ServiceResult GetGradesFromStudent(Student std)
        {
            var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.StudentId == std.Id).ToArray());
            return new() { success = true, Grades = grades };
        }

        public async Task<ServiceResult> GetAverageGradesFromStudentsAndClass(ICollection<Student> students, string ClassCode)
        {
            Dictionary<string, double> res = new();
            Dictionary<string, List<Grade>> gradesRes = new();
            var task = Task.Factory.StartNew(() =>
            {
                foreach (var s in students)
                {
                    gradesRes[s.Id] = new();
                }
            });
            var grades = await dbContext.Grades.Where(s => s.ClassCode == ClassCode).ToArrayAsync();
            await task;
            foreach (var g in grades)
            {
                gradesRes[g.StudentId].Add(g);
            }
            foreach (var s in students)
            {
                res[s.Id] = CalculateAverageGrade(gradesRes[s.Id]);
            }
            return new() { success = true, AverageGrades = res };
        }

        public ServiceResult GetGradesFromStudentAndClass(string studentName, string classCode)
        {
            if (string.IsNullOrEmpty(studentName) && string.IsNullOrEmpty(classCode))
            {
                var grades = new SortedSet<Grade>(dbContext.Grades.ToArray());
                return new() { success = true, Grades = grades };
            }

            else if (string.IsNullOrEmpty(studentName))
            {
                Class class1 = GetClassFromCode(classCode);
                var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.ClassCode == class1.Code).ToArray());
                return new() { success = true, Grades = grades };
            }
            else if (string.IsNullOrEmpty(classCode))
            {
                Student student1 = GetStudentFromName(studentName);
                var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.StudentId == student1.Id).ToArray());
                return new() { success = true, Grades = grades };
            }
            else
            {
                Class class1 = GetClassFromCode(classCode);
                Student student1 = GetStudentFromName(studentName);
                var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.StudentId == student1.Id && s.ClassCode == class1.Code).ToArray());
                return new() { success = true, Grades = grades };
            }
        }

        public async Task<double> CalculateAverageGradeOfClass(string ClassCode)
        {
            var grades = await dbContext.Grades.Where(s => s.ClassCode == ClassCode).ToArrayAsync();
            return CalculateAverageGrade(grades);
        }

        public double CalculateAverageGrade(ICollection<Grade> Grades)
        {
            if (Grades.Any())
            {
                double x = 0;
                double y = 0;
                foreach (var grade in Grades)
                {
                    x += grade.GradeinNum * grade.RelativeValue;
                    y += grade.RelativeValue;
                }
                return Math.Round(x / y, 1);
            }
            return 0;
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

        public async Task<List<string>> GetAllStudentNames()
        {
            var res = await dbContext.Students.Select(s => s.Name).ToListAsync();
            res.Sort();
            return res;
        }

        public SortedSet<Student> GetAllStudents()
        {
            return new (dbContext.Students.ToArray());
        }

        public async Task<ServiceResult> CreateNewGradeAsyncBetter(Grade NewGrade, Class GClass, Student GStudent)
        {
            Grade myGrade;
            myGrade = new()
            {
                Student = GStudent,
                Classroom = GClass,
                GradeinNum = NewGrade.GradeinNum,
                StdName = GStudent.Name,
                ExamName = NewGrade.ExamName,
                ExamTime = NewGrade.ExamTime,
                RelativeValue = NewGrade.RelativeValue
            };
            await CreateNewGradeAsync(myGrade);
            GStudent.Grades.Add(myGrade);
            GClass.Grades.Add(myGrade);
            return new() { success = true };

        }

        public async Task<ServiceResult> DeleteGrade (string gradeId)
        {
            var grade = dbContext.Grades.Find(gradeId);
            if (grade is not null)
            {
                dbContext.Grades.Remove(grade);
                await dbContext.SaveChangesAsync();
                return new() { success = true };
            }
            return new() { success = false, err = "Grade does not exist." };
        }

        public async Task<ServiceResult> DeleteManyGradesAsync(HashSet<Grade> grades)
        {
            dbContext.Grades.RemoveRange(grades);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

    }
}
