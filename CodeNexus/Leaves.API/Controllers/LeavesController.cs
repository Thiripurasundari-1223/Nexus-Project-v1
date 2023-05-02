using Leaves.DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Leaves;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using static SharedLibraries.ViewModels.Leaves.WeeklyOverviewReportView;

namespace Leaves.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        private readonly LeaveServices _leaveServices;
        private readonly string StatusText = "Something went wrong, please try again later";
        public LeavesController(LeaveServices leaveServices/*, IConfiguration iconfiguration*/)
        {
            //_configuration = iconfiguration;
            _leaveServices = leaveServices;
        }
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Leaves API - GET Method"
            });
        }

        #region Add or update Holiday       
        [HttpPost]
        [Route("AddOrUpdateHoliday")]
        public IActionResult AddOrUpdateHoliday(HolidayDetailView pholiday)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                string result = _leaveServices.HolidayNameDuplication(pholiday);
                if (result != null && result != "")
                {
                    if (result.ToLower() == "nameDuplication".ToLower())
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Same Holiday Name already exists. Please change holiday name and try again.",
                            Data = 0
                        });
                    }
                    if (result.ToLower() == "dateDuplication".ToLower())
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Same Holiday Date already exists. Please change holiday date and try again.",
                            Data = 0
                        });
                    }
                }
                else
                {
                    int HolidayID = _leaveServices.AddOrUpdateHoliday(pholiday).Result;
                    if (HolidayID > 0)
                    {
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = ("Holiday saved successfully."),
                            Data = HolidayID
                        }); ;
                    }
                }

                //int HolidayID = _leaveServices.AddOrUpdateHoliday(pholiday).Result;
                //if (HolidayID > 0)
                //{
                //    return Ok(new
                //    {
                //        StatusCode = "SUCCESS",
                //        StatusText = ("Holiday saved successfully."),
                //        Data = HolidayID
                //    }); ;
                //}

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AddOrUpdateHoliday", JsonConvert.SerializeObject(pholiday));
                //StatusText = ex.Message;
                //logger.Error(ex.Message);
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });

        }
        #endregion

        #region Delete Holiday
        [HttpGet]
        [Route("DeleteHoliday")]
        public IActionResult DeleteHoliday(int holidayId)
        {
            try
            {
                if (_leaveServices.DeleteHoliday(holidayId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Holiday deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        //StatusText = "Unexpected error occurred. Try again.",
                        StatusText,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/DeleteHoliday", JsonConvert.SerializeObject(holidayId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = false
                });
            }
        }
        #endregion

        #region Get All Holidays
        [HttpGet]
        [Route("GetAllHolidays")]
        public IActionResult GetAllHolidays()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAllHolidays()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAllHolidays");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<HolidayView>()
                });
            }
        }
        #endregion

        #region Get Get Holiday Detail By Id
        [HttpGet]
        [Route("GetByHolidayID")]
        public IActionResult GetByHolidayID(int pHolidayID)
        {
            HolidayDetailView holidayDetailView = new();
            try
            {
                holidayDetailView = _leaveServices.GetByHolidayID(pHolidayID);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = holidayDetailView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetByHolidayID", Convert.ToString(pHolidayID));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = holidayDetailView
                });
            }
        }
        #endregion

        #region Get All Upcoming Holidays
        [HttpPost]
        [Route("GetUpcomingHolidays")]
        public IActionResult GetUpcomingHolidays(WeekMonthAttendanceView employeeDetails)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _leaveServices.GetUpcomingHolidays(employeeDetails)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetUpcomingHolidays", JsonConvert.SerializeObject(employeeDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new HolidayDetailsView()
                });
            }
        }
        #endregion

        #region Add or Update Leave
        [HttpPost]
        [Route("AddorUpdateLeave")]
        public async Task<IActionResult> AddorUpdateLeave(LeaveDetailsView leaveDetailsView)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                LeaveTypesDetailView LeaveData = await _leaveServices.AddorUpdateLeave(leaveDetailsView);
                if (LeaveData?.LeaveTypeId > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = ("Leave saved successfully."),
                        Data = LeaveData
                    }); ;
                }
                else if (LeaveData.LeaveTypeId == -1)
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = ("This Leave Type Already Exists. Please try again different name."),
                        Data = LeaveData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AddOrUpdateLeave", JsonConvert.SerializeObject(leaveDetailsView));
                //StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = new LeaveTypesDetailView()
            });
        }
        #endregion

        #region Delete Leave
        [HttpGet]
        [Route("DeleteLeave")]
        public IActionResult DeleteLeave(int pLeaveTypeId)
        {
            try
            {

                DocumnentsPathView LeaveDetailsView = _leaveServices.DeleteLeave(pLeaveTypeId)?.Result;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = LeaveDetailsView
                });
                //var LeaveData = _leaveServices.DeleteLeave(pLeaveTypeId)?.Result;
                //if(LeaveData!=null)
                //{
                //    return Ok(new
                //    {
                //        StatusCode = "SUCCESS",
                //        StatusText = "Leave deleted successfully.",
                //        Data = LeaveData
                //    });
                //}
                //else
                //{
                //    return Ok(new
                //    {
                //        StatusCode = "FAILURE",
                //        StatusText,
                //        Data = LeaveData
                //    });
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/DeleteLeave", JsonConvert.SerializeObject(pLeaveTypeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<string>()
                });
            }
        }
        #endregion

        #region Get All Leaves
        [HttpGet]
        [Route("GetAllLeaves")]
        public IActionResult GetAllLeaves()
        {

            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAllLeaves()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAllLeaves");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<LeaveView>()
                });
            }
        }
        #endregion

        #region Get Leave Details By Leave Id
        [HttpGet]
        [Route("GetLeaveDetailsByLeaveId")]
        public IActionResult GetLeaveDetailsByLeaveId(int leaveId)
        {
            try
            {
                LeaveDetailsView LeaveDetailsView = _leaveServices.GetLeaveDetailsByLeaveId(leaveId);
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
                    StatusText
                });
            }
        }
        #endregion


        #region Get leaves master data
        [HttpGet]
        [AllowAnonymous]
        [Route("GetLeaveEmployeeMasterData")]
        public IActionResult GetLeaveEmployeeMasterData()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _leaveServices.GetLeaveEmployeeMasterData()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new LeavesMasterDataView()
                });
            }
        }
        #endregion

        #region Insert Apply Leave
        [HttpPost]
        [Route("InsertorUpdateApplyLeave")]
        public async Task<IActionResult> InsertorUpdateApplyLeave(ApplyLeavesView ApplyLeavesView)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            string StatusText = "Something went wrong, please try again later";
            try
            {
                bool isAllowRquestPeriod = true;
                bool isMinimumGapbetweenTwoApplication = true;
                bool isMaximumLeaveAvailable = true;
                bool isActiveLeavetype = true;
                bool isActiveHoliday = true;
                bool isMaximumConsecutiveDay = true;
                bool isWeekendOrholiday = true;
                bool checkSkipWeekendOrholiday = true;
                string maximumLeaveAvailable = "";
                string allowRequestPeriod = "";
                LeaveTypeRestrictionsView leaverestriction = new();
                leaverestriction = _leaveServices.GetLeaveRestrictionsDetailsByLeaveTypeId(ApplyLeavesView.LeaveTypeId);
                if (leaverestriction?.MaxLeaveAvailedYearId != null && leaverestriction?.MaxLeaveAvailedYearId > 0 && leaverestriction?.MaxLeaveAvailedDays != null && leaverestriction?.MaxLeaveAvailedDays > 0)
                {
                    if (leaverestriction?.EntitlementAppConstantsView?.Count > 0)
                    {
                        maximumLeaveAvailable = leaverestriction?.EntitlementAppConstantsView[0]?.AppConstantValue?.ToLower();
                        FinancialYearDateView financialYearDateView = new();
                        if (leaverestriction?.EntitlementAppConstantsView[0]?.AppConstantValue?.ToLower() == "monthly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearMonthly((DateTime)ApplyLeavesView?.FromDate);
                        }
                        else if (leaverestriction?.EntitlementAppConstantsView[0]?.AppConstantValue?.ToLower() == "quarterly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearQuarter((DateTime)ApplyLeavesView?.FromDate);
                        }
                        else if (leaverestriction?.EntitlementAppConstantsView[0]?.AppConstantValue?.ToLower() == "halfyearly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearHalfYearly((DateTime)ApplyLeavesView?.FromDate);
                        }
                        else if (leaverestriction?.EntitlementAppConstantsView[0]?.AppConstantValue?.ToLower() == "yearly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearly((DateTime)ApplyLeavesView?.FromDate);
                        }
                        if (financialYearDateView != null)
                        {
                            bool isEdit = false;
                            if (ApplyLeavesView?.LeaveId != 0)
                            {
                                isEdit = true;
                            }
                            decimal totalapplyleave = _leaveServices.GetLeaveByEmployeeId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId, financialYearDateView.FromDate, financialYearDateView.ToDate, ApplyLeavesView.LeaveId, isEdit);
                            decimal applyleavecount = totalapplyleave;
                            applyleavecount += ApplyLeavesView.NoOfDays;
                            if (applyleavecount > leaverestriction?.MaxLeaveAvailedDays)
                            {
                                isMaximumLeaveAvailable = false;
                            }
                        }
                    }
                }
                //if (leaverestriction?.AllowRequestPeriodId != null && leaverestriction?.AllowRequestPeriodId > 0 && leaverestriction?.MaximumLeave != null && leaverestriction?.MaximumLeave == true && leaverestriction?.MaximumLeavePerApplication != null && leaverestriction?.MaximumLeavePerApplication > 0)
                if (leaverestriction?.AppConstantsView?.Count > 0 && leaverestriction?.AppConstantsView != null && leaverestriction?.AllowRequestPeriodId != null && leaverestriction?.AllowRequestPeriodId > 0 && leaverestriction?.MinimumNoOfApplicationsPeriod != null && leaverestriction?.MinimumNoOfApplicationsPeriod > 0)
                {
                    //if (leaverestriction?.AppConstantsView?.Count > 0)
                    //{
                    allowRequestPeriod = leaverestriction?.AppConstantsView[0]?.AppConstantValue?.ToLower();
                        FinancialYearDateView financialYearDateView = new();
                        if (leaverestriction?.AppConstantsView[0]?.AppConstantValue?.ToLower() == "weekly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialWeek((DateTime)ApplyLeavesView?.FromDate);
                        }
                        else if (leaverestriction?.AppConstantsView[0]?.AppConstantValue?.ToLower() == "monthly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearMonthly((DateTime)ApplyLeavesView?.FromDate);
                        }
                        else if (leaverestriction?.AppConstantsView[0]?.AppConstantValue?.ToLower() == "quarterly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearQuarter((DateTime)ApplyLeavesView?.FromDate);
                        }
                        else if (leaverestriction?.AppConstantsView[0]?.AppConstantValue?.ToLower() == "halfyearly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearHalfYearly((DateTime)ApplyLeavesView?.FromDate);
                        }
                        else if (leaverestriction?.AppConstantsView[0]?.AppConstantValue?.ToLower() == "yearly")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearly((DateTime)ApplyLeavesView?.FromDate);
                        }
                        if (financialYearDateView != null)
                        {
                            List<ApplyLeaves> applyleavelist = new();
                            bool isEdit = false;
                            if (ApplyLeavesView?.LeaveId != 0)
                            {
                                isEdit = true;
                            }
                            applyleavelist = _leaveServices.GetLeaveRequestByEmployeeId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId, financialYearDateView.FromDate, financialYearDateView.ToDate, ApplyLeavesView.LeaveId, isEdit);
                            if (applyleavelist?.Count >= leaverestriction?.MinimumNoOfApplicationsPeriod)
                            {
                                isAllowRquestPeriod = false;
                            }
                        }
                    //}
                }
                isWeekendOrholiday = _leaveServices.CheckWeekendOrHoliday(ApplyLeavesView.EmployeeId, ApplyLeavesView, ApplyLeavesView.WeekendList, leaverestriction);
                if (leaverestriction?.MaximumConsecutiveDays > 0)
                {

                    decimal maxconsicutiveDays = leaverestriction?.MaximumConsecutiveDays == null ? 0 : (decimal)leaverestriction?.MaximumConsecutiveDays;
                    isMaximumConsecutiveDay = _leaveServices.GetConsecutiveEmployeAppliedLeaveDetails(ApplyLeavesView.EmployeeId, ApplyLeavesView, ApplyLeavesView.WeekendList, maxconsicutiveDays);

                }
                if (leaverestriction?.Weekendsbetweenleaveperiod == true || leaverestriction?.Holidaybetweenleaveperiod == true)
                {
                    checkSkipWeekendOrholiday = _leaveServices.CheckEmployeeSkipWeekendOrHoliday(ApplyLeavesView.EmployeeId, ApplyLeavesView, ApplyLeavesView.WeekendList, leaverestriction);

                }
                if (leaverestriction?.MinimumGap == true && leaverestriction?.MinimumGapTwoApplication > 0)
                {
                    isMinimumGapbetweenTwoApplication = _leaveServices.checkMinimumGap(ApplyLeavesView, leaverestriction?.MinimumGapTwoApplication == null ? 0 : (decimal)leaverestriction.MinimumGapTwoApplication);

                }
                ApplyLeavesView applyLeavesList = new();
                if (leaverestriction?.activeLeaveType != null && leaverestriction?.activeLeaveType?.Count > 0)
                {
                    List<int?> activeLeaveTypeId = leaverestriction?.activeLeaveType?.Select(rs => rs.leaveTypeId).ToList();
                    //DateTime previousDate = ApplyLeavesView.FromDate.Value.AddDays(-1);
                    //List<DateTime> leaveDate = ApplyLeavesView.AppliedLeaveDetails.Select(x => x.Date).ToList();
                    applyLeavesList = _leaveServices.GetAppliedLeaveByLeaveIds(ApplyLeavesView.EmployeeId, activeLeaveTypeId, ApplyLeavesView, ApplyLeavesView.WeekendList);
                    if (applyLeavesList != null)
                    {
                        //isActiveHoliday = false;
                        isActiveLeavetype = false;
                    }

                }
                Holiday activeHolidayList = new();
                if (leaverestriction?.activeHoliday != null && leaverestriction?.activeHoliday?.Count > 0)
                {
                    List<int?> activeHolidayId = leaverestriction?.activeHoliday?.Select(rs => rs.holidayID).ToList();
                    DateTime previousDate = ApplyLeavesView.FromDate.Value.AddDays(-1);
                    DateTime nextDate = ApplyLeavesView.FromDate.Value.AddDays(1);
                    activeHolidayList = _leaveServices.GetHolidayDetailByDate(activeHolidayId, ApplyLeavesView, ApplyLeavesView.WeekendList);
                    if (activeHolidayList != null)
                    {
                        //isActiveLeavetype = false;
                        isActiveHoliday = false;
                    }
                }
                if (isMaximumLeaveAvailable)
                {
                    if(ApplyLeavesView.RelivingDate !=null && ApplyLeavesView.CreatedBy== ApplyLeavesView.EmployeeId)
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Sorry, System won't allow you to apply leave while serving on notice period",
                            Data = 0
                        });
                    }
                    else if (_leaveServices.ApplyLeaveDatesDupilication(ApplyLeavesView))
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Leaves on this dates are already applied. Please change your dates and try again.",
                            Data = 0
                        });
                    }
                    else
                    {
                        if (isWeekendOrholiday)
                        {
                            if (isAllowRquestPeriod)
                            {
                                if (isMaximumConsecutiveDay)
                                {
                                    if (isMinimumGapbetweenTwoApplication)
                                    {
                                        if (isActiveLeavetype)
                                        {
                                            if (checkSkipWeekendOrholiday)
                                            {
                                                if (isActiveHoliday)
                                                {
                                                    if (ApplyLeavesView?.LeaveId != 0)
                                                    {
                                                        List<ApplyLeavesView> applyLeavesListView = new();
                                                        applyLeavesListView = _leaveServices.GetAppliedLeaveDetailsByEmployeeIdAndLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveId);
                                                        if (applyLeavesListView != null && applyLeavesListView?.Count > 0)
                                                        {
                                                            if (applyLeavesListView?.Count > 0)
                                                            {
                                                                int LeaveID = await _leaveServices.AddorUpdateApplyLeave(ApplyLeavesView, true);
                                                                if (LeaveID > 0)
                                                                {
                                                                    return Ok(new
                                                                    {
                                                                        StatusCode = "SUCCESS",
                                                                        StatusText = ("Leave updated successfully."),
                                                                        data = LeaveID
                                                                    });
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
                                                        else
                                                        {
                                                            return Ok(new
                                                            {
                                                                StatusCode = "FAILURE",
                                                                StatusText = ("Leave could not able to update because leave status is " + (applyLeavesListView?.Count > 0 ? applyLeavesListView[0].Status : "")),
                                                                Data = 0
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        int LeaveID = await _leaveServices.AddorUpdateApplyLeave(ApplyLeavesView, false);
                                                        if (LeaveID > 0)
                                                        {
                                                            return Ok(new
                                                            {
                                                                StatusCode = "SUCCESS",
                                                                StatusText = ("Leave saved successfully."),
                                                                data = LeaveID
                                                            });
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
                                                else
                                                {
                                                    return Ok(new
                                                    {
                                                        StatusCode = "FAILURE",
                                                        StatusText = "Sorry, This leave you can't be taken together with " + (activeHolidayList == null ? "" : activeHolidayList.HolidayName) + ".",
                                                        Data = 0
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                return Ok(new
                                                {
                                                    StatusCode = "FAILURE",
                                                    StatusText = "Sorry, System won't allow to skip weekend/holiday for this leave type. Please apply with weekend/holiday",
                                                    Data = 0
                                                });
                                            }
                                        }
                                        else
                                        {
                                            return Ok(new
                                            {
                                                StatusCode = "FAILURE",
                                                StatusText = "Sorry, This leave you can't be taken together with " + (applyLeavesList == null ? "" : applyLeavesList.LeaveType) + ".",
                                                Data = 0
                                            });
                                        }

                                    }
                                    else
                                    {
                                        return Ok(new
                                        {
                                            StatusCode = "FAILURE",
                                            StatusText = "This leave minimum gap of (" + GetHalfDayValues(leaverestriction?.MinimumGapTwoApplication == null ? 0 : (decimal)leaverestriction.MinimumGapTwoApplication) + " days) between two applications are required.",
                                            Data = 0
                                        });
                                    }
                                }
                                else
                                {
                                    return Ok(new
                                    {
                                        StatusCode = "FAILURE",
                                        StatusText = "This leave exceed maximum number of consecutive (" + GetHalfDayValues(leaverestriction?.MaximumConsecutiveDays == null ? 0 : (decimal)leaverestriction.MaximumConsecutiveDays) + " days) days. Please try other leave.",
                                        Data = 0
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new
                                {
                                    StatusCode = "FAILURE",
                                    StatusText = "This leave exceed maximum number of leave applications (" + GetRoundOff(leaverestriction?.MinimumNoOfApplicationsPeriod == null ? 0 : (decimal)leaverestriction.MinimumNoOfApplicationsPeriod) + ") per "+ allowRequestPeriod + ". Please try other leaves.",
                                    Data = 0
                                });
                            }
                        }
                        else
                        {
                            return Ok(new
                            {
                                StatusCode = "FAILURE",
                                StatusText = "Sorry, You can't apply this leave on weekend/holiday. Please try other days.",
                                Data = 0
                            });
                        }
                    }
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "This leave you can't take more than " + GetRoundOff(leaverestriction?.MaxLeaveAvailedDays == null ? 0 : (decimal)leaverestriction?.MaxLeaveAvailedDays) + " times per "+ maximumLeaveAvailable+". Exceed maximum limit, Please try with other leave.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/InsertorUpdateApplyLeave", JsonConvert.SerializeObject(ApplyLeavesView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText
                });
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });
        }

        #endregion

        //#region Get All Applied Leaves By Employee Id
        //[HttpGet]
        //[Route("GetAllAppliedLeavesByEmpId")]
        //public IActionResult GetAllAppliedLeavesByEmpId(int employeeId, DateTime FromDate, DateTime ToDate,int departmentId)
        //{
        //    try
        //    {
        //        return Ok(new
        //        {
        //            StatusCode = "SUCCESS",
        //            StatusText = string.Empty,
        //            Data = _leaveServices.GetAllAppliedLeavesByEmpId(employeeId, FromDate, ToDate, departmentId)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAllAppliedLeavesByEmpId", Convert.ToString(employeeId));
        //        return Ok(new
        //        {
        //            StatusCode = "FAILURE",
        //            StatusText = ex.ToString(),
        //            Data = new List<AppliedLeaveView>()
        //        });
        //    }
        //}
        //#endregion

        #region Get All Applied Leaves By Employee Id and Leave Id
        [HttpGet]
        [Route("GetAppliedLeaveToEdit")]
        public IActionResult GetAppliedLeaveToEdit(int leaveId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAppliedLeaveToEdit(leaveId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAppliedLeaveToEdit", Convert.ToString(leaveId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new AppliedLeaveEditView()
                });
            }
        }
        #endregion

        #region Delete Leave By Leave Id
        [HttpGet]
        [Route("DeleteAppliedLeaveByLeaveId")]
        public IActionResult DeleteAppliedLeaveByLeaveId(int leaveId)
        {
            try
            {
                if (_leaveServices.DeleteAppliedLeaveByLeaveId(leaveId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "Leave Deleted Successfully",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "Failure",
                        StatusText = "Unable To Delete Leave",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/DeleteAppliedLeaveByLeaveId", Convert.ToString(leaveId));
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = false
                });
            }
        }

        #endregion

        //#region Get Available Leave details By Employee Id
        //[HttpGet]
        //[Route("GetAvailableLeaveDetailsByEmployeeId")]
        //public IActionResult GetAvailableLeavesDetailByEmployeeId(int employeeId, DateTime FromDate, DateTime ToDate)
        //{
        //    try
        //    {
        //        return Ok(new
        //        {
        //            StatusCode = "SUCCESS",
        //            StatusText = string.Empty,
        //            Data = _leaveServices.GetAvailableLeaveDetailsByEmployeeId(employeeId, FromDate, ToDate)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAvailableLeaveDetailsByEmployeeId", Convert.ToString(employeeId));
        //        return Ok(new
        //        {
        //            StatusCode = "FAILURE",
        //            StatusText = ex.ToString(),
        //            Data = new List<EmployeeAvailableLeaveDetails>()
        //        });
        //    }
        //}
        //#endregion

        #region Get Team Leaves
        [HttpGet]
        [Route("GetTeamLeave")]
        public IActionResult GetTeamLeave(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetTeamLeave(managerTeamLeaveView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetTeamLeave");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<TeamLeaveView>()
                });
            }
        }
        #endregion

        #region Approve or Reject Leave
        [HttpPost]
        [Route("ApproveOrRejectLeave")]
        public IActionResult ApproveOrRejectLeave(ApproveOrRejectLeave pApproveOrRejectLeave)
        {
            StatusandApproverDetails statusandApprover = new();
            try
            {
                statusandApprover = _leaveServices.ApproveOrRejectLeave(pApproveOrRejectLeave).Result;
                if (statusandApprover.Status != "")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Leave(s) are " + statusandApprover.Status + " successfully.",
                        Data = statusandApprover
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText,
                        Data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/ApproveOrRejectLeave", JsonConvert.SerializeObject(pApproveOrRejectLeave));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = statusandApprover
                });
            }


        }
        #endregion

        #region Get Leave Types Dropdown
        [HttpGet]
        [Route("GetLeaveTypes")]
        public IActionResult GetLeaveTypes(int departmentId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _leaveServices.GetLeaveTypes(departmentId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveTypes");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<AvailableLeaveDetailsView>()
                });
            }
        }
        #endregion

        #region Get Employee LeaveAdjustment
        [HttpGet]
        [Route("GetEmployeeLeaveAdjustmentDetails")]
        public IActionResult GetEmployeeLeaveAdjustmentDetails(int employeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetEmployeeLeaveAdjustmentDetails(employeeId, fromDate, toDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeLeaveAdjustmentDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<EmployeeLeaveAdjustment>()
                });
            }
        }
        #endregion

        #region Get Weekly Leaves detail By Employee Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetWeeklyLeavesHolidayByEmployeeId")]
        public IActionResult GetWeeklyLeavesHolidayByEmployeeId(int employeeId)
        {
            WeeklyLeaveHolidayOverview WeeklyLeaveHolidayOverview = new();
            try
            {
                WeeklyLeaveHolidayOverview = _leaveServices.GetWeeklyLeavesHolidayByEmployeeId(employeeId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = WeeklyLeaveHolidayOverview
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetWeeklyLeavesByEmployeeId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = WeeklyLeaveHolidayOverview
                });
            }
        }
        #endregion

        #region Get Attendance Leave details By Employee Id
        [HttpGet]
        [Route("GetAttendanceLeaveDetailsByEmployeeId")]
        public IActionResult GetAttendanceLeavesDetailByEmployeeId(int employeeId, int departmentId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAttendanceLeaveDetailsByEmployeeId(employeeId, departmentId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAttendanceLeaveDetailsByEmployeeId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new AttendanceDaysAndHoursDetailsView()
                });
            }
        }
        #endregion

        #region Get Leave holiday details By Employee Id
        [HttpPost]
        [Route("GetLeaveHolidayByEmployeeId")]
        public IActionResult GetLeaveHolidayByEmployeeId(WeekMonthAttendanceView weekMonthAttendanceView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetLeaveHolidayByEmployeeId(weekMonthAttendanceView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveHolidayByEmployeeId", JsonConvert.SerializeObject(weekMonthAttendanceView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new LeaveHolidayView()
                });
            }
        }
        #endregion

        #region Get Leave Rejection Reason
        [HttpGet]
        [Route("GetLeaveRejectionReason")]
        public IActionResult GetLeaveRejectionReason()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _leaveServices.GetLeaveRejectionReason()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveRejectionReason");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<AvailableLeaveDetailsView>()
                });
            }
        }
        #endregion
        #region 
        [HttpPost]
        [Route("GetEmployeeHolidayByDepartment")]
        public IActionResult GetEmployeeHolidayByDepartment(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetEmployeeHolidayByDepartment(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeHolidayByDepartment", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<Holiday>()
                });
            }
        }
        #endregion

        #region Get Available Leave details By Employee 
        [HttpGet]
        [Route("GetAvailableLeaveDetailsByEmployee")]
        public IActionResult GetAvailableLeavesDetailByEmployee(int employeeID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAvailableLeaveDetailsByEmployee(employeeID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAvailableLeaveDetailsByEmployee", Convert.ToString(employeeID));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<AttendanceDaysAndHoursDetailsView>()
                });
            }
        }
        #endregion
        #region Get Team Leaves and Rejection
        [HttpPost]
        [Route("GetTeamLeaveAndRejection")]
        public IActionResult GetTeamLeaveAndRejection(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetTeamLeaveAndRejection(managerTeamLeaveView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetTeamLeaveAndRejection", JsonConvert.SerializeObject(managerTeamLeaveView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new TeamLeaveAndRejectionListView()
                });
            }
        }
        #endregion
        #region Get Available Leave and Duration details By Employee Id
        [HttpGet]
        [Route("GetAvailableLeaveAndDurationDetailsByEmployeeId")]
        public IActionResult GetAvailableLeaveAndDurationDetailsByEmployeeId(int employeeId, int departmentId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAvailableLeaveAndDurationDetailsByEmployeeId(employeeId, departmentId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAvailableLeaveAndDurationDetailsByEmployeeId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new AvailableLeaveAndDurationDetailsView()
                });
            }
        }
        #endregion
        #region Get Employee Leave and Restriction Details By Employee Id
        [HttpPost]
        [Route("GetEmployeeLeaveAndRestrictionDetails")]
        public async Task<IActionResult> GetEmployeeLeaveAndRestrictionDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = await _leaveServices.GetEmployeeLeaveAndRestrictionDetails(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeLeaveAndRestrictionDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new IndividualLeaveList()
                });
            }
        }
        #endregion
        #region Get Employee appled leave details
        [HttpPost]
        [Route("GetEmployeeAppliedLeaveDetails")]
        public IActionResult GetEmployeeAppliedLeaveDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetEmployeeAppliedLeaveDetails(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeAppliedLeaveDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new IndividualLeaveList()
                });
            }
        }
        #endregion
        #region Get appled leave details
        [HttpPost]
        [Route("GetAppliedLeaveDetails")]
        public IActionResult GetAppliedLeaveDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAppliedLeaveDetails(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAppliedLeaveDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new IndividualLeaveList()
                });
            }
        }
        #endregion

        #region Accrued Employee Leaves
        [HttpPost]
        [Route("AccruedEmployeeLeaves")]
        //public IActionResult AccruedEmployeeLeaves(List<EmployeeDetailsForLeaveView> EmployeeList)
        public IActionResult AccruedEmployeeLeaves(tempparameter temp)
        {
            try
            {
                //bool IsCarryForwardLeaveSuccess = _leaveServices.CreditCarryForwardLeaves(EmployeeList).Result;
                //bool IsLeaveAccruedSuccess = _leaveServices.AccruedEmployeeLeaves(EmployeeList).Result;

                //bool IsCarryForwardLeaveSuccess =  _leaveServices.CreditCarryForwardLeaves(temp).Result;
                bool IsLeaveAccruedSuccess = _leaveServices.AccruedEmployeeLeaves(temp).Result;
                DateTime todayDate = temp.executedate == null ? DateTime.Now.Date : (DateTime)temp.executedate;
                //bool IsLeaveGrantSuccess = _leaveServices.UpdateGrantLeavesRequest(todayDate.Date).Result;
                bool IsLeaveAccruedOnEffectiveFromDate = _leaveServices.LeaveAccruedOnEffectiveFromDate(temp).Result;
                bool IsUpdateBalanceSuccess = _leaveServices.UpdateAppliedLeaveBalance().Result;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = 0

                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AccruedEmployeeLeaves", JsonConvert.SerializeObject(temp));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = 0
                });
            }
        }
        #endregion
        #region Carry Forward Leaves
        [HttpPost]
        [Route("CarryForwardLeaves")]
        public IActionResult CarryForwardLeaves(tempparameter temp)
        {
            try
            {

                bool IsCarryForwardLeaveSuccess = _leaveServices.CreditCarryForwardLeaves(temp).Result;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = 0

                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/CarryForwardLeaves", JsonConvert.SerializeObject(temp));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = 0
                });
            }
        }
        #endregion

        #region Insert One Time Leave
        [HttpPost]
        [Route("InsertOneTimeLeave")]
        public IActionResult InsertOneTimeLeave(OneTimeEmployeeLeaveView EmployeeList)
        {
            try
            {
                //OneTimeEmployeeLeaveView employeeLeaveView = new OneTimeEmployeeLeaveView();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _leaveServices.InsertOneTimeLeave(EmployeeList)
                }); ;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/InsertOneTimeLeave", JsonConvert.SerializeObject(EmployeeList));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Leave Balance
        [HttpPost]
        [Route("GetLeaveBalance")]
        public async Task<IActionResult> GetLeaveBalance(EmployeeListView employee)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = await _leaveServices.GetLeaveBalance(employee)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveBalance", JsonConvert.SerializeObject(employee));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new LeaveBalanceView()
                });
            }
        }
        #endregion

        #region Update Employee Leave Balance
        [HttpPost]
        [Route("UpdateEmployeeLeaveBalance")]
        public IActionResult UpdateEmployeeLeaveBalance(EmployeeLeaveBalanceUpdateView leaveBalanceUpdateView)
        {
            try
            {
                if (_leaveServices.UpdateEmployeeLeaveBalance(leaveBalanceUpdateView).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Employee Leave Balance Updated Succesfully!",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "Failure",
                        StatusText = string.Empty,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateEmployeeLeaveBalance", JsonConvert.SerializeObject(leaveBalanceUpdateView));
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = false
                });
            }
        }
        #endregion

        #region Get Appconstant By ID
        [HttpGet]
        [Route("GetAppconstantByID")]
        public IActionResult GetAppconstantByID(int appConstantID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAppconstantByID(appConstantID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAppconstantByID", Convert.ToString(appConstantID));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = appConstantID
                });
            }
        }
        #endregion

        #region Get Holiday by Dept and Location and Shift and HolidayDate
        [HttpPost]
        [Route("GetHolidaybyDeptandLocandShifandDate")]
        public IActionResult GetHolidaybyDeptandLocandShifandDate(HolidayInput holidayInput)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetHolidaybyDeptandLocandShifandDate(holidayInput.DepartmentId, holidayInput.FromDate,
                                                                                holidayInput.ToDate, holidayInput.LocationId, holidayInput.ShiftDetailsId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetHolidaybyDeptandLocandShifandDate", JsonConvert.SerializeObject(holidayInput));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<Holiday>()
                });
            }
        }
        #endregion

        #region  Get Employees Leave list for Timesheet 
        [HttpPost]
        [Route("GetEmployeeLeavesForTimesheet")]
        public IActionResult GetEmployeeLeavesForTimesheet(EmployeeLeavesForTimeSheetViewInput employeeLeavesForTimeSheetViewInput)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetEmployeeLeavesForTimesheet(employeeLeavesForTimeSheetViewInput.resourceId,
                                                                        employeeLeavesForTimeSheetViewInput.fromDate, employeeLeavesForTimeSheetViewInput.toDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeLeavesForTimesheet", JsonConvert.SerializeObject(employeeLeavesForTimeSheetViewInput));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<EmployeeLeavesForTimeSheetView>()
                });
            }
        }
        #endregion

        #region Get Exists Leave By EmployeeId 
        [HttpGet]
        [Route("GetEmployeeExistsLeaves")]
        public IActionResult GetEmployeeExistsLeaves(int employeeId, DateTime fromDate)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetEmployeeExistsLeaves(employeeId, fromDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeExistsLeaves", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AppliedLeaveDetailsView>()
                });
            }
        }
        #endregion
        #region Update Leave Type Status 
        [HttpGet]
        [Route("UpdateLeaveTypeStatus")]
        public IActionResult UpdateLeaveTypeStatus(int leaveTypeId, bool isEnabled, int updatedBy)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.UpdateLeaveTypeStatus(leaveTypeId, isEnabled, updatedBy).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateLeaveTypeStatus", "LeaveTypeId -" + Convert.ToString(leaveTypeId) + " isEnabled -" + Convert.ToString(isEnabled) + " updatedBy -" + Convert.ToString(updatedBy));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion
        #region Update Holiday Status  
        [HttpGet]
        [Route("UpdateHolidayStatus")]
        public IActionResult UpdateHolidayStatus(int holidayId, bool isEnabled, int updatedBy)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.UpdateHolidayStatus(holidayId, isEnabled, updatedBy).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateHolidayStatus", "holidayId -" + Convert.ToString(holidayId) + " isEnabled -" + Convert.ToString(isEnabled) + " updatedBy -" + Convert.ToString(updatedBy));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<ApplyLeaves>()
                });
            }
        }
        #endregion
        #region Add or Update Grant Leave
        [HttpPost]
        [Route("AddorUpdateLeaveGrant")]
        public IActionResult AddorUpdateLeaveGrant(LeaveGrantRequestAndDocumentView leaveGrantRequestView)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                bool isMinimumGapbetweenTwoApplication = true;
                bool isGrantMaximumNoOfPeriodDay = true;
                int grantRequestFutureDay = 0;
                LeaveTypeRestrictionsView leaverestriction = new();
                leaverestriction = _leaveServices.GetLeaveRestrictionsDetailsByLeaveTypeId(leaveGrantRequestView.leaveGrantRequestDetail.LeaveTypeId);
                if (leaverestriction.GrantResetLeaveAfterDays == null)
                {
                    grantRequestFutureDay = 0;
                }
                else
                {
                    grantRequestFutureDay = (int)leaverestriction?.GrantResetLeaveAfterDays;
                }
                DateTime effectFrom = new DateTime((int)leaveGrantRequestView?.leaveGrantRequestDetail?.EffectiveFromDate?.Year, (int)leaveGrantRequestView.leaveGrantRequestDetail.EffectiveFromDate?.Month, (int)leaveGrantRequestView.leaveGrantRequestDetail.EffectiveFromDate?.Day);
                if (leaverestriction.GrantMaximumNoOfDay > 0)
                {
                    if (leaverestriction.GrantMaximumPeriodAppConstantsView != null && leaverestriction.GrantMaximumPeriodAppConstantsView?.Count > 0)
                    {
                        FinancialYearDateView financialYearDateView = new();
                        if (leaverestriction?.GrantMaximumPeriodAppConstantsView[0]?.AppConstantValue.ToLower() == "month")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearMonthly(effectFrom);
                        }
                        else if (leaverestriction?.GrantMaximumPeriodAppConstantsView[0]?.AppConstantValue.ToLower() == "year")
                        {
                            financialYearDateView = LeaveServices.GetFinancialYearly(effectFrom);
                        }
                        if (financialYearDateView != null)
                        {
                            List<LeaveGrantRequestDetails> leaveGrantRequestDetails = new();
                            if (leaveGrantRequestView?.leaveGrantRequestDetail?.LeaveTypeId > 0)
                            {
                                bool isEdit = false;
                                if (leaveGrantRequestView?.leaveGrantRequestDetail?.LeaveGrantDetailId != 0)
                                {
                                    isEdit = true;
                                }
                                leaveGrantRequestDetails = _leaveServices.GetGrantLeaveListByTypeAndEmployeeID(leaveGrantRequestView.leaveGrantRequestDetail.LeaveTypeId, leaveGrantRequestView.leaveGrantRequestDetail.EmployeeID, financialYearDateView.FromDate, financialYearDateView.ToDate, leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId, isEdit);
                                if (leaveGrantRequestDetails != null && leaveGrantRequestDetails?.Count > 0)
                                {
                                    if (leaveGrantRequestDetails?.Count >= leaverestriction.GrantMaximumNoOfDay)
                                    {
                                        isGrantMaximumNoOfPeriodDay = false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (leaverestriction.GrantMinimumGapTwoApplicationDay != null && leaverestriction?.GrantMinimumGapTwoApplicationDay > 0)
                {
                    List<LeaveGrantRequestDetails> leaveGrantRequestDetails = new();
                    if (leaveGrantRequestView?.leaveGrantRequestDetail?.LeaveTypeId > 0)
                    {

                        bool isEdit = false;
                        if (leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId != 0)
                        {
                            isEdit = true;
                        }
                        leaveGrantRequestDetails = new List<LeaveGrantRequestDetails>();
                        leaveGrantRequestDetails = _leaveServices.GetBackwardLeaveGrantGapByTypeIdAndEmployeeID(leaveGrantRequestView.leaveGrantRequestDetail.LeaveTypeId, leaveGrantRequestView.leaveGrantRequestDetail.EmployeeID, leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId, effectFrom, isEdit);
                        if (leaveGrantRequestDetails != null && leaveGrantRequestDetails?.Count > 0)
                        {
                            TimeSpan daycountdifference = (effectFrom - (DateTime)leaveGrantRequestDetails[0].EffectiveFromDate);
                            decimal daycount = Convert.ToDecimal(daycountdifference.TotalDays);
                            if (daycount <= leaverestriction?.GrantMinimumGapTwoApplicationDay)
                            {
                                isMinimumGapbetweenTwoApplication = false;
                            }
                        }
                        leaveGrantRequestDetails = new List<LeaveGrantRequestDetails>();
                        leaveGrantRequestDetails = _leaveServices.GetForwardLeaveGrantGapByTypeIdAndEmployeeID(leaveGrantRequestView.leaveGrantRequestDetail.LeaveTypeId, leaveGrantRequestView.leaveGrantRequestDetail.EmployeeID, leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId, effectFrom, isEdit);
                        if (leaveGrantRequestDetails != null && leaveGrantRequestDetails?.Count > 0)
                        {
                            TimeSpan daycountdifference = ((DateTime)leaveGrantRequestDetails[0].EffectiveFromDate - effectFrom);
                            decimal daycount = Convert.ToDecimal(daycountdifference.TotalDays);
                            if (daycount <= leaverestriction?.GrantMinimumGapTwoApplicationDay)
                            {
                                isMinimumGapbetweenTwoApplication = false;
                            }
                        }
                    }
                }
                if (leaveGrantRequestView?.GrantLeaveApprover?.RelivingDate !=null && leaveGrantRequestView?.leaveGrantRequestDetail?.CreatedBy != leaveGrantRequestView?.leaveGrantRequestDetail?.EmployeeID)
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Sorry, System won't allow you to request grant leave while serving on notice period",
                        Data = 0
                    });
                }
                else if (_leaveServices.ApplyLeaveGrantDatesDupilication(leaveGrantRequestView))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Leaves grant request on this dates are already applied. Please change your dates and try again.",
                        Data = 0
                    });
                }
                else
                {
                    if (isGrantMaximumNoOfPeriodDay)
                    {
                        if (isMinimumGapbetweenTwoApplication)
                        {
                            int LeaveGrantDetailId = _leaveServices.AddorUpdateLeaveGrant(leaveGrantRequestView, grantRequestFutureDay).Result;
                            if (LeaveGrantDetailId > 0)
                            {
                                string statustextmsg = "Leave Grant saved successfully.";
                                if (leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId > 0)
                                {
                                    statustextmsg = "Leave Grant Updated successfully.";
                                }


                                return Ok(new
                                {
                                    StatusCode = "SUCCESS",
                                    StatusText = (statustextmsg),
                                    Data = LeaveGrantDetailId
                                });
                            }
                        }
                        else
                        {
                            return Ok(new
                            {
                                StatusCode = "FAILURE",
                                StatusText = "Sorry, You are not able to request grant leave. Minimum gap of (" + GetRoundOff(leaverestriction.GrantMinimumGapTwoApplicationDay == null ? 0 : (decimal)leaverestriction.GrantMinimumGapTwoApplicationDay) + " days) between two applications are required.",
                                Data = 0
                            });
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "You already availed " + leaverestriction.GrantMaximumNoOfDay + " days leave in the quarter. Leaves more than " + leaverestriction.GrantMaximumNoOfDay + " days in a quarter needs Business / Function head approval with sufficient leave balance.",
                            Data = 0
                        });
                    }

                }

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AddorUpdateLeaveGrant", JsonConvert.SerializeObject(leaveGrantRequestView));
                //StatusText;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = StatusText,
                Data = 0
            });
        }
        #endregion
        #region Get Grant Leave By EmployeeId 
        [HttpGet]
        [Route("GetGrantLeaveByEmployeeId")]
        public IActionResult GetGrantLeaveByEmployeeId(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetGrantLeaveByEmployeeId(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetGrantLeaveByEmployeeId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<LeaveGrantRequestAndDocumentView>()
                });
            }
        }
        #endregion
        #region Get Grant Leave By EmployeeId and GrandLeave Id
        [HttpGet]
        [Route("GetGrantLeaveByEmployeeIdAndLeaveGrantId")]
        public IActionResult GetGrantLeaveByEmployeeIdAndLeaveGrantId(int employeeId, int leaveGrantDetailId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetGrantLeaveByEmployeeIdAndLeaveGrantId(employeeId, leaveGrantDetailId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetGrantLeaveByEmployeeIdAndLeaveGrantId", Convert.ToString(leaveGrantDetailId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<LeaveGrantRequestAndDocumentView>()
                });
            }
        }
        #endregion
        #region Delete Leave Grant By Leave GrantId
        [HttpGet]
        [Route("DeleteAppliedGrantLeaveByLeaveGrantId")]
        public IActionResult DeleteAppliedGrantLeaveByLeaveGrantId(int leaveGrantDetailId)
        {
            try
            {
                if (_leaveServices.DeleteAppliedGrantLeaveByLeaveGrantId(leaveGrantDetailId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "Leave Grant Deleted Successfully",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "Failure",
                        StatusText = "Unable To Delete Leave Grant",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/DeleteAppliedGrantLeaveByLeaveGrantId", Convert.ToString(leaveGrantDetailId));
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = false
                });
            }
        }

        #endregion
        #region Delete Leave Grant By Leave GrantId
        [HttpGet]
        [Route("DeleteGrantLeaveDocumentByLeaveGrantDocId")]
        public IActionResult DeleteGrantLeaveDocumentByLeaveGrantDocId(int leaveGrantDocumentDetailId)
        {
            try
            {
                if (_leaveServices.DeleteGrantLeaveDocumentByLeaveGrantDocId(leaveGrantDocumentDetailId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "Leave Grant Document Deleted Successfully",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "Failure",
                        StatusText = "Unable To Delete Leave Grant Document",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/DeleteGrantLeaveDocumentByLeaveGrantDocId", Convert.ToString(leaveGrantDocumentDetailId));
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = false
                });
            }
        }

        #endregion
        #region Dowload document
        [HttpGet]
        [Route("DownloadLeaveGrantDocumentById")]
        public IActionResult DownloadLeaveGrantDocumentById(int documentId)
        {
            SupportingDocuments supportDocuments = new SupportingDocuments();
            try
            {
                supportDocuments = _leaveServices.DownloadLeaveGrantDocumentById(documentId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = supportDocuments
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Leaves/DownloadLeaveGrantDocumentById", Convert.ToString(documentId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = supportDocuments
                });
            }
        }

        #endregion
        #region Add New Employee Leave
        [HttpPost]
        [Route("AddNewEmployeeLeave")]
        public async Task<IActionResult> AddNewEmployeeLeave(EmployeeDetailsForLeaveView employeedetails)
        {
            try
            {
                bool NewEmployeeLeave = _leaveServices.AddNewEmployeeLeave(employeedetails).Result;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = 0

                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AddNewEmployeeLeave");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = 0
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
        #region 
        [HttpPost]
        [Route("GetLeavesByEmployeeId")]
        public IActionResult GetLeavesByEmployeeId(WeekMonthAttendanceView weekMonthAttendanceView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetLeavesByEmployeeId(weekMonthAttendanceView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeavesByEmployeeId", JsonConvert.SerializeObject(weekMonthAttendanceView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<AppliedLeaveTypeDetails>()
                });
            }
        }
        #endregion
        #region  
        [HttpGet]
        [Route("GetAppconstantsList")]
        public IActionResult GetAppconstantsList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetAppconstantsList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAppconstantsList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<AppConstants>()
                });
            }
        }
        #endregion
        #region Get GetRound Off Values
        public static decimal GetHalfDayValues(decimal count)
        {
            decimal sd = default;
            if (count != null && count != 0)
            {
                //var decimalValues = (decimal)1.5112548;
                decimal decimalValue = decimal.Round(count, 2, MidpointRounding.AwayFromZero);
                var split = decimalValue.ToString(CultureInfo.InvariantCulture).Split('.');

                var de = Convert.ToDecimal("0." + (split.Length > 1 ? split[1] : 0));
                if (de is >= (decimal)0.5 and <= (decimal)0.99)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.5);
                }
                else
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.0);
                }
            }
            else
            {
                sd = 0;
            }
            return sd;
        }
        #endregion
        #region  Get leave history
        [HttpPost]
        [Route("GetLeaveHistoryByLeaveType")]
        public IActionResult GetLeaveHistoryByLeaveType(LeaveHistoryModel model)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetLeaveHistoryByLeaveType(model)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveHistoryByLeaveType");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<LeaveHistoryView>()
                });
            }
        }
        #endregion

        #region Get Employee Leave and Balance Details By Employee Id
        [HttpPost]
        [Route("GetEmployeeLeavesBalanceDetails")]
        public async Task<IActionResult> GetEmployeeLeavesBalanceDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = await _leaveServices.GetEmployeeLeavesBalanceDetails(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeLeavesBalanceDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new IndividualLeaveList()
                });
            }
        }
        #endregion
        #region Get Employee Leave  Details By Employee Id  and Leave Id
        [HttpPost]
        [Route("GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId")]
        public async Task<IActionResult> GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = await _leaveServices.GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new IndividualLeaveList()
                });
            }
        }
        #endregion
        #region Get Employee Leave  Details By Employee Id  and Leave Id
        [HttpPost]
        [Route("GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId")]
        public async Task<IActionResult> GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = await _leaveServices.GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new IndividualLeaveList()
                });
            }
        }
        #endregion
        #region Get Employee appled leave details
        [HttpPost]
        [Route("GetEmployeeHolidayDetails")]
        public IActionResult GetEmployeeHolidayDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _leaveServices.GetEmployeeHolidayDetails(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetEmployeeHolidayDetails", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new IndividualLeaveList()
                });
            }
        }
        #endregion
        #region Get appled leave details By Employee Id
        [HttpPost]
        [Route("GetAppliedLeaveDetailsByEmployeeId")]
        public IActionResult GetAppliedLeaveDetailsByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                var LeaveList = _leaveServices.GetAppliedLeaveDetailsByEmployeeId(employeeLeaveandRestriction);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = LeaveList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetAppliedLeaveDetailsByEmployeeId", JsonConvert.SerializeObject(employeeLeaveandRestriction));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new IndividualLeaveList()
                });
            }
        }
        #endregion

        #region Approve or Reject Leave
        [HttpPost]
        [Route("MultiSelectApproveOrRejectLeave")]
        public IActionResult MultiSelectApproveOrRejectLeave(List<ApproveOrRejectLeave> pApproveOrRejectLeave)
        {
            List<StatusandApproverDetails> statusandApproverList = new();
            try
            {
                foreach (var data in pApproveOrRejectLeave)
                {
                    StatusandApproverDetails statusandApprover = _leaveServices.ApproveOrRejectLeave(data).Result;
                    statusandApprover.LeaveId = data.LeaveId;
                    statusandApprover.LeaveGrantDetailId = data.LeaveGrantDetailId == null ? 0 : (int)data.LeaveGrantDetailId;
                    statusandApproverList.Add(statusandApprover);
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Selected request approved successfully.",
                    Data = statusandApproverList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/MultiSelectApproveOrRejectLeave", Newtonsoft.Json.JsonConvert.SerializeObject(pApproveOrRejectLeave));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = statusandApproverList
                });
            }
        }
        #endregion
        #region
        [HttpPost]
        [Route("LeaveRequestCount")]
        public IActionResult LeaveRequestCount(EmployeeListByDepartment employeeList)
        {
           
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Selected request approved successfully.",
                    Data = _leaveServices.GetPendingLeaveCount(employeeList)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/LeaveRequestCount", Newtonsoft.Json.JsonConvert.SerializeObject(employeeList));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = employeeList
                });
            }
        }
        #endregion
        #region
        [HttpGet]
        [Route("GetGrantLeaveByManagerId")]
        public IActionResult GetGrantLeaveByManagerId(int managerId)
        {

            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Selected request approved successfully.",
                    Data = _leaveServices.GetGrantLeaveByManagerId(managerId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/LeaveRequestCount", Newtonsoft.Json.JsonConvert.SerializeObject(managerId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = managerId
                });
            }
        }
        #endregion
    }
}
