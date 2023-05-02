using Attendance.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace Attendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AttendanceServices _attendanceServices;
        private readonly string StatusText = "Something went wrong, please try again later";
        public AttendanceController(AttendanceServices attendanceServices, IConfiguration iconfiguration)
        {
            _configuration = iconfiguration;
            _attendanceServices = attendanceServices;
        }
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Attendance API - GET Method"
            });
        }

        #region Add Or Update Shift        
        [HttpPost]
        [Route("AddOrUpdateShift")]
        public IActionResult AddOrUpdateShift(ShiftDetailsView shiftDetailsView)
        {
            try
            {
                int shiftDetailsId = _attendanceServices.AddOrUpdateShift(shiftDetailsView).Result;
                if (shiftDetailsId > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Shift Details saved successfully.",
                        Data = shiftDetailsId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText,
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/AddOrUpdateShift", JsonConvert.SerializeObject(shiftDetailsView));
                return Ok(new { StatusCode = "FAILURE", StatusText, Data = 0 });
            }
        }
        #endregion

        #region Delete Shift
        [HttpGet]
        [Route("DeleteShift")]
        public IActionResult DeleteShift(int ShiftDetailsId)
        {
            try
            {
                if (_attendanceServices.DeleteShift(ShiftDetailsId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Shift deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText ,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = false
                });
            }
        }
        #endregion

        #region Get All ShiftDetails
        [HttpGet]
        [Route("GetAllShiftDetails")]
        public IActionResult GetAllShiftDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.GetAllShiftDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAllShiftDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<ShiftView>()
                });
            }
        }
        #endregion

        #region Get ShiftDetails By Id
        [HttpGet]
        [Route("GetShiftDetailsById")]
        public IActionResult GetShiftDetailsById(int pShiftDetailsId)
        {
            try
            {
                AttendanceShiftDetailsView attendanceShiftDetails = _attendanceServices.GetByShiftDetailsId(pShiftDetailsId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = attendanceShiftDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetShiftDetailsById", Convert.ToString(pShiftDetailsId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new AttendanceShiftDetailsView()
                });
            }
        }
        #endregion

        #region InsertorUpdateAttendance
        [HttpPost]
        [Route("InsertorUpdateAttendance")]
        public IActionResult InsertorUpdateAttendance(AttendanceView Attendance)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            WeeklyMonthlyAttendance attendanceDetail = new WeeklyMonthlyAttendance();
            try
            {
                int attendanceID = _attendanceServices.AddorUpdateAttendance(Attendance).Result;
                if(attendanceID==-1)
                {
                    attendanceDetail.AttendanceId = -1;
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText= "Exists Attendance timelog details. Please try again.",
                        Data = attendanceDetail
                    });
                }
                if (attendanceID >= 0)
                {
                    WeekMonthAttendanceView attendanceDetails = new WeekMonthAttendanceView()
                    {
                        EmployeeId= Attendance.EmployeeId,
                        ShiftDetailsId= Attendance.ShiftId,
                        ShiftDate = Attendance.Date 
                    };
                    attendanceDetail = _attendanceServices.GetDailyAttendancedetail(attendanceDetails);
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Attendance saved successfully.",
                        Data = attendanceDetail
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/InsertorUpdateAttendance", JsonConvert.SerializeObject(Attendance));
                //StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = attendanceDetail
            });
        }
        #endregion

        #region Get Daily Attendance Detail
        [HttpPost("GetDailyAttendanceDetails")]
        public IActionResult GetDailyAttendanceDetails(WeekMonthAttendanceView attendance)
        {
            //string statusText = "";
            try
            {
                WeeklyMonthlyAttendance Result = _attendanceServices.GetDailyAttendancedetail(attendance);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetDailyAttendanceDetails", JsonConvert.SerializeObject(attendance));
                //statusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = new WeeklyMonthlyAttendance()
            });
        }
        #endregion

        #region Get Weekly and Monthly Attendance Details
        [HttpPost]
        [Route("GetWeeklyAndMonthlyAttendanceDetail")]
        public IActionResult GetWeeklyAndMonthlyAttendanceDetail(WeekMonthAttendanceView weekMonthAttendance)
        {
            try
            {
                WeekMonthAttendanceDetailedView WeeklyAttendanceDetail = _attendanceServices.GetWeeklyMonthlyAttendanceDetail(weekMonthAttendance);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = WeeklyAttendanceDetail
                    //Data = WeeklyAttendanceDetail == null ? new List<WeeklyMonthlyAttendance>() : WeeklyAttendanceDetail
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetWeeklyMonthlyAttendanceDetail", JsonConvert.SerializeObject(weekMonthAttendance));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new WeekMonthAttendanceDetailedView()
                });
            }
        }
        #endregion

        #region Get Attendance Details By Date
        [HttpPost]
        [Route("GetWeeklyAttendanceDetailByDate")]
        public IActionResult GetWeeklyAttendanceDetailByDate(WeekMonthAttendanceView weekMonthAttendance)
        {
            try
            {
                List<WeeklyMonthlyAttendance> WeeklyAttendanceDetail = _attendanceServices.GetWeeklyAttendanceDetailByDate(weekMonthAttendance);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = WeeklyAttendanceDetail
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetWeeklyAttendanceDetailByDate", JsonConvert.SerializeObject(weekMonthAttendance));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<WeeklyMonthlyAttendance>()
                });
            }
        }
        #endregion

        #region Get All Attendance Details
        [HttpPost]
        [Route("GetAttendanceDetails")]
        public IActionResult GetAttendanceDetails(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            try
            {
                EmployeeAttendanceShiftDetailsView employeesAttendances = _attendanceServices.GetEmployeeAttendanceDetails(managerTeamLeaveView);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = employeesAttendances == null ? new EmployeeAttendanceShiftDetailsView() : employeesAttendances
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new EmployeeAttendanceShiftDetailsView()
                });
            }
        }
        #endregion

        #region Get Attendance Detail By EmployeeId
        [HttpPost]
        [Route("GetAttendanceDetailByEmployeeId")]
        public IActionResult GetAttendanceDetailByEmployeeId(AttendanceWeekView attendanceWeekView)
        {
            try
            {
                List<DetailsView> detailsView = _attendanceServices.GetAttendanceDetailsById(attendanceWeekView);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = detailsView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceDetailByEmployeeId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<DetailsView>()
                });
            }
        }
        #endregion

        #region Insert or update TimeLog
        [HttpPost]
        [Route("InsertorUpdateTimeLog")]
        public IActionResult InsertorUpdateTimeLog(TimelogView timelogView)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            int AttendanceID = 0;
            try
            {
                AttendanceID = _attendanceServices.InsertorUpdateTimeLog(timelogView).Result;
                if (AttendanceID >= 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "TimeLog saved successfully.",
                        Data = AttendanceID
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/InsertorUpdateTimeLog", JsonConvert.SerializeObject(timelogView));
                //StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = AttendanceID
            });
        }
        #endregion

        #region Get Shift Name By Id
        [HttpPost]
        [Route("GetShiftNameById")]
        public IActionResult GetShiftNameById(List<int> shiftId)
        {
            try
            {
                List<KeyWithValue> detailsView = _attendanceServices.GetShiftNameById(shiftId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = detailsView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetShiftNameById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<KeyWithValue>()
                });
            }
        }
        #endregion

        #region Get Shift Id By Name
        [HttpGet]
        [Route("GetShiftIdByName")]
        public IActionResult GetShiftIdByName(string pShiftName)
        {
            try
            {
                int ShiftDetailsId = _attendanceServices.GetShiftIdByName(pShiftName);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = ShiftDetailsId
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetShiftIdByName");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get All Shift Name
        [HttpGet]
        [Route("GetAllShiftName")]
        public IActionResult GetAllShiftName()
        {
            try
            {
                List<KeyWithValue> detailsView = _attendanceServices.GetAllShiftName();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = detailsView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAllShiftName");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<KeyWithValue>()
                });
            }
        }
        #endregion

        #region Get Employee Shift Master Data
        [HttpGet]
        [Route("GetEmployeeShiftMasterData")]
        public IActionResult GetEmployeeShiftMasterData()
        {
            List<EmployeeShiftMasterData> employeeShiftMasterData = new List<EmployeeShiftMasterData>();
            try
            {
                employeeShiftMasterData = _attendanceServices.GetEmployeeShiftMasterData();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeShiftMasterData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeShiftMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = employeeShiftMasterData
                });
            }
        }
        #endregion

        #region Get Shift Weekend Detais
        [HttpGet]
        [Route("GetShiftWeekendDetails")]
        public IActionResult GetShiftWeekendDetails(int ShiftDetailsId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _attendanceServices.GetShiftWeekendDetails(ShiftDetailsId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetShiftWeekendDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new ShiftDetailedView()
                });
            }
        }
        #endregion

        #region Get Attendance Home Report
        [HttpGet]
        [Route("GetAttendanceHomeReport")]
        public IActionResult GetAttendanceHomeReport()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _attendanceServices.GetAttendanceHomeReport()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetShiftWeekendDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new ShiftViewDetails()
                });
            }
        }
        #endregion

        #region
        [HttpPost("GetEmployeeAbsentList")]
        public IActionResult GetEmployeeAbsentList(EmployeeDepartmentAndLocationView employeeDepartments)
        {
            //string statusText = "";
            try
            {
                List<ApplyLeavesView> Result = _attendanceServices.GetEmployeeAbsentList(employeeDepartments);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeAbsentList", JsonConvert.SerializeObject(employeeDepartments));
                //statusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = new List<ApplyLeavesView>()
            });
        }
        #endregion

        #region GetEmployeeAttendanceDetailsByEmployeeId
        [HttpGet]
        [Route("GetEmployeeAttendanceDetailsByEmployeeId")]
        public IActionResult GetEmployeeAttendanceDetailsByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _attendanceServices.GetEmployeeAttendanceDetailsByEmployeeId(employeeId, fromDate, toDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeAttendanceDetailsByEmployeeId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<EmployeesAttendanceDetails>()
                });
            }
        }
        #endregion
        #region InsertorUpdateAttendanceDetail Time Log
        [HttpPost]
        [Route("InsertorUpdateAttendanceDetailTimeLog")]
        public IActionResult InsertorUpdateAttendanceDetailTimeLog(TimeLogDetailView timeLogDetail)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            int AttendanceDetailID = 0;
            try
            {
                AttendanceDetailID = _attendanceServices.InsertorUpdateAttendanceDetailTimeLog(timeLogDetail).Result;
                if (AttendanceDetailID > 0)
                {
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
                        Data = -1
                    });
                }
                else if (AttendanceDetailID == -2)
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "System won't allow to apply time log for today/future date.",
                        Data = -2
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
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/InsertorUpdateAttendanceDetailTimeLog", JsonConvert.SerializeObject(timeLogDetail));
                //StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = AttendanceDetailID
            });
        }
        #endregion
        #region Get Attentance Details by EmployeeId and Date
        [HttpGet]
        [Route("GetAttendanceDetailByIdAndDate")]
        public IActionResult GetAttendanceDetailByIdAndDate(int employeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _attendanceServices.GetAttendanceDetailByIdAndDate(employeeId, fromDate, toDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceDetailByIdAndDate");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<SharedLibraries.Models.Attendance.Attendance>()
                });
            }
        }
        #endregion

        #region Get Employee Time and Weekend Definition by ShiftID
        [HttpGet]
        [Route("GetEmployeeTimeandWeekendDefinitionbyShiftID")]
        public IActionResult GetEmployeeTimeandWeekendDefinitionbyShiftID(int ShiftDetailsId)
        {
            try
            {

                TimeandWeekendDefinitionView timeandweekdetails = _attendanceServices.GetEmployeeTimeandWeekendDefinitionbyShiftID(ShiftDetailsId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timeandweekdetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeTimeandWeekendDefinitionbyShiftID", Convert.ToString(ShiftDetailsId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new TimeandWeekendDefinitionView()
                });
            }
        }
        #endregion,
        #region Employee Time Log Approve and Reject
        [HttpPost]
        [Route("TimeLogApproveOrReject")]
        public IActionResult TimeLogApproveOrReject(TimeLogApproveOrRejectView timeLogApproveOrRejectView)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            string AttendanceDetailID = "";
            try
            {
                AttendanceDetailID = _attendanceServices.TimeLogApproveOrReject(timeLogApproveOrRejectView).Result;
                if (AttendanceDetailID != null)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Attendance timelog details " + AttendanceDetailID +" successfully.",
                        Data = AttendanceDetailID
                    });
                }
                //else if (AttendanceDetailID == -1)
                //{
                //    return Ok(new
                //    {
                //        StatusCode = "SUCCESS",
                //        StatusText = "Rejected/Cancelled Attendance timelog details successfully.",
                //        Data = -1
                //    });
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/TimeLogApproveOrReject", JsonConvert.SerializeObject(timeLogApproveOrRejectView));
                //StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = AttendanceDetailID
            });
        }
        #endregion
        #region
        [HttpPost]
        [Route("GetEmployeeRegularizationList")]
        public IActionResult GetEmployeeRegularizationList(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.GetEmployeeRegularizationList(managerTeamLeaveView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeRegularizationList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<TeamLeaveView>()
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetDefaultShiftId")]
        public IActionResult GetDefaultShiftId()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.GetDefaultShiftId()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetDefaultShiftId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new EmployeeShiftDetailsView()
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetAttendanceDetailByDate")]
        public IActionResult GetAttendanceDetailByDate(int employeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _attendanceServices.GetAttendanceDetailByDate(employeeId, fromDate, toDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceDetailByDate");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<AttendaceDetails>()
                });
            }
        }
        #endregion
        #region
        [HttpGet]
        [Route("GetEmployeeRegularizationById")]
        public IActionResult GetEmployeeRegularizationById(int employeeId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.GetEmployeeRegularizationById(employeeId, FromDate, ToDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeRegularizationById", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<ApplyLeavesView>()
                });
            }
        }
        #endregion
        #region Delete Regularization
        [HttpGet]
        [Route("DeleteRegularizationByAttendanceDetailId")]
        public IActionResult DeleteRegularizationByAttendanceDetailId(int attendanceDetailId)
        {
            try
            {
                if (_attendanceServices.DeleteRegularizationByAttendanceDetailId(attendanceDetailId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Regularization Deleted Successfully",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unable To Delete Regularization",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/DeleteRegularizationByAttendanceDetailId", Convert.ToString(attendanceDetailId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = false
                });
            }
        }

        #endregion
        #region 
        [HttpGet]
        [Route("GetAttendanceHoursByDate")]
        public IActionResult GetAttendanceHoursByDate(int employeeId, DateTime fromDate)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _attendanceServices.GetAttendanceHoursByDate(employeeId, fromDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAttendanceHoursByDate");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new AttendaceDetails()
                });
            }
        }
        #endregion
        #region Update Shift Status  
        [HttpGet]
        [Route("UpdateShiftStatus")]
        public IActionResult UpdateShiftStatus(int shiftId, bool isEnabled, int updatedBy)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.UpdateShiftStatus(shiftId, isEnabled, updatedBy).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/UpdateShiftStatus", "shiftId -" + Convert.ToString(shiftId) + " isEnabled -" + Convert.ToString(isEnabled) + " updatedBy -" + Convert.ToString(updatedBy));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new ShiftDetails()
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetDefaultShiftDetailsById")]
        public IActionResult GetDefaultShiftDetailsById(int shiftId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.GetDefaultShiftDetailsById(shiftId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetDefaultShiftDetailsById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new EmployeeShiftDetailsView()
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetShiftDetails")]
        public IActionResult GetShiftDetails(int ShiftId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _attendanceServices.GetShiftDetails(ShiftId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetShiftDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new EmployeeShiftDetailView()
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetShiftDetailsList")]
        public IActionResult GetShiftDetailsList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.GetShiftDetailsList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetShiftDetailsList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new EmployeeShiftDetailsListView()
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetShiftWeekendDetailsList")]
        public IActionResult GetShiftWeekendDetailsList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.GetShiftWeekendDetailsList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetShiftWeekendDetailsList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<ShiftViewDetails>()
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetEmployeeTimeandWeekendbyShiftID")]
        public IActionResult GetEmployeeTimeandWeekendbyShiftID()
        {
            try
            {

                List<ShiftTimeandWeekendView> timeandweekdetails = _attendanceServices.GetEmployeeTimeandWeekendbyShiftID();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timeandweekdetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeTimeandWeekendbyShiftID");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new ShiftTimeandWeekendView()
                });
            }
        }
        #endregion,
        #region insert or update AbsentSetting        
        [HttpPost]
        [Route("InsertAndUpdateAbsentSetting")]
        public IActionResult InsertAndUpdateAbsentSetting(AbsentSettingsView absentSettingsView)
        {
            try
            {
                int AbsentSettingId = _attendanceServices.InsertAndUpdateAbsentSetting(absentSettingsView).Result;
                if (AbsentSettingId > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Absent settings saved successfully.",
                        Data = AbsentSettingId
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
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/InsertAndUpdateAbsentSetting", JsonConvert.SerializeObject(absentSettingsView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = 0
                });
            }
        }
        #endregion
        #region 
        [HttpGet]
        [Route("GetAbsentSettingDetails")]
        public IActionResult GetAbsentSettingDetails()
        {
            try
            {
                AbsentSettingsView absentSettingsView = _attendanceServices.GetAbsentSettingDetails();
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
                    StatusText
                });
            }
        }
        #endregion
        #region Get all active shift details
        [HttpGet]
        [Route("GetAllActiveShift")]
        public IActionResult GetAllActiveShift()
        {             
            try
            {
                
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText,
                    Data = _attendanceServices.GetAllActiveShift()
            });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAllActiveShift");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data=new List<ShiftView>()
                });
            }
        }
        #endregion

        #region
        [HttpPost]
        [Route("GetEmployeeRegularizationDetailById")]
        public IActionResult GetEmployeeRegularizationDetailById(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _attendanceServices.GetEmployeeRegularizationByEmployeeId(employeeLeaveandRestriction)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetEmployeeRegularizationList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new RegularizationDetailView()
                });
            }
        }
        #endregion

        #region Employee Time Log Approve and Reject
        [HttpPost]
        [Route("MultiselectTimeLogApproveOrReject")]
        public IActionResult MultiSelectTimeLogApproveOrReject(List<TimeLogApproveOrRejectView> timeLogApproveOrRejectView)
        {
            //string StatusText = "Unexpected error occurred. Try again.";
            string AttendanceDetailID = "";
            try
            {
                foreach (var data in timeLogApproveOrRejectView)
                {
                    AttendanceDetailID = _attendanceServices.TimeLogApproveOrReject(data).Result;
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Requests "+ AttendanceDetailID + " successfully.",
                    Data = AttendanceDetailID
                });
                //else if (AttendanceDetailID == -1)
                //{
                //    return Ok(new
                //    {
                //        StatusCode = "SUCCESS",
                //        StatusText = "Rejected/Cancelled Attendance timelog details successfully.",
                //        Data = -1
                //    });
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/MultiSelectTimeLogApproveOrReject", Newtonsoft.Json.JsonConvert.SerializeObject(timeLogApproveOrRejectView));
                //StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = AttendanceDetailID
            });
        }
        #endregion

        #region
        [HttpPost]
        [Route("AttendanceRequestCount")]
        public IActionResult AttendanceRequestCount(List<int> employeeIdList)
        {

            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _attendanceServices.GetAttendanceRequestCount(employeeIdList)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/LeaveRequestCount", Newtonsoft.Json.JsonConvert.SerializeObject(employeeIdList));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = employeeIdList
                });
            }
        }
        #endregion
    }
}