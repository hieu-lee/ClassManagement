using ClassManagement.Data;
using ClassManagement.Models;
using Dapper;
using Microsoft.Data.Sqlite;
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
        private readonly string UsernameState;
        public GradesService (AppDbContext dbContext, SessionService session)
        {
            this.dbContext = dbContext;
            UsernameState = session.UsernameState;
        }

        public async Task<Grade> GetGradeAsync(Student student)
        {
            var res = await dbContext.Grades.Where(s => s.StudentId == student.Id && s.OwnerUsername == UsernameState).FirstOrDefaultAsync();
            return res;
        }

        public SortedSet<Grade> GetAllGrades()
        {
            return new(dbContext.Grades.ToArray());
        }

        public Student GetStudentFromName (string studentName)
        {
            var student = dbContext.Students.Where(s => s.Name == studentName && s.OwnerUsername == UsernameState).FirstOrDefault();
            //if (student is null)
            //{
            //    Student newStudent = new() { Name = studentName, Gender = "Male", DateOfBirth = DateTime.Now.Date};
            //    return newStudent;
            //}
            return student;
        }

        public Class GetClassFromCode(string classCode)
        {
            var clss2 = dbContext.Classes.Where(s => s.Code == classCode && s.OwnerUsername == UsernameState).FirstOrDefault();
            return clss2;
        }
        
        public ServiceResult GetGradesFromStudent(Student std)
        {
            var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.StudentId == std.Id && s.OwnerUsername == UsernameState).ToArray());
            return new() { success = true, Grades = grades };
        }

        public async Task<double> GetAverageGradeFromStudentAndClass(string StudentId, string ClassId)
        {
            var grades = await dbContext.Grades.Where(s => s.ClassId == ClassId && s.StudentId == StudentId && s.OwnerUsername == UsernameState).ToArrayAsync();
            return CalculateAverageGrade(grades);
        }

        public async Task<ServiceResult> GetAverageGradesFromStudentAndClasses(ICollection<Class> classes, string StudentId)
        {
            Dictionary<string, double> res = new();
            Dictionary<string, List<Grade>> gradesRes = new();
            var task = Task.Factory.StartNew(() =>
            {
                foreach (var s in classes)
                {
                    gradesRes[s.Id] = new();
                }
            });
            var grades = await dbContext.Grades.Where(s => s.StudentId == StudentId).ToArrayAsync();
            await task;
            foreach (var g in grades)
            {
                gradesRes[g.ClassId].Add(g);
            }
            foreach (var c in classes)
            {
                res[c.Id] = CalculateAverageGrade(gradesRes[c.Id]);
            }
            return new() { success = true, AverageGrades = res };
        }

        public async Task<ServiceResult> GetAverageGradesFromStudentsAndClass(ICollection<Student> students, string ClassId)
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
            var grades = await dbContext.Grades.Where(s => s.ClassId == ClassId && s.OwnerUsername == UsernameState).ToArrayAsync();
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
                var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.OwnerUsername == UsernameState).ToArray());
                return new() { success = true, Grades = grades };
            }

            else if (string.IsNullOrEmpty(studentName))
            {
                Class class1 = GetClassFromCode(classCode);
                var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.ClassCode == class1.Id).ToArray());
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
                var grades = new SortedSet<Grade>(dbContext.Grades.Where(s => s.StudentId == student1.Id && s.ClassCode == class1.Id).ToArray());
                return new() { success = true, Grades = grades };
            }
        }

        public async Task<double> CalculateAverageGradeOfClass(string ClassId)
        {
            var grades = await dbContext.Grades.Where(s => s.ClassId == ClassId).ToArrayAsync();
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

        public async Task<List<string>> GetAllStudentNames()
        {
            var res = await dbContext.Students.Select(s => s.Name).ToListAsync();
            res.Sort();
            return res;
        }

        public SortedSet<Student> GetAllStudents()
        {
            return new (dbContext.Students.Where(s => s.OwnerUsername == UsernameState).ToArray());
        }

        public async Task<ServiceResult> CreateNewGradeAsyncBetter(Grade NewGrade, string ClassCode, Student GStudent)
        {
            ClassStudent res;
            var ClassesId = dbContext.Classes.Where(s => s.Code == ClassCode && s.OwnerUsername == UsernameState).Select(s => s.Id).FirstOrDefault();
            var StudentsId = GStudent.Id;
            using (var dbConnection = new SqliteConnection("Data Source=classmanagement.db"))
            {
                await dbConnection.OpenAsync();
                res = dbConnection.Query<ClassStudent>(@"SELECT ClassesId, StudentsId FROM ClassStudent WHERE ClassesId = @ClassesId AND StudentsId = @StudentsId", new { ClassesId, StudentsId }).FirstOrDefault();
            }
            if (res is null)
            {
                return new() { success = false, err = "Class does not contain student." };
            }
            else
            {
                Class GClass = GetClassFromCode(ClassCode);
                Grade myGrade;
                myGrade = new()
                {
                    Student = GStudent,
                    Classroom = GClass,
                    ClassCode = GClass.Code,
                    GradeinNum = NewGrade.GradeinNum,
                    StdName = GStudent.Name,
                    ExamName = NewGrade.ExamName,
                    ExamTime = NewGrade.ExamTime,
                    RelativeValue = NewGrade.RelativeValue,
                    OwnerUsername = UsernameState
                };
                dbContext.Grades.Add(myGrade);
                await dbContext.SaveChangesAsync();
                return new() { success = true };
            }
        }

        //public async Task<ServiceResult> CreateNewGradeAsyncBetter(Grade NewGrade, string ClassCode, Student GStudent)
        //{
        //    Class GClass = GetClassFromCode(ClassCode);
        //    Grade myGrade;
        //    myGrade = new()
        //        {
        //            Student = GStudent,
        //            Classroom = GClass,
        //            ClassCode = GClass.Code,
        //            GradeinNum = NewGrade.GradeinNum,
        //            StdName = GStudent.Name,
        //            ExamName = NewGrade.ExamName,
        //            ExamTime = NewGrade.ExamTime,
        //            RelativeValue = NewGrade.RelativeValue,
        //            OwnerUsername = UsernameState
        //};
        //dbContext.Grades.Add(myGrade);
        //        await dbContext.SaveChangesAsync();
        //        return new() { success = true };
        //}

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

        public async Task<ServiceResult> DeleteSchedule (string scheduleId)
        {
            var schedule = dbContext.ClassSchedules.Find(scheduleId);
            if (schedule is not null)
            {
                dbContext.ClassSchedules.Remove(schedule);
                await dbContext.SaveChangesAsync();
                return new() { success = true };
            }
            return new() { success = false, err = "Schedule does not exist." };
        }

        public async Task<ServiceResult> DeleteManyGradesAsync(HashSet<Grade> grades)
        {
            dbContext.Grades.RemoveRange(grades);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> DeleteManySchedulesAsync (HashSet<ClassSchedule> schedules)
        {
            dbContext.ClassSchedules.RemoveRange(schedules);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }
        public SortedSet<ClassSchedule> GetAllSchedules()
        {
            return new(dbContext.ClassSchedules.Where(s => s.OwnerUsername == UsernameState).ToArray());
        }

        //public SortedSet<ClassSchedule> GetSchedulesDayOfWeek(int i)
        //{
        //    if (i == 0)
        //        return new(dbContext.ClassSchedules.Where(s => s.Day == DayOfWeek.Monday && s.OwnerUsername == UsernameState).ToArray());
        //    else if (i == 1)
        //        return new(dbContext.ClassSchedules.Where(s => s.Day == DayOfWeek.Tuesday && s.OwnerUsername == UsernameState).ToArray());
        //    else if (i == 2)
        //        return new(dbContext.ClassSchedules.Where(s => s.Day == DayOfWeek.Wednesday && s.OwnerUsername == UsernameState).ToArray());
        //    else if (i == 3)
        //        return new(dbContext.ClassSchedules.Where(s => s.Day == DayOfWeek.Thursday && s.OwnerUsername == UsernameState).ToArray());
        //    else if (i == 4)
        //        return new(dbContext.ClassSchedules.Where(s => s.Day == DayOfWeek.Friday && s.OwnerUsername == UsernameState).ToArray());
        //    else if (i == 5)
        //        return new(dbContext.ClassSchedules.Where(s => s.Day == DayOfWeek.Saturday && s.OwnerUsername == UsernameState).ToArray());
        //    else
        //        return new(dbContext.ClassSchedules.Where(s => s.Day == DayOfWeek.Sunday && s.OwnerUsername == UsernameState).ToArray());

        //}

    }
}
