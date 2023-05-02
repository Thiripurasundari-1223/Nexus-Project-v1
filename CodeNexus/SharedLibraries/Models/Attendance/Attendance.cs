using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedLibraries.Models.Attendance
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public TimeSpan? TotalHours { get; set; }
        public TimeSpan? BreakHours { get; set; }
        public bool IsCheckin { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
