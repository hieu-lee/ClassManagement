using ClassManagement.Data;
using ClassManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Services
{
    public class SchedulesService
    {
        private readonly AppDbContext dbContext;

        public SchedulesService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<ClassSchedule> GetAllSchedules()
        {
            return dbContext.ClassSchedules.ToList();
        }

        public List<ClassSchedule> GetScheduleFromDate(DateTime Date)
        {
            return dbContext.ClassSchedules.Where(s => s.Day == Date.DayOfWeek).ToList();
        }

        public ClassSchedule GetOneNextSchedule(DateTime Date, HashSet<ClassSchedule> NotifiedSchedule)
        {
            var time = Date.TimeOfDay.Add(TimeSpan.FromMinutes(15));
            return dbContext.ClassSchedules.Where(s => (s.Day == Date.DayOfWeek) && (!NotifiedSchedule.Contains(s)) && (s.StartTime.Value <= time)).FirstOrDefault();
        }
    }
}
