using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class TimelogView
    {
        public int? EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string Reason { get; set; }
        public bool? Status { get; set; }

    }
}
