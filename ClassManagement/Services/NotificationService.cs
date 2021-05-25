using ClassManagement.Models;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace ClassManagement.Services
{
    public class NotificationService
    {
        SessionService session; 
        ISnackbar Snackbar;
        Timer notificationTimer = new(30000);
        NavigationManager navigationManager;
        public NotificationService(SessionService session, ISnackbar Snackbar, NavigationManager navigationManager)
        {
            this.session = session;
            this.Snackbar = Snackbar;
            this.navigationManager = navigationManager;
            if (!session.NotificationTimerStarted)
            {
                notificationTimer.Enabled = true;
                notificationTimer.Elapsed += async (s, e) =>
                {
                    await PushNotification();
                };
                session.NotificationTimerStarted = false;
            }
        }
        public async Task<ClassSchedule> GetOneNextScheduleAsync(DateTime Date, HashSet<ClassSchedule> NotifiedSchedule)
        {
            var time1 = Date.TimeOfDay;
            var time2 = Date.TimeOfDay.Add(TimeSpan.FromMinutes(15));
            var Day = Date.DayOfWeek;
            ClassSchedule schedule;
            using (var dbConnection = new SqliteConnection("Data Source=classmanagement.db"))
            {
                await dbConnection.OpenAsync();
                var schedules = dbConnection.Query<ClassSchedule>(@"SELECT Id, HashCode, Day, StartTime, EndTime, ClassroomCode FROM ClassSchedules WHERE Day = @Day", new { Day }).ToList();
                schedule = schedules.Where(s => (!NotifiedSchedule.Contains(s)) && (time1 <= s.StartTime.Value) && (s.StartTime.Value <= time2)).FirstOrDefault();
            }
            return schedule;
        }
        public async Task PushNotification()
        {
            var schedule = await GetOneNextScheduleAsync(DateTime.Now, session.NotifiedSchedules);
            if (schedule is not null)
            {
                Snackbar.Add($"{schedule.ClassroomCode} starts at {schedule.StartTime}", Severity.Info, config =>
                {
                    config.Onclick = snackbar =>
                    {
                        navigationManager.NavigateTo("/Classes");
                        return Task.CompletedTask;
                    };
                });
                session.NotifiedSchedules.Add(schedule);
            }
        }
    }
}
