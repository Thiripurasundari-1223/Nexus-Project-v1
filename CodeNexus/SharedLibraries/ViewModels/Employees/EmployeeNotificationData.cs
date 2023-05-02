using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeNotificationData
    {
      
            public int EmployeeId { get; set; }

            public string EmployeeName { get; set; }

            public string EmployeeEmailAddress { get; set; }

            public EmployeeManagerDetails OldManagerDetails { get; set; }
            public EmployeeManagerDetails NewManagerDetails { get; set; }

        public EmployeeManagerDetails ManagerDetailsForProbationStatus { get; set; }

        public EmployeeMasterEmailTemplate EmployeeMasterEmailTemplateForReportingManagerChange { get; set; }

        public EmployeeMasterEmailTemplate EmployeeMasterEmailTemplateForDesignation { get; set; }

        public EmployeeMasterEmailTemplate EmployeeMasterEmailTemplateForBaseWorkLocation { get; set; }

        public EmployeeMasterEmailTemplate EmployeeMasterEmailTemplateForProbationStatus { get; set; }



        public EmployeeDesignationDetails OldEmployeeDesignationDetails { get; set; }
        public EmployeeDesignationDetails NewEmployeeDesignationDetails { get; set; }

        public EmployeeBaseWorkLocationDetails OldEmployeeBaseWorkLocationDetails { get; set; }

        public EmployeeBaseWorkLocationDetails NewEmployeeBaseWorkLocationDetails { get; set; }

        public EmployeeProbationStatusDetails OldEmployeeProbationStatusDetails { get; set; }

        public EmployeeProbationStatusDetails NewEmployeeProbationStatusDetails { get; set; }
        public bool checkLeave { get; set; }

    }
}
