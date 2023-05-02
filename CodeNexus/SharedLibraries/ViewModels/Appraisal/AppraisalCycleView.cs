using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class AppraisalCycleView
    {
        public int AppCycleId { get; set; }
        public int EntityId { get; set; }
        public int VersionId { get; set; }
        public string AppCycleName { get; set; }
        public string AppCycleDesc { get; set; }
        public DateTime AppCycleStartDate { get; set; }
        public DateTime AppCycleEndDate { get; set; }
        public DateTime AppraiseeReviewStartDate { get; set; }
        public DateTime AppraiseeReviewEndDate { get; set; }
        public DateTime AppraiserReviewStartDate { get; set; }
        public DateTime AppraiserReviewEndDate { get; set; }
        public DateTime MgmtReviewStartDate { get; set; }
        public DateTime MgmtReviewEndDate { get; set; }
        public int AppraisalStatus { get; set; }
        public DateTime DateOfJoining { get; set; }
        public int EmployeesTypeId { get; set; }
        public int? DurationId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
