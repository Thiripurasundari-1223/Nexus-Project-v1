using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class EmployeeAttendanceShiftDetailsView
    {
       public List<EmployeesAttendanceDetails> EmployeesAttendances { get; set; }
       public List<ShiftViewDetails> ShiftViewDetails { get; set; }
    }
}
