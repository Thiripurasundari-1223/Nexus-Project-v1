using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeAppraisalComment
    {
        public int Comment_Id { get; set; }
        public int? App_Cycle_Id { get; set; }
        public int? Employee_Id { get; set; }
        public string Comment { get; set; }
        public int? Created_By { get; set; }
        public bool IsAppraisalReveiew { get; set; }
    }
    public class EmployeeAppraisalBUHeadComment
    {
        public int Comment_Id { get; set; }
        public int? App_Cycle_Id { get; set; }
        public int? Employee_Id { get; set; }
        public string Comment { get; set; }
        public int? Created_By { get; set; }
        public string BUHeadName { get; set; }
        public string BUHeadEmailId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmailId { get; set; }
        public string ManagerEmailId { get; set; }
    }

}
