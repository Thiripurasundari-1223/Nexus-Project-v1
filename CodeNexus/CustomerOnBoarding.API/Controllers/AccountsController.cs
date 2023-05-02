using CustomerOnBoarding.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.Accounts;
using SharedLibraries.ViewModels.Home;
using System;
using System.Collections.Generic;

namespace CustomerOnBoarding.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly COBServices _cobServices;
        public AccountsController(COBServices cobServices)
        {
            _cobServices = cobServices;
        }

        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Accounts API - GET Method"
            });
        }

        #region Bulk Insert Account
        [HttpPost]
        [Route("BulkInsertAccount")]
        public IActionResult BulkInsertAccount(ImportExcelView import)
        {
            try
            {
                var result = _cobServices.BulkInsertAccount(import);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Inserted Successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/BulkInsertAccount");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Insert And Update Account
        [HttpPost]
        [Route("InsertAndUpdateAccount")]
        public IActionResult InsertAndUpdateAccount(AddAccountView accountContactDetails)
        {
            try
            {
                if (_cobServices.AccountNameDuplication(accountContactDetails.AccountDetails))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Customer Name is already exists. Please change your customer name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    int AccountId = _cobServices.InsertUpdateAccount(accountContactDetails).Result;
                    if (AccountId > 0)
                    {
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = (accountContactDetails.AccountDetails.IsDraft == true ? "Customer drafted successfully." : "Customer approval request sent successfully."),
                            Data = AccountId
                        }); ;
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Unexpected error occurred. Try again.",
                            Data = 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/InsertAndUpdateAccount", JsonConvert.SerializeObject(accountContactDetails.AccountDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }

        }
        #endregion

        #region Delete Account
        [HttpPost]
        [Route("DeleteAccount")]
        public IActionResult DeleteAccount(DeleteAccount deleteAccount)
        {
            try
            {
                if (_cobServices.DeleteAccount(deleteAccount.AccountId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Account details deleted successfully."
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/DeleteAccount", JsonConvert.SerializeObject(deleteAccount));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                });
            }
        }
        #endregion

        #region Get accounts master data
        [HttpGet]
        [Route("GetMasterDataForCreateCustomer")]
        public IActionResult GetMasterDataForCreateCustomer()
        {
            try
            {
                AccountMasterData accountMasterData = new()
                {
                    //AccountTypeList = _cobServices.GetAllAccountTypes(),
                    //StateList = _cobServices.GetAllStates(),
                    //CountryList = _cobServices.GetAllCountry(),
                    BillingCycleList = _cobServices.GetAllBillingCycle(),
                    //AccountStatusList= _cobServices.GetAppConstantByType("CustomerStatus")

                };
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = accountMasterData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetAccountsMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new AccountMasterData()
                });
            }
        }
        #endregion

        #region Get accounts master data
        [HttpGet]
        [Route("GetMasterDataForCustomerDetails")]
        public IActionResult GetMasterDataForCustomerDetails()
        {
            try
            {
                AccountMasterData accountMasterData = new()
                {
                    AccountTypeList = _cobServices.GetAllAccountTypes(),
                    CountryList = _cobServices.GetAllCountry(),
                    AccountFlowStatusList = _cobServices.GetAppConstantByType("CustomerFlowStatus"),
                    AccountStatusList = _cobServices.GetAccountStatusList(),
                    AccountNameList = _cobServices.GetAllAccountName(),
                    AccountManagerList = _cobServices.GetAllAccountManagerName()
                };
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = accountMasterData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetAccountsMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new AccountMasterData()
                });
            }
        }
        #endregion

        #region Get Account By Id
        [HttpGet]
        [Route("GetAccountById")]
        public IActionResult GetAccountById(int pAccountId, int versionId, bool isLastVersion)
        {
            try
            {
                AccountDetailView accountDetails = _cobServices.GetAccountById(pAccountId, versionId, isLastVersion);
                accountDetails.accountComments = isLastVersion == false ? null : _cobServices.GetAccountCommentsByAccountId(pAccountId);
                accountDetails.accountChangeRequestLists = isLastVersion == false ? null : _cobServices.GetAccountChangeRequestByAccountId(pAccountId);
                accountDetails.CustomerContactDetailsView = _cobServices.GetCustomerContactInformationByAccountId(pAccountId, versionId, isLastVersion);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = accountDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetAccountById", Convert.ToString(pAccountId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new AccountDetailView()
                });
            }
        }
        #endregion

        #region List All Drafted Accounts
        [HttpGet]
        [Route("ListAllDraftAccounts")]
        public IActionResult ListAllDraftAccounts(int pResourceId)
        {
            try
            {
                List<AccountListView> lstAccountDetails = _cobServices.GetAllDraftAccounts(pResourceId);
                //string userRole = _cobServices.GetUserRoleByResourceId(pResourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = lstAccountDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/ListAllDraftAccounts", Convert.ToString(pResourceId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<AccountListView>()
                });
            }
        }
        #endregion

        #region List All Accounts By ResourceId
        [HttpPost]
        [Route("ListAllAccountsByResourceId")]
        public IActionResult ListAllAccountsByResourceId(AccountInput inputData)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _cobServices.GetAllAccountsByResourceId(inputData)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/ListAllAccountsByResourceId", JsonConvert.SerializeObject(inputData));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AccountListView>()
                });
            }
        }
        #endregion

        #region List All States By Country Id
        [HttpGet]
        [Route("ListAllStateByCountryId")]
        public IActionResult ListAllStateByCountryId(int CountryId)
        {
            try
            {
                List<State> states = _cobServices.GetAllStateByCountryId(CountryId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = states
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/ListAllStateByCountryId", Convert.ToString(CountryId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<State>()
                });
            }
        }
        #endregion

        #region Approve Account
        [HttpPost]
        [Route("ApproveAccount")]
        public IActionResult ApproveAccount(ApproveAccount pApproveAccount)
        {
            try
            {
                if (_cobServices.ApproveAccount(pApproveAccount).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Customer approved successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/ApproveAccount", JsonConvert.SerializeObject(pApproveAccount));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion

        #region Request Changes Account
        [HttpPost]
        [Route("RequestChangesAccount")]
        public IActionResult RequestChangesAccount(RequestChangesAccount requestChangesAccount)
        {
            try
            {
                if (_cobServices.RequestChangesAccount(requestChangesAccount).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Customer change request(s) are saved successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Dat = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/RequestChangesAccount", JsonConvert.SerializeObject(requestChangesAccount));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion

        #region List All Account Related Issues
        [HttpGet]
        [Route("ListAllAccountRelatedIssues")]
        public IActionResult ListAllAccountRelatedIssues()
        {
            try
            {
                List<AccountRelatedIssue> lstAccountRelatedIssueList = _cobServices.GetAllAccountRelatedIssues();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = lstAccountRelatedIssueList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/ListAllAccountRelatedIssues");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<AccountRelatedIssue>()
                });
            }
        }
        #endregion        

        #region Get account name by id
        [HttpPost]
        [Route("GetAccountNameById")]
        public IActionResult GetAccountNameById(List<int> accountId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _cobServices.GetAccountNameById(accountId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetAccountNameById", JsonConvert.SerializeObject(accountId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AccountNames>()
                });
            }
        }
        #endregion

        #region Get All Accounts 
        [HttpGet]
        [Route("GetAllAccounts")]
        public IActionResult GetAllAccounts(bool pIsActive = false)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _cobServices.GetAllAccounts(pIsActive)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetAllAccounts", pIsActive.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AccountView>()
                });
            }
        }
        #endregion

        #region Get All Accounts Details
        [HttpGet]
        [Route("GetAllAccountsDetails")]
        public IActionResult GetAllAccountsDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _cobServices.GetAllAccountsDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetAllAccountsDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AccountDetails>()
                });
            }
        }
        #endregion

        #region Get Customer On Board HomeReport
        [HttpGet]
        [Route("GetCustomerOnBoardHomeReport")]
        public IActionResult GetCustomerOnBoardHomeReport(int resourceId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _cobServices.GetCustomerOnBoardHomeReport(resourceId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetCustomerOnBoardHomeReport");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<HomeReportData>()
                });
            }
        }
        #endregion

        #region Get Employee Country master data
        [HttpGet]
        [Route("GetEmployeeCountryMasterData")]
        public IActionResult GetEmployeeCountryMasterData()
        {
            try
            {
                List<Country> countries = new List<Country>();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _cobServices.GetAllCountry()
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetEmployeeCountryMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new Country()
                });
            }
        }
        #endregion

        #region Assign Logo For Account
        [HttpPost]
        [Route("AssignLogoForAccount")]
        public IActionResult AssignLogoForAccount(UpdateAccountLogo updateAccountLogo)
        {
            try
            {
                if (_cobServices.AssignLogoForAccount(updateAccountLogo).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Customer Logo assigned successfully.",
                        Data = updateAccountLogo.AccountId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/AssignLogoForAccount", JsonConvert.SerializeObject(updateAccountLogo));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "Unexpected error occurred. Try again.",
                Data = 0
            });
        }
        #endregion

        #region Remove Customer Logo
        [HttpPost]
        [Route("RemoveCustomerLogo")]
        public IActionResult RemoveCustomerLogo(RemoveLogoAccount removeLogoAccount)
        {
            try
            {
                if (_cobServices.RemoveCustomerLogo(removeLogoAccount.AccountId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Customer Logo removed successfully."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/AssignLogoForAccount", JsonConvert.SerializeObject(removeLogoAccount));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "Unexpected error occurred. Try again."
            });
        }
        #endregion

        #region Get Country and State data
        [HttpGet]
        [Route("GetCountryAndStateData")]
        public IActionResult GetCountryAndStateData()
        {
            try
            {
                List<State> states = _cobServices.GetAllStates();
                List<Country> countries = _cobServices.GetAllCountry();
                CountryAndState countryAndState = new()
                {
                    StateList = states,
                    CountryList = countries,
                };
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = countryAndState
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetCountryAndStateData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new CountryAndState()
                });
            }
        }
        #endregion

        #region Get Country and State by Id
        [HttpGet]
        [Route("GetCountryAndStateById")]
        public IActionResult GetCountryAndStateById(int countryId, int stateId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _cobServices.GetCountryAndStateById(countryId, stateId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetCountryAndStateById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new CountryNameAndStateName()
                });
            }
        }
        #endregion

        #region List All Accounts By ResourceId
        [HttpPost]
        [Route("GetAllAccountsCount")]
        public IActionResult GetAllAccountsCount(AccountInput inputData)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _cobServices.GetAllAccountsCount(inputData)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetAllAccountsCount", JsonConvert.SerializeObject(inputData));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Version list
        [HttpGet]
        [Route("GetAllVersionByAccountId")]
        public IActionResult GetAllVersionByAccountId(int accountId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _cobServices.GetAllVersionByAccountId(accountId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Accounts", "Accounts/GetAllVersionByAccountId", JsonConvert.SerializeObject(accountId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = 0
                });
            }
        }
        #endregion  
    }
}