using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveHistoryView
    {
        public DateTime Date { get; set; }
        public string Remark { get; set; }
        public decimal Added { get; set; }
        public decimal Used { get; set; }
        public decimal Balance { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class LeaveHistoryModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
    }
}
