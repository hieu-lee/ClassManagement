using ClassManagement.Data;
using ClassManagement.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<ClassSchedule>> GetAllSchedulesAsync()
        {
            return await dbContext.ClassSchedules.ToListAsync();
        }

        public async Task<List<ClassSchedule>> GetScheduleFromDateAsync(DateTime Date)
        {
            return await dbContext.ClassSchedules.Where(s => s.Day == Date.DayOfWeek).ToListAsync();
        }

        public async Task<ClassSchedule> GetOneNextScheduleAsync(DateTime Date, HashSet<ClassSchedule> NotifiedSchedule)
        {
            var time = Date.TimeOfDay.Add(TimeSpan.FromMinutes(15));
            return await dbContext.ClassSchedules.Where(s => (s.Day == Date.DayOfWeek) && (!NotifiedSchedule.Contains(s)) && (s.StartTime.Value <= time)).FirstOrDefaultAsync();
        }
    }
}
