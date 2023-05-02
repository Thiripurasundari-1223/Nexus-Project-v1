using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels.Notifications;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Projects
{
    public class ProjectMasterData
    {
        public List<CurrencyType> ListOfCurrencyTypes { get; set; }
        public List<ProjectType> ListOfProjectTypes { get; set; }
        public List<Skillsets> ListOfRequiredSkillSets { get; set; }
        public List<RateFrequency> ListOfRateFrequencys { get; set; }
        public List<Allocation> ListOfAllocations { get; set; }
        public List<StatusViewModel> ProjectStatusList { get; set; }
        public List<RoleName> ListOfRoleName { get; set; }
        public List<Designation> ListOfDesignation { get; set; }
        public List<BUAccountableForProject> BUAccountableForProjects { get; set; } 
    }
}