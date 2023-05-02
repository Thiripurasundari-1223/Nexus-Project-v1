using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class SharePolicyDocumentView
    {
        public int SharePolicyDocumentId { get; set; }
        public int? PolicyDocumentId { get; set; }
        public List<int?>? DepartmentId { get; set; }
        public List<int?>? LocationId { get; set; }
        public List<int?>? RoleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}