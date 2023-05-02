using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Appraisal
{
    public class WorkDayDetail
    {
        [Key]
        public int WorkDayDetailId { get; set; }
        public int WorkDayId { get; set; }
        public int WorkdayKRAId { get; set; }
        public TimeSpan? WorkHours { get; set; }
        public DateTime? WorkDate { get; set; }
        public string EmployeeRemark { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverRemark { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}