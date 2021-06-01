using ClassManagement.Data;
using ClassManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Services
{
    public class ClassesService
    {
        private readonly AppDbContext dbContext;
        private readonly string UsernameState;

        public ClassesService(AppDbContext dbContext, SessionService session)
        {
            this.dbContext = dbContext;
            UsernameState = session.UsernameState;
        }

        public SortedSet<Class> GetAllClasses()
        {
            return new(dbContext.Classes.Where(s => s.OwnerUsername == UsernameState).Include(s => s.Schedules).ToArray());
        }

        public async Task<List<string>> GetAllClassCodes()
        {
            var res = await dbContext.Classes.Where(s => s.OwnerUsername == UsernameState).Select(s => s.Code).ToListAsync();
            res.Sort();
            return res;
        }

        public async Task<Class> GetClassAsync(string ClassId)
        {
            var res = await dbContext.Classes.Where(s => s.Id == ClassId && s.OwnerUsername == UsernameState).Include(s => s.Schedules).Include(s => s.Students).AsSplitQuery().FirstOrDefaultAsync();
            return res;
        }

        public ServiceResult GetClassesFromDay(DayOfWeek Day)
        {
            var classes = new SortedSet<Class>();
            var dayClasses = dbContext.ClassSchedules.Where(s => s.Day == Day && s.OwnerUsername == UsernameState).ToArray();
            for (int i = 0; i < dayClasses.Length; i++)
            {
                classes.Add(dayClasses[i].Classroom);
            }
            return new() { success = true, Classes = classes };
        }

        public ServiceResult GetAllStudents()
        {
            var students = new SortedSet<Student>(dbContext.Students.Where(s => s.OwnerUsername == UsernameState).Include(s => s.Classes).ToArray());
            return new() { success = true, Students = students };
        }
        public async Task<ServiceResult> GetStudentsFromOtherClasses(string ClassId)
        {
            var task = dbContext.Classes.FindAsync(ClassId);
            var AllStudents = dbContext.Students.Where(s => s.OwnerUsername == UsernameState).Include(s => s.Classes).ToArray();
            var students = new SortedSet<Student>();
            var cls = await task;
            foreach (var s in AllStudents)
            {
                if (!s.Classes.Contains(cls))
                {
                    students.Add(s);
                }
            }
            return new() { success = true, Students = students };
        }

        public async Task<ServiceResult> GetStudentAsync(string StudentId)
        {
            var res = await dbContext.Students.Where(s => s.Id == StudentId && s.OwnerUsername == UsernameState).Include(s => s.Classes).Include(s => s.Grades).AsSplitQuery().FirstOrDefaultAsync();
            return new() { svStudent = res };
        }

        public async Task<ServiceResult> GetAllNotesFromClass(string ClassId)
        {
            var res = await dbContext.ClassNotes.Where(s => s.ClassroomCode == ClassId && s.OwnerUsername == UsernameState).ToArrayAsync();
            var notes = new SortedSet<ClassNote>(res);
            return new() { success = true, Notes = notes };
        }

        public ServiceResult GetNotesFromDay(DateTime Day)
        {
            var notes = dbContext.ClassNotes.Where(s => s.Day == Day && s.OwnerUsername == UsernameState).ToHashSet();
            return new() { success = true, DayNotes = notes };
        }

        public async Task<Grade[]> GetGradesFromClassAsync(string ClassId)
        {
            var res = await dbContext.Grades.Where(s => s.ClassId == ClassId && s.OwnerUsername == UsernameState).ToArrayAsync();
            return res;
        }

        public async Task<Grade[]> GetGradesFromStudentAsync(string StudentId)
        {
            var res = await dbContext.Grades.Where(s => s.StudentId == StudentId && s.OwnerUsername == UsernameState).ToArrayAsync();
            return res;
        }

        public async Task<ServiceResult> CreateNewClass(Class NewClass)
        {
            var Class = dbContext.Classes.Where(s => s.Code == NewClass.Code && s.OwnerUsername == UsernameState).FirstOrDefault();
            if (Class is not null)
            {
                return new() { success = false, err = "Class has already existed" };
            }
            NewClass.OwnerUsername = UsernameState;
            dbContext.Classes.Add(NewClass);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> CreateNewStudent(Student student, HashSet<string> codes)
        {
            var classes = dbContext.Classes.Where(s => s.OwnerUsername == UsernameState && codes.Contains(s.Code)).ToHashSet();
            student.Classes = classes;
            student.OwnerUsername = UsernameState;
            dbContext.Students.Add(student);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> AddNewStudentToClass(Student student, string ClassId)
        {
            var task = dbContext.Classes.Where(s => s.Id == ClassId && s.OwnerUsername == UsernameState).FirstOrDefaultAsync();
            student = dbContext.Students.Find(student.Id);
            var Class = await task;
            if (Class is null)
            {
                return new() { success = false, err = "Class does not exist" };
            }
            student.Classes.Add(Class);
            Class.Students.Add(student);
            dbContext.Classes.Update(Class);
            dbContext.Students.Update(student);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> AddNewScheduleToClass(ClassSchedule schedule, string ClassId)
        {
            schedule.OwnerUsername = UsernameState;
            var Class = dbContext.Classes.Where(s => s.Id == ClassId && s.OwnerUsername == UsernameState).FirstOrDefault();
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
            note.OwnerUsername = UsernameState;
            dbContext.ClassNotes.Add(note);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> UpdateClass(Class Class)
        {
            var OldClass = dbContext.Classes.Find(Class.Id);
            if (OldClass is null)
            {
                return new() { success = false, err = "Class does not exist" };
            }
            OldClass = Class;
            dbContext.Classes.Update(OldClass);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> DeleteStudentFromClass(string studentId, string ClassId)
        {
            var Class = dbContext.Classes.Where(s => s.Id == ClassId).Include(s => s.Students).FirstOrDefault();
            var Student = dbContext.Students.Where(s => s.Id == studentId).Include(s => s.Classes).FirstOrDefault();
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
        public async Task<ServiceResult> DeleteManyStudentsFromClass(HashSet<Student> students, string ClassId)
        {
            var Class = dbContext.Classes.Where(s => s.Id == ClassId).Include(s => s.Students).FirstOrDefault();
            if (Class is null)
            {
                return new() { success = false, err = "Invalid Class" };
            }
            foreach (var std in students)
            {
                var Student = dbContext.Students.Where(s => s.Id == std.Id).Include(s => s.Classes).FirstOrDefault();
                Class.Students.Remove(Student);
                Student.Classes.Remove(Class);
                dbContext.Students.Update(Student);
            }
            dbContext.Classes.Update(Class);
            await dbContext.SaveChangesAsync();
            return new() { success = true };
        }

        public async Task<ServiceResult> DeleteClass(string ClassId)
        {
            var Class = dbContext.Classes.Find(ClassId);
            if (Class is not null)
            {
                var task = Task.Factory.StartNew(() => { dbContext.ClassNotes.RemoveRange(Class.Notes); });
                var task1 = Task.Factory.StartNew(() => { dbContext.ClassSchedules.RemoveRange(Class.Schedules); });
                await task;
                await task1;
                dbContext.Classes.Remove(Class);
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
                await dbContext.SaveChangesAsync();
                return new() { success = true };
            }
            return new() { success = false, err = "The student doesn't exist" };
        }

        public async Task<ServiceResult> DeleteNoteAsync(string NoteId)
        {
            var Note = await dbContext.ClassNotes.Where(s => s.Id == NoteId && s.OwnerUsername == UsernameState).FirstOrDefaultAsync();
            if (Note is not null)
            {
                dbContext.ClassNotes.Remove(Note);
                await dbContext.SaveChangesAsync();
                return new() { success = true };
            }
            return new() { success = false, err = "Note doesn not exist" };
        }

        public async Task<ServiceResult> DeleteSchedule(string ScheduleId)
        {
            var Schedule = dbContext.ClassSchedules.Find(ScheduleId);
            if (Schedule is not null)
            {
                var Class = dbContext.Classes.Find(Schedule.ClassroomId);
                Class.Schedules.Remove(Schedule);
                dbContext.ClassSchedules.Remove(Schedule);
                dbContext.Classes.Update(Class);
                await dbContext.SaveChangesAsync();
                return new() { success = true };
            }
            return new() { success = false, err = "Schedule doesn't exist" };
        }
    }
}
