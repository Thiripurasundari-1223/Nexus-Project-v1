using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Projects
{
    public class ProjectTimesheet
    {
        public List<ProjectDetails> ProjectDetails { get; set; }
        public List<ResourceAllocation> ResourceAllocation { get; set; }

        public List<Allocation> Allocation { get; set; }


        public int ResourceId { get; set; }

        public DateTime? WeekStartDate { get; set; }
        public int SPOCId { get; set; }
        public int TimesheetId { get; set; }
    }
}
