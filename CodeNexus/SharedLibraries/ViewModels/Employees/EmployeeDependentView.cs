using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeDependentView
    {
            public int EmployeeDependentId { get; set; }
            public string EmployeeRelationName { get; set; }
            public int? EmployeeRelationshipId { get; set; }
             public string EmployeeRelationship { get; set; }
             public DateTime? EmployeeRelationDateOfBirth { get; set; }
            public DocumentsToUpload DependentDetailsProof { get; set; }
             public int? EmployeeID { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
    }
}
