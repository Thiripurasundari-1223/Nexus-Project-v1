using CustomerOnBoarding.DAL.Repository;
using ExcelDataReader;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Accounts;
using SharedLibraries.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomerOnBoarding.DAL.Services
{
    public class COBServices
    {
        private readonly IAccountDetailsRepository _accountDetailsRepository;
        private readonly IAccountTypeRepository _accountTypeRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IAccountChangeRequestRepository _accountChangeRequestRepository;
        private readonly IAccountCommentsRepository _accountCommentsRepository;
        private readonly IBillingCycleRepository _billingCycleRepository;
        private readonly ICustomerContactDetailsRepository _customerContactDetailsRepository;
        private readonly IAppConstantsRepository _appConstantsRepository;
        private readonly IVersionAccountDetailsRepository _versionAccountDetailsRepository;
        private readonly IVersionCustomerContactDetailsRepository _versionCustomerContactDetailsRepository;

        #region Constructor
        public COBServices(IAccountDetailsRepository accountDetailsRepository,
            IAccountTypeRepository accountTypeRepository, ICountryRepository countryRepository,
            IStateRepository stateRepository, IAccountChangeRequestRepository accountChangeRequestRepository,
            IAccountCommentsRepository accountCommentsRepository, IBillingCycleRepository billingCycleRepository,
            ICustomerContactDetailsRepository customerContactDetailsRepository,
            IAppConstantsRepository appConstantsRepository,
            IVersionAccountDetailsRepository versionAccountDetailsRepository,
            IVersionCustomerContactDetailsRepository versionCustomerContactDetailsRepository)
        {
            _accountDetailsRepository = accountDetailsRepository;
            _accountTypeRepository = accountTypeRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _accountChangeRequestRepository = accountChangeRequestRepository;
            _accountCommentsRepository = accountCommentsRepository;
            _billingCycleRepository = billingCycleRepository;
            _customerContactDetailsRepository = customerContactDetailsRepository;
            _appConstantsRepository = appConstantsRepository;
            _versionAccountDetailsRepository = versionAccountDetailsRepository;
            _versionCustomerContactDetailsRepository = versionCustomerContactDetailsRepository;
        }
        #endregion

        #region AccountName Duplication
        public bool AccountNameDuplication(AccountDetails pAccountDetails)
        {
            if (pAccountDetails.IsDraft == true) return false;
            return _accountDetailsRepository.AccountNameDuplication(pAccountDetails.AccountName, pAccountDetails.AccountId);
        }
        #endregion

        #region Insert Update Account
        public async Task<int> InsertUpdateAccount(AddAccountView accountContactDetails)
        {
            try
            {
                int AccountId = 0;
                AccountDetails accountDetails = new();
                string versionAccountDetails = "";
                string versionCustomerContactDetails = "";
                if (accountContactDetails.AccountDetails.AccountId != 0)
                {
                    accountDetails = _accountDetailsRepository.GetByID(accountContactDetails.AccountDetails.AccountId);
                    if (accountDetails.AccountStatus == "Active" && accountContactDetails.AccountDetails.IsDraft != true)
                    {
                        versionAccountDetails = JsonConvert.SerializeObject(accountDetails);
                    }
                }
                if (accountDetails != null)
                {
                    accountDetails.FormattedAccountId = accountContactDetails.AccountDetails.FormattedAccountId;
                    accountDetails.AccountName = accountContactDetails.AccountDetails.AccountName;
                    accountDetails.AccountDescription = accountContactDetails.AccountDetails.AccountDescription;
                    accountDetails.AccountManagerId = accountContactDetails.AccountDetails.AccountManagerId;
                    accountDetails.AccountTypeId = accountContactDetails.AccountDetails.AccountTypeId;
                    accountDetails.AccountLocation = accountContactDetails.AccountDetails.AccountLocation;
                    accountDetails.OfficeAddress = accountContactDetails.AccountDetails.OfficeAddress;
                    accountDetails.CountryId = accountContactDetails.AccountDetails.CountryId;
                    accountDetails.StateId = accountContactDetails.AccountDetails.StateId;
                    accountDetails.PostalCode = accountContactDetails.AccountDetails.PostalCode;
                    accountDetails.City = accountContactDetails.AccountDetails.City;
                    accountDetails.WebSite = accountContactDetails.AccountDetails.WebSite;
                    accountDetails.BillingCycleFrequenyId = accountContactDetails.AccountDetails.BillingCycleFrequenyId;
                    accountDetails.PANNumber = accountContactDetails.AccountDetails.PANNumber;
                    accountDetails.TANNumber = accountContactDetails.AccountDetails.TANNumber;
                    accountDetails.GSTNumber = accountContactDetails.AccountDetails.GSTNumber;
                    accountDetails.CompanyRegistrationNumber = accountContactDetails.AccountDetails.CompanyRegistrationNumber;
                    accountDetails.DirectorFirstName = accountContactDetails.AccountDetails.DirectorFirstName;
                    accountDetails.DirectorLastName = accountContactDetails.AccountDetails.DirectorLastName;
                    accountDetails.DirectorPhoneNumber = accountContactDetails.AccountDetails.DirectorPhoneNumber;
                    accountDetails.TaxcertificateOfTheRespectiveCounty = accountContactDetails.AccountDetails.TaxcertificateOfTheRespectiveCounty;
                    accountDetails.EntityRegistrationDocuments = accountContactDetails.AccountDetails.EntityRegistrationDocuments;
                    accountDetails.Documents = accountContactDetails.AccountDetails.Documents;
                    accountDetails.AccountManagerName = accountContactDetails.AccountDetails.AccountManagerName;
                    if (accountContactDetails.AccountDetails.IsDraft == true)
                        accountDetails.AdditionalComments = accountContactDetails.AccountDetails.AdditionalComments;
                    else
                        accountDetails.AdditionalComments = null;
                    if (!string.IsNullOrEmpty(accountContactDetails.AccountDetails.AccountStatus))
                        accountDetails.AccountStatus = accountContactDetails.AccountDetails.AccountStatus;
                    accountDetails.AccountApprovedDate = accountContactDetails.AccountDetails.AccountApprovedDate;
                    accountDetails.IsDraft = accountContactDetails.AccountDetails.IsDraft;
                    accountDetails.FinanceManagerId = accountContactDetails.AccountDetails.FinanceManagerId;
                    accountDetails.AccountStatusCode = accountContactDetails.AccountDetails.AccountStatusCode;
                    if (accountContactDetails.AccountDetails.AccountId == 0)
                    {
                        accountDetails.CreatedOn = DateTime.UtcNow;
                        accountDetails.CreatedBy = accountContactDetails.AccountDetails.CreatedBy;
                        await _accountDetailsRepository.AddAsync(accountDetails);
                    }
                    else
                    {
                        if (accountContactDetails.AccountDetails.IsDraft == false && !string.IsNullOrEmpty(accountContactDetails.AccountDetails.AccountChanges))
                        {
                            accountDetails.AccountChanges = string.IsNullOrEmpty(accountDetails.AccountChanges) ? accountContactDetails.AccountDetails.AccountChanges : accountDetails.AccountChanges + "," + accountContactDetails.AccountDetails.AccountChanges;
                            //List<string> chList = JsonConvert.DeserializeObject <List<string>> (pAccountDetails.AccountChanges);
                            //if (!string.IsNullOrEmpty(accountDetails.AccountChanges))
                            //{
                            //    string[] exChList = Regex.Split(accountDetails.AccountChanges, ",");
                            //    chList = chList.Concat(exChList.ToList()).Distinct().ToList();
                            //}
                            //accountDetails.AccountChanges = string.Join(",", chList);
                        }
                        accountDetails.ModifiedOn = DateTime.UtcNow;
                        accountDetails.ModifiedBy = accountContactDetails.AccountDetails.ModifiedBy;
                        _accountDetailsRepository.Update(accountDetails);
                    }
                    await _accountDetailsRepository.SaveChangesAsync();
                    AccountId = accountDetails.AccountId;
                    if (accountContactDetails.AccountDetails.AdditionalComments != "" && accountContactDetails.AccountDetails.IsDraft == false)
                    {
                        AccountComments accountComments = new AccountComments
                        {
                            AccountId = AccountId,
                            Comments = accountContactDetails.AccountDetails.AdditionalComments,
                            CreatedBy = (accountContactDetails.AccountDetails.AccountId == 0 ? accountContactDetails.AccountDetails.CreatedBy : accountContactDetails.AccountDetails.ModifiedBy),
                            CreatedOn = DateTime.UtcNow,
                            CreatedByName = accountContactDetails.CreatedByName
                        };
                        await _accountCommentsRepository.AddAsync(accountComments);
                        await _accountCommentsRepository.SaveChangesAsync();
                    }
                    if (accountContactDetails.CustomerContactDetails.Count > 0 && AccountId > 0)
                    {
                        List<CustomerContactDetails> customerContactdetailslist = _customerContactDetailsRepository.GetByID(AccountId);
                        if (customerContactdetailslist != null && customerContactdetailslist.Count > 0)
                        {
                            if (versionAccountDetails != "")
                            {
                                versionCustomerContactDetails = JsonConvert.SerializeObject(customerContactdetailslist);
                            }
                            foreach (CustomerContactDetails contactitem in customerContactdetailslist)
                            {
                                _customerContactDetailsRepository.Delete(contactitem);
                                await _customerContactDetailsRepository.SaveChangesAsync();
                            }
                        }
                        foreach (var items in accountContactDetails.CustomerContactDetails)
                        {
                            CustomerContactDetails contactInfo = new CustomerContactDetails();
                            contactInfo.AccountId = AccountId;
                            contactInfo.ContactPersonFirstName = items.ContactPersonFirstName;
                            contactInfo.ContactPersonLastName = items.ContactPersonLastName;
                            contactInfo.ContactPersonPhoneNumber = items.ContactPersonPhoneNumber;
                            contactInfo.ContactPersonEmailAddress = items.ContactPersonEmailAddress;
                            contactInfo.CreatedOn = DateTime.UtcNow;
                            contactInfo.CreatedBy = accountContactDetails.AccountDetails.CreatedBy;

                            contactInfo.DesignationName = items.DesignationName;
                            contactInfo.CountryId = items.CountryId;
                            contactInfo.AddressDetail = items.AddressDetail;
                            contactInfo.CityName = items.CityName;
                            contactInfo.StateId = items.StateId;
                            contactInfo.Postalcode = items.Postalcode;

                            await _customerContactDetailsRepository.AddAsync(contactInfo);
                            await _customerContactDetailsRepository.SaveChangesAsync();

                        }
                    }
                    if (versionAccountDetails != "")
                    {
                        await InsertVersionDetails(versionAccountDetails, versionCustomerContactDetails);
                    }
                }
                return AccountId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> BulkInsertAccount(ImportExcelView import)
        {
            IDictionary<string, string> output = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(import.Base64Format))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                byte[] bytes = Convert.FromBase64String(import.Base64Format);
                MemoryStream ms = new MemoryStream(bytes);

                using (var reader = ExcelReaderFactory.CreateReader(ms))
                {
                    DataSet dataSet = reader?.AsDataSet();
                    if (dataSet?.Tables?.Count > 0)
                    {
                        #region Customer Details
                        DataTable customerDetails = dataSet?.Tables["Customer_Details"];
                        DataTable customerContactDetails = dataSet?.Tables["Contact_Details"];
                        if (customerDetails?.Rows?.Count > 0 && customerContactDetails?.Rows?.Count > 0)
                        {
                            output.Add("TotalCount", customerDetails?.Rows?.Count.ToString());
                            var accounts = AppendCustomerDetailsToView(customerDetails, customerContactDetails, import.EmployeeDetails);
                            #region Validate Customer Records From Excel
                            var validRecords = ValidateCustomerRecords(accounts, ref output);
                            #endregion
                            foreach (var validRecord in validRecords)
                            {
                                await InsertUpdateAccount(validRecord);
                                output.Add(validRecord.AccountDetails.AccountName, "Success");
                            }
                        }
                        else
                        {
                            output.Add("Error", "No Records found in the imported file");
                        }
                        #endregion                       
                    }
                    else
                    {
                        output.Add("Error", "No Records found in the imported file");
                    }
                }
            }
            return JsonConvert.SerializeObject(output);
        }

        private List<AddAccountView> ValidateCustomerRecords(List<AddAccountView> addAccountViews, ref IDictionary<string, string> output)
        {
            List<AddAccountView> validCustomerRecords = new List<AddAccountView>();
            var accounts = GetAllAccounts(true);
            foreach (var validRecord in addAccountViews)
            {
                var account = accounts.Find(x => x.AccountName == validRecord.AccountDetails.AccountName);
                if (account != null && account.AccountId > 0)
                {
                    output.Add(validRecord.AccountDetails.AccountName, "Failed - Account Name: " + validRecord.AccountDetails.AccountName + "already exists");
                    continue;
                }

                if (validRecord.AccountDetails.AccountManagerId < 0)
                {
                    output.Add(validRecord.AccountDetails.AccountName, "Failed - Account Manager Id cannot be found");
                    continue;
                }
                if (validRecord.AccountDetails.AccountTypeId < 0)
                {
                    output.Add(validRecord.AccountDetails.AccountName, "Failed - Account Type Id cannot be found");
                    continue;
                }
                if (validRecord.AccountDetails.FinanceManagerId < 0)
                {
                    output.Add(validRecord.AccountDetails.AccountName, "Failed - Finance Manager Id cannot be found");
                    continue;
                }
                if (validRecord.AccountDetails.CountryId < 0)
                {
                    output.Add(validRecord.AccountDetails.AccountName, "Failed - Country cannot be found");
                    continue;
                }
                if (validRecord.AccountDetails.StateId < 0)
                {
                    output.Add(validRecord.AccountDetails.AccountName, "Failed - State cannot be found");
                    continue;
                }
                for (int i = 0; i < validRecord.CustomerContactDetails.Count; i++)
                {
                    if (validRecord.CustomerContactDetails[i].ContactPersonPhoneNumber != null && !string.IsNullOrEmpty(validRecord.CustomerContactDetails[i].ContactPersonPhoneNumber))
                    {
                        if (!IsValidPhone(validRecord.CustomerContactDetails[i].ContactPersonPhoneNumber))
                        {
                            output.Add(validRecord.AccountDetails.AccountName, "Failed - Phone Number Invalid");
                            continue;
                        }
                    }
                    //else
                    //{
                    //    output.Add("Error", "Account Name: " + validRecord.AccountDetails.AccountName + "Phone Number cannot be empty");
                    //    continue;
                    //}
                }
                validCustomerRecords.Add(validRecord);
            }
            return validCustomerRecords;
        }

        private bool IsValidPhone(string contactPersonPhoneNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(contactPersonPhoneNumber))
                    return false;
                var r = new Regex(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$");
                return r.IsMatch(contactPersonPhoneNumber);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<AddAccountView> AppendCustomerDetailsToView(DataTable customerDetails, DataTable customerContactDetails, List<EmployeeDetail> employeeDetails)
        {
            List<AddAccountView> accounts = new List<AddAccountView>();
            var accountTypes = GetAllAccountTypes();
            var countries = GetAllCountry();
            var billingFrequencies = GetAllBillingCycle();
            for (int i = 1; i < customerDetails?.Rows?.Count; i++)
            {
                AddAccountView addAccountView = new AddAccountView();
                addAccountView.AccountDetails = new AccountDetails();
                addAccountView.CustomerContactDetails = new List<CustomerContactDetails>();
                addAccountView.AccountDetails.AccountName = customerDetails.Rows[i][0] != null ? customerDetails.Rows[i][0].ToString()?.Trim() : "";
                addAccountView.AccountDetails.AccountDescription = customerDetails.Rows[i][1] != null ? customerDetails.Rows[i][1].ToString()?.Trim() : "";
                addAccountView.AccountDetails.AccountManagerId = customerDetails.Rows[i][2] != null ? GetEmployeeIdByEmail(customerDetails.Rows[i][2].ToString()?.Trim(), employeeDetails) : -1;
                addAccountView.AccountDetails.AccountTypeId = customerDetails.Rows[i][3] != null ? GetAccountTypeId(customerDetails.Rows[i][3].ToString()?.Trim(), accountTypes) : -1;
                addAccountView.AccountDetails.FinanceManagerId = customerDetails.Rows[i][4] != null ? GetEmployeeIdByEmail(customerDetails.Rows[i][4].ToString()?.Trim(), employeeDetails) : -1;
                addAccountView.AccountDetails.AccountStatus = "Approved";
                addAccountView.AccountDetails.AccountLocation = customerDetails.Rows[i][6] != null ? customerDetails.Rows[i][6].ToString()?.Trim() : "";
                addAccountView.AccountDetails.OfficeAddress = customerDetails.Rows[i][7] != null ? customerDetails.Rows[i][7].ToString()?.Trim() : "";
                addAccountView.AccountDetails.CountryId = customerDetails.Rows[i][9] != null ? GetCountryId(customerDetails.Rows[i][9].ToString()?.Trim(), countries) : -1;
                var states = GetAllStateByCountryId((int)addAccountView.AccountDetails.CountryId);
                addAccountView.AccountDetails.StateId = customerDetails.Rows[i][10] != null ? GetStateId(customerDetails.Rows[i][10].ToString()?.Trim(), states) : -1;
                addAccountView.AccountDetails.PostalCode = customerDetails.Rows[i][11] != null ? customerDetails.Rows[i][11].ToString()?.Trim() : "";
                addAccountView.AccountDetails.City = customerDetails.Rows[i][12] != null ? customerDetails.Rows[i][12].ToString()?.Trim() : "";
                addAccountView.AccountDetails.WebSite = customerDetails.Rows[i][13] != null ? customerDetails.Rows[i][13].ToString()?.Trim() : "";
                addAccountView.AccountDetails.BillingCycleFrequenyId = customerDetails.Rows[i][14] != null ? GetBillingCycleId(customerDetails.Rows[i][14].ToString()?.Trim(), billingFrequencies) : -1;
                addAccountView.AccountDetails.PANNumber = customerDetails.Rows[i][15] != null ? customerDetails.Rows[i][15].ToString()?.Trim() : "";
                addAccountView.AccountDetails.TANNumber = customerDetails.Rows[i][16] != null ? customerDetails.Rows[i][16].ToString()?.Trim() : "";
                addAccountView.AccountDetails.GSTNumber = customerDetails.Rows[i][17] != null ? customerDetails.Rows[i][17].ToString()?.Trim() : "";
                addAccountView.AccountDetails.CompanyRegistrationNumber = customerDetails.Rows[i][18] != null ? customerDetails.Rows[i][18].ToString()?.Trim() : "";
                addAccountView.AccountDetails.DirectorFirstName = customerDetails.Rows[i][19] != null ? customerDetails.Rows[i][19].ToString()?.Trim() : "";
                addAccountView.AccountDetails.DirectorLastName = customerDetails.Rows[i][20] != null ? customerDetails.Rows[i][20].ToString()?.Trim() : "";
                addAccountView.AccountDetails.DirectorPhoneNumber = customerDetails.Rows[i][21] != null ? customerDetails.Rows[i][21].ToString()?.Trim() : "";
                addAccountView.AccountDetails.TaxcertificateOfTheRespectiveCounty = customerDetails.Rows[i][22] != null ? customerDetails.Rows[i][22].ToString()?.Trim() : "";
                addAccountView.AccountDetails.EntityRegistrationDocuments = customerDetails.Rows[i][23] != null ? customerDetails.Rows[i][23].ToString()?.Trim() : "";
                addAccountView.AccountDetails.IsDraft = false;
                addAccountView.AccountDetails.AccountApprovedDate = DateTime.Now;


                addAccountView.AccountDetails.AccountStatusCode = "ACT";
                addAccountView.AccountDetails.CreatedBy = addAccountView.AccountDetails.AccountManagerId;
                for (int j = 1; j < customerDetails.Rows.Count; j++)
                {
                    if (customerContactDetails.Rows[j][0] != null && customerContactDetails.Rows[j][0].ToString()?.Trim() == addAccountView.AccountDetails.AccountName)
                    {
                        addAccountView.CustomerContactDetails.Add(new CustomerContactDetails()
                        {
                            ContactPersonEmailAddress = customerContactDetails.Rows[j][1].ToString()?.Trim(),
                            ContactPersonFirstName = customerContactDetails.Rows[j][2].ToString()?.Trim(),
                            ContactPersonLastName = customerContactDetails.Rows[j][3].ToString()?.Trim(),
                            ContactPersonPhoneNumber = customerContactDetails.Rows[j][4].ToString()?.Trim(),
                            CreatedBy = addAccountView.AccountDetails.AccountManagerId
                        });
                    }
                }
                accounts.Add(addAccountView);
            }
            return accounts;
        }

        private int GetBillingCycleId(string billngCycle, List<BillingCycle> billingFrequencies)
        {
            var billingCycleId = -1;
            if (!string.IsNullOrEmpty(billngCycle))
            {
                billingCycleId = billingFrequencies.Find(x => x.BillingCycleDescription == billngCycle).BillingCycleId;
                if (billingCycleId > 0)
                    return billingCycleId;
                else
                    return -1;
            }
            return -1;
        }

        private int GetStateId(string stateName, List<State> states)
        {
            var stateId = -1;
            if (!string.IsNullOrEmpty(stateName))
            {
                stateId = states.Find(x => x.StateName == stateName).StateId;
                if (stateId > 0)
                    return stateId;
                return -1;
            }
            return -1;
        }

        private int GetCountryId(string countryName, List<Country> countries)
        {
            int countryId = -1;
            if (!string.IsNullOrEmpty(countryName))
            {
                countryId = countries.Find(x => x.CountryName == countryName).CountryId;
                if (countryId > 0)
                {
                    return countryId;
                }
                return -1;
            }
            return -1;
        }

        private int GetAccountTypeId(string accountType, List<AccountType> accountTypes)
        {
            int accountTypeId = -1;
            if (!string.IsNullOrEmpty(accountType))
            {
                accountTypeId = accountTypes.Find(x => x.AccountTypeName == accountType).AccountTypeId;
                if (accountTypeId > 0)
                {
                    return accountTypeId;
                }
                return -1;
            }
            return -1;
        }

        private int GetEmployeeIdByEmail(string employeeEmailId, List<EmployeeDetail> employeeDetails)
        {
            var employee = employeeDetails.Find(x => x.EmailAddress == employeeEmailId);
            if (employee != null && employee.EmployeeID > 0)
            {
                return employee.EmployeeID;
            }
            return -1;
        }
        #endregion

        #region Delete Account
        public async Task<bool> DeleteAccount(int pAccountID)
        {
            try
            {
                AccountDetails accountDetails = _accountDetailsRepository.GetByID(pAccountID);
                if (accountDetails != null && accountDetails.AccountId > 0)
                {
                    List<CustomerContactDetails> customerContactdetailslist = _customerContactDetailsRepository.GetByID(pAccountID);
                    if (customerContactdetailslist != null && customerContactdetailslist.Count > 0)
                    {
                        foreach (CustomerContactDetails contactitem in customerContactdetailslist)
                        {
                            _customerContactDetailsRepository.Delete(contactitem);
                            await _customerContactDetailsRepository.SaveChangesAsync();
                        }
                    }
                    _accountDetailsRepository.Delete(accountDetails);
                    await _accountDetailsRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Get Account By Id
        public AccountDetailView GetAccountById(int pAccountId, int versionId, bool isLastVersion)
        {
            try
            {
                return _accountDetailsRepository.GetByAccountID(pAccountId, versionId, isLastVersion);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region List All Drafted Accounts
        public List<AccountListView> GetAllDraftAccounts(int pResourceId)
        {
            return _accountDetailsRepository.GetAllDraftAccounts(pResourceId);
        }
        #endregion

        #region List All Accounts By ResourceId
        public List<AccountListView> GetAllAccountsByResourceId(AccountInput inputData)
        {
            return _accountDetailsRepository.GetAllAccountsByResourceId(inputData);
        }
        #endregion

        #region List All Account Types
        public List<AccountType> GetAllAccountTypes()
        {
            return _accountTypeRepository.GetAccountType();
        }
        #endregion

        #region List All Countries
        public List<Country> GetAllCountry()
        {
            return _countryRepository.GetAllCountry();
        }
        #endregion

        #region List All States
        public List<State> GetAllStates()
        {
            return _stateRepository.GetAllState();
        }
        #endregion

        #region List All States By Contry Id
        public List<State> GetAllStateByCountryId(int CountryId)
        {
            return _stateRepository.GetAllStateByCountryId(CountryId);
        }
        #endregion

        #region Approve Account
        public async Task<bool> ApproveAccount(ApproveAccount pApproveAccount)
        {
            AccountDetails accountDetails = _accountDetailsRepository.GetByID(pApproveAccount.AccountId);
            if (accountDetails != null)
            {
                if (pApproveAccount.AccountStatus == "Active")
                {
                    accountDetails.AccountChanges = string.Empty;
                }
                accountDetails.AccountStatus = pApproveAccount.AccountStatus;
                accountDetails.AccountApprovedDate = DateTime.UtcNow;
                accountDetails.ModifiedBy = pApproveAccount.ApprovedBy;
                accountDetails.ModifiedOn = DateTime.UtcNow;
                _accountDetailsRepository.Update(accountDetails);
                await _accountDetailsRepository.SaveChangesAsync();
                if (pApproveAccount.Comments != "")
                {
                    AccountComments accountComments = new()
                    {
                        AccountId = pApproveAccount.AccountId,
                        Comments = pApproveAccount.Comments,
                        CreatedByName = pApproveAccount.ApprovedByName,
                        CreatedBy = pApproveAccount.ApprovedBy,
                        CreatedOn = DateTime.UtcNow
                    };
                    await _accountCommentsRepository.AddAsync(accountComments);
                    await _accountCommentsRepository.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Request Changes Account
        public async Task<bool> RequestChangesAccount(RequestChangesAccount requestChangesAccount)
        {
            AccountDetails accountDetails = _accountDetailsRepository.GetByID(requestChangesAccount.AccountId);
            if (accountDetails != null)
            {
                List<AccountChangeRequest> changeRequests = _accountChangeRequestRepository.GetByAccountID(requestChangesAccount.AccountId);
                if (changeRequests.Count > 0)
                {
                    foreach (AccountChangeRequest changeRequest in changeRequests)
                    {
                        //changeRequest.IsActive = false;
                        //changeRequest.ModifiedBy = requestChangesAccount.RejectById;
                        //changeRequest.ModifiedOn = DateTime.UtcNow;
                        _accountChangeRequestRepository.Delete(changeRequest);
                        await _accountChangeRequestRepository.SaveChangesAsync();
                    }
                }
                accountDetails.AccountStatus = requestChangesAccount.AccountStatus;
                if (requestChangesAccount.AccountRelatedIssues != null)
                {
                    foreach (int AccountRelatedIssueId in requestChangesAccount.AccountRelatedIssues)
                    {
                        AccountChangeRequest accountChangeRequest = new()
                        {
                            AccountId = requestChangesAccount.AccountId,
                            AccountRelatedIssueId = AccountRelatedIssueId,
                            CreatedBy = requestChangesAccount.CreatedById,
                            CreatedByName = requestChangesAccount.CreatedByName,
                            CreatedOn = DateTime.UtcNow,
                            IsActive = true
                        };
                        await _accountChangeRequestRepository.AddAsync(accountChangeRequest);
                    }
                    await _accountChangeRequestRepository.SaveChangesAsync();
                }
                if (requestChangesAccount.Comments != "")
                {
                    AccountComments accountComments = new()
                    {
                        AccountId = requestChangesAccount.AccountId,
                        Comments = requestChangesAccount.Comments,
                        CreatedBy = requestChangesAccount.CreatedById,
                        CreatedOn = DateTime.UtcNow,
                        CreatedByName = requestChangesAccount.CreatedByName
                    };
                    await _accountCommentsRepository.AddAsync(accountComments);
                    await _accountCommentsRepository.SaveChangesAsync();
                }
                accountDetails.ModifiedBy = requestChangesAccount.CreatedById;
                accountDetails.ModifiedOn = DateTime.UtcNow;
                _accountDetailsRepository.Update(accountDetails);
                await _accountDetailsRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion

        #region List All Account Related Issues
        public List<AccountRelatedIssue> GetAllAccountRelatedIssues()
        {
            return _accountDetailsRepository.GetAllAccountRelatedIssues();
        }
        #endregion

        #region List Account Related Issues
        public List<AccountCommentsView> GetAccountCommentsByAccountId(int pAccountId)
        {
            try
            {
                return _accountCommentsRepository.GetByAccountID(pAccountId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region List Account change requests
        public List<AccountChangeRequestView> GetAccountChangeRequestByAccountId(int pAccountId)
        {
            try
            {
                return _accountChangeRequestRepository.GetAccountChangeRequestListByAccountID(pAccountId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region List All Billing Cycle
        public List<BillingCycle> GetAllBillingCycle()
        {
            return _billingCycleRepository.GetAllBillingCycle();
        }
        #endregion

        #region Get account name by accountid
        public List<AccountNames> GetAccountNameById(List<int> accountId)
        {
            return _accountDetailsRepository.GetAccountNameById(accountId);
        }
        #endregion

        #region Get All Accounts
        public List<AccountView> GetAllAccounts(bool pIsActive = false)
        {
            return _accountDetailsRepository.GetAllAccounts(pIsActive);
        }
        #endregion

        #region Get all Accounts Details
        public List<AccountDetails> GetAllAccountsDetails()
        {
            return _accountDetailsRepository.GetAllAccountsDetails();
        }
        #endregion

        #region Get Customer On Board HomeReport
        public List<HomeReportData> GetCustomerOnBoardHomeReport(int resourceId)
        {
            return _accountDetailsRepository.GetCustomerOnBoardHomeReport(resourceId);
        }
        #endregion

        #region Get Customer Contact Info By Account Id
        public List<CustomerContactDetailsView> GetCustomerContactInformationByAccountId(int pAccountID, int versionId, bool isLastVersion)
        {
            return _customerContactDetailsRepository.GetCustomerContactInformationByAccountId(pAccountID, versionId, isLastVersion);
        }
        #endregion

        #region Assign Logo For Account
        public async Task<bool> AssignLogoForAccount(UpdateAccountLogo pUpdateAccountLogo)
        {
            try
            {
                AccountDetails accountDetails = _accountDetailsRepository.GetByID(pUpdateAccountLogo.AccountId);
                if (accountDetails != null)
                {
                    accountDetails.Logo = pUpdateAccountLogo.Logo;
                    accountDetails.ModifiedBy = pUpdateAccountLogo.ModifiedBy;
                    accountDetails.ModifiedOn = DateTime.UtcNow;
                    _accountDetailsRepository.Update(accountDetails);
                    await _accountDetailsRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Remove Customer Logo
        public async Task<bool> RemoveCustomerLogo(int pAccountId)
        {
            try
            {
                AccountDetails accountDetails = _accountDetailsRepository.GetByID(pAccountId);
                if (accountDetails != null)
                {
                    if (accountDetails.Logo != null && File.Exists(accountDetails.Logo))
                    {
                        File.Delete(accountDetails.Logo);
                    }
                    accountDetails.Logo = null;
                    _accountDetailsRepository.Update(accountDetails);
                    await _accountDetailsRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Get Country and State by Id
        public CountryNameAndStateName GetCountryAndStateById(int countryId, int stateId)
        {
            List<State> states = new List<State>();
            CountryNameAndStateName countryAndState = new();
            countryAndState.State = _stateRepository.GetByID(stateId);
            countryAndState.Country = _countryRepository.GetByID(countryId);
            return countryAndState;
        }
        #endregion

        #region Get app constant by type
        public List<KeyWithValue> GetAppConstantByType(string appConstantType)
        {
            return _appConstantsRepository.GetAppConstantByType(appConstantType);
        }
        #endregion 
        public List<StringKeyWithValue> GetAccountStatusList()
        {
            return _appConstantsRepository.GetAccountStatusList();
        }

        #region Get app constant by type
        public List<KeyWithValue> GetAllAccountName()
        {
            return _accountDetailsRepository.GetAllAccountName();
        }
        #endregion

        #region Get app constant by type
        public List<StringKeyWithValue> GetAllAccountManagerName()
        {
            return _accountDetailsRepository.GetAllAccountManagerName();
        }
        #endregion

        #region Get version list
        public List<KeyWithValue> GetAllVersionByAccountId(int accountId)
        {
            List<KeyWithValue> varsionList = _versionAccountDetailsRepository.GetVersionNameByAccountId(accountId);
            if (varsionList?.Count > 0)
            {
                KeyWithValue lastVersion = varsionList[0];
                string[] version = lastVersion.Value.Split(".");
                if (version.Length > 0)
                {
                    varsionList.Add(new KeyWithValue { Key = 0, Value = (Convert.ToInt32(version[0]) + 1).ToString() + ".0" });
                }
            }
            else
            {
                varsionList = new List<KeyWithValue>();
                varsionList.Add(new KeyWithValue { Key = 0, Value = "1.0" });
            }
            return varsionList;
        }
        #endregion

        #region Get app constant by type
        public int GetAllAccountsCount(AccountInput inputData)
        {
            return _accountDetailsRepository.GetAllAccountsCount(inputData);
        }
        #endregion

        #region Insert version Details
        public async Task<bool> InsertVersionDetails(string accountData, string customerContactData)
        {
            try
            {
                AccountDetails accountDetails = JsonConvert.DeserializeObject<AccountDetails>(accountData);
                List<CustomerContactDetails> customerContactDetails = JsonConvert.DeserializeObject<List<CustomerContactDetails>>(customerContactData);
                VersionAccountDetails versionAccountDetails = new();
                versionAccountDetails.FormattedAccountId = accountDetails.FormattedAccountId;
                versionAccountDetails.AccountId = accountDetails.AccountId;
                List<KeyWithValue> varsionList = _versionAccountDetailsRepository.GetVersionNameByAccountId(accountDetails.AccountId);
                if (varsionList?.Count > 0)
                {
                    KeyWithValue lastVersion = varsionList[0];
                    string[] version = lastVersion.Value.Split(".");
                    if (version.Length > 0)
                    {
                        versionAccountDetails.VersionName = (Convert.ToInt32(version[0]) + 1).ToString() + ".0";
                    }
                }
                else
                {
                    versionAccountDetails.VersionName = "1.0";
                }
                versionAccountDetails.AccountName = accountDetails.AccountName;
                versionAccountDetails.AccountDescription = accountDetails.AccountDescription;
                versionAccountDetails.AccountManagerId = accountDetails.AccountManagerId;
                versionAccountDetails.AccountTypeId = accountDetails.AccountTypeId;
                versionAccountDetails.AccountLocation = accountDetails.AccountLocation;
                versionAccountDetails.OfficeAddress = accountDetails.OfficeAddress;
                versionAccountDetails.CountryId = accountDetails.CountryId;
                versionAccountDetails.StateId = accountDetails.StateId;
                versionAccountDetails.PostalCode = accountDetails.PostalCode;
                versionAccountDetails.City = accountDetails.City;
                versionAccountDetails.WebSite = accountDetails.WebSite;
                versionAccountDetails.BillingCycleFrequenyId = accountDetails.BillingCycleFrequenyId;
                versionAccountDetails.PANNumber = accountDetails.PANNumber;
                versionAccountDetails.TANNumber = accountDetails.TANNumber;
                versionAccountDetails.GSTNumber = accountDetails.GSTNumber;
                versionAccountDetails.CompanyRegistrationNumber = accountDetails.CompanyRegistrationNumber;
                versionAccountDetails.DirectorFirstName = accountDetails.DirectorFirstName;
                versionAccountDetails.DirectorLastName = accountDetails.DirectorLastName;
                versionAccountDetails.DirectorPhoneNumber = accountDetails.DirectorPhoneNumber;
                versionAccountDetails.TaxcertificateOfTheRespectiveCounty = accountDetails.TaxcertificateOfTheRespectiveCounty;
                versionAccountDetails.EntityRegistrationDocuments = accountDetails.EntityRegistrationDocuments;
                versionAccountDetails.Documents = accountDetails.Documents;
                versionAccountDetails.AccountManagerName = accountDetails.AccountManagerName;
                versionAccountDetails.AccountStatus = accountDetails.AccountStatus;
                versionAccountDetails.AccountApprovedDate = accountDetails.AccountApprovedDate;
                versionAccountDetails.IsDraft = accountDetails.IsDraft;
                versionAccountDetails.FinanceManagerId = accountDetails.FinanceManagerId;
                versionAccountDetails.AccountStatusCode = accountDetails.AccountStatusCode;
                versionAccountDetails.CreatedOn = DateTime.UtcNow;
                versionAccountDetails.CreatedBy = accountDetails.CreatedBy;
                await _versionAccountDetailsRepository.AddAsync(versionAccountDetails);
                await _versionAccountDetailsRepository.SaveChangesAsync();
                if (customerContactDetails.Count > 0 && versionAccountDetails.VersionId > 0)
                {
                    foreach (var items in customerContactDetails)
                    {
                        VersionCustomerContactDetails contactInfo = new VersionCustomerContactDetails();
                        contactInfo.VersionId = versionAccountDetails.VersionId;
                        contactInfo.ContactPersonFirstName = items.ContactPersonFirstName;
                        contactInfo.ContactPersonLastName = items.ContactPersonLastName;
                        contactInfo.ContactPersonPhoneNumber = items.ContactPersonPhoneNumber;
                        contactInfo.ContactPersonEmailAddress = items.ContactPersonEmailAddress;
                        contactInfo.CreatedOn = DateTime.UtcNow;
                        contactInfo.CreatedBy = accountDetails.CreatedBy;
                        contactInfo.DesignationName = items.DesignationName;
                        contactInfo.CountryId = items.CountryId;
                        contactInfo.AddressDetail = items.AddressDetail;
                        contactInfo.CityName = items.CityName;
                        contactInfo.StateId = items.StateId;
                        contactInfo.Postalcode = items.Postalcode;
                        await _versionCustomerContactDetailsRepository.AddAsync(contactInfo);
                        await _versionCustomerContactDetailsRepository.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}