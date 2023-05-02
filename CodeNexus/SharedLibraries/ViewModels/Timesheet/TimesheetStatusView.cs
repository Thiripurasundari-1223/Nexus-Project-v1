using System;

namespace SharedLibraries.ViewModels
{
    public class TimesheetStatusView
    {
        public int TimesheetId { get; set; }
        public bool IsBillable { get; set; }
        public bool IsApproved { get; set; }
        public int RejectionReasonId { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }
        public int ResourceId { get; set; }
        public DateTime? WeekStartDate { get; set; }
        public string ProjectName { get; set; }
    }
}