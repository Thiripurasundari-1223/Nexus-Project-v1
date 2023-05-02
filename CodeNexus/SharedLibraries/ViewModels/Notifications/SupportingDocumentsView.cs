using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedLibraries.ViewModels.Notifications
{
    public class DocumentDetails
    {
        public int DocumentId { get; set; }
        public string DocumentCategory { get; set; }
        public bool IsApproved { get; set; }
        public decimal DocumentSize { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
    }
    public class SupportingDocumentsView
    {
        public List<DocumentDetails> ListOfDocuments { get; set; }
        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public int CreatedBy { get; set; }
        public string BaseDirectory { get; set; }
        public string DocumentType { get; set; }
        public int EmployeeId { get; set; }
        public List<DocumentsToUpload> EmployeeDocumentList { get; set; }
        public Guid proofDocumentId { get; set; }
    }
}
