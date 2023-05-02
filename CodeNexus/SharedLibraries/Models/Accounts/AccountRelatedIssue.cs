using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Accounts
{
    public class AccountRelatedIssue
    {
        [Key]
        public int AccountRelatedIssueId { get; set; }
        public string AccountRelatedIssueReason { get; set; }
        public string ReasonDescription { get; set; }
    }
}