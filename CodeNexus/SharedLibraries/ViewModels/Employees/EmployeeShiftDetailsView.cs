using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeShiftDetailsView
    {
        public int EmployeeID { get; set; }
        public int? ShiftDetailsId { get; set; }
        public DateTime? ShiftFromDate { get; set; }
        public DateTime? ShiftToDate { get; set; }
        public string ShiftFromTime { get; set; }
        public string ShiftToTime { get; set; }
        public bool? IsFlexyShift { get; set; }
        public List<WeekendDetails> WeekendId { get; set; }
    }
}
