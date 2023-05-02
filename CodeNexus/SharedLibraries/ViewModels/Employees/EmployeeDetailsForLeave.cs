using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeDetailsForLeave
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public int departmentId { get; set; }
        public int locationId { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? WeddingAnniversary { get; set; }
        public DateTime? DateOfJoining { get; set; }
    }
}
