using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class PolicyAcknowledgementView
    {
        public int PolicyAcknowledgedId { get; set; }
        public int? PolicyDocumentId { get; set; }
        public string AcknowledgedStatus { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public int? AcknowledgedBy { get; set; }
        public string AcknowledgedByName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string FileName { get; set; }
    }
    public class PolicyEmployeeAcknowledgementListView
    {
        public List<PolicyAcknowledgementView> PolicyAcknowledgements { get; set; }
        public bool? ToShareWithAll { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> LocationIds { get; set; }
        public List<int> DepartmentIds { get; set; }
    }
}