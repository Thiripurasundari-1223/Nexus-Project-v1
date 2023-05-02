using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class TimesheetConfigurationView
    {
        public int TimesheetConfigurationId { get; set; }
        public int TimesheetSubmissionDayId { get; set; }
        public string TimesheetSubmissionTime { get; set; }
        public int TimesheetAlertSubmissionFromDayId { get; set; }
        public int TimesheetAlertSubmissionToDayId { get; set; }
        public int TimesheetApprovalFromDayId { get; set; }
        public int TimesheetApprovalToDayId { get; set; }
        public int TimesheetAlertApprovalFromDayId { get; set; }
        public int TimesheetAlertApprovalToDayId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
