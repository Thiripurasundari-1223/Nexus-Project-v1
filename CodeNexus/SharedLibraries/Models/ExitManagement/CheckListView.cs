using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class CheckListView
    {
        public int CheckListViewId { get; set; }
        public string ApproverRole { get; set; }
        public bool? Manager { get; set; }
        public bool? PMO { get; set; }
        public bool? IT { get; set; }
        public bool? Finance { get; set; }
        public bool? Admin { get; set; }
        public bool? HR { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
