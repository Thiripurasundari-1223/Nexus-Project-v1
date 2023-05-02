using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Attendance
{
    public class TimeDefinition
    {
        [Key]
        public int TimeDefinitionId { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }
        public TimeSpan? BreakTime { get; set; }
        public TimeSpan? TotalHours { get; set; }
        public TimeSpan? AbsentFromHour { get; set; }
        public string AbsentFromOperator { get; set; }
        public TimeSpan? AbsentToHour { get; set; }
        public string AbsentToOperator { get; set; }
        public TimeSpan? HalfaDayFromHour { get; set; }
        public string HalfaDayFromOperator { get; set; }
        public TimeSpan? HalfaDayToHour { get; set; }
        public string HalfaDayToOperator { get; set; }
        public TimeSpan? PresentHour { get; set; }
        public string PresentOperator { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int ShiftDetailsId { get; set; }
        public bool? IsConsiderAbsent { get; set; }
        public bool? IsConsiderPresent { get; set; }
        public bool? IsConsiderHalfaDay { get; set; }
    }
}