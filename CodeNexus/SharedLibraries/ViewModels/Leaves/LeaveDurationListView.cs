using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveDurationListView
    {
        public int LeaveTypeId { get; set; }
        public int DurationId { get; set; }
        public string AppConstantType { get; set; }
        public string DisplayName { get; set; }
        public string AppConstantValue { get; set; }
    }
}
