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

        public ServiceResult GetClasses(DayOfWeek? Day = null)
        {
            HashSet<Class> classes = new();
            if (Day is not null)
            {
                classes = dbContext.Classes.Where(s => true).ToHashSet();
                return new() { success = true, Classes = classes };
            }
            else
            {
                var schedules = dbContext.ClassSchedules.Where(s => s.Day == Day.Value).ToArray();
                HashSet<ClassSchedule> classSchedules = new();
                Parallel.ForEach(schedules, schedule =>
                {
                    var Class = dbContext.Classes.Find(schedule.ClassRoom.Code);
                    schedule.ClassRoom = Class;
                    classSchedules.Add(schedule);
                });
                return new() { success = true, Schedules = classSchedules };
            }
        }

        public ServiceResult GetStudents(Class ClassRoom = null)
        {
            HashSet<Student> students = new();
            if (ClassRoom is not null)
            {
                students = dbContext.Students.Where(s => s.Classes.Contains(ClassRoom)).ToHashSet();
            }
            else
            {
                students = dbContext.Students.Where(s => true).ToHashSet();
            }
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
    }
}
