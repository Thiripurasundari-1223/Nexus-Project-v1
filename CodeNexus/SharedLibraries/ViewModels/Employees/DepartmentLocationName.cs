using SharedLibraries.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Employees
{
    public class DepartmentLocationName
    {
        public List<int> DepartmentId { get; set; }
        public List<int> LocationId { get; set; }
        public List<KeyWithValue> Department { get; set; }
        public List<KeyWithValue> Location { get; set; }
    }
}
