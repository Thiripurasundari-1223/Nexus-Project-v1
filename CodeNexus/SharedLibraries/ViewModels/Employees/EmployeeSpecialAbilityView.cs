using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeSpecialAbilityView
    {
        public int? EmployeeSpecialAbilityId { get; set; }
        public int? EmployeeId { get; set; }
        public int? SpecialAbilityId { get; set; }
        public string SpecialAbilityRemark { get; set; }
    }
}
