using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class AppraisalWorkDayFilterView
    {
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class AppraisalWorkDayKRAView
    {
       public int KeyResultId { get; set; }
       public string KeyResultName { get; set; }
       public List<WorkDayDetail> SubmittedWorkDayDetailList { get; set; }
    }
    public class AppraisalWorkDayObjectiveView
    {
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public List<AppraisalWorkDayKRAView> EmployeeKRAList { get; set; }
        public List<AppraisalWorkDayKRAGroupView> GroupKRAList { get; set; }
    }

    public class AppraisalWorkDayKRAGroupView
    {
        public int KeyResultGroupId { get; set; }
        public string KeyResultGroupName { get; set; }
        public List<AppraisalWorkDayKRAView> GroupKRADetailList { get; set; }
    }
    public class AppraisalWorkDayView
    {
        public int AppCycleId { get; set; }
        public int VersionId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<AppraisalWorkDayObjectiveView> EmployeeObjectiveList { get; set; }
    }
}
