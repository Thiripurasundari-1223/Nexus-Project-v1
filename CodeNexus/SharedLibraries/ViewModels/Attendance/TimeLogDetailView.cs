using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class TimeLogDetailView
    {
        public int AttendanceDetailId { get; set; }
        public int EmployeeId { get; set; }
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime CheckoutTime { get; set; }
        public string Reason { get; set; }
        public bool? Status { get; set; }
        public bool? Isregularize { get; set; }
        public int ShiftId { get; set; }
        public int ApproverManagerId { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string RegStatus { get; set; }
    }
}
