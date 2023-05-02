using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels.Accounts
{
    public class AccountCommentsView
    {
        [Key]
        public int AccountCommentId { get; set; }
        public int? AccountId { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}