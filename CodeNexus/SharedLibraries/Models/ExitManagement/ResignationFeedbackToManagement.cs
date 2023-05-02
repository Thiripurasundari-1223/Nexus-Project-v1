using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class ResignationFeedbackToManagement
    {
        [Key]
        public int ResignationFeedbackId { get; set; }
        public int ResignationInterviewId { get; set; }
        public int? TrainingId { get; set; }
        public string TrainingRemark { get; set; }
        public int? NatureOfWorkId { get; set; }
        public string NatureOfWorkRemark { get; set; }
        public int? ImmediateSupervisorInvolmentId { get; set; }
        public string ImmediateSupervisorInvolmentRemark { get; set; }
        public int? JobRecognitionId { get; set; }
        public string JobRecognitionRemark { get; set; }
        public int? PerformanceFeedbackId { get; set; }
        public string PerformanceFeedbackRemark { get; set; }
        public int? GrowthOpportunityId { get; set; }
        public string GrowthOpportunityRemark { get; set; }
        public int? NewSkillsOpportunityId { get; set; }
        public string NewSkillsOpportunityRemark { get; set; }
        public int? CompensationId { get; set; }
        public string CompensationRemark { get; set; }
        public int? AnnualIncrementId { get; set; }
        public string AnnualIncrementRemark { get; set; }
        public int? InformationSharingId { get; set; }
        public string InformationSharingRemark { get; set; }
        public bool Other { get; set; }
        public string OtherRemark { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? OrgPoliciesId { get; set; }
        public string OrgPoliciesRemark { get; set; }
    }
}
