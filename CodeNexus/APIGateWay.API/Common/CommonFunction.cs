using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Leaves;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;
using Microsoft.Extensions.Configuration;
using SharedLibraries;
using SharedLibraries.Common;
using System.Net.Http;
using APIGateWay.API.Model;
using SharedLibraries.Models.Notifications;
using Newtonsoft.Json;
using SharedLibraries.ViewModels;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace APIGateWay.API.Common
{
    public class CommonFunction
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HTTPClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _leavesBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _notificationBaseURL = string.Empty;
        private readonly string _attendanceBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";
        #region Constructor
        public CommonFunction(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _configuration = configuration;
            _leavesBaseURL = _configuration.GetValue<string>("ApplicationURL:Leaves:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _notificationBaseURL = _configuration.GetValue<string>("ApplicationURL:Notifications");
            _attendanceBaseURL = _configuration.GetValue<string>("ApplicationURL:Attendance:BaseURL");
        }
        #endregion
        public async Task<List<WeeklyMonthlyAttendance>> GetAbsentList(List<WeeklyMonthlyAttendance> WeeklyMarkedAttendance, List<ShiftViewDetails> WeekendList, DateTime fromDate, DateTime toDate, LeaveHolidayView leaveHoliday, List<EmployeeShiftDetailsView> EmployeeShiftDetails, DateTime DOJ, AbsentRestrictionView restrictionView)
        {
            ShiftViewDetails WeekendShiftList = new();
            List<WeeklyMonthlyAttendance> WeeklyAttendanceDetail = new();
            int totalDays = Convert.ToInt32((toDate - fromDate).TotalDays);
            for (int days = 0; days <= totalDays; days++)
            {
                WeeklyMonthlyAttendance weekMonthDay = new WeeklyMonthlyAttendance();
                weekMonthDay.Date = fromDate.AddDays(days).Date;
                WeeklyMonthlyAttendance attendanceMarkedDay = WeeklyMarkedAttendance?.Where(x => x.Date == fromDate.AddDays(days)).Select(x => x).FirstOrDefault();
                if (EmployeeShiftDetails?.Count > 0)
                {
                    foreach (EmployeeShiftDetailsView item in EmployeeShiftDetails)
                    {
                        if (weekMonthDay.Date >= item.ShiftFromDate && (item.ShiftToDate == null || weekMonthDay.Date <= item.ShiftToDate))
                        {
                            WeekendShiftList = WeekendList?.Where(x => x.ShiftDetailsId == item.ShiftDetailsId).Select(x => x).FirstOrDefault();
                        }
                    }
                    if (WeekendShiftList?.ShiftDetailsId == 0)
                    {
                        WeekendShiftList = WeekendList?.Where(x => x.IsGenralShift == true).Select(x => x).FirstOrDefault();
                        if (WeekendShiftList?.ShiftDetailsId == 0)
                        {
                            WeekendShiftList = WeekendList?.Select(x => x).FirstOrDefault();
                        }
                    }
                }
                else
                {
                    WeekendShiftList = WeekendList?.Where(x => x.IsGenralShift == true).Select(x => x).FirstOrDefault();
                    if (WeekendShiftList.ShiftDetailsId == 0)
                    {
                        WeekendShiftList = WeekendList?.Select(x => x).FirstOrDefault();
                    }
                }
              
                if (attendanceMarkedDay != null)
                {
                    //TimeSpan? utcTime = new TimeSpan(5, 30, 0);
                    //string[] hour = attendanceMarkedDay?.TotalHours?.Split(":");
                    //TimeSpan? totalHours = new TimeSpan(hour?.Length > 0 ? Convert.ToInt32(hour[0]) : 0, hour?.Length > 1 ? Convert.ToInt32(hour[1]) : 0, hour?.Length > 2 ? Convert.ToInt32(hour[2]) : 0);
                    //string[] AbsentFromHour = WeekendShiftList?.AbsentFromHour?.Split(":");
                    //TimeSpan? absentFromHour = new TimeSpan(AbsentFromHour?.Length > 0 ? Convert.ToInt32(AbsentFromHour[0]) : 0, AbsentFromHour?.Length > 1 ? Convert.ToInt32(AbsentFromHour[1]) : 0, AbsentFromHour?.Length > 2 ? Convert.ToInt32(AbsentFromHour[2]) : 0);
                    //string[] AbsentToHour = WeekendShiftList?.AbsentToHour?.Split(":");
                    //TimeSpan? absentToHour = new TimeSpan(AbsentToHour?.Length > 0 ? Convert.ToInt32(AbsentToHour[0]) : 0, AbsentToHour?.Length > 1 ? Convert.ToInt32(AbsentToHour[1]) : 0, AbsentToHour?.Length > 2 ? Convert.ToInt32(AbsentToHour[2]) : 0);
                    //string[] HalfaDayFromHour = WeekendShiftList?.HalfaDayFromHour?.Split(":");
                    //TimeSpan? halfaDayFromHour = new TimeSpan(HalfaDayFromHour?.Length > 0 ? Convert.ToInt32(HalfaDayFromHour[0]) : 0, HalfaDayFromHour?.Length > 1 ? Convert.ToInt32(HalfaDayFromHour[1]) : 0, HalfaDayFromHour?.Length > 2 ? Convert.ToInt32(HalfaDayFromHour[2]) : 0);
                    //string[] HalfaDayToHour = WeekendShiftList?.HalfaDayToHour?.Split(":");
                    //TimeSpan? halfaDayToHour = new TimeSpan(HalfaDayToHour?.Length > 0 ? Convert.ToInt32(HalfaDayToHour[0]) : 0, HalfaDayToHour?.Length > 1 ? Convert.ToInt32(HalfaDayToHour[1]) : 0, HalfaDayToHour?.Length > 2 ? Convert.ToInt32(HalfaDayToHour[2]) : 0);
                    //string[] PresentHour = WeekendShiftList?.PresentHour?.Split(":");
                    //TimeSpan? presentHour = new TimeSpan(PresentHour?.Length > 0 ? Convert.ToInt32(PresentHour[0]) : 0, PresentHour?.Length > 1 ? Convert.ToInt32(PresentHour[1]) : 0, PresentHour?.Length > 2 ? Convert.ToInt32(PresentHour[2]) : 0);
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
                    weekMonthDay = attendanceMarkedDay;
                    //int i = 0;
                    //TimeSpan? CurrentTime = new TimeSpan();
                    //if (attendanceMarkedDay?.WeeklyMonthlyAttendanceDetail?.Count > 0)
                    //{
                    //    DateTime checkinTime = attendanceMarkedDay.WeeklyMonthlyAttendanceDetail[i].CheckinTime.Value;
                    //    CurrentTime = checkinTime.TimeOfDay;
                    //}
                    if (WeekendShiftList != null)
                    {
                        weekMonthDay.ShiftHour = (WeekendShiftList.TotalHours == null || WeekendShiftList.TotalHours == "") ? "0" : WeekendShiftList.TotalHours;
                    }
                    else
                    {
                        weekMonthDay.ShiftHour = "0";
                    }
                    //Holiday holiday = leaveHoliday?.Holiday?.Where(x => x.HolidayDate == fromDate.AddDays(days).Date).Select(x => x).FirstOrDefault();
                    //if (holiday != null)
                    //{
                    //    weekMonthDay.IsAbsent = false;
                    //    weekMonthDay.IsHoliday = true;
                    //    weekMonthDay.Remark = holiday.HolidayName;
                    //}
                    if (WeekendShiftList != null)
                    {
                        var dayname = fromDate.AddDays(days).Date.ToString("dddd");
                        if (WeekendShiftList?.WeekendList?.Where(rs => rs.WeekendDayName == dayname).ToList().Count > 0)
                        {
                            weekMonthDay.IsAbsent = false;
                            weekMonthDay.IsWeekend = true;
                            weekMonthDay.Remark = "Weekend";
                        }
                    }
                    if (weekMonthDay.Remark == null || weekMonthDay.Remark == string.Empty)
                    {

                        weekMonthDay = getAbsentDetails(WeekendShiftList, attendanceMarkedDay, DOJ, fromDate.AddDays(days));

                        List<AppliedLeaveTypeDetails> leaves = leaveHoliday?.appliedLeaveDetails?.Where(x => x.Date.Date <= fromDate.AddDays(days).Date && x.Date.Date >= fromDate.AddDays(days).Date).Select(x => x).ToList();
                        weekMonthDay.LeaveDetails = leaves;
                        if (leaves != null && leaves?.Count > 0)
                        {
                            foreach (var item in leaves)
                            {
                                weekMonthDay.LeaveReason = item.reason;
                                weekMonthDay.NoOfDays = item.NoOfDays;
                                weekMonthDay.LeaveId = item.LeaveId == null ? 0 : (int)item.LeaveId;
                            }
                            //List<WeeklyMonthlyAttendance> Leavelist = new();
                            //foreach (var item in leaves)
                            //{
                            //    WeeklyMonthlyAttendance leave = new();
                            //    if (item.IsFirstHalf == true) { leave.IsFirstHalf = true; leave.IsSecondHalf = false; leave.IsLeave = true; }
                            //    else if (item.IsSecondHalf == true) { leave.IsSecondHalf = true; leave.IsLeave = true; leave.IsFirstHalf = false; }
                            //    else { leave.IsFirstHalf = false; leave.IsSecondHalf = false; }
                            //    Leavelist.Add(leave);
                            //}
                            bool Leavelists = leaves?.Where(x => x?.IsFirstHalf == true || x?.IsSecondHalf == true).Select(x => x)?.Count() >= 2 ? true : false;
                            if (Leavelists == true) // First off & Second off leave
                            {
                                var firstLeave = leaves.Select(x => x).ToList();
                                var list = firstLeave.Where(x => x.IsFirstHalf).Select(x => x).FirstOrDefault();
                                {
                                    
                                    weekMonthDay.FirstHalfRemark = list.LeaveType;
                                    weekMonthDay.FirstHalfLeaveType = list.LeaveTypeName;
                                    weekMonthDay.FirstHalfLeaveStatus = list.AppliedLeaveStatus == true ? "Leave Approved" : list.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied"; 
                                }
                                var list2 = firstLeave.Where(x => x.IsSecondHalf).Select(x => x).FirstOrDefault();
                                {
                                    weekMonthDay.SecondHalfRemark = list2.LeaveType;
                                    weekMonthDay.SecondHalfLeaveType = list2.LeaveTypeName;
                                    weekMonthDay.SecondHalfLeaveStatus = list.AppliedLeaveStatus == true ? "Leave Approved" : list.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";
                                }
                                //weekMonthDay.LeaveTypeName = firstLeave.LeaveTypeName;
                                //weekMonthDay.LeaveType = firstLeave.LeaveType;
                                weekMonthDay.IsLeave = true;
                                weekMonthDay.Remark = "";
                                weekMonthDay.IsFirstHalf = true;
                                weekMonthDay.IsSecondHalf = true;
                                weekMonthDay.IsAbsent = false;
                            }
                            else //First off or Second off or full day
                            {
                                var firstLeave = leaves.Select(x => x).FirstOrDefault();
                                weekMonthDay.IsLeave = true;
                                //if (firstLeave.IsFirstHalf && weekMonthDay?.IsSecondHalfAbsent == true && weekMonthDay.IsFirstHalfAbsent == null && weekMonthDay?.Remark == "Half Day Absent")
                                //{
                                //    weekMonthDay.IsSecondHalfAbsent = false;
                                //    weekMonthDay.IsFirstHalfAbsent = false;
                                //}
                                //else if (firstLeave.IsSecondHalf && weekMonthDay?.IsFirstHalfAbsent == true && weekMonthDay.IsSecondHalfAbsent == null && weekMonthDay?.Remark == "Half Day Absent")
                                //{
                                //    weekMonthDay.IsSecondHalfAbsent = false;
                                //    weekMonthDay.IsFirstHalfAbsent = false;
                                //}

                                if (firstLeave.IsFirstHalf == false && firstLeave.IsSecondHalf == false)
                                {
                                    weekMonthDay.Remark = firstLeave.LeaveType;
                                    weekMonthDay.LeaveTypeName = firstLeave.LeaveTypeName;
                                    weekMonthDay.LeaveStatus = firstLeave.AppliedLeaveStatus == true ? "Leave Approved" : firstLeave.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";
                                    weekMonthDay.IsAbsent = false;
                                    weekMonthDay.IsFirstHalfAbsent = false;
                                    weekMonthDay.IsSecondHalfAbsent = false;
                                }
                                else if (firstLeave.IsFirstHalf)
                                {
                                    //weekMonthDay.Remark = "Half Day Leave";
                                    if (weekMonthDay?.IsFirstHalfAbsent == true)
                                    {
                                        weekMonthDay.IsFirstHalfAbsent = false;
                                    }
                                    if (weekMonthDay.Remark == "Full Day Absent")
                                    {
                                        weekMonthDay.IsSecondHalfAbsent = true;
                                    }
                                    weekMonthDay.Remark = "Half Day Absent";
                                    weekMonthDay.FirstHalfRemark = firstLeave.LeaveType;
                                    weekMonthDay.FirstHalfLeaveType = firstLeave.LeaveTypeName;
                                    weekMonthDay.FirstHalfLeaveStatus = firstLeave.AppliedLeaveStatus == true ? "Leave Approved" : firstLeave.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";
                                }
                                else if (firstLeave.IsSecondHalf)
                                {
                                    //weekMonthDay.Remark = "Half Day Leave";
                                    if (weekMonthDay?.IsSecondHalfAbsent == true)
                                    {
                                        weekMonthDay.IsSecondHalfAbsent = false;
                                    }
                                    if (weekMonthDay.Remark == "Full Day Absent")
                                    {
                                        weekMonthDay.IsFirstHalfAbsent = true;
                                    }
                                    weekMonthDay.Remark = "Half Day Absent";
                                    weekMonthDay.SecondHalfRemark = firstLeave.LeaveType;
                                    weekMonthDay.SecondHalfLeaveType = firstLeave.LeaveTypeName;
                                    weekMonthDay.SecondHalfLeaveStatus = firstLeave.AppliedLeaveStatus == true ? "Leave Approved" : firstLeave.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";
                                }
                                weekMonthDay.IsFirstHalf = firstLeave.IsFirstHalf;
                                weekMonthDay.IsSecondHalf = firstLeave.IsSecondHalf;
                                weekMonthDay.IsFullDay = firstLeave.IsFullDay;
                                weekMonthDay.AppliedLeaveStatus = firstLeave.AppliedLeaveStatus;

                            }
                        }
                        else
                        {
                            Holiday holiday = leaveHoliday?.Holiday?.Where(x => x.HolidayDate == fromDate.AddDays(days).Date).Select(x => x).FirstOrDefault();
                            if (holiday != null)
                            {
                                weekMonthDay.IsAbsent = false;
                                weekMonthDay.IsHoliday = true;
                                weekMonthDay.Remark = holiday.HolidayName;
                                if (holiday.IsRestrictHoliday == true)
                                {
                                    weekMonthDay.LeaveType = "RestrictedHoliday";
                                }
                            }
                        }
                    }
                    WeeklyAttendanceDetail.Add(weekMonthDay);
                }
                else
                {
                    if (WeekendShiftList != null)
                    {
                        weekMonthDay.ShiftHour = (WeekendShiftList.TotalHours == null || WeekendShiftList.TotalHours == "") ? "0" : WeekendShiftList?.TotalHours;
                    }
                    else
                    {
                        weekMonthDay.ShiftHour = "0";
                    }
                    List<AppliedLeaveTypeDetails> leaves = leaveHoliday?.appliedLeaveDetails?.Where(x => x.Date.Date <= fromDate.AddDays(days).Date && x.Date.Date >= fromDate.AddDays(days).Date).Select(x => x).ToList();
                    weekMonthDay.LeaveDetails = leaves;
                    if (leaves != null && leaves?.Count > 0)
                    {
                        List<WeeklyMonthlyAttendance> Leavelist = new();
                        foreach (var item in leaves)
                        {
                            weekMonthDay.LeaveReason = item.reason;
                            weekMonthDay.NoOfDays = item.NoOfDays;
                            weekMonthDay.LeaveId = item.LeaveId == null ? 0: (int)item.LeaveId;
                            WeeklyMonthlyAttendance leave = new();
                            if (item.IsFirstHalf == true) { leave.IsFirstHalf = true; leave.IsSecondHalf = false; leave.IsLeave = true; }
                            else if (item.IsSecondHalf == true) { leave.IsSecondHalf = true; leave.IsLeave = true; leave.IsFirstHalf = false; }
                            else { leave.IsFirstHalf = false; leave.IsSecondHalf = false; }
                            Leavelist.Add(leave);
                        }
                        bool Leavelists = Leavelist?.Where(x => (bool)x?.IsFirstHalf || (bool)x?.IsSecondHalf).Select(x => x)?.Count() >= 2 ? true : false;
                        if (!Leavelists)
                        {
                            var firstLeave = leaves.Select(x => x).FirstOrDefault();
                            weekMonthDay.IsLeave = true;
                            if (firstLeave.IsFirstHalf && fromDate.AddDays(days).Date < DateTime.Now.Date)
                            {
                                weekMonthDay.Remark = "Half Day Absent";
                                weekMonthDay.FirstHalfRemark = firstLeave.LeaveType;
                                weekMonthDay.FirstHalfLeaveType = firstLeave.LeaveTypeName;
                                weekMonthDay.FirstHalfLeaveStatus = firstLeave.AppliedLeaveStatus == true ? "Leave Approved" : firstLeave.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else if (firstLeave.IsFirstHalf && fromDate.AddDays(days).Date >= DateTime.Now.Date)
                            {
                                weekMonthDay.Remark = "";
                                weekMonthDay.FirstHalfRemark = firstLeave.LeaveType;
                                weekMonthDay.FirstHalfLeaveType = firstLeave.LeaveTypeName;
                                weekMonthDay.FirstHalfLeaveStatus = firstLeave.AppliedLeaveStatus == true ? "Leave Approved" : firstLeave.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";
                            }
                            else if (firstLeave.IsSecondHalf && fromDate.AddDays(days).Date < DateTime.Now.Date)
                            {
                                weekMonthDay.Remark = "Half Day Absent";
                                weekMonthDay.SecondHalfRemark = firstLeave.LeaveType;
                                weekMonthDay.SecondHalfLeaveType = firstLeave.LeaveTypeName;
                                weekMonthDay.SecondHalfLeaveStatus = firstLeave.AppliedLeaveStatus == true ? "Leave Approved" : firstLeave.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                            else if (firstLeave.IsSecondHalf && fromDate.AddDays(days).Date >= DateTime.Now.Date)
                            {
                                weekMonthDay.Remark = "";
                                weekMonthDay.SecondHalfRemark = firstLeave.LeaveType;
                                weekMonthDay.SecondHalfLeaveType = firstLeave.LeaveTypeName;
                                weekMonthDay.SecondHalfLeaveStatus = firstLeave.AppliedLeaveStatus == true ? "Leave Approved" : firstLeave.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";

                            }
                            else
                            {
                                weekMonthDay.Remark = firstLeave.LeaveType;
                                weekMonthDay.LeaveType = firstLeave.LeaveTypeName;
                                weekMonthDay.LeaveStatus = firstLeave.AppliedLeaveStatus == true ? "Leave Approved" : firstLeave.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";
                            }
                            //weekMonthDay.LeaveTypeName = firstLeave.LeaveTypeName;
                            //weekMonthDay.LeaveType = firstLeave.LeaveType;
                            weekMonthDay.IsFirstHalf = firstLeave.IsFirstHalf;
                            weekMonthDay.IsSecondHalf = firstLeave.IsSecondHalf;
                            weekMonthDay.IsFullDay = firstLeave.IsFullDay;
                            weekMonthDay.AppliedLeaveStatus = firstLeave.AppliedLeaveStatus;
                        }
                        else
                        {
                            var firstLeave = leaves.Select(x => x).ToList();
                            var list = firstLeave.Where(x => x.IsFirstHalf).Select(x => x).FirstOrDefault();
                            {
                                weekMonthDay.FirstHalfRemark = list.LeaveType;
                                weekMonthDay.FirstHalfLeaveType = list.LeaveTypeName;
                                weekMonthDay.FirstHalfLeaveStatus = list.AppliedLeaveStatus == true ? "Leave Approved" : list.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";

                            }
                            var list2 = firstLeave.Where(x => x.IsSecondHalf).Select(x => x).FirstOrDefault();
                            {
                                weekMonthDay.SecondHalfRemark = list2.LeaveType;
                                weekMonthDay.SecondHalfLeaveType = list2.LeaveTypeName;
                                weekMonthDay.SecondHalfLeaveStatus = list2.AppliedLeaveStatus == true ? "Leave Approved" : list2.AppliedLeaveStatus == false ? "Leave Rejected" : "Leave Applied";

                            }
                            //weekMonthDay.LeaveTypeName = firstLeave.LeaveTypeName;
                            //weekMonthDay.LeaveType = firstLeave.LeaveType;
                            weekMonthDay.IsLeave = true;
                            weekMonthDay.Remark = "";
                            weekMonthDay.IsFirstHalf = true;
                            weekMonthDay.IsSecondHalf = true;
                        }
                    }
                    else
                    {
                        Holiday holiday = leaveHoliday?.Holiday?.Where(x => x.HolidayDate == fromDate.AddDays(days).Date).Select(x => x).FirstOrDefault();
                        if (holiday != null)
                        {
                            weekMonthDay.IsHoliday = true;
                            weekMonthDay.Remark = holiday.HolidayName;
                            if (holiday.IsRestrictHoliday == true)
                            {
                                weekMonthDay.LeaveType = "RestrictedHoliday";
                            }
                        }
                        else
                        {
                            if (WeekendShiftList != null)
                            {
                                var dayname = fromDate.AddDays(days).Date.ToString("dddd");
                                if (WeekendShiftList?.WeekendList?.Where(rs => rs.WeekendDayName == dayname).ToList().Count > 0)
                                {
                                    weekMonthDay.IsWeekend = true;
                                    weekMonthDay.Remark = "Weekend";
                                }
                                else
                                {
                                    if (DateTime.Now.Date > fromDate.AddDays(days).Date && fromDate.AddDays(days).Date >= DOJ.Date)
                                    {
                                        weekMonthDay.IsAbsent = true;
                                        weekMonthDay.Remark = "Full Day Absent";
                                    }
                                }
                            }
                            else
                            {
                                if (DateTime.Now.Date > fromDate.AddDays(days).Date && fromDate.AddDays(days).Date >= DOJ.Date)
                                {
                                    weekMonthDay.IsAbsent = true;
                                    weekMonthDay.Remark = "Full Day Absent";
                                }
                            }
                        }
                    }
                    WeeklyAttendanceDetail.Add(weekMonthDay);
                }
            }
            if (restrictionView != null && (restrictionView?.WeekendsBetweenAttendacePeriod == true || restrictionView?.HolidaysBetweenAttendancePeriod == true))
            {
                List<WeeklyMonthlyAttendance> attendanceListView = new();
                List<WeeklyMonthlyAttendance> attendances = new();
                attendances = WeeklyAttendanceDetail;
                DateTime? startDate = WeeklyAttendanceDetail.Select(x => x.Date).FirstOrDefault();
                DateTime? endDate = WeeklyAttendanceDetail.Select(x => x.Date).LastOrDefault();
                attendanceListView = WeeklyAttendanceDetail.Where(x => x.Date >= startDate.Value.AddDays(+7) && x.Date <= endDate.Value.AddDays(-7)).Select(x => x).ToList();
                if (restrictionView.WeekendsBetweenAttendacePeriod == true)
                {
                    //DateTime? firstDay = attendanceListView.Select(x => x.Date).FirstOrDefault();
                    //DateTime? lastDay = attendanceListView.Select(x => x.Date).LastOrDefault();
                    //for (DateTime? days = firstDay; days <= lastDay; days = days.Value.AddDays(+1))
                    foreach (var item in attendanceListView)
                    {
                        bool IsFirstAbsent = false;
                        bool IsSecondAbsent = false;
                        if (item.IsWeekend == true)
                        {
                            var firstday = attendances.Where(x => x.Date == item.Date.Value.Date).Select(x => new WeeklyMonthlyAttendance
                            {
                                Remark = x.Remark,
                                IsAbsent = x.IsAbsent,
                                IsHoliday = x.IsHoliday,
                                IsLeave = x.IsLeave,
                                IsWeekend = x.IsWeekend,
                                TotalHours = x.TotalHours
                            }).FirstOrDefault();
                            WeeklyMonthlyAttendance attends = new();
                            attends = getAbsentDetails(WeekendShiftList, firstday, DOJ, item.Date.Value.Date);
                            if (attends.Remark == "Full Day Absent")
                            {
                                DateTime beforeDate = item.Date.Value.AddDays(-1);
                                while (attendances != null)
                                {
                                    var week = attendances.Where(x => x.Date == beforeDate).Select(x => new WeeklyMonthlyAttendance
                                    {
                                        Remark = x.Remark,
                                        IsAbsent = x.IsAbsent,
                                        IsHoliday = x.IsHoliday,
                                        IsLeave = x.IsLeave,
                                        IsWeekend = x.IsWeekend,
                                        TotalHours = x.TotalHours
                                    }).FirstOrDefault();
                                    WeeklyMonthlyAttendance attend = new();
                                    attend = getAbsentDetails(WeekendShiftList, week, DOJ, beforeDate);
                                    if ((week.IsHoliday == true && attend.Remark == "Full Day Absent") || (week.IsWeekend == true && attend.Remark == "Full Day Absent"))
                                    {
                                        beforeDate = beforeDate.Date.AddDays(-1);
                                        continue;
                                    }
                                    else if (week.IsLeave == true)
                                    {
                                        break;
                                    }
                                    else if (week.IsAbsent == true && attend.Remark == "Full Day Absent")
                                    {
                                        IsFirstAbsent = true;
                                        break;
                                    }
                                    break;
                                }
                                DateTime afterDate = item.Date.Value.AddDays(+1);
                                while (attendances != null)
                                {
                                    var week = attendances.Where(x => x.Date == afterDate).Select(x => new WeeklyMonthlyAttendance
                                    {
                                        Remark = x.Remark,
                                        IsAbsent = x.IsAbsent,
                                        IsHoliday = x.IsHoliday,
                                        IsLeave = x.IsLeave,
                                        IsWeekend = x.IsWeekend,
                                        TotalHours = x.TotalHours
                                    }).FirstOrDefault();
                                    WeeklyMonthlyAttendance attend = new();
                                    attend = getAbsentDetails(WeekendShiftList, week, DOJ, afterDate);
                                    if ((week.IsHoliday == true && attend.Remark == "Full Day Absent") || (week.IsWeekend == true && attend.Remark == "Full Day Absent"))
                                    {
                                        afterDate = afterDate.Date.AddDays(+1);
                                        continue;
                                    }
                                    else if (week.IsLeave == true)
                                    {
                                        break;
                                    }
                                    else if (week.IsAbsent == true && attend.Remark == "Full Day Absent")
                                    {
                                        IsSecondAbsent = true;
                                        break;
                                    }
                                    break;
                                }
                            }
                            if (IsFirstAbsent == true && IsSecondAbsent == true)
                            {
                                item.Remark = "Full Day Absent";
                                item.IsAbsent = true;
                            }
                        }
                    }
                }
                if (restrictionView.HolidaysBetweenAttendancePeriod == true)
                {
                    foreach (var item in attendanceListView)
                    {
                        bool IsFirstAbsent = false;
                        bool IsSecondAbsent = false;
                        if (item.IsHoliday == true)
                        {
                            var firstday = attendances.Where(x => x.Date == item.Date.Value.Date).Select(x => new WeeklyMonthlyAttendance
                            {
                                Remark = x.Remark,
                                IsAbsent = x.IsAbsent,
                                IsHoliday = x.IsHoliday,
                                IsLeave = x.IsLeave,
                                IsWeekend = x.IsWeekend,
                                TotalHours = x.TotalHours
                            }).FirstOrDefault();
                            WeeklyMonthlyAttendance attends = new();
                            attends = getAbsentDetails(WeekendShiftList, firstday, DOJ, item.Date.Value.Date);
                            if (attends.Remark == "Full Day Absent")
                            {
                                DateTime beforeDate = item.Date.Value.AddDays(-1);
                                while (attendances != null)
                                {
                                    var holiday = attendances.Where(x => x.Date == beforeDate).Select(x => new WeeklyMonthlyAttendance
                                    {
                                        Remark = x.Remark,
                                        IsAbsent = x.IsAbsent,
                                        IsHoliday = x.IsHoliday,
                                        IsLeave = x.IsLeave,
                                        IsWeekend = x.IsWeekend,
                                        TotalHours = x.TotalHours
                                    }).FirstOrDefault();
                                    WeeklyMonthlyAttendance attend = new();
                                    attend = getAbsentDetails(WeekendShiftList, holiday, DOJ, beforeDate);
                                    if ((holiday.IsHoliday == true && attend.Remark == "Full Day Absent") || (holiday.IsWeekend == true && attend.Remark == "Full Day Absent"))
                                    {
                                        beforeDate = beforeDate.Date.AddDays(-1);
                                        continue;
                                    }
                                    else if (holiday.IsLeave == true)
                                    {
                                        break;
                                    }
                                    else if (holiday.IsAbsent == true && attend.Remark == "Full Day Absent")
                                    {
                                        IsFirstAbsent = true;
                                        break;
                                    }
                                    break;
                                }
                                DateTime afterDate = item.Date.Value.AddDays(+1);
                                while (attendances != null)
                                {
                                    var holiday = attendances.Where(x => x.Date == afterDate).Select(x => new WeeklyMonthlyAttendance
                                    {
                                        Remark = x.Remark,
                                        IsAbsent = x.IsAbsent,
                                        IsHoliday = x.IsHoliday,
                                        IsLeave = x.IsLeave,
                                        IsWeekend = x.IsWeekend,
                                        TotalHours = x.TotalHours
                                    }).FirstOrDefault();
                                    WeeklyMonthlyAttendance attend = new();
                                    attend = getAbsentDetails(WeekendShiftList, holiday, DOJ, afterDate);
                                    if ((holiday.IsHoliday == true && attend.Remark == "Full Day Absent") || (holiday.IsWeekend == true && attend.Remark == "Full Day Absent"))
                                    {
                                        afterDate = afterDate.Date.AddDays(+1);
                                        continue;
                                    }
                                    else if (holiday.IsLeave == true)
                                    {
                                        break;
                                    }
                                    else if (holiday.IsAbsent == true && attend.Remark == "Full Day Absent")
                                    {
                                        IsSecondAbsent = true;
                                        break;
                                    }
                                    break;
                                }
                            }
                            if (IsFirstAbsent == true && IsSecondAbsent == true)
                            {
                                item.Remark = "Full Day Absent";
                                item.IsAbsent = true;
                            }
                        }
                    }
                }
                WeeklyAttendanceDetail = attendanceListView;
            }
            return WeeklyAttendanceDetail == null ? new List<WeeklyMonthlyAttendance>() : WeeklyAttendanceDetail;
        }
        public WeeklyMonthlyAttendance getAbsentDetails(ShiftViewDetails WeekendShiftList, WeeklyMonthlyAttendance attendanceMarkedDay, DateTime DOJ, DateTime fromDate)
        {
            WeeklyMonthlyAttendance weekMonthDay = new WeeklyMonthlyAttendance();
            TimeSpan? utcTime = new TimeSpan(5, 30, 0);
            string[] hour = attendanceMarkedDay?.TotalHours?.Split(":");
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
            TimeSpan? cutOffTime = (duration + timeFrom);
            TimeSpan? utcTimeTo = new TimeSpan(0, 0, 0);
            if (cutOffTime.Value.Ticks >= utcTime.Value.Ticks)
            {
                utcTimeTo = cutOffTime - utcTime;
            }
            DateTime dtc = new DateTime(utcTimeTo.Value.Ticks);
            DateTime utcCutOff = dtc;
            TimeSpan? cutOffHour = utcCutOff.TimeOfDay;
            weekMonthDay = attendanceMarkedDay;
            int i = 0;
            TimeSpan? CurrentTime = new TimeSpan();
            TimeSpan? LastCheckoutTime = new TimeSpan();
            if (attendanceMarkedDay?.WeeklyMonthlyAttendanceDetail?.Count > 0)
            {
                //DateTime checkinTime = attendanceMarkedDay.WeeklyMonthlyAttendanceDetail[i].CheckinTime.Value;
                CurrentTime = attendanceMarkedDay?.WeeklyMonthlyAttendanceDetail[i]?.CheckinTime.Value.TimeOfDay;
                DateTime? lastCheckOut = attendanceMarkedDay?.WeeklyMonthlyAttendanceDetail[(attendanceMarkedDay.WeeklyMonthlyAttendanceDetail.Count) - 1]?.CheckoutTime;
                if(lastCheckOut != null)
                {
                    LastCheckoutTime = lastCheckOut.Value.TimeOfDay;
                    if (LastCheckoutTime > CurrentTime)
                    {
                        TimeSpan? diffHour = LastCheckoutTime - CurrentTime;
                        if (diffHour != null)
                        {
                            CurrentTime = new TimeSpan(CurrentTime.Value.Ticks + (diffHour.Value.Ticks / 2));
                        }
                    }
                }                
            }
            if (WeekendShiftList?.IsConsiderAbsent == true || WeekendShiftList?.IsConsiderHalfaDay == true || WeekendShiftList?.IsConsiderPresent == true)
            {
                //Absent
                if (WeekendShiftList?.IsConsiderAbsent == true && fromDate.Date < DateTime.Now.Date && fromDate.Date >= DOJ.Date)
                {
                    if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours > absentFromHour && totalHours > absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours > absentFromHour && totalHours < absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours > absentFromHour && totalHours >= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours > absentFromHour && totalHours <= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                    {
                        if (totalHours > absentFromHour && totalHours == absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours < absentFromHour && totalHours > absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours < absentFromHour && totalHours < absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours < absentFromHour && totalHours <= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours < absentFromHour && totalHours >= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lessthan" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                    {
                        if (totalHours < absentFromHour && totalHours == absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours >= absentFromHour && totalHours > absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours >= absentFromHour && totalHours < absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours >= absentFromHour && totalHours >= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours >= absentFromHour && totalHours <= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                    {
                        if (totalHours >= absentFromHour && totalHours == absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours <= absentFromHour && totalHours > absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours <= absentFromHour && totalHours < absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours <= absentFromHour && totalHours >= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours <= absentFromHour && totalHours <= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                    {
                        if (totalHours <= absentFromHour && totalHours == absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours == absentFromHour && totalHours > absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours == absentFromHour && totalHours < absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours == absentFromHour && totalHours >= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours == absentFromHour && totalHours <= absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                    else if (WeekendShiftList?.AbsentFromOperator.ToLower() == "equalto" && WeekendShiftList?.AbsentToOperator.ToLower() == "equalto")
                    {
                        if (totalHours == absentFromHour && totalHours == absentToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Full Day Absent";
                        }
                    }
                }
                //Half Day Absent
                if (WeekendShiftList?.IsConsiderHalfaDay == true && fromDate.Date < DateTime.Now.Date && fromDate.Date >= DOJ.Date)
                {
                    if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours > halfaDayFromHour && totalHours > halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours > halfaDayFromHour && totalHours < halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours > halfaDayFromHour && totalHours >= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours > halfaDayFromHour && totalHours <= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                    {
                        if (totalHours > halfaDayFromHour && totalHours == halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours < halfaDayFromHour && totalHours > halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours < halfaDayFromHour && totalHours < halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours < halfaDayFromHour && totalHours <= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours < halfaDayFromHour && totalHours >= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lessthan" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                    {
                        if (totalHours < halfaDayFromHour && totalHours == halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours >= halfaDayFromHour && totalHours > halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours >= halfaDayFromHour && totalHours < halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours >= halfaDayFromHour && totalHours >= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours >= halfaDayFromHour && totalHours <= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "greaterthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                    {
                        if (totalHours >= halfaDayFromHour && totalHours == halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours <= halfaDayFromHour && totalHours > halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours <= halfaDayFromHour && totalHours < halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours <= halfaDayFromHour && totalHours >= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours <= halfaDayFromHour && totalHours <= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "lesserthanequalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                    {
                        if (totalHours <= halfaDayFromHour && totalHours <= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours == halfaDayFromHour && totalHours > halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lessthan")
                    {
                        if (totalHours == halfaDayFromHour && totalHours < halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours == halfaDayFromHour && totalHours >= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours == halfaDayFromHour && totalHours <= halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                    else if (WeekendShiftList?.HalfaDayFromOperator.ToLower() == "equalto" && WeekendShiftList?.HalfaDayToOperator.ToLower() == "equalto")
                    {
                        if (totalHours == halfaDayFromHour && totalHours == halfaDayToHour)
                        {
                            weekMonthDay.IsAbsent = true;
                            weekMonthDay.Remark = "Half Day Absent";
                            if (CurrentTime != null && CurrentTime <= cutOffHour)
                            {
                                weekMonthDay.IsSecondHalfAbsent = true;
                            }
                            else
                            {
                                weekMonthDay.IsFirstHalfAbsent = true;
                            }
                        }
                    }
                }
                //Present
                if (WeekendShiftList?.IsConsiderPresent == true && fromDate.Date < DateTime.Now.Date && fromDate.Date >= DOJ.Date)
                {
                    if (WeekendShiftList?.PresentHourOperator.ToLower() == "greaterthan")
                    {
                        if (totalHours > presentHour)
                        {
                            weekMonthDay = attendanceMarkedDay;
                        }
                    }
                    else if (WeekendShiftList?.PresentHourOperator.ToLower() == "lessthan")
                    {
                        if (totalHours < presentHour)
                        {
                            weekMonthDay = attendanceMarkedDay;
                        }
                    }
                    else if (WeekendShiftList?.PresentHourOperator.ToLower() == "greaterthanequalto")
                    {
                        if (totalHours >= presentHour)
                        {
                            weekMonthDay = attendanceMarkedDay;
                        }
                    }
                    else if (WeekendShiftList?.PresentHourOperator.ToLower() == "lesserthanequalto")
                    {
                        if (totalHours <= presentHour)
                        {
                            weekMonthDay = attendanceMarkedDay;
                        }
                    }
                    else if (WeekendShiftList?.PresentHourOperator.ToLower() == "equalto")
                    {
                        if (totalHours == presentHour)
                        {
                            weekMonthDay = attendanceMarkedDay;
                        }
                    }
                }
                if (totalHours != null && fromDate.Date < DateTime.Now.Date && fromDate.Date >= DOJ.Date)
                {
                    if (totalHours.Value.Ticks == 0)
                    {
                        weekMonthDay.IsAbsent = true;
                        weekMonthDay.Remark = "Full Day Absent";
                    }
                    else
                    {
                        weekMonthDay = attendanceMarkedDay;
                    }
                }
            }
            else
            {
                string[] shifthour = WeekendShiftList?.TotalHours?.Split(":");
                if (shifthour != null && shifthour?.Length > 0)
                {
                    TimeSpan? shiftHours = new TimeSpan(shifthour?.Length > 0 ? Convert.ToInt32(shifthour[0]) : 0, shifthour?.Length > 1 ? Convert.ToInt32(shifthour[1]) : 0, shifthour?.Length > 2 ? Convert.ToInt32(shifthour[2]) : 0);
                    if (totalHours.Value.Ticks < shiftHours.Value.Ticks && fromDate.Date < DateTime.Now.Date && fromDate.Date >= DOJ.Date)
                    {
                        weekMonthDay.IsAbsent = true;
                        weekMonthDay.Remark = "Full Day Absent";
                    }
                    else
                    {
                        weekMonthDay = attendanceMarkedDay;
                    }
                }
                else
                {
                    if (fromDate.Date < DateTime.Now.Date && fromDate.Date >= DOJ.Date)
                    {
                        weekMonthDay.IsAbsent = true;
                        weekMonthDay.Remark = "Full Day Absent";
                    }
                }
            }
            return weekMonthDay;
        }
        #region Leave Notification And Mail
        public async Task<string> NotificationMail(SendEmailView sendMailDetails)
        {
            try
            {
                string MailSubject = null, MailBody = null, Subject = null, Body = null;
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                SendEmailView sendMailbyleaverequest = new();
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
                LoggerManager.LoggingErrorTrack(ex, "Common", "Common/NotificationMail", JsonConvert.SerializeObject(sendMailDetails));

            }
            return string.Empty;
        }
        #endregion

        #region Leave Notification
        public async Task<string> Notification(Notifications notification)
        {
            try
            {
                string statusText = "";
                SendEmailView sendMailbyleaverequest = new();
                //Notification Alert                                   
                List<Notifications> notifications = new List<Notifications>();
                Notifications empNotification = new Notifications();
                empNotification = new Notifications
                {

                    CreatedBy = notification.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    FromId = notification.FromId,
                    ToId = notification.ToId,
                    MarkAsRead = false,
                    NotificationSubject = notification.NotificationSubject,
                    NotificationBody = notification.NotificationBody,
                    PrimaryKeyId = notification.PrimaryKeyId,
                    ButtonName = notification.ButtonName,
                    SourceType = notification.SourceType,
                    Data = notification.Data
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
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Common", "Common/Notification", JsonConvert.SerializeObject(notification));

            }
            return string.Empty;
        }
        #endregion

        #region Leave Notification Mail for Policy Mgmt
        public string NotificationMailForPolicyMgmt(List<SendEmailView> mailList)
        {
            try
            {
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                foreach (var sendMailDetails in mailList)
                {
                    SendEmailView sendMailbyleaverequest = new();
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
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Common", "Common/NotificationMailForPolicyMgmt", JsonConvert.SerializeObject(mailList));

            }
            return string.Empty;
        }
        #endregion

    }
}
