using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeBasicInfoView
    {
        public EmployeeView Employee { get; set; }
        public EmployeePersonalInfoView EmployeesPersonalInfo { get; set; }
        public List<EmployeeDependentView> EmployeeDependent { get; set; }
        public List<EmployeeShiftDetails> EmployeeShiftDetails { get; set; }
        public List<EmployeeDocument> AddressProof { get; set; }
        public DocumentsToUpload CommunicationAddressProof { get; set; }
        public DocumentsToUpload PermanentAddressProof { get; set; }
        public DocumentsToUpload MaritalStatusProof { get; set; }
    }
}
