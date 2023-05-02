using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Accounts
{
    public class AccountComments
    {
        [Key]
        public int AccountCommentId { get; set; }
        public int? AccountId { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string CreatedByName { get; set; }
    }
}