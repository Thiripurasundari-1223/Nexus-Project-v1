using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static SharedLibraries.ViewModels.Leaves.WeeklyOverviewReportView;

namespace Leaves.DAL.Repository
{
    public interface ILeaveApplyRepository : IBaseRepository<ApplyLeaves>
    {
        ApplyLeaves GetApplyleaveById(int leaveId);
        //List<AppliedLeaveView> GetAllAppliedLeavesByEmpId(int employeeId, DateTime FromDate, DateTime ToDate);
        AppliedLeaveEditView GetAppliedLeaveToEdit(int leaveId);
        ApplyLeaves GetAppliedleaveByIdToDelete(int leaveId);
        List<ApplyLeaves> GetAppliedleaveByEmployeeId(int employeeId,int leaveTypeId);
        ApplyLeaves GetTeamLeaveByID(int pLeaveId);
        List<AvailableLeaveDetailsView> GetAvailableLeaveDetailsByEmployeeId(int employeeId, int departmentId);
        AttendanceDaysAndHoursDetailsView GetAttendanceLeaveDetailsByEmployeeId(int employeeId, int departmentId);

        List<LeaveData> GetWeeklyLeavesByEmployeeId(int employeeId);
        List<AvailableLeaveDetailsView> GetAvailableLeaveDetailsByEmployee(int employeeID);
        List<LeaveDurationListView> GetAvailableLeaveDurationList();
        List<AppConstantsView> GetAppconstantList(string AppConstantType);
        List<HolidayViewDetails> GetCurrentFinancialYearHolidayList();
        List<LeaveTypesDetailView> GetActiveLeaveTypeList();
        LeaveBalanceView GetLeaveBalanceByEmployeeId(EmployeeListView employee);
        LeaveBalanceList GetEmployeeLeaveBalance(int employeeId, DateTime fromDate, DateTime toDate);
        List<ApplyLeaves> GetAppliedleaveByTypeId(int leaveTypeId);
        List<EmployeeAvailableLeaveDetails> GetActiveLeaveBalanceDetails(EmployeeListView employee);
    }
    public class LeaveApplyRepository : BaseRepository<ApplyLeaves>, ILeaveApplyRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveApplyRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        #region Get Applied Leave By Leave Id
        public ApplyLeaves GetApplyleaveById(int leaveId)
        {
            ApplyLeaves LeaveDetail = new ApplyLeaves();
            LeaveDetail = dbContext.ApplyLeave.Where(x => x.LeaveId == leaveId).FirstOrDefault();
            if (LeaveDetail != null)
            {
                return LeaveDetail;
            }
            return LeaveDetail;
        }
        #endregion

        //#region Get AppliedLeave By employee Id
        //public List<AppliedLeaveView> GetAllAppliedLeavesByEmpId(int employeeId, DateTime FromDate, DateTime ToDate)
        //{
        //    List<AppliedLeaveView> appliedLeaveList = (from leave in dbContext.ApplyLeave
        //                                               join ldetail in dbContext.LeaveTypes on leave.LeaveTypeId equals ldetail.LeaveTypeId
        //                                               //join aldetails in dbContext.AppliedLeaveDetails on leave.LeaveId equals aldetails.LeaveId
        //                                               where leave.EmployeeId == employeeId && leave.FromDate >= FromDate && leave.ToDate <= ToDate && leave.IsActive == true
        //                                               select new AppliedLeaveView
        //                                               {
        //                                                   LeaveId = leave.LeaveId,
        //                                                   EmployeeId = leave.EmployeeId,
        //                                                   LeaveTypeId = leave.LeaveTypeId,
        //                                                   LeaveType = ldetail.LeaveType,
        //                                                   FromDate = leave.FromDate,
        //                                                   ToDate = leave.ToDate,
        //                                                   NoOfDays = leave.NoOfDays,
        //                                                   Reason = leave.Reason,
        //                                                   Status = leave.Status,
        //                                                   IsActive = leave.IsActive,
        //                                                   AppliedLeaveDetails = dbContext.AppliedLeaveDetails.Where(x => x.LeaveId == leave.LeaveId).Select(x =>
        //                                                                        new AppliedLeaveDetailsView
        //                                                                        {
        //                                                                            AppliedLeaveDetailsID = x.AppliedLeaveDetailsID,
        //                                                                            LeaveId = x.LeaveId,
        //                                                                            Date = x.Date,
        //                                                                            IsFullDay = x.IsFullDay,
        //                                                                            IsFirstHalf = x.IsFirstHalf,
        //                                                                            IsSecondHalf = x.IsSecondHalf,
        //                                                                            CompensatoryOffId = x.CompensatoryOffId,
        //                                                                            AppliedLeaveStatus = x.AppliedLeaveStatus,
        //                                                                            CreatedBy = x.CreatedBy,
        //                                                                        }
        //                                                                       ).ToList()
        //                                               }).ToList();
        //    return appliedLeaveList;
        //}
        //#endregion

        #region Get AppliedLeave By employee Id
        public AppliedLeaveEditView GetAppliedLeaveToEdit(int leaveId)
        {
            ApplyLeaves applyLeaves = dbContext.ApplyLeave.Where(x => x.LeaveId == leaveId).FirstOrDefault();
            if (applyLeaves != null && applyLeaves?.LeaveId > 0)
            {
                AppliedLeaveEditView applyLeavesView = new AppliedLeaveEditView();
                applyLeavesView.LeaveId = applyLeaves.LeaveId;
                applyLeavesView.EmployeeId = applyLeaves.EmployeeId;
                applyLeavesView.LeaveTypeId = applyLeaves.LeaveTypeId;
                applyLeavesView.FromDate = applyLeaves.FromDate;
                applyLeavesView.ToDate = applyLeaves.ToDate;
                applyLeavesView.Reason = applyLeaves.Reason;
                applyLeavesView.CreatedBy = applyLeaves.CreatedBy;
                List<AppliedLeaveDetails> lstAppliedLeave = new List<AppliedLeaveDetails>();
                List<AppliedLeaveDetailsView> appliedLeaveDetailsViews = new List<AppliedLeaveDetailsView>();
                lstAppliedLeave = dbContext.AppliedLeaveDetails.Where(x => x.LeaveId == leaveId).ToList();
                foreach (AppliedLeaveDetails appliedLeave in lstAppliedLeave)
                {
                    AppliedLeaveDetailsView appliedLeaveDetailsView = new AppliedLeaveDetailsView
                    {
                        AppliedLeaveDetailsID = appliedLeave.AppliedLeaveDetailsID,
                        LeaveId = appliedLeave.LeaveId,
                        Date = appliedLeave.Date,
                        IsFullDay = appliedLeave.IsFullDay,
                        IsFirstHalf = appliedLeave.IsFirstHalf,
                        IsSecondHalf = appliedLeave.IsSecondHalf,
                        CompensatoryOffId = appliedLeave.CompensatoryOffId,
                        CreatedBy = appliedLeave.CreatedBy,
                    };
                    appliedLeaveDetailsViews.Add(appliedLeaveDetailsView);
                }
                applyLeavesView.AppliedLeaveDetails = appliedLeaveDetailsViews;
                return applyLeavesView;
            }
            return null;
        }
        #endregion

        #region Get Applied Leave to Delete By Leave Id
        public ApplyLeaves GetAppliedleaveByIdToDelete(int leaveId)
        {
            ApplyLeaves LeaveDetail = new ApplyLeaves();
            LeaveDetail = dbContext.ApplyLeave.Where(x => x.LeaveId == leaveId).FirstOrDefault();
            if (LeaveDetail != null)
            {
                return LeaveDetail;
            }
            return LeaveDetail;
        }
        #endregion 
        #region Get Applied Leave by employeeId
        public List<ApplyLeaves> GetAppliedleaveByEmployeeId(int employeeId, int leaveTypeId)
        {
             
            List<ApplyLeaves> LeaveDetail = dbContext.ApplyLeave.Where(x => x.EmployeeId == employeeId && x.LeaveTypeId== leaveTypeId).ToList();
            
            return LeaveDetail==null?new List<ApplyLeaves>(): LeaveDetail;
        }
        #endregion

        #region Get Available Leaves By employee Id
        public List<AvailableLeaveDetailsView> GetAvailableLeaveDetailsByEmployeeId(int employeeId, int departmentId)
        {
            List<TotalAvailableView> TotalAvailable = new List<TotalAvailableView>();
            TotalAvailable = (from leave in dbContext.LeaveDepartment
                              join ldetail in dbContext.LeaveTypes on leave.LeaveTypeId equals ldetail.LeaveTypeId
                              where (leave.LeaveApplicableDepartmentId == 0 || leave.LeaveApplicableDepartmentId == departmentId)
                              && leave.LeaveExceptionDepartmentId != departmentId
                              select new TotalAvailableView
                              {
                                  LeaveTypeId = ldetail.LeaveTypeId,
                                  LeaveType = ldetail.LeaveType,
                                  TotalLeaves = ldetail.LeaveAccruedNoOfDays,
                              }).Distinct().ToList();
            List<AppliedLeavesView> AppliedLeaves = new List<AppliedLeavesView>();
            AppliedLeaves = (from leave in dbContext.ApplyLeave
                             join ldetail in dbContext.LeaveTypes on leave.LeaveTypeId equals ldetail.LeaveTypeId
                             where leave.EmployeeId == employeeId
                             group leave by leave.LeaveTypeId into g
                             select new AppliedLeavesView
                             {
                                 LeaveTypeId = g.Key,
                                 NoOfLeavesTaken = g.Sum(x => x.NoOfDays),
                             }).ToList();
            TotalAvailable?.ForEach(x => x.AppliedLeavesView = AppliedLeaves?.Where(y => y.LeaveTypeId == x.LeaveTypeId).ToList());
            List<AvailableLeaveDetailsView> availableLeaveDetailsView = new List<AvailableLeaveDetailsView>();
            if (TotalAvailable?.Count > 0)
            {
                for (int i = 0; i <= TotalAvailable?.Count - 1; i++)
                {
                    AvailableLeaveDetailsView availleave = new AvailableLeaveDetailsView
                    {
                        LeaveTypeId = TotalAvailable[i].LeaveTypeId,
                        LeaveType = TotalAvailable[i].LeaveType,
                        AvailableLeaves = TotalAvailable[i].TotalLeaves,
                        BookedLeaves = TotalAvailable[i].AppliedLeavesView.Count > 0 ? TotalAvailable[i].AppliedLeavesView[0].NoOfLeavesTaken : 0,
                        //AvailableLeaves = TotalAvailable[i].AppliedLeavesView.Count > 0 ?
                        //(TotalAvailable[i].TotalLeaves - TotalAvailable[i].AppliedLeavesView[0].NoOfLeavesTaken) : TotalAvailable[i].TotalLeaves,
                    };
                    availleave.LeaveRestrictionsViewDetails = (from lr in dbContext.LeaveRestrictions
                                                               join lt in dbContext.LeaveTypes on lr.LeaveTypeId equals lt.LeaveTypeId
                                                               where lr.LeaveTypeId == TotalAvailable[i].LeaveTypeId
                                                               select new LeaveRestrictionsViewDetails
                                                               {
                                                                   LeaveRestrictionsId = lr.LeaveRestrictionsId,
                                                                   //WeekendCountAfterDays = x.lr.WeekendCountAfterDays,
                                                                   //HolidayCountAfterDays = x.lr.HolidayCountAfterDays,
                                                                   ExceedLeaveBalance = lr.ExceedLeaveBalance,
                                                                   AllowUsersViewId = lr.AllowUsersViewId,
                                                                   BalanceDisplayedId = lr.BalanceDisplayedId,
                                                                   DaysInAdvance = lr.DaysInAdvance,
                                                                   AllowRequestNextDays = lr.AllowRequestNextDays,
                                                                   DatesAppliedAdvance = lr.DatesAppliedAdvance,
                                                                   MaximumLeavePerApplication = lr.MaximumLeavePerApplication,
                                                                   MinimumGapTwoApplication = lr.MinimumGapTwoApplication,
                                                                   MaximumConsecutiveDays = lr.MaximumConsecutiveDays,
                                                                   EnableFileUpload = lr.EnableFileUpload,
                                                                   MinimumNoOfApplicationsPeriod = lr.MinimumNoOfApplicationsPeriod,
                                                                   AllowRequestPeriodId = lr.AllowRequestPeriodId,
                                                                   MaximumLeave = lr.MaximumLeave,
                                                                   MinimumGap = lr.MinimumGap,
                                                                   MaximumConsecutive = lr.MaximumConsecutive,
                                                                   EnableFile = lr.EnableFile,
                                                                   CannotBeTakenTogether = lr.CannotBeTakenTogether,
                                                                   AllowRequestDates = lr.AllowRequestDates,
                                                                   LeaveTypeId = lr.LeaveTypeId,
                                                                   AllowPastDates = lr.AllowPastDates,
                                                                   AllowFutureDates = lr.AllowFutureDates,
                                                                   IsAllowRequestNextDays = lr.IsAllowRequestNextDays,
                                                                   IsToBeApplied = lr.IsToBeApplied,
                                                                   Weekendsbetweenleaveperiod = lr.Weekendsbetweenleaveperiod,
                                                                   Holidaybetweenleaveperiod = lr.Holidaybetweenleaveperiod,
                                                                   DurationsAllowedDetails = dbContext.LeaveDuration.Join(dbContext.AppConstants, ld => ld.DurationId, ac => ac.AppConstantId, (ld, ac) => new { ld, ac }).Where(rs => rs.ld.LeaveTypeId == lr.LeaveTypeId).Select(rs => new DurationsAllowedDetails { LeaveTypeId = rs.ld.LeaveTypeId, DurationId = rs.ac.AppConstantId, DisplayName = rs.ac.DisplayName, AppConstantValue = rs.ac.AppConstantValue }).ToList()
                                                                   //DurationsAllowedDetails = dbContext.LeaveDuration.Join(dbContext.AppConstants, ld => ld.AppConstantId, ac => ac.AppConstantId, (ld, ac) => new { ld, ac }).Where(rs => rs.ld.LeaveTypeId == lr.LeaveTypeId).Select(rs => new DurationsAllowedDetails { LeaveTypeId = rs.ld.LeaveTypeId, AppConstantId = rs.ac.AppConstantId, DisplayName = rs.ac.DisplayName, AppConstantValue = rs.ac.AppConstantValue }).ToList()
                                                               }).FirstOrDefault();
                    //};
                    availableLeaveDetailsView.Add(availleave);
                }
            }
            return availableLeaveDetailsView;
        }
        #endregion

        #region Get Team Leave By ID
        public ApplyLeaves GetTeamLeaveByID(int LeaveId)
        {
            return dbContext.ApplyLeave.Where(x => x.LeaveId == LeaveId).FirstOrDefault();
        }
        #endregion

        #region Get AppliedLeave By employee Id
        public List<LeaveData> GetWeeklyLeavesByEmployeeId(int employeeId)
        {
            return (from leave in dbContext.ApplyLeave
                    join ldetail in dbContext.LeaveTypes on leave.LeaveTypeId equals ldetail.LeaveTypeId
                    where leave.EmployeeId == employeeId
                    select new LeaveData
                    {
                        LeaveTypeId = leave.LeaveTypeId,
                        EmployeeId = leave.EmployeeId,
                        FromDate = leave.FromDate,
                        ToDate = leave.ToDate,
                        NoOfDays = leave.NoOfDays,
                        TotalLeaves = ldetail.LeaveAccruedNoOfDays
                    }).ToList();
        }
        #endregion

        #region
        public AttendanceDaysAndHoursDetailsView GetAttendanceLeaveDetailsByEmployeeId(int employeeId, int departmentId)
        {
            AttendanceDaysAndHoursDetailsView attendanceDaysAndHoursDetailsViews = new AttendanceDaysAndHoursDetailsView();
            attendanceDaysAndHoursDetailsViews = new AttendanceDaysAndHoursDetailsView()
            {
                PayableDays = (from leave in dbContext.LeaveDepartment
                               where leave.LeaveApplicableDepartmentId == departmentId
                               select new { leave.LeaveTypeId }).ToList().Distinct().Count(),
                Holidays = (from holiday in dbContext.Holiday
                            join department in dbContext.HolidayDepartment on holiday.HolidayID equals department.HolidayId
                            where department.DepartmentId == departmentId
                            select new { holiday.HolidayID }).ToList().Distinct().Count(),
            };
            return attendanceDaysAndHoursDetailsViews;
        }
        #endregion

        #region Get Available Leaves By employee
        public List<AvailableLeaveDetailsView> GetAvailableLeaveDetailsByEmployee(int employeeID)
        {
            List<TotalAvailableView> TotalAvailable = new List<TotalAvailableView>();
            TotalAvailable = (from leave in dbContext.LeaveDepartment
                              join ldetail in dbContext.LeaveTypes on leave.LeaveTypeId equals ldetail.LeaveTypeId
                              where (leave.LeaveApplicableDepartmentId == 0 || leave.LeaveApplicableDepartmentId == 5)
                              && leave.LeaveExceptionDepartmentId != 5
                              select new TotalAvailableView
                              {
                                  LeaveTypeId = ldetail.LeaveTypeId,
                                  LeaveType = ldetail.LeaveType,
                                  TotalLeaves = ldetail.LeaveAccruedNoOfDays
                              }).Distinct().ToList();
            List<AppliedLeavesView> AppliedLeaves = new List<AppliedLeavesView>();
            AppliedLeaves = (from leave in dbContext.ApplyLeave
                             join ldetail in dbContext.LeaveTypes on leave.LeaveTypeId equals ldetail.LeaveTypeId
                             where leave.EmployeeId == employeeID
                             group leave by leave.LeaveTypeId into g
                             select new AppliedLeavesView
                             {
                                 LeaveTypeId = g.Key,
                                 NoOfLeavesTaken = g.Sum(x => x.NoOfDays),
                             }).ToList();
            TotalAvailable?.ForEach(x => x.AppliedLeavesView = AppliedLeaves?.Where(y => y.LeaveTypeId == x.LeaveTypeId).ToList());
            List<AvailableLeaveDetailsView> availableLeaveDetailsView = new List<AvailableLeaveDetailsView>();
            if (TotalAvailable?.Count > 0)
            {
                for (int i = 0; i <= TotalAvailable?.Count - 1; i++)
                {
                    AvailableLeaveDetailsView availleave = new AvailableLeaveDetailsView
                    {
                        LeaveTypeId = TotalAvailable[i].LeaveTypeId,
                        LeaveType = TotalAvailable[i].LeaveType,
                        AvailableLeaves = TotalAvailable[i].AppliedLeavesView.Count > 0 ?
                        (TotalAvailable[i].TotalLeaves - TotalAvailable[i].AppliedLeavesView[0].NoOfLeavesTaken) : TotalAvailable[i].TotalLeaves,
                    };
                    availableLeaveDetailsView.Add(availleave);
                }
            }
            return availableLeaveDetailsView;
        }
        #endregion
        #region Get Available Leave Duration List
        public List<LeaveDurationListView> GetAvailableLeaveDurationList()
        {
            List<LeaveDurationListView> levedurationList = dbContext.LeaveDuration.Join(dbContext.AppConstants, ld => ld.DurationId, ac => ac.AppConstantId, (ld, ac) => new { ld, ac }).OrderBy(rs => rs.ld.LeaveTypeId)
                .Select(rs => new LeaveDurationListView { LeaveTypeId = rs.ld.LeaveTypeId, DurationId = rs.ld.DurationId, AppConstantType = rs.ac.AppConstantType, DisplayName = rs.ac.DisplayName, AppConstantValue = rs.ac.AppConstantValue }).ToList();
            return levedurationList;
        }
        #endregion
        #region Get Appconstant Type List
        public List<AppConstantsView> GetAppconstantList(string AppConstantType)
        {
            List<AppConstantsView> appConstantList = dbContext.AppConstants.Where(x => x.AppConstantType == AppConstantType).Select(x => new AppConstantsView { AppConstantId = x.AppConstantId, AppConstantType = x.AppConstantType, DisplayName = x.DisplayName, AppConstantValue = x.AppConstantValue }).ToList();
            return appConstantList;
        }
        #endregion
        #region Get Current Financial year Holiday List
        public List<HolidayViewDetails> GetCurrentFinancialYearHolidayList()
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            DateTime fromDate, toDate;
            if (currentMonth <= 3)
            {
                fromDate = new DateTime(currentYear - 1, 4, 1);
                toDate = new DateTime(currentYear, 3, 31);
            }
            else
            {
                fromDate = new DateTime(currentYear, 4, 1);
                toDate = new DateTime(currentYear + 1, 3, 31);
            }
            List<HolidayViewDetails> holidayList = dbContext.Holiday.Where(x => x.HolidayDate >= fromDate && x.HolidayDate <= toDate && x.IsActive == true)
                .Select(x => new HolidayViewDetails
                { HolidayID = x.HolidayID, Year = x.Year, HolidayName = x.HolidayName, HolidayCode = x.HolidayCode, HolidayDescription = x.HolidayDescription, HolidayDate = x.HolidayDate, IsActive = x.IsActive, IsRestrictHoliday = x.IsRestrictHoliday }).ToList();
            return holidayList;
        }
        #endregion
        #region Get Active Leave Type Details
        public List<LeaveTypesDetailView> GetActiveLeaveTypeList()
        {
            DateTime todate = DateTime.Now;
            List<LeaveTypesDetailView> activeLeaveList = dbContext.LeaveTypes.Where(x => x.EffectiveToDate == null || x.EffectiveToDate >= todate)
                .Select(x => new LeaveTypesDetailView
                { LeaveTypeId = x.LeaveTypeId, LeaveType = x.LeaveType, LeaveCode = x.LeaveCode, LeaveAccruedType = x.LeaveAccruedType, LeaveAccruedDay = x.LeaveAccruedDay, LeaveAccruedNoOfDays = x.LeaveAccruedNoOfDays, LeaveDescription = x.LeaveDescription, ProRate = x.ProRate, EffectiveFromDate = x.EffectiveFromDate, EffectiveToDate = x.EffectiveToDate }).ToList();
            return activeLeaveList;
        }
        #endregion
        #region Get Active Leave Balance Details
        public List<EmployeeAvailableLeaveDetails> GetActiveLeaveBalanceDetails(EmployeeListView employee)
        {
            List<int> employeeList = employee?.EmployeeDetails?.Select(x => x.EmployeeID).ToList();
            List<EmployeeAvailableLeaveDetails> EmployeeAvailableLeaveListDetails = dbContext.EmployeeLeaveDetails
                 .Join(dbContext.LeaveTypes, eld => eld.LeaveTypeID, lt => lt.LeaveTypeId, (eld, lt) => new { eld, lt })
                 .Join(dbContext.LeaveRestrictions, eldlt => eldlt.lt.LeaveTypeId, lr => lr.LeaveTypeId, (eldlt, lr) => new { eldlt, lr })
                 .Join(dbContext.LeaveEntitlement, eldltlr => eldltlr.eldlt.eld.LeaveTypeID, le => le.LeaveTypeId, (eldltlr, le) => new { eldltlr, le })
                 .Where(x => employeeList.Contains(x.eldltlr.eldlt.eld.EmployeeID == null ? 0 : (int)x.eldltlr.eldlt.eld.EmployeeID) && x.eldltlr.eldlt.lt.IsActive == true && x.eldltlr.eldlt.lt.EffectiveFromDate <= employee.ToDate && (x.eldltlr.eldlt.lt.EffectiveToDate == null || x.eldltlr.eldlt.lt.EffectiveToDate >= employee.FromDate))
                 .Select(x => new EmployeeAvailableLeaveDetails
                 {
                     EmployeeID = x.eldltlr.eldlt.eld.EmployeeID,
                     LeaveTypeID = x.eldltlr.eldlt.eld.LeaveTypeID,
                     BalanceLeave = x.eldltlr.eldlt.eld.BalanceLeave,
                     ActualBalanceLeave = (x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate != null) ? x.eldltlr.eldlt.eld.AdjustmentBalanceLeave : x.eldltlr.eldlt.eld.BalanceLeave,
                     BookedLeaveCount = dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, al => al.LeaveId, ald => ald.LeaveId, (al, ald) => new { al, ald }).Where(rs => rs.al.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && rs.al.EmployeeId == x.eldltlr.eldlt.eld.EmployeeID && rs.al.Status != "Cancelled" && rs.ald.AppliedLeaveStatus != false && rs.al.FromDate >= employee.FromDate && rs.al.ToDate <= employee.ToDate).Sum(rs => (rs.ald.IsFirstHalf == true || rs.ald.IsSecondHalf == true) ? 0.5m : 1),
                     LeaveType = x.eldltlr.eldlt.lt.LeaveType,
                     LeaveCode = x.eldltlr.eldlt.lt.LeaveCode,
                     LeaveAccruedType = x.eldltlr.eldlt.lt.LeaveAccruedType,
                     LeaveAccruedDay = x.eldltlr.eldlt.lt.LeaveAccruedDay,
                     LeaveAccruedNoOfDays = x.eldltlr.eldlt.lt.LeaveAccruedNoOfDays,
                     LeaveTypesId = x.eldltlr.eldlt.lt.LeaveTypesId,
                     ProRate = x.eldltlr.eldlt.lt.ProRate,
                     EffectiveFromDate = x.eldltlr.eldlt.lt.EffectiveFromDate,
                     EffectiveToDate = x.eldltlr.eldlt.lt.EffectiveToDate,
                     CarryForwardId = x.le.CarryForwardId,
                     MaximumCarryForwardDays = x.le.MaximumCarryForwardDays,
                     ReimbursementId = x.le.ReimbursementId,
                     MaximumReimbursementDays = x.le.MaximumReimbursementDays,
                     ResetYear = x.le.ResetYear,
                     ResetMonth = x.le.ResetMonth,
                     ResetDay = x.le.ResetDay,
                     AdjustmentEffectiveFromDate = x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate,
                     AdjustmentBalanceLeave = x.eldltlr.eldlt.eld.AdjustmentBalanceLeave,
                     BalanceBasedOn = dbContext.AppConstants.Where(rs => rs.AppConstantId == x.eldltlr.eldlt.lt.BalanceBasedOn).Select(rs => new BalanceBasedOnDetails { BalanceBasedOnId = rs.AppConstantId, BalanceBasedOnText = rs.DisplayName, BalanceBasedOnValue = rs.AppConstantValue }).ToList(),
                     ResetPeriod = dbContext.AppConstants.Where(y => y.AppConstantId == x.le.ResetYear).Select(z => z.AppConstantValue).FirstOrDefault(),
                     LeaveCarryForward = dbContext.LeaveCarryForward.Where(y => y.EmployeeID == x.eldltlr.eldlt.eld.EmployeeID && y.LeaveTypeID == x.eldltlr.eldlt.eld.LeaveTypeID).OrderByDescending(z => z.ResetDate).ToList(),
                     ToBeAdvanced = x.eldltlr.lr.ToBeAdvanced,
                     ResetLeaveAfter = x.eldltlr.lr.GrantResetLeaveAfterDays,
                     LeaveGrantRequestDetails = dbContext.LeaveGrantRequestDetails.Where(y => y.EmployeeID == x.eldltlr.eldlt.eld.EmployeeID &&
                       y.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && y.Status == "Approved" && y.BalanceDay != 0).OrderBy(x => x.EffectiveFromDate).ToList(),
                     AppliedLeaveDates = (from leave in dbContext.ApplyLeave
                                          join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                                          where leave.EmployeeId == x.eldltlr.eldlt.eld.EmployeeID && leave.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && ald.Date >= employee.FromDate.AddYears(-1) && ald.Date <= employee.ToDate.AddYears(1) && (leave.Status != "Cancelled" &&
                                          (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true)))
                                          select new AppliedLeaveTypeDetails
                                          {
                                              Date = ald.Date,
                                              IsFirstHalf = ald.IsFirstHalf,
                                              IsSecondHalf = ald.IsSecondHalf,
                                              IsFullDay = ald.IsFullDay,
                                              LeaveId = ald.LeaveId
                                          }).ToList()

                 }).ToList();
            return EmployeeAvailableLeaveListDetails;
        }
        #endregion
        public LeaveBalanceView GetLeaveBalanceByEmployeeId(EmployeeListView employee)
        {
            LeaveBalanceView leaveBalance = new();
            List<int> employeeList = employee?.EmployeeDetails?.Select(x => x.EmployeeID).ToList();
            //DateTime todaydate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);

            //int currrentFinanceYear = DateTime.Now.Year;
            //if (DateTime.Now.Month > 3)
            //    currrentFinanceYear = DateTime.Now.Year;
            //else
            //    currrentFinanceYear = DateTime.Now.Year - 1;
            //if (employee.FromDate.Year == currrentFinanceYear)
            //{
            //    leaveBalance.Leaves = (from ld in dbContext.EmployeeLeaveDetails
            //                               //join lt in dbContext.LeaveTypes on ld.LeaveTypeID equals lt.LeaveTypeId
            //                           where employeeList.Contains(ld.EmployeeID == null ? 0 : (int)ld.EmployeeID)
            //                           //&& lt.EffectiveFromDate >= employee.FromDate && (lt.EffectiveToDate == null || lt.EffectiveToDate <= employee.ToDate)
            //                           group ld by ld.EmployeeID into g
            //                           select new LeaveBalanceList
            //                           {
            //                               EmployeeId = g.Key,
            //                               BalanceLeaves = GetRoundOffValues(g.Sum(x => x.AdjustmentEffectiveFromDate != null || x.AdjustmentEffectiveFromDate <= todaydate ? x.AdjustmentBalanceLeave == null ? 0 : (decimal)x.AdjustmentBalanceLeave : x.BalanceLeave == null ? 0 : (decimal)x.BalanceLeave))                                           
            //                           }).ToList();
            //}
            //else
            //{
            //    List<LeaveBalanceList> leaveBalanceList = new List<LeaveBalanceList>();

            //    List<EmployeeAvailableLeaveDetails> EmployeeAvailableLeaveListDetails = dbContext.EmployeeLeaveDetails
            //     .Join(dbContext.LeaveTypes, eld => eld.LeaveTypeID, lt => lt.LeaveTypeId, (eld, lt) => new { eld, lt })
            //     .Join(dbContext.LeaveRestrictions, eldlt => eldlt.lt.LeaveTypeId, lr => lr.LeaveTypeId, (eldlt, lr) => new { eldlt, lr })
            //     .Join(dbContext.LeaveEntitlement, eldltlr => eldltlr.eldlt.eld.LeaveTypeID, le => le.LeaveTypeId, (eldltlr, le) => new { eldltlr, le })
            //     .Where(x =>  employeeList.Contains(x.eldltlr.eldlt.eld.EmployeeID==null?0:(int)x.eldltlr.eldlt.eld.EmployeeID) && x.eldltlr.eldlt.lt.IsActive == true && x.eldltlr.eldlt.lt.EffectiveFromDate <= employee.ToDate && (x.eldltlr.eldlt.lt.EffectiveToDate == null || x.eldltlr.eldlt.lt.EffectiveToDate >= employee.FromDate))
            //     .Select(x => new EmployeeAvailableLeaveDetails
            //     {
            //         EmployeeID = x.eldltlr.eldlt.eld.EmployeeID,
            //         LeaveTypeID = x.eldltlr.eldlt.eld.LeaveTypeID,
            //         BalanceLeave = x.eldltlr.eldlt.eld.BalanceLeave,
            //         ActualBalanceLeave = (x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate != null ) ? x.eldltlr.eldlt.eld.AdjustmentBalanceLeave : x.eldltlr.eldlt.eld.BalanceLeave,
            //         BookedLeaveCount = dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, al => al.LeaveId, ald => ald.LeaveId, (al, ald) => new { al, ald }).Where(rs => rs.al.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && rs.al.EmployeeId == x.eldltlr.eldlt.eld.EmployeeID && rs.al.Status != "Cancelled" && rs.ald.AppliedLeaveStatus != false && rs.al.FromDate >= employee.FromDate && rs.al.ToDate <= employee.ToDate).Sum(rs => (rs.ald.IsFirstHalf == true || rs.ald.IsSecondHalf == true) ? 0.5m : 1),
            //         LeaveType = x.eldltlr.eldlt.lt.LeaveType,
            //         LeaveCode = x.eldltlr.eldlt.lt.LeaveCode,
            //         LeaveAccruedType = x.eldltlr.eldlt.lt.LeaveAccruedType,
            //         LeaveAccruedDay = x.eldltlr.eldlt.lt.LeaveAccruedDay,
            //         LeaveAccruedNoOfDays = x.eldltlr.eldlt.lt.LeaveAccruedNoOfDays,
            //         LeaveTypesId = x.eldltlr.eldlt.lt.LeaveTypesId,
            //         ProRate = x.eldltlr.eldlt.lt.ProRate,
            //         EffectiveFromDate = x.eldltlr.eldlt.lt.EffectiveFromDate,
            //         EffectiveToDate = x.eldltlr.eldlt.lt.EffectiveToDate,                     
            //         CarryForwardId = x.le.CarryForwardId,
            //         MaximumCarryForwardDays = x.le.MaximumCarryForwardDays,
            //         ReimbursementId = x.le.ReimbursementId,
            //         MaximumReimbursementDays = x.le.MaximumReimbursementDays,
            //         ResetYear = x.le.ResetYear,
            //         ResetMonth = x.le.ResetMonth,
            //         ResetDay = x.le.ResetDay,                     
            //         AdjustmentEffectiveFromDate = x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate,
            //         AdjustmentBalanceLeave = x.eldltlr.eldlt.eld.AdjustmentBalanceLeave,
            //         BalanceBasedOn = dbContext.AppConstants.Where(rs => rs.AppConstantId == x.eldltlr.eldlt.lt.BalanceBasedOn).Select(rs => new BalanceBasedOnDetails { BalanceBasedOnId = rs.AppConstantId, BalanceBasedOnText = rs.DisplayName, BalanceBasedOnValue = rs.AppConstantValue }).ToList(),                     
            //         ResetPeriod = dbContext.AppConstants.Where(y => y.AppConstantId == x.le.ResetYear).Select(z => z.AppConstantValue).FirstOrDefault()

            //     }).ToList();

            //    if (EmployeeAvailableLeaveListDetails?.Count > 0)
            //    {
            //        foreach (var item in EmployeeAvailableLeaveListDetails)
            //        {
            //            decimal balanceLeave = 0;
            //            DateTime? leaveAccruedDate;
            //            int duration = 0;
            //            DateTime? dateOfJoining = employee.EmployeeDetails.Where(x => x.EmployeeID == item.EmployeeID).Select(x => x.DOJ).FirstOrDefault();
            //            if (dateOfJoining != null && dateOfJoining > item.EffectiveFromDate)
            //            {
            //                leaveAccruedDate = item.EffectiveFromDate;
            //            }
            //            else
            //            {
            //                leaveAccruedDate = dateOfJoining;
            //            }
            //            string appConstants = dbContext.AppConstants.Where(x => x.AppConstantId == item.LeaveAccruedType).Select(x => x.AppConstantValue).FirstOrDefault();
            //            if (appConstants?.ToLower() == "monthly")
            //            {
            //                if (leaveAccruedDate <= employee.FromDate)
            //                {
            //                    duration = 12;
            //                }
            //                else
            //                {
            //                    duration = 12 - (leaveAccruedDate == null ? 0 : leaveAccruedDate.Value.Month);
            //                    if (leaveAccruedDate?.Month > 3)
            //                    {
            //                        duration = duration + 3;
            //                    }
            //                }
                            
            //            }
            //            else if (appConstants?.ToLower() == "halfyearly")
            //            {
            //                if (leaveAccruedDate <= employee.FromDate)
            //                {
            //                    duration = 2;
            //                }
            //                else
            //                {
            //                    if (leaveAccruedDate != null && leaveAccruedDate?.Month < 10 && leaveAccruedDate?.Month > 4)
            //                    {
            //                        duration = 2;
            //                    }
            //                    else
            //                    {
            //                        duration = 1;
            //                    }

            //                }
                            
            //            }
            //            else if (appConstants?.ToLower() == "quarterly")
            //            {

            //                if (leaveAccruedDate <= employee.FromDate)
            //                {
            //                    duration = 4;
            //                }
            //                else
            //                {
            //                    if (leaveAccruedDate != null && leaveAccruedDate?.Month >= 4 && leaveAccruedDate?.Month <= 6)
            //                    {
            //                        duration = 4;
            //                    }
            //                    else if (leaveAccruedDate != null && leaveAccruedDate?.Month >= 7 && leaveAccruedDate?.Month <= 9)
            //                    {
            //                        duration = 3;
            //                    }
            //                    else if (leaveAccruedDate != null && leaveAccruedDate?.Month >= 10 && leaveAccruedDate?.Month <= 12)
            //                    {
            //                        duration = 2;
            //                    }
            //                    else if (leaveAccruedDate != null && leaveAccruedDate?.Month >= 1 && leaveAccruedDate?.Month <= 3)
            //                    {
            //                        duration = 1;
            //                    }
            //                }
                            
            //            }
            //            else if (appConstants?.ToLower() == "yearly")
            //            {
            //                duration = 1;
            //            }
            //            if (appConstants?.ToLower() == "onetime")
            //            {
            //                balanceLeave = item.BalanceLeave == null ? 0 : (decimal)item.BalanceLeave;
            //            }
            //            else
            //            {
            //                balanceLeave = item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays * duration;
            //            }
            //            decimal finalBalance = 0;
            //            finalBalance = balanceLeave - (item.BookedLeaveCount == null ? 0 : (decimal)item.BookedLeaveCount);
            //            if (item.MaximumCarryForwardDays != null && item?.MaximumCarryForwardDays > 0)
            //            {
            //                decimal financialYearEndBalance = 0;

            //                duration = 0;
            //                if (appConstants?.ToLower() == "monthly")
            //                {
            //                    duration = 12 - (DateTime.Now.Month);
            //                    if (leaveAccruedDate?.Month > 3)
            //                    {
            //                        duration = duration + 3;
            //                    }
                                
            //                }
            //                else if (appConstants?.ToLower() == "halfyearly")
            //                {
            //                    if (DateTime.Now.Month < 10 && DateTime.Now.Month > 4)
            //                    {
            //                        duration = 2;
            //                    }
            //                    else
            //                    {
            //                        duration = 1;
            //                    }
                                
            //                }
            //                else if (appConstants?.ToLower() == "quarterly")
            //                {
            //                    if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 6)
            //                    {
            //                        duration = 4;
            //                    }
            //                    else if (DateTime.Now.Month >= 7 && DateTime.Now.Month <= 9)
            //                    {
            //                        duration = 3;
            //                    }
            //                    else if (DateTime.Now.Month >= 10 && DateTime.Now.Month <= 12)
            //                    {
            //                        duration = 2;
            //                    }
            //                    else if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 3)
            //                    {
            //                        duration = 1;
            //                    }
                                
            //                }
            //                else if (appConstants?.ToLower() == "yearly")
            //                {
            //                    duration = 1;

            //                }
            //                if (appConstants?.ToLower() == "onetime")
            //                {
            //                    finalBalance = item.BalanceLeave == null ? 0 : (decimal)item.BalanceLeave;
            //                }
            //                else
            //                {
            //                    financialYearEndBalance = (item.BalanceLeave == null ? 0 : (decimal)item.BalanceLeave) + (item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays * duration);
            //                    if (financialYearEndBalance > item?.MaximumCarryForwardDays)
            //                    {
            //                        finalBalance = finalBalance + (item.MaximumCarryForwardDays == null ? 0 : (decimal)item.MaximumCarryForwardDays);
            //                    }
            //                    else
            //                    {
            //                        finalBalance = finalBalance + financialYearEndBalance;
            //                    }
            //                }
            //            }
            //            LeaveBalanceList employeeLeaveBalance = new LeaveBalanceList();
            //            employeeLeaveBalance.BalanceLeaves= finalBalance;
            //            employeeLeaveBalance.EmployeeId = item?.EmployeeID;
            //            leaveBalanceList.Add(employeeLeaveBalance);
            //        }
                    
            //        leaveBalance.Leaves = (from leave in leaveBalanceList
            //                               group leave by leave.EmployeeId into g
            //                               select new LeaveBalanceList
            //                               {
            //                                   EmployeeId = g.Key,
            //                                   BalanceLeaves = GetRoundOffValues(g.Sum(x => x.BalanceLeaves == null ? 0 : (decimal)x.BalanceLeaves))
            //                               }).ToList();
            //    }

            //}
            leaveBalance.LeaveTypes = (from el in dbContext.EmployeeLeaveDetails
                                       join lt in dbContext.LeaveTypes on el.LeaveTypeID equals lt.LeaveTypeId
                                       where employeeList.Contains(el.EmployeeID==null?0:(int)el.EmployeeID)
                                       //&& lt.EffectiveFromDate >= employee.FromDate && (lt.EffectiveToDate == null || lt.EffectiveToDate <= employee.ToDate)
                                       select new LeaveTypesView
                                       {
                                           EmployeeId = el.EmployeeID,
                                           LeaveTypeId = el.LeaveTypeID,
                                           LeaveType = lt.LeaveType,
                                           LeaveBalance = el.BalanceLeave
                                       }).ToList();
            return leaveBalance;
        }
        public LeaveBalanceList GetEmployeeLeaveBalance(int employeeId, DateTime fromDate, DateTime toDate)
        {
            LeaveBalanceList leaveBalance = new();
            leaveBalance = (from ld in dbContext.EmployeeLeaveDetails
                            join lt in dbContext.LeaveTypes on ld.LeaveTypeID equals lt.LeaveTypeId
                            where ld.EmployeeID == employeeId 
                            //&& lt.EffectiveFromDate >= fromDate && (lt.EffectiveToDate == null || lt.EffectiveToDate <= toDate)
                            group ld by ld.EmployeeID into g
                            select new LeaveBalanceList
                            {
                                EmployeeId = g.Key,
                                BalanceLeaves = GetRoundOffValues((decimal)g.Sum(x => x.AdjustmentEffectiveFromDate != null || x.AdjustmentEffectiveFromDate <= fromDate ? x.AdjustmentBalanceLeave : x.BalanceLeave)),
                            }).FirstOrDefault();
            return leaveBalance;
        }

        #region Get Applied Leave By Leave Type Id
        public List<ApplyLeaves> GetAppliedleaveByTypeId(int leaveTypeId)
        {
            return dbContext.ApplyLeave.Where(x => x.LeaveTypeId == leaveTypeId && x.IsActive == true).ToList();
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
        #region Get RoundOff
        public static decimal GetRoundOffValues(decimal count)
        {
            decimal sd = default;
            string s = default;
            if (count != null && count != 0)
            {
                //var decimalValues = (decimal)1.5112548;
                decimal decimalValue = decimal.Round(count, 2, MidpointRounding.AwayFromZero);
                var split = decimalValue.ToString(CultureInfo.InvariantCulture).Split('.');

                char x = split[0].First();
                bool neg = default;

                if (x == '-')
                {
                    var value = decimalValue.ToString(CultureInfo.InvariantCulture).Split('-');
                    neg = true;
                    split = value[1].Split('.');
                    var d = Convert.ToDecimal("0." + (split.Length > 1 ? split[1] : 0));

                    if (d is >= (decimal)0.0 and <= (decimal)0.24)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.0);

                    }
                    else if (d is >= (decimal)0.25 and <= (decimal)0.49)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.25);

                    }
                    else if (d is >= (decimal)0.50 and <= (decimal)0.74)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.50);

                    }
                    else if (d is >= (decimal)0.75 and <= (decimal)0.99)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.75);

                    }
                    if (neg == true)
                    {
                        s = sd.ToString();
                        s = s.Insert(0, "-");
                        sd = decimal.Parse(s);
                    }
                }
                else
                {
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
            }
            else
            {
                sd = 0;
            }

            return sd;
        }
        #endregion

    }
}