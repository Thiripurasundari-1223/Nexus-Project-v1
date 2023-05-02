using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class ProjectReport
    {
        public int ProjectId { get; set; }
        public string FormattedProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectSPOC { get; set; }
        public int ProjectSPOCId { get; set; }
        public string ProjectType { get; set; }
        public string AccountName { get; set; }
        public int AccountId { get; set; }
        public string OwnerName { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public decimal? ProjectDuration { get; set; }
        public string ProjectStatus { get; set; }
        public List<ProjectTeamDetail> ProjectTeamDetail { get; set; }
    }
}
