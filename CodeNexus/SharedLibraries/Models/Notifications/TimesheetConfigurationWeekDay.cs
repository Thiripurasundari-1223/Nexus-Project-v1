using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Notifications
{
    public class TimesheetConfigurationWeekDay
    {
        [Key]
        public int TimesheetConfigurationWeekdayId { get; set; }
        public string DayName { get; set; }
    }
}
