using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Accounts;
using SharedLibraries.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace CustomerOnBoarding.DAL.Repository
{
    public interface IAccountDetailsRepository : IBaseRepository<AccountDetails>
    {
        //AccountDetails GetByName(string pAccountName, int pAccountID = 0);
        AccountDetails GetByID(int pAccountID);
        AccountDetailView GetByAccountID(int pAccountID, int versionId, bool isLastVersion);
        //List<AccountDetails> GetByName(string[] pAccountNames);
        List<AccountListView> GetAllAccountsByResourceId(AccountInput inputData);
        List<AccountListView> GetAllDraftAccounts(int pResourceId);
        List<AccountRelatedIssue> GetAllAccountRelatedIssues();
        //AccountRelatedIssue GetAllAccountRelatedIssueById(int pAccountRelatedIssueId);
        public bool AccountNameDuplication(string pAccountName, int pAccountID);
        List<AccountNames> GetAccountNameById(List<int> accountId);
        List<AccountView> GetAllAccounts(bool pIsActive = false);
        List<AccountDetails> GetAllAccountsDetails();
        List<HomeReportData> GetCustomerOnBoardHomeReport(int resourceId);
        List<KeyWithValue> GetAllAccountName();
        List<StringKeyWithValue> GetAllAccountManagerName();
        int GetAllAccountsCount(AccountInput inputData);
    }
    public class AccountDetailsRepository : BaseRepository<AccountDetails>, IAccountDetailsRepository
    {
        private readonly COBDBContext dbContext;
        public AccountDetailsRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        //public AccountDetails GetByName(string pAccountName, int pAccountID = 0)
        //{
        //    if (pAccountID > 0)
        //    {
        //        return dbContext.AccountDetails.Where(x => x.AccountName == pAccountName && x.AccountId == pAccountID).FirstOrDefault();
        //    }
        //    return dbContext.AccountDetails.Where(x => x.AccountId == pAccountID).FirstOrDefault();
        //}
        public bool AccountNameDuplication(string pAccountName, int pAccountID)
        {
            List<AccountDetails> accountDetails = dbContext.AccountDetails.Where(x => x.AccountName == pAccountName && x.AccountId != pAccountID).ToList();
            if (accountDetails.Count > 0)
                return true;
            return false;
        }
        public AccountDetails GetByID(int pAccountID)
        {
            return dbContext.AccountDetails.Where(x => x.AccountId == pAccountID).FirstOrDefault();
        }
        //public List<AccountDetails> GetByName(string[] pAccountNames)
        //{
        //    return dbContext.AccountDetails.Where(x => pAccountNames.Contains(x.AccountName)).ToList();
        //}
        public AccountDetailView GetByAccountID(int pAccountID, int versionId, bool isLastVersion)
        {
            dynamic accountDetails = new AccountDetails();
            if (isLastVersion == true)
            {
                accountDetails = dbContext.AccountDetails.Where(x => x.AccountId == pAccountID).FirstOrDefault();
            }
            else
            {
                accountDetails = dbContext.VersionAccountDetails.Where(x => x.AccountId == pAccountID && x.VersionId == versionId).FirstOrDefault();
            }
            if (accountDetails != null)
            {
                AccountDetailView accountDetailView = new()
                {
                    AccountId = accountDetails.AccountId,
                    FormattedAccountId = "ACC1" + accountDetails.AccountId.ToString().PadLeft(4, '0'),
                    AccountName = accountDetails.AccountName,
                    AccountDescription = accountDetails.AccountDescription,
                    AccountManagerId = accountDetails.AccountManagerId,
                    AccountManager = accountDetails.AccountManagerName,
                    AccountTypeId = accountDetails.AccountTypeId,
                    //AccountType = dbContext.AccountType.Where(x => x.AccountTypeId == accountDetails.AccountTypeId).Select(x => x.AccountTypeName).FirstOrDefault(),
                    AccountLocation = accountDetails.AccountLocation,
                    OfficeAddress = accountDetails.OfficeAddress,
                    CountryId = accountDetails.CountryId,
                    //Country = dbContext.Country.Where(x => x.CountryId == accountDetails.CountryId).Select(x => x.CountryName).FirstOrDefault(),
                    StateId = accountDetails.StateId,
                    PostalCode = accountDetails.PostalCode,
                    City = accountDetails.City,
                    WebSite = accountDetails.WebSite,
                    BillingCycleFrequenyId = accountDetails.BillingCycleFrequenyId,
                    //BillingCycleFrequeny = dbContext.BillingCycle.Where(x => x.BillingCycleId == accountDetails.BillingCycleFrequenyId).Select(x => x.BillingCycleDescription).FirstOrDefault(),
                    PANNumber = accountDetails.PANNumber,
                    TANNumber = accountDetails.TANNumber,
                    GSTNumber = accountDetails.GSTNumber,
                    CompanyRegistrationNumber = accountDetails.CompanyRegistrationNumber,
                    DirectorFirstName = accountDetails.DirectorFirstName,
                    DirectorLastName = accountDetails.DirectorLastName,
                    DirectorPhoneNumber = accountDetails.DirectorPhoneNumber,
                    TaxcertificateOfTheRespectiveCounty = accountDetails.TaxcertificateOfTheRespectiveCounty,
                    EntityRegistrationDocuments = accountDetails.EntityRegistrationDocuments,
                    Documents = accountDetails.Documents,
                    AccountStatus = accountDetails.AccountStatus,
                    AccountApprovedDate = accountDetails.AccountApprovedDate,
                    AdditionalComments = accountDetails.AdditionalComments,
                    CreatedOn = accountDetails.CreatedOn,
                    CreatedBy = accountDetails.CreatedBy,
                    ModifiedOn = accountDetails.ModifiedOn,
                    ModifiedBy = accountDetails.ModifiedBy,
                    IsDraft = accountDetails.IsDraft,
                    UserRole = "",
                    AccountStatusCode = accountDetails.AccountStatusCode,                    
                    AccountChanges = string.IsNullOrEmpty(accountDetails.AccountChanges) ? string.Empty : SharedLibraries.CommonLib.RemoveDuplicateFromJsonString(accountDetails.AccountChanges),
                    LogoBase64 = ConvertToBase64String(accountDetails.Logo),
                    Logo = accountDetails.Logo
                };
                if(!string.IsNullOrEmpty(accountDetailView?.AccountStatusCode))
                {
                    accountDetailView.ActualAccountStatus = dbContext.AppConstants.Where(x => x.AppConstantType == "CustomerStatus" && x.AppConstantValue == accountDetailView.AccountStatusCode).Select(x => x.DisplayName).FirstOrDefault();
                }
                if (accountDetailView?.StateId !=null && accountDetailView?.StateId > 0)
                {
                    accountDetailView.State = dbContext.State.Where(x => x.StateId == accountDetailView.StateId).Select(x => x.StateName).FirstOrDefault();
                }
                if (accountDetailView?.BillingCycleFrequenyId != null && accountDetailView?.BillingCycleFrequenyId > 0)
                {
                    accountDetailView.BillingCycleFrequeny = dbContext.BillingCycle.Where(x => x.BillingCycleId == accountDetailView.BillingCycleFrequenyId).Select(x => x.BillingCycleDescription).FirstOrDefault();
                }
                return accountDetailView;
            }
            return null;
        }
        public List<AccountListView> GetAllAccountsByResourceId(AccountInput inputData)
        {
            var query = "(AccountName.Contains( \"@accountName\") || \"@accountName\" == \"\" )" +
                "&&((AccountManagerId == \"@accountManagerId\") || \"@accountManagerId\" == \"\" )" +
                "&&((AccountTypeId == \"@accountTypeId\") || \"@accountTypeId\" == \"\" )" +
                 "&&((AccountStatus == \"@accountStatus\") || \"@accountStatus\" == \"\" )" +
                "&&((CountryId == \"@countryId\") || \"@countryId\" == \"\" )" +
                "&&@draft";
            query = query.Replace("@accountName", inputData?.AccountName == null ? "" : inputData?.AccountName);
            query = query.Replace("@accountManagerId", inputData?.AccountManagerId == 0 ? "" : inputData?.AccountManagerId.ToString());
            query = query.Replace("@accountTypeId", inputData?.AccountTypeId == 0 ? "" : inputData?.AccountTypeId.ToString());
            query = query.Replace("@accountStatus", inputData?.AccountStatus == null ? "" : inputData?.AccountStatus);
            query = query.Replace("@countryId", inputData?.CountryId == 0 ? "" : inputData?.CountryId.ToString());
            query = query.Replace("@draft", inputData.IsDraft == false ? "((IsDraft == false) || (IsDraft == null))" : "(IsDraft == true)");
            if (inputData.ManagementRole.Contains(inputData.RoleName?.ToLower()))
            {
                return dbContext.AccountDetails.Where(query).Select(accList =>
                                                new AccountListView
                                                {
                                                    AccountId = accList.AccountId,
                                                    AccountName = accList.AccountName,
                                                    AccountStatus = accList.AccountStatus,
                                                    Logo = ConvertToBase64String(accList.Logo)
                                                }).OrderBy(inputData.AccountNameSortBy == null ? "AccountId DESC" : "AccountName " + inputData.AccountNameSortBy).Skip(inputData.NoOfRecord * (inputData.PageNumber)).Take(inputData.NoOfRecord).ToList();

            }
            else
            {
                if (inputData?.EmployeeList == null)
                    inputData.EmployeeList = new List<int>();

                return dbContext.AccountDetails.Where(query).Where(x => ((inputData.EmployeeList != null && inputData.EmployeeList.Contains(x.CreatedBy == null ? 0 : (int)x.CreatedBy))
                                                || (inputData.EmployeeList != null && inputData.EmployeeList.Contains(x.FinanceManagerId == null ? 0 : (int)x.FinanceManagerId))
                                                || (inputData.EmployeeList != null && inputData.EmployeeList.Contains(x.AccountManagerId == null ? 0 : (int)x.AccountManagerId)))
                                                ).Select(accList =>
                                                new AccountListView
                                                {
                                                    AccountId = accList.AccountId,
                                                    AccountName = accList.AccountName,
                                                    AccountStatus = accList.AccountStatus,
                                                    Logo = ConvertToBase64String(accList.Logo)
                                                }).OrderBy(inputData.AccountNameSortBy == null ? "AccountId DESC" : "AccountName " + inputData.AccountNameSortBy).Skip(inputData.NoOfRecord * (inputData.PageNumber)).Take(inputData.NoOfRecord).ToList();
            }
        }

        public int GetAllAccountsCount(AccountInput inputData)
        {
            var query = "(AccountName.Contains( \"@accountName\") || \"@accountName\" == \"\" )" +
                "&&((AccountManagerId == \"@accountManagerId\") || \"@accountManagerId\" == \"\" )" +
                "&&((AccountTypeId == \"@accountTypeId\") || \"@accountTypeId\" == \"\" )" +
                "&&((AccountStatus == \"@accountStatus\") || \"@accountStatus\" == \"\" )" +
                "&&((CountryId == \"@countryId\") || \"@countryId\" == \"\" )" +
                "&&@draft";
            query = query.Replace("@accountName", inputData?.AccountName == null ? "" : inputData?.AccountName);
            query = query.Replace("@accountManagerId", inputData?.AccountManagerId == 0 ? "" : inputData?.AccountManagerId.ToString());
            query = query.Replace("@accountTypeId", inputData?.AccountTypeId == 0 ? "" : inputData?.AccountTypeId.ToString());
            query = query.Replace("@accountStatus", inputData?.AccountStatus == null ? "" : inputData?.AccountStatus);
            query = query.Replace("@countryId", inputData?.CountryId == 0 ? "" : inputData?.CountryId.ToString());
            query = query.Replace("@draft", inputData.IsDraft == false ? "((IsDraft == false) || (IsDraft == null))" : "(IsDraft == true)");
            if (inputData.ManagementRole.Contains(inputData.RoleName?.ToLower()))
            {
                return dbContext.AccountDetails.Where(query).Count();

            }
            else
            {
                if (inputData?.EmployeeList == null)
                    inputData.EmployeeList = new List<int>();

                return dbContext.AccountDetails.Where(query).Where(x => ((inputData.EmployeeList != null && inputData.EmployeeList.Contains(x.CreatedBy == null ? 0 : (int)x.CreatedBy))
                                                || (inputData.EmployeeList != null && inputData.EmployeeList.Contains(x.FinanceManagerId == null ? 0 : (int)x.FinanceManagerId))
                                                || (inputData.EmployeeList != null && inputData.EmployeeList.Contains(x.AccountManagerId == null ? 0 : (int)x.AccountManagerId)))
                                                ).Count();
            }
        }
        public List<AccountListView> GetAllDraftAccounts(int pResourceId)
        {
            return dbContext.AccountDetails.Where(x => (x.IsDraft == true && x.CreatedBy == pResourceId)).Select(accList =>
                                                new AccountListView
                                                {
                                                    AccountId = accList.AccountId,
                                                    AccountName = accList.AccountName,
                                                    //AccountManagerId = accList.AccountManagerId,
                                                    //AccountManager = "",
                                                    //AccountStatus = accList.AccountStatus,
                                                    //ContactPerson = dbContext.CustomerContactDetails.Where(x => x.AccountId == accList.AccountId).Select(x => x.ContactPersonFirstName + " " + x.ContactPersonLastName).FirstOrDefault(),
                                                    //AcountSPOC = "--",
                                                    //ProjectTimesheet = "",
                                                    //Associates = 0,
                                                    //CreatedBy = accList.CreatedBy,
                                                    //AccountType = dbContext.AccountType.Where(x => x.AccountTypeId == accList.AccountTypeId).Select(x => x.AccountTypeName).FirstOrDefault(),
                                                    //AccountStatusCode = accList.AccountStatusCode,
                                                    //AccountStatusName = "",
                                                    //AccountChanges = string.IsNullOrEmpty(accList.AccountChanges) ? string.Empty : SharedLibraries.CommonLib.RemoveDuplicateFromJsonString(accList.AccountChanges),
                                                    Logo = ConvertToBase64String(accList.Logo)
                                                }).ToList();
        }
        private static string ConvertToBase64String(string pLogo)
        {
            string strValue = null;
            if (pLogo != null && pLogo != "" && System.IO.File.Exists(pLogo))
            {
                string extension = Path.GetExtension(pLogo);
                if (extension != "")
                    strValue = "data:image/" + extension.ToLower().Replace(".", "") + ";base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(pLogo));
            }
            return strValue;
        }
        //private List<AccountListView> GetAccountLists(int pResourceId = 0, bool pIsDraft = false, List<int> listResourceId = null)
        //{
        //    List<AccountListView> accountLists = new();
        //    var accLists = (from a in dbContext.AccountDetails
        //                    select new
        //                    {
        //                        a.AccountId,
        //                        a.AccountName,
        //                        a.AccountManagerId,
        //                        AccountManager = "",
        //                        a.AccountStatus,
        //                        ContactPerson = "",
        //                        AcountSPOC = "--",
        //                        ProjectTimesheet = "",
        //                        a.CreatedBy,
        //                        a.IsDraft,
        //                        a.FinanceManagerId,
        //                        a.AccountTypeId,
        //                        a.AccountStatusCode,
        //                        a.AccountChanges,
        //                        a.Logo
        //                    }).ToList();
        //    if (pResourceId > 0 && !pIsDraft)
        //    {
        //        accLists = accLists.Where(x => (x.CreatedBy == pResourceId || x.AccountManagerId == pResourceId || x.FinanceManagerId == pResourceId) && (x.IsDraft == false || x.IsDraft == null)).ToList();
        //    }
        //    else if (pIsDraft)
        //    {
        //        accLists = accLists.Where(x => x.IsDraft == pIsDraft && x.CreatedBy == pResourceId).ToList();
        //    }
        //    else if (listResourceId != null && listResourceId.Count > 0)
        //    {
        //        accLists = accLists.Where(x => ((x.CreatedBy.HasValue && listResourceId.Contains((int)x.CreatedBy))
        //                                        || (x.AccountManagerId.HasValue && listResourceId.Contains((int)x.AccountManagerId))
        //                                        || (x.FinanceManagerId.HasValue && listResourceId.Contains((int)x.FinanceManagerId)))
        //                                        && (x.IsDraft == false || x.IsDraft == null)).ToList();
        //    }
        //    else
        //    {
        //        accLists = accLists.Where(x => (x.IsDraft == false || x.IsDraft == null)).ToList();
        //    }
        //    foreach (var accList in accLists.OrderByDescending(x => x.AccountId))
        //    {
        //        AccountListView accountList = new()
        //        {
        //            AccountId = accList.AccountId,
        //            CompanyName = (accList.AccountName == null && pIsDraft) ? "Untitled" : accList.AccountName,
        //            AccountManagerId = accList.AccountManagerId,
        //            AccountManager = "",
        //            AccountStatus = accList.AccountStatus,
        //            ContactPerson = dbContext.CustomerContactDetails.Where(x => x.AccountId == accList.AccountId).Select(x => x.ContactPersonFirstName + " " + x.ContactPersonLastName).FirstOrDefault(),
        //            AcountSPOC = accList.AcountSPOC,
        //            ProjectTimesheet = "",
        //            Associates = 0,
        //            CreatedBy = accList.CreatedBy,
        //            AccountType = dbContext.AccountType.Where(x => x.AccountTypeId == accList.AccountTypeId).Select(x => x.AccountTypeName).FirstOrDefault(),
        //            AccountStatusCode = accList.AccountStatusCode,
        //            AccountStatusName = "",
        //            AccountChanges = string.IsNullOrEmpty(accList.AccountChanges) ? string.Empty : SharedLibraries.CommonLib.RemoveDuplicateFromJsonString(accList.AccountChanges),
        //            Logo = accList.Logo
        //        };
        //        accountLists.Add(accountList);
        //    }
        //    return accountLists;
        //}
        public List<AccountRelatedIssue> GetAllAccountRelatedIssues()
        {
            return dbContext.AccountRelatedIssue.ToList();
        }
        //public AccountRelatedIssue GetAllAccountRelatedIssueById(int pAccountRelatedIssueId)
        //{
        //    return dbContext.AccountRelatedIssue.Where(x => x.AccountRelatedIssueId == pAccountRelatedIssueId).FirstOrDefault();
        //}
        public List<AccountNames> GetAccountNameById(List<int> accountId)
        {
            return dbContext.AccountDetails.Where(x => accountId.Contains(x.AccountId)).Select(x => new AccountNames { AccountId = x.AccountId, AccountName = x.AccountName }).ToList();
        }
        public List<AccountView> GetAllAccounts(bool pIsActive = false)
        {
            List<AccountView> accountViews = new();
            List<AccountDetails> accountDetails = new();
            accountDetails = dbContext.AccountDetails.Where(x => pIsActive == true ? x.AccountStatus == "Active" : x.AccountStatus != "Draft").ToList();
            foreach (AccountDetails accountDetail in accountDetails)
            {
                AccountView accountView = new()
                {
                    AccountContact = dbContext.CustomerContactDetails.Where(x => x.AccountId == accountDetail.AccountId).Select(y => y.ContactPersonFirstName + " " + y.ContactPersonLastName).FirstOrDefault(),
                    AccountId = accountDetail.AccountId,
                    AccountName = accountDetail.AccountName,
                    AccountManagerId = accountDetail.AccountManagerId,
                    AccountManager = "",
                    AccountSPOC = "",
                    AccountDescription = accountDetail.AccountDescription,
                    AccountStatus = accountDetail.AccountStatus,
                    ProjectTimesheetStatus = "",
                    Logo = accountDetail.Logo
                };
                accountViews.Add(accountView);
            }
            return accountViews;
        }
        public List<AccountDetails> GetAllAccountsDetails()
        {
            return dbContext.AccountDetails.ToList();
        }
        public List<HomeReportData> GetCustomerOnBoardHomeReport(int resourceId)
        {
            List<HomeReportData> customerOnBoard = new();
            if (resourceId > 0)
            {
                DateTime date = DateTime.Now;
                int quarterNumber = (date.Month - 1) / 3 + 1;
                DateTime currentQuarterFirstDay = new(date.Year, (quarterNumber - 1) * 3 + 1, 1);
                DateTime currentQuarterLastDay = currentQuarterFirstDay.AddMonths(3).AddDays(-1);
                HomeReportData currentQuator = new();
                currentQuator.ReportTitle = currentQuarterFirstDay.ToString("MMM") + "-" + currentQuarterLastDay.ToString("MMM") + " " + (currentQuarterFirstDay.Month >= 4 ? currentQuarterFirstDay.ToString("yy") + "-" + currentQuarterFirstDay.AddYears(1).ToString("yy") : currentQuarterFirstDay.AddYears(-1).ToString("yy") + "-" + currentQuarterFirstDay.ToString("yy"));
                currentQuator.ReportData = dbContext.AccountDetails.Where(x => x.AccountStatus == "Active" && x.CreatedOn.Value.Date >= currentQuarterFirstDay.Date && x.CreatedOn.Value.Date <= currentQuarterLastDay.Date && (x.AccountManagerId == resourceId || x.FinanceManagerId == resourceId || x.CreatedBy == resourceId)).ToList().Count().ToString();
                customerOnBoard.Add(currentQuator);
                DateTime previousQuarterFirstDay = new();
                DateTime previousQuarterLastDay = new();
                if (quarterNumber == 1)
                {
                    quarterNumber = 4;
                    previousQuarterFirstDay = new(DateTime.Now.Year - 1, (quarterNumber - 1) * 3 + 1, 1);
                }
                else
                {
                    quarterNumber--;
                    previousQuarterFirstDay = new(DateTime.Now.Year, (quarterNumber - 1) * 3 + 1, 1);
                }
                previousQuarterLastDay = previousQuarterFirstDay.AddMonths(3).AddDays(-1);
                HomeReportData previousQuator = new();
                previousQuator.ReportTitle = previousQuarterFirstDay.ToString("MMM") + "-" + previousQuarterLastDay.ToString("MMM") + " " + (previousQuarterFirstDay.Month >= 4 ? previousQuarterFirstDay.ToString("yy") + "-" + previousQuarterFirstDay.AddYears(1).ToString("yy") : previousQuarterFirstDay.AddYears(-1).ToString("yy") + "-" + previousQuarterFirstDay.ToString("yy"));
                previousQuator.ReportData = dbContext.AccountDetails.Where(x => x.AccountStatus == "Active" && x.CreatedOn.Value.Date >= previousQuarterLastDay.Date && x.CreatedOn.Value.Date <= previousQuarterLastDay.Date && (x.AccountManagerId == resourceId || x.FinanceManagerId == resourceId || x.CreatedBy == resourceId)).ToList().Count().ToString();
                customerOnBoard.Add(previousQuator);
            }
            return customerOnBoard;
        }
        public List<KeyWithValue> GetAllAccountName()
        {
            return dbContext.AccountDetails.Where(x => (x.IsDraft == false || x.IsDraft == null)).Select(x => new KeyWithValue { Key = x.AccountId, Value = x.AccountName }).ToList();
        }
        public List<StringKeyWithValue> GetAllAccountManagerName()
        {
            return dbContext.AccountDetails.Where(x => x.IsDraft == false && x.AccountManagerName != null && x.AccountManagerName.Trim() != "").Select(x => new StringKeyWithValue { Key = (x.AccountManagerId == null ? "0" : x.AccountManagerId.ToString()), Value = x.AccountManagerName }).Distinct().ToList();
        }
    }
}