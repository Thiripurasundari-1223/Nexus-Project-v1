using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels.Accounts
{
    public class AccountView
    {
        [Key]
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public int? AccountManagerId { get; set; }
        public string AccountManager { get; set; }
        public string AccountContact { get; set; }
        public string AccountSPOC { get; set; }
        public string AccountStatus { get; set; }
        public string ProjectTimesheetStatus { get; set; }
        public string Logo { get; set; }
        public string LogoBase64 { get; set; }
    }
}