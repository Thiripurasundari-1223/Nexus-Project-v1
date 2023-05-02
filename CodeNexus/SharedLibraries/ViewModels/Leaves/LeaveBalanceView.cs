using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveBalanceView
    {
        public List<LeaveBalanceList> Leaves { get; set; }

        public List<LeaveTypesView> LeaveTypes { get; set; }
    }
    public class LeaveBalanceList
    {
        public int? EmployeeId { get; set; }
        public decimal? BalanceLeaves { get; set; }
    }
}
