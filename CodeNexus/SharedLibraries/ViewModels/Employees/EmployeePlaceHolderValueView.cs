using System;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeePlaceHolderValueView
    {
        public int EmployeeID { get; set; }
        public string Entity { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string BaseWorkLocation { get; set; }
        public string CurrentWorkLocation { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string FormattedEmployeeID { get; set; }
        public string OtherEmail { get; set; }
        public string PersonalMobileNumber { get; set; }
        public string PermanentAddress { get; set; }
        public string CommunicationAddress { get; set; }
    }
}