using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeKRRatingView
    {
        public int APPCycle_Id { get; set; }
        public int Employee_Id { get; set; }
        public int Objective_Id { get; set; }
        public int Key_Result_Id { get; set; }
        public decimal? Key_Result_Actual_Value { get; set; }
        public decimal? Key_Result_Max_Rating { get; set; }
        public decimal? Key_Result_Rating { get; set; }
        public int? Key_Result_Status { get; set; }
        public int? Created_By { get; set; }
        public int? Is_Addressed { get; set; }
    }
    public class ManagerRatingView
    {
        public int APPCycle_Id { get; set; }
        public int Employee_Id { get; set; }
        public int Objective_Id { get; set; }
        public int? Key_Result_Status { get; set; }
        public int? Created_By { get; set; }
        public int? Is_Addressed { get; set; }
    }
}
