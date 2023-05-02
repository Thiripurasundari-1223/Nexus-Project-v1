using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class WorkDayView
    {
        public int WorkDayId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public int? KRAId { get; set; }
        public string KRAName { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
