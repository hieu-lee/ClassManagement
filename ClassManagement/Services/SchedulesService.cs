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

        public async Task<List<ClassSchedule>> GetSchedulesFromDateAsync(DateTime Date)
        {
            return await dbContext.ClassSchedules.Where(s => s.Day == Date.DayOfWeek).ToListAsync();
        }
    }
}
