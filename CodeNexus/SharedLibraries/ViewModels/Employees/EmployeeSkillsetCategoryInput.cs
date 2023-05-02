using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeSkillsetCategoryInput
    {
        public List<int> SkillsetId { get; set; }
        public string Condition { get; set; }
    }
}
