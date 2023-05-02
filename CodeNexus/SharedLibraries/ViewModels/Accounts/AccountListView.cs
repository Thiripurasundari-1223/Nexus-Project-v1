using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Accounts
{
    public class AccountListView
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string CompanyName { get; set; }
        public int? AccountManagerId { get; set; }
        public string AccountManager { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public string ContactPerson { get; set; }
        public string AcountSPOC { get; set; }
        public string ProjectTimesheet { get; set; }
        public int Associates { get; set; }
        public int? CreatedBy { get; set; }
        public string AccountStatusCode { get; set; }
        public string AccountStatusName { get; set; }
        public string AccountChanges { get; set; }
        public string Logo { get; set; }
        public string LogoBase64 { get; set; }
    }
    public class AccountManagerList
    {
        public int AccountManagerId { get; set; }
        public string AccountManagerName { get; set; }
    }
    public class ApproveAccount
    {
        public int AccountId { get; set; }
        public string AccountStatus { get; set; }
        public string Comments { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
    }
    public class RequestChangesAccount
    {
        public int AccountId { get; set; }
        public int CreatedById { get; set; }
        public int FinanceManagerId { get; set; }
        public string AccountStatus { get; set; }
        public string Comments { get; set; }
        public List<int> AccountRelatedIssues { get; set; }
        public string CreatedByName { get; set; }
    }
    public class RemoveLogoAccount
    {
        public int AccountId { get; set; }
    }
}