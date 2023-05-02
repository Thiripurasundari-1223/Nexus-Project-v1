using System;

namespace SharedLibraries.ViewModels
{
    public class TeamTimesheet
    {
        public string UserName { get; set; }
        public string RequiredHours { get; set; }
        public string ClockedHours { get; set; }
        public int UserId { get; set; }
        public int ReportingPersonId { get; set; }
        public string Destination { get; set; }
        public string Status { get; set; }
        public int? TimesheetId { get; set; }
        public int? RejectionReasonId { get; set; }
        public string OtherReasonForRejection { get; set; }
        public string shiftHours { get; set; }
    }
}