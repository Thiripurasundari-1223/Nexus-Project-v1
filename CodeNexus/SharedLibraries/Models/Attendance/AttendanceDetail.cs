using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedLibraries.Models.Attendance
{
    [Table("AttendanceDetail")]
    public class AttendanceDetail
    {
        [Key]
        public int AttendanceDetailId { get; set; }
        public int AttendanceId { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public TimeSpan? TotalHours { get; set; }
        public TimeSpan? BreakHours { get; set; }
        public DateTime? BreakoutTime { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public bool? Isregularize { get; set; }
        public string RejectReason { get; set; }
        public int? ManagerId { get; set; }
    }
}
