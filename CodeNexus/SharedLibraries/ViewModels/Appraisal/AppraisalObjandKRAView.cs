using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class AppraisalKRAView
    {
       public int KeyResult_Id { get; set; }
       public string KeyResult_Name { get; set; }
       public  decimal? Key_Result_Actual_Value { get; set; }
       public  decimal? Key_Result_Max_Rating { get; set; }
       public  decimal? Key_Result_Rating { get; set; }
       public  int? Key_Result_Status { get; set; }
       public  int? IS_Addressed { get; set; }
    }
    public class ObjectiveandKRAs
    {
       public int AppCycleId { get; set; }
       public int EmployeeId { get; set; }
       public int ObjectiveId { get; set; }
       public string ObjectiveName { get; set; }
       public decimal ObjectivemaxRating { get; set; }
       public decimal ObjectiveRating { get; set; }
       public List<AppraisalKRAView> AppraisalKRAView { get; set; }
    }
    public class AppraisalObjectiveandKRAListView
    {
        public List<EmployeeObjectiveRating> employeeObjectiveRatingsList { get; set; }
        public List<EmployeeKeyResultRating> employeeKeyResultRatingsList { get; set; }
    }
}
