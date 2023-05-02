using SharedLibraries.Common;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ResignationInterviewDetailView
    {
        public int ResignationInterviewId { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string Department { get; set; }
        public DateTime? DOJ { get; set; }
        public string Designation { get; set; }
        public string OverallView { get; set; }
        public List<int?> ReasonOfRelievingPositionId { get; set; }
        public string ReasonOfRelieving { get; set; }
        public string EventTriggeredForAlternativeJob { get; set; }
        public string ShareProspectiveEmployer { get; set; }
        public string AttractedProspectiveEmployer { get; set; }
        public string UnresolvedIssues { get; set; }
        public string EnjoyDislike { get; set; }
        public string FeelAboutManagement { get; set; }
        public string FeedbackOnPerformance { get; set; }
        public string SufficientSupportTraining { get; set; }
        public string PreventFromLeaving { get; set; }
        public string RejoinORRecommend { get; set; }
        public string Suggestion { get; set; }
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
        public List<KeyWithValue> ResignationInterview { get; set; }
        public List<KeyWithValue> ReasonRelievingPosition { get; set; }
        public string EmployeeType { get; set; }
        public ExitManagementEmailTemplate EmailTemplate { get; set; }
        public DateTime? ResignationDate { get; set; }
        public int? ResignationDetailsId { get; set; }

        public int? OrgPoliciesId { get; set; }
        public string OrgPoliciesRemark { get; set; }

    }
}
