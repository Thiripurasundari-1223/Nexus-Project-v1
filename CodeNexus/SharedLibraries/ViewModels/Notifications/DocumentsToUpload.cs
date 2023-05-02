using System;

namespace SharedLibraries.ViewModels.Notifications
{
    public class DocumentsToUpload
    {
        public int DocumentId { get; set; }
        public string DocumentCategory { get; set; }
        public bool IsApproved { get; set; }
        public string DocumentName { get; set; }
        public decimal DocumentSize { get; set; }
        public string DocumentAsBase64 { get; set; }
        public byte[] DocumentAsByteArray { get; set; }
        public string Path { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}