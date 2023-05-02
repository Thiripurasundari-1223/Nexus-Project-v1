using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class ResourceReportMasterData
    {
        public List<ProjectDetails> ProjectDetails { get; set; }
        public List<ResourceAllocation> ResourceAllocation { get; set; }
        public List<Allocation> Allocation { get; set; }
        //public List<RequiredSkillSet> RequiredSkillSets { get; set; }
    }
}
