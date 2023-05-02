using SharedLibraries.Models.Leaves;
using SharedLibraries.Models.Timesheet;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Leaves;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels
{
    public class ResourceTimesheet
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TotalProjectClockedHours { get; set; }
        public bool IsSubmited { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public List<TimesheetLogView> TimesheetLog { get; set; }
        public bool? IsBillable { get; set; }
        public int? TimesheetId { get; set; }
        public int? RejectionReasonId { get; set; }
        public string RejectionReason { get; set; }
        public string OtherReasonForRejection { get; set; }
        //public string Comments { get; set; }
    }

    public class TimesheetSet
    {
        public List<ResourceTimesheet> LstResourceTimesheet { get; set; }
        public List<string> TotalDayHours { get; set; }
        public string TotalClockedHours { get; set; }
        public bool IsSubmited { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }

    }
    public class ResourceWeeklyTimesheet
    {
        public List<TimesheetSet> LstTimesheetSet { get; set; }
        public bool IsSubmited { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string Status { get; set; }
        public List<ResourceProjectList> ListOfProject { get; set; }
        public List<ResourceTimeList> ResourceTimeList { get; set; }
        public List<WeeklyTimesheetComments> WeeklyTimesheetComments { get; set; }
        public string Comments { get; set; }
        public bool IsEnableApprove { get; set; }
        public bool IsEnableSubmit { get; set; }
        public List<EmployeeAttendanceHours> EmployeeAttendanceHours { get; set; }
        public List<Holiday> EmployeeHolidayList { get; set; }
        public List<EmployeeLeavesForTimeSheetView> EmployeeLeaveList{ get; set; }
        public TimeandWeekendDefinitionView EmployeeShiftTiming { get; set; }
        public string TimesheetSubmissionTime { get; set; }
        public int? TimesheetSubmissionDayId { get; set; }
        public int CurrentDayId { get; set; }
    }
    public class WeeklyTimesheetComments
    {
        public int? TimesheetCommentsId { get; set; }
        public int? TimesheetId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string Comments { get; set; }
    }
}