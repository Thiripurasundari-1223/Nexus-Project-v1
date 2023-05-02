using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Attendance
{
    public class WeekendDefinition
    {
        [Key]
        public int WeekendDayId { get; set; }
        public string WeekendDayName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ShiftDetailsId { get; set; }
    }
}
