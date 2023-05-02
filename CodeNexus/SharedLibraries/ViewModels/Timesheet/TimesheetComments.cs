using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels
{
    public class TimesheetComments
    {
        [Key]
        public int TimesheetCommentsId { get; set; }
        public int? TimesheetId { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public Guid? WeekTimesheetId { get; set; }
    }
}