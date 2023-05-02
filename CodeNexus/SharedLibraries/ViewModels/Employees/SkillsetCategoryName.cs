using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class SkillsetCategoryName
    {
        public int SkillsetCategoryId { get; set; }
        public List<Skillsets> SkillsetCategory { get; set; }
    }
}
