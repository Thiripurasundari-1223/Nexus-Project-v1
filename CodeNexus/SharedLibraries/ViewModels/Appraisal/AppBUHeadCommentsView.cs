using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class AppBUHeadCommentsView
    {
        public int AppraisalBUHeadCommentsId { get; set; }
        public int AppCycle_Id { get; set; }
        public int Department_Id { get; set; }
        public int Employee_Id { get; set; }
        public string Comment { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string DepartmentName { get; set; }
        public string BUHeadName { get; set; }
    }
}
