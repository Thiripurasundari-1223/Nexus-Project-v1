using SharedLibraries.ViewModels.Employee;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeWorkAndEducationDetailView
    {
        public int EmployeeId { get; set; }
        public List<WorkHistoryView> WorkHistoriesList { get; set; }
        public List<EducationDetailView> EducationDetailsList { get; set; }
        public SupportingDocumentsView SupportingDocumentsViews { get; set; }
        public List<EmployeeRequestListView> employeeRequestDetails { get; set; }
    }
}
