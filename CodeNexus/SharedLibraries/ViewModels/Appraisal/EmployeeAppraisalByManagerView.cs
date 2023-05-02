using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeAppraisalByManagerView
    {
        public int? AppCycleID { get; set; }
        public int? EntityID { get; set; }
        public int? ManagerRoleID { get; set; }
        public int? ManagerDeptID { get; set; }
        public int? EmployeeManagerID { get; set; }
        public string EmployeeManagerName { get; set; }
        public List<AppraisalEmployeeLists> EmployeeLists { get; set; }
        //public List<EmployeeAppraisalByManager> EmployeeLists { get; set; }
    }
    //Remove
    public class AppraisalEmployeeLists
    {
        public int? AppCycleID { get; set; }
        public int? EmployeeID { get; set; }
        public int? EntityID { get; set; }
        public int? EmployeeRoleID { get; set; }
        public int? EmployeeDeptID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        //public int? APPRAISAL_STATUS { get; set; }
        //public string APPRAISAL_STATUS_NAME { get; set; }
        public string Status { get; set; }
    }

    public class EmployeeAppraisalByManager
    {
        public int? APP_CYCLE_ID { get; set; }
        public int EMPLOYEE_ID { get; set; }
        public int? ENTITY_ID { get; set; }
        public int? EMPLOYEE_ROLE_ID { get; set; }
        public int? EMPLOYEE_DEPT_ID { get; set; }
        public int? APPRAISAL_STATUS { get; set; }
        public string APPRAISAL_STATUS_NAME { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string FullName { get; set; }
        public int? EMPLOYEE_MANAGER_ID { get; set; }
    }
    public class ManagerDetail
    {
        public int? APP_CYCLE_ID { get; set; }
        public int? ENTITY_ID { get; set; }
        public int? MANAGER_ROLE_ID { get; set; }
        public int? MANAGER_DEPARTMENT_ID { get; set; }
        public int? MANAGER_ID { get; set; }
        public string MANAGER_FIRSTNAME { get; set; }
        public string MANAGER_LASTNAME { get; set; }
        public string MANAGER_FULLNAME { get; set; }
    }

    public class EmployeeByManager
    {
        public int EmployeeID { get; set; }
        public int? ReportingManagerRoleID { get; set; }
        public int? ReportingManagerDeptID { get; set; }
        public int? ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}

