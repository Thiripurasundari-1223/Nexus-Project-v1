using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Projects
{
    public class ProjectAuditView
    {

        public ProjectDetails projectDetails { get; set; }

        public ProjectAudit projectAudit { get; set; }

        public List<ResourceAllocation> resourceAllocationLists { get; set; }

        public List<ChangeRequest> changeRequests { get; set; }

      
     
    }
}
