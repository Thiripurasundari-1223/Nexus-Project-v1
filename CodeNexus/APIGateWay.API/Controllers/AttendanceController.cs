using APIGateWay.API.Common;
using APIGateWay.API.Model;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries;
using SharedLibraries.Common;
using SharedLibraries.Models.Attendance;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Leaves;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HTTPClient _client;
        private readonly CommonFunction _commonFunction;
        private readonly IConfiguration _configuration;
        private readonly string _attendanceBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _leaveBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";
        private readonly string _appraisalBaseURL = string.Empty;

        #region Constructor
        public AttendanceController(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _commonFunction = new CommonFunction(configuration);
            _configuration = configuration;
            _attendanceBaseURL = _configuration.GetValue<string>("ApplicationURL:Attendance:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _leaveBaseURL = _configuration.GetValue<string>("ApplicationURL:Leaves:BaseURL");
            _appraisalBaseURL = _configuration.GetValue<string>("ApplicationURL:Appraisal:BaseURL");

        }
        #endregion

        #region Add or update Shift       
        [HttpPost]
        [Route("AddOrUpdateShift")]
        public async Task<IActionResult> AddOrUpdateShift(ShiftDetailsView shiftDetailsView)
        {
            int ShiftDetailsId = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(shiftDetailsView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:AddOrUpdateShift"));
                ShiftDetailsId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && result?.StatusCode == "SUCCESS")
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        ShiftDetailsId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/AddOrUpdateShift", JsonConvert.SerializeObject(shiftDetailsView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ShiftDetailsId
            });
        }
        #endregion

        #region Delete Shift
        [HttpGet]
        [Route("DeleteShift")]
        public async Task<IActionResult> DeleteShift(int ShiftDetailsId)
        {
            try
            {
                var Result = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:DeleteShift") + ShiftDetailsId);
                if (Result != null && Result?.StatusCode == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Shift deleted successfully."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/DeleteShift", JsonConvert.SerializeObject(ShiftDetailsId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get All ShiftDetails
        [HttpGet]
        [Route("GetAllShiftDetails")]
        public async Task<IActionResult> GetAllShiftDetails()
        {
            List<ShiftView> shiftView = new();
            try
            {
                var result = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAllShiftDetails"));
                shiftView = JsonConvert.DeserializeObject<List<ShiftView>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    shiftView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/GetAllShiftDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    shiftView
                });
            }
        }
        #endregion

        #region Get ShiftDetails By Id
        [HttpGet]
        [Route("GetShiftDetailsById")]
        public async Task<IActionResult> GetShiftDetailsById(int pShiftDetailsId)
        {
            AttendanceShiftDetailsView attendanceShiftDetailsView = new();
            try
            {
                var result = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftDetailsById") + pShiftDetailsId);
                attendanceShiftDetailsView = JsonConvert.DeserializeObject<AttendanceShiftDetailsView>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = attendanceShiftDetailsView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/GetShiftDetailsById", Convert.ToString(pShiftDetailsId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = attendanceShiftDetailsView
                });
            }
        }
        #endregion

        #region Insert or Update Attendance
        [HttpPost]
        [Route("InsertorUpdateAttendance")]
        public async Task<IActionResult> InsertorUpdateAttendance(AttendanceView Attendance)
        {
            WeeklyMonthlyAttendance attendanceDetails = new();
            List<AppliedLeaveDetailsView> Leaves = new();
            try
            {
                //Attendance.CheckinCheckoutTime = new DateTime(2021 , 11 , 08,09, 10,00);
                int ShiftId = Attendance.ShiftId;
                var shiftDetais = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetails") + ShiftId);
                ShiftDetailedView WeekendList = JsonConvert.DeserializeObject<ShiftDetailedView>(JsonConvert.SerializeObject(shiftDetais?.Data));
                ShiftViewDetails WeekendShiftList = new();
                WeekendShiftList = WeekendList?.shiftViewDetails;
                TimeSpan? utcTime = new TimeSpan(5, 30, 0);
                string[] totalhour = WeekendShiftList?.TotalHours?.Split(":");
                TimeSpan? totalHours = new TimeSpan(totalhour?.Length > 0 ? Convert.ToInt32(totalhour[0]) : 0, totalhour?.Length > 1 ? Convert.ToInt32(totalhour[1]) : 0, totalhour?.Length > 2 ? Convert.ToInt32(totalhour[2]) : 0);
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

                if (timeTo.Value.Ticks >= timeFrom.Value.Ticks) {
                    duration = (timeTo - timeFrom) / 2; }
                TimeSpan? cutOffTime = (duration + timeFrom);
                TimeSpan? utcTimeTo = new TimeSpan(0, 0, 0);
                if (cutOffTime.Value.Ticks >= utcTime.Value.Ticks)
                {
                    utcTimeTo = cutOffTime - utcTime;
                }
                DateTime dtc = new DateTime(utcTimeTo.Value.Ticks);
                DateTime utcCutOff = dtc;
                TimeSpan? cutOffHour = utcCutOff.TimeOfDay;
                TimeSpan? checkinTime = Attendance.CheckinCheckoutTime.Value.TimeOfDay;

                DateTime fromdate = new DateTime(Attendance.Date.Year, Attendance.Date.Month, Attendance.Date.Day);
                //DateTime fromdate = new DateTime(2021, 09, 30);
                var resultdata = await _client.GetAsync(_leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeExistsLeaves") + Attendance.EmployeeId + "&fromDate=" + fromdate.ToString("yyyy-MM-dd"));
                Leaves = JsonConvert.DeserializeObject<List<AppliedLeaveDetailsView>>(JsonConvert.SerializeObject(resultdata?.Data));
                AppliedLeaveDetailsView applyLeaves = new();
                if (Leaves != null && Leaves?.Count > 0)
                {
                    List<AppliedLeaveDetailsView> Leavelist = new();
                    foreach (var item in Leaves)
                    {
                        AppliedLeaveDetailsView leave = new();
                        if (item.IsFirstHalf == true) { leave.IsFirstHalf = true; leave.IsSecondHalf = false; leave.IsLeave = true; }
                        else if (item.IsSecondHalf == true) { leave.IsSecondHalf = true; leave.IsLeave = true; leave.IsFirstHalf = false; }
                        else { leave.IsFirstHalf = false; leave.IsSecondHalf = false; }
                        Leavelist.Add(leave);
                    }
                    bool Leavelists = Leavelist?.Where(x => (bool)x?.IsFirstHalf || (bool)x?.IsSecondHalf).Select(x => x)?.Count() >= 2 ? true : false;
                    if (Leavelists)
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Exists Leave on date( " + Attendance.Date.ToString("dd MMM yyyy") + "). Could not able to check-in.",
                            Data = new WeeklyMonthlyAttendance()
                        });
                    }
                    else if (!Leavelists)
                    {
                        var firstLeave = Leaves?.Select(x => x).FirstOrDefault();
                        if (firstLeave.IsFirstHalf == true)
                        {
                            if (checkinTime <= cutOffHour && Attendance.IsCheckin == true)
                            {
                                return Ok(new
                                {
                                    StatusCode = "FAILURE",
                                    StatusText = "Exists Leave on date( " + Attendance?.Date.ToString("dd MMM yyyy") + "). Could not able to check-in.",
                                    Data = new WeeklyMonthlyAttendance()
                                });
                            }
                            else
                            {
                                var result = await _client.PostAsJsonAsync(Attendance, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:InsertorUpdateAttendance"));
                                attendanceDetails = JsonConvert.DeserializeObject<WeeklyMonthlyAttendance>(JsonConvert.SerializeObject(result?.Data));
                                attendanceDetails.ShiftHour = WeekendShiftList?.TotalHours;
                                if (attendanceDetails?.AttendanceId ==-1)
                                {
                                    return Ok(new
                                    {
                                        result.StatusCode,
                                        StatusText = result.StatusText,
                                        Data = new WeeklyMonthlyAttendance()
                                    });
                                }
                                else if (attendanceDetails?.AttendanceId > 0)
                                {
                                    return Ok(new
                                    {
                                        result.StatusCode,
                                        StatusText = "Attendance saved successfully.",
                                        Data = attendanceDetails
                                    });
                                }
                            }
                        }
                        else if (firstLeave.IsSecondHalf == true)
                        {
                            if (checkinTime >= cutOffHour && Attendance.IsCheckin == true)
                            {
                                return Ok(new
                                {
                                    StatusCode = "FAILURE",
                                    StatusText = "Exists Leave on date( " + Attendance?.Date.ToString("dd MMM yyyy") + "). Could not able to check-in.",
                                    Data = 0
                                });
                            }
                            else
                            {
                                var result = await _client.PostAsJsonAsync(Attendance, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:InsertorUpdateAttendance"));
                                attendanceDetails = JsonConvert.DeserializeObject<WeeklyMonthlyAttendance>(JsonConvert.SerializeObject(result?.Data));
                                attendanceDetails.ShiftHour = WeekendShiftList?.TotalHours;
                                if (attendanceDetails?.AttendanceId > 0)
                                {
                                    return Ok(new
                                    {
                                        result.StatusCode,
                                        StatusText = "Attendance saved successfully.",
                                        Data = attendanceDetails
                                    });
                                }
                            }
                        }
                        else if (firstLeave.IsFullDay == true)
                        {
                            return Ok(new
                            {
                                StatusCode = "FAILURE",
                                StatusText = "Exists Leave on date( " + Attendance?.Date.ToString("dd MMM yyyy") + "). Could not able to check-in.",
                                Data = new WeeklyMonthlyAttendance()
                            });
                        }
                    }
                }


                else
                {
                    var result = await _client.PostAsJsonAsync(Attendance, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:InsertorUpdateAttendance"));
                    attendanceDetails = JsonConvert.DeserializeObject<WeeklyMonthlyAttendance>(JsonConvert.SerializeObject(result?.Data));
                    attendanceDetails.ShiftHour = WeekendShiftList?.TotalHours;

                    if (attendanceDetails?.AttendanceId == -1)
                    {
                        return Ok(new
                        {
                            result.StatusCode,
                            StatusText = result.StatusText,
                            Data = new WeeklyMonthlyAttendance()
                        });
                    }
                    else if(attendanceDetails?.AttendanceId > 0)
                    {
                        return Ok(new
                        {
                            result.StatusCode,
                            StatusText = "Attendance saved successfully.",
                            Data = attendanceDetails
                        });
                    }
                }
                //else
                //{
                //    return Ok(new
                //    {
                //        StatusCode = "FAILURE",
                //        StatusText = "Exists Leave on date( " + Attendance.Date.ToString("dd MMM yyyy") + "). Could not able to check-in.",
                //        Data = 0
                //    });
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/InsertorUpdateAttendance", JsonConvert.SerializeObject(Attendance));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = attendanceDetails
            });
        }
        #endregion

        #region Get Daily Attendance Detail
        [HttpPost]
        [Route("GetDailyAttendanceDetails")]
        public async Task<IActionResult> GetDailyAttendanceDetails(WeekMonthAttendanceView attendance)
        {
            WeeklyMonthlyAttendance AttendanceWithBreakDetail = new();
            try
            {
                var result = await _client.PostAsJsonAsync(attendance, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetDailyAttendanceDetails"));
                AttendanceWithBreakDetail = JsonConvert.DeserializeObject<WeeklyMonthlyAttendance>(JsonConvert.SerializeObject(result?.Data));
                //var shiftDetais = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetails") + ShiftId);
                //ShiftViewDetails WeekendShift = JsonConvert.DeserializeObject<ShiftViewDetails>(JsonConvert.SerializeObject(shiftDetais.Data));
                //AttendanceWithBreakDetail.ShiftHour = WeekendShift?.TotalHours;
                //AttendanceWithBreakDetail.ShiftFromTime = WeekendShift?.ShiftTimeFrom;
                //AttendanceWithBreakDetail.ShiftToTime = WeekendShift?.ShiftTimeTo;
                return Ok(new
                {
                    result.StatusCode,
                    StatusText = "",
                    AttendanceWithBreakDetail
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/GetDailyAttendanceDetails", JsonConvert.SerializeObject(attendance));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                AttendanceWithBreakDetail
            });
        }
        #endregion

        #region Get Weekly and Monthly Attendance Details
        [HttpPost]
        [Route("GetWeeklyAndMonthlyAttendanceDetail")]
        public async Task<IActionResult> GetWeeklyAndMonthlyAttendanceDetail(WeekMonthAttendanceView weekMonthAttendance)
        {
            List<WeeklyMonthlyAttendance> WeeklyAttendanceDetail = new();
            List<EmployeeList> ReportingEmployeeList = new();
            EmployeeShiftDetailView ShiftDetails = new();
            DateTime inputFromDate = weekMonthAttendance.FromDate;
            DateTime inputToDate = weekMonthAttendance.ToDate;
            try
            {
                int employeeId = weekMonthAttendance.EmployeeId;
                DateTime fromDate = weekMonthAttendance.FromDate;
                DateTime toDate = weekMonthAttendance.ToDate;
                int ShiftId = (int)weekMonthAttendance.ShiftDetailsId;
                //var result = await _client.PostAsJsonAsync(weekMonthAttendance, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetWeeklyAndMonthlyAttendanceDetail"));
                //List<WeeklyMonthlyAttendance> WeeklyMarkedAttendance = JsonConvert.DeserializeObject<List<WeeklyMonthlyAttendance>>(JsonConvert.SerializeObject(result?.Data));
                var departmentResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDepartmentAndLocation") + employeeId);
                EmployeeDepartmentAndLocationView employeeDepartments = JsonConvert.DeserializeObject<EmployeeDepartmentAndLocationView>(JsonConvert.SerializeObject(departmentResult?.Data));
                if (employeeDepartments != null)
                {
                    ReportingEmployeeList = employeeDepartments?.ReportingEmployeeList;
                    List<EmployeeList> ShiftList = ReportingEmployeeList?.Where(x => x.ShiftId == null || x.ShiftId == 0).Select(x => x).ToList();
                    if (ShiftList?.Count > 0)
                    {
                        var results = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetDefaultShiftId"));
                        EmployeeShiftDetailsView employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDetailsView>(JsonConvert.SerializeObject(results?.Result.Data));
                        foreach (var item in ShiftList)
                        {
                            item.ShiftId = employeeShift?.ShiftDetailsId;
                        }
                    }
                }
                weekMonthAttendance.IsMail = false;
                weekMonthAttendance.EmployeeDetails = employeeDepartments?.EmployeeDetails;
                var result = await _client.PostAsJsonAsync(weekMonthAttendance, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetWeeklyAndMonthlyAttendanceDetail"));
                WeekMonthAttendanceDetailedView Attendance = JsonConvert.DeserializeObject<WeekMonthAttendanceDetailedView>(JsonConvert.SerializeObject(result?.Data));
                List<WeeklyMonthlyAttendance> WeeklyMarkedAttendance = new();
                WeeklyMarkedAttendance = Attendance?.Attendances;
                AbsentRestrictionView restrictionView = new();
                restrictionView = Attendance?.Restriction;
                if(Attendance.IsApplicable == true)
                {
                    fromDate = weekMonthAttendance.FromDate.AddDays(-7);
                    toDate = weekMonthAttendance.ToDate.AddDays(+7);
                    weekMonthAttendance.FromDate = weekMonthAttendance.FromDate.AddDays(-7);
                    weekMonthAttendance.ToDate = weekMonthAttendance.ToDate.AddDays(+7);
                }
                DateTime DOJ = DateTime.MinValue;
                if (employeeDepartments != null)
                {
                    DOJ = employeeDepartments.DOJ;
                    weekMonthAttendance.DOJ = employeeDepartments.DOJ;
                }
                //var shiftDetails = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftDetails") + ShiftId);
                //EmployeeShiftDetailView ShiftDetailList = JsonConvert.DeserializeObject<EmployeeShiftDetailView>(JsonConvert.SerializeObject(shiftDetails?.Data));
                //var shiftDetais = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetailsList"));
                //List<ShiftViewDetails> WeekendList = JsonConvert.DeserializeObject<List<ShiftViewDetails>>(JsonConvert.SerializeObject(shiftDetais?.Data));
                List<ShiftViewDetails> WeekendList = Attendance?.WeekendShiftDetailList;
                List <EmployeeShiftDetailsView> EmployeeShiftDetails = new();
                EmployeeShiftDetails = employeeDepartments?.EmployeeShiftDetails;
                var holidayResult = await _client.PostAsJsonAsync(weekMonthAttendance, _leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveHolidayByEmployeeId"));
                LeaveHolidayView leaveHoliday = JsonConvert.DeserializeObject<LeaveHolidayView>(JsonConvert.SerializeObject(holidayResult?.Data));
                List<Holiday> financialHolidayList = new();
                if (leaveHoliday?.holidayDetails?.HolidayList?.Count > 0)
                {
                    foreach (var item in leaveHoliday?.holidayDetails?.HolidayList)
                    {
                        int? shiftId = 0;
                        if (EmployeeShiftDetails?.Count > 0)
                        {
                            foreach (EmployeeShiftDetailsView shift in EmployeeShiftDetails)
                            {
                                if (item.HolidayDate >= shift.ShiftFromDate && (shift.ShiftToDate == null || item.HolidayDate <= shift.ShiftToDate))
                                {
                                    shiftId = WeekendList.Where(x => x.ShiftDetailsId == shift.ShiftDetailsId).Select(x => x.ShiftDetailsId).FirstOrDefault();
                                }
                            }
                        }
                        if (shiftId == 0)
                        {
                            shiftId = WeekendList?.Where(x => x.IsGenralShift == true).Select(x => x.ShiftDetailsId).FirstOrDefault();
                            if (shiftId == 0)
                            {
                                shiftId = WeekendList?.Select(x => x.ShiftDetailsId).FirstOrDefault();
                            }
                        }
                        Holiday holiday = await getApplicableHolidayList(item, weekMonthAttendance.DepartmentId, weekMonthAttendance.LocationId, (int)shiftId, leaveHoliday?.holidayDetails);
                        financialHolidayList.Add(holiday);
                    }
                    leaveHoliday.Holiday = financialHolidayList.Where(x => x.HolidayName != null).Select(x => x).ToList();
                }
                List<WeeklyMonthlyAttendance> WeeklyMonthlyAttendance = await _commonFunction.GetAbsentList(WeeklyMarkedAttendance, WeekendList, fromDate, toDate, leaveHoliday, EmployeeShiftDetails,DOJ, restrictionView);
               WeeklyMonthlyAttendance = WeeklyMonthlyAttendance?.Where(x => x.Date >= inputFromDate && x.Date <= inputToDate).Select(x=>x).ToList();
                if (WeeklyMonthlyAttendance != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        StatusText = "",
                        WeeklyAttendanceDetail = WeeklyMonthlyAttendance,
                        ReportingEmployeeList = ReportingEmployeeList == null ? new List<EmployeeList>() : ReportingEmployeeList,
                        ShiftDetails = Attendance?.ShiftDetail == null ? new EmployeeShiftDetailView() : Attendance?.ShiftDetail,
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/GetWeeklyAndMonthlyAttendanceDetail");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                WeeklyAttendanceDetail = WeeklyAttendanceDetail,
                ReportingEmployeeList = ReportingEmployeeList == null ? new List<EmployeeList>() : ReportingEmployeeList

            });
        }
        #endregion

        #region Get All Attendance Details
        [HttpPost]
        [Route("GetAttendanceDetails")]
        public async Task<IActionResult> GetAttendanceDetails(EmployeesAttendanceFilterView employeesAttendanceFilterView)
        {
            EmployeeAttendanceShiftDetailsView attendanceDetails = new();
            List<EmployeeAttendanceDetails> employeeAttendanceDetails = new();
            try
            {
                
                var results = await _client.PostAsJsonAsync(employeesAttendanceFilterView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAttendanceDetails"));
                employeeAttendanceDetails = JsonConvert.DeserializeObject<List<EmployeeAttendanceDetails>>(JsonConvert.SerializeObject(results?.Data));
                //.EmployeeAttendanceDetailsView employeesAttendance = new();
                //employeesAttendance = JsonConvert.DeserializeObject<EmployeeAttendanceDetailsView>(JsonConvert.SerializeObject(results?.Data));
                //employeeAttendanceDetails = employeesAttendance?.AttendanceDetailsList;
                List<int> lstResourceId = employeeAttendanceDetails?.Select(x => x.EmployeeId).ToList();
                ReportingManagerTeamLeaveView managerTeamLeaveView = new();
                if (lstResourceId?.Count > 0)
                {
                    managerTeamLeaveView.ResourceId = lstResourceId;
                    managerTeamLeaveView.FromDate = employeesAttendanceFilterView.FromDate;
                    managerTeamLeaveView.ToDate = employeesAttendanceFilterView.ToDate;
                }
                var result = await _client.PostAsJsonAsync(managerTeamLeaveView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAttendanceDetails"));
                attendanceDetails = JsonConvert.DeserializeObject<EmployeeAttendanceShiftDetailsView>(JsonConvert.SerializeObject(result?.Data));
                employeeAttendanceDetails?.ForEach(x => x.attendanceDetails = attendanceDetails?.EmployeesAttendances?.Where(y => y?.EmployeeId == x.EmployeeId).ToList());
                List<EmployeeAttendanceDetails> employeeAttendanceDetail = new();
                //var shiftResult = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeShiftMasterData"));
                //List<EmployeeShiftMasterData> employeeShiftMasterData = JsonConvert.DeserializeObject<List<EmployeeShiftMasterData>>(JsonConvert.SerializeObject(shiftResult?.Result.Data));
                int? shiftId = 0;
                string shiftName = "";
                if (employeeAttendanceDetails != null)
                {
                    foreach (var items in employeeAttendanceDetails)
                    {
                            shiftId = attendanceDetails?.ShiftViewDetails?.Where(x => x.IsGenralShift == true).Select(x => x.ShiftDetailsId).FirstOrDefault();
                            shiftName = attendanceDetails?.ShiftViewDetails?.Where(x => x.IsGenralShift == true).Select(x => x.ShiftName).FirstOrDefault();
                            if (shiftId == 0 || shiftId == null)
                            {
                                shiftId = attendanceDetails?.ShiftViewDetails?.Select(x => x.ShiftDetailsId).FirstOrDefault();
                                shiftName = attendanceDetails?.ShiftViewDetails?.Select(x => x.ShiftName).FirstOrDefault();
                            }
                        EmployeeAttendanceDetails AttendanceDetails = new()
                        {

                            EmployeeId = items.EmployeeId,
                            FormattedEmployeeId = items.FormattedEmployeeId,
                            EmployeeName = items.EmployeeName,
                            EmployeeEmailId = items.EmployeeEmailId,
                            Department = items.Department,
                            Date = employeesAttendanceFilterView.FromDate,
                            Location = items.Location,
                            Designation = items.Designation,
                            //TotalHours = items.attendanceDetails.Count() == 0 ? "00:00" : items.attendanceDetails.First().TotalHours,
                            //BreakHours = items.attendanceDetails.Count() == 0 ? "00:00" : items.attendanceDetails.First().BreakHours,
                            TotalHours = items.attendanceDetails.First().TotalHours,
                            BreakHours = items.attendanceDetails.First().BreakHours,
                            //ShiftDetailId = items.ShiftDetailId,
                            //ShiftName = employeeShiftMasterData != null ? employeeShiftMasterData?.Find(x => x.ShiftDetailsId == items?.ShiftDetailId)?.ShiftName : string.Empty,
                            ShiftDetailId = items.ShiftDetailId ==  null ? shiftId : items.ShiftDetailId,
                            ShiftName = attendanceDetails?.ShiftViewDetails != null ? attendanceDetails?.ShiftViewDetails?.Where(x => x.ShiftDetailsId == items?.ShiftDetailId).Select(x=>x.ShiftName).FirstOrDefault() == null ? shiftName :
                            attendanceDetails?.ShiftViewDetails?.Where(x => x.ShiftDetailsId == items?.ShiftDetailId).Select(x => x.ShiftName).FirstOrDefault():string.Empty,
                        };
                        employeeAttendanceDetail?.Add(AttendanceDetails);
                    }
                }
                employeeAttendanceDetail = employeeAttendanceDetail?.OrderBy(x => x.EmployeeName).ToList();
                //employeesAttendance.AttendanceDetailsList = employeeAttendanceDetail;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeAttendanceDetail,
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeeAttendanceDetails>()
                });
            }
        }
        #endregion

        #region Get Details
        [HttpPost]
        [Route("GetAttendanceDetailByEmployeeId")]
        public async Task<IActionResult> GetAttendanceDetailByEmployeeId(AttendanceWeekView attendanceWeekView)//(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<DetailsView> detailsView = new();
            try
            {

                var result = await _client.PostAsJsonAsync(attendanceWeekView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAttendanceDetailByEmployeeId"));
                detailsView = JsonConvert.DeserializeObject<List<DetailsView>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    detailsView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/GetAttendanceDetailByEmployeeId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    detailsView
                });
            }
        }
        #endregion

        #region Insert or Update Attendance TimeLog
        [HttpPost]
        [Route("InsertorUpdateTimeLog")]
        public async Task<IActionResult> InsertorUpdateTimeLog(TimelogView timelogView)
        {
            WeeklyMonthlyAttendance attendanceDetails = new();
            try
            {
                var result = await _client.PostAsJsonAsync(timelogView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:InsertorUpdateTimeLog"));
                attendanceDetails = JsonConvert.DeserializeObject<WeeklyMonthlyAttendance>(JsonConvert.SerializeObject(result?.Data));
                if (attendanceDetails?.AttendanceId > 0)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        StatusText = "TimeLog saved successfully.",
                        Data = attendanceDetails
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/InsertorUpdateTimeLog", JsonConvert.SerializeObject(timelogView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = attendanceDetails
            });
        }
        #endregion

        #region Get Employee Attendance Details By Employee Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmployeeAttendanceDetailsByEmployeeId")]
        public async Task<IActionResult> GetEmployeeAttendanceDetailsByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<EmployeesAttendanceDetails> attendanceDetails = new();
            try
            {
                var results = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeAttendanceDetailsByEmployeeId") + employeeId + "&FromDate=" + fromDate.ToString("yyyy-MM-dd") + "&ToDate=" + toDate.ToString("yyyy-MM-dd"));
                attendanceDetails = JsonConvert.DeserializeObject<List<EmployeesAttendanceDetails>>(JsonConvert.SerializeObject(results?.Data));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = attendanceDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeesAttendanceDetails>()
                });
            }
        }
        #endregion

        #region Insert or Update Attendance
        [HttpPost]
        [Route("InsertorUpdateAttendanceDetailTimeLog")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertorUpdateAttendanceDetailTimeLog(TimeLogDetailView timelogView)
        {
            int AttendanceDetailID = 0;
            List<AppliedLeaveDetailsView> applyLeaves = new();
            try
            {
                // ---For Mail purpose.
                EmployeeandManagerView employeeandManager = new EmployeeandManagerView();
                var employeeID = timelogView?.EmployeeId;
                string statusText = "", MailSubject = null, MailBody = null, Subject = null, Body = null;
                var empresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeID);
                employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(empresults?.Data));
                timelogView.ApproverManagerId = employeeandManager?.ReportingManagerID == null ? 0 : (int)employeeandManager?.ReportingManagerID;
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                SendEmailView sendMailbyleaverequest = new();

                TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime localcheckintime = TimeZoneInfo.ConvertTimeFromUtc(timelogView.CheckinTime, zone);
                TimeSpan? iSTcheckinTime = localcheckintime.TimeOfDay;
                DateTime iSTlocalcheckouttime = TimeZoneInfo.ConvertTimeFromUtc(timelogView.CheckoutTime, zone);
                TimeSpan? iSTcheckoutTime = iSTlocalcheckouttime.TimeOfDay;

                var CheckinTotalHrs = timelogView.CheckoutTime - timelogView.CheckinTime;
                //DateTime TotalHrscheckin = new DateTime(CheckinTotalHrs.Ticks);
                string TotalHrs = CheckinTotalHrs.ToString("hh\\:mm");
                var checkindateTime = new DateTime(iSTcheckinTime.Value.Ticks); // Date part is 01-01-0001
                var formattedCheckinTime = checkindateTime.ToString("h:mm tt", CultureInfo.InvariantCulture);
                var checkoutdateTime = new DateTime(iSTcheckoutTime.Value.Ticks); // Date part is 01-01-0001
                var formattedCheckOutTime = checkoutdateTime.ToString("h:mm tt", CultureInfo.InvariantCulture);

                DateTime fromdate = new DateTime(timelogView.Date.Year, timelogView.Date.Month, timelogView.Date.Day);
               var date = fromdate.ToShortDateString();
                //int employeeId = timelogView.EmployeeId;
                //int ShiftId = timelogView.ShiftId;
                //var shiftResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetShifByDate") + employeeId + "&date=" + fromdate.ToString("yyyy-MM-dd"));
                //EmployeeShiftDetails employeeShift = JsonConvert.DeserializeObject<EmployeeShiftDetails>(JsonConvert.SerializeObject(shiftResult?.Data));
                //int shiftId = (employeeShift?.ShiftDetailsId == null ? 0 : (int)employeeShift?.ShiftDetailsId);
                //var shiftDetais = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetails") + shiftId);
                //ShiftDetailedView WeekendList = JsonConvert.DeserializeObject<ShiftDetailedView>(JsonConvert.SerializeObject(shiftDetais?.Data));
                //ShiftViewDetails WeekendShiftList = new();
                //WeekendShiftList = WeekendList?.shiftViewDetails;
                //TimeSpan? utcTime = new TimeSpan(5, 30, 0);
                //string[] totalhour = WeekendShiftList?.TotalHours?.Split(":");
                //TimeSpan? totalHours = new TimeSpan(totalhour?.Length > 0 ? Convert.ToInt32(totalhour[0]) : 0, totalhour?.Length > 1 ? Convert.ToInt32(totalhour[1]) : 0, totalhour?.Length > 2 ? Convert.ToInt32(totalhour[2]) : 0);
                //string[] fromHour = WeekendShiftList?.TimeFrom?.Split(":");
                //TimeSpan? timeFrom = new TimeSpan(fromHour?.Length > 0 ? Convert.ToInt32(fromHour[0]) : 0, fromHour?.Length > 1 ? Convert.ToInt32(fromHour[1]) : 0, fromHour?.Length > 2 ? Convert.ToInt32(fromHour[2]) : 0);
                //string[] toHour = WeekendShiftList?.TimeTo?.Split(":");
                //TimeSpan? timeTo = new TimeSpan(toHour?.Length > 0 ? Convert.ToInt32(toHour[0]) : 0, toHour?.Length > 1 ? Convert.ToInt32(toHour[1]) : 0, toHour?.Length > 2 ? Convert.ToInt32(toHour[2]) : 0);
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
                //TimeSpan? checkinTime = timelogView.CheckinTime.TimeOfDay;
                //TimeSpan? checkoutTime = timelogView.CheckoutTime.TimeOfDay;
                string checkindate = timelogView.Date.ToString("dd MMM yyyy");
                string checkintime = Convert.ToString(formattedCheckinTime);
                string checkouttime = Convert.ToString(formattedCheckOutTime);
                //email details

                Notifications notification = new Notifications();
                notification = new Notifications
                {

                    CreatedBy = timelogView?.EmployeeId == null ? 0 : (int)timelogView?.EmployeeId,
                    CreatedOn = DateTime.UtcNow,
                    FromId = timelogView?.EmployeeId == null ? 0 : (int)timelogView?.EmployeeId,
                    ToId = employeeandManager?.ReportingManagerID == null ? 0 : (int)employeeandManager?.ReportingManagerID,
                    MarkAsRead = false,
                    NotificationSubject = "New Regularization request from " + employeeandManager?.EmployeeName + ".",
                    NotificationBody = employeeandManager?.EmployeeName + "'s " + checkindate + " regularization request is waiting for your approval.",
                    PrimaryKeyId = timelogView?.AttendanceId,
                    ButtonName = "Approve Regularization",
                    SourceType = "Regularization"
                };
                Notifications notificationForEmployee = new Notifications();
                notificationForEmployee = new Notifications
                {

                    CreatedBy = timelogView?.EmployeeId == null ? 0 : (int)timelogView?.EmployeeId,
                    CreatedOn = DateTime.UtcNow,
                    FromId = timelogView?.EmployeeId == null ? 0 : (int)timelogView?.EmployeeId,
                    ToId = timelogView?.EmployeeId == null ? 0 : (int)timelogView?.EmployeeId,
                    MarkAsRead = false,
                    NotificationSubject = "Regularization sent for approval.",
                    NotificationBody = "Your regularization request for " + checkindate + " has been sent for approval.",
                    PrimaryKeyId = timelogView?.AttendanceId,
                    ButtonName = "View Regularization",
                    SourceType = "Regularization"
                };
                SendEmailView sendMailDetails = new();
                string baseURL = appsetting.GetSection("BaseURL").Value;
                string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#FFA93E'  style='text-align:center';><td><b>Check-In</b></td><td><b>Check-Out</b></td><td><b>Logged Hours</b></td></tr>";
                textBody += "<tr style='text-align:center';><td >" + checkintime + "</td><td > " + checkouttime + "</td><td >" + (TotalHrs + " Hrs").ToString() + "</td></tr></table>";
                MailSubject = "{EmployeeName} sent request for Regularization.";
                MailBody = @"<html>
                                    <body>                                    
                                    <p>Dear {ManagerName},</p>                                    
                                    <p>{EmployeeName} applied Regularization on {FromDate}. Please click <a href='{link}/#/pmsnexus/workday?isManager=true&RequestType=Attendance'>here</a> to Approve/Reject. </p>
                                    </br>                                 
                                    <div>{table}</div>  
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>
                                    </html>";
                MailSubject = MailSubject.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                MailBody = MailBody.Replace("{ManagerName}", employeeandManager?.ManagerName);
                MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                MailBody = MailBody.Replace("{FromDate}", checkindate);
                MailBody = MailBody.Replace("{table}", textBody);
                MailBody = MailBody.Replace("{Feedback}", timelogView?.Reason);
                MailBody = MailBody.Replace("{link}", baseURL);
                sendMailDetails = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = employeeandManager?.ManagerEmailID,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = employeeandManager?.EmployeeEmailID
                };


                var resultdata = await _client.GetAsync(_leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeExistsLeaves") + timelogView.EmployeeId + "&fromDate=" + fromdate.ToString("yyyy-MM-dd"));
                applyLeaves = JsonConvert.DeserializeObject<List<AppliedLeaveDetailsView>>(JsonConvert.SerializeObject(resultdata?.Data));
                if (applyLeaves != null && applyLeaves?.Count > 0 && applyLeaves[0]?.IsFullDay==true)
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Exists Leave on date " + date + ". Please try again.",
                        Data = 0
                    });

                    //List<AppliedLeaveDetailsView> Leavelist = new();
                    //foreach (var item in applyLeaves)
                    //{
                    //    AppliedLeaveDetailsView leave = new();
                    //    if (item.IsFirstHalf == true) { leave.IsFirstHalf = true; leave.IsSecondHalf = false; leave.IsLeave = true; }
                    //    else if (item.IsSecondHalf == true) { leave.IsSecondHalf = true; leave.IsLeave = true; leave.IsFirstHalf = false; }
                    //    else { leave.IsFirstHalf = false; leave.IsSecondHalf = false; }
                    //    Leavelist.Add(leave);
                    //}
                    //bool Leavelists = Leavelist?.Where(x => (bool)x?.IsFirstHalf || (bool)x?.IsSecondHalf).Select(x => x)?.Count() >= 2 ? true : false;
                    //if (Leavelists)
                    //{
                    //    return Ok(new
                    //    {
                    //        StatusCode = "FAILURE",
                    //        StatusText = "Exists Leave. Please try again.",
                    //        Data = 0
                    //    });
                    //}
                    //if (!Leavelists)
                    //{
                    //    var firstLeave = applyLeaves?.Select(x => x).FirstOrDefault();

                    //    if (firstLeave.IsFirstHalf == true)
                    //    {
                    //        if (checkinTime <= cutOffHour)
                    //        {
                    //            return Ok(new
                    //            {
                    //                StatusCode = "FAILURE",
                    //                StatusText = "Exists Leave. Please try again.",
                    //                Data = 0
                    //            });
                    //        }
                    //        else
                    //        {
                    //            var result = await _client.PostAsJsonAsync(timelogView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:InsertorUpdateAttendanceDetailTimeLog"));
                    //            AttendanceDetailID = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                    //            if (AttendanceDetailID > 0)
                    //            {
                    //                if (timelogView?.RegStatus?.ToLower() == "pending" || timelogView?.RegStatus?.ToLower() == "")
                    //                {
                    //                    string mail = AttendanceDetailTimeLogNotificationMail(notification, sendMailDetails, notificationForEmployee).Result;
                    //                }
                    //                return Ok(new
                    //                {
                    //                    StatusCode = "SUCCESS",
                    //                    StatusText = "Attendance timelog details saved successfully.",
                    //                    Data = AttendanceDetailID
                    //                });
                    //            }
                    //            else if (AttendanceDetailID == -1)
                    //            {
                    //                return Ok(new
                    //                {
                    //                    StatusCode = "FAILURE",
                    //                    StatusText = "Exists Attendance timelog details. Please try again.",
                    //                    Data = 0
                    //                });
                    //            }
                    //            else if (AttendanceDetailID == -2)
                    //            {
                    //                return Ok(new
                    //                {
                    //                    StatusCode = "FAILURE",
                    //                    StatusText = "System won't allow to apply time log for today/future date.",
                    //                    Data = 0
                    //                });
                    //            }
                    //            else if (AttendanceDetailID == -3)
                    //            {
                    //                return Ok(new
                    //                {
                    //                    StatusCode = "FAILURE",
                    //                    StatusText = " Timelog exceeds more than 24 hours.",
                    //                    Data = -3
                    //                });
                    //            }
                    //        }
                    //    }
                    //    else if (firstLeave.IsSecondHalf == true)
                    //    {
                    //        if (checkoutTime >= cutOffHour)
                    //        {
                    //            return Ok(new
                    //            {
                    //                StatusCode = "FAILURE",
                    //                StatusText = "Exists Leave. Please try again.",
                    //                Data = 0
                    //            });
                    //        }
                    //        else
                    //        {
                    //            var result = await _client.PostAsJsonAsync(timelogView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:InsertorUpdateAttendanceDetailTimeLog"));
                    //            AttendanceDetailID = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                    //            if (AttendanceDetailID > 0)
                    //            {
                    //                if (timelogView?.RegStatus?.ToLower() == "pending" || timelogView?.RegStatus?.ToLower() == "")
                    //                {
                    //                    string mail = AttendanceDetailTimeLogNotificationMail(notification, sendMailDetails, notificationForEmployee).Result;
                    //                }
                    //                return Ok(new
                    //                {
                    //                    StatusCode = "SUCCESS",
                    //                    StatusText = "Attendance timelog details saved successfully.",
                    //                    Data = AttendanceDetailID
                    //                });
                    //            }
                    //            else if (AttendanceDetailID == -1)
                    //            {
                    //                return Ok(new
                    //                {
                    //                    StatusCode = "FAILURE",
                    //                    StatusText = "Exists Attendance timelog details. Please try again.",
                    //                    Data = 0
                    //                });
                    //            }
                    //            else if (AttendanceDetailID == -2)
                    //            {
                    //                return Ok(new
                    //                {
                    //                    StatusCode = "FAILURE",
                    //                    StatusText = "System won't allow to apply time log for today/future date.",
                    //                    Data = 0
                    //                });
                    //            }
                    //            else if (AttendanceDetailID == -3)
                    //            {
                    //                return Ok(new
                    //                {
                    //                    StatusCode = "FAILURE",
                    //                    StatusText = "Timelog exceeds more than 24 hours.",
                    //                    Data = -3
                    //                });
                    //            }
                    //        }
                    //    }
                    //    else if (firstLeave.IsFullDay == true)
                    //    {
                    //        return Ok(new
                    //        {
                    //            StatusCode = "FAILURE",
                    //            StatusText = "Exists Leave on date " + date + ". Please try again.",
                    //            Data = 0
                    //        });
                    //    }
                    //}
                }
                else
                {
                    var result = await _client.PostAsJsonAsync(timelogView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:InsertorUpdateAttendanceDetailTimeLog"));
                    AttendanceDetailID = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                    if (AttendanceDetailID > 0)
                    {
                        if (timelogView?.RegStatus?.ToLower() == "pending" || timelogView?.RegStatus?.ToLower() == "")
                        {
                            string mail = AttendanceDetailTimeLogNotificationMail(notification, sendMailDetails, notificationForEmployee).Result;
                        }
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "Attendance timelog details saved successfully.",
                            Data = AttendanceDetailID
                        });
                    }
                    else if (AttendanceDetailID == -1)
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Exists Attendance timelog details. Please try again.",
                            Data = 0
                        });
                    }
                    else if (AttendanceDetailID == -2)
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "System won't allow to apply time log for today/future date.",
                            Data = 0
                        });
                    }
                    else if (AttendanceDetailID == -3)
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Timelog exceeds more than 24 hours.",
                            Data = -3
                        });
                    }
                }
                //}
                //else
                //{
                //    return Ok(new
                //    {
                //        StatusCode = "FAILURE",
                //        StatusText = "Exists Leave on date " + date + ". Could not able to regularize timelog.",
                //        Data = 0
                //    });
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/InsertorUpdateAttendance", JsonConvert.SerializeObject(timelogView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = AttendanceDetailID
            });
        }
        #endregion
        #region Attendance Detail TimeLog Notification Mail
        [NonAction]
        public async Task<string> AttendanceDetailTimeLogNotificationMail(Notifications notification, SendEmailView sendMailDetails, Notifications notificationForEmployee)
        {
            try
            {
                string statusText = "", MailSubject = null, MailBody = null, Subject = null, Body = null;
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                SendEmailView sendMailbyleaverequest = new();
                //Notification Alert                                   
                List<Notifications> notifications = new List<Notifications>();
                Notifications managerNotification = new Notifications();
                Notifications empNotification = new Notifications();
                managerNotification = new Notifications
                {

                    CreatedBy = notification.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    FromId =notification.FromId,
                    ToId = notification.ToId,
                    MarkAsRead = false,
                    NotificationSubject = notification.NotificationSubject,
                    NotificationBody = notification.NotificationBody,
                    PrimaryKeyId = notification.PrimaryKeyId,
                    ButtonName = notification.ButtonName,
                    SourceType = notification.SourceType
                };
                notifications.Add(managerNotification);
                empNotification = new Notifications
                {
                    CreatedBy = notificationForEmployee.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    FromId = notificationForEmployee.FromId,
                    ToId = notificationForEmployee.ToId,
                    MarkAsRead = false,
                    NotificationSubject = notificationForEmployee.NotificationSubject,
                    NotificationBody = notificationForEmployee.NotificationBody,
                    PrimaryKeyId = notificationForEmployee.PrimaryKeyId,
                    ButtonName = notificationForEmployee.ButtonName,
                    SourceType = notificationForEmployee.SourceType
                };
                notifications.Add(empNotification);
                using var notificationClient = new HttpClient
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                };
                HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();
                if (notificationResponse?.IsSuccessStatusCode == false)
                {
                    statusText = notificationResult?.Result?.StatusText;
                }

                // Mail 
                sendMailbyleaverequest = new()
                {
                    FromEmailID = sendMailDetails.FromEmailID,
                    ToEmailID = sendMailDetails.ToEmailID,
                    Subject = sendMailDetails.Subject,
                    MailBody = sendMailDetails.MailBody,
                    ResourceEmail = sendMailDetails.ResourceEmail,
                    Port = sendMailDetails.Port,
                    Host = sendMailDetails.Host,
                    FromEmailPassword = sendMailDetails.FromEmailPassword,
                    CC = sendMailDetails.CC
                };

                SendEmail.Sendmail(sendMailbyleaverequest);
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeTimeandWeekendDefinitionbyShiftID");
               
            }
            return string.Empty;
        }
        #endregion
        #region Get Employee TimeandWeekend Definition by ShiftID
        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmployeeTimeandWeekendDefinitionbyShiftID")]
        public async Task<IActionResult> GetEmployeeTimeandWeekendDefinitionbyShiftID(int ShiftDetailsId)
        {
            TimeandWeekendDefinitionView timeandweekdetails = new();
            try
            {
                var results = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeTimeandWeekendDefinitionbyShiftID") + ShiftDetailsId);
                timeandweekdetails = JsonConvert.DeserializeObject<TimeandWeekendDefinitionView>(JsonConvert.SerializeObject(results?.Data));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timeandweekdetails,
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeTimeandWeekendDefinitionbyShiftID");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new TimeandWeekendDefinitionView()
                });
            }
        }
        #endregion

        #region Insert or Update Attendance
        [HttpPost]
        [Route("TimeLogApproveOrReject")]
        [AllowAnonymous]
        public async Task<IActionResult> TimeLogApproveOrReject(TimeLogApproveOrRejectView timeLogApproveOrRejectView)
        {
           // string StatusText = "Unexpected error occurred. Try again.";
            int AttendanceDetailID = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(timeLogApproveOrRejectView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:TimeLogApproveOrReject"));
                AttendanceDetailID = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (AttendanceDetailID > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Attendance timelog details Approved successfully.",
                        Data = AttendanceDetailID
                    });
                }
                else if (AttendanceDetailID == -1)
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Rejected/Cancelled Attendance timelog details successfully.",
                        Data = -1
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/TimeLogApproveOrReject", JsonConvert.SerializeObject(timeLogApproveOrRejectView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = AttendanceDetailID
            });
        }
        #endregion

        #region Delete Regularization 
        [HttpGet]
        [Route("DeleteRegularizationByAttendanceDetailId")]
        public async Task<IActionResult> DeleteRegularizationByAttendanceDetailId(int attendanceDetailId)
        {
            try
            {
                var Result = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:DeleteRegularizationByAttendanceDetailId") + attendanceDetailId);
                if (Result != null && Result?.StatusCode == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Regularization deleted successfully.",
                        data = true
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/DeleteRegularizationByAttendanceDetailId", JsonConvert.SerializeObject(attendanceDetailId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = false
            });
        }
        #endregion
        #region Update Shift Status 
        [HttpGet]
        [Route("UpdateShiftStatus")]
        public async Task<IActionResult> UpdateShiftStatus(int shiftId, bool isEnabled, int updatedBy)
        {
            try
            {
                var result = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:UpdateShiftStatus") + shiftId + "&isEnabled=" + isEnabled + "&updatedBy=" + updatedBy);
                bool isUpdate = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = isUpdate
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/UpdateShiftStatus");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion
        #region Absent Notification
        [HttpGet] 
        [Route("GetAbsentNotification")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAbsentNotification()
        {
            try
            {
                //int employeeId = 0;
                DateTime date = DateTime.Now.AddDays(-1);
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAbsentNotificationEmployeeList"));
                List<EmployeeList> employeeListView = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(employeeResult?.Data));
                var shiftDetais = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetailsList"));
                List<ShiftViewDetails> WeekendList = JsonConvert.DeserializeObject<List<ShiftViewDetails>>(JsonConvert.SerializeObject(shiftDetais?.Data));
             
                //Test Notification
                //employeeListView = employeeListView.Where(x => (x.EmployeeId == 126 || x.EmployeeId == 263 || x.EmployeeId == 121
                //|| x.EmployeeId == 152 || x.EmployeeId == 247 || x.EmployeeId == 122 || x.EmployeeId == 142)).Select(x => x).ToList();
               // List<EmployeeList> exceptEmployeeList = employeeListView.Where(x => (x.EmployeeId == 6 || x.EmployeeId == 27 || x.EmployeeId == 233
               //|| x.EmployeeId == 8 || x.EmployeeId== 145 || x.EmployeeId== 798)).Select(x => x).ToList();

               // employeeListView = employeeListView.Except(exceptEmployeeList).ToList();
                if (employeeListView?.Count > 0)
                {
                    foreach (EmployeeList item in employeeListView)
                    {
                        WeeklyMonthlyAttendance AttendanceDetail = new();
                        WeekMonthAttendanceView weekMonthAttendance = new();
                        DateTime DOJ = DateTime.MinValue;
                            weekMonthAttendance.EmployeeId = item.EmployeeId;
                            weekMonthAttendance.FromDate = date.Date;
                            weekMonthAttendance.ToDate = date.Date;
                            DOJ = item.DOJ == null ? DateTime.MinValue.Date : (DateTime)item.DOJ;
                            weekMonthAttendance.DepartmentId = item.DepartmentId == null ? 0 : (int)item.DepartmentId;
                            weekMonthAttendance.LocationId = item.LocationId == null ? 0 : (int)item.LocationId;
                        weekMonthAttendance.IsMail = true;
                        if (item.employeeShiftDetails?.Count > 0)
                            {
                                foreach (EmployeeShiftDetailsView shift in item.employeeShiftDetails)
                                {
                                    if (date >= shift.ShiftFromDate && (shift.ShiftToDate == null || date <= shift.ShiftToDate))
                                    {
                                        weekMonthAttendance.ShiftDetailsId = WeekendList?.Where(x => x.ShiftDetailsId == shift.ShiftDetailsId).Select(x => x.ShiftDetailsId).FirstOrDefault();
                                    }
                                }
                            }
                            if (weekMonthAttendance.ShiftDetailsId == 0 || weekMonthAttendance.ShiftDetailsId == null)
                            {
                                weekMonthAttendance.ShiftDetailsId = WeekendList?.Where(x => x.IsGenralShift == true).Select(x => x.ShiftDetailsId).FirstOrDefault();
                                if (weekMonthAttendance.ShiftDetailsId == 0)
                                {
                                    weekMonthAttendance.ShiftDetailsId = WeekendList?.Select(x => x.ShiftDetailsId).FirstOrDefault();
                                }
                            }
                            DateTime fromDate = date.Date;
                            DateTime toDate = date.Date;
                            var result = await _client.PostAsJsonAsync(weekMonthAttendance, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetWeeklyAttendanceDetailByDate"));
                        List<WeeklyMonthlyAttendance> WeeklyMarkedAttendance = JsonConvert.DeserializeObject<List<WeeklyMonthlyAttendance>>(JsonConvert.SerializeObject(result?.Data));
                        //List<WeeklyMonthlyAttendance> WeeklyMarkedAttendance = new();
                        //WeeklyMarkedAttendance = Attendance?.Attendances;
                        AbsentRestrictionView restrictionView = null;
                        //restrictionView = Attendance?.Restriction;
                        List<EmployeeShiftDetailsView> EmployeeShiftDetails = new();
                            EmployeeShiftDetails = item?.employeeShiftDetails;
                            var holidayResult = await _client.PostAsJsonAsync(weekMonthAttendance, _leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetLeaveHolidayByEmployeeId"));
                            LeaveHolidayView leaveHoliday = JsonConvert.DeserializeObject<LeaveHolidayView>(JsonConvert.SerializeObject(holidayResult?.Data));
                            List<WeeklyMonthlyAttendance> WeeklyMonthlyAttendance = await _commonFunction.GetAbsentList(WeeklyMarkedAttendance, WeekendList, fromDate, toDate, leaveHoliday, EmployeeShiftDetails, DOJ, restrictionView);

                        if (WeeklyMonthlyAttendance != null && WeeklyMonthlyAttendance?.Count > 0)
                        {
                                AttendanceDetail = WeeklyMonthlyAttendance.Select(x => x).FirstOrDefault();
                                if(AttendanceDetail?.Remark?.ToLower() == "Full Day Absent".ToLower() || (AttendanceDetail?.Remark?.ToLower() == "Half Day Absent".ToLower() && ( AttendanceDetail?.IsSecondHalfAbsent == true || AttendanceDetail?.IsFirstHalfAbsent ==true)))
                                {
                                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                                    string statusText = "", MailSubject = null, MailBody = null, Subject = null, Body = null;
                                    Notifications notificationForEmployee = new();
                                string notificationBody = string.Empty; string mailBody = string.Empty;
                                if(AttendanceDetail?.Remark?.ToLower() == "Full Day Absent".ToLower())
                                {
                                    notificationBody = "You have been marked as full day Absent on " + date.Date.ToString("dd MMM yyyy") + ".";
                                    mailBody = @"<html>
                                    <body>                                    
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>&emsp; &nbsp; You have been marked as full day Absent on {FromDate}.</p>
                                    </br></br></br>                              
                                    <p>&emsp; &nbsp; Automated mail from <a href='{link}'>{link}</a></p>
                                    </br>                                 
                                    </body>
                                    </html>";
                                }
                                else 
                                {
                                    notificationBody = "You have been marked as half day Absent on " + date.Date.ToString("dd MMM yyyy") + ".";
                                    mailBody = @"<html>
                                    <body>                                    
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>&emsp; &nbsp; You have been marked as half day Absent on {FromDate}.</p>
                                    </br></br></br>                              
                                    <p>&emsp; &nbsp; Automated mail from <a href='{link}'>{link}</a></p> 
                                    </br>                                 
                                    </body>
                                    </html>";
                                }
                                    notificationForEmployee = new Notifications
                                    {

                                        CreatedBy = item?.EmployeeId == null ? 0 : (int)item?.EmployeeId,
                                        CreatedOn = DateTime.UtcNow,
                                        FromId = item?.EmployeeId == null ? 0 : (int)item?.EmployeeId,
                                        ToId = item?.EmployeeId == null ? 0 : (int)item?.EmployeeId,
                                        MarkAsRead = false,
                                        NotificationSubject = "Absent Notification.",
                                        NotificationBody = notificationBody,
                                        PrimaryKeyId = AttendanceDetail?.AttendanceId,
                                        ButtonName = "View ",
                                        SourceType = "Attendance"
                                    };
                                    string employeeNotification = _commonFunction.Notification(notificationForEmployee).Result;
                                    SendEmailView sendMailDetails = new();
                                    string baseURL = appsetting.GetSection("BaseURL").Value;
                                    MailSubject = "Absent Notification";
                                    MailBody = mailBody;
                                    MailSubject = MailSubject.Replace("{EmployeeName}", item?.EmployeeName);
                                    MailBody = MailBody.Replace("{FromDate}", date.Date.ToString("dd MMM yyyy"));
                                    MailBody = MailBody.Replace("{EmployeeName}", item?.EmployeeName);
                                    MailBody = MailBody.Replace("{link}", baseURL);
                                sendMailDetails = new()
                                    {
                                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                        ToEmailID = item?.EmployeeEmailId,
                                        Subject = MailSubject,
                                        MailBody = MailBody,
                                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                        Host = appsetting.GetSection("EmailHost").Value,
                                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                                    };
                                    string mail = _commonFunction.NotificationMail(sendMailDetails).Result;
                                }
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAbsentNotification");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                });
            }
        }

        #endregion
        [NonAction]
        public async Task<Holiday> getApplicableHolidayList(Holiday holidayList, int departmentId, int locationId, int shiftId, HolidayDetailsView holidayDetails)
        {
            Holiday empHolidayList = new Holiday();
            if (holidayList != null)
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
        #region Insert Or Update Absent Setting
        [HttpPost]
        [Route("InsertAndUpdateAbsentSetting")]
        public async Task<IActionResult> InsertAndUpdateAbsentSetting(AbsentSettingsView absentSettingsView)
        {
            int absentSettingId = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(absentSettingsView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:InsertAndUpdateAbsentSetting"));
                absentSettingId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        absentSettingId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/InsertAndUpdateAbsentSetting", JsonConvert.SerializeObject(absentSettingsView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                absentSettingId
            });
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetAbsentSettingDetails")]
        public async Task<IActionResult> GetAbsentSettingDetails()
        {
            AbsentSettingsView absentSettingsView = new();
            try
            {
                var result = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAbsentSettingDetails"));
                absentSettingsView = JsonConvert.DeserializeObject<AbsentSettingsView>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = absentSettingsView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAbsentSettingDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = absentSettingsView
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetAttendanceMasterData")]
        public IActionResult GetAttendanceMasterData()
        {
            try
            {
                //AttendanceMasterDataView attendanceMasterData = new();
                LeavesMasterDataView LeavesEmployeeMasterDataView = new();
                var result1 = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetLeaveEmployeeMasterData"));
                LeavesEmployeeMasterDataView = JsonConvert.DeserializeObject<LeavesMasterDataView>(JsonConvert.SerializeObject(result1?.Result?.Data));
                AttendanceMasterDataView attendanceMasterData = new()
                {
                    ProbationStatusList = LeavesEmployeeMasterDataView?.ProbationStatusList,
                    DepartmentList = LeavesEmployeeMasterDataView?.EmployeeDepartmentList,
                    EmployeeTypeList = LeavesEmployeeMasterDataView?.EmployeeTypeList,
                    RoleNamesList = LeavesEmployeeMasterDataView?.RoleNamesList,
                    EmployeeList = LeavesEmployeeMasterDataView?.EmployeeList,
                    LocationList = LeavesEmployeeMasterDataView?.EmployeeLocationList,
                    DesignationList = LeavesEmployeeMasterDataView?.EmployeeDesignationList,
                };
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = attendanceMasterData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new AttendanceMasterDataView()
                });
            }
        }
        #endregion
        #region Get All ShiftDetails
        [HttpGet]
        [Route("GetAllShiftIdWithName")]
        public async Task<IActionResult> GetAllShiftIdWithName()
        {
            List<EmployeeShiftMasterData> employeeShiftMasterData= new();
            try
            {
                var shiftResult = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeShiftMasterData"));
                employeeShiftMasterData = JsonConvert.DeserializeObject<List<EmployeeShiftMasterData>>(JsonConvert.SerializeObject(shiftResult?.Result.Data));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data= employeeShiftMasterData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Attendance/GetAllShiftIdWithName");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data= employeeShiftMasterData
                });
            }
        }
        #endregion

        #region Get All Attendance Details
        [HttpPost]
        [Route("GetAttendanceDetailsCount")]
        public async Task<IActionResult> GetAttendanceDetailsCount(EmployeesAttendanceFilterView employeesAttendanceFilterView)
        {
            int totalCount = 0;
            try
            {

                var results = await _client.PostAsJsonAsync(employeesAttendanceFilterView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAttendanceDetailsCount"));
                totalCount = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(results?.Data));
               
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = totalCount,
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeeAttendanceDetails>()
                });
            }
        }
        #endregion

        #region Get All Attendance Details
        [HttpPost]
        [Route("GetReporteesAttendanceDetails")]
        public async Task<IActionResult> GetReporteesAttendanceDetails(ReporteesView reportees)
        {
            List<EmployeeList> reporteesList = new();
            List<EmployeeList> grantLeaveEmployeeList = new();
            List<EmployeeRequestCount> AttendanceCount = new();
            List<EmployeeRequestCount> LeaveCount = new();
            List<EmployeeRequestCount> TimesheetCount = new();
            EmployeeListByDepartment employees = new();
            List<int> employeeIds = new();

            try
            {
                if(reportees.isManager == true)
                {
                   if(reportees.isListView == true)
                   {
                        var result = await _client.GetAsync(_leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetGrantLeaveByManagerId") + reportees.Employeeid);
                        employeeIds = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(result?.Data));
                        if (employeeIds.Count > 0)
                        {
                            employees.EmployeeId = employeeIds;
                            employees.managerId = reportees.Employeeid;
                            var employeeResult = await _client.PostAsJsonAsync(employees, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDetailForGrantLeaveById"));
                            grantLeaveEmployeeList = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(employeeResult?.Data));
                        }
                    }
                  
                   var results = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesForManagerId") + reportees.Employeeid);
                   reporteesList = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(results?.Data));
                   reporteesList = reporteesList == null ? new List<EmployeeList>() : reporteesList;
                   reporteesList = grantLeaveEmployeeList?.Count > 0 ? reporteesList.Concat(grantLeaveEmployeeList).ToList() : reporteesList;
                   employeeIds = reporteesList?.Select(x => x.EmployeeId).ToList();
                   employees.EmployeeId = employeeIds;
                   employees.managerId = reportees.Employeeid;
                }
                else
                {
                    employeeIds.Add(reportees.Employeeid);
                    reporteesList = new List<EmployeeList>();
                    EmployeeList employee = new EmployeeList();
                    employee.EmployeeId = employeeIds[0];
                    employees.EmployeeId = employeeIds;
                    employees.managerId = 0;
                    reporteesList.Add(employee);
                }
                if(reportees.isListView == true)
                {
                    var leaveResult = await _client.PostAsJsonAsync(employees, _leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:LeaveRequestCount"));
                    LeaveCount = JsonConvert.DeserializeObject<List<EmployeeRequestCount>>(JsonConvert.SerializeObject(leaveResult?.Data));
                    var attendanceResult = await _client.PostAsJsonAsync(employeeIds, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:AttendanceRequestCount"));
                    AttendanceCount = JsonConvert.DeserializeObject<List<EmployeeRequestCount>>(JsonConvert.SerializeObject(attendanceResult?.Data));
                    var timesheetResult = await _client.PostAsJsonAsync(employeeIds, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:WorkdayDetailCount"));
                    TimesheetCount = JsonConvert.DeserializeObject<List<EmployeeRequestCount>>(JsonConvert.SerializeObject(timesheetResult?.Data));

                    foreach(EmployeeList employee in reporteesList)
                    {
                        employee.leaveCount = LeaveCount?.Where(x => x.EmployeeId == employee.EmployeeId)?.Select(x => x.RequestCount).FirstOrDefault();
                        employee.attendanceCount = AttendanceCount?.Where(x => x.EmployeeId == employee.EmployeeId)?.Select(x => x.RequestCount).FirstOrDefault();
                        employee.timesheetCount = TimesheetCount?.Where(x => x.EmployeeId == employee.EmployeeId)?.Select(x => x.RequestCount).FirstOrDefault();
                    }
                }
                
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = reporteesList,
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetReporteesAttendanceDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion
       
    }
}