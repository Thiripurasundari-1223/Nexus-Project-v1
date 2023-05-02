using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class TeamLeaveAndRejectionListView
    {
        public List<TeamLeaveView> TeamLeaveView { get; set; }
        public List<LeaveRejectionReason> LeaveRejectionReason { get; set; }
        public List<LeaveTypesView> LeaveTypesView { get; set; }
        public List<string> LeaveStatus { get; set; }
    }
}
