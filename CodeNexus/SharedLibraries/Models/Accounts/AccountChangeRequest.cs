using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Accounts
{
    public class AccountChangeRequest
    {
        [Key]
        public int AccountChangeRequestId { get; set; }
        public int? AccountId { get; set; }
        public int? AccountRelatedIssueId { get; set; }
        public string Comments { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string CreatedByName { get; set; }
    }
}