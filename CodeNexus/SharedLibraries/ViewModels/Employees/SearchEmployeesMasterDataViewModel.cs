using System.Collections.Generic;

namespace SharedLibraries.ViewModels
{
    public class SearchEmployeesMasterDataViewModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public List<EmployeesSkillsetViewModel> employeesSkillsetList { get; set; }
        public int Availability { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string Designation { get; set; }
    }
    public class EmployeesSkillsetViewModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SkillSetId { get; set; }
        public string SkillSetName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string Designation { get; set; }
    }
}