using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class WeeklyOverviewReportView
    {
        //Weekly Overview
        public class LeaveData
        {
            public int LeaveTypeId { get; set; }
            public int? EmployeeId { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public decimal? NoOfDays { get; set; }
            public decimal? TotalLeaves { get; set; }
        }
        public class HolidaysData
        {
            public int EmployeeDepartmentId { get; set; }
            public DateTime? HolidayDate { get; set; }
            public int? Year { get; set; }
        }
        public class WeeklyLeaveHolidayOverview
        {
            public List<LeaveData> LeaveData { get; set; }
            public List<HolidaysData> HolidaysData { get; set; }
        }
        public class WeeklyOverview
        {
            public string Type { get; set; }
            public string Hours { get; set; }
            public decimal? Percentage { get; set; }
        }
    }
}
