using APIGateWay.API.Common;
using APIGateWay.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries;
using SharedLibraries.Common;
using SharedLibraries.Models.Attendance;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Leaves;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    //[AllowAnonymous]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HTTPClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _leavesBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _notificationBaseURL = string.Empty;
        private readonly string _attendanceBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";
        private readonly CommonFunction _commonFunction;
        #region Constructor
        public LeavesController(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _configuration = configuration;
            _commonFunction = new CommonFunction(configuration);
            _leavesBaseURL = _configuration.GetValue<string>("ApplicationURL:Leaves:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _notificationBaseURL = _configuration.GetValue<string>("ApplicationURL:Notifications");
            _attendanceBaseURL = _configuration.GetValue<string>("ApplicationURL:Attendance:BaseURL");
        }
        #endregion

        #region Add or update Holiday       
        [HttpPost]
        [Route("AddOrUpdateHoliday")]
        public async Task<IActionResult> AddOrUpdateHoliday(HolidayDetailView holiday)
        {
            int holidayID = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(holiday, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:AddOrUpdateHoliday"));
                holidayID = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        holidayID
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/AddOrUpdateHoliday", JsonConvert.SerializeObject(holiday));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                holidayID
            });
        }
        #endregion

        #region Delete Holiday
        [HttpGet]
        [Route("DeleteHoliday")]
        public async Task<IActionResult> DeleteHoliday(int holidayId)
        {
            try
            {
                var Result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:DeleteHoliday") + holidayId);
                if (Result != null && Result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Holiday deleted successfully."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/DeleteHoliday", JsonConvert.SerializeObject(holidayId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get holiday employee master data        
        [HttpGet]
        [Route("GetHolidayEmployeeMasterData")]
        public async Task<IActionResult> GetHolidayEmployeeMasterData()
        {
            HolidayEmployeeMasterData holidayEmployeeMasterData = new();
            try
            {
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetHolidayEmployeeMasterData"));
                holidayEmployeeMasterData = JsonConvert.DeserializeObject<HolidayEmployeeMasterData>(JsonConvert.SerializeObject(result?.Data));
                var shiftResult = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAllShiftName"));
                holidayEmployeeMasterData.ShiftList = JsonConvert.DeserializeObject<List<KeyWithValue>>(JsonConvert.SerializeObject(shiftResult?.Data));
                if (result != null && result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "",
                        HolidayEmployeeMasterData = holidayEmployeeMasterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetHolidayEmployeeMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                HolidayEmployeeMasterData = holidayEmployeeMasterData
            });
        }
        #endregion

        #region Get All Holidays
        [HttpGet]
        [Route("GetAllHolidays")]
        public async Task<IActionResult> GetAllHolidays()
        {
            HolidayMasterDataView holidayView = new();
            //List<HolidayView> holidays = new();

            DepartmentLocationName departmentLocationDetail = new();
            DepartmentLocationName departmentLocationId = new()
            {
                DepartmentId = new List<int>(),
                LocationId = new List<int>()
            };
            List<KeyWithValue> shiftName = new();
            HolidayEmployeeMasterData holidayEmployeeMasterData = new();
            try
            {
                var masterresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetHolidayEmployeeMasterData"));
                holidayView.HolidayEmployeeMasterData = JsonConvert.DeserializeObject<HolidayEmployeeMasterData>(JsonConvert.SerializeObject(masterresult?.Data));
                var shiftResult = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAllShiftName"));
                holidayView.HolidayEmployeeMasterData.ShiftList = JsonConvert.DeserializeObject<List<KeyWithValue>>(JsonConvert.SerializeObject(shiftResult?.Data));
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAllHolidays"));
                holidayView.HolidayViews = JsonConvert.DeserializeObject<List<HolidayView>>(JsonConvert.SerializeObject(result?.Data));
                if (holidayView.HolidayViews?.Count > 0)
                {
                    List<int> shiftId = new List<int>();
                    foreach (var item in holidayView?.HolidayViews)
                    {
                        departmentLocationId.DepartmentId = departmentLocationId?.DepartmentId?.Concat(item?.EmployeeDepartmentId).ToList();
                        departmentLocationId.LocationId = departmentLocationId?.LocationId?.Concat(item?.EmployeeLocationId).ToList();
                        shiftId = shiftId?.Concat(item?.EmployeeShiftId).ToList();
                    }
                    if (departmentLocationId?.DepartmentId?.Count > 0 || departmentLocationId?.LocationId?.Count > 0)
                    {
                        var departmentLocationResult = await _client.PostAsJsonAsync(departmentLocationId, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetDepartmentLocationNameById"));
                        departmentLocationDetail = JsonConvert.DeserializeObject<DepartmentLocationName>(JsonConvert.SerializeObject(departmentLocationResult?.Data));
                        if (departmentLocationDetail != null)
                        {
                            holidayView.HolidayViews.ForEach(x => x.EmployeeDepartmentName = string.Join(",", departmentLocationDetail?.Department?.Where(y => x.EmployeeDepartmentId.Contains(y.Key)).Select(y => y.Value).ToList()));
                            holidayView.HolidayViews.ForEach(x => x.EmployeeLocationName = string.Join(",", departmentLocationDetail?.Location?.Where(y => x.EmployeeLocationId.Contains(y.Key)).Select(y => y.Value).ToList()));
                        }
                    }
                    if (shiftId?.Count > 0)
                    {
                        var shifftResult = await _client.PostAsJsonAsync(shiftId, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftNameById"));
                        shiftName = JsonConvert.DeserializeObject<List<KeyWithValue>>(JsonConvert.SerializeObject(shifftResult?.Data));
                        if (shiftName?.Count > 0)
                            holidayView?.HolidayViews?.ForEach(x => x.EmployeeShiftName = string.Join(",", shiftName?.Where(y => x.EmployeeShiftId.Contains(y.Key)).Select(y => y.Value).ToList()));
                    }
                    holidayView.HolidayViews = holidayView?.HolidayViews?.OrderByDescending(x => x.CreatedOn).ToList();
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    holidayView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetAllHolidays");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    holidayView
                });
            }
        }
        #endregion

        #region Get Holiday By Id
        [HttpGet]
        [Route("GetByHolidayID")]
        public async Task<IActionResult> GetByHolidayID(int pHolidayID)
        {
            HolidayDetailView holidayDetailView = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetByHolidayID") + pHolidayID);
                holidayDetailView = JsonConvert.DeserializeObject<HolidayDetailView>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = holidayDetailView 
                }); 
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetByHolidayID", Convert.ToString(pHolidayID));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = holidayDetailView 
                });
            }
        }
        #endregion

        #region Delete Leave
        [HttpGet]
        [Route("DeleteLeave")]
        public async Task<IActionResult> DeleteLeave(int pLeaveTypeId)
        {
            try
            {
                DocumnentsPathView leaveData = new();
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:DeleteLeave") + pLeaveTypeId);
                leaveData = JsonConvert.DeserializeObject<DocumnentsPathView>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                {
                    if (leaveData != null)
                    {
                        string SourceType = _configuration.GetValue<string>("LeavesSourceType");
                        string BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                        string leaveSource = _configuration.GetValue<string>("GrantLeaveSourceType");
                        if (leaveData?.LeaveId?.Count > 0)
                        {
                            foreach (var item in leaveData?.LeaveId)
                            {
                                string id = item.ToString();
                                string documentPath = Path.Combine(BaseDirectory, SourceType, id);
                                var dir = new DirectoryInfo(documentPath);
                                if (dir.Exists)
                                {
                                    dir.Delete(true);
                                }
                            }
                        }
                        if (leaveData?.requestDetails?.Count > 0)
                        {
                            foreach (var item in leaveData.requestDetails)
                            {
                                string id = item.LeaveTypeId.ToString();
                                string employeeId = item.EmployeeID.ToString();
                                string documentPath = Path.Combine(BaseDirectory, SourceType, leaveSource, employeeId, id);
                                var dir = new DirectoryInfo(documentPath);
                                if (dir.Exists)
                                {
                                    dir.Delete(true);
                                }
                            }
                        }
                    }
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Leave deleted successfully."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/DeleteLeave", JsonConvert.SerializeObject(pLeaveTypeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get All Leaves
        [HttpGet]
        [Route("GetAllLeaves")]
        public IActionResult GetAllLeaves()
        {
            List<LeaveView> leaveView = new();
            List<EmployeeTypeNames> lstEmployeeTypeNames = new();
            try
            {
                var result = _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAllLeaves"));
                leaveView = JsonConvert.DeserializeObject<List<LeaveView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                lstEmployeeTypeNames = GetEmployeeTypeNameById(leaveView?.Select(x => x.EmployeesTypeId).ToList()).Result;
                leaveView?.ForEach(x => x.EmployeesType = lstEmployeeTypeNames?.Where(y => y.EmployeesTypeId == x.EmployeesTypeId).Select(y => y.EmployeesType).FirstOrDefault());
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    leaveView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetAllLeaves");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    leaveView
                });
            }
        }
        #endregion
        #region Get All Leaves List
        [HttpGet]
        [Route("GetAllLeaveslist")]
        public IActionResult GetAllLeavesList()
        {
            List<LeaveView> leaveView = new();
            List<EmployeeTypeNames> lstEmployeeTypeNames = new();
            try
            {
                var result = _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAllLeaves"));
                leaveView = JsonConvert.DeserializeObject<List<LeaveView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                 return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    leaveView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetAllLeavesList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    leaveView
                });
            }
        }
        #endregion
        #region Get employee type name by id
        [NonAction]
        public async Task<List<EmployeeTypeNames>> GetEmployeeTypeNameById(List<int?> lstEmployeesTypeId)
        {
            List<EmployeeTypeNames> lstEmployeeTypeNames = new();
            try
            {
                using var employeeTypeNameClient = new HttpClient
                {
                    BaseAddress = new Uri(_employeeBaseURL)
                };
                var lstEmpTypeId = lstEmployeesTypeId?.Where(x => x != 0).Select(x => x).Distinct().ToList();
                HttpResponseMessage employeetypenameResponse = await employeeTypeNameClient.PostAsJsonAsync("Employee/GetEmployeeTypeNameById", lstEmpTypeId);
                if (employeetypenameResponse?.IsSuccessStatusCode == true)
                {
                    var employeetypenameResult = employeetypenameResponse?.Content?.ReadAsAsync<SuccessData>();
                    lstEmployeeTypeNames = JsonConvert.DeserializeObject<List<EmployeeTypeNames>>(JsonConvert.SerializeObject(employeetypenameResult?.Result?.Data));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeTypeNameById", JsonConvert.SerializeObject(lstEmployeesTypeId));
            }
            return lstEmployeeTypeNames;
        }
        #endregion

        #region Get All Upcoming Holidays
        [HttpGet]
        [Route("GetUpcomingHolidays")]
        public async Task<IActionResult> GetUpcomingHolidays(int employeeId)
        {
            List<UpcomingHoliday> UpcomingHoliday = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetUpcomingHolidays") + employeeId);
                UpcomingHoliday = JsonConvert.DeserializeObject<List<UpcomingHoliday>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = UpcomingHoliday
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetUpcomingHolidays");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = UpcomingHoliday 
                });
            }
        }
        #endregion

        #region Add or Update Leave
        [HttpPost]
        [Route("AddorUpdateLeave")]
        public async Task<IActionResult> AddorUpdateLeave(LeaveDetailsView leaveDetailsView)
        {
            LeaveTypesDetailView leaveTypesDetail = new LeaveTypesDetailView();
            try
            {
                var result = await _client.PostAsJsonAsync(leaveDetailsView, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:AddorUpdateLeave"));
                leaveTypesDetail = JsonConvert.DeserializeObject<LeaveTypesDetailView>(JsonConvert.SerializeObject(result?.Data));

                if (result != null && result?.StatusCode?.ToLower()== "SUCCESS".ToLower())
                {
                    // Need to Change.
                    AppConstants appConstants = new();
                    AppConstants basedonappConstants = new();
                    List<AppConstants> appConstantList = new();
                    var app = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAppconstantsList"));
                    appConstantList = JsonConvert.DeserializeObject<List<AppConstants>>(JsonConvert.SerializeObject(app?.Data));
                    if (leaveDetailsView?.LeaveTypesId != null)
                    {
                        //var appresults = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAppconstantByID") + leaveDetailsView?.LeaveTypesId);
                        //appConstants = JsonConvert.DeserializeObject<AppConstants>(JsonConvert.SerializeObject(appresults?.Data));
                        appConstants = appConstantList?.Where(x => x.AppConstantId == leaveDetailsView?.LeaveTypesId).Select(x => x).FirstOrDefault();
                        if (appConstants?.AppConstantValue?.ToLower() == "RestrictedHoliday".ToLower())
                        {
                            leaveDetailsView.LeaveAccruedType = appConstants?.AppConstantId;
                        }
                    }
                    //var appresult = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAppconstantByID") + leaveDetailsView?.LeaveAccruedType);
                    //appConstants = JsonConvert.DeserializeObject<AppConstants>(JsonConvert.SerializeObject(appresult?.Data));
                    //appConstants = appConstantList?.Where(x => x.AppConstantId == leaveDetailsView?.LeaveAccruedType).Select(x => x).FirstOrDefault();
                    //var basedonappresult = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAppconstantByID") + leaveDetailsView?.BalanceBasedOn);
                    //basedonappConstants = JsonConvert.DeserializeObject<AppConstants>(JsonConvert.SerializeObject(basedonappresult?.Data));
                    //basedonappConstants = appConstantList?.Where(x => x.AppConstantId == leaveDetailsView?.BalanceBasedOn).Select(x => x).FirstOrDefault();

                    //Old
                    /*if (appConstants?.AppConstantValue?.ToLower() == "Onetime".ToLower() || basedonappConstants?.AppConstantValue?.ToLower() == "LeaveGrant".ToLower() || leaveDetailsView?.EffectiveFromDate <= DateTime.UtcNow.Date)*/  /// leaveDetailsView.LeaveTypeId == 0 && && leaveDetailsView.EffectiveFromDate!=null
                    //New 
                    if (leaveDetailsView?.EffectiveFromDate <= DateTime.Now.Date)
                    {
                        //bool IsNewLeave = false;
                        //bool IsGrant = false;
                        //if (leaveDetailsView?.LeaveTypeId==0)
                        //{
                        //    IsNewLeave = true;
                        //}
                       
                        //if (basedonappConstants?.AppConstantValue?.ToLower() == "LeaveGrant".ToLower())
                        //{
                        //    IsGrant = true;
                        //}

                        List<EmployeeDetailsForLeaveView> EmployeeDetails = new();
                        OneTimeEmployeeLeaveView employeeLeaveView = new OneTimeEmployeeLeaveView();

                        //employeeLeaveView.IsOnCreate = leaveDetailsView.EffectiveFromDate <= DateTime.UtcNow.Date ? true : false;
                        var results = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeforLeave"));
                        employeeLeaveView.EmployeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetailsForLeaveView>>(JsonConvert.SerializeObject(results?.Data));
                        employeeLeaveView.LeaveTypeDetails = leaveTypesDetail;
                        var resultset = await _client.PostAsJsonAsync(employeeLeaveView, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:InsertOneTimeLeave"));

                    }
                }
                return Ok(new
                {
                    StatusCode = result?.StatusCode,
                    StatusText = result?.StatusText,
                    Data = leaveTypesDetail.LeaveTypeId
                }); ;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AddorUpdateLeave", JsonConvert.SerializeObject(leaveDetailsView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = leaveTypesDetail.LeaveTypeId
            });
        }
        #endregion

        #region Get Leave Details By Leave Id
        [HttpGet]
        [Route("GetLeaveDetailsByLeaveId")]
        public async Task<IActionResult> GetLeaveDetailsByLeaveId(int leaveId)
        {
            LeaveDetailsView LeaveDetailsView = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveDetailsByLeaveId") + leaveId);
                LeaveDetailsView = JsonConvert.DeserializeObject<LeaveDetailsView>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = LeaveDetailsView 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveDetailsByLeaveId", Convert.ToString(leaveId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = LeaveDetailsView 
                });
            }
        }
        #endregion

        #region Get leaves master data
        [HttpGet]
        [Route("GetLeaveEmployeeMasterData")]
        public IActionResult GetLeaveEmployeeMasterData()
        {
            try
            {
                LeavesMasterDataView LeavesMasterDataView = new();
                LeavesMasterDataView LeavesEmployeeMasterDataView = new();
                var result = _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveEmployeeMasterData"));
                LeavesMasterDataView = JsonConvert.DeserializeObject<LeavesMasterDataView>(JsonConvert.SerializeObject(result?.Result?.Data));
                var result1 = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetLeaveEmployeeMasterData"));
                LeavesEmployeeMasterDataView = JsonConvert.DeserializeObject<LeavesMasterDataView>(JsonConvert.SerializeObject(result1?.Result?.Data));
                LeavesMasterDataView LeavesMasterDataViewFinal = new()
                {
                    LeaveMaxLimitActionList = LeavesMasterDataView?.LeaveMaxLimitActionList,
                    MonthList = LeavesMasterDataView?.MonthList,
                    DaysList = LeavesMasterDataView?.DaysList,
                    ProbationStatusList = LeavesEmployeeMasterDataView?.ProbationStatusList,
                    AllowUserList = LeavesMasterDataView?.AllowUserList,
                    BalanceToBeDisplayList = LeavesMasterDataView?.BalanceToBeDisplayList,
                    EmployeeDepartmentList = LeavesEmployeeMasterDataView?.EmployeeDepartmentList,
                    EmployeeTypeList = LeavesEmployeeMasterDataView?.EmployeeTypeList,
                    RoleNamesList = LeavesEmployeeMasterDataView?.RoleNamesList,
                    EmployeeList = LeavesEmployeeMasterDataView?.EmployeeList,
                    EmployeeLocationList = LeavesEmployeeMasterDataView?.EmployeeLocationList,
                    EmployeeDesignationList = LeavesEmployeeMasterDataView?.EmployeeDesignationList,
                    LeaveDurationList = LeavesMasterDataView?.LeaveDurationList,
                    ReportConfigurationList = LeavesMasterDataView?.ReportConfigurationList,
                    CurrentFinancialYearHolidayList = LeavesMasterDataView?.CurrentFinancialYearHolidayList,
                    ActiveLeaveTypeList = LeavesMasterDataView?.ActiveLeaveTypeList,
                    LeaveTypeList = LeavesMasterDataView?.LeaveTypeList,
                    LeaveAccuredList = LeavesMasterDataView?.LeaveAccuredList,
                    AllowRequestPeriod = LeavesMasterDataView?.AllowRequestPeriod,
                    SpecificEmployeeLeaveList = LeavesMasterDataView?.SpecificEmployeeLeaveList,
                    CarryForwardList = LeavesMasterDataView?.CarryForwardList,
                    ReimbursementList = LeavesMasterDataView?.ReimbursementList,
                    BalanceBasedOn = LeavesMasterDataView?.BalanceBasedOn,
                    GrantLeaveRequestPeriod = LeavesMasterDataView?.GrantLeaveRequestPeriod,
                    GrantLeaveApproval = LeavesMasterDataView?.GrantLeaveApproval
                };
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                     Data = LeavesMasterDataViewFinal
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new LeavesMasterDataView()
                });
            }
        }
        #endregion

        #region Get employee LeaveAdjustment        
        [HttpPost]
        [Route("GetEmployeeLeaveAdjustment")]
        public async Task<IActionResult> GetEmployeeLeaveAdjustment(EmployeeLeaveAdjustmentFilterView employeeLeaveAdjustmentView)
        {
            LeaveAdjustmentView leaveAdjustment = new();
            //List<EmployeeLeaveAdjustmentView> leaveAdjustment = new();
            try
            {
                //EmployeeListView employeeList = new();
                //employeeList.ToDate = toDate;
                var result = await _client.PostAsJsonAsync(employeeLeaveAdjustmentView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeLeaveAdjustment"));
                //leaveAdjustment = JsonConvert.DeserializeObject<LeaveAdjustmentView>(JsonConvert.SerializeObject(result?.Data));
                leaveAdjustment.LeaveAdjustments = JsonConvert.DeserializeObject<List<EmployeeLeaveAdjustmentView>>(JsonConvert.SerializeObject(result?.Data));
                EmployeeListView employee = new();

                ////Test Record start
                //List<EmployeeLeaveAdjustmentView> list = new List<EmployeeLeaveAdjustmentView>();
                //EmployeeLeaveAdjustmentView leave = new EmployeeLeaveAdjustmentView();
                //leave.Department = "Engineering";
                //leave.DepartmentId = 4;
                //leave.Designation = "Software Engineer";
                //leave.DOJ = new DateTime(2021, 11, 15);
                //leave.EmployeeID = 122;
                //leave.EmployeeName = "Nebora Macrin Valdaris";
                //leave.FirstName = null;
                //leave.FormattedEmployeeId = "NXT 843";
                //leave.LastName = null;
                //list.Add(leave);

                //EmployeeLeaveAdjustmentView leave1 = new EmployeeLeaveAdjustmentView();
                //leave1.Department = "Engineering";
                //leave1.DepartmentId = 4;
                //leave1.Designation = "Software Engineer";
                //leave1.DOJ = new DateTime(2020, 9, 15);
                //leave1.EmployeeID = 126;
                //leave1.EmployeeName = "Selva";
                //leave1.FirstName = null;
                //leave1.FormattedEmployeeId = "NXT 850";
                //leave1.LastName = null;
                //list.Add(leave1);
                //leaveAdjustment.LeaveAdjustments = list;
                ////Test Record end

                //List<int> employeeId = leaveAdjustment.LeaveAdjustments.Select(x => x.EmployeeID).ToList();
                //List<int?> departmentId = leaveAdjustment.LeaveAdjustments.Select(x => x.DepartmentId).ToList();
                //employee.EmployeeId = employeeId;
                employee.FromDate = employeeLeaveAdjustmentView.FromDate;
                employee.ToDate = employeeLeaveAdjustmentView.ToDate;
                employee.EmployeeDetails = leaveAdjustment?.LeaveAdjustments;

                var results = await _client.PostAsJsonAsync(employee, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveBalance"));
                leaveAdjustment.LeaveBalanceView = JsonConvert.DeserializeObject<LeaveBalanceView>(JsonConvert.SerializeObject(results?.Data));
                if (leaveAdjustment?.LeaveAdjustments?.Count > 0)
                {
                    foreach (EmployeeLeaveAdjustmentView item in leaveAdjustment?.LeaveAdjustments)
                    {
                        decimal? leaveBalance = leaveAdjustment?.LeaveBalanceView?.Leaves?.Where(y => y.EmployeeId == item?.EmployeeID).Select(y => y.BalanceLeaves).FirstOrDefault();
                        item.LeaveBalance = leaveBalance == null ? 0 : leaveBalance;
                    }
                }
                //if (leaveAdjustment != null)
                //{
                return Ok(new
                {
                    results.StatusCode,
                    results.StatusText,
                    leaveAdjustment
                });
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeLeaveAdjustment");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                leaveAdjustment
            });
        }
        #endregion

        #region Get Team Leaves
        [HttpGet]
        [Route("GetTeamLeave")]
        public async Task<IActionResult> GetTeamLeave()
        {
            List<TeamLeaveView> teamLeaveView = new();
            List<EmployeeName> lstEmployeeName = new();
            List<SupportingDocuments> ListOfDocument = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetTeamLeave"));
                teamLeaveView = JsonConvert.DeserializeObject<List<TeamLeaveView>>(JsonConvert.SerializeObject(result?.Data));
                lstEmployeeName = GetEmployeeNameById(teamLeaveView?.Select(x => x.EmployeeId).ToList()).Result;
                teamLeaveView?.ForEach(x => x.EmployeeName = lstEmployeeName?.Where(y => y.EmployeeId == x.EmployeeId).Select(y => y.EmployeeFullName).FirstOrDefault());
                ListOfDocument = GetDocumentByLeaveId(teamLeaveView?.Select(x => x.LeaveId).ToList()).Result;
                teamLeaveView?.ForEach(x => x.ListOfDocuments = ListOfDocument?.Where(y => y.SourceId == x.LeaveId)
                .Select(x => new SupportingDocuments
                {
                    DocumentId = x.DocumentId,
                    DocumentName = x.DocumentName,
                    DocumentSize = x.DocumentSize,
                    DocumentCategory = x.DocumentCategory,
                    IsApproved = x.IsApproved,
                    DocumentType = x.DocumentType
                }).ToList());
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    teamLeaveView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetTeamLeave");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    teamLeaveView
                });
            }
        }
        #endregion

        #region Get employee name by id
        [NonAction]
        public async Task<List<EmployeeName>> GetEmployeeNameById(List<int> lstEmployeeId)
        {
            List<EmployeeName> lstEmployeeName = new();
            try
            {
                using var employeeNameClient = new HttpClient
                {
                    BaseAddress = new Uri(_employeeBaseURL)
                };
                var lstEmpId = lstEmployeeId?.Where(x => x != 0).Select(x => x).Distinct().ToList();
                HttpResponseMessage employeenameResponse = await employeeNameClient.PostAsJsonAsync("Employee/GetEmployeeNameById", lstEmpId);
                if (employeenameResponse?.IsSuccessStatusCode == true)
                {
                    var employeenameResult = employeenameResponse?.Content?.ReadAsAsync<SuccessData>();
                    lstEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeenameResult?.Result?.Data));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeNameById", JsonConvert.SerializeObject(lstEmployeeId));
            }
            return lstEmployeeName;
        }
        #endregion

        #region Approve or Reject Leave
        [HttpPost]
        [Route("ApproveOrRejectLeave")]
        public async Task<IActionResult> ApproveOrRejectLeave(ApproveOrRejectLeave approveOrRejectLeave)
        {
            StatusandApproverDetails statusandApprover = new();
            string LeaveId = "", statusText = "", RegularizeId = "", Subject = null, Body = null;
            try
            {
                // For Mail purpose
                EmployeeandManagerView employeeandManager = new EmployeeandManagerView();
                var employeeID = approveOrRejectLeave?.EmployeeId;
                var empresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeID);
                employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(empresults?.Data));

                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                string MailSubject = null, MailBody = null;
                SendEmailView sendMailbyleaverequest = new();

                if (approveOrRejectLeave?.IsRegularize == true)
                {

                    TimeLogApproveOrRejectView timeLogApproveOrRejectView = new();
                    timeLogApproveOrRejectView = approveOrRejectLeave?.TimeLog;
                    var results = await _client.PostAsJsonAsync(timeLogApproveOrRejectView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:TimeLogApproveOrReject"));
                    RegularizeId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(results?.Data));
                    string regStatus = approveOrRejectLeave?.TimeLog?.IsApproveOrCancel;
                    if (results != null)
                    {
                        if (regStatus?.ToLower() == "Approved"?.ToLower() || regStatus?.ToLower() == "Rejected"?.ToLower() || regStatus?.ToLower() == "Cancelled"?.ToLower())
                        {
                            // Notification
                            if (regStatus?.ToLower() == "Approved"?.ToLower())
                            {
                                Subject = "Your Regularization are Approved.";
                                Body = employeeandManager?.ManagerName + " has approved your Regularization are Approved.";
                            }
                            else if (regStatus?.ToLower() == "Rejected"?.ToLower())
                            {
                                Subject = "Your Regularization are Rejected";
                                Body = "Your Regularization are Rejected";
                            }
                            else if (regStatus?.ToLower() == "Cancelled"?.ToLower())
                            {
                                Subject = "Your Regularization are Cancelled";
                                Body = "Your Regularization are Cancelled";
                            }

                            List<Notifications> notifications = new();
                            Notifications notification = new();
                            notification = new()
                            {
                                CreatedBy = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                                CreatedOn = DateTime.UtcNow,
                                FromId = employeeandManager?.ReportingManagerID == null ? 0 : (int)employeeandManager?.ReportingManagerID,
                                ToId = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                                MarkAsRead = false,
                                NotificationSubject = Subject,
                                NotificationBody = approveOrRejectLeave?.ApproverName + " has " + regStatus?.ToLower() + " your regularization request for " + approveOrRejectLeave?.TimeLog?.LeaveDate?.ToString() + ".",
                                PrimaryKeyId = approveOrRejectLeave?.TimeLog?.AttendanceDetailId,
                                ButtonName = "View Regularization",
                                SourceType = "Regularization",
                            };
                            string regNotification = _commonFunction.Notification(notification).Result;

                            DateTime checkindateTime = DateTime.Parse(approveOrRejectLeave?.TimeLog?.FromTime);
                            DateTime checkoutdateTime = DateTime.Parse(approveOrRejectLeave?.TimeLog?.ToTime);
                            var CheckinTotalHrs = checkoutdateTime - checkindateTime;
                            var TotalHrscheckin = new DateTime(CheckinTotalHrs.Ticks);
                            var TotalHrs = CheckinTotalHrs.ToString("hh\\:mm");
                            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime localcheckintime = TimeZoneInfo.ConvertTimeFromUtc(checkindateTime, zone);
                            TimeSpan? iSTcheckinTime = localcheckintime.TimeOfDay;
                            DateTime iSTlocalcheckouttime = TimeZoneInfo.ConvertTimeFromUtc(checkoutdateTime, zone);
                            TimeSpan? iSTcheckoutTime = iSTlocalcheckouttime.TimeOfDay;
                            var checkinTime = new DateTime(iSTcheckinTime.Value.Ticks); // Date part is 01-01-0001
                            var formattedCheckinTime = checkinTime.ToString("h:mm tt", CultureInfo.InvariantCulture);
                            var checkoutTime = new DateTime(iSTcheckoutTime.Value.Ticks); // Date part is 01-01-0001
                            var formattedCheckOutTime = checkoutTime.ToString("h:mm tt", CultureInfo.InvariantCulture);
                            string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Date</b></td><td><b>Check-in Time</b></td><td><b>Check-out Time</b></td><td><b>Logged Hours</b></td><td><b>Regularization Status</b></td></tr>";
                                textBody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.TimeLog?.LeaveDate?.ToString() + "</td><td >" + formattedCheckinTime?.ToString() + "</td><td > " + formattedCheckOutTime?.ToString() + "</td><td >" + (TotalHrs+" Hrs")?.ToString() + "</td><td >" + regStatus?.ToString() + "</td></tr></table>";

                                MailSubject = "Your request for Regularization is {leaveStatus}";
                                MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your {LeaveTypeName} request has been {Status} by {ApproverName}. Please click <a href='{link}/#/pmsnexus/workday?isManager=true&RequestType=Attendance'>here</a> to view.</p>                                   
                                    <div>{table}</div>                                   
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>    
                                    </html>";
                                MailSubject = MailSubject.Replace("{leaveStatus}", regStatus);
                                MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                MailBody = MailBody.Replace("{LeaveTypeName}", approveOrRejectLeave?.LeaveTypeName);
                                MailBody = MailBody.Replace("{Status}", regStatus);
                                MailBody = MailBody.Replace("{ApproverName}", approveOrRejectLeave?.ApproverName);
                                MailBody = MailBody.Replace("{link}", baseURL);
                            if (regStatus?.ToLower() == "approved")
                            {
                                MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                            }
                            else
                            {
                                MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.TimeLog?.RejectReason);
                            }
                                MailBody = MailBody.Replace("{table}", textBody);
                           
                            sendMailbyleaverequest = new()
                            {
                                FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                ToEmailID = employeeandManager?.EmployeeEmailID,
                                Subject = MailSubject,
                                MailBody = MailBody,
                                ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                Host = appsetting.GetSection("EmailHost").Value,
                                FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                CC = employeeandManager?.ManagerEmailID

                            };
                            string regMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                        }
                    }                  
                    return Ok(new
                    {
                        results.StatusCode,
                        results.StatusText,

                    });
                }
                else
                {
                    var result = await _client.PostAsJsonAsync(approveOrRejectLeave, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:ApproveOrRejectLeave"));
                    statusandApprover = JsonConvert.DeserializeObject<StatusandApproverDetails>(JsonConvert.SerializeObject(result?.Data));
                    var reasonResult = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveRejectionReason"));
                    List<LeaveRejectionReason> reasonList = JsonConvert.DeserializeObject<List<LeaveRejectionReason>>(JsonConvert.SerializeObject(reasonResult?.Data));
                    int Feedback = approveOrRejectLeave.LeaveRejectionReasonId;
                    if (approveOrRejectLeave.Feedback == null || approveOrRejectLeave?.Feedback == string.Empty) 
                    {
                        approveOrRejectLeave.Feedback = reasonList?.Where(x => x.LeaveRejectionReasonId == Feedback).Select(x => x.LeaveRejectionReasons).FirstOrDefault();
                    }
                    if (result != null && result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                    {
                        if (approveOrRejectLeave?.Status.ToLower() == "Approved".ToLower() || approveOrRejectLeave?.Status.ToLower() == "Rejected".ToLower() || approveOrRejectLeave?.Status.ToLower() == "Cancelled".ToLower())
                        {                                                                              
                            // Send Mail                            
                            List<MailLeaveList> leavelist = new();
                            MailLeaveList mailLeaveList = new();

                            if ((approveOrRejectLeave?.IsGrantLeaveRequest == false || approveOrRejectLeave.IsGrantLeaveRequest == null) && (approveOrRejectLeave?.IsRegularize == false || approveOrRejectLeave.IsRegularize == null))
                            {
                                // Notification

                                if (approveOrRejectLeave?.Status.ToLower() == "Approved".ToLower())
                                {
                                    Subject = "Your Leave(s) are Approved.";
                                    Body = "Your Leave(s) are Approved.";
                                }
                                else if (approveOrRejectLeave?.Status.ToLower() == "Rejected".ToLower())
                                {
                                    Subject = "Your Leave(s) are Rejected";
                                    Body = "Your Leave(s) are Rejected";
                                }
                                else if (approveOrRejectLeave?.Status.ToLower() == "Cancelled".ToLower())
                                {
                                    Subject = "Your Leave(s) are Cancelled";
                                    Body = "Your Leave(s) are Cancelled";
                                }
                                DateTime? fromdate=DateTime.MinValue , todate = DateTime.MinValue;
                                if (approveOrRejectLeave?.AppliedLeaveApproveOrReject!=null)
                                {
                                    fromdate = approveOrRejectLeave.AppliedLeaveApproveOrReject.Select(x => x.LeaveDate).FirstOrDefault();
                                    todate = approveOrRejectLeave.AppliedLeaveApproveOrReject.Select(x => x.LeaveDate).LastOrDefault();
                                }
                                List<Notifications> notifications = new();
                                Notifications notification = new();
                                notification = new()
                                {
                                    CreatedBy = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                                    CreatedOn = DateTime.UtcNow,
                                    FromId = approveOrRejectLeave?.ManagerId == null ? 0 : (int)approveOrRejectLeave?.ManagerId,
                                    ToId = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                                    MarkAsRead = false,
                                    NotificationSubject = Subject,
                                    NotificationBody = approveOrRejectLeave?.ApproverName + " has " + approveOrRejectLeave?.Status.ToLower() + " your request for " + approveOrRejectLeave?.LeaveTypeName + " " + fromdate?.ToString("dd MMM yyyy") + " to " + todate?.ToString("dd MMM yyyy") + ".",
                                    PrimaryKeyId = approveOrRejectLeave?.LeaveId,
                                    ButtonName = "View Leave",
                                    SourceType = "Leaves",
                                };
                                string leaveNotification = _commonFunction.Notification(notification).Result;
                               
                                // Mail Template
                                if (approveOrRejectLeave?.Status.ToLower() == "Cancelled".ToLower())
                                {
                                    string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>From Date</b></td><td><b>To Date</b></td><td><b>Leave Status</b></td></tr>";
                                    textBody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.FromDate.ToString() + "</td><td > " + approveOrRejectLeave?.ToDate.ToString() + "</td><td >" + approveOrRejectLeave?.Status.ToString() + "</td></tr></table>";

                                    MailSubject = "Your request for leave is {leaveStatus}";
                                    MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your {LeaveTypeName} request has been {Status} by {ApproverName}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Leaves'>here</a> to view.</p>                                       
                                    <div>{table}</div>  
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback} </p></td></tr></tbody></table>
                                    </body>                                   
                                    </html>";
                                    MailSubject = MailSubject.Replace("{leaveStatus}", approveOrRejectLeave?.Status);
                                    MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                    MailBody = MailBody.Replace("{LeaveTypeName}", approveOrRejectLeave?.LeaveTypeName);
                                    MailBody = MailBody.Replace("{Status}", approveOrRejectLeave?.Status);
                                    MailBody = MailBody.Replace("{ApproverName}", approveOrRejectLeave?.ApproverName);
                                    MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                                    MailBody = MailBody.Replace("{table}", textBody);
                                    MailBody = MailBody.Replace("{link}", baseURL);
                                }
                                else
                                {
                                    foreach (var item in approveOrRejectLeave?.AppliedLeaveApproveOrReject)
                                    {
                                        mailLeaveList = new()
                                        {
                                            LeaveDate = item?.LeaveDate?.ToString("dd MMM yyyy"),
                                            LeaveStatus = item?.AppliedLeaveStatus == true ? "Approved" : "Rejected",
                                            LeaveDuration = item?.IsFirstHalf == true ? "FirstHalf" : item?.IsSecondHalf == true ? "SecondHalf" : "FullDay",
                                        };
                                        leavelist.Add(mailLeaveList);
                                    }

                                    string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Date</b></td><td ><b>Leave Duration</b></td><td><b>Leave Status</b></td></tr>";
                                    for (int i = 0; i < leavelist?.Count; i++)
                                    {
                                        textBody += "<tr style='text-align:center';><td >" + leavelist[i].LeaveDate.ToString() + "</td><td > " + leavelist[i].LeaveDuration.ToString() + "</td><td >" + leavelist[i].LeaveStatus.ToString() + "</td> </tr>";
                                    }
                                    textBody += "</table>";
                                    MailSubject = "Your request for leave is {leaveStatus}";
                                    MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your {LeaveTypeName} request has been {Status}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Leaves'>here</a> to view.</p>                                   
                                    <div>{table}</div>
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>                                   
                                    </html>";
                                    MailSubject = MailSubject.Replace("{leaveStatus}", approveOrRejectLeave?.Status);
                                    MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                    MailBody = MailBody.Replace("{LeaveTypeName}", approveOrRejectLeave?.LeaveTypeName);
                                    MailBody = MailBody.Replace("{Status}", approveOrRejectLeave?.Status);
                                    MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                                    MailBody = MailBody.Replace("{table}", textBody);
                                    //<table><tbody><tr><td><p><b>Comments:</b>{Feedback}</p></td></tr></tbody></table>
                                    MailBody = MailBody.Replace("{link}", baseURL);

                                }

                                sendMailbyleaverequest = new()
                                {
                                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                    ToEmailID = employeeandManager?.EmployeeEmailID,
                                    Subject = MailSubject,
                                    MailBody = MailBody,
                                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                    Host = appsetting.GetSection("EmailHost").Value,
                                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                    CC = employeeandManager?.ManagerEmailID

                                };
                                string leaveMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                            }
                            else if (approveOrRejectLeave?.IsGrantLeaveRequest == true)
                            {
                                //Next level approver details
                                EmployeeandManagerView nextlevelmanager = new EmployeeandManagerView();
                                if (statusandApprover?.ApproverID != null)
                                {
                                    var nextmanresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + statusandApprover.ApproverID);
                                    nextlevelmanager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(nextmanresults?.Data));
                                }

                                // Notification
                               
                                if (approveOrRejectLeave?.Status.ToLower() == "Approved".ToLower())
                                {
                                    Subject = "Your GrantLeave Request are Approved.";
                                    Body = approveOrRejectLeave?.ApproverName + " has approved your "+ approveOrRejectLeave?.LevelId + " - level request for " + approveOrRejectLeave?.LeaveTypeName + ". Effective From Date : " + approveOrRejectLeave?.FromDate?.ToString() + ".";
                                }
                                else if (approveOrRejectLeave?.Status.ToLower() == "Rejected".ToLower())
                                {
                                    Subject = "Your GrantLeave Request are Rejected";
                                    Body = approveOrRejectLeave?.ApproverName + " has rejected your " + approveOrRejectLeave?.LevelId + " - level request for " + approveOrRejectLeave?.LeaveTypeName + ". Effective From Date : " + approveOrRejectLeave?.FromDate?.ToString() + ".";
                                }
                                else if (approveOrRejectLeave?.Status.ToLower() == "Cancelled".ToLower())
                                {
                                    Subject = "Your GrantLeave Request are Cancelled";
                                    Body = approveOrRejectLeave?.ApproverName + " has cancelled your " + approveOrRejectLeave?.LevelId + " - level request for " + approveOrRejectLeave?.LeaveTypeName + ". Effective From Date : " + approveOrRejectLeave?.FromDate?.ToString() + ".";
                                }

                                List<Notifications> notifications = new();
                                Notifications notification = new();
                                notification = new()
                                {
                                    CreatedBy = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                                    CreatedOn = DateTime.UtcNow,
                                    FromId = approveOrRejectLeave?.ManagerId == null ? 0 : (int)approveOrRejectLeave.ManagerId,
                                    ToId = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                                    MarkAsRead = false,
                                    NotificationSubject = Subject,
                                    NotificationBody = Body,
                                    PrimaryKeyId = approveOrRejectLeave?.LeaveId,
                                    ButtonName = "View Grant Leave",
                                    SourceType = "GrantLeaves",
                                };
                                string grantNotification = _commonFunction.Notification(notification).Result;

                                //Next Level Manager Notification
                                Notifications nextManagerNotification = new();
                                if (nextlevelmanager != null)
                                {
                                    nextManagerNotification = new()
                                    {
                                        CreatedBy = employeeandManager?.EmployeeID == null ? 0 : (int)employeeandManager?.EmployeeID,
                                        CreatedOn = DateTime.UtcNow,
                                        FromId = employeeandManager?.EmployeeID == null ? 0 : (int)employeeandManager?.EmployeeID,
                                        ToId = nextlevelmanager?.EmployeeID == null ? 0 : (int)nextlevelmanager?.EmployeeID,
                                        MarkAsRead = false,
                                        NotificationSubject = "New GrantLeave request from " + employeeandManager?.EmployeeName + ".",
                                        NotificationBody = employeeandManager?.EmployeeName + "'s " + approveOrRejectLeave?.LeaveTypeName+" " + statusandApprover?.LevelId + " - level request is waiting for your approval." + "Effective From Date : " + approveOrRejectLeave?.FromDate?.ToString() + ".",
                                        PrimaryKeyId = approveOrRejectLeave?.LeaveId,
                                        ButtonName = "Approve Grant Leave",
                                        SourceType = "GrantLeaves",
                                    };
                                    string grantManagerNotification = _commonFunction.Notification(nextManagerNotification).Result;
                                }
                           
                                // Mail Template

                                EmployeeandManagerView approvedmanager = new EmployeeandManagerView();
                                var nextresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + statusandApprover.ApproverManagerId);
                                approvedmanager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(nextresults?.Data));

                                if (approveOrRejectLeave?.Status?.ToLower() == "Cancelled".ToLower())
                                {
                                    string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Type</b></td><td><b>Effective From Date</b></td><td><b>No.Of Days</b></td><td><b>Leave Status</b></td></tr>";
                                    textBody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.LeaveTypeName + "</td><td >" + approveOrRejectLeave?.FromDate?.ToString() + "</td><td >" + approveOrRejectLeave?.NoOfDays + "</td><td >" + approveOrRejectLeave?.Status?.ToString() + "</td></tr></table>";
                                    MailSubject = "Your request for leave is {leaveStatus}";
                                    MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your {LeaveTypeName} request has been {Status} by {ApproverName}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Leaves'>here</a> to view.</p>                                   
                                    <div>{table}</div>                                   
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>                                   
                                    </html>";
                                    MailSubject = MailSubject.Replace("{leaveStatus}", approveOrRejectLeave?.Status);
                                    MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                    MailBody = MailBody.Replace("{LeaveTypeName}", approveOrRejectLeave?.LeaveTypeName);
                                    MailBody = MailBody.Replace("{Status}", approveOrRejectLeave?.Status);
                                    MailBody = MailBody.Replace("{ApproverName}", approveOrRejectLeave?.ApproverName);
                                    MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                                    MailBody = MailBody.Replace("{table}", textBody);
                                    MailBody = MailBody.Replace("{link}", baseURL);
                                    sendMailbyleaverequest = new()
                                    {
                                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                        ToEmailID = employeeandManager?.EmployeeEmailID,
                                        Subject = MailSubject,
                                        MailBody = MailBody,
                                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                        Host = appsetting.GetSection("EmailHost").Value,
                                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                        CC = approvedmanager?.EmployeeEmailID
                                    };
                                    string grantMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                                }
                                else
                                {
                                    //EmployeeandManagerView approvedmanager = new EmployeeandManagerView();
                                    //var nextresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + statusandApprover.ApproverManagerId);
                                    //approvedmanager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(nextresults?.Data));

                                    string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Type</b></td><td><b>Effective From Date</b></td><td><b>No.Of Days</b></td><td><b>Leave Status</b></td></tr>";
                                    textBody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.LeaveTypeName + "</td><td >" + approveOrRejectLeave?.FromDate?.ToString() + "</td><td >" + approveOrRejectLeave?.NoOfDays + "</td><td >" + approveOrRejectLeave?.Status?.ToString() + "</td></tr></table>";

                                    MailSubject = "Your request for grant leave is {leaveStatus}.";
                                    MailBody = @"<html>
                                    <body>                                    
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your grant leave request has been {Status} by {ManagerName}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Leaves'>here</a> to view.</p> 
                                    <div>{table}</div>                                   
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>                                
                                    </html>";
                                    MailSubject = MailSubject.Replace("{leaveStatus}", approveOrRejectLeave?.Status);
                                    MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                    MailBody = MailBody.Replace("{LeaveName}", approveOrRejectLeave?.LeaveTypeName);
                                    MailBody = MailBody.Replace("{Status}", approveOrRejectLeave?.Status);
                                    if (approveOrRejectLeave?.ApproverName != null)
                                    {
                                        MailBody = MailBody.Replace("{ManagerName}", approveOrRejectLeave?.ApproverName);
                                    }
                                    else
                                    {
                                        MailBody = MailBody.Replace("{ManagerName}", employeeandManager?.ManagerName);
                                    }
                                    MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                                    MailBody = MailBody.Replace("{table}", textBody);
                                    MailBody = MailBody.Replace("{link}", baseURL);

                                    sendMailbyleaverequest = new()
                                    {
                                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                            ToEmailID = employeeandManager?.EmployeeEmailID,
                                            Subject = MailSubject,
                                            MailBody = MailBody,
                                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                            Host = appsetting.GetSection("EmailHost").Value,
                                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                            CC = approvedmanager?.EmployeeEmailID
                                    };
                                    string grantMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;

                                    if (statusandApprover?.ApproverID !=null)
                                    {
                                        //Next level approver mail

                                        string textbody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Type</b></td><td><b>Effective From Date</b></td><td><b>No.Of Days</b></td></tr>";
                                        textbody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.LeaveTypeName + "</td><td > " + approveOrRejectLeave?.FromDate?.ToString() + "</td><td >" + approveOrRejectLeave?.NoOfDays + "</td></tr></table>";

                                        MailSubject = "{EmployeeName} sent grant leave request.";
                                        MailBody = @"<html>
                                        <body>                                  
                                        <p>Dear {ManagerName},</p>                                    
                                        <p> {EmployeeName} requested for grant leave from {FromDate}. Please click <a href='{link}/#/pmsnexus/workday?isManager=true&RequestType=Leaves'>here</a> to Approve/Reject.</p>                                    
                                        <div>{table}</div>  
                                        <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                        </body>                                   
                                        </html>";
                                        MailSubject = MailSubject.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                        MailBody = MailBody.Replace("{ManagerName}", nextlevelmanager?.EmployeeName);
                                        MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                        MailBody = MailBody.Replace("{FromDate}", approveOrRejectLeave?.FromDate);
                                        MailBody = MailBody.Replace("{table}", textbody);
                                        MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Reason);
                                        MailBody = MailBody.Replace("{link}", baseURL);

                                        sendMailbyleaverequest = new()
                                        {
                                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                            ToEmailID = nextlevelmanager?.EmployeeEmailID,
                                            Subject = MailSubject,
                                            MailBody = MailBody,
                                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                            Host = appsetting.GetSection("EmailHost").Value,
                                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                            CC = employeeandManager?.EmployeeEmailID
                                        };
                                        string grantNextlLevelMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                                    }
                                }                                                                               
                            }                                                     
                        }
                        return Ok(new
                        {
                            result.StatusCode,
                            result.StatusText,
                        });
                    }                    
                }             
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/ApproveOrRejectLeave", JsonConvert.SerializeObject(approveOrRejectLeave));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                LeaveId,
            });
        }
        #endregion

        #region Add or update Leaves to Apply Leave       
        [HttpPost]
        [Route("InsertorUpdateApplyLeave")]
        public async Task<IActionResult> InsertorUpdateApplyLeave(AddLeaveView AddLeaveView)
        {
            int leaveId = 0;
            string statusText = "", statusCode = "";
            List<AttendaceDetails> employeeAttendanceList = new();
            try
            {
                bool isExistAttendance = false;
                DateTime from_date = new DateTime((int)AddLeaveView?.LeaveDetails?.FromDate?.Year, (int)AddLeaveView?.LeaveDetails?.FromDate?.Month, (int)AddLeaveView?.LeaveDetails?.FromDate?.Day);
                DateTime to_date = new DateTime(AddLeaveView.LeaveDetails.ToDate.Year, AddLeaveView.LeaveDetails.ToDate.Month, AddLeaveView.LeaveDetails.ToDate.Day);
                //* Get employee attendance details to apply leave on existing attendance:
                //var resultdata = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAttendanceDetailByDate") + AddLeaveView.LeaveDetails.EmployeeId + "&fromDate=" + from_date.ToString("yyyy-MM-dd") + "&toDate=" + to_date.ToString("yyyy-MM-dd"));
                //employeeAttendanceList = JsonConvert.DeserializeObject<List<AttendaceDetails>>(JsonConvert.SerializeObject(resultdata?.Data));

                //List<AttendanceTimeDetailsView> attendancetimeSpans = new List<AttendanceTimeDetailsView>();
                //if (employeeAttendanceList != null && employeeAttendanceList?.Count > 0)
                //{
                //    foreach (AttendaceDetails attendaceitem in employeeAttendanceList)
                //    {
                //        if (attendaceitem != null)
                //        {
                //            AttendanceTimeDetailsView attendanceTimeDetailsView = new();
                //            attendanceTimeDetailsView.AttendanceDate = attendaceitem?.Date;
                //            string hourval = attendaceitem?.TotalHours;
                //            string[] hoursval = default;
                //            if (hourval != null && hourval != "")
                //            {
                //                hoursval = hourval.Split(":");
                //                TimeSpan? attendaceHours = new TimeSpan(hoursval?.Length > 0 ? Convert.ToInt32(hoursval[0]) : 0, hoursval?.Length > 1 ? Convert.ToInt32(hoursval[1]) : 0, hoursval?.Length > 2 ? Convert.ToInt32(hoursval[2]) : 0);
                //                attendanceTimeDetailsView.timeSpans = attendaceHours;
                //            }
                //            DateTime checkedinTime = (DateTime)(attendaceitem?.CheckinTime.Value);
                //            string t = string.Format("{0:hh\\:mm\\:ss}", checkedinTime.TimeOfDay);
                //            string[] time = t?.Split(":");
                //            TimeSpan? checkInTime = new TimeSpan(time?.Length > 0 ? Convert.ToInt32(time[0]) : 0, time?.Length > 1 ? Convert.ToInt32(time[1]) : 0, time?.Length > 2 ? Convert.ToInt32(time[2]) : 0);
                //            attendanceTimeDetailsView.checkinTime = checkInTime;
                //            attendancetimeSpans.Add(attendanceTimeDetailsView);
                //        }
                //    }
                //}

                //string hour = employeeAttendanceList?.Select(x => x.TotalHours).FirstOrDefault();
                //string[] hours = default;
                //if (hour != null)
                //{
                //    hours = hour.Split(":");
                //}
                //TimeSpan? attendaceHour = new TimeSpan(hours?.Length > 0 ? Convert.ToInt32(hours[0]) : 0, hours?.Length > 1 ? Convert.ToInt32(hours[1]) : 0, hours?.Length > 2 ? Convert.ToInt32(hours[2]) : 0);

                //TimeSpan? hours = employeeAttendanceList?.Select(x => x.TotalHours).FirstOrDefault();
                //TimeSpan? attendaceHour = hours == null ? new TimeSpan(0,0,0) : hours;
                bool? IsCheckin = employeeAttendanceList?.Select(x => x?.IsCheckin).FirstOrDefault();
                var shiftResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetShifByDate") + AddLeaveView.LeaveDetails.EmployeeId + "&date=" + from_date.ToString("yyyy-MM-dd"));
                EmployeeShiftDetails employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDetails>(JsonConvert.SerializeObject(shiftResult?.Data));
                int ShiftId = (employeeShift?.ShiftDetailsId == null ? 0 : (int)employeeShift?.ShiftDetailsId);
                var shiftDetais = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetails") + ShiftId);
                ShiftDetailedView WeekendList = JsonConvert.DeserializeObject<ShiftDetailedView>(JsonConvert.SerializeObject(shiftDetais?.Data));
                ShiftViewDetails WeekendShiftList = new();
                WeekendShiftList = WeekendList?.shiftViewDetails;

                //string[] absentHour = WeekendShiftList?.HalfaDayFromHour?.Split(":");
                //TimeSpan? Halfdaypresenthours = new TimeSpan(absentHour?.Length > 0 ? Convert.ToInt32(absentHour[0]) : 0, absentHour?.Length > 1 ? Convert.ToInt32(absentHour[1]) : 0, absentHour?.Length > 2 ? Convert.ToInt32(absentHour[2]) : 0);
                //string[] totalhour = WeekendShiftList?.TotalHours?.Split(":");
                //TimeSpan? totalHours = new TimeSpan(totalhour?.Length > 0 ? Convert.ToInt32(totalhour[0]) : 0, totalhour?.Length > 1 ? Convert.ToInt32(totalhour[1]) : 0, totalhour?.Length > 2 ? Convert.ToInt32(totalhour[2]) : 0);
                //string[] fromHour = WeekendShiftList?.TimeFrom?.Split(":");
                //TimeSpan? timeFrom = new TimeSpan(fromHour?.Length > 0 ? Convert.ToInt32(fromHour[0]) : 0, fromHour?.Length > 1 ? Convert.ToInt32(fromHour[1]) : 0, fromHour?.Length > 2 ? Convert.ToInt32(fromHour[2]) : 0);
                //string[] toHour = WeekendShiftList?.TimeTo?.Split(":");
                //TimeSpan? timeTo = new TimeSpan(toHour?.Length > 0 ? Convert.ToInt32(toHour[0]) : 0, toHour?.Length > 1 ? Convert.ToInt32(toHour[1]) : 0, toHour?.Length > 2 ? Convert.ToInt32(toHour[2]) : 0);
                //TimeSpan? utcTime = new TimeSpan(5, 30, 0);
                //TimeSpan? duration = new TimeSpan(0, 0, 0);
                //if (timeTo.Value.Ticks < timeFrom.Value.Ticks)
                //{
                //    DateTime dt = new DateTime(timeTo.Value.Ticks).AddDays(1);
                //    DateTime df = new DateTime(timeFrom.Value.Ticks);
                //    if (dt >= df)
                //    {
                //        duration = (dt - df) / 2;
                //    }
                //}
                //if (timeTo.Value.Ticks >= timeFrom.Value.Ticks)
                //{
                //    duration = (timeTo - timeFrom) / 2;
                //}
                //TimeSpan? cutOffTime = (duration + timeFrom);
                //TimeSpan? utcTimeTo = new TimeSpan(0, 0, 0);
                //if (cutOffTime.Value.Ticks >= utcTime.Value.Ticks)
                //{
                //    utcTimeTo = cutOffTime - utcTime;
                //}
                //DateTime dtc = new DateTime(utcTimeTo.Value.Ticks);
                //DateTime utcCutOff = dtc;
                //TimeSpan? cutOffHour = utcCutOff.TimeOfDay;
                //WeekMonthAttendanceView weekMonthAttendance = new();
                //weekMonthAttendance.EmployeeId = AddLeaveView.LeaveDetails.EmployeeId;
                //weekMonthAttendance.FromDate = from_date;
                //weekMonthAttendance.ToDate = to_date;
                //var holidayResult = await _client.PostAsJsonAsync(weekMonthAttendance, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeavesByEmployeeId"));
                //List<AppliedLeaveTypeDetails> leaveHoliday = JsonConvert.DeserializeObject<List<AppliedLeaveTypeDetails>>(JsonConvert.SerializeObject(holidayResult?.Data));
                //List<AppliedLeaveTypeDetails> leaves = leaves = leaveHoliday?.Where(x => x.Date.Date <= from_date && x.Date.Date >= to_date).Select(x => x).ToList();
                //bool halfday = default;
                //if (leaves != null && leaves?.Count > 0)
                //{
                //    List<WeeklyMonthlyAttendance> Leavelist = new();
                //    foreach (var item in leaves)
                //    {
                //        WeeklyMonthlyAttendance leave = new();
                //        if (item.IsFirstHalf == true) { leave.IsFirstHalf = true; leave.IsSecondHalf = false; leave.IsLeave = true; }
                //        else if (item.IsSecondHalf == true) { leave.IsSecondHalf = true; leave.IsLeave = true; leave.IsFirstHalf = false; }
                //        else { leave.IsFirstHalf = false; leave.IsSecondHalf = false; }
                //        Leavelist.Add(leave);
                //    }
                //    halfday = Leavelist?.Where(x => (bool)x?.IsFirstHalf || (bool)x?.IsSecondHalf).Select(x => x)?.Count() <= 2 ? true : false;
                //}

                // *RESTRICT LEAVE ON EXISTING ATTENDANCE:
                //foreach (AppliedLeaveDetailsView appliedLeaveitem in AddLeaveView?.LeaveDetails?.AppliedLeaveDetails)
                //{
                //    List<AttendanceTimeDetailsView> attendanceTimeDetailsList = attendancetimeSpans?.Where(rs => rs.AttendanceDate == appliedLeaveitem.Date).ToList();
                //    if (attendanceTimeDetailsList != null && attendanceTimeDetailsList?.Count > 0)
                //    {
                //        if (appliedLeaveitem.IsFullDay)
                //        {
                //            if (attendanceTimeDetailsList[0].timeSpans >= totalHours)
                //            {
                //                isExistAttendance = true;
                //            }
                //            if (attendanceTimeDetailsList[0].timeSpans >= Halfdaypresenthours)
                //            {
                //                isExistAttendance = true;
                //            }
                //        }
                //        else if (appliedLeaveitem.IsFirstHalf)
                //        {
                //            if (attendanceTimeDetailsList[0].timeSpans >= totalHours)
                //            {
                //                isExistAttendance = true;
                //            }
                //            if (attendanceTimeDetailsList[0].timeSpans >= Halfdaypresenthours && attendanceTimeDetailsList[0]?.checkinTime <= cutOffHour)
                //            {
                //                isExistAttendance = true;
                //            }
                //        }
                //        else if (appliedLeaveitem.IsSecondHalf)
                //        {
                //            if (attendanceTimeDetailsList[0].timeSpans >= totalHours)
                //            {
                //                isExistAttendance = true;
                //            }
                //            if (attendanceTimeDetailsList[0].timeSpans >= Halfdaypresenthours && attendanceTimeDetailsList[0]?.checkinTime >= cutOffHour)
                //            {
                //                isExistAttendance = true;
                //            }
                //        }
                //    }
                //}
                //if (employeeAttendanceList != null && employeeAttendanceList.Count == 0)
                //if (isExistAttendance)
                //{
                //    return Ok(new
                //    {
                //        StatusCode = "FAILURE",
                //        StatusText = "Exists Attendance details. Can not able to Apply Leave.",
                //        Data = 0
                //    });
                //}
                //else
                //{
                      EmployeeandManagerView employeeandManager = new EmployeeandManagerView();
                     var employeeID = AddLeaveView?.LeaveDetails?.EmployeeId;

                    var results = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeID);
                    employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(results?.Data));
                    AddLeaveView.LeaveDetails.ApproverManagerId = employeeandManager.ReportingManagerID == null ? 0 : (int)employeeandManager.ReportingManagerID;
                    ApplyLeavesView leaveDetails = AddLeaveView?.LeaveDetails;
                    leaveDetails.DepartmentId = employeeandManager?.DepartmentId == null ? 0 : (int)employeeandManager?.DepartmentId;
                    leaveDetails.LocationId = employeeandManager?.LocationId == null ? 0 : (int)employeeandManager?.LocationId;
                    leaveDetails.ShiftId = ShiftId;
                    leaveDetails.WeekendList = WeekendShiftList?.WeekendList;
                leaveDetails.RelivingDate = employeeandManager.RelivingDate;
                var result = await _client.PostAsJsonAsync(leaveDetails, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:InsertorUpdateApplyLeave"));
                    leaveId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                    if (result != null && result?.Data != 0)
                    {
                        int LeaveId = (int)result?.Data;
                        if (LeaveId > 0 && AddLeaveView?.ListOfDocuments?.Count > 0)
                        {
                            SupportingDocumentsView supportingDocuments = new()
                            {
                                ListOfDocuments = new List<DocumentDetails>(),
                                SourceId = LeaveId,
                                SourceType = _configuration.GetValue<string>("LeavesSourceType"),
                                BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory"),
                                CreatedBy = leaveDetails?.CreatedBy == null ? 0 : (int)leaveDetails?.CreatedBy
                            };
                            //Add supporting documents
                            if (!string.IsNullOrEmpty(supportingDocuments.BaseDirectory))
                            {
                                //Create base directory
                                if (!Directory.Exists(supportingDocuments.BaseDirectory))
                                {
                                    Directory.CreateDirectory(supportingDocuments.BaseDirectory);
                                }
                                //Create source type directory
                                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType)))
                                {
                                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType));
                                }
                                //Create leaveId directory
                                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString())))
                                {
                                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString()));
                                }
                            }
                            string directoryPath = Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString());
                            List<DocumentDetails> docList = new List<DocumentDetails>();
                            foreach (var item in AddLeaveView?.ListOfDocuments)
                            {
                                string documentPath = Path.Combine(directoryPath, item.DocumentName);
                                if (!System.IO.File.Exists(item.DocumentName) && item.DocumentSize > 0)
                                {
                                    if (item.DocumentAsBase64.Contains(","))
                                    {
                                        item.DocumentAsBase64 = item.DocumentAsBase64.Substring(item.DocumentAsBase64.IndexOf(",") + 1);
                                    }
                                    item.DocumentAsByteArray = Convert.FromBase64String(item.DocumentAsBase64);
                                    using (Stream fileStream = new FileStream(documentPath, FileMode.Create))
                                    {
                                        fileStream.Write(item.DocumentAsByteArray, 0, item.DocumentAsByteArray.Length);
                                    }
                                }
                                DocumentDetails docDetails = new()
                                {
                                    DocumentCategory = item.DocumentCategory,
                                    IsApproved = false,
                                    DocumentSize = item.DocumentSize,
                                    DocumentName = item.DocumentName,
                                    DocumentType = Path.GetExtension(item.DocumentName)
                                };
                                docList.Add(docDetails);
                            }
                            supportingDocuments.ListOfDocuments = docList;
                            using var documentClient = new HttpClient
                            {
                                BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                            };
                            HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/AddSupportingDocuments", supportingDocuments);
                            var documentResult = documentResponse.Content.ReadAsAsync<SuccessData>();
                            if (documentResponse?.IsSuccessStatusCode == false)
                            {
                                statusCode = "FAILURE";
                                statusText = documentResult?.Result?.StatusText;
                            }
                        }
                        if(AddLeaveView?.LeaveDetails?.Status?.ToLower() == "pending" || AddLeaveView?.LeaveDetails?.Status?.ToLower() == "")
                        { 
                        //Notification Alert
                        //EmployeeandManagerView employeeandManager = new EmployeeandManagerView();
                        //var employeeID = AddLeaveView?.LeaveDetails?.EmployeeId;

                        //var results = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeID);
                        //employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(results?.Data));
                        List<Notifications> notifications = new List<Notifications>();
                        Notifications notification = new Notifications();
                        Notifications empNotification = new Notifications();
                        string notificationButtonName = "", notificationSourceType="";
                        if (AddLeaveView?.LeaveDetails?.IsGrantLeave == true)
                        {
                            notificationButtonName = "Approve Grant Leave";
                            notificationSourceType = "GrantLeaves";
                        }
                        else
                        {
                            notificationButtonName = "Approve Leave";
                            notificationSourceType = "Leaves";
                        }
                        notification = new Notifications
                        {
                            CreatedBy = AddLeaveView?.LeaveDetails?.EmployeeId == null ? 0 : (int)AddLeaveView?.LeaveDetails?.EmployeeId,
                            CreatedOn = DateTime.UtcNow,
                            FromId = AddLeaveView?.LeaveDetails?.EmployeeId == null ? 0 : (int)AddLeaveView?.LeaveDetails?.EmployeeId,
                            ToId = employeeandManager?.ReportingManagerID == null ? 0 : (int)employeeandManager?.ReportingManagerID,
                            MarkAsRead = false,
                            NotificationSubject = "New Leave request from " + employeeandManager?.EmployeeName + ".",
                            NotificationBody = employeeandManager?.EmployeeName + "'s " + AddLeaveView?.LeaveDetails?.FromDate?.ToString("dd MMM yyyy") + " to " + AddLeaveView?.LeaveDetails?.ToDate.ToString("dd MMM yyyy") +" "+ AddLeaveView?.LeaveDetails?.LeaveType +" request is waiting for your approval.",
                            PrimaryKeyId = AddLeaveView?.LeaveDetails?.LeaveId,
                            ButtonName = notificationButtonName,
                            SourceType = notificationSourceType
                        };
                        string leaveManagerNotification = _commonFunction.Notification(notification).Result;
                        if (AddLeaveView?.LeaveDetails?.IsGrantLeave == true)
                        {
                            notificationButtonName = "View Grant Leave";
                            notificationSourceType = "GrantLeaves";
                        }
                        else
                        {
                            notificationButtonName = "View Leave";
                            notificationSourceType = "Leaves";
                        }
                        empNotification = new Notifications
                        {
                            CreatedBy = AddLeaveView?.LeaveDetails?.EmployeeId == null ? 0 : (int)AddLeaveView?.LeaveDetails?.EmployeeId,
                            CreatedOn = DateTime.UtcNow,
                            FromId = AddLeaveView?.LeaveDetails?.EmployeeId == null ? 0 : (int)AddLeaveView?.LeaveDetails?.EmployeeId,
                            ToId = AddLeaveView?.LeaveDetails?.EmployeeId == null ? 0 : (int)AddLeaveView?.LeaveDetails?.EmployeeId,
                            MarkAsRead = false,
                            NotificationSubject = "Leave sent for approval.",
                            NotificationBody = "Your " + AddLeaveView?.LeaveDetails?.LeaveType +" request for " + AddLeaveView?.LeaveDetails?.FromDate?.ToString("dd MMM yyyy") + " to " + AddLeaveView?.LeaveDetails?.ToDate.ToString("dd MMM yyyy") + " has been sent for approval.",
                            PrimaryKeyId = AddLeaveView?.LeaveDetails?.LeaveId,
                            ButtonName = notificationButtonName,
                            SourceType = notificationSourceType
                        };
                        string leaveEmpNotification = _commonFunction.Notification(empNotification).Result;

                        // Send Mail
                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                        string MailSubject, MailBody;
                        List<MailLeaveList> leavelist = new();
                        MailLeaveList mailLeaveList = new();

                        //var fromdate = AddLeaveView.LeaveDetails.FromDate?.ToString("dd MMM yyyy");
                        //var todate = AddLeaveView.LeaveDetails.ToDate.ToString("dd MMM yyyy");     //("MM/dd/yyyy");

                        //if (fromdate == todate)
                        //{
                        //    Applyleavedate = fromdate;
                        //}
                        //else
                        //{
                        //    Applyleavedate = fromdate + " to " + todate;
                        //}


                        //Mail


                        foreach (var item in AddLeaveView?.LeaveDetails?.AppliedLeaveDetails)
                        {
                            mailLeaveList = new()
                            {
                                LeaveDate = item.Date.ToString("dd MMM yyyy"),
                                LeaveStatus = "Pending",
                                LeaveDuration = item.IsFirstHalf == true ? "FirstHalf" : item.IsSecondHalf == true ? "SecondHalf" : "FullDay",
                            };
                            leavelist.Add(mailLeaveList);
                        }

                        string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Date</b></td><td ><b>Leave Duration</b></td><td><b>Leave Status</b></td></tr>";
                        for (int i = 0; i < leavelist?.Count; i++)
                        {
                            textBody += "<tr style='text-align:center;'><td >" + leavelist[i].LeaveDate.ToString() + "</td><td > " + leavelist[i].LeaveDuration.ToString() + "</td><td >" + leavelist[i].LeaveStatus.ToString() + "</td> </tr>";
                        }
                        textBody += "</table>";
                        if (AddLeaveView?.LeaveDetails?.IsGrantLeave==true )
                        {
                            string baseURL = appsetting.GetSection("BaseURL").Value;
                            MailSubject = @"{EmployeeName} sent Leave request.";
                            MailBody = @"<html>
                                    <body>                                   
                                    <p>Dear {ManagerName},</p>
                                    <p>{EmployeeName} has requested for grant leave on below date. Please click <a href='{link}/#/pmsnexus/leaves/leave-team'>here</a> to Approve/Reject. </p>   
                                    <div>{table}</div>
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>
                                    </html>";
                            MailSubject = MailSubject.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                            MailBody = MailBody.Replace("{ManagerName}", employeeandManager?.ManagerName);
                            MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                            MailBody = MailBody.Replace("{table}", textBody);
                            MailBody = MailBody.Replace("{link}", baseURL);
                            MailBody = MailBody.Replace("{Feedback}", AddLeaveView?.LeaveDetails.Reason);
                        }
                        else
                        {
                            string baseURL = appsetting.GetSection("BaseURL").Value;
                            MailSubject = @"{EmployeeName} sent Leave request.";
                            MailBody = @"<html>
                                    <body>
                                    <p>Dear {ManagerName},</p>   
                                    <p>{EmployeeName} has requested {LeaveType} on below date. Please click <a href='{link}/#/pmsnexus/leaves/leave-team'>here</a> to Approve/Reject.</p>  
                                    <div>{table}</div>  
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>
                                    </html>";
                            //<table><tbody><tr><td><p style='font-size: 14px;'><b>Comments :</b>{Feedback}</p></td></tr></tbody></table>
                            MailSubject = MailSubject.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                            MailBody = MailBody.Replace("{ManagerName}", employeeandManager?.ManagerName);
                            MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                            MailBody = MailBody.Replace("{table}", textBody);
                            MailBody = MailBody.Replace("{Feedback}", AddLeaveView.LeaveDetails?.Reason);
                            MailBody = MailBody.Replace("{LeaveType}", AddLeaveView.LeaveDetails?.LeaveType);
                            MailBody = MailBody.Replace("{link}", baseURL);
                        }
                        SendEmailView sendMailbyleaverequest = new();
                        sendMailbyleaverequest = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = employeeandManager?.ManagerEmailID,
                            Subject = MailSubject,
                            MailBody = MailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                            CC=employeeandManager?.EmployeeEmailID
                        };
                        string mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                        }
                        if (statusCode == "")
                        {
                            return Ok(new
                            {
                                result?.StatusCode,
                                result?.StatusText,
                                leaveId
                            });
                        }
                    }
                    else
                        statusText = result?.StatusText;
                //}
                
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/InsertorUpdateApplyLeave", JsonConvert.SerializeObject(AddLeaveView));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText,
                leaveId
            });
        }
        #endregion

        //#region Get All Applied Leave Details By Emploee Id Id
        //[HttpGet]
        //[AllowAnonymous]
        //[Route("GetAllAppliedLeavesByEmployeeId")]
        //public async Task<IActionResult> GetAllAppliedLeavesByEmployeeId(int employeeId, DateTime FromDate, DateTime ToDate)
        //{
        //    IndividualLeaveList individualLeaveList = new();
        //    try
        //    {
        //        var departmentResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDepartmentIdByEmployeeId") + employeeId);
        //        int departmentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(departmentResult?.Data));
        //        var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAllAppliedLeavesByEmpId") + employeeId + "&FromDate=" + FromDate.ToString("yyyy-MM-dd") + "&ToDate=" + ToDate.ToString("yyyy-MM-dd") + "&departmentId=" + departmentId);
        //        individualLeaveList = JsonConvert.DeserializeObject<IndividualLeaveList>(JsonConvert.SerializeObject(result?.Data));
        //        if (individualLeaveList != null)
        //        {
        //            var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDepartmentAndLocation") + employeeId);
        //            EmployeeDepartmentAndLocationView employeeDepartments = JsonConvert.DeserializeObject<EmployeeDepartmentAndLocationView>(JsonConvert.SerializeObject(employeeResult?.Data));
        //            int ShiftId = 0;
        //            if (employeeDepartments != null)
        //            {
        //                employeeDepartments.fromDate = FromDate;
        //                employeeDepartments.toDate = ToDate;
        //                ShiftId = employeeDepartments.ShiftId;
        //            }
        //            var results = await _client.PostAsJsonAsync(employeeDepartments, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeAbsentList"));
        //            List<EmployeeAbsentListView> absentListViews = JsonConvert.DeserializeObject<List<EmployeeAbsentListView>>(JsonConvert.SerializeObject(results?.Data));
        //            var hoildayResult = await _client.PostAsJsonAsync(employeeDepartments, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeHolidayByDepartment"));
        //            LeaveHolidayView holidayView = JsonConvert.DeserializeObject<LeaveHolidayView>(JsonConvert.SerializeObject(hoildayResult?.Data));
        //            var shiftResult = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetails") + ShiftId);
        //            ShiftViewDetails WeekendShiftList = JsonConvert.DeserializeObject<ShiftViewDetails>(JsonConvert.SerializeObject(shiftResult.Data));
        //            if (absentListViews?.Count > 0)
        //            {
        //                List<EmployeeAbsentListView> absentDates = new();
        //                List<DateTime> leaveDate = holidayView?.ApplyLeaves.Select(x => x.FromDate).ToList();
        //                var absent = absentListViews?.Where(m => !leaveDate.Contains(m.FromDate)).ToList();
        //                List<DateTime> holidayDate = holidayView?.Holiday.Select(x => (DateTime)x.HolidayDate).ToList();
        //                var absentDate = absent?.Where(m => !holidayDate.Contains(m.FromDate)).ToList();
        //                List<string> weekendDays = WeekendShiftList?.WeekendList.Select(x => x.WeekendDayName).ToList();
        //                absentDates = absentDate?.Where(m => !weekendDays.Contains(m.FromDate.DayOfWeek.ToString())).ToList();
        //                List<AppliedLeaveView> AbsentListView = new();
        //                AbsentListView = (from item in absentDates
        //                                  select new AppliedLeaveView
        //                                  {
        //                                      EmployeeId = item.EmployeeId,
        //                                      FromDate = item.FromDate,
        //                                      ToDate = item.ToDate,
        //                                      NoOfDays = (decimal)((item.FromDate.Subtract(item.ToDate)).TotalDays + 1),
        //                                      LeaveType = item.IsAbsent == true ? "Absent" : "",
        //                                  }).ToList();
        //                individualLeaveList.applyLeavesView = (individualLeaveList?.applyLeavesView.Concat(AbsentListView)).ToList();
        //                List<AvailableLeaveDetailsView> AbsentListViewa = new();
        //                AbsentListViewa = (from item in absentDates
        //                                   select new AvailableLeaveDetailsView
        //                                   {
        //                                       FromDate = item.FromDate,
        //                                       ToDate = item.ToDate,
        //                                       AvailableLeaves = (decimal)((item.FromDate.Subtract(item.ToDate)).TotalDays + 1),
        //                                       LeaveType = item.IsAbsent == true ? "Absent" : "",
        //                                   }).ToList();
        //                individualLeaveList.AvailableLeaveDetails = (individualLeaveList?.AvailableLeaveDetails.Concat(AbsentListViewa)).ToList();
        //            }
        //        }
        //        individualLeaveList.applyLeavesView = individualLeaveList?.applyLeavesView?.OrderByDescending(x => x.FromDate).Select(x => x).ToList();
        //        return Ok(new
        //        {
        //            StatusCode = "SUCCESS",
        //            Data = individualLeaveList
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAllAppliedLeavesByEmpId", Convert.ToString(employeeId));
        //        return Ok(new
        //        {
        //            StatusCode = "FAILURE",
        //            StatusText = strErrorMsg,
        //            Data = individualLeaveList
        //        });
        //    }
        //}
        //#endregion

        #region Get Applied Leave By Leave Id to Edit
        [HttpGet]
        [Route("GetAppliedLeaveToEdit")]
        public async Task<IActionResult> GetAppliedLeaveToEdit(int leaveId)
        {
            AppliedLeaveEditView applyLeavesView = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAppliedLeaveToEdit") + leaveId);
                applyLeavesView = JsonConvert.DeserializeObject<AppliedLeaveEditView>(JsonConvert.SerializeObject(result?.Data));
                if (applyLeavesView != null)
                {
                    applyLeavesView.ListOfDocuments = new();
                    using var documentClient = new HttpClient();
                    SourceDocuments sourceDocuments = new()
                    {
                        SourceId = new List<int> { leaveId },
                        SourceType = new List<string> { _configuration.GetValue<string>("LeavesSourceType") }
                    };
                    documentClient.BaseAddress = new Uri(_notificationBaseURL);
                    HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/GetDocumentBySourceIdAndType", sourceDocuments);
                    if (documentResponse?.IsSuccessStatusCode == true)
                    {
                        var documentResults = documentResponse.Content.ReadAsAsync<SuccessData>();
                        List<SupportingDocuments> lstOfSupDocument = JsonConvert.DeserializeObject<List<SupportingDocuments>>(JsonConvert.SerializeObject(documentResults.Result.Data));
                        if (lstOfSupDocument?.Count > 0)
                        {
                            applyLeavesView.ListOfDocuments = lstOfSupDocument?.Where(x => x.DocumentName != null)
                                .Select(x => new DocumentDetails
                                {
                                    DocumentId = x.DocumentId,
                                    DocumentName = x.DocumentName,
                                    DocumentSize = x.DocumentSize,
                                    DocumentCategory = x.DocumentCategory,
                                    IsApproved = x.IsApproved,
                                    DocumentType = x.DocumentType
                                }).ToList();
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = applyLeavesView 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAppliedLeaveToEdit", Convert.ToString(leaveId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = applyLeavesView 
                });
            }
        }
        #endregion

        #region Delete Applied Leave
        [HttpGet]
        [Route("DeleteAppliedLeaveByLeaveId")]
        public async Task<IActionResult> DeleteAppliedLeaveByLeaveId(int leaveId)
        {
            try
            {
                var Result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:DeleteAppliedLeaveByLeaveId") + leaveId);
                if (Result != null && Result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                {
                    if (leaveId > 0)
                    {
                        string SourceType = _configuration.GetValue<string>("LeavesSourceType");
                        string BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                        string id = leaveId.ToString();
                            string documentPath = Path.Combine(BaseDirectory, SourceType, id);
                            var dir = new DirectoryInfo(documentPath);
                            if (dir.Exists)
                            {
                                dir.Delete(true);
                            }
                    }
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Leave deleted successfully.",
                        data = true
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/DeleteAppliedLeaveByLeaveId", JsonConvert.SerializeObject(leaveId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = false
            });
        }
        #endregion

        #region Get Available Applied Leaves
        [HttpGet]
        [Route("GetAvailableLeaveDetailsByEmployeeId")]
        public async Task<IActionResult> GetAvailableLeaveDetailsByEmployeeId(int employeeId)
        {
            List<AvailableLeaveDetailsView> AvailableLeaveDetails = new();
            try
            {
                var departmentResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDepartmentIdByEmployeeId") + employeeId);
                int departmentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(departmentResult?.Data));
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAvailableLeaveDetailsByEmployeeId") + employeeId + "&departmentId=" + departmentId);
                AvailableLeaveDetails = JsonConvert.DeserializeObject<List<AvailableLeaveDetailsView>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = AvailableLeaveDetails 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetAvailableLeaveDetailsByEmployeeId", JsonConvert.SerializeObject(employeeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = AvailableLeaveDetails 
            });
        }
        #endregion

        #region Get Leave Types Master Dropdown
        [HttpGet]
        [Route("GetLeaveTypes")]
        public IActionResult GetLeaveTypes(int employeeId)
        {
            List<LeaveTypesView> leaveTypesView = new();
            try
            {
                var depId = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDepartmentIdByEmployeeId") + employeeId);
                int departmentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(depId?.Result?.Data));
                var result = _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveTypes") + departmentId);
                leaveTypesView = JsonConvert.DeserializeObject<List<LeaveTypesView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    leaveTypesView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetLeaveTypes");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    leaveTypesView
                });
            }
        }
        #endregion

        #region Get Employee LeaveAdjustment
        [HttpGet]
        [Route("GetEmployeeLeaveAdjustmentDetails")]
        public async Task<IActionResult> GetEmployeeLeaveAdjustmentDetails(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<EmployeeLeaveAdjustment> employeeLeaveAdjustment = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeLeaveAdjustmentDetails") + employeeId + "&fromDate=" + fromDate.ToString("yyyy-MM-dd") + "&toDate=" + toDate.ToString("yyyy-MM-dd"));
                employeeLeaveAdjustment = JsonConvert.DeserializeObject<List<EmployeeLeaveAdjustment>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeLeaveAdjustment 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeLeaveAdjustmentDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeeLeaveAdjustment>()
                });
            }
        }
        #endregion

        #region Get Attendance Leave Details 
        [HttpGet]
        [Route("GetAttendanceLeaveDetailsByEmployeeId")]
        public async Task<IActionResult> GetAttendanceLeaveDetailsByEmployeeId(int employeeId, int departmentId)
        {
            AttendanceDaysAndHoursDetailsView leaveDetails = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAttendanceLeaveDetailsByEmployeeId") + employeeId + "&departmentId=" + departmentId);
                leaveDetails = JsonConvert.DeserializeObject<AttendanceDaysAndHoursDetailsView>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = leaveDetails 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetAttendanceLeaveDetailsByEmployeeId", JsonConvert.SerializeObject(employeeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = leaveDetails 
            });
        }
        #endregion

        #region List All Leave Rejection Reason
        [HttpGet]
        [Route("GetLeaveRejectionReason")]
        public async Task<IActionResult> GetLeaveRejectionReason()
        {
            List<LeaveRejectionReason> leaveRejectionReasons = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveRejectionReason"));
                leaveRejectionReasons = JsonConvert.DeserializeObject<List<LeaveRejectionReason>>(JsonConvert.SerializeObject(result?.Data));
                if (leaveRejectionReasons?.Count > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        ListOfLeaveRejectionReasons = leaveRejectionReasons 
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetLeaveRejectionReason");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfLeaveRejectionReasons = leaveRejectionReasons 
            });
        }
        #endregion

        #region Get employee name by id
        [NonAction]
        public async Task<List<SupportingDocuments>> GetDocumentByLeaveId(List<int> lstLeaveId)
        {
            List<DocumentDetails> documentDetails = new();
            List<SupportingDocuments> documentDetail = new();
            try
            {
                using var documentClient = new HttpClient();
                SourceDocuments sourceDocuments = new()
                {
                    SourceId = lstLeaveId,
                    SourceType = new List<string> { _configuration.GetValue<string>("LeavesSourceType") }
                };
                documentClient.BaseAddress = new Uri(_notificationBaseURL);
                HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/GetDocumentBySourceIdAndType", sourceDocuments);
                if (documentResponse?.IsSuccessStatusCode == true)
                {
                    var documentResults = documentResponse?.Content?.ReadAsAsync<SuccessData>();
                    List<SupportingDocuments> lstOfSupDocument = JsonConvert.DeserializeObject<List<SupportingDocuments>>(JsonConvert.SerializeObject(documentResults?.Result?.Data));
                    if (lstOfSupDocument?.Count > 0)
                    {
                        documentDetail = lstOfSupDocument?.Where(x => x.DocumentName != null).
                            Select(x => new SupportingDocuments
                            {
                                SourceId = x.SourceId,
                                DocumentId = x.DocumentId,
                                DocumentName = x.DocumentName,
                                DocumentSize = x.DocumentSize,
                                DocumentCategory = x.DocumentCategory,
                                IsApproved = x.IsApproved,
                                DocumentType = x.DocumentType
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "SupportingDocuments/GetDocumentBySourceIdAndType", JsonConvert.SerializeObject(lstLeaveId));
            }
            return documentDetail;
        }
        #endregion

        #region Get Available Applied Leaves by Employee
        [HttpGet]
        [Route("GetAvailableLeaveDetailsByEmployee")]
        public async Task<IActionResult> GetAvailableLeaveDetailsByEmployee(int employeeID)
        {
            List<AvailableLeaveDetailsView> AvailableLeaveDetails = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAvailableLeaveDetailsByEmployee") + employeeID);
                AvailableLeaveDetails = JsonConvert.DeserializeObject<List<AvailableLeaveDetailsView>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = AvailableLeaveDetails 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetAvailableLeaveDetailsByEmployee", JsonConvert.SerializeObject(employeeID));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = AvailableLeaveDetails 
            });
        }
        #endregion

        #region Accrued Employee Leaves
        [HttpGet]
        [AllowAnonymous]
        [Route("AccruedEmployeeLeaves")]
        public async Task<IActionResult> AccruedEmployeeLeaves(DateTime? executedate = null)
        {


            List<EmployeeDetailsForLeaveView> EmployeeDetails = new();
            tempparameter temp = new();
            temp.executedate = executedate == null ? DateTime.Now.Date : executedate;


            try
            {

                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeforLeave"));
                //EmployeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetailsForLeaveView>>(JsonConvert.SerializeObject(result?.Data));
                temp.EmployeeDetail = JsonConvert.DeserializeObject<List<EmployeeDetailsForLeaveView>>(JsonConvert.SerializeObject(result?.Data));

                //var results = await _client.PostAsJsonAsync(EmployeeDetails, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:AccruedEmployeeLeaves"));
                var results = await _client.PostAsJsonAsync(temp, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:AccruedEmployeeLeaves"));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = "SUCCESS"
                });
            }
            catch (Exception ex)
            {

                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/AccruedEmployeeLeaves");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = "FAILURE"
            });
        }
        #endregion
        #region Accrued Employee Leaves
        [HttpGet]
        [AllowAnonymous]
        [Route("CarryForwardLeaves")]
        public async Task<IActionResult> CarryForwardLeaves(DateTime? executedate = null)
        {
            List<EmployeeDetailsForLeaveView> EmployeeDetails = new();
            tempparameter temp = new();
            temp.executedate = executedate == null ? DateTime.Now.Date : executedate;
            try
            {
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeforLeave"));
                temp.EmployeeDetail = JsonConvert.DeserializeObject<List<EmployeeDetailsForLeaveView>>(JsonConvert.SerializeObject(result?.Data));
                var results = await _client.PostAsJsonAsync(temp, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:CarryForwardLeaves"));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = "SUCCESS"
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/CarryForwardLeaves");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = "FAILURE"
            });
        }
        #endregion

        #region Get Team Leave and Rejection
        [HttpPost]
        [Route("GetTeamLeaveAndRejection")]
        public async Task<IActionResult> GetTeamLeaveAndRejection(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            TeamLeaveAndRejectionListView teamLeaveAndRejectionView = new();
            List<EmployeeName> lstEmployeeName = new();
            List<SupportingDocuments> ListOfDocument = new();
            List<TeamLeaveView> teamRegularisationList = new();
            try

            {
                //var resourceTeamId = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetManagerIdList") + employeeId);
                //List<EmployeeList> employeeLists = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(resourceTeamId.Result.Data));
                //ReportingManagerTeamLeaveView managerTeamLeaveView = new();
                //managerTeamLeaveView.FromDate = FromDate;
                //managerTeamLeaveView.ToDate = ToDate;
                //managerTeamLeaveView.ManagerEmployeeId = employeeId;
                //managerTeamLeaveView.ManagerId = employeeId;
                // List<int> approverManagerId = employeeLists?.Select(x => (int)x.ManagerId).Distinct().ToList();
                //managerTeamLeaveView.ResourceId = approverManagerId?.Count > 0? approverManagerId : new List<int>();
                var result = await _client.PostAsJsonAsync(managerTeamLeaveView, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetTeamLeaveAndRejection"));
                teamLeaveAndRejectionView = JsonConvert.DeserializeObject<TeamLeaveAndRejectionListView>(JsonConvert.SerializeObject(result?.Data));

                var results = await _client.PostAsJsonAsync(managerTeamLeaveView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeRegularizationList"));
                teamRegularisationList = JsonConvert.DeserializeObject<List<TeamLeaveView>>(JsonConvert.SerializeObject(results?.Data));

                teamLeaveAndRejectionView.TeamLeaveView = teamLeaveAndRejectionView?.TeamLeaveView?.Concat(teamRegularisationList).OrderByDescending(x => x.CreatedOn).Skip(managerTeamLeaveView.NoOfRecord * (managerTeamLeaveView.PageNumber)).Take(managerTeamLeaveView.NoOfRecord).ToList();

                // teamLeaveAndRejectionView?.TeamLeaveView?.ForEach(x => x.ManagerId = (employeeLists?.Where(y => y.EmployeeId == x.EmployeeId).Select(y => y.ManagerId).FirstOrDefault()));

                lstEmployeeName = GetEmployeeNameById(teamLeaveAndRejectionView?.TeamLeaveView.Select(x => x.EmployeeId).ToList()).Result;
                teamLeaveAndRejectionView?.TeamLeaveView?.ForEach(x => x.EmployeeName = lstEmployeeName?.Where(y => y.EmployeeId == x.EmployeeId).Select(y => (y.EmployeeFullName)).FirstOrDefault());
                teamLeaveAndRejectionView?.TeamLeaveView?.ForEach(x => x.EmployeeFormattedId = lstEmployeeName?.Where(y => y.EmployeeId == x.EmployeeId).Select(y => (y.FormattedEmployeeId)).FirstOrDefault());
                List<EmployeeName> lstManagerName = GetEmployeeNameById(teamLeaveAndRejectionView?.TeamLeaveView.Where(x => x.ManagerId !=null).Select(x => (int)x.ManagerId).ToList()).Result;
                teamLeaveAndRejectionView?.TeamLeaveView?.ForEach(x => x.ManagerName = lstManagerName?.Where(y => y.EmployeeId == x.ManagerId).Select(y => (y.EmployeeFullName)).FirstOrDefault());
                teamLeaveAndRejectionView?.TeamLeaveView?.ForEach(x => x.ManagerFormattedId = lstManagerName?.Where(y => y.EmployeeId == x.ManagerId).Select(y => (y.FormattedEmployeeId)).FirstOrDefault());

                ListOfDocument = GetDocumentByLeaveId(teamLeaveAndRejectionView?.TeamLeaveView?.Select(x => x.LeaveId).ToList()).Result;
                foreach (TeamLeaveView item in teamLeaveAndRejectionView?.TeamLeaveView)
                {
                    if (item.IsGrantLeaveRequest != true)
                    {
                        item.ListOfDocuments = ListOfDocument?.Where(y => y.SourceId == item.LeaveId)
                .Select(x => new SupportingDocuments
                {
                    DocumentId = x.DocumentId,
                    DocumentName = x.DocumentName,
                    DocumentSize = x.DocumentSize,
                    DocumentCategory = x.DocumentCategory,
                    IsApproved = x.IsApproved,
                    DocumentType = x.DocumentType
                }).ToList();
                    }
                }
                //teamLeaveAndRejectionView.TeamLeaveView.ForEach(x => x.ListOfDocuments = ListOfDocument.Where(y => y.SourceId == x.LeaveId)
                //.Select(x => new SupportingDocuments
                //{
                //    DocumentId = x.DocumentId,
                //    DocumentName = x.DocumentName,
                //    DocumentSize = x.DocumentSize,
                //    DocumentCategory = x.DocumentCategory,
                //    IsApproved = x.IsApproved,
                //    DocumentType = x.DocumentType
                //}).ToList());
                teamLeaveAndRejectionView.TeamLeaveView = teamLeaveAndRejectionView.TeamLeaveView.OrderByDescending(x => x.CreatedOn).ToList();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    teamLeaveAndRejectionView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetTeamLeave");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    teamLeaveAndRejectionView
                });
            }
        }
        #endregion

        #region Get Available Leave and Duration details By Employee Id
        [HttpGet]
        [Route("GetAvailableLeaveAndDurationDetailsByEmployeeId")]
        public async Task<IActionResult> GetAvailableLeaveAndDurationDetailsByEmployeeId(int employeeId)
        {
            AvailableLeaveAndDurationDetailsView AvailableLeaveAndDurationDetails = new();
            try
            {
                var departmentResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDepartmentIdByEmployeeId") + employeeId);
                int departmentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(departmentResult?.Data));
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAvailableLeaveAndDurationDetailsByEmployeeId") + employeeId + "&departmentId=" + departmentId);
                AvailableLeaveAndDurationDetails = JsonConvert.DeserializeObject<AvailableLeaveAndDurationDetailsView>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = AvailableLeaveAndDurationDetails 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetAvailableLeaveAndDurationDetailsByEmployeeId", JsonConvert.SerializeObject(employeeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = AvailableLeaveAndDurationDetails 
            });
        }
        #endregion

        #region Get Employee Leave and Restriction Details By Employee Id
        [HttpPost]
        [Route("GetEmployeeLeaveAndRestrictionDetails")]
        public async Task<IActionResult> GetEmployeeLeaveAndRestrictionDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;

                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeLeaveAndRestrictionDetails"));
                individualLeaveList = JsonConvert.DeserializeObject<IndividualLeaveList>(JsonConvert.SerializeObject(result?.Data));


                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = individualLeaveList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeLeaveAndRestrictionDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = individualLeaveList
            });
        }
        #endregion
        #region Get Employee applied leave details
        [HttpPost]
        [Route("GetEmployeeAppliedLeaveDetails")]
        public async Task<IActionResult> GetEmployeeAppliedLeaveDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            //Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;
                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeAppliedLeaveDetails"));
                individualLeaveList = JsonConvert.DeserializeObject<IndividualLeaveList>(JsonConvert.SerializeObject(result?.Data));
                
                EmployeeNameDepartmentLocation ApproverDetails = new();
                ApproverDetails.EmployeeId = employeeId;
                List<int> employeeIdList = new List<int>();
                foreach (ApplyLeavesView item in individualLeaveList?.AppliedLeaveList)
                {
                    if (item?.GrantLeaveApprovalStatus?.Count > 0)
                    {
                        employeeIdList = employeeIdList?.Concat(item?.GrantLeaveApprovalStatus?.Select(x => x.LevelApprovalEmployeeId == null ? 0 : (int)x.LevelApprovalEmployeeId).ToList()).ToList();
                    }
                }
                ApproverDetails.EmployeeList = employeeIdList;
                var employeeResult = await _client.PostAsJsonAsync(ApproverDetails, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:LeaveEmployeeDetails"));
                EmployeeNameDepartmentAndLocationView employeeNameDepartments = JsonConvert.DeserializeObject<EmployeeNameDepartmentAndLocationView>(JsonConvert.SerializeObject(employeeResult?.Data));

                //Update approver Name
                foreach (ApplyLeavesView item in individualLeaveList?.AppliedLeaveList)
                {
                    if (item?.GrantLeaveApprovalStatus?.Count > 0)
                    {
                        item.GrantLeaveApprovalStatus?.ForEach(x => x.LevelApproverName = employeeNameDepartments?.EmployeeName?.Where(y => y.EmployeeId == x.LevelApprovalEmployeeId).Select(z => z.EmployeeFullName).FirstOrDefault());
                    }
                }
                
                List<ApplyLeavesView> teamRegularisationList = new();
                var regularisationResults = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeRegularizationById") + employeeId + "&FromDate=" + employeeLeaveandRestriction.FromDate.ToString("yyyy-MM-dd") + "&ToDate=" + employeeLeaveandRestriction.ToDate.ToString("yyyy-MM-dd"));
                teamRegularisationList = JsonConvert.DeserializeObject<List<ApplyLeavesView>>(JsonConvert.SerializeObject(regularisationResults?.Data));
                List<ApplyLeavesView> appliedLeaveList = (individualLeaveList?.AppliedLeaveList?.Concat(teamRegularisationList)).ToList();
                individualLeaveList.AppliedLeaveList = appliedLeaveList?.OrderByDescending(x => x.CreatedOn).Select(x => x).ToList();
                if (employeeLeaveandRestriction != null)
                {
                    individualLeaveList.employeePersonalDetails = new EmployeePersonalDetails
                    {
                        EmployeeID = employeeLeaveandRestriction.EmployeeId,
                        DateOfJoining = employeeLeaveandRestriction?.DateOfJoining,
                        BirthDate = employeeNameDepartments?.BirthDate,
                        WeddingAnniversary = employeeNameDepartments?.WeddingAnniversary
                    };
                }

                var employeeShiftResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeShiftDetailsById") + employeeId);
                List<EmployeeShiftDetailsView> employeeShiftDetails = JsonConvert.DeserializeObject<List<EmployeeShiftDetailsView>>(JsonConvert.SerializeObject(employeeShiftResult?.Data));

                var shiftList = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftDetailsList"));
                EmployeeShiftDetailsListView ShiftDetailList = JsonConvert.DeserializeObject<EmployeeShiftDetailsListView>(JsonConvert.SerializeObject(shiftList.Data));

                List<Holiday> financialHolidayList = new();
                if (individualLeaveList?.HolidayDetails?.HolidayList?.Count > 0)
                {
                    foreach (var item in individualLeaveList?.HolidayDetails?.HolidayList)
                    {
                        int? shiftId = 0;
                        if (employeeShiftDetails?.Count > 0)
                        {
                            foreach (EmployeeShiftDetailsView shift in employeeShiftDetails)
                            {
                                if (item.HolidayDate >= shift.ShiftFromDate && (shift.ShiftToDate == null || item.HolidayDate <= shift.ShiftToDate))
                                {
                                    shiftId = ShiftDetailList?.employeeShifts.Where(x => x.ShiftDetailsId == shift.ShiftDetailsId).Select(x => x.ShiftDetailsId).FirstOrDefault();
                                }
                            }
                        }
                        if (shiftId == 0)
                        {
                            shiftId = ShiftDetailList?.DefaultShiftView.ShiftDetailsId;
                        }
                        Holiday holiday = await getApplicableHolidayList(item, employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.LocationId, (int)shiftId, individualLeaveList?.HolidayDetails);
                        financialHolidayList.Add(holiday);
                    }
                    individualLeaveList.HolidayList = new List<Holiday>();
                    individualLeaveList.HolidayList = financialHolidayList.Where(x => x.HolidayName != null).Select(x => x).ToList();
                }
                if (ShiftDetailList != null)
                {
                    if (employeeShiftDetails?.Count > 0)
                    {
                        employeeShiftDetails?.ForEach(x => x.WeekendId = ShiftDetailList?.employeeShifts?.Where(y => y.ShiftDetailsId == x.ShiftDetailsId).Select(y => y.WeekendId).FirstOrDefault());
                    }
                    individualLeaveList.EmployeeShiftDetailsList = new EmployeeShiftDetailsListView();
                    if (ShiftDetailList.DefaultShiftView != null)
                    {
                        individualLeaveList.EmployeeShiftDetailsList.DefaultShiftView = new EmployeeShiftDetailsView
                        {
                            EmployeeID = ShiftDetailList.DefaultShiftView.EmployeeID,
                            ShiftDetailsId = ShiftDetailList.DefaultShiftView.ShiftDetailsId,
                            ShiftFromDate = ShiftDetailList.DefaultShiftView.ShiftFromDate,
                            ShiftToDate = ShiftDetailList.DefaultShiftView.ShiftToDate,
                            WeekendId = ShiftDetailList.DefaultShiftView.WeekendId
                        };
                    }
                    individualLeaveList.EmployeeShiftDetailsList.employeeShifts = new List<EmployeeShiftDetailsView>();
                    individualLeaveList.EmployeeShiftDetailsList.employeeShifts = individualLeaveList?.EmployeeShiftDetailsList?.employeeShifts?.Concat(employeeShiftDetails).ToList();
                }

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = individualLeaveList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeAppliedLeaveDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = individualLeaveList
            });
        }
        #endregion
        #region Get applied leave details
        [HttpPost]
        [Route("GetAppliedLeaveDetails")]
        public async Task<IActionResult> GetAppliedLeaveDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            //Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;
                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAppliedLeaveDetails"));
                individualLeaveList = JsonConvert.DeserializeObject<IndividualLeaveList>(JsonConvert.SerializeObject(result?.Data));

                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeShiftDetailsById") + employeeId);
                List<EmployeeShiftDetailsView> employeeShiftDetails = JsonConvert.DeserializeObject<List<EmployeeShiftDetailsView>>(JsonConvert.SerializeObject(employeeResult?.Data));

                var shiftList = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftDetailsList"));
                EmployeeShiftDetailsListView ShiftDetailList = JsonConvert.DeserializeObject<EmployeeShiftDetailsListView>(JsonConvert.SerializeObject(shiftList.Data));

                List<ApplyLeavesView> AbsentListView = new();
                List<Holiday> financialHolidayList = new();
                if (individualLeaveList?.HolidayDetails?.HolidayList?.Count > 0)
                {
                    foreach (var item in individualLeaveList?.HolidayDetails?.HolidayList)
                    {
                        int? shiftId = 0;
                        if (employeeShiftDetails?.Count > 0)
                        {
                            foreach (EmployeeShiftDetailsView shift in employeeShiftDetails)
                            {
                                if (item.HolidayDate >= shift.ShiftFromDate && (shift.ShiftToDate == null || item.HolidayDate <= shift.ShiftToDate))
                                {
                                    shiftId = ShiftDetailList?.employeeShifts.Where(x => x.ShiftDetailsId == shift.ShiftDetailsId).Select(x => x.ShiftDetailsId).FirstOrDefault();
                                }
                            }
                        }
                        if (shiftId == 0)
                        {
                            shiftId = ShiftDetailList?.DefaultShiftView.ShiftDetailsId;
                        }
                        Holiday holiday = await getApplicableHolidayList(item, employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.LocationId, (int)shiftId, individualLeaveList?.HolidayDetails);
                        financialHolidayList.Add(holiday);
                    }
                    individualLeaveList.HolidayList = new List<Holiday>();
                    individualLeaveList.HolidayList = financialHolidayList.Where(x => x.HolidayName != null).Select(x => x).ToList();
                }

                //AbsentListView = await GetAbsentList(employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeId, employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.LocationId, individualLeaveList.AppliedLeaveDetails, employeeLeaveandRestriction.DateOfJoining == null ? DateTime.MinValue.Date : (DateTime)employeeLeaveandRestriction.DateOfJoining, individualLeaveList.HolidayList);
                AbsentListView = await GetAbsentList(employeeLeaveandRestriction, individualLeaveList.AppliedLeaveDetails, individualLeaveList.HolidayList);
                //individualLeaveList.AppliedLeaveList = (individualLeaveList?.AppliedLeaveList?.Concat(AbsentListView)).ToList();
                individualLeaveList.AppliedLeaveList = new List<ApplyLeavesView>();
                individualLeaveList.AppliedLeaveList = AbsentListView?.OrderByDescending(x => x.CreatedOn).Select(x => x).ToList(); 
                /*
                List<ApplyLeavesView> fullDayCount = new();
                //fullDayCount = AbsentListView?.Where(x => x.LeaveType == "Full Day Absent" || x.Lop == "Full Day").ToList();
                fullDayCount = AbsentListView?.Where(x => x.LeaveType == "Full Day Absent").ToList();
                foreach (var view in fullDayCount)
                {
                    view.NoOfDays = 1;
                }
                List<ApplyLeavesView> halfDayCount = new();
                //halfDayCount = AbsentListView?.Where(x => x.LeaveType == "Half Day Absent" || x.Lop == "Half Day").ToList();
                halfDayCount = AbsentListView?.Where(x => x.LeaveType == "Half Day Absent").ToList();
                foreach (var view in halfDayCount)
                {
                    view.NoOfDays = 0.5m;
                }
                decimal daysCount = fullDayCount.Sum(x => x.NoOfDays) + halfDayCount.Sum(x => x.NoOfDays);
                */
                decimal daysCount = (decimal)(AbsentListView?.Where(x => x.LeaveType == "Full Day Absent").Select(x => x.NoOfDays = 1).ToList().Sum(x => x)
                    + AbsentListView?.Where(x => x.LeaveType == "Half Day Absent").Select(x => x.NoOfDays = 0.5m).ToList().Sum(x => x));
                EmployeeAvailableLeaveDetails AbsentListViewas = new();
                List<DateTime> dlist = new List<DateTime>();
                dlist = AbsentListView?.Select(x => (DateTime)x.FromDate).ToList();

                AbsentListViewas = (from items in AbsentListView
                                    select new EmployeeAvailableLeaveDetails
                                    {
                                        EmployeeID = items.EmployeeId,
                                        LeaveType = "Absent",
                                        NoOfAbsentDays = daysCount,
                                        //NoOfAbsentDays = holidayView?.leaveBalance?.BalanceLeaves==null?0:  daysCount <= (holidayView?.leaveBalance?.BalanceLeaves) ? daysCount: holidayView?.leaveBalance?.BalanceLeaves,
                                        AbsentDatesList = dlist
                                    }).FirstOrDefault();

                if (AbsentListViewas != null)
                {
                    individualLeaveList.EmployeeAvailableLeaveDetails = new List<EmployeeAvailableLeaveDetails>();
                    individualLeaveList?.EmployeeAvailableLeaveDetails.Add(AbsentListViewas);
                }
                individualLeaveList.FromDate = employeeLeaveandRestriction?.FromDate;
                //individualLeaveList.AppliedLeaveList = individualLeaveList?.AppliedLeaveList?.OrderByDescending(x => x.CreatedOn).Select(x => x).ToList();

                //if (ShiftDetailList != null)
                //{
                //    if (employeeShiftDetails?.Count > 0)
                //    {
                //        employeeShiftDetails?.ForEach(x => x.WeekendId = ShiftDetailList?.employeeShifts?.Where(y => y.ShiftDetailsId == x.ShiftDetailsId).Select(y => y.WeekendId).FirstOrDefault());
                //    }
                //    individualLeaveList.EmployeeShiftDetailsList = new EmployeeShiftDetailsListView();
                //    if (ShiftDetailList.DefaultShiftView != null)
                //    {
                //        individualLeaveList.EmployeeShiftDetailsList.DefaultShiftView = new EmployeeShiftDetailsView
                //        {
                //            EmployeeID = ShiftDetailList.DefaultShiftView.EmployeeID,
                //            ShiftDetailsId = ShiftDetailList.DefaultShiftView.ShiftDetailsId,
                //            ShiftFromDate = ShiftDetailList.DefaultShiftView.ShiftFromDate,
                //            ShiftToDate = ShiftDetailList.DefaultShiftView.ShiftToDate,
                //            WeekendId = ShiftDetailList.DefaultShiftView.WeekendId
                //        };
                //    }
                //    individualLeaveList.EmployeeShiftDetailsList.employeeShifts = new List<EmployeeShiftDetailsView>();
                //    individualLeaveList.EmployeeShiftDetailsList.employeeShifts = individualLeaveList?.EmployeeShiftDetailsList?.employeeShifts?.Concat(employeeShiftDetails).ToList();
                //}
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = individualLeaveList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeAppliedLeaveDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = individualLeaveList
            });
        }
        #endregion
        [NonAction]
        //public async Task<List<ApplyLeavesView>> GetAbsentList(DateTime FromDate, DateTime ToDate, int employeeId, int departmentId, int LocationId, List<AppliedLeaveTypeDetails> appliedLeaveDetails, DateTime DOJ, List<Holiday> holidayList)
        public async Task<List<ApplyLeavesView>> GetAbsentList(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction, List<AppliedLeaveTypeDetails> appliedLeaveDetails,  List<Holiday> holidayList)
        {
            DateTime FromDate = employeeLeaveandRestriction.FromDate; 
            DateTime ToDate = employeeLeaveandRestriction.ToDate; 
            int employeeId = employeeLeaveandRestriction.EmployeeId; 
            int departmentId = employeeLeaveandRestriction.DepartmentId; 
            int LocationId = employeeLeaveandRestriction.LocationId;
            DateTime DOJ = employeeLeaveandRestriction.DateOfJoining == null ? DateTime.MinValue.Date : (DateTime)employeeLeaveandRestriction.DateOfJoining;
            
            List<ApplyLeavesView> AbsentListView = new();
            try
            {
                EmployeeDepartmentAndLocationView employeeDepartments = new();
                employeeDepartments.fromDate = FromDate;
                employeeDepartments.toDate = ToDate;
                employeeDepartments.employeeId = employeeId;
                employeeDepartments.DepartmentId = departmentId;
                employeeDepartments.LocationId = LocationId;
                employeeDepartments.DOJ = DOJ;
                employeeDepartments.PageNumber = employeeLeaveandRestriction.PageNumber;
                employeeDepartments.NoOfRecord = employeeLeaveandRestriction.NoOfRecord;
                employeeDepartments.TotalCount = employeeLeaveandRestriction.TotalCount;
                var results = await _client.PostAsJsonAsync(employeeDepartments, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeAbsentList"));
                List<ApplyLeavesView> absentListViews = JsonConvert.DeserializeObject<List<ApplyLeavesView>>(JsonConvert.SerializeObject(results?.Data));
                if (absentListViews?.Count > 0)
                {
                    List<ApplyLeavesView> absentDates = new();
                    List<DateTime> leaveDate = appliedLeaveDetails?.Where(x => x.IsFullDay).Select(x => x.Date).ToList();
                    var absent = new List<ApplyLeavesView>();
                    if (leaveDate?.Count > 0)
                    {
                        absent = absentListViews?.Where(m => !leaveDate.Contains((DateTime)m.FromDate))?.ToList();
                    }
                    else { absent = absentListViews; }
                    List<DateTime> holidayDate = holidayList?.Select(x => (DateTime)x.HolidayDate)?.ToList();
                    holidayDate = holidayDate == null ? new List<DateTime>() : holidayDate;
                    absentDates = absent?.Where(m => !holidayDate.Contains((DateTime)m.FromDate))?.ToList();
                    //List<string> weekendDays = WeekendShiftList?.WeekendList?.Select(x => x.WeekendDayName).ToList();
                    //weekendDays = weekendDays == null ? new List<string>() : weekendDays;
                    //absentDates = absentDate?.Where(m => !weekendDays.Contains(m.FromDate?.DayOfWeek.ToString())).ToList();
                    absentDates = absentDates == null ? new List<ApplyLeavesView>() : absentDates;
                    foreach (var item in absentDates)
                    //ApplyLeavesView item = new();
                    //item = absentDates.Where(x => x.FromDate == new DateTime(2021, 11, 09)).Select(x => x).FirstOrDefault();
                    //if (item.FromDate == new DateTime(2021, 11, 09))
                    {
                        var shiftResults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetShifByDate") + employeeId + "&date=" + item.FromDate);
                        EmployeeShiftDetails employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDetails>(JsonConvert.SerializeObject(shiftResults?.Data));
                        int shiftId = (employeeShift?.ShiftDetailsId == null ? 0 : (int)employeeShift?.ShiftDetailsId);
                        var shiftResult = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetails") + shiftId);
                        ShiftDetailedView WeekendList = JsonConvert.DeserializeObject<ShiftDetailedView>(JsonConvert.SerializeObject(shiftResult?.Data));
                        ShiftViewDetails WeekendShiftList = new();
                        WeekendShiftList = WeekendList?.shiftViewDetails;
                        List<string> weekendDays = WeekendShiftList?.WeekendList?.Select(x => x.WeekendDayName).ToList();
                        weekendDays = weekendDays == null ? new List<string>() : weekendDays;
                        if (weekendDays.Contains(item.FromDate.Value.DayOfWeek.ToString()))
                        {
                            continue;
                        }
                        ApplyLeavesView absentView = new();
                        ApplyLeavesView attendanceMarkedDay = absentListViews?.Where(x => x.FromDate == item.FromDate).Select(x => x).FirstOrDefault();
                        string[] hour = item?.TotalHours?.Split(":");
                        TimeSpan? totalHours = new TimeSpan(hour?.Length > 0 ? Convert.ToInt32(hour[0]) : 0, hour?.Length > 1 ? Convert.ToInt32(hour[1]) : 0, hour?.Length > 2 ? Convert.ToInt32(hour[2]) : 0);
                        string[] AbsentFromHour = WeekendShiftList?.AbsentFromHour?.Split(":");
                        TimeSpan? absentFromHour = new TimeSpan(AbsentFromHour?.Length > 0 ? Convert.ToInt32(AbsentFromHour[0]) : 0, AbsentFromHour?.Length > 1 ? Convert.ToInt32(AbsentFromHour[1]) : 0, AbsentFromHour?.Length > 2 ? Convert.ToInt32(AbsentFromHour[2]) : 0);
                        string[] AbsentToHour = WeekendShiftList?.AbsentToHour?.Split(":");
                        TimeSpan? absentToHour = new TimeSpan(AbsentToHour?.Length > 0 ? Convert.ToInt32(AbsentToHour[0]) : 0, AbsentToHour?.Length > 1 ? Convert.ToInt32(AbsentToHour[1]) : 0, AbsentToHour?.Length > 2 ? Convert.ToInt32(AbsentToHour[2]) : 0);
                        string[] HalfaDayFromHour = WeekendShiftList?.HalfaDayFromHour?.Split(":");
                        TimeSpan? halfaDayFromHour = new TimeSpan(HalfaDayFromHour?.Length > 0 ? Convert.ToInt32(HalfaDayFromHour[0]) : 0, HalfaDayFromHour?.Length > 1 ? Convert.ToInt32(HalfaDayFromHour[1]) : 0, HalfaDayFromHour?.Length > 2 ? Convert.ToInt32(HalfaDayFromHour[2]) : 0);
                        string[] HalfaDayToHour = WeekendShiftList?.HalfaDayToHour?.Split(":");
                        TimeSpan? halfaDayToHour = new TimeSpan(HalfaDayToHour?.Length > 0 ? Convert.ToInt32(HalfaDayToHour[0]) : 0, HalfaDayToHour?.Length > 1 ? Convert.ToInt32(HalfaDayToHour[1]) : 0, HalfaDayToHour?.Length > 2 ? Convert.ToInt32(HalfaDayToHour[2]) : 0);
                        string[] PresentHour = WeekendShiftList?.PresentHour?.Split(":");
                        TimeSpan? presentHour = new TimeSpan(PresentHour?.Length > 0 ? Convert.ToInt32(PresentHour[0]) : 0, PresentHour?.Length > 1 ? Convert.ToInt32(PresentHour[1]) : 0, PresentHour?.Length > 2 ? Convert.ToInt32(PresentHour[2]) : 0);
                        string[] fromHour = WeekendShiftList?.TimeFrom?.Split(":");
                        TimeSpan? timeFrom = new TimeSpan(fromHour?.Length > 0 ? Convert.ToInt32(fromHour[0]) : 0, fromHour?.Length > 1 ? Convert.ToInt32(fromHour[1]) : 0, fromHour?.Length > 2 ? Convert.ToInt32(fromHour[2]) : 0);
                        string[] toHour = WeekendShiftList?.TimeTo?.Split(":");
                        TimeSpan? timeTo = new TimeSpan(toHour?.Length > 0 ? Convert.ToInt32(toHour[0]) : 0, toHour?.Length > 1 ? Convert.ToInt32(toHour[1]) : 0, toHour?.Length > 2 ? Convert.ToInt32(toHour[2]) : 0);
                        TimeSpan? duration = new TimeSpan(0, 0, 0);
                        if (timeTo.Value.Ticks < timeFrom.Value.Ticks)
                        {
                            DateTime dt = new DateTime(timeTo.Value.Ticks).AddDays(1);
                            DateTime df = new DateTime(timeFrom.Value.Ticks);
                            if (dt >= df)
                            {
                                duration = (dt - df) / 2;
                            }
                        }
                        if (timeTo.Value.Ticks >= timeFrom.Value.Ticks)
                        {
                            duration = (timeTo - timeFrom) / 2;
                        }
                        TimeSpan? cutOffHour = (duration + timeFrom);
                        string[] shifthour = WeekendShiftList?.TotalHours?.Split(":");
                        TimeSpan? shiftHours = new TimeSpan(shifthour?.Length > 0 ? Convert.ToInt32(shifthour[0]) : 0, shifthour?.Length > 1 ? Convert.ToInt32(shifthour[1]) : 0, shifthour?.Length > 2 ? Convert.ToInt32(shifthour[2]) : 0);
                        absentView = attendanceMarkedDay;
                        int i = 0;
                        TimeSpan? CurrentTime = new TimeSpan(0, 0, 0);
                        if (item?.WeeklyMonthlyAttendanceDetail?.Count > 0)
                        {
                            DateTime checkinTime = item.WeeklyMonthlyAttendanceDetail[i].CheckinTime.Value;
                            CurrentTime = checkinTime.TimeOfDay;
                        }
                        bool IsHalfdayLeave = default;
                        bool IsHalfDayAbsent = default;
                        if (WeekendShiftList?.IsConsiderAbsent == true || WeekendShiftList?.IsConsiderHalfaDay == true || WeekendShiftList?.IsConsiderPresent == true)
                        {
                            //Full Day Absent
                            if (WeekendShiftList?.IsConsiderAbsent == true)
                            {
                                if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                                {
                                    if (totalHours > absentFromHour && totalHours > absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                                {
                                    if (totalHours > absentFromHour && totalHours < absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                                {
                                    if (totalHours > absentFromHour && totalHours >= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                                {
                                    if (totalHours > absentFromHour && totalHours <= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                                {
                                    if (totalHours > absentFromHour && totalHours == absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                                {
                                    if (totalHours < absentFromHour && totalHours > absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                                {
                                    if (totalHours < absentFromHour && totalHours < absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                                {
                                    if (totalHours < absentFromHour && totalHours <= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                                {
                                    if (totalHours < absentFromHour && totalHours >= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                                {
                                    if (totalHours < absentFromHour && totalHours == absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                                {
                                    if (totalHours >= absentFromHour && totalHours > absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                                {
                                    if (totalHours >= absentFromHour && totalHours < absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                                {
                                    if (totalHours >= absentFromHour && totalHours >= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                                {
                                    if (totalHours >= absentFromHour && totalHours >= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                                {
                                    if (totalHours >= absentFromHour && totalHours == absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                                {
                                    if (totalHours <= absentFromHour && totalHours > absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                                {
                                    if (totalHours <= absentFromHour && totalHours < absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                                {
                                    if (totalHours <= absentFromHour && totalHours >= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                                {
                                    if (totalHours <= absentFromHour && totalHours <= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                                {
                                    if (totalHours <= absentFromHour && totalHours <= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                                {
                                    if (totalHours == absentFromHour && totalHours > absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                                {
                                    if (totalHours == absentFromHour && totalHours < absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                                {
                                    if (totalHours == absentFromHour && totalHours >= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                                {
                                    if (totalHours == absentFromHour && totalHours <= absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                                else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                                {
                                    if (totalHours == absentFromHour && totalHours == absentToHour)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                }
                            }
                            //Half Day Absent
                            if (WeekendShiftList?.IsConsiderHalfaDay == true)
                            {
                                List<AppliedLeaveTypeDetails> Appliedleaves = Appliedleaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate).Select(x => x).ToList();
                                if (Appliedleaves != null)
                                {
                                    List<AppliedLeaveTypeDetails> Leavelist = new();
                                    foreach (var leave in Appliedleaves)
                                    {
                                        AppliedLeaveTypeDetails halfleave = new();
                                        if (leave.IsFirstHalf == true) { halfleave.IsFirstHalf = true; halfleave.IsSecondHalf = false; }
                                        else if (leave.IsSecondHalf == true) { halfleave.IsSecondHalf = true; halfleave.IsFirstHalf = false; }
                                        else { halfleave.IsFirstHalf = false; halfleave.IsSecondHalf = false; }
                                        Leavelist.Add(halfleave);
                                    }
                                    bool Leavelists = Leavelist?.Where(x => (bool)x?.IsFirstHalf || (bool)x?.IsSecondHalf).Select(x => x)?.Count() >= 2 ? true : false;
                                    if (Leavelists) { absentView.LeaveType = ""; }
                                    //}
                                    else
                                    {
                                        if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                                        {
                                            //if (totalHours > halfaDayFromHour && totalHours > halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (totalHours == null && leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (totalHours == null && leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours > halfaDayFromHour && totalHours > halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                                //}
                                            }
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                                        {
                                            //if (totalHours > halfaDayFromHour && totalHours < halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours > halfaDayFromHour && totalHours < halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                                        {
                                            //if (totalHours > halfaDayFromHour && totalHours >= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours > halfaDayFromHour && totalHours >= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                                        {
                                            //if (totalHours > halfaDayFromHour && totalHours <= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours > halfaDayFromHour && totalHours <= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                                        {
                                            //if (totalHours > halfaDayFromHour && totalHours == halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours > halfaDayFromHour && totalHours == halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                                        {
                                            //if (totalHours < halfaDayFromHour && totalHours > halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours < halfaDayFromHour && totalHours > halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                                        {
                                            //if (totalHours < halfaDayFromHour && totalHours < halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours < halfaDayFromHour && totalHours < halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                                        {
                                            //if (totalHours < halfaDayFromHour && totalHours <= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours < halfaDayFromHour && totalHours <= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                                        {
                                            //if (totalHours < halfaDayFromHour && totalHours >= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours < halfaDayFromHour && totalHours >= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                                        {
                                            //if (totalHours < halfaDayFromHour && totalHours == halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours < halfaDayFromHour && totalHours == halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                                        {
                                            //if (totalHours >= halfaDayFromHour && totalHours > halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours >= halfaDayFromHour && totalHours > halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                                        {
                                            //if (totalHours >= halfaDayFromHour && totalHours < halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours >= halfaDayFromHour && totalHours < halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                                        {
                                            //if (totalHours >= halfaDayFromHour && totalHours >= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours >= halfaDayFromHour && totalHours >= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                                        {
                                            //if (totalHours >= halfaDayFromHour && totalHours <= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours >= halfaDayFromHour && totalHours <= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                                        {
                                            //if (totalHours >= halfaDayFromHour && totalHours == halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours >= halfaDayFromHour && totalHours == halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                                        {
                                            //if (totalHours <= halfaDayFromHour && totalHours > halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours <= halfaDayFromHour && totalHours > halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                                        {
                                            //if (totalHours <= halfaDayFromHour && totalHours < halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours <= halfaDayFromHour && totalHours < halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                                        {
                                            //if (totalHours <= halfaDayFromHour && totalHours >= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours <= halfaDayFromHour && totalHours >= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                                        {
                                            //if (totalHours <= halfaDayFromHour && totalHours <= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours <= halfaDayFromHour && totalHours <= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                                        {
                                            //if (totalHours <= halfaDayFromHour && totalHours == halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours <= halfaDayFromHour && totalHours == halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                                        {
                                            //if (totalHours == halfaDayFromHour && totalHours > halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours == halfaDayFromHour && totalHours > halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                                        {
                                            //if (totalHours == halfaDayFromHour && totalHours < halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours == halfaDayFromHour && totalHours < halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                                        {
                                            //if (totalHours == halfaDayFromHour && totalHours >= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours == halfaDayFromHour && totalHours >= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                                        {
                                            //if (totalHours == halfaDayFromHour && totalHours <= halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours == halfaDayFromHour && totalHours <= halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                                        {
                                            //if (totalHours == halfaDayFromHour && totalHours == halfaDayToHour)
                                            //{
                                            AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                            if (leaves != null && leaves?.LeaveId > 0)
                                            {
                                                if (leaves.IsFirstHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                                else if (leaves.IsSecondHalf)
                                                {
                                                    //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                    absentView.LeaveType = "Half Day Absent";
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfdayLeave = true;
                                                }
                                            }
                                            //else
                                            //{
                                            if (totalHours == halfaDayFromHour && totalHours == halfaDayToHour)
                                            {
                                                //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                                absentView.LeaveType = "Half Day Absent";
                                                if (CurrentTime != null && CurrentTime < cutOffHour)
                                                {
                                                    absentView.IsFirstHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                else
                                                {
                                                    absentView.IsSecondHalfAbsent = true;
                                                    IsHalfDayAbsent = true;
                                                }
                                                if (IsHalfdayLeave && IsHalfDayAbsent)
                                                {
                                                    absentView.LeaveType = "";
                                                }
                                            }
                                            //}
                                            //}
                                        }
                                        // AbsentListView.Add(absentView);
                                    }
                               }
                            }
                            if (totalHours.Value.Ticks == 0)
                            {
                                List<AppliedLeaveTypeDetails> Appliedleaves = Appliedleaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate).Select(x => x).ToList();
                                if (Appliedleaves != null && Appliedleaves?.Count() > 0)
                                {
                                    List<AppliedLeaveTypeDetails> Leavelist = new();
                                    foreach (var leave in Appliedleaves)
                                    {
                                        AppliedLeaveTypeDetails halfleave = new();
                                        if (leave.IsFirstHalf == true) { halfleave.IsFirstHalf = true; halfleave.IsSecondHalf = false; }
                                        else if (leave.IsSecondHalf == true) { halfleave.IsSecondHalf = true; halfleave.IsFirstHalf = false; }
                                        else { halfleave.IsFirstHalf = false; halfleave.IsSecondHalf = false; }
                                        Leavelist.Add(halfleave);
                                    }
                                    bool Leavelists = Leavelist?.Where(x => (bool)x?.IsFirstHalf || (bool)x?.IsSecondHalf).Select(x => x)?.Count() >= 2 ? true : false;
                                    if (Leavelists) { absentView.LeaveType = ""; }
                                }
                                else
                                {
                                    AppliedLeaveTypeDetails leaves = leaves = appliedLeaveDetails?.Where(x => x.Date == item.FromDate && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).FirstOrDefault();
                                    if (leaves != null && leaves?.LeaveId > 0)
                                    {
                                        if (leaves.IsFirstHalf)
                                        {
                                            //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                            //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                            absentView.LeaveType = "Half Day Absent";
                                            absentView.IsSecondHalfAbsent = true;
                                            IsHalfdayLeave = true;
                                        }
                                        else if (leaves.IsSecondHalf)
                                        {
                                            //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                            //absentView.Lop = absentView.LeaveType == "LOP" ? "Half Day" : "";
                                            absentView.LeaveType = "Half Day Absent";
                                            absentView.IsFirstHalfAbsent = true;
                                            IsHalfdayLeave = true;
                                        }
                                    }
                                    if (totalHours.Value.Ticks == 0)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Full Day Absent";
                                    }
                                    if (totalHours.Value.Ticks == 0 && IsHalfdayLeave)
                                    {
                                        //absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Half Day Absent" : "LOP");
                                        //absentView.Lop = absentView.LeaveType == "LOP" ? "Full Day" : "";
                                        absentView.LeaveType = "Half Day Absent";
                                    }
                                    else
                                    {
                                        absentView = attendanceMarkedDay;
                                    }
                                }
                            }
                        }
                        //else
                        //{
                        //    string[] shifthour = WeekendShiftList?.TotalHours.Split(":");
                        //    TimeSpan? shiftHours = new TimeSpan(shifthour?.Length > 0 ? Convert.ToInt32(shifthour[0]) : 0, shifthour?.Length > 1 ? Convert.ToInt32(shifthour[1]) : 0, shifthour?.Length > 2 ? Convert.ToInt32(shifthour[2]) : 0);
                        //    if (totalHours.Value.Ticks < shiftHours.Value.Ticks)
                        //    {
                        //        absentView.LeaveType = (holidayView?.leaveBalance?.BalanceLeaves > 0 ? "Full Day Absent" : "LOP");
                        //    }
                        //    else
                        //    {
                        //        absentView = attendanceMarkedDay;
                        //    }
                        //}
                        AbsentListView.Add(absentView);
                    }
                    AbsentListView = AbsentListView?.Where(x => x.LeaveType != null && x.LeaveType != string.Empty).Select(x => x).ToList();
                }

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAbsentList");
            }
            return AbsentListView;
        }

        #region       
        [HttpPost]
        [Route("UpdateEmployeeLeaveBalance")]
        public async Task<IActionResult> UpdateEmployeeLeaveBalance(EmployeeLeaveBalanceUpdateView leaveBalanceUpdateView)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(leaveBalanceUpdateView, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:UpdateEmployeeLeaveBalance"));
                //int leave = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result.Data));
                return Ok(new
                {
                    StatusCode = result.StatusCode,
                    StatusText = result.StatusText,
                    data = result?.Data,
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/UpdateEmployeeLeaveBalance");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                });
            }
        }
        #endregion

        #region Get Appconstant By ID
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAppconstantByID")]
        public async Task<IActionResult> GetAppconstantByID(int appConstantID)
        {
            AppConstants appConstants = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAppconstantByID") + appConstantID);
                appConstants = JsonConvert.DeserializeObject<AppConstants>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = appConstants 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAppconstantByID");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = appConstants 
                });
            }
        }
        #endregion

        #region Get Holiday by Dept and Location and Shift and HolidayDate
        [HttpGet]
        [Route("GetHolidaybyDeptandLocandShifandDate")]
        public async Task<IActionResult> GetHolidaybyDeptandLocandShifandDate(int DepartmentId, DateTime FromDate, DateTime ToDate, int LocationId, int ShiftDetailsId)
        {
            List<Holiday> Holidaylist = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetHolidaybyDeptandLocandShifandDate") + DepartmentId + "&FromDate=" + FromDate.ToString("yyyy-MM-dd") + "&ToDate=" + ToDate.ToString("yyyy-MM-dd") + "&LocationId=" + LocationId + "&ShiftDetailsId=" + ShiftDetailsId);
                Holidaylist = JsonConvert.DeserializeObject<List<Holiday>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = Holidaylist 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetHolidaybyDeptandLocandShifandDate");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<Holiday>()
                });
            }
        }
        #endregion

        #region Get Employee Leaves list For Timesheet
        [HttpGet]
        [Route("GetEmployeeLeavesForTimesheet")]
        public async Task<IActionResult> GetEmployeeLeavesForTimesheet(int employeeID, DateTime fromDate, DateTime toDate)
        {
            List<EmployeeLeavesForTimeSheetView> Holidaylist = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeLeavesForTimesheet") + employeeID + "&FromDate=" + fromDate + "&ToDate=" + toDate);
                Holidaylist = JsonConvert.DeserializeObject<List<EmployeeLeavesForTimeSheetView>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = Holidaylist 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeLeavesForTimesheet");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeeLeavesForTimeSheetView>()
                });
            }
        }
        #endregion
        #region Update Leave Type Status 
        [HttpGet]
        [Route("UpdateLeaveTypeStatus")]
        public async Task<IActionResult> UpdateLeaveTypeStatus(int leaveTypeId, bool isEnabled, int updatedBy)
        {
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:UpdateLeaveTypeStatus") + leaveTypeId + "&isEnabled=" + isEnabled + "&updatedBy=" + updatedBy);
                bool isUpdate = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = isUpdate
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateLeaveTypeStatus");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion
        #region Update Holiday Status 
        [HttpGet]
        [Route("UpdateHolidayStatus")]
        public async Task<IActionResult> UpdateHolidayStatus(int holidayId, bool isEnabled, int updatedBy)
        {
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:UpdateHolidayStatus") + holidayId + "&isEnabled=" + isEnabled + "&updatedBy=" + updatedBy);
                bool isUpdate = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = isUpdate
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateHolidayStatus");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion

        #region Add or Update Leave Grant Leave
        [HttpPost]
        [Route("AddorUpdateLeaveGrant")]
        public async Task<IActionResult> AddorUpdateLeaveGrant(LeaveGrantRequestView leaveGrantRequestView)
        {
            int LeaveGrantDetailId = 0;
            string statusText = "";
            try
            {
                LeaveGrantRequestAndDocumentView leaveGrantRequestAndDocument = new LeaveGrantRequestAndDocumentView();
                leaveGrantRequestAndDocument.leaveGrantRequestDetail = leaveGrantRequestView?.leaveGrantRequestDetails;

                string SourceType = _configuration.GetValue<string>("LeavesSourceType");
                string BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                string hrDepartmentName = _configuration.GetValue<string>("GrantLeaveApproveHRDepartmentName");
                hrDepartmentName = hrDepartmentName == null ? "People Experience" : hrDepartmentName;
                string LeaveGrantDirectory = _configuration.GetValue<string>("GrantLeaveSourceType");
                string EmployeeId = leaveGrantRequestView?.leaveGrantRequestDetails?.EmployeeID.ToString();
                string LeaveTypeId = leaveGrantRequestView?.leaveGrantRequestDetails?.LeaveTypeId.ToString();
                string datetimenow = DateTime.Now.ToString("dd-MM-yyyy")+"-" +DateTime.Now.Hour.ToString()+ DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                List<LeaveGrantDocument> docList = new();
                if (leaveGrantRequestView?.LeaveGrantDocumentList?.Count > 0)
                {
                    if (!string.IsNullOrEmpty(BaseDirectory))
                    {
                        //Create Base Directory
                        if (!Directory.Exists(BaseDirectory))
                        {
                            Directory.CreateDirectory(BaseDirectory);
                        }
                        //Create Source Type Directory
                        if (!Directory.Exists(Path.Combine(BaseDirectory, SourceType)))
                        {
                            Directory.CreateDirectory(Path.Combine(BaseDirectory, SourceType));
                        }
                        //Create Leave Grant Document  Directory
                        if (!Directory.Exists(Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory)))
                        {
                            Directory.CreateDirectory(Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory));
                        }
                        //Create EmployeeID  Directory
                        if (!Directory.Exists(Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory, EmployeeId)))
                        {
                            Directory.CreateDirectory(Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory, EmployeeId));
                        }
                        if (!Directory.Exists(Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory, EmployeeId, LeaveTypeId)))
                        {
                            Directory.CreateDirectory(Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory, EmployeeId, LeaveTypeId));
                        }
                        //Create Date Directory
                        if (!Directory.Exists(Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory, EmployeeId, LeaveTypeId, datetimenow)))
                        {
                            Directory.CreateDirectory(Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory, EmployeeId, LeaveTypeId, datetimenow));
                        }
                    }

                    string directoryPath = Path.Combine(BaseDirectory, SourceType, LeaveGrantDirectory, EmployeeId, LeaveTypeId, datetimenow);
                    
                    foreach (var item in leaveGrantRequestView?.LeaveGrantDocumentList)
                    {
                        string documentPath = Path.Combine(directoryPath, item.DOC_NAME);
                        if (!System.IO.File.Exists(item.DOC_NAME))    //&& item.DocumentSize > 0
                        {
                            if (item.DocumentAsBase64.Contains(","))
                            {
                                item.DocumentAsBase64 = item.DocumentAsBase64.Substring(item.DocumentAsBase64.IndexOf(",") + 1);
                            }
                            item.DocumentAsByteArray = Convert.FromBase64String(item.DocumentAsBase64);
                            using (Stream fileStream = new FileStream(documentPath, FileMode.Create))
                            {
                                fileStream.Write(item.DocumentAsByteArray, 0, item.DocumentAsByteArray.Length);
                            }
                        }
                        string doc = Path.GetExtension(item.DOC_NAME);
                        string docType = doc.Substring(1);

                        LeaveGrantDocument leaveGrantDocument = new();
                        leaveGrantDocument.DocumentName = item.DOC_NAME;
                        leaveGrantDocument.DocumentPath = documentPath;
                        leaveGrantDocument.DocumentType = docType;
                        leaveGrantDocument.IsActive = true;
                        docList.Add(leaveGrantDocument);
                    }

                }
                leaveGrantRequestAndDocument.leaveGrantDocument = docList;
                var employeeResults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetGrantLeaveApprover") + leaveGrantRequestView.leaveGrantRequestDetails.EmployeeID + "&hrDepartmentName=" + hrDepartmentName);
                leaveGrantRequestAndDocument.GrantLeaveApprover = JsonConvert.DeserializeObject<GrantLeaveApproverView>(JsonConvert.SerializeObject(employeeResults?.Data));

                var result = await _client.PostAsJsonAsync(leaveGrantRequestAndDocument, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:AddorUpdateLeaveGrant"));
                LeaveGrantDetailId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (LeaveGrantDetailId > 0)
                {
                    if(leaveGrantRequestView?.leaveGrantRequestDetails?.Status?.ToLower() == "pending" || leaveGrantRequestView?.leaveGrantRequestDetails?.Status?.ToLower() == "")
                    { 
                    string fromdate = leaveGrantRequestView?.leaveGrantRequestDetails?.EffectiveFromDate?.ToString("dd MMM yyyy");
                    string MailSubject = null, MailBody = null, Subject = null, Body = null;
                    SendEmailView sendMailbyleaverequest = new();
                    // Notification
                    // Need to change
                    EmployeeandManagerView employeeandManager = new EmployeeandManagerView();
                    var employeeID = leaveGrantRequestView?.leaveGrantRequestDetails?.EmployeeID;
                        //var empresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeID);
                        //employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(empresults?.Data));
                        var empresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAndApproverDetails") + employeeID + "&approverId=" + LeaveGrantDetailId);
                        employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(empresults?.Data));

                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    List<Notifications> notifications = new();
                    Notifications notification = new();
                    Notifications empNotification = new();
                    notification = new()
                    {
                        CreatedBy = leaveGrantRequestView?.leaveGrantRequestDetails.EmployeeID == null ? 0 : (int)leaveGrantRequestView?.leaveGrantRequestDetails?.EmployeeID,
                        CreatedOn = DateTime.UtcNow,
                        FromId = leaveGrantRequestView?.leaveGrantRequestDetails.EmployeeID == null ? 0 : (int)leaveGrantRequestView?.leaveGrantRequestDetails?.EmployeeID,
                        ToId = employeeandManager?.ReportingManagerID == null ? 0 : (int)employeeandManager?.ReportingManagerID,
                        MarkAsRead = false,
                        NotificationSubject = "New GrantLeave request from " + employeeandManager?.EmployeeName + ".",
                        NotificationBody = employeeandManager?.EmployeeName + "'s " + leaveGrantRequestView?.leaveGrantRequestDetails?.LeaveName + " 1 - level request is waiting for your approval." + "Effective From Date : " + fromdate + ".",
                        PrimaryKeyId = leaveGrantRequestView?.leaveGrantRequestDetails?.LeaveGrantDetailId,
                        ButtonName = "Approve Grant Leave",
                        SourceType = "GrantLeaves",
                    };
                    string grantnotification = _commonFunction.Notification(notification).Result;
                    empNotification = new Notifications
                    {
                        CreatedBy = leaveGrantRequestView?.leaveGrantRequestDetails.EmployeeID == null ? 0 : (int)leaveGrantRequestView?.leaveGrantRequestDetails?.EmployeeID,
                        CreatedOn = DateTime.UtcNow,
                        FromId = leaveGrantRequestView?.leaveGrantRequestDetails.EmployeeID == null ? 0 : (int)leaveGrantRequestView?.leaveGrantRequestDetails?.EmployeeID,
                        ToId = leaveGrantRequestView?.leaveGrantRequestDetails.EmployeeID == null ? 0 : (int)leaveGrantRequestView?.leaveGrantRequestDetails?.EmployeeID,
                        MarkAsRead = false,
                        NotificationSubject = "GrantLeave sent for approval.",
                        NotificationBody = "Your " + leaveGrantRequestView?.leaveGrantRequestDetails?.LeaveName + " 1 - level request has been sent for approval." + "Effective From Date : " + fromdate + ".",
                        PrimaryKeyId = leaveGrantRequestView?.leaveGrantRequestDetails?.LeaveGrantDetailId,
                        ButtonName = "View Grant Leave",
                        SourceType = "GrantLeaves",
                    };
                    string grantEmpNotification = _commonFunction.Notification(empNotification).Result;

                    string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Type</b></td><td><b>Effective From Date</b></td><td><b>No.Of Days</b></td></tr>";
                    decimal NoOfDays;
                    decimal d = (decimal)leaveGrantRequestView?.leaveGrantRequestDetails?.NumberOfDay;
                    if ((d % 1) > 0)
                    {
                        //is decimal
                        NoOfDays = GetRoundOff((decimal)leaveGrantRequestView?.leaveGrantRequestDetails?.NumberOfDay);
                    }
                    else
                    {
                        //is int
                        NoOfDays = (decimal)leaveGrantRequestView?.leaveGrantRequestDetails?.NumberOfDay;
                    }
                    textBody += "<tr style='text-align:center';><td >" + leaveGrantRequestView?.leaveGrantRequestDetails?.LeaveName + "</td><td > " + fromdate + "</td><td >" + NoOfDays + "</td></tr></table>";
                    string baseURL = appsetting.GetSection("BaseURL").Value;
                    MailSubject = "{EmployeeName} sent grant leave request.";
                    MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {ManagerName},</p>                                    
                                    <p> {EmployeeName} requested for grant leave from {FromDate}. Please click <a href='{link}/#/pmsnexus/leaves/leave-team'>here</a> to Approve/Reject.</p>                                    
                                    <div>{table}</div>  
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>                                   
                                    </html>";
                    MailSubject = MailSubject.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                    MailBody = MailBody.Replace("{ManagerName}", employeeandManager?.ManagerName);
                    MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                    MailBody = MailBody.Replace("{FromDate}", fromdate);
                    MailBody = MailBody.Replace("{table}", textBody);
                    MailBody = MailBody.Replace("{Feedback}", leaveGrantRequestView?.leaveGrantRequestDetails?.Reason);
                    MailBody = MailBody.Replace("{link}", baseURL);

                    sendMailbyleaverequest = new()
                    {
                        FromEmailID =appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = employeeandManager?.ManagerEmailID,
                        Subject = MailSubject,
                        MailBody = MailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = employeeandManager?.EmployeeEmailID
                    };
                    string grantMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                }
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = LeaveGrantDetailId
                    });
                }
                else if (LeaveGrantDetailId == 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AddorUpdateLeaveGrant", JsonConvert.SerializeObject(leaveGrantRequestView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = LeaveGrantDetailId
            });
        }
        #endregion
        #region Get Grant Leaves list For EmployeeId
        [HttpGet]
        [Route("GetGrantLeaveByEmployeeId")]
        public async Task<IActionResult> GetGrantLeaveByEmployeeId(int employeeId)
        {
            List<LeaveGrantRequestAndDocumentView> leaveGrantRequestList = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetGrantLeaveByEmployeeId") + employeeId);
                leaveGrantRequestList = JsonConvert.DeserializeObject<List<LeaveGrantRequestAndDocumentView>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = leaveGrantRequestList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetGrantLeaveByEmployeeId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<LeaveGrantRequestAndDocumentView>()
                });
            }
        }
        #endregion
        #region Get Grant Leaves Details For EmployeeId And Leave Grant Id
        [HttpGet]
        [Route("GetGrantLeaveByEmployeeIdAndLeaveGrantId")]
        public async Task<IActionResult> GetGrantLeaveByEmployeeIdAndLeaveGrantId(int employeeId, int leaveGrantDetailId)
        {
            List<LeaveGrantRequestAndDocumentView> leaveGrantRequestList = new();
            try
            {
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetGrantLeaveByEmployeeIdAndLeaveGrantId") + employeeId + "&leaveGrantDetailId=" + leaveGrantDetailId);
                leaveGrantRequestList = JsonConvert.DeserializeObject<List<LeaveGrantRequestAndDocumentView>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = leaveGrantRequestList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetGrantLeaveByEmployeeIdAndLeaveGrantId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<LeaveGrantRequestAndDocumentView>()
                });
            }
        }
        #endregion
        #region Delete Applied Leave Grant
        [HttpGet]
        [Route("DeleteAppliedGrantLeaveByLeaveGrantId")]
        public async Task<IActionResult> DeleteAppliedGrantLeaveByLeaveGrantId(int leaveGrantDetailId)
        {
            try
            {
                var Result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:DeleteAppliedGrantLeaveByLeaveGrantId") + leaveGrantDetailId);
                if (Result != null && Result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Leave Grant deleted successfully.",
                        data = true
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/DeleteAppliedGrantLeaveByLeaveGrantId", JsonConvert.SerializeObject(leaveGrantDetailId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = false
            });
        }
        #endregion
        #region Delete Applied Leave Grant Document
        [HttpGet]
        [Route("DeleteGrantLeaveDocumentByLeaveGrantDocId")]
        public async Task<IActionResult> DeleteGrantLeaveDocumentByLeaveGrantDocId(int leaveGrantDocumentDetailId)
        {
            try
            {
                var Result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:DeleteGrantLeaveDocumentByLeaveGrantDocId") + leaveGrantDocumentDetailId);
                if (Result != null && Result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Leave Grant document deleted successfully.",
                        data = true
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/DeleteGrantLeaveDocumentByLeaveGrantDocId", JsonConvert.SerializeObject(leaveGrantDocumentDetailId));
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
        [Route("DownloadLeaveGrantDocumentById")]
        public async Task<IActionResult> DownloadLeaveGrantDocumentById(int documentId)
        {
            SupportingDocuments documents = new();
            try
            {                
                var result = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:DownloadLeaveGrantDocumentById") + documentId);                
                documents = JsonConvert.DeserializeObject<SupportingDocuments>(JsonConvert.SerializeObject(result?.Data));
                //Read the File into a Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(documents.DocumentPath);
                string contentType;
                new FileExtensionContentTypeProvider().TryGetContentType(documents.DocumentName, out contentType);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    BaseString = Convert.ToBase64String(bytes),
                    ContentType = contentType ?? "application/octet-stream"
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/DownloadLeaveGrantDocumentById", Convert.ToString(documentId));
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
        #region Get employee list
        [HttpGet]
        [Route("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList(int employeeId)
        {
            try
            {
                var result1 = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListByManagerId") + employeeId);
                List<EmployeeList> employeeListView = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(result1?.Data));

                //List<EmployeeList> ShiftList = employeeListView?.Where(x => x.ShiftId == null || x.ShiftId == 0).Select(x => x).ToList();
                var result = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftDetailsList"));
                //List<ShiftView> shiftView = JsonConvert.DeserializeObject<List<ShiftView>>(JsonConvert.SerializeObject(result.Data));
                EmployeeShiftDetailsListView shiftView = JsonConvert.DeserializeObject<EmployeeShiftDetailsListView>(JsonConvert.SerializeObject(result?.Data));
                if (shiftView.DefaultShiftView!=null && employeeListView?.Count>0)
                {
                    //var results = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetDefaultShiftId"));
                    //EmployeeShiftDetailsView employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDetailsView>(JsonConvert.SerializeObject(results.Result.Data));
                    ////List<EmployeeList> ShiftList = employeeListView?.Where(x => x.ShiftId == null || x.ShiftId == 0).Select(x => x).ToList();
                    //foreach (var item in ShiftList)
                    //{
                    //    item.ShiftId = employeeShift.ShiftDetailsId;
                    //}
                    //int generalshiftId = shiftView.DefaultShiftView.Where(x => x.ShiftName?.ToLower() == "general shift").Select(x => x.ShiftDetailsId).FirstOrDefault();
                    //if(generalshiftId==0)
                    //{
                    //    generalshiftId = shiftView.Select(x => x.ShiftDetailsId).FirstOrDefault();
                    //}
                    foreach (var item in employeeListView)
                    {
                        if (item.ShiftId == null || item.ShiftId == 0)
                        {
                            item.ShiftId = shiftView.DefaultShiftView.ShiftDetailsId;
                            //item.ShiftFromTime= shiftView.Where(x => x.ShiftDetailsId == item.ShiftId).Select(x => x.TimeFrom).FirstOrDefault();
                            //item.ShiftToTime = shiftView.Where(x => x.ShiftDetailsId == item.ShiftId).Select(x => x.TimeTo).FirstOrDefault();
                            //item.WeekendId = shiftView.Where(x => x.ShiftDetailsId == item.ShiftId).Select(x => x.WeekendId).FirstOrDefault();
                            item.ShiftFromTime = shiftView.DefaultShiftView.ShiftFromTime;
                            item.ShiftToTime = shiftView.DefaultShiftView.ShiftToTime;
                            item.WeekendId = shiftView.DefaultShiftView.WeekendId;
                            item.IsFlexyShift = shiftView?.DefaultShiftView?.IsFlexyShift == null ? false : (bool)shiftView.DefaultShiftView.IsFlexyShift;
                        }
                        else
                        {
                            bool? isFlex = shiftView?.employeeShifts?.Where(x => x.ShiftDetailsId == item.ShiftId).Select(x => x.IsFlexyShift).FirstOrDefault();
                            item.IsFlexyShift = isFlex==null?false:(bool)isFlex;
                        }
                    }

                    if (employeeListView?.Count > 0)
                    {
                        foreach (EmployeeList item in employeeListView) 
                        {
                            if (item.employeeShiftDetails?.Count > 0) 
                            {
                                foreach (EmployeeShiftDetailsView items in item.employeeShiftDetails)
                                {
                                    items.WeekendId = shiftView?.employeeShifts?.Where(y => y.ShiftDetailsId == items.ShiftDetailsId).Select(y => y.WeekendId).FirstOrDefault();
                                }
                            }
                        }
                    }
                    if (employeeListView?.Count > 0)
                    {
                        foreach (EmployeeList item in employeeListView)
                        {
                            if (item.defaultShiftDetails == null)
                            {
                                item.defaultShiftDetails = shiftView?.DefaultShiftView;
                            }
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeListView 
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion

        #region Get RoundOff
        public static decimal GetRoundOff(decimal count)
        {
            decimal sd = default;

            if (count != null && count != 0)
            {
                //var decimalValues = (decimal)1.5112548;
                decimal decimalValue = decimal.Round(count, 2, MidpointRounding.AwayFromZero);
                var split = decimalValue.ToString(CultureInfo.InvariantCulture).Split('.');
                
                var de = Convert.ToDecimal("0." + (split.Length > 1 ? split[1] : 0));
                if (de is >= (decimal)0.0 and <= (decimal)0.24)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.0);

                }
                else if (de is >= (decimal)0.25 and <= (decimal)0.49)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.25);

                }
                else if (de is >= (decimal)0.50 and <= (decimal)0.74)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.50);

                }
                else if (de is >= (decimal)0.75 and <= (decimal)0.99)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.75);

                }
            }
            else
            {
                sd = 0;
            }

            return sd;
        }
        #endregion
        [NonAction]
        public async Task<Holiday> getApplicableHolidayList(Holiday holidayList, int departmentId, int locationId, int shiftId, HolidayDetailsView holidayDetails)
        {
            Holiday empHolidayList = new Holiday();

                if (holidayList!=null)
                {
                    bool applicable = false;
                    //Check department
                    var depList = holidayDetails?.HolidayDepartment.Where(x => x.HolidayId == holidayList.HolidayID).Select(x => x.DepartmentId).ToList();
                    if (depList?.Count > 0)
                    {
                        if (depList.Contains(departmentId))
                        {
                            applicable = true;
                        }
                        else
                        {
                            applicable = false;
                        }
                    }
                    else
                    {
                        applicable = true;
                    }
                    //Check shift
                    if (applicable)
                    {
                        var shiftList = holidayDetails?.HolidayShift.Where(x => x.HolidayId == holidayList.HolidayID).Select(x => x.ShiftDetailsId).ToList();
                        if (shiftList?.Count > 0)
                        {
                            if (shiftList.Contains(shiftId))
                            {
                                applicable = true;
                            }
                            else
                            {
                                applicable = false;
                            }
                        }
                        else
                        {
                            applicable = true;
                        }
                        //check location
                        if (applicable)
                        {
                            var locationList = holidayDetails?.HolidayLocation.Where(x => x.HolidayId == holidayList.HolidayID).Select(x => x.LocationId).ToList();
                            if (locationList?.Count > 0)
                            {
                                if (locationList.Contains(locationId))
                                {
                                    applicable = true;
                                }
                                else
                                {
                                    applicable = false;
                                }
                            }
                            else
                            {
                                applicable = true;
                            }
                        }

                    }


                    if (applicable)
                    {
                        return holidayList;
                    }

                }
            return new Holiday();
        }

        #region Get leave history
        [HttpPost]
        [Route("GetLeaveHistoryByLeaveType")]
        public async Task<IActionResult> GetLeaveHistoryByLeaveType(LeaveHistoryModel model)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(model, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveHistoryByLeaveType"));
                List<LeaveHistoryView> leaveHistory = JsonConvert.DeserializeObject<List<LeaveHistoryView>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = string.Empty,
                        data = leaveHistory
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetLeaveHistoryByLeaveType", JsonConvert.SerializeObject(model));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = new List<LeaveHistoryView>()
            });
        }
        #endregion

        #region Get Employee Leave and Balance Details By Employee Id
        [HttpPost]
        [Route("GetEmployeeLeavesBalanceDetails")]
        public async Task<IActionResult> GetEmployeeLeavesBalanceDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            //Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;

                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeLeavesBalanceDetails"));
                individualLeaveList = JsonConvert.DeserializeObject<IndividualLeaveList>(JsonConvert.SerializeObject(result?.Data));


                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = individualLeaveList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeLeavesBalanceDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = individualLeaveList
            });
        }
        #endregion
        #region Get Employee Leave and Balance Details By Employee Id
        [HttpPost]
        [Route("GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId")]
        public async Task<IActionResult> GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            EmployeeAvailableLeaveDetails leaveDetails = new();
            //Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;

                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId"));
                leaveDetails = JsonConvert.DeserializeObject<EmployeeAvailableLeaveDetails>(JsonConvert.SerializeObject(result?.Data));


                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = leaveDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = leaveDetails
            });
        }
        #endregion
        #region Get Employee Leave and Balance Details By Employee Id
        [HttpPost]
        [Route("GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId")]
        public async Task<IActionResult> GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            List<ApplyLeavesView> leaveDetails = new();
            //Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;

                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId"));
                leaveDetails = JsonConvert.DeserializeObject<List<ApplyLeavesView>>(JsonConvert.SerializeObject(result?.Data));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = leaveDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = leaveDetails
            });
        }
        #endregion
        #region GetEmployeePersonalDetailsForLeaves
        [HttpGet]
        [Route("GetEmployeePersonalDetailsForLeaves")]
        public async Task<IActionResult> GetEmployeePersonalDetailsForLeaves(int employeeId)
        {
            List<ApplyLeavesView> leaveDetails = new();
            //Employees employeeDetails = new();
            try
            {
                var employeeResult =  _client.GetAsync( _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:EmployeeDetailsForLeave") + employeeId);
                EmployeeDetailsForLeave employeeDetail = JsonConvert.DeserializeObject<EmployeeDetailsForLeave>(JsonConvert.SerializeObject(employeeResult.Result.Data));


                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = employeeDetail
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeePersonalDetailsForLeaves", JsonConvert.SerializeObject(employeeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = leaveDetails
            });
        }
        #endregion

        #region Get Employee applied leave details
        [HttpPost]
        [Route("GetEmployeeShiftAndHolidayList")]
        public async Task<IActionResult> GetEmployeeShiftAndHolidayList(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            //Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;
                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeHolidayDetails"));
                individualLeaveList = JsonConvert.DeserializeObject<IndividualLeaveList>(JsonConvert.SerializeObject(result?.Data));
                var employeeShiftResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeShiftDetailsById") + employeeId);
                List<EmployeeShiftDetailsView> employeeShiftDetails = JsonConvert.DeserializeObject<List<EmployeeShiftDetailsView>>(JsonConvert.SerializeObject(employeeShiftResult?.Data));
                var shiftList = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftDetailsList"));
                EmployeeShiftDetailsListView ShiftDetailList = JsonConvert.DeserializeObject<EmployeeShiftDetailsListView>(JsonConvert.SerializeObject(shiftList.Data));
                List<Holiday> financialHolidayList = new();
                if (individualLeaveList?.HolidayDetails?.HolidayList?.Count > 0)
                {
                    foreach (var item in individualLeaveList?.HolidayDetails?.HolidayList)
                    {
                        int? shiftId = 0;
                        if (employeeShiftDetails?.Count > 0)
                        {
                            foreach (EmployeeShiftDetailsView shift in employeeShiftDetails)
                            {
                                if (item.HolidayDate >= shift.ShiftFromDate && (shift.ShiftToDate == null || item.HolidayDate <= shift.ShiftToDate))
                                {
                                    shiftId = ShiftDetailList?.employeeShifts.Where(x => x.ShiftDetailsId == shift.ShiftDetailsId).Select(x => x.ShiftDetailsId).FirstOrDefault();
                                }
                            }
                        }
                        if (shiftId == 0)
                        {
                            shiftId = ShiftDetailList?.DefaultShiftView.ShiftDetailsId;
                        }
                        Holiday holiday = await getApplicableHolidayList(item, employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.LocationId, (int)shiftId, individualLeaveList?.HolidayDetails);
                        financialHolidayList.Add(holiday);
                    }
                    individualLeaveList.HolidayList = new List<Holiday>();
                    individualLeaveList.HolidayList = financialHolidayList.Where(x => x.HolidayName != null).Select(x => x).ToList();
                }
                if (ShiftDetailList != null)
                {
                    if (employeeShiftDetails?.Count > 0)
                    {
                        employeeShiftDetails?.ForEach(x => x.WeekendId = ShiftDetailList?.employeeShifts?.Where(y => y.ShiftDetailsId == x.ShiftDetailsId).Select(y => y.WeekendId).FirstOrDefault());
                    }
                    individualLeaveList.EmployeeShiftDetailsList = new EmployeeShiftDetailsListView();
                    if (ShiftDetailList.DefaultShiftView != null)
                    {
                        individualLeaveList.EmployeeShiftDetailsList.DefaultShiftView = new EmployeeShiftDetailsView
                        {
                            EmployeeID = ShiftDetailList.DefaultShiftView.EmployeeID,
                            ShiftDetailsId = ShiftDetailList.DefaultShiftView.ShiftDetailsId,
                            ShiftFromDate = ShiftDetailList.DefaultShiftView.ShiftFromDate,
                            ShiftToDate = ShiftDetailList.DefaultShiftView.ShiftToDate,
                            WeekendId = ShiftDetailList.DefaultShiftView.WeekendId
                        };
                    }
                    individualLeaveList.EmployeeShiftDetailsList.employeeShifts = new List<EmployeeShiftDetailsView>();
                    individualLeaveList.EmployeeShiftDetailsList.employeeShifts = individualLeaveList?.EmployeeShiftDetailsList?.employeeShifts?.Concat(employeeShiftDetails).ToList();
                }

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = individualLeaveList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeAppliedLeaveDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = individualLeaveList
            });
        }
        #endregion
        #region Get applied leave details By EmployeeId
        [HttpPost]
        [Route("GetAppliedLeaveDetailsByEmployeeId")]
        public async Task<IActionResult> GetAppliedLeaveDetailsByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            List<SupportingDocuments> ListOfDocument = new();
            //Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;
                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetAppliedLeaveDetailsByEmployeeId"));
                individualLeaveList = JsonConvert.DeserializeObject<IndividualLeaveList>(JsonConvert.SerializeObject(result?.Data));
                ListOfDocument = GetDocumentByLeaveId(individualLeaveList.AppliedLeaveDetails?.Select(x => x.LeaveId ?? 0).ToList()).Result;
                individualLeaveList.AppliedLeaveDetails?.ForEach(x => x.ListOfDocuments = ListOfDocument?.Where(y => x.LeaveId != 0 && y.SourceId == x.LeaveId ) 
                .Select(x => new SupportingDocuments
                {
                    DocumentId = x.DocumentId,
                    DocumentName = x.DocumentName,
                    DocumentSize = x.DocumentSize,
                    DocumentCategory = x.DocumentCategory,
                    IsApproved = x.IsApproved,
                    DocumentType = x.DocumentType
                }).ToList());

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = individualLeaveList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeAppliedLeaveDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = individualLeaveList
            });
        }
        #endregion
        #region Get applied leave details By EmployeeId
        [HttpPost]
        [Route("GetRegularizationDetailsByEmployeeId")]
        public async Task<IActionResult> GetRegularizationDetailsByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            RegularizationDetailView regularizationList = new();
            //Employees employeeDetails = new();
            try
            {
                int employeeId = employeeLeaveandRestriction.EmployeeId;
                var result = await _client.PostAsJsonAsync(employeeLeaveandRestriction, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeRegularizationDetailById"));
                regularizationList = JsonConvert.DeserializeObject<RegularizationDetailView>(JsonConvert.SerializeObject(result?.Data));


                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    data = regularizationList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/GetEmployeeRegularizationDetailById", JsonConvert.SerializeObject(employeeLeaveandRestriction));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = regularizationList
            });
        }
        #endregion
        #region Approve or Reject Leave
        [HttpPost]
        [Route("MultiSelectApproveOrRejectLeave")]
        public async Task<IActionResult> MultiSelectApproveOrRejectLeave(List<ApproveOrRejectLeave> approveOrRejectLeave)
        {

            string LeaveId = "", statusText = "", RegularizeId = "", Subject = null, Body = null;
            try
            {
                // For Mail purpose
                List<TimeLogApproveOrRejectView> timeLogApproveOrRejectView = approveOrRejectLeave.Where(x => x.IsRegularize == true).Select(x => x.TimeLog).ToList();
                List<ApproveOrRejectLeave> leaveData = approveOrRejectLeave.Where(x => x.IsRegularize == false).ToList();
                if (timeLogApproveOrRejectView?.Count > 0)
                {

                    //TimeLogApproveOrRejectView timeLogApproveOrRejectView = new();
                    //timeLogApproveOrRejectView = approveOrRejectLeave?.TimeLog;
                    var results = await _client.PostAsJsonAsync(timeLogApproveOrRejectView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:MultiSelectTimeLogApproveOrReject"));
                    RegularizeId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(results?.Data));
                    //string regStatus = approveOrRejectLeave?.TimeLog?.IsApproveOrCancel;
                    var data = approveOrRejectLeave.Where(x => x.IsRegularize == true).ToList();
                    if (data?.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            TimeLogApproveOrRejectNotification(item);
                        }
                    }

                }
                else if (leaveData?.Count > 0)
                {
                    var result = await _client.PostAsJsonAsync(leaveData, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:MultiSelectApproveOrRejectLeave"));
                    List<StatusandApproverDetails> statusandApprover = JsonConvert.DeserializeObject<List<StatusandApproverDetails>>(JsonConvert.SerializeObject(result?.Data));
                    //var reasonResult = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveRejectionReason"));
                    //List<LeaveRejectionReason> reasonList = JsonConvert.DeserializeObject<List<LeaveRejectionReason>>(JsonConvert.SerializeObject(reasonResult?.Data));
                    //int Feedback = approveOrRejectLeave.LeaveRejectionReasonId;
                    //if (approveOrRejectLeave.Feedback == null || approveOrRejectLeave?.Feedback == string.Empty) 
                    //{
                    //    approveOrRejectLeave.Feedback = reasonList?.Where(x => x.LeaveRejectionReasonId == Feedback).Select(x => x.LeaveRejectionReasons).FirstOrDefault();
                    //}
                    foreach (var item in leaveData)
                    {
                        StatusandApproverDetails data = new StatusandApproverDetails();
                        if (item.IsGrantLeaveRequest == true)
                        {
                            data = statusandApprover.Where(x => x.LeaveGrantDetailId == item.LeaveGrantDetailId).FirstOrDefault();
                        }
                        else
                        {
                            data = statusandApprover.Where(x => x.LeaveId == item.LeaveId).FirstOrDefault();
                        }
                        ApproveOrRejectLeaveNotification(item, data);
                    }

                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Selected request(s) "+ approveOrRejectLeave[0].Status+" successfully",
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/ApproveOrRejectLeave", JsonConvert.SerializeObject(approveOrRejectLeave));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                LeaveId,
            });
        }
        #endregion
        #region 
        [NonAction]
        public async Task<string> TimeLogApproveOrRejectNotification(ApproveOrRejectLeave approveOrRejectLeave)
        {
            //StatusandApproverDetails statusandApprover = new();
            string LeaveId = "", statusText = "",
                //RegularizeId = "", 
                Subject = null, Body = null;
            try
            {
                // For Mail purpose
                EmployeeandManagerView employeeandManager = new EmployeeandManagerView();
                var employeeID = approveOrRejectLeave?.EmployeeId;
                var empresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeID);
                employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(empresults?.Data));

                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                string MailSubject = null, MailBody = null;
                SendEmailView sendMailbyleaverequest = new();

                //if (approveOrRejectLeave?.IsRegularize == true)
                //{

                TimeLogApproveOrRejectView timeLogApproveOrRejectView = new();
                timeLogApproveOrRejectView = approveOrRejectLeave?.TimeLog;
                //var results = await _client.PostAsJsonAsync(timeLogApproveOrRejectView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:TimeLogApproveOrReject"));
                //RegularizeId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(results?.Data));
                string regStatus = approveOrRejectLeave?.TimeLog?.IsApproveOrCancel;
                //if (results != null)
                //{
                if (regStatus?.ToLower() == "Approved"?.ToLower() || regStatus?.ToLower() == "Rejected"?.ToLower() || regStatus?.ToLower() == "Cancelled"?.ToLower())
                {
                    // Notification
                    if (regStatus?.ToLower() == "Approved"?.ToLower())
                    {
                        Subject = "Your Regularization are Approved.";
                        Body = employeeandManager?.ManagerName + " has approved your Regularization are Approved.";
                    }
                    else if (regStatus?.ToLower() == "Rejected"?.ToLower())
                    {
                        Subject = "Your Regularization are Rejected";
                        Body = "Your Regularization are Rejected";
                    }
                    else if (regStatus?.ToLower() == "Cancelled"?.ToLower())
                    {
                        Subject = "Your Regularization are Cancelled";
                        Body = "Your Regularization are Cancelled";
                    }

                    List<Notifications> notifications = new();
                    Notifications notification = new();
                    notification = new()
                    {
                        CreatedBy = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                        CreatedOn = DateTime.UtcNow,
                        FromId = employeeandManager?.ReportingManagerID == null ? 0 : (int)employeeandManager?.ReportingManagerID,
                        ToId = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                        MarkAsRead = false,
                        NotificationSubject = Subject,
                        NotificationBody = approveOrRejectLeave?.ApproverName + " has " + regStatus?.ToLower() + " your regularization request for " + approveOrRejectLeave?.TimeLog?.LeaveDate?.ToString() + ".",
                        PrimaryKeyId = approveOrRejectLeave?.TimeLog?.AttendanceDetailId,
                        ButtonName = "View Regularization",
                        SourceType = "Regularization",
                    };
                    string regNotification = _commonFunction.Notification(notification).Result;

                    DateTime checkindateTime = DateTime.Parse(approveOrRejectLeave?.TimeLog?.FromTime);
                    DateTime checkoutdateTime = DateTime.Parse(approveOrRejectLeave?.TimeLog?.ToTime);
                    var CheckinTotalHrs = checkoutdateTime - checkindateTime;
                    var TotalHrscheckin = new DateTime(CheckinTotalHrs.Ticks);
                    var TotalHrs = CheckinTotalHrs.ToString("hh\\:mm");
                    TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime localcheckintime = TimeZoneInfo.ConvertTimeFromUtc(checkindateTime, zone);
                    TimeSpan? iSTcheckinTime = localcheckintime.TimeOfDay;
                    DateTime iSTlocalcheckouttime = TimeZoneInfo.ConvertTimeFromUtc(checkoutdateTime, zone);
                    TimeSpan? iSTcheckoutTime = iSTlocalcheckouttime.TimeOfDay;
                    var checkinTime = new DateTime(iSTcheckinTime.Value.Ticks); // Date part is 01-01-0001
                    var formattedCheckinTime = checkinTime.ToString("h:mm tt", CultureInfo.InvariantCulture);
                    var checkoutTime = new DateTime(iSTcheckoutTime.Value.Ticks); // Date part is 01-01-0001
                    var formattedCheckOutTime = checkoutTime.ToString("h:mm tt", CultureInfo.InvariantCulture);
                    string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Date</b></td><td><b>Check-in Time</b></td><td><b>Check-out Time</b></td><td><b>Logged Hours</b></td><td><b>Regularization Status</b></td></tr>";
                    textBody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.TimeLog?.LeaveDate?.ToString() + "</td><td >" + formattedCheckinTime?.ToString() + "</td><td > " + formattedCheckOutTime?.ToString() + "</td><td >" + (TotalHrs + " Hrs")?.ToString() + "</td><td >" + regStatus?.ToString() + "</td></tr></table>";

                    MailSubject = "Your request for Regularization is {leaveStatus}";
                    MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your {LeaveTypeName} request has been {Status} by {ApproverName}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Attendance'>here</a> to view.</p>                                   
                                    <div>{table}</div>                                   
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>    
                                    </html>";
                    MailSubject = MailSubject.Replace("{leaveStatus}", regStatus);
                    MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                    MailBody = MailBody.Replace("{LeaveTypeName}", approveOrRejectLeave?.LeaveTypeName);
                    MailBody = MailBody.Replace("{Status}", regStatus);
                    MailBody = MailBody.Replace("{ApproverName}", approveOrRejectLeave?.ApproverName);
                    MailBody = MailBody.Replace("{link}", baseURL);
                    if (regStatus?.ToLower() == "approved")
                    {
                        MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                    }
                    else
                    {
                        MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.TimeLog?.RejectReason);
                    }
                    MailBody = MailBody.Replace("{table}", textBody);

                    sendMailbyleaverequest = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = employeeandManager?.EmployeeEmailID,
                        Subject = MailSubject,
                        MailBody = MailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = employeeandManager?.ManagerEmailID

                    };
                    string regMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                }
                //}
                //return Ok(new
                //{
                //    results.StatusCode,
                //    results.StatusText,

                //});
                //}

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/TimeLogApproveOrRejectNotification", JsonConvert.SerializeObject(approveOrRejectLeave));
                statusText = strErrorMsg;
            }
            return "Success";
        }
        [NonAction]
        public async Task<string> ApproveOrRejectLeaveNotification(ApproveOrRejectLeave approveOrRejectLeave, StatusandApproverDetails statusandApprover)
        {
            //StatusandApproverDetails statusandApprover = new();
            string LeaveId = "", statusText = "",
                //RegularizeId = "", 
                Subject = null, Body = null;
            try
            {
                // For Mail purpose
                EmployeeandManagerView employeeandManager = new EmployeeandManagerView();
                var employeeID = approveOrRejectLeave?.EmployeeId;
                var empresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeID);
                employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(empresults?.Data));

                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                string MailSubject = null, MailBody = null;
                SendEmailView sendMailbyleaverequest = new();

                //var result = await _client.PostAsJsonAsync(approveOrRejectLeave, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:ApproveOrRejectLeave"));
                //statusandApprover = JsonConvert.DeserializeObject<StatusandApproverDetails>(JsonConvert.SerializeObject(result?.Data));
                var reasonResult = await _client.GetAsync(_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveRejectionReason"));
                List<LeaveRejectionReason> reasonList = JsonConvert.DeserializeObject<List<LeaveRejectionReason>>(JsonConvert.SerializeObject(reasonResult?.Data));
                int Feedback = approveOrRejectLeave.LeaveRejectionReasonId;
                if (approveOrRejectLeave.Feedback == null || approveOrRejectLeave?.Feedback == string.Empty)
                {
                    approveOrRejectLeave.Feedback = reasonList?.Where(x => x.LeaveRejectionReasonId == Feedback).Select(x => x.LeaveRejectionReasons).FirstOrDefault();
                }
                //if (result != null && result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                //{
                if (approveOrRejectLeave?.Status.ToLower() == "Approved".ToLower() || approveOrRejectLeave?.Status.ToLower() == "Rejected".ToLower() || approveOrRejectLeave?.Status.ToLower() == "Cancelled".ToLower())
                {
                    // Send Mail                            
                    List<MailLeaveList> leavelist = new();
                    MailLeaveList mailLeaveList = new();

                    if ((approveOrRejectLeave?.IsGrantLeaveRequest == false || approveOrRejectLeave.IsGrantLeaveRequest == null) && (approveOrRejectLeave?.IsRegularize == false || approveOrRejectLeave.IsRegularize == null))
                    {
                        // Notification

                        if (approveOrRejectLeave?.Status.ToLower() == "Approved".ToLower())
                        {
                            Subject = "Your Leave(s) are Approved.";
                            Body = "Your Leave(s) are Approved.";
                        }
                        else if (approveOrRejectLeave?.Status.ToLower() == "Rejected".ToLower())
                        {
                            Subject = "Your Leave(s) are Rejected";
                            Body = "Your Leave(s) are Rejected";
                        }
                        else if (approveOrRejectLeave?.Status.ToLower() == "Cancelled".ToLower())
                        {
                            Subject = "Your Leave(s) are Cancelled";
                            Body = "Your Leave(s) are Cancelled";
                        }
                        DateTime? fromdate = DateTime.MinValue, todate = DateTime.MinValue;
                        if (approveOrRejectLeave?.AppliedLeaveApproveOrReject != null)
                        {
                            fromdate = approveOrRejectLeave.AppliedLeaveApproveOrReject.Select(x => x.LeaveDate).FirstOrDefault();
                            todate = approveOrRejectLeave.AppliedLeaveApproveOrReject.Select(x => x.LeaveDate).LastOrDefault();
                        }
                        List<Notifications> notifications = new();
                        Notifications notification = new();
                        notification = new()
                        {
                            CreatedBy = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                            CreatedOn = DateTime.UtcNow,
                            FromId = approveOrRejectLeave?.ManagerId == null ? 0 : (int)approveOrRejectLeave?.ManagerId,
                            ToId = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                            MarkAsRead = false,
                            NotificationSubject = Subject,
                            NotificationBody = approveOrRejectLeave?.ApproverName + " has " + approveOrRejectLeave?.Status.ToLower() + " your request for " + approveOrRejectLeave?.LeaveTypeName + " " + fromdate?.ToString("dd MMM yyyy") + " to " + todate?.ToString("dd MMM yyyy") + ".",
                            PrimaryKeyId = approveOrRejectLeave?.LeaveId,
                            ButtonName = "View Leave",
                            SourceType = "Leaves",
                        };
                        string leaveNotification = _commonFunction.Notification(notification).Result;

                        // Mail Template
                        if (approveOrRejectLeave?.Status.ToLower() == "Cancelled".ToLower())
                        {
                            string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>From Date</b></td><td><b>To Date</b></td><td><b>Leave Status</b></td></tr>";
                            textBody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.FromDate.ToString() + "</td><td > " + approveOrRejectLeave?.ToDate.ToString() + "</td><td >" + approveOrRejectLeave?.Status.ToString() + "</td></tr></table>";

                            MailSubject = "Your request for leave is {leaveStatus}";
                            MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your {LeaveTypeName} request has been {Status} by {ApproverName}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Leaves'>here</a> to view.</p>                                       
                                    <div>{table}</div>  
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback} </p></td></tr></tbody></table>
                                    </body>                                   
                                    </html>";
                            MailSubject = MailSubject.Replace("{leaveStatus}", approveOrRejectLeave?.Status);
                            MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                            MailBody = MailBody.Replace("{LeaveTypeName}", approveOrRejectLeave?.LeaveTypeName);
                            MailBody = MailBody.Replace("{Status}", approveOrRejectLeave?.Status);
                            MailBody = MailBody.Replace("{ApproverName}", approveOrRejectLeave?.ApproverName);
                            MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                            MailBody = MailBody.Replace("{table}", textBody);
                            MailBody = MailBody.Replace("{link}", baseURL);
                        }
                        else
                        {
                            foreach (var item in approveOrRejectLeave?.AppliedLeaveApproveOrReject)
                            {
                                mailLeaveList = new()
                                {
                                    LeaveDate = item?.LeaveDate?.ToString("dd MMM yyyy"),
                                    LeaveStatus = item?.AppliedLeaveStatus == true ? "Approved" : "Rejected",
                                    LeaveDuration = item?.IsFirstHalf == true ? "FirstHalf" : item?.IsSecondHalf == true ? "SecondHalf" : "FullDay",
                                };
                                leavelist.Add(mailLeaveList);
                            }

                            string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Date</b></td><td ><b>Leave Duration</b></td><td><b>Leave Status</b></td></tr>";
                            for (int i = 0; i < leavelist?.Count; i++)
                            {
                                textBody += "<tr style='text-align:center';><td >" + leavelist[i].LeaveDate.ToString() + "</td><td > " + leavelist[i].LeaveDuration.ToString() + "</td><td >" + leavelist[i].LeaveStatus.ToString() + "</td> </tr>";
                            }
                            textBody += "</table>";
                            MailSubject = "Your request for leave is {leaveStatus}";
                            MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your {LeaveTypeName} request has been {Status}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Leaves'>here</a> to view.</p>                                   
                                    <div>{table}</div>
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>                                   
                                    </html>";
                            MailSubject = MailSubject.Replace("{leaveStatus}", approveOrRejectLeave?.Status);
                            MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                            MailBody = MailBody.Replace("{LeaveTypeName}", approveOrRejectLeave?.LeaveTypeName);
                            MailBody = MailBody.Replace("{Status}", approveOrRejectLeave?.Status);
                            MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                            MailBody = MailBody.Replace("{table}", textBody);
                            //<table><tbody><tr><td><p><b>Comments:</b>{Feedback}</p></td></tr></tbody></table>
                            MailBody = MailBody.Replace("{link}", baseURL);

                        }

                        sendMailbyleaverequest = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = employeeandManager?.EmployeeEmailID,
                            Subject = MailSubject,
                            MailBody = MailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                            CC = employeeandManager?.ManagerEmailID

                        };
                        string leaveMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                    }
                    else if (approveOrRejectLeave?.IsGrantLeaveRequest == true)
                    {
                        //Next level approver details
                        EmployeeandManagerView nextlevelmanager = new EmployeeandManagerView();
                        if (statusandApprover?.ApproverID != null)
                        {
                            var nextmanresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + statusandApprover.ApproverID);
                            nextlevelmanager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(nextmanresults?.Data));
                        }

                        // Notification

                        if (approveOrRejectLeave?.Status.ToLower() == "Approved".ToLower())
                        {
                            Subject = "Your GrantLeave Request are Approved.";
                            Body = approveOrRejectLeave?.ApproverName + " has approved your " + approveOrRejectLeave?.LevelId + " - level request for " + approveOrRejectLeave?.LeaveTypeName + ". Effective From Date : " + approveOrRejectLeave?.FromDate?.ToString() + ".";
                        }
                        else if (approveOrRejectLeave?.Status.ToLower() == "Rejected".ToLower())
                        {
                            Subject = "Your GrantLeave Request are Rejected";
                            Body = approveOrRejectLeave?.ApproverName + " has rejected your " + approveOrRejectLeave?.LevelId + " - level request for " + approveOrRejectLeave?.LeaveTypeName + ". Effective From Date : " + approveOrRejectLeave?.FromDate?.ToString() + ".";
                        }
                        else if (approveOrRejectLeave?.Status.ToLower() == "Cancelled".ToLower())
                        {
                            Subject = "Your GrantLeave Request are Cancelled";
                            Body = approveOrRejectLeave?.ApproverName + " has cancelled your " + approveOrRejectLeave?.LevelId + " - level request for " + approveOrRejectLeave?.LeaveTypeName + ". Effective From Date : " + approveOrRejectLeave?.FromDate?.ToString() + ".";
                        }

                        List<Notifications> notifications = new();
                        Notifications notification = new();
                        notification = new()
                        {
                            CreatedBy = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                            CreatedOn = DateTime.UtcNow,
                            FromId = approveOrRejectLeave?.ManagerId == null ? 0 : (int)approveOrRejectLeave.ManagerId,
                            ToId = approveOrRejectLeave?.EmployeeId == null ? 0 : (int)approveOrRejectLeave?.EmployeeId,
                            MarkAsRead = false,
                            NotificationSubject = Subject,
                            NotificationBody = Body,
                            PrimaryKeyId = approveOrRejectLeave?.LeaveId,
                            ButtonName = "View Grant Leave",
                            SourceType = "GrantLeaves",
                        };
                        string grantNotification = _commonFunction.Notification(notification).Result;

                        //Next Level Manager Notification
                        Notifications nextManagerNotification = new();
                        if (nextlevelmanager != null)
                        {
                            nextManagerNotification = new()
                            {
                                CreatedBy = employeeandManager?.EmployeeID == null ? 0 : (int)employeeandManager?.EmployeeID,
                                CreatedOn = DateTime.UtcNow,
                                FromId = employeeandManager?.EmployeeID == null ? 0 : (int)employeeandManager?.EmployeeID,
                                ToId = nextlevelmanager?.EmployeeID == null ? 0 : (int)nextlevelmanager?.EmployeeID,
                                MarkAsRead = false,
                                NotificationSubject = "New GrantLeave request from " + employeeandManager?.EmployeeName + ".",
                                NotificationBody = employeeandManager?.EmployeeName + "'s " + approveOrRejectLeave?.LeaveTypeName + " " + statusandApprover?.LevelId + " - level request is waiting for your approval." + "Effective From Date : " + approveOrRejectLeave?.FromDate?.ToString() + ".",
                                PrimaryKeyId = approveOrRejectLeave?.LeaveId,
                                ButtonName = "Approve Grant Leave",
                                SourceType = "GrantLeaves",
                            };
                            string grantManagerNotification = _commonFunction.Notification(nextManagerNotification).Result;
                        }

                        // Mail Template

                        EmployeeandManagerView approvedmanager = new EmployeeandManagerView();
                        var nextresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + statusandApprover.ApproverManagerId);
                        approvedmanager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(nextresults?.Data));

                        if (approveOrRejectLeave?.Status?.ToLower() == "Cancelled".ToLower())
                        {
                            string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Type</b></td><td><b>Effective From Date</b></td><td><b>No.Of Days</b></td><td><b>Leave Status</b></td></tr>";
                            textBody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.LeaveTypeName + "</td><td >" + approveOrRejectLeave?.FromDate?.ToString() + "</td><td >" + approveOrRejectLeave?.NoOfDays + "</td><td >" + approveOrRejectLeave?.Status?.ToString() + "</td></tr></table>";
                            MailSubject = "Your request for leave is {leaveStatus}";
                            MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your {LeaveTypeName} request has been {Status} by {ApproverName}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Leaves'>here</a> to view.</p>                                   
                                    <div>{table}</div>                                   
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>                                   
                                    </html>";
                            MailSubject = MailSubject.Replace("{leaveStatus}", approveOrRejectLeave?.Status);
                            MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                            MailBody = MailBody.Replace("{LeaveTypeName}", approveOrRejectLeave?.LeaveTypeName);
                            MailBody = MailBody.Replace("{Status}", approveOrRejectLeave?.Status);
                            MailBody = MailBody.Replace("{ApproverName}", approveOrRejectLeave?.ApproverName);
                            MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                            MailBody = MailBody.Replace("{table}", textBody);
                            MailBody = MailBody.Replace("{link}", baseURL);
                            sendMailbyleaverequest = new()
                            {
                                FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                ToEmailID = employeeandManager?.EmployeeEmailID,
                                Subject = MailSubject,
                                MailBody = MailBody,
                                ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                Host = appsetting.GetSection("EmailHost").Value,
                                FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                CC = approvedmanager?.EmployeeEmailID
                            };
                            string grantMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                        }
                        else
                        {
                            //EmployeeandManagerView approvedmanager = new EmployeeandManagerView();
                            //var nextresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + statusandApprover.ApproverManagerId);
                            //approvedmanager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(nextresults?.Data));

                            string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Type</b></td><td><b>Effective From Date</b></td><td><b>No.Of Days</b></td><td><b>Leave Status</b></td></tr>";
                            textBody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.LeaveTypeName + "</td><td >" + approveOrRejectLeave?.FromDate?.ToString() + "</td><td >" + approveOrRejectLeave?.NoOfDays + "</td><td >" + approveOrRejectLeave?.Status?.ToString() + "</td></tr></table>";

                            MailSubject = "Your request for grant leave is {leaveStatus}.";
                            MailBody = @"<html>
                                    <body>                                    
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your grant leave request has been {Status} by {ManagerName}. Please click <a href='{link}/#/pmsnexus/workday?isManager=false&RequestType=Leaves'>here</a> to view.</p> 
                                    <div>{table}</div>                                   
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>                                
                                    </html>";
                            MailSubject = MailSubject.Replace("{leaveStatus}", approveOrRejectLeave?.Status);
                            MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                            MailBody = MailBody.Replace("{LeaveName}", approveOrRejectLeave?.LeaveTypeName);
                            MailBody = MailBody.Replace("{Status}", approveOrRejectLeave?.Status);
                            if (approveOrRejectLeave?.ApproverName != null)
                            {
                                MailBody = MailBody.Replace("{ManagerName}", approveOrRejectLeave?.ApproverName);
                            }
                            else
                            {
                                MailBody = MailBody.Replace("{ManagerName}", employeeandManager?.ManagerName);
                            }
                            MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Feedback);
                            MailBody = MailBody.Replace("{table}", textBody);
                            MailBody = MailBody.Replace("{link}", baseURL);

                            sendMailbyleaverequest = new()
                            {
                                FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                ToEmailID = employeeandManager?.EmployeeEmailID,
                                Subject = MailSubject,
                                MailBody = MailBody,
                                ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                Host = appsetting.GetSection("EmailHost").Value,
                                FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                CC = approvedmanager?.EmployeeEmailID
                            };
                            string grantMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;

                            if (statusandApprover?.ApproverID != null)
                            {
                                //Next level approver mail

                                string textbody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Leave Type</b></td><td><b>Effective From Date</b></td><td><b>No.Of Days</b></td></tr>";
                                textbody += "<tr style='text-align:center';><td >" + approveOrRejectLeave?.LeaveTypeName + "</td><td > " + approveOrRejectLeave?.FromDate?.ToString() + "</td><td >" + approveOrRejectLeave?.NoOfDays + "</td></tr></table>";

                                MailSubject = "{EmployeeName} sent grant leave request.";
                                MailBody = @"<html>
                                        <body>                                  
                                        <p>Dear {ManagerName},</p>                                    
                                        <p> {EmployeeName} requested for grant leave from {FromDate}. Please click <a href='{link}/#/pmsnexus/leaves/leave-team'>here</a> to Approve/Reject.</p>                                    
                                        <div>{table}</div>  
                                        <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                        </body>                                   
                                        </html>";
                                MailSubject = MailSubject.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                MailBody = MailBody.Replace("{ManagerName}", nextlevelmanager?.EmployeeName);
                                MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                                MailBody = MailBody.Replace("{FromDate}", approveOrRejectLeave?.FromDate);
                                MailBody = MailBody.Replace("{table}", textbody);
                                MailBody = MailBody.Replace("{Feedback}", approveOrRejectLeave?.Reason);
                                MailBody = MailBody.Replace("{link}", baseURL);

                                sendMailbyleaverequest = new()
                                {
                                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                    ToEmailID = nextlevelmanager?.EmployeeEmailID,
                                    Subject = MailSubject,
                                    MailBody = MailBody,
                                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                    Host = appsetting.GetSection("EmailHost").Value,
                                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                    CC = employeeandManager?.EmployeeEmailID
                                };
                                string grantNextlLevelMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                            }
                        }
                    }
                }
                //    return Ok(new
                //    {
                //        result.StatusCode,
                //        result.StatusText,
                //    });
                //}
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Leaves/ApproveOrRejectLeave", JsonConvert.SerializeObject(approveOrRejectLeave));
                statusText = strErrorMsg;
            }
            return "Success";
        }
        #endregion
    }
}