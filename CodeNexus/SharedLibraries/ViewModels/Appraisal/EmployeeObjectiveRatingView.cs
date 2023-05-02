using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeObjectiveRatingView
    {
        public int VersionId { get; set; }
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public decimal? Rating { get; set; }
        public decimal? MaxRating { get; set; }
    }
    public class AppraisalMilestone
    {
        public DateTime? App_Cycle_Start_Date { get; set; }
        public DateTime? App_Cycle_End_Date { get; set; }
        public DateTime? Appraisee_Review_Start_Date { get; set; }
        public DateTime? Appraisee_Review_End_Date { get; set; }
        public DateTime? Appraiser_Review_Start_Date { get; set; }
        public DateTime? Appraiser_Review_End_Date { get; set; }
        public DateTime? Mgmt_Review_Start_Date { get; set; }
        public DateTime? Mgmt_Review_End_Date { get; set; }
        public string Status { get; set; }
    }
    public class AppraisalReport
    {
       public List<EmployeeObjectiveRatingView> objectiveRating { get; set; }
      // public List<AppraisalMilestone> appraisalMilestones { get; set; }
        public List<AppraisalMilestonedetails> appraisalMilestonedetails { get; set; }
       public decimal TotalScore { get; set; }
    }
}
