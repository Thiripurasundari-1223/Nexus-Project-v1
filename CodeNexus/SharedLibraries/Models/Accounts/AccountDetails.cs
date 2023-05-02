using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Accounts
{
    public class AccountDetails
    {
        [Key]
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public int? AccountManagerId { get; set; }
        public int? AccountTypeId { get; set; }
        public string AccountLocation { get; set; }
        public string OfficeAddress { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string WebSite { get; set; }
        public int? BillingCycleFrequenyId { get; set; }
        public string PANNumber { get; set; }
        public string TANNumber { get; set; }
        public string GSTNumber { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string DirectorFirstName { get; set; }
        public string DirectorLastName { get; set; }
        public string DirectorPhoneNumber { get; set; }
        public string TaxcertificateOfTheRespectiveCounty { get; set; }
        public string EntityRegistrationDocuments { get; set; }
        public string Documents { get; set; }
        public string AccountStatus { get; set; }
        public DateTime? AccountApprovedDate { get; set; }
        public string AdditionalComments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDraft { get; set; }
        public int? FinanceManagerId { get; set; }
        public string AccountStatusCode { get; set; }
        public string AccountChanges { get; set; }
        public string FormattedAccountId { get; set; }
        public string Logo { get; set; }
        public string AccountManagerName { get; set; }
    }
    public class DeleteAccount
    {
        public int AccountId { get; set; }
    }

    public class UpdateAccountLogo
    {
        public int AccountId { get; set; }
        public string Logo { get; set; }
        public int? ModifiedBy { get; set; }
    }
}