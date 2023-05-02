using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class AttendanceView
    {
        public int EmployeeId { get; set; }
        public DateTime? CheckinCheckoutTime { get; set; }
        public bool IsCheckin { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime Date { get; set; }
        public int ShiftId { get; set; }
    }
}
