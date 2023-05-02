using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedLibraries.Models.Attendance
{
    [Table ("ShiftWeekendDefinition")]
    public class ShiftWeekendDefinition
    {
        [Key]
        public int ShiftWeekendDefinitionId { get; set; }
        public int WeekendDayId { get; set; }
        public int ShiftDetailsId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime?  ModifiedOn { get; set; }
        public int?  ModifiedBy { get; set; }
    }
}
