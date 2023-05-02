using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Attendance
{
    public class ShiftDetails
    {
        [Key]
        public int ShiftDetailsId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftCode { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }
        public string ShiftDescription { get; set; }
        public int EmployeeGroupId { get; set; }
        public bool OverTime { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsFlexyShift { get; set; }
    }
}