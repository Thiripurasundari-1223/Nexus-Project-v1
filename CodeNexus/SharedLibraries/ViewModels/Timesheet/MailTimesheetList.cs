using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class MailTimesheetList
    {
        public string? ProjectName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan ClockedHours { get; set; }
        public string? Status { get; set; }
        public string Reason { get; set; }
        public int? ProjectId { get; set; }
    }
}
