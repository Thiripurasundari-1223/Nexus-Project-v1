using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class ProjectReportMasterData
    {
        public List<ProjectDetails> ProjectDetails { get; set; }
        public List<ProjectType> ProjectType { get; set; }
        public List<ResourceAllocation> ResourceAllocation { get; set; }
    }
}
