using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class IndividualAppraisalView
    {
        public AppraisalEmployeeStatusView AppraisalEmployeeStatusView { get; set; }
        public List<IndividualAppraisalObjKRAView> IndividualAppraisalObjKRAView { get; set; }
        public List<AppraisalMilestonedetails> appraisalMilestonedetails { get; set; }
        public List<IndividualAppraisalCommentsView> individualAppraisalCommentsViews { get; set; }
    }
}
