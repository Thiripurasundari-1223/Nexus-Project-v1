using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels
{
    public class EmployeesSearchViewModel
    {
        public int EmployeeId { get; set; }
        public int SkillsetId { get; set; }
        public int AllocationId { get; set; }
        public string SearchText { get; set; }
    }
}
