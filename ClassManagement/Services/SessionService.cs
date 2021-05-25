using ClassManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Services
{
    public class SessionService
    {
        public HashSet<ClassSchedule> NotifiedSchedules { get; set; } = new();
        public bool NotificationTimerStarted { get; set; } = false;
    }
}
