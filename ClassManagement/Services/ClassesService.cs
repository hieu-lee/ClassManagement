using ClassManagement.Data;
using ClassManagement.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Services
{
    public class ClassesService
    {
        private AppDbContext dbContext;
        public ClassesService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ServiceResult GetAllClasses(DayOfWeek? Day = null)
        {
            HashSet<Class> classes = new();
            classes = dbContext.Classes.Where(s => true).ToHashSet();
            return new() { success = true, Classes = classes };
        }

        public ServiceResult GetClassesFromDay(DayOfWeek Day)
        {
            var schedules = dbContext.ClassSchedules.Where(s => s.Day == Day).ToArray();
            HashSet<ClassSchedule> classSchedules = new();
            Parallel.ForEach(schedules, schedule =>
            {
                var Class = dbContext.Classes.Find(schedule.ClassRoom.Code);
                schedule.ClassRoom = Class;
                classSchedules.Add(schedule);
            });
            return new() { success = true, Schedules = classSchedules };
        }

        public ServiceResult GetAllStudents()
        {
            var students = dbContext.Students.Where(s => true).ToHashSet();
            return new() { success = true, Students = students };
        }

        public ServiceResult GetStudentsFromClass(Class ClassRoom)
        {
            var students = dbContext.Students.Where(s => s.Classes.Contains(ClassRoom)).ToHashSet();
            return new() { success = true, Students = students };
        }

        public ServiceResult GetAllNotes()
        {
            var notes = dbContext.ClassNotes.Where(s => true).ToHashSet();
            return new() { success = true, Notes = notes };
        }

        public ServiceResult GetNotesFromDay(DateTime Day)
        {
            HashSet<ClassNote> notes = new();
            notes = dbContext.ClassNotes.Where(s => s.Day == Day).ToHashSet();
            return new() { success = true, Notes = notes };
        }

        public async Task<ServiceResult> CreateNewClass(Class NewClass)
        {
            var Class = dbContext.Classes.Find(NewClass.Code);
            if (Class is not null)
            {
                return new() { success = false, err = "Class has already existed" };
            }
            dbContext.Classes.Add(NewClass);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> AddNewStudentToClass(Student student, string ClassCode)
        {
            var Class = dbContext.Classes.Find(ClassCode);
            student = dbContext.Students.Find(student.Id);
            if (Class is null)
            {
                return new() { success = false, err = "Class does not exist" };
            }
            if (Class.Students.Contains(student))
            {
                return new() { success = false, err = "Student has already been in class" };
            }
            student.Classes.Add(Class);
            Class.Students.Add(student);
            dbContext.Classes.Update(Class);
            dbContext.Students.Update(student);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> AddNewScheduleToClass(ClassSchedule schedule, string ClassCode)
        {
            var Class = dbContext.Classes.Find(ClassCode);
            if (Class is null)
            {
                return new() { success = false, err = "Class does not exist" };
            }
            if (Class.Schedules.Contains(schedule))
            {
                return new() { success = false, err = "Class has already occurred on given schedule" };
            }
            Class.Schedules.Add(schedule);
            dbContext.Classes.Update(Class);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> AddNewNote(ClassNote note)
        {
            dbContext.ClassNotes.Add(note);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> UpdateClass(Class Class)
        {
            var OldClass = dbContext.Classes.Find(Class.Code);
            if (OldClass is null)
            {
                return new() { success = false, err = "Class does not exist" };
            }
            OldClass = Class;
            dbContext.Classes.Update(OldClass);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> DeleteStudentFromClass(string studentId, string ClassCode) 
        {
            var Class = dbContext.Classes.Find(ClassCode);
            var Student = dbContext.Students.Find(studentId);
            if (Class is null)
            {
                return new() { success = false, err = "Invalid Class" };
            }
            if (Student is null)
            {
                return new() { success = false, err = "Invalid Student" };
            }
            Class.Students.Remove(Student);
            Student.Classes.Remove(Class);
            dbContext.Classes.Update(Class);
            dbContext.Students.Update(Student);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> DeleteClass(string ClassCode)
        {
            var Class = dbContext.Classes.Find(ClassCode);
            if (Class is not null)
            {
                dbContext.Classes.Remove(Class);
                using (var connection = new SqliteConnection("Data Source=app.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"DELETE FROM ClassStudent WHERE ClassesCode = $id";
                    command.Parameters.AddWithValue("$id", ClassCode);
                    command.ExecuteNonQuery();
                }
                await dbContext.SaveChangesAsync();
                return new() { success = true };
            }
            return new() { success = false, err = "The class doesn't exist" };
        }

        public async Task<ServiceResult> DeleteStudent(string StudentId)
        {
            var Student = dbContext.Students.Find(StudentId);
            if (Student is not null)
            {
                dbContext.Students.Remove(Student);
                using (var connection = new SqliteConnection("Data Source=app.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"DELETE FROM ClassStudent WHERE StudentsCode = $id";
                    command.Parameters.AddWithValue("$id", StudentId);
                    command.ExecuteNonQuery();
                }
                await dbContext.SaveChangesAsync();
                return new() { success = true };
            }
            return new() { success = false, err = "The student doesn't exist" };
        }

        
    }
}
