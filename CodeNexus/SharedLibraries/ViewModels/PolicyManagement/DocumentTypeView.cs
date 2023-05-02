using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class DocumentTypeView
    {
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string TemplateFile { get; set; }
        public string SignatureFile { get; set; }
        public string ExistingFilePath { get; set; }
        public DocumentsToUpload DocumentToUpload { get; set; }
        public DocumentsToUpload Signature { get; set; }
        public List<DocumentTagView> DocumentTag { get; set; }
    }
}