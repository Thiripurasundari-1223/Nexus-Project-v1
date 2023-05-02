using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class EmployeesAttendanceFilterView : PaginationViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? EmployeeId { get; set; }
        public List<int> ResourceId { get; set; }
        public List<int> DepartmentId { get; set; }
        public List<int> DesignationId { get; set; }
        public List<int> LocationId { get; set; }
    }
}
