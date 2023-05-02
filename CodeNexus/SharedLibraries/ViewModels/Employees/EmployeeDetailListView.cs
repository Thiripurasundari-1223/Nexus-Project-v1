using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeDetailListView
    {
        public int EmployeeId { get; set; }
        public String FormattedEmployeeId { get; set; }
        public String EmployeeFullName { get; set; }
        public String EmployeementType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string EmailId { get; set; }
        public String ProfilePic { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? HavingPendingRequest { get; set; }
    }
    public class EmployeeDateInput
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        
    }
}
