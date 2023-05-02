using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class ResignationInterview
    {
        [Key]
        public int ResignationInterviewId { get; set; }
        public int EmployeeID { get; set; }
        public string OverallView { get; set; }
        public int ReasonOfRelievingPositionId { get; set; }
        public string ReasonOfRelieving { get; set; }
        public string EventTriggeredForAlternativeJob { get; set; }
        public string ShareProspectiveEmployer { get; set; }
        public string AttractedProspectiveEmployer { get; set; }
        public string UnresolvedIssues { get; set; }
        public string EnjoyDislike { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? ResignationDetailsId { get; set; }
        public string FeelAboutManagement { get; set; }
        public string FeedbackOnPerformance { get; set; }
        public string SufficientSupportTraining { get; set; }
        public string PreventFromLeaving { get; set; }
        public string RejoinORRecommend { get; set; }
        public string Suggestion { get; set; }
    }
}
