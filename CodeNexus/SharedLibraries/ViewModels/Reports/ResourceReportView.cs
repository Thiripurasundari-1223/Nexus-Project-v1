using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class ResourceReportView
    {
        public List<ResourceReport> ResourceReport { get; set; }
        public List<ReportData> ActiveProjectList { get; set; }
        public List<ReportData> ResourceWiseUtillisationReport { get; set; }
        public List<ResourceUtilization> ProjectWiseResourceUtilisationReport { get; set; }
        public List<ReportData> ResourceBillabilityStatusReport { get; set; }
        public List<ReportData> SkillSetWiseResourceReport { get; set; }
        public List<ReportData> SkillSetWiseBenchReport { get; set; }
        public List<ReportData> ProjectSkillsSetReport { get; set; }
        public List<Skillsets> Skillsets { get; set; }
    }
}
