using ClassManagement.Data;
using ClassManagement.Models;
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
    }
}
