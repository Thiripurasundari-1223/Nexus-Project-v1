using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels
{
    public class ChangeRequestView
    {
        [Key]
        public int ChangeRequestId { get; set; }
        public string FormattedChangeRequestId { get; set; }
        public string ChangeRequestName { get; set; }
        public int? ProjectId { get; set; }
        public string FormattedProjectId { get; set; }
        public int? ChangeRequestTypeId { get; set; }
        public string ChangeRequestType { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyType { get; set; }
        public decimal? SOWAmount { get; set; }
        public decimal? ChangeRequestDuration { get; set; }
        public DateTime? ChangeRequestStartDate { get; set; }
        public DateTime? ChangeRequestEndDate { get; set; }
        public string ChangeRequestDescription { get; set; }
        public string ChangeRequestStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string AdditionalComments { get; set; }
        public List<ResourceAllocationList> ResourceAllocation { get; set; }
        public List<DocumentDetails> ListOfDocuments { get; set; }
        public string CRStatusCode { get; set; }
        public string CRStatusName { get; set; }
        public string CRChanges { get; set; }
    }
}