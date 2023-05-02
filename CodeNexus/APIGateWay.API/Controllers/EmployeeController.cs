using APIGateWay.API.Common;
using APIGateWay.API.Model;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Attendance;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Employee;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;
using SharedLibraries.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
     [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _projectBaseURL = string.Empty;
        private readonly string _accountsBaseURL = string.Empty;
        private readonly string _appraisalBaseURL = string.Empty;
        private readonly string _timesheetBaseURL = string.Empty;
        private readonly string _attendanceBaseURL = string.Empty;
        private readonly string _leaveBaseURL = string.Empty;
        private readonly string _policyMgmtBaseURL = string.Empty;
        private readonly string _notificationBaseURL = string.Empty;
        private readonly HTTPClient _client;
        private readonly string strErrorMsg = "Something went wrong, please try again later";
        private readonly CommonFunction _commonFunction;

        #region Constructor
        public EmployeeController(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _configuration = configuration;
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _projectBaseURL = _configuration.GetValue<string>("ApplicationURL:Projects:BaseURL");
            _accountsBaseURL = _configuration.GetValue<string>("ApplicationURL:Accounts:BaseURL");
            _appraisalBaseURL = _configuration.GetValue<string>("ApplicationURL:Appraisal:BaseURL");
            _timesheetBaseURL = _configuration.GetValue<string>("ApplicationURL:Timesheet:BaseURL");
            _attendanceBaseURL = _configuration.GetValue<string>("ApplicationURL:Attendance:BaseURL");
            _leaveBaseURL = _configuration.GetValue<string>("ApplicationURL:Leaves:BaseURL");
            _notificationBaseURL = _configuration.GetValue<string>("ApplicationURL:Notification:BaseURL");
            _policyMgmtBaseURL = _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:BaseURL");
            _commonFunction = new CommonFunction(configuration);
        }
        #endregion

        private bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        #region Get employee list        
        [HttpGet]
        [Route("GetEmployeeDropDownList")]
        public IActionResult GetEmployeeDropDownList(bool isAll=true)
        {
            List<EmployeeDataForDropDown> employeeList = new List<EmployeeDataForDropDown>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDropDownList") + isAll);
                employeeList = JsonConvert.DeserializeObject<List<EmployeeDataForDropDown>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeList = employeeList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeDropDownList" + isAll.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeList = employeeList
            });
        }
        #endregion

        #region Get Department list        
        [HttpGet]
        [Route("GetDepartmentDropDownList")]
        public IActionResult GetDepartmentDropDownList()
        {
            List<Department> depList = new List<Department>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetDepartmentDropDownList"));
                depList = JsonConvert.DeserializeObject<List<Department>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        DepartmentList = depList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetDepartmentDropDownList");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeList = depList
            });
        }
        #endregion

        #region Get Designation list        
        [HttpGet]
        [Route("GetDesignationDropDownList")]
        public IActionResult GetDesignationDropDownList()
        {
            List<Designation> desList = new List<Designation>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetDesignationDropDownList"));
                desList = JsonConvert.DeserializeObject<List<Designation>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        DesignationList = desList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetDesignationDropDownList");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeList = desList
            });
        }
        #endregion

        #region Get Location list        
        [HttpGet]
        [Route("GetLocationDropDownList")]
        public IActionResult GetLocationDropDownList()
        {
            List<EmployeeLocation> locList = new List<EmployeeLocation>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetLocationDropDownList"));
                locList = JsonConvert.DeserializeObject<List<EmployeeLocation>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        LocationList = locList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetLocationDropDownList");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeList = locList
            });
        }
        #endregion

        #region Add or update employees        
        [HttpPost]
        [Route("AddOrUpdateEmployee")]
        public  IActionResult AddOrUpdateEmployee(EmployeesViewModel employee)
        {
            //string msg = "";
            EmployeeDetailsForLeaveView employees = new EmployeeDetailsForLeaveView();
            try
            {
                SupportingDocumentsView supportingDocuments = new();
                supportingDocuments.SourceType = _configuration.GetValue<string>("Employee");
                supportingDocuments.BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                supportingDocuments.CreatedBy = employee.Employee.CreatedBy == null ? 0 : (int)employee.Employee.CreatedBy;
                employee.supportingDocumentsViews = supportingDocuments;
                var result =  _client.PostAsJsonAsync(employee, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AddOrUpdateEmployee"));
                EmployeeNotificationData data =  JsonConvert.DeserializeObject<EmployeeNotificationData>(JsonConvert.SerializeObject(result?.Result?.Data));
                //if (employee?.Employee?.EmployeeID>0)
                //{
                //    bool isActive = true;
                //    if(employee?.Employee?.DateOfRelieving?.Date < DateTime.Now.Date)
                //    {
                //        isActive = false;
                //    }
                //    EmployeeStatusView employeeStatus = new EmployeeStatusView();
                //    employeeStatus.EmployeeId = employee.Employee.EmployeeID;
                //    employeeStatus.IsEnabled = isActive;
                //    employeeStatus.ModifiedBy = employee.Employee.ModifiedBy;
                //    var employeeResult = _client.PostAsJsonAsync(employeeStatus, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeeStatus"));
                //    bool? isUpdate = JsonConvert.DeserializeObject<bool?>(JsonConvert.SerializeObject(employeeResult?.Result?.Data));

                //}

                if (result != null)
                {
                    if (result?.Result?.StatusCode == "SUCCESS")
                    {

                        // int EmployeeID = Convert.ToInt32(msg);
                        //var results = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetNewEmployeeDetailsbyID") + data);
                        //employees = JsonConvert.DeserializeObject<EmployeeDetailsForLeaveView>(JsonConvert.SerializeObject(results?.Result?.Data));
                        EmployeeDetailsForLeaveView employeesDetails = new EmployeeDetailsForLeaveView
                        {
                            EmployeeID = data.EmployeeId,
                            EmployeeTypeID = employee.Employee.EmployeeTypeId,
                            DepartmentID = employee.Employee.DepartmentId,
                            RoleID = employee.Employee.RoleId,
                            IsActive = employee.Employee.IsActive,
                            Gender = employee.Employee.Gender,
                            LocationID = employee.Employee.LocationId,
                            MaritalStatus = employee.Employee.Maritalstatus,
                            DesignationID = employee.Employee.DesignationId,
                            DateOfJoining = employee.Employee.DateOfJoining,
                            DateOfContract = employee.Employee.DateOfContract,
                            ProbationStatusID = employee.Employee.ProbationStatusId
                        };
                        if (employee?.Employee?.EmployeeID==0 || data.checkLeave == true)
                        {
                            var empleave = _client.PostAsJsonAsync(employeesDetails, _leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:AddNewEmployeeLeave"));
                            //var resp = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(empleave?.Result?.Data));
                        }
                        if (data != null && employee?.IsTriggerNotification==true)
                        {
                            if (data?.OldManagerDetails?.ManagerID!=null&&data?.NewManagerDetails?.ManagerID!=null&& data?.OldManagerDetails?.ManagerID!= data?.NewManagerDetails?.ManagerID)
                            {
                                 EmailNotificationForNewManager(data);
                               
                            }
                            if(data?.OldEmployeeDesignationDetails?.DesignationId!=null&&data?.NewEmployeeDesignationDetails?.DesignationId!=null&& data?.OldEmployeeDesignationDetails?.DesignationId!= data?.NewEmployeeDesignationDetails?.DesignationId)
                            {
                                EmailNotificationForNewDesignation(data);
                            }
                            if (data?.OldEmployeeBaseWorkLocationDetails?.LocationId != null && data?.NewEmployeeBaseWorkLocationDetails?.LocationId!= null && data?.OldEmployeeBaseWorkLocationDetails?.LocationId!= data?.NewEmployeeBaseWorkLocationDetails?.LocationId)
                            {
                                EmailNotificationForNewBaseWorkLocation(data);

                            }
                            if (data?.OldEmployeeProbationStatusDetails?.ProbationStatusId != null && data?.NewEmployeeProbationStatusDetails?.ProbationStatusId != null && data?.OldEmployeeProbationStatusDetails?.ProbationStatusId != data?.NewEmployeeProbationStatusDetails?.ProbationStatusId)
                            {
                                EmailNotificationForNewProbationStatus(data);
                            }
                        }
                        return Ok(new
                        {
                            StatusCode=result?.Result?.StatusCode,
                            StatusText = (employee?.Employee?.EmployeeID > 0 ? "Employee updated successfully." : "Employee added successfully."),
                            EmployeeId = data.EmployeeId
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = result?.Result?.StatusText,
                            EmployeeId = 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/AddOrUpdateEmployee", JsonConvert.SerializeObject(employee));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeId = 0
            });
        }
        #endregion

        #region Get employee details by employee id        
        [HttpGet]
        [Route("GetEmployeeDetailsByEmployeeId")]
        public IActionResult GetEmployeeDetailsByEmployeeId(int employeeId)
        {
            EmployeesViewModel employee = new EmployeesViewModel();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDetailsByEmployeeId") + employeeId);
                employee = JsonConvert.DeserializeObject<EmployeesViewModel>(JsonConvert.SerializeObject(result?.Result?.Data));
                
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeDetails = employee
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeDetailsByEmployeeId", Convert.ToString(employeeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employee
            });
        }
        #endregion

        #region Get employee master data        
        [HttpGet]
        [Route("GetEmployeeMasterData")]
        public async Task<IActionResult> GetEmployeeMasterData()
        {
            EmployeeMasterData masterData = new EmployeeMasterData();
            try
            {
                var result =await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeMasterData"));
                masterData = JsonConvert.DeserializeObject<EmployeeMasterData>(JsonConvert.SerializeObject(result?.Data));
                //var countryResult = _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetEmployeeCountryMasterData"));
                //List<Country> masterDatas = JsonConvert.DeserializeObject<List<Country>>(JsonConvert.SerializeObject(countryResult?.Result?.Data));
                //if (masterDatas?.Count > 0)
                //{
                //    masterData.CountryList = masterDatas;
                //}
                var shiftResult = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeShiftMasterData"));
                List<EmployeeShiftMasterData> employeeShiftMasterData = JsonConvert.DeserializeObject<List<EmployeeShiftMasterData>>(JsonConvert.SerializeObject(shiftResult?.Data));
                if (employeeShiftMasterData?.Count > 0)
                {
                    masterData.EmployeeShiftMasterDataList = employeeShiftMasterData;
                }
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        EmployeeMasterData = masterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeMasterData = masterData
            });
        }
        #endregion

        #region Get employee master data for org chart       
        [HttpGet]
        [Route("GetEmployeeMasterDataForOrgChart")]
        public IActionResult GetEmployeeMasterDataForOrgChart()
        {
            EmployeeMasterDataForOrgChart masterData = new EmployeeMasterDataForOrgChart();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeMasterDataForOrgChart"));
                masterData = JsonConvert.DeserializeObject<EmployeeMasterDataForOrgChart>(JsonConvert.SerializeObject(result?.Result?.Data));
                //var countryResult = _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetEmployeeCountryMasterData"));
                //List<Country> masterDatas = JsonConvert.DeserializeObject<List<Country>>(JsonConvert.SerializeObject(countryResult?.Result?.Data));
                //if (masterDatas?.Count > 0)
                //{
                //    masterData.CountryList = masterDatas;
                //}
                //var shiftResult = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeShiftMasterData"));
                //List<EmployeeShiftMasterData> employeeShiftMasterData = JsonConvert.DeserializeObject<List<EmployeeShiftMasterData>>(JsonConvert.SerializeObject(shiftResult?.Result?.Data));
                //if (employeeShiftMasterData?.Count > 0)
                //{
                //    masterData.EmployeeShiftMasterDataList = employeeShiftMasterData;
                //}
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeMasterData = masterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeMasterData = masterData
            });
        }
        #endregion

        #region Add or update department        
        [HttpPost]
        [Route("AddOrUpdateDepartment")]
        public IActionResult AddOrUpdateDepartment(Department department)
        {
            int DepartmentId = 0;
            try
            {
                var result = _client.PostAsJsonAsync(department, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AddOrUpdateDepartment"));
                DepartmentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.Result?.StatusCode,
                        result?.Result?.StatusText,
                        DepartmentId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/AddOrUpdateDepartment", JsonConvert.SerializeObject(department));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                DepartmentId
            });
        }
        #endregion

        #region Add or update role        
        [HttpPost]
        [Route("InsertOrUpdateRole")]
        public IActionResult InsertOrUpdateRole(Roles role)
        {
            int RoleId = 0;
            try
            {
                var result = _client.PostAsJsonAsync(role, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:InsertOrUpdateRole"));
                RoleId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.Result?.StatusCode,
                        result?.Result?.StatusText,
                        RoleId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/InsertOrUpdateRole", JsonConvert.SerializeObject(role));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                RoleId
            });
        }
        #endregion

        #region Get employee list        
        [HttpGet]
        [Route("GetEmployeesList")]
        public IActionResult GetEmployeesList()
        {
            List<EmployeeDetail> employeeList = new List<EmployeeDetail>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeesList"));
                employeeList = JsonConvert.DeserializeObject<List<EmployeeDetail>>(JsonConvert.SerializeObject(result?.Result?.Data));               
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeList = employeeList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesList");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeList = employeeList
            });
        }
        #endregion

        #region delete employee        
        [HttpGet]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int employeeid, int modifiedBy)
        {
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_employeeBaseURL)
                };
                HttpResponseMessage response = await client.DeleteAsync("Employee/DeleteEmployee?employeeid=" + employeeid + "&modifiedBy=" + modifiedBy);
                if (response?.IsSuccessStatusCode == true)
                {
                    var result = response.Content.ReadAsAsync<SuccessData>();
                    if (result != null)
                    {
                        return Ok(new
                        {
                            result.Result.StatusCode,
                            StatusText = "Employee deleted successfully."
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = strErrorMsg
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = strErrorMsg
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/DeleteEmployee", " EmployeeId- " + employeeid.ToString() + " ModifiedBy- " + modifiedBy.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg
                });
            }
        }
        #endregion

        #region Employee Login 
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(Login loginParam)
        {
            string statusText = "";
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_employeeBaseURL)
                };
                HttpResponseMessage response = await client.PostAsJsonAsync("Employee/Login", loginParam);
                if (response?.IsSuccessStatusCode == true)
                {
                    var result = response?.Content?.ReadAsAsync<SuccessData>()?.Result;
                    string data = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(result?.Data));
                    if (result != null && !string.IsNullOrEmpty(result?.StatusCode) && !string.IsNullOrEmpty(data) && result?.StatusCode == "SUCCESS")
                    {
                        UserToken userTokenWithData = JsonConvert.DeserializeObject<UserToken>(data);
                        DiscoveryDocumentResponse tokenEndpoint = await GetDiscoveryDocumentAsync();
                        if (tokenEndpoint.IsError)
                        {
                            statusText = tokenEndpoint?.Error;
                        }
                        else
                        {
                            TokenResponse tokenResult = await GetIdentityAccessToken(tokenEndpoint);
                            userTokenWithData.AccessToken = tokenResult?.AccessToken;
                            userTokenWithData.RefreshToken = "";
                            int shiftId = userTokenWithData.userShiftId == null ? 0 :(int) userTokenWithData.userShiftId;
                            var results = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetDefaultShiftDetailsById")+ shiftId);
                            EmployeeShiftDetailsView employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDetailsView>(JsonConvert.SerializeObject(results?.Result?.Data));
                            if (employeeShift !=null)
                            {
                                userTokenWithData.userShiftId = employeeShift?.ShiftDetailsId;
                                userTokenWithData.ShiftFromTime = employeeShift?.ShiftFromTime;
                                userTokenWithData.ShiftToTime = employeeShift?.ShiftToTime;
                                userTokenWithData.WeekendId = employeeShift?.WeekendId;
                                userTokenWithData.IsFlexyShift = employeeShift?.IsFlexyShift==null?false:(bool)employeeShift.IsFlexyShift;
                            }
                            int? UserId = userTokenWithData.UserId;
                            if (UserId > 0)
                            {
                                int? LocationId = 0, RoleId = 0, DepartmentId = 0, CurrentWorkPlaceId = 0, CurrentWorkLocationId = 0;
                                EmployeeBasicInfoView employee = new();
                                var employeeInfo = await _client.GetAsync(_employeeBaseURL,
                                                _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeBasicInfoByEmployeeID") + UserId);
                                employee = JsonConvert.DeserializeObject<EmployeeBasicInfoView>(JsonConvert.SerializeObject(employeeInfo?.Data));
                                if (employee != null)
                                {
                                    LocationId = employee.Employee.LocationId ?? 0;
                                    RoleId = employee.Employee.RoleId ?? 0;
                                    DepartmentId = employee.Employee.DepartmentId ?? 0;
                                    CurrentWorkLocationId = employee.Employee.CurrentWorkLocationId ?? 0;
                                    CurrentWorkPlaceId = employee.Employee.CurrentWorkPlaceId ?? 0;
                                }
                                result = await _client.GetAsync(_policyMgmtBaseURL,
                                                    _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetPolicyAcknowledgementByEmployee") + UserId +
                                                    "&LocationId=" + LocationId + "&RoleId=" + RoleId + "&DepartmentId=" + DepartmentId +
                                                    "&CurrentWorkLocationId=" + CurrentWorkLocationId + "&CurrentWorkPlaceId=" + CurrentWorkPlaceId);
                                int PolicyDocumentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                                userTokenWithData.PolicyDocumentId = PolicyDocumentId;
                                userTokenWithData.IsPolicyRequiredToAcknowledge = PolicyDocumentId > 0;
                            }
                            return Ok(new
                            {
                                isSuccess = true,
                                Data = userTokenWithData,
                                message = ""
                            });
                        }
                    }
                    else if (!string.IsNullOrEmpty(result?.StatusText))
                    {
                        statusText = result?.StatusText;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/Login");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                isSuccess = false,
                Data = "",
                message = statusText
            });
        }
        private static async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync()
        {
            try
            {
                var client = new HttpClient();
                return await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            }
            catch (Exception e)
            {
                LoggerManager.LoggingErrorTrack(e, "APIGateWay", "Employee/GetDiscoveryDocumentAsync");
                throw;
            }
        }
        private static async Task<TokenResponse> GetIdentityAccessToken(DiscoveryDocumentResponse disco)
        {
            try
            {
                var client = new HttpClient();
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "nexus_ui",
                    ClientSecret = "Nexus@Tvsn2021",
                    Scope = "NexusAPI"
                });
                return tokenResponse;
            }
            catch (Exception e)
            {
                LoggerManager.LoggingErrorTrack(e, "APIGateWay", "Employee/GetIdentityAccessToken");
                throw;
            }
        }
        private async Task<TokenResponse> GetIdentityRefreshToken(DiscoveryDocumentResponse disco)
        {
            try
            {
                var client = new HttpClient();
                var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "nexus_ui",
                    ClientSecret = "Nexus@Tvsn2021",
                    Scope = "NexusAPI",
                    RefreshToken = Guid.NewGuid().ToString()
                });
                return tokenResponse;
            }
            catch (Exception e)
            {
                LoggerManager.LoggingErrorTrack(e, "APIGateWay", "Employee/GetIdentityAccessToken");
                throw;
            }
        }
        #endregion

        #region Insert Or Update Role Permissions
        [HttpPost("InsertOrUpdateRolePermissions")]

        public IActionResult InsertOrUpdateRolePermissions(List<RolePermissions> rolePermissions)
        {
            int IsSuccess = 0;
            try
            {
                var result = _client.PostAsJsonAsync(rolePermissions, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:InsertOrUpdateRolePermissions"));
                IsSuccess = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        IsSuccess
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/InsertOrUpdateRolePermissions", JsonConvert.SerializeObject(rolePermissions));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                IsSuccess
            });
        }
        #endregion

        #region Get Role Permission's By Email or Role Id
        [HttpGet("GetRolePermissionsByEmailOrRoleId")]
        public IActionResult GetRolePermissionsByEmail(string email, int pRoleId)
        {
            List<RolesDetail> rolePermissionViews = new List<RolesDetail>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetRolePermissionsByEmailOrRoleId") + email + "&pRoleId=" + pRoleId);
                rolePermissionViews = JsonConvert.DeserializeObject<List<RolesDetail>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        Data = rolePermissionViews
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetRolePermissionsByEmail", " email- " + email + " RoleId- " + pRoleId.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = rolePermissionViews
            });
        }
        #endregion

        #region Get Module Wise Feature Details
        [HttpGet("GetModuleWiseFeatureDetails")]
        public IActionResult GetModuleWiseFeatureDetails()
        {
            List<ModuleWiseFeatureDetails> moduleWiseFeatureDetails = new List<ModuleWiseFeatureDetails>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetModuleWiseFeatureDetails"));
                moduleWiseFeatureDetails = JsonConvert.DeserializeObject<List<ModuleWiseFeatureDetails>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        Data = moduleWiseFeatureDetails
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetModuleWiseFeatureDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = moduleWiseFeatureDetails
            });
        }
        #endregion

        #region Get employees master data for search         
        [HttpGet]
        [Route("GetEmployeesMasterDataForSearch")]
        public IActionResult GetEmployeesMasterDataForSearch()
        {
            List<SearchEmployeesMasterDataViewModel> employeeList = new List<SearchEmployeesMasterDataViewModel>();
            try
            {
                var empResult = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesMasterDataForSearch"));
                employeeList = JsonConvert.DeserializeObject<List<SearchEmployeesMasterDataViewModel>>(JsonConvert.SerializeObject(empResult?.Result?.Data));
                if (empResult != null && employeeList?.Count > 0)
                {
                    var empAllResult = _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetResourceUtilisationData"));
                    List<ReportData> employeeUtilisationData = JsonConvert.DeserializeObject<List<ReportData>>(JsonConvert.SerializeObject(empAllResult?.Result?.Data));
                    employeeList?.ForEach(x => x.Availability = (100 - employeeUtilisationData.Where(y => y.Id == x.EmployeeId).Select(x => x.Count).FirstOrDefault()));
                    return Ok(new
                    {
                        empResult?.Result?.StatusCode,
                        empResult?.Result?.StatusText,
                        EmployeeList = employeeList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesMasterDataForSearch");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeList = employeeList
            });
        }
        #endregion

        #region Get Module Description List 
        [HttpGet]
        [AllowAnonymous]
        [Route("GetModuleDescription")]
        public async Task<IActionResult> GetModuleDescriptionAsync()
        {
            List<Modules> moduleDescriptions = new List<Modules>();
            try
            {
                //try
                //{
                //    var resultAccounts = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetEmptyMethod"));
                //    var resultProjects = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetEmptyMethod"));
                //    var resultAppraisal = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetEmptyMethod"));
                //    var resultTimesheet = await _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetEmptyMethod"));
                //    var resultIAM = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmptyMethodIAM"));
                //    var resultEmployee = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmptyMethod"));
                //    var resultLeave = await _client.GetAsync(_leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmptyMethod"));
                //    var resultAttendance = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmptyMethod"));
                //    var resultNotifications = await _client.GetAsync(_notificationBaseURL, _configuration.GetValue<string>("ApplicationURL:Notification:GetEmptyMethod"));
                //}
                //catch (Exception ex) { LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetModuleDescription"); }
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetModuleDescription"));
                moduleDescriptions = JsonConvert.DeserializeObject<List<Modules>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        Data = moduleDescriptions
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetModuleDescription");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = moduleDescriptions
            });
        }
        #endregion

        #region Sync User From AD
        [HttpPost]
        [Route("SyncUserFromAD")]
        public async Task<IActionResult> SyncUserFromADAsync(ADUserToken userToken)
        {
            try
            {
                var shiftResult = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftIdByName") + "General shift");
                int ShiftDetailsId = 0;
                ShiftDetailsId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(shiftResult?.Result?.Data));
                userToken.pShiftDetailsId = ShiftDetailsId;
                var result = await _client.PostAsJsonAsync(userToken, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:SyncUserFromAD"));
                if (JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data)))
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        Data = "Employee(s) are synced successfully from Azure AD."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/SyncUserFromAD");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                Data = strErrorMsg
            });
        }
        #endregion

        #region Add or update EmployeeCategory
        [HttpPost]
        [Route("AddOrUpdateEmployeeCategory")]
        public IActionResult AddOrUpdateEmployeeCategory(EmployeeCategory employeeCategory)
        {
            int EmployeeCategoryId = 0;
            try
            {
                var result = _client.PostAsJsonAsync(employeeCategory, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AddOrUpdateEmployeeCategory"));
                EmployeeCategoryId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null && EmployeeCategoryId > 0)
                {
                    return Ok(new
                    {
                        result?.Result?.StatusCode,
                        result?.Result?.StatusText,
                        EmployeeCategoryId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/AddOrUpdateEmployeeCategory", JsonConvert.SerializeObject(employeeCategory));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeCategoryId
            });
        }
        #endregion
        #region      
        [HttpGet]
        [Route("GetEmployeeShiftDetails")]
        public IActionResult GetEmployeeShiftDetails(int employeeID)
        {
            List<EmployeeShiftDetailsView> employee = new();
            EmployeeShiftDetailsView employeeShift = new();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeShiftDetails") + employeeID);
                employee = JsonConvert.DeserializeObject<List<EmployeeShiftDetailsView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (employee?.Count == 0 || employee == null)
                {
                    var results = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetDefaultShiftId"));
                    employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDetailsView>(JsonConvert.SerializeObject(results?.Result?.Data));
                    if (employeeShift?.ShiftDetailsId > 0)
                    {
                        return Ok(new
                        {
                            results?.Result?.StatusCode,
                            results?.Result?.StatusText,
                            EmployeeDetails = employeeShift 
                        });
                    }
                }
                else
                {
                    if (result != null)
                    {
                        return Ok(new
                        {
                            result.Result.StatusCode,
                            result.Result.StatusText,
                            EmployeeDetails = employee
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeShiftDetails", Convert.ToString(employeeID));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employee
            });
        }
        #endregion

        #region Add or update system role        
        [HttpPost]
        [Route("InsertOrUpdateSystemRole")]
        public IActionResult InsertOrUpdateSystemRole(SystemRoles role)
        {
            int RoleId = 0;
            try
            {
                var result = _client.PostAsJsonAsync(role, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:InsertOrUpdateSystemRole"));
                RoleId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.Result?.StatusCode,
                        result?.Result?.StatusText,
                        RoleId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/InsertOrUpdateSystemRole", JsonConvert.SerializeObject(role));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                RoleId
            });
        }
        #endregion

        #region Update Employee Status 
        [HttpPost]
        [Route("UpdateEmployeeStatus")]
        public async Task<IActionResult> UpdateEmployeeStatus(EmployeeStatusView employeeStatus)
        {
            try
            {
                var result = _client.PostAsJsonAsync(employeeStatus, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeeStatus"));
                bool isUpdate = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = isUpdate
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/UpdateEmployeeStatus");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion
        #region Deactivate The Employee After Resignation Checklist Details
        [HttpGet]
        [Route("DeactivateEmployeeStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> DeactivateEmployeeStatus()
        {

            List<int> employeeList = new List<int>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:DeactivateEmployeeStatus"));
                bool satus = JsonConvert.DeserializeObject<Boolean>(JsonConvert.SerializeObject(result.Result.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = satus
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employees/DeactivateEmployeeStatus");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = employeeList
                });
            }
        }
        #endregion


        #region Bulk Insert Employee
        [HttpPost]
        [Route("BulkInsertEmployee")]
        public async Task<IActionResult> BulkInsertEmployee(BulkUploadClass uploadFile)
        {
            try
            {
                if (uploadFile.DocumentSize > 0)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var ms = new MemoryStream())
                    {
                        //uploadFile.CopyTo(ms);
                        ImportEmployeeExcelView import = new ImportEmployeeExcelView();
                        import.Base64Format = uploadFile.DocumentAsBase64.Substring(uploadFile.DocumentAsBase64.IndexOf(",") + 1);
                        import.UploadedBy = uploadFile.UploadedBy;
                        var result = await _client.PostAsJsonAsync(import, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:BulkInsertEmployee"));
                        var outputResult = JsonConvert.DeserializeObject<List<ImportDataStatus>>(JsonConvert.SerializeObject(result?.Data));
                        var memoryStream = new MemoryStream();

                        var stream = new MemoryStream();
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        byte[] bytes;
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                            workSheet.Cells.LoadFromCollection(outputResult, true);
                            package.Save();
                            bytes = package.GetAsByteArray();
                        }
                        //stream.Position = 0;
                        //string excelName = $"Import Result-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                       
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "Employee List Inserted Successfully",
                            BaseString = Convert.ToBase64String(bytes),
                            ContentType = contentType ?? "application/octet-stream"
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Empty File is uploaded",

                    });
                }
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = strErrorMsg
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/BulkInsertEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg
                });
            }
        }
        #endregion

        #region Bulk Update Employee
        [HttpPost]
        [Route("BulkUpdateEmployee")]
        public async Task<IActionResult> BulkUpdateEmployee(BulkUploadClass uploadFile)
        {
            try
            {
                if (uploadFile.DocumentSize > 0)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var ms = new MemoryStream())
                    {
                        //uploadFile.CopyTo(ms);
                        ImportEmployeeExcelView import = new ImportEmployeeExcelView();
                        import.Base64Format = uploadFile.DocumentAsBase64.Substring(uploadFile.DocumentAsBase64.IndexOf(",") + 1);
                        import.UploadedBy = uploadFile.UploadedBy;
                        var result = await _client.PostAsJsonAsync(import, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:BulkUpdateEmployee"));
                        var outputResult = JsonConvert.DeserializeObject<List<ImportDataStatus>>(JsonConvert.SerializeObject(result?.Data));
                        var memoryStream = new MemoryStream();

                        var stream = new MemoryStream();
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        byte[] bytes;
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                            workSheet.Cells.LoadFromCollection(outputResult, true);
                            package.Save();
                            bytes = package.GetAsByteArray();
                        }
                        //stream.Position = 0;
                        //string excelName = $"Import Result-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "Employee List Updated Successfully",
                            BaseString = Convert.ToBase64String(bytes),
                            ContentType = contentType ?? "application/octet-stream"
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Empty File is uploaded",
                       
                    });
                }
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = strErrorMsg
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/BulkUpdateEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg
                });
            }
        }
        #endregion

        #region Add or Update employee Work History
        [HttpPost]
        [Route("AddOrUpdateEmployeeWorkHistory")]
        public IActionResult AddOrUpdateEmployeeWorkHistory(List<WorkHistoryView> workHistoryDetail)
        {
            try
            {
                EmployeeWorkAndEducationDetailView workAndEducationDetailView = new EmployeeWorkAndEducationDetailView();
                workAndEducationDetailView.WorkHistoriesList = workHistoryDetail;
                SupportingDocumentsView supportingDocuments = new();
                supportingDocuments.SourceType = _configuration.GetValue<string>("Employee");
                supportingDocuments.BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                workAndEducationDetailView.SupportingDocumentsViews = supportingDocuments;
                var result = _client.PostAsJsonAsync(workAndEducationDetailView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AddOrUpdateEmployeeWorkHistory"));
                bool isAdded = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = isAdded
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/AddOrUpdateEmployeeWorkHistory");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion

        #region Add or Update employee Education Detail 
        [HttpPost]
        [Route("AddOrUpdateEducationDetails")]
        public IActionResult AddOrUpdateEducationDetails(List<EducationDetailView> educationDetail)
        {
            try
            {
                EmployeeWorkAndEducationDetailView workAndEducationDetailView = new EmployeeWorkAndEducationDetailView();
                workAndEducationDetailView.EducationDetailsList = educationDetail;
                SupportingDocumentsView supportingDocuments = new();
                supportingDocuments.SourceType = _configuration.GetValue<string>("Employee");
                supportingDocuments.BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                workAndEducationDetailView.SupportingDocumentsViews = supportingDocuments;
                var result = _client.PostAsJsonAsync(workAndEducationDetailView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AddOrUpdateEducationDetails"));
                bool isAdded = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = isAdded
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/AddOrUpdateEducationDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion

        #region Add or Update employee Compensation Detail 
        [HttpPost]
        [Route("AddOrUpdateEmployeeCompensationDetails")]
        public IActionResult AddOrUpdateEmployeeCompensationDetails(List<CompensationDetailView> compensationDetail)
        {
            try
            {
                var result = _client.PostAsJsonAsync(compensationDetail, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AddOrUpdateEmployeeCompensationDetails"));
                bool isAdded = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = isAdded
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/AddOrUpdateEmployeeCompensationDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion
        #region Add or Update employee Compensation Detail 
        [HttpGet]
        [Route("GetEmployeesCompensationDetail")]
        public IActionResult GetEmployeesCompensationDetail(int employeeId)
        {
            try
            {
                var result =  _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesCompensationDetail") + employeeId);
                List<CompensationDetail> compensationDetailList = JsonConvert.DeserializeObject<List<CompensationDetail>>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = compensationDetailList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesCompensationDetail");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<CompensationDetail>()
                }); ;
            }
        }
        #endregion

        #region Get Employees Work History Detail
        [HttpGet]
        [Route("GetEmployeesWorkHistoryDetail")]
        public IActionResult GetEmployeesWorkHistoryDetail(int employeeId)
        {
            try
            {
                EmployeeWorkAndEducationDetailView workDetails = new EmployeeWorkAndEducationDetailView();

                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeWorkHistory") + employeeId);
                workDetails = JsonConvert.DeserializeObject<EmployeeWorkAndEducationDetailView>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = workDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesCompensationDetail");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<CompensationDetail>()
                }); ;
            }
        }
        #endregion

        #region Get Employees Education Detail
        [HttpGet]
        [Route("GetEmployeeEducationDetail")]
        public IActionResult GetEmployeesEducationDetail(int employeeId)
        {
            try
            {
                EmployeeWorkAndEducationDetailView educationDetail = new EmployeeWorkAndEducationDetailView();

                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeEducationDetail") + employeeId);
                educationDetail = JsonConvert.DeserializeObject<EmployeeWorkAndEducationDetailView>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = educationDetail
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesEducationDetail");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<CompensationDetail>()
                }); ;
            }
        }
        #endregion

        #region Get Employees List For Grid
        [HttpPost]
        [Route("GetEmployeesListForGrid")]
        public IActionResult GetEmployeesListForGrid(PaginationView pagination)
        {
            try
            {
                EmployeeListDetails employeeList = new EmployeeListDetails();

                var result = _client.PostAsJsonAsync(pagination,_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesListForGrid"));
                employeeList = JsonConvert.DeserializeObject<EmployeeListDetails>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesListForGrid");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new EmployeeListDetails()
                }) ;
            }
        }
        #endregion
        #region Get Employees List For Grid
        [HttpPost]
        [Route("GetEmployeesListForOrgChart")]
        public IActionResult GetEmployeesListForOrgChart(PaginationView pagination)
        {
            try
            {
                EmployeeListDetailsForOrgChart employeeList = new EmployeeListDetailsForOrgChart();

                var result = _client.PostAsJsonAsync(pagination, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesListForOrgChart"));
                employeeList = JsonConvert.DeserializeObject<EmployeeListDetailsForOrgChart>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesListForOrgChart");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new EmployeeListDetailsForOrgChart()
                });
            }
        }
        #endregion

        #region Get Employee Basic Information By Id
        [HttpGet]
        [Route("GetEmployeeBasicInformationById")]
        public IActionResult GetEmployeeBasicInformationById(int employeeId)
        {
            EmployeeBasicInfoView employee = new EmployeeBasicInfoView();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeBasicInformationById") + employeeId);
                employee = JsonConvert.DeserializeObject<EmployeeBasicInfoView>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeDetails = employee
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeBasicInformationById");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employee
            });
        }
        #endregion

        #region Get Employee Compensation Detail For View
        [HttpPost]
        [Route("GetEmployeeCompensationDetailForView")]
        public IActionResult GetEmployeeCompensationDetailForView(EmployeeCompensationCompareView compensationCompareView)
        {
            List<CompensationDetailView> compensationDetails = new List<CompensationDetailView>();
            try
            {
                var result = _client.PostAsJsonAsync(compensationCompareView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeCompensationDetailForView"));
                compensationDetails = JsonConvert.DeserializeObject< List<CompensationDetailView>> (JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        data = compensationDetails
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeCompensationDetailForView");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = compensationDetails
            });
        }
        #endregion
        #region Get List Of Employee BirthDay
        [HttpPost]
        [Route("GetListOfEmployeeBirthDay")]
        public IActionResult GetListOfEmployeeBirthDay(EmployeeDateInput data)
        {
            List<EmployeeDetailListView> bithdayEmployee = new List<EmployeeDetailListView>();
            try
            {                
                var result = _client.PostAsJsonAsync(data, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetListOfEmployeeBirthDay"));
                bithdayEmployee = JsonConvert.DeserializeObject<List<EmployeeDetailListView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        data = bithdayEmployee
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetListOfEmployeeBirthDay");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = bithdayEmployee
            });
        }
        #endregion

        #region Add or update Designation

        [HttpPost]
        [Route("InsertOrUpdateDesignation")]
        public IActionResult InsertOrUpdateDesignation(Designation designation)
        {
            int DesignationId = 0;
            try
            {
                var result = _client.PostAsJsonAsync(designation, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:InsertOrUpdateDesignation"));
                DesignationId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.Result?.StatusCode,
                        result?.Result?.StatusText,
                        DesignationId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/InsertOrUpdateDesignation", JsonConvert.SerializeObject(designation));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                DesignationId
            });
        }
        #endregion

        #region Get Designation List and count        
        [HttpGet]
        [Route("GetDesignationListAndCount")]
        public IActionResult GetDesignationListAndCount()
        {
            List<DesignationDetail> designationList = new List<DesignationDetail>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetDesignationListAndCount"));
                designationList = JsonConvert.DeserializeObject<List<DesignationDetail>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeList = designationList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetDesignationListAndCount");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeList = designationList
            });
        }
        #endregion

        #region Get employee list by designation Id  
        [HttpGet]
        [Route("GetEmployeeListByDesignationId")]
        public IActionResult GetEmployeesForDesignationId(int designationId)
        {
            List<DesignationEmployeeDetails> employeeList = new List<DesignationEmployeeDetails>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListByDesignationId") + "?designationId=" + designationId);
                employeeList = JsonConvert.DeserializeObject<List<DesignationEmployeeDetails>>(JsonConvert.SerializeObject(result?.Result?.Data));

                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeDetails = employeeList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeListByDesignationId?designationId=" + designationId);
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employeeList
            });
        }
        #endregion

        #region Get employee list by department Id  
        [HttpGet]
        [Route("GetEmployeeListByDepartmentId")]
        public IActionResult GetEmployeeListByDepartmentId(int departmentId)
        {
            List<DepartmentEmployeeList> employeeList = new List<DepartmentEmployeeList>();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListByDepartmentId") + "?departmentId=" + departmentId);
                employeeList = JsonConvert.DeserializeObject<List<DepartmentEmployeeList>>(JsonConvert.SerializeObject(result?.Result?.Data));

                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeDetails = employeeList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeListByDepartmentId=" + departmentId);
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employeeList
            });
        }
        #endregion

        #region Add or update Department

        [HttpPost]
        [Route("InsertOrUpdateDepartment")]
        public IActionResult InsertOrUpdateDepartment(Department department)
        {
            int DepartmentId = 0;
            try
            {
                var result = _client.PostAsJsonAsync(department, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:InsertOrUpdateDepartment"));
                DepartmentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.Result?.StatusCode,
                        result?.Result?.StatusText,
                        DepartmentId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/InsertOrUpdateDepartment", JsonConvert.SerializeObject(department));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                DepartmentId
            });
        }
        #endregion

        #region Get employee master data        
        [HttpGet]
        [Route("GetDepartmentMasterData")]
        public IActionResult GetDepartmentMasterData()
        {
            DepartmentMasterData masterData = new DepartmentMasterData();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetDepartmentMasterData"));
                masterData = JsonConvert.DeserializeObject<DepartmentMasterData>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        DepartmentMasterData = masterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetDepartmentMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                DepartmentMasterData = masterData
            });
        }
        #endregion

        #region Get Employees List For Filter
        [HttpPost]
        [Route("GetEmployeesListByFilter")]
        public IActionResult GetEmployeesListByFilter(PaginationView paginationView)
        {
            try
            {
                List<EmployeeDetailListView> employeeList = new List<EmployeeDetailListView>();
                var result = _client.PostAsJsonAsync(paginationView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesListByFilter"));
                employeeList = JsonConvert.DeserializeObject<List<EmployeeDetailListView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesListByFilter");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new EmployeeListDetails()
                });
            }
        }
        #endregion
        #region Get employee list by skillset Id
        [HttpPost]
        [Route("GetEmployeeListBySkillsetId")]
        public IActionResult GetEmployeeListBySkillsetId(EmployeeSkillsetCategoryInput skillsetInput)
        {
            List<SkillsetEmployeeDetails> employeeList = new List<SkillsetEmployeeDetails>();
            try
            {
                var result = _client.PostAsJsonAsync(skillsetInput,_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListBySkillsetId"));
                employeeList = JsonConvert.DeserializeObject<List<SkillsetEmployeeDetails>>(JsonConvert.SerializeObject(result?.Result?.Data));

                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeDetails = employeeList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeListBySkillsetId=" + JsonConvert.SerializeObject(skillsetInput));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employeeList
            });
        }
        #endregion

        #region Add or Update Skillset

        [HttpPost]
        [Route("InsertOrUpdateSkillset")]
        public IActionResult InsertOrUpdateSkillset(Skillsets skillsets)
        {
            int SkillsetId = 0;
            try
            {
                var result = _client.PostAsJsonAsync(skillsets, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:InsertOrUpdateSkillset"));
                SkillsetId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.Result?.StatusCode,
                        result?.Result?.StatusText,
                        SkillsetId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/InsertOrUpdateSkillset", JsonConvert.SerializeObject(skillsets));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                SkillsetId
            });
        }
        #endregion

        #region Get SkillsetDetails      
        [HttpGet]
        [Route("GetSkillsetDetails")]
        public IActionResult GetSkillsetDetails()
        {

            SkillsetDetails skillsetDetails = new SkillsetDetails();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetSkillsetDetails"));
                skillsetDetails = JsonConvert.DeserializeObject<SkillsetDetails>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        Data = skillsetDetails
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetDesignationListAndCount");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = skillsetDetails
            });
        }
        #endregion

        #region Get SkillsetHistory by skillset Id  
        [HttpGet]
        [Route("GetSkillsetHistoryBySkillsetId")]
        public IActionResult GetSkillsetHistoryBySkillsetId(int skillsetId)
        {

            SkillsetHistoryView skillsetHistory = new SkillsetHistoryView();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetSkillsetHistoryBySkillsetId") + "?skillsetId=" + skillsetId);
                skillsetHistory = JsonConvert.DeserializeObject<SkillsetHistoryView>(JsonConvert.SerializeObject(result?.Result?.Data));

                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        Data = skillsetHistory
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetSkillsetHistoryBySkillsetId=" + skillsetId);
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = skillsetHistory
            });
        }
        #endregion

        #region Add Employee Request

        [HttpPost]
        [Route("AddEmployeeRequest")]
        public async Task<IActionResult> AddEmployeeRequest(EmployeesViewModel employeesViewModel)
        {
            try
            {
                SupportingDocumentsView supportingDocuments = new();
                supportingDocuments.SourceType = _configuration.GetValue<string>("RequestProof");
                supportingDocuments.BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                employeesViewModel.supportingDocumentsViews = supportingDocuments;
                var result =  await _client.PostAsJsonAsync(employeesViewModel, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AddEmployeeRequest"));
                ChangeRequestEmailView emailData = JsonConvert.DeserializeObject<ChangeRequestEmailView>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && result.StatusCode == "SUCCESS")
                {
                   AddChangeRequestEmail(emailData);
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/AddEmployeeRequest", JsonConvert.SerializeObject(employeesViewModel));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                
            });
        }
        #endregion

        #region Get List Of Work Aniversary
        [HttpPost]
        [Route("GetListOfEmployeeWorkAnniversaries")]
        public IActionResult GetListOfEmployeeWorkAnniversaries(EmployeeWorkAnniversariesInput data)
        {
            List<EmployeeWorkAnniversaries> workAniversaryEmployee = new List<EmployeeWorkAnniversaries>();
            try
            {
                var result = _client.PostAsJsonAsync(data, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetListOfEmployeeWorkAnniversaries"));
                workAniversaryEmployee = JsonConvert.DeserializeObject<List<EmployeeWorkAnniversaries>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        data = workAniversaryEmployee
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetListOfEmployeeWorkAnniversaries");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = workAniversaryEmployee
            });
        }
        #endregion

        #region Get employee list by skillset Id For Download
        [HttpPost]
        [Route("GetEmployeeListBySkillsetIdForDownload")]
        public IActionResult GetEmployeeListBySkillsetIdForDownload(EmployeeSkillsetCategoryInput skillsetInput)
        {
            List<EmployeeDetails> employeeList = new List<EmployeeDetails>();
            try
            {
                var result = _client.PostAsJsonAsync(skillsetInput, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListBySkillsetIdForDownload"));
                employeeList = JsonConvert.DeserializeObject<List<EmployeeDetails>>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null && employeeList.Count > 0)
                {
                    var memoryStream = new MemoryStream();

                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                byte[] bytes;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Employee List");
                        workSheet.Cells[1, 1].Value = "Employee Id";
                        workSheet.Cells[1, 2].Value = "Employee Name";
                        workSheet.Cells[1, 3].Value = "Employee Email Id";
                        workSheet.Cells[1, 4].Value = "Employee Skill";

                        // Inserting the article data into excel
                        // sheet by using the for each loop
                        // As we have values to the first row 
                        // we will start with second row
                        int recordIndex = 2;

                        foreach (var employee in employeeList)
                        {
                            workSheet.Cells[recordIndex, 1].Value = employee.FormattedEmployeeId;
                            workSheet.Cells[recordIndex, 2].Value = employee.EmployeeName;
                            workSheet.Cells[recordIndex, 3].Value = employee.EmployeeEmailId;
                            workSheet.Cells[recordIndex, 4].Value = employee.Skillset;
                            recordIndex++;
                        }

                        // By default, the column width is not 
                        // set to auto fit for the content
                        // of the range, so we are using
                        // AutoFit() method here. 
                        workSheet.Column(1).AutoFit();
                        workSheet.Column(2).AutoFit();
                        workSheet.Column(3).AutoFit();
                        workSheet.Column(4).AutoFit();
                        //workSheet.Cells.LoadFromCollection(employeeList, true);
                         package.Save();
                         bytes = package.GetAsByteArray();
                       }
                //stream.Position = 0;
                //string excelName = $"Import Result-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Skillset List Exported Successfully",
                    BaseString = Convert.ToBase64String(bytes),
                    ContentType = contentType ?? "application/octet-stream"
                });
                
                  
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "No Employee available for selected skillset"
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeListBySkillsetIdForDownload=" + JsonConvert.SerializeObject(skillsetInput));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employeeList
            });
        }
        #endregion
        #region Get Organization chart details
        [HttpGet]
        [Route("GetorganizationchartDetails")]
        public IActionResult GetorganizationchartDetails(int employeeId)
        {
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetorganizationchartDetails") + employeeId);
                EmployeeorganizationcharView response = JsonConvert.DeserializeObject<EmployeeorganizationcharView>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetorganizationchartDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = new EmployeeorganizationcharView()
            });
        }
        #endregion
        #region Get Organization chart details
        [HttpPost]
        [Route("GetEmployeesListCount")]
        public async Task<IActionResult> GetEmployeesListCount(PaginationView paginationView)
        {
            try
            {
                var result =await _client.PostAsJsonAsync(paginationView,_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesListCount"));
                int response = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeesListCount");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = ""
            });
        }
        #endregion
        #region Get Employee List For Download
        [HttpPost]
        [Route("EmployeeListDownload")]
        public async Task<IActionResult> EmployeeListDownload(PaginationView paginationView)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(paginationView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:EmployeeListDownload"));
                EmployeeDownloadData response = JsonConvert.DeserializeObject<EmployeeDownloadData>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/EmployeeListDownload");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = new EmployeeDownloadData()
            });
        }
        [NonAction]
        public async Task<string> EmailNotificationForNewManager(EmployeeNotificationData data)
        {
            try
            {
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string MailSubject = data.EmployeeMasterEmailTemplateForReportingManagerChange.Subject, MailBody = data.EmployeeMasterEmailTemplateForReportingManagerChange.Body;
                MailBody = MailBody.Replace("@employeeName", data.EmployeeName);
                MailBody = MailBody.Replace("@reportingManagerPrev", data.OldManagerDetails.ManagerName);
                MailBody = MailBody.Replace("@reportingManagerCurrent", data.NewManagerDetails.ManagerName);

                SendEmailView sendEmail;
                sendEmail = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = data.EmployeeEmailAddress,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("PeopleExperience").Value+","+ data.OldManagerDetails.EmailAddress + "," + data.NewManagerDetails.EmailAddress+","+ appsetting.GetSection("Techsupport").Value + ","+ appsetting.GetSection("PMO").Value,
                   //CC=data.OldManagerDetails.EmailAddress+","+data.NewManagerDetails.EmailAddress,

                };
                if (sendEmail != null)
                {
                    var mail = _commonFunction.NotificationMail(sendEmail).Result;
                };

                Notifications notification = new();
                notification = new()
                {
                    CreatedBy =1,
                    CreatedOn = DateTime.UtcNow,
                    FromId = 1,
                    ToId = data?.EmployeeId == null ? 0 : (int)data?.EmployeeId,
                    MarkAsRead = false,
                    NotificationSubject = "Change of Reporting Manage",
                    NotificationBody = "Your Reporting Manager is change to " + data.NewManagerDetails.ManagerName,
                    PrimaryKeyId = data?.EmployeeId == null ? 0 : (int)data?.EmployeeId,
                    ButtonName = "View Profile",
                    SourceType = "Employee",
                };
                return "Success";
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/getEmployeeEmailTemplate");
                return "Failure";
            }

           
        }
        [NonAction]
        public async Task<string> EmailNotificationForNewDesignation(EmployeeNotificationData data)
        {
            try
            {
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string MailSubject = data.EmployeeMasterEmailTemplateForDesignation.Subject, MailBody = data.EmployeeMasterEmailTemplateForDesignation.Body;
                MailBody = MailBody.Replace("@employeeName", data.EmployeeName);
                MailBody = MailBody.Replace("@designationPrev", data.OldEmployeeDesignationDetails.DesignationName);
                MailBody = MailBody.Replace("@designationCurrent", data.NewEmployeeDesignationDetails.DesignationName);

                SendEmailView sendEmail;
                sendEmail = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = data.EmployeeEmailAddress,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("PeopleExperience").Value,
                };
                if (sendEmail != null)
                {
                    var mail = _commonFunction.NotificationMail(sendEmail).Result;
                };
                Notifications notification = new();
                notification = new()
                {
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow,
                    FromId = 1,
                    ToId = data?.EmployeeId == null ? 0 : (int)data?.EmployeeId,
                    MarkAsRead = false,
                    NotificationSubject = "Change of Designation",
                    NotificationBody = "Your Designation has been change as " + data.NewEmployeeDesignationDetails.DesignationName,
                    PrimaryKeyId = data?.EmployeeId == null ? 0 : (int)data?.EmployeeId,
                    ButtonName = "View Profile",
                    SourceType = "Employee",
                };
                return "Success";
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/getEmployeeEmailTemplateForDesignation");
                return "Failure";
            }


        }
        [NonAction]
        public async Task<string> EmailNotificationForNewBaseWorkLocation(EmployeeNotificationData data)
        {
            try
            {
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string MailSubject = data.EmployeeMasterEmailTemplateForBaseWorkLocation.Subject, MailBody = data.EmployeeMasterEmailTemplateForBaseWorkLocation.Body;
                MailBody = MailBody.Replace("@employeeName", data.EmployeeName);
                MailBody = MailBody.Replace("@baseWorkLocationPrev", data.OldEmployeeBaseWorkLocationDetails.LocationName);
                MailBody = MailBody.Replace("@baseWorkLocationCurrent", data.NewEmployeeBaseWorkLocationDetails.LocationName);

                SendEmailView sendEmail;
                sendEmail = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = data.EmployeeEmailAddress,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("PeopleExperience").Value,
                };
                if (sendEmail != null)
                {
                    var mail = _commonFunction.NotificationMail(sendEmail).Result;
                };
                Notifications notification = new();
                notification = new()
                {
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow,
                    FromId = 1,
                    ToId = data?.EmployeeId == null ? 0 : (int)data?.EmployeeId,
                    MarkAsRead = false,
                    NotificationSubject = "Change of Base Location",
                    NotificationBody = "Your Base Location has been change as " + data.NewEmployeeBaseWorkLocationDetails.LocationName,
                    PrimaryKeyId = data?.EmployeeId == null ? 0 : (int)data?.EmployeeId,
                    ButtonName = "View Profile",
                    SourceType = "Employee",
                };
                return "Success";
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/getEmployeeEmailTemplateForBaseWorkLocation");
                return "Failure";
            }


        }
        [NonAction]
        public async Task<string> EmailNotificationForNewProbationStatus(EmployeeNotificationData data)
        {
            try
            {
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string MailSubject = data.EmployeeMasterEmailTemplateForProbationStatus.Subject, MailBody = data.EmployeeMasterEmailTemplateForProbationStatus.Body;
                MailBody = MailBody.Replace("@employeeName", data.EmployeeName);
                //MailBody = MailBody.Replace("@probationstatusPrev", data.OldEmployeeProbationStatusDetails.ProbationStatusName);
                //MailBody = MailBody.Replace("@probationstatusCurrent", data.NewEmployeeProbationStatusDetails.ProbationStatusName);

                SendEmailView sendEmail;
                sendEmail = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = data.EmployeeEmailAddress,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("PeopleExperience").Value+","+ data.ManagerDetailsForProbationStatus.EmailAddress,
                    //CC=data.ManagerDetailsForProbationStatus.EmailAddress,
                  
                };
                if (sendEmail != null)
                {
                    var mail = _commonFunction.NotificationMail(sendEmail).Result;
                };
                Notifications notification = new();
                notification = new()
                {
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow,
                    FromId = 1,
                    ToId = data?.EmployeeId == null ? 0 : (int)data?.EmployeeId,
                    MarkAsRead = false,
                    NotificationSubject = "Change of Probation Status",
                    NotificationBody = "Your Probation Status has been change as " + data.NewEmployeeProbationStatusDetails.ProbationStatusName,
                    PrimaryKeyId = data?.EmployeeId == null ? 0 : (int)data?.EmployeeId,
                    ButtonName = "View Profile",
                    SourceType = "Employee",
                };
                return "Success";
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/getEmployeeEmailTemplateForProbationStatus");
                return "Failure";
            }


        }
        #endregion
        #region Get employee List Audit Data
        [HttpGet]
        [Route("GetAuditListByEmployeeId")]
        public async Task<IActionResult> GetAuditListByEmployeeId(int employeeId)
        {
            try
            {
                var result =await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAuditListByEmployeeId") + employeeId);
                List<AuditDetailView> response = JsonConvert.DeserializeObject<List<AuditDetailView>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetAuditListByEmployeeId");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = new List<AuditDetailView>()
            });
            
        }
        #endregion

        #region Approve Or Reject Employee Request
        [HttpPost]
        [Route("ApproveOrRejectEmployeeRequest")]
        public async Task<IActionResult> ApproveOrRejectEmployeeRequest(ApproveOrRejectEmpRequestListView approveOrRejectEmpRequestList)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(approveOrRejectEmpRequestList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:ApproveOrRejectEmployeeRequest"));
                ChangeRequestEmailView emailData = JsonConvert.DeserializeObject<ChangeRequestEmailView>(JsonConvert.SerializeObject(result?.Data));
                if (result.Data != null)
                {
                    ApproveOrRejectEmail(emailData);
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = result?.Data
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/ApproveOrRejectEmployeeRequest");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = false
            });
        }
        #endregion
        //#region Get employee List Audit Data
        //[HttpGet]
        //[Route("GetEmployeeRequestForAdmin")]
        //public IActionResult GetEmployeeRequestForAdmin()
        //{
        //    try
        //    {
        //        var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeRequestForAdmin"));
        //        EmployeeRequestListView response = JsonConvert.DeserializeObject<EmployeeRequestListView>(JsonConvert.SerializeObject(result?.Result?.Data));
        //        if (result != null)
        //        {
        //            return Ok(new
        //            {
        //                result.Result.StatusCode,
        //                result.Result.StatusText,
        //                data = response
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeRequestForAdmin");
        //    }
        //    return Ok(new
        //    {
        //        StatusCode = "FAILURE",
        //        StatusText = strErrorMsg,
        //        data = new EmployeeRequestListView()
        //    });

        //}
        //#endregion
        #region Get employee List Audit Data
        [HttpGet]
        [Route("GetEmployeeRequest")]
        public async Task<IActionResult> GetEmployeeRequest(int employeeId)
        {
            try
            {
                var result =await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeRequest") + employeeId);
                List<EmployeeRequestListView> response = JsonConvert.DeserializeObject<List<EmployeeRequestListView>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeRequest");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = new List<EmployeeRequestListView>()
            }); ;

        }
        #endregion
        #region Get Employee Approval
        [HttpGet]
        [Route("GetEmployeeApproval")]
        public async Task<IActionResult> GetEmployeeApproval(int employeeId)
        {
            try
            {
                var result =await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeApproval") + employeeId);
                EmployeesRequestList response = JsonConvert.DeserializeObject<EmployeesRequestList>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeApproval");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = new EmployeesRequestList()
            }); ;

        }
        #endregion

        [NonAction]
        #region Trigger change request email
        private async Task<bool> AddChangeRequestEmail(ChangeRequestEmailView changeRequestEmail)
        {
            var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
            string baseURL = appsetting.GetSection("BaseURL").Value;
            foreach(EmployeeRequestView employeeRequest in changeRequestEmail?.employeeRequests)
            {
                string MailSubject = changeRequestEmail?.employeeMasterEmail?.Subject, MailBody = changeRequestEmail?.employeeMasterEmail?.Body;
                MailSubject = MailSubject.Replace("@employeeName", changeRequestEmail?.EmployeeDetail.EmployeeFullName + " - " + changeRequestEmail?.EmployeeDetail.FormattedEmployeeId);
                MailBody = MailBody.Replace("@employeeName", changeRequestEmail?.EmployeeDetail.EmployeeFullName + " - " + changeRequestEmail?.EmployeeDetail.FormattedEmployeeId);
                MailBody = MailBody.Replace("@changeRequestCategory", employeeRequest.RequestCategory);
                
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                //var COOMail = appsetting.GetSection("COOEmialId").Value;
                string textBody = "";
                if(employeeRequest.RequestCategory !="Skills" && employeeRequest.RequestCategory != "EMERGENCY CONTACT INFORMATION")
                {
                    MailBody = MailBody.Replace("@link", "Please click <a href='@baseURL/#/pmsnexus/employees/my-approval' target=\"_blank\">here</a> to approve.");
                    MailBody = MailBody.Replace("@baseURL", baseURL); 
                }
                else
                {
                    MailBody = MailBody.Replace("@link", "");
                }
                foreach (EmployeeRequestDetailsView requestDetailsView in employeeRequest?.EmployeeRequestDetailslst)
                    {
                    if (employeeRequest.RequestCategory == "Photo")
                    {
                        textBody += "<tr  style='text-align:center';>" +
                             "<td><b>" + requestDetailsView.Field + "</b></td>" +
                             "<td><b> "+ (string.IsNullOrEmpty(requestDetailsView.OldValue) ? "NA" : ( "<img src= '" +requestDetailsView.OldValue) + "' width='100px' height= '100px'  style='border-radius: 50%'; >") + " </b></td >" +
                             "<td><b>" + (string.IsNullOrEmpty(requestDetailsView.NewValue) ? "NA" : ("<img src='" + requestDetailsView.NewValue) + "' width='100px' height= '100px' style='border-radius: 50%'; >") + "</b></td >" +
                             "</tr>";
                    }
                    else
                    {
                        textBody += "<tr  style='text-align:center';>" +
                             "<td><b>" + requestDetailsView.Field + "</b></td>" +
                             "<td><b>" + (string.IsNullOrEmpty(requestDetailsView.OldValue) ? "NA" : requestDetailsView.OldValue.ToString().Replace("00:00:00", "").Trim()) + "</b></td >" +
                             "<td><b>" + (string.IsNullOrEmpty(requestDetailsView.NewValue) ? "NA" : requestDetailsView.NewValue.ToString().Replace("00:00:00", "").Trim()) + "</b></td >" +
                             "</tr>";
                    }
                }
               
                MailBody = MailBody.Replace("@textBody", textBody);
                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = HRTeamMail,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("PeopleExperience").Value + "," + changeRequestEmail?.EmployeeDetail.EmployeeEmailId 
                };
                if (employeeRequest?.EmployeeRequestDetailslst?.Count > 0)
                {
                    string mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;

                    List<Notifications> notifications = new();
                    Notifications notification = new();
                    Notifications empNotification = new();

                    notification = new()
                    {
                        CreatedBy = changeRequestEmail?.EmployeeDetail?.EmployeeId == null ? 0 : (int)changeRequestEmail?.EmployeeDetail?.EmployeeId,
                        CreatedOn = DateTime.UtcNow,
                        FromId = changeRequestEmail?.EmployeeDetail?.EmployeeId == null ? 0 : (int)changeRequestEmail?.EmployeeDetail?.EmployeeId,
                        ToId = 0,
                        MarkAsRead = false,
                        NotificationSubject = "New change of detail request from " + changeRequestEmail?.EmployeeDetail?.EmployeeFullName,
                        NotificationBody = "" + changeRequestEmail?.EmployeeDetail?.EmployeeFullName + " has request for " + employeeRequest.RequestCategory + " Approval .",
                        PrimaryKeyId = changeRequestEmail?.EmployeeDetail?.EmployeeId,
                        ButtonName = "View Approvals",
                        SourceType = "Employee Request",
                    };
                    string regNotification = _commonFunction.Notification(notification).Result;
                    empNotification = new Notifications
                    {
                        CreatedBy = changeRequestEmail?.EmployeeDetail?.EmployeeId == null ? 0 : (int)changeRequestEmail?.EmployeeDetail?.EmployeeId,
                        CreatedOn = DateTime.UtcNow,
                        FromId = 0,
                        ToId = changeRequestEmail?.EmployeeDetail?.EmployeeId == null ? 0 : (int)changeRequestEmail?.EmployeeDetail?.EmployeeId,
                        MarkAsRead = false,
                        NotificationSubject = "" + employeeRequest.RequestCategory + " sent for approval.",
                        NotificationBody = "Your " + employeeRequest.RequestCategory + " request has been sent for approval.",
                        PrimaryKeyId = employeeRequest.EmployeeRequestId,
                        ButtonName = "View Request ",
                        SourceType = "Employee Requests",
                    };
                    string empnotification = _commonFunction.Notification(empNotification).Result;
                }
            }

            return true;
        }
        #endregion
        //#region Send mail for  resignation notification 
        //[NonAction]
        //public async Task<string> SendRequestChangesNotification(ChangeRequestEmailView changeRequestEmail)
        //{
    
        //    List<Notifications> notifications = new();
        //    Notifications notification = new();
        //    Notifications empNotification = new();

        //    notification = new()
        //    {
        //        CreatedBy = changeRequestEmail?.EmployeeDetail?.EmployeeId == null ? 0 : (int)changeRequestEmail?.EmployeeDetail?.EmployeeId,
        //        CreatedOn = DateTime.UtcNow,
        //        FromId = changeRequestEmail?.EmployeeDetail?.EmployeeId == null ? 0 : (int)changeRequestEmail?.EmployeeDetail?.EmployeeId,
        //        ToId = 0,
        //        MarkAsRead = false,
        //        NotificationSubject = "New change of detail request from " + changeRequestEmail?.EmployeeDetail?.EmployeeFullName,
        //        NotificationBody = "" + changeRequestEmail?.EmployeeDetail?.EmployeeFullName + " has request for " + requestType + " Approval .",
        //        PrimaryKeyId = changeRequestEmail?.EmployeeDetail?.EmployeeId,
        //        ButtonName = buttonName,
        //        SourceType = "Resignation",
        //    };
        //    string regNotification = _commonFunction.Notification(notification).Result;
        //    empNotification = new Notifications
        //    {
        //        CreatedBy = resignationDetails?.CreatedBy == null ? 0 : (int)resignationDetails?.CreatedBy,
        //        CreatedOn = DateTime.UtcNow,
        //        FromId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
        //        ToId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
        //        MarkAsRead = false,
        //        NotificationSubject = "" + requestType + " sent for approval.",
        //        NotificationBody = "Your " + requestType + " request has been sent to " + resignationDetails.ApproverName + "'s approval.",
        //        PrimaryKeyId = resignationDetails.EmployeeResignationDetailsId,
        //        ButtonName = "View " + requestType,
        //        SourceType = "Resignation",
        //    };
        //    string empnotification = _commonFunction.Notification(empNotification).Result;
        //    return regNotification;
        //}
        //#endregion
        [NonAction]
        #region Trigger change request email
        private async Task<bool> ApproveOrRejectEmail(ChangeRequestEmailView changeRequestEmail)
        {
            var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
            string baseURL = appsetting.GetSection("BaseURL").Value;
            foreach (EmployeeRequest employeeRequest in changeRequestEmail?.ApprovedData)
            {
                string MailSubject = changeRequestEmail?.employeeMasterEmail?.Subject, MailBody = changeRequestEmail?.employeeMasterEmail?.Body;
                MailBody = MailBody.Replace("@employeeName", changeRequestEmail?.EmployeeDetail.EmployeeFullName + " - " + changeRequestEmail?.EmployeeDetail.FormattedEmployeeId);
                MailBody = MailBody.Replace("@detail", employeeRequest.RequestCategory);
                if(changeRequestEmail?.employeeMasterEmail?.TemplateName == "ApproveOrReject")
                {
                    MailBody = MailBody.Replace("@Status", employeeRequest.Status);
                    MailSubject = MailSubject.Replace("@Status", employeeRequest.Status);
                    MailSubject = MailSubject.Replace("@detail", employeeRequest.RequestCategory);
                    string textBody = (employeeRequest.Remark != null && employeeRequest.Remark != "") ? ("<p> Remark : " + employeeRequest.Remark + "<p>") : "";
                    MailBody = MailBody.Replace("@rejectedRemark", textBody);
                }
                else
                {
                    MailSubject = MailSubject.Replace("@employeeName", changeRequestEmail?.EmployeeDetail.EmployeeFullName + " - " + changeRequestEmail?.EmployeeDetail.FormattedEmployeeId);
                }
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                //var COOMail = appsetting.GetSection("COOEmialId").Value;
               
                MailBody = MailBody.Replace("@baseURL", baseURL);
                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = changeRequestEmail?.EmployeeDetail.EmployeeEmailId,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("PeopleExperience").Value + "," + HRTeamMail
                };
                string mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;

                Notifications notification = new();
                notification = new()
                {
                    CreatedBy = employeeRequest.ApprovedBy == null ? 0 : (int)employeeRequest.ApprovedBy,
                    CreatedOn = DateTime.UtcNow,
                    FromId =  1,
                    ToId = changeRequestEmail?.EmployeeDetail?.EmployeeId == null ? 0 : (int)changeRequestEmail?.EmployeeDetail?.EmployeeId,
                    MarkAsRead = false,
                    NotificationSubject = "Your Change of detail request has been "+ employeeRequest.Status,
                    NotificationBody = "Your Change of detail request has been " + employeeRequest.Status,
                    PrimaryKeyId = employeeRequest.EmployeeRequestId,
                    ButtonName = "View Request",
                    SourceType = "RequestApproval",
                };
            }

            return true;
        }
        #endregion

        #region Get Employee Approval
        [HttpPost]
        [Route("GetMyApprovalEmployeeList")]
        public async Task<IActionResult> GetMyApprovalEmployeeList(PaginationView pagination)
        {
            try
            {
                var result =await _client.PostAsJsonAsync(pagination,_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetMyApprovalEmployeeList"));
                List<EmployeeDetailListView> response = JsonConvert.DeserializeObject<List<EmployeeDetailListView>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetMyApprovalEmployeeList", JsonConvert.SerializeObject(pagination));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = new List<EmployeeDetailListView>()
            });

        }
        #endregion
        #region Get approval Employee count
        [HttpPost]
        [Route("GetMyApprovalEmployeeCount")]
        public async Task<IActionResult> GetMyApprovalEmployeeCount(PaginationView pagination)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(pagination, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetMyApprovalEmployeeCount"));
                int response = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetMyApprovalEmployeeCount", JsonConvert.SerializeObject(pagination));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = 0
            });

        }
        #endregion

        #region Get Employee Approval
        [HttpGet]
        [Route("ContractEndDateNotification")]
        [AllowAnonymous]
        public async Task<IActionResult> ContractEndDateNotification()
        {
            try
            {
                var result = await _client.GetAsync( _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:ContractEndDateNotification"));
                ChangeRequestEmailView response = JsonConvert.DeserializeObject<ChangeRequestEmailView>(JsonConvert.SerializeObject(result?.Data));
                if (result.Data != null)
                {
                    foreach (EmployeeName employeeDetail in response.NotifiedEmployee)
                    {
                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                        string baseURL = appsetting.GetSection("BaseURL").Value;
                        string MailSubject = response?.employeeMasterEmail?.Subject, MailBody = response?.employeeMasterEmail?.Body;
                        MailSubject = MailSubject.Replace("@employeeName", employeeDetail?.EmployeeFullName + " - " + employeeDetail?.FormattedEmployeeId);
                        MailBody = MailBody.Replace("@employeeName", employeeDetail?.EmployeeFullName + " - " + employeeDetail?.FormattedEmployeeId);
                        MailBody = MailBody.Replace("@contractEndDate", employeeDetail?.ContractEndDate==null?"": (employeeDetail.ContractEndDate.Value.Day +"/"+ employeeDetail.ContractEndDate.Value.Month+"/"+ employeeDetail.ContractEndDate.Value.Year));
                        MailBody = MailBody.Replace("@reportingManager", employeeDetail.ReportingManagerData.ReportingManagerName);
                        SendEmailView sendMailbyleaverequest = new();
                        sendMailbyleaverequest = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = employeeDetail.ReportingManagerData.ReportingManagerEmailId,
                            Subject = MailSubject,
                            MailBody = MailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                            CC = appsetting.GetSection("PeopleExperience").Value + ","+ appsetting.GetSection("TVSN_HR").Value+","+ appsetting.GetSection("PMO").Value+"," + employeeDetail.EmployeeEmailId
                        };
                        string mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;

                        Notifications notification = new();
                        notification = new()
                        {
                            CreatedBy =1,
                            CreatedOn = DateTime.UtcNow,
                            FromId = 1,
                            ToId = employeeDetail?.EmployeeId == null ? 0 : (int)employeeDetail?.EmployeeId,
                            MarkAsRead = false,
                            NotificationSubject = "Contract Ends On " + employeeDetail.ContractEndDate.ToString(),
                            NotificationBody = "Your Contract Ends On  " + employeeDetail.ContractEndDate.ToString(),
                            PrimaryKeyId = employeeDetail?.EmployeeId == null ? 0 : (int)employeeDetail?.EmployeeId,
                            //ButtonName = "View Request",
                            SourceType = "Contract ",
                        };
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Notification Sent Successfully",
                    data = response
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/ContractEndDateNotification");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = new ChangeRequestEmailView()
            });

        }
        #endregion

        #region Update Employee Designation 
        [HttpGet]
        [Route("UpdateEmployeeDesignation")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateEmployeeDesignation()
        {
            try
            {
                var result = await _client.GetAsync( _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeeDesignation"));
                bool response = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                if (response)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/UpdateEmployeeDesignation");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = false
            });

        }
        #endregion

        #region Dowload document
        [HttpGet]
        [Route("DownloadDocumentById")]
        public async Task<IActionResult> DownloadDocumentById(int documentId)
        {
            DocumentsToUpload documents = new();
            try
            {
              
                    var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:DownloadDocumentById") +documentId);
                    documents = JsonConvert.DeserializeObject<DocumentsToUpload>(JsonConvert.SerializeObject(result.Data));
                    if (result.StatusCode == "SUCCESS")
                    {
                        //Read the File into a Byte Array.
                        string contentType;
                        new FileExtensionContentTypeProvider().TryGetContentType(documents.DocumentName, out contentType);
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "",
                            BaseString = documents.DocumentAsBase64,
                            ContentType = contentType ?? "application/octet-stream"
                        });
                    }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/DownloadDocumentById", Convert.ToString(documentId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ByteArray = new ByteArrayContent(new byte[1]),
                ContentType = "application/octet-stream"
            });
        }
        #endregion

        #region Add or update department        
        [HttpPost]
        [Route("AddOrUpdateLocation")]
        public IActionResult AddOrUpdateLocation(EmployeeLocation location)
        {
            int LocationId = 0;
            try
            {
                var result = _client.PostAsJsonAsync(location, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AddOrUpdateLocation"));
                LocationId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.Result?.StatusCode,
                        result?.Result?.StatusText,
                        LocationId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/AddOrUpdateLocation", JsonConvert.SerializeObject(location));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                LocationId
            });
        }
        #endregion

        #region Get Employee PersonalInfo By EmployeeID
        [HttpGet]
        [Route("GetEmployeeBasicInfoByEmployeeID")]
        public IActionResult GetEmployeeBasicInfoByEmployeeID(int employeeId)
        {
            EmployeeBasicInfoView employee = new EmployeeBasicInfoView();
            try
            {
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeBasicInfoByEmployeeID") + employeeId);
                employee = JsonConvert.DeserializeObject<EmployeeBasicInfoView>(JsonConvert.SerializeObject(result?.Result?.Data));

                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        EmployeeDetails = employee
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/GetEmployeeBasicInfoByEmployeeID", Convert.ToString(employeeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employee
            });
        }
        #endregion

        #region Get employee list for new resignation
        [HttpPost]
        [Route("GetEmployeeListForNewResignation")]
        public async Task<IActionResult> GetEmployeeListForNewResignation()
        {
            try
            {
                List<EmployeeDataForDropDown> employeeListView = new List<EmployeeDataForDropDown>();
                var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForNewResignation") );
                employeeListView = JsonConvert.DeserializeObject<List<EmployeeDataForDropDown>>(JsonConvert.SerializeObject(employeeListresult.Data));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeListView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateway", "Employee/GetEmployeeListForNewResignation");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeeDataForDropDown>()
                });
            }
        }
        #endregion
    }

}