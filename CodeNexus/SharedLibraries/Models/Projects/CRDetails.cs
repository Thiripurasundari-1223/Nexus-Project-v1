using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class ChangeRequest
    {
        [Key]
        public int ChangeRequestId { get; set; }
        public string ChangeRequestName { get; set; }
        //public string Documents_SOW { get; set; }
        public int? ProjectId { get; set; }
        public int? ChangeRequestTypeId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? SOWAmount { get; set; }
        public decimal? ChangeRequestDuration { get; set; }
        public DateTime? ChangeRequestStartDate { get; set; }
        public DateTime? ChangeRequestEndDate { get; set; }
        public string ChangeRequestDescription { get; set; }
        public string ChangeRequestStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string CRStatusCode { get; set; }
        public string CRChanges { get; set; }
        public string FormattedChangeRequestId { get; set; }
    }
}