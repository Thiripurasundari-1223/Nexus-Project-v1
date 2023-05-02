using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class AppraisalEmployeeStatusView
    {
        public int? ApprisalStatusID { get; set; }
        public string ApprisalStatus { get; set; }
        public decimal? EmployeeSelfRating { get; set; }
        public bool? IsBUHeadRevert { get; set; }
        public bool? IsRevertRating { get; set; }
        public bool? IsBUHeadApproved { get; set; }
    }
}
