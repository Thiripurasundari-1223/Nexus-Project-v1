using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class DocumnentsPathView
    {
        public List<LeaveGrantRequestDetails> requestDetails { get; set; }
        public List<int> LeaveId { get; set; }
    }
}
