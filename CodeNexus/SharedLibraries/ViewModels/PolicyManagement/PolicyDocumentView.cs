using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class PolicyDocumentView
    {
        public int PolicyDocumentId { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public int? FolderId { get; set; }
        public string FolderName { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool? Acknowledgement { get; set; }
        public bool? IsGenerated { get; set; }
        public bool? AlreadyAcknowledged { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string[] NotifyThrough { get; set; }
        public string[] ToView { get; set; }
        public string[] ToDownload { get; set; }
        public string FilePath { get; set; }
        public string ExistingFilePath { get; set; }
        public bool? ToShareWithAll { get; set; }
        public DocumentsToUpload DocumentToUpload { get; set; }
        public List<SharePolicyDocumentView> SharePolicyDocuments { get; set; }
    }
}