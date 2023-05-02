using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class SkillsetDetails
    {
        public List<SkillsetCategory> GetSkillsetCategory { get; set; }
        public List<SkillsetCategoryName> SkillsetCategoryName { get; set; }
    }
}
