using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class SkillsetEmployeeDetails
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string EmployeeTypeName { get; set; }
        public string DesignationName { get; set; }
        public List<string> Skillset { get; set; }
        public int? SkillSetCount { get; set; }
        public string EmailAddress { get; set; }
        public String ProfilePic { get; set; }
    }
}
