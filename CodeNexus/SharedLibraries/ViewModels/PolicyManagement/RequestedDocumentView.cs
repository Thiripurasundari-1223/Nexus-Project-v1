using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class RequestedDocumentViewGroupByDocumentType
    {
        public int? DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public List<RequestedDocumentView> RequestedDocumentView { get; set; }
    }
    public class RequestedDocumentView
    {
        public int RequestedDocumentId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentPurpose { get; set; }
        public string OtherDocumentType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string RejectedReason { get; set; }
        public DateTime? ApprovedOrRejectedOn { get; set; }
        public int? ApprovedOrRejectedBy { get; set; }
        public string ApprovedOrRejectedByName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? PolicyDocumentId { get; set; }
        public string ModifiedByName { get; set; }
        public string DocumentFilePath { get; set; }
        public string SignatureFilePath { get; set; }
        public List<PlaceHolderValue> PlaceHolderValues { get; set; }
    }
    public class PlaceHolderValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string TagName { get; set; }
    }

    public class EmployeeListForRequestedDocuments
    {
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeStatus { get; set; }
        public string EmployeePhoto { get; set; }
        public bool? IsRequestPending { get; set; }
    }
}