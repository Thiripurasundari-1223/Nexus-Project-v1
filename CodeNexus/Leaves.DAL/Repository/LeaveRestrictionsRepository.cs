using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaves.DAL.Repository
{
    public interface ILeaveRestrictionsRepository : IBaseRepository<LeaveRestrictions>
    {
        LeaveRestrictions GetRestrictionsByLeaveId(int leaveTypeId);
        LeaveRestrictions GetByID(int pLeaveTypeId);
        List<EmployeeAvailableLeaveDetails> GetEmployeeLeaveAndRestrictionDetails(int employeeId, DateTime FromDate, DateTime ToDate, DateTime? dateOfJoining);
        decimal? GetGrantLeaveExpireDate(int leaveTypeId);
        List<EmployeeAvailableLeaveDetails> GetEmployeeLeavesBalanceDetails(int employeeId, DateTime FromDate, DateTime ToDate, DateTime? dateOfJoining);
        EmployeeAvailableLeaveDetails GetLeaveDetailsByEmployeeIdAndLeaveId(int employeeId, DateTime FromDate, DateTime ToDate, int leaveId);
    }
    public class LeaveRestrictionsRepository : BaseRepository<LeaveRestrictions>, ILeaveRestrictionsRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveRestrictionsRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveRestrictions GetRestrictionsByLeaveId(int leaveTypeId)
        {
            LeaveRestrictions restrictions = new LeaveRestrictions();
            restrictions = dbContext.LeaveRestrictions.Where(x => x.LeaveTypeId == leaveTypeId).FirstOrDefault();
            return restrictions == null ? new LeaveRestrictions() : restrictions;
        }
        public LeaveRestrictions GetByID(int pLeaveTypeId)
        {
            return dbContext.LeaveRestrictions.Where(x => x.LeaveTypeId == pLeaveTypeId).FirstOrDefault();
        }
        public List<EmployeeAvailableLeaveDetails> GetEmployeeLeaveAndRestrictionDetails(int employeeId, DateTime FromDate, DateTime ToDate, DateTime? dateOfJoining)
        {
            int currrentFinanceYear = DateTime.Now.Year;
            List<EmployeeAvailableLeaveDetails> EmployeeAvailableLeaveListDetails = new List<EmployeeAvailableLeaveDetails>();
            if (DateTime.Now.Month > 3)
                currrentFinanceYear = DateTime.Now.Year;
            else
                currrentFinanceYear = DateTime.Now.Year - 1;

            EmployeeAvailableLeaveListDetails = dbContext.EmployeeLeaveDetails
                 .Join(dbContext.LeaveTypes, eld => eld.LeaveTypeID, lt => lt.LeaveTypeId, (eld, lt) => new { eld, lt })
                 .Join(dbContext.LeaveRestrictions, eldlt => eldlt.lt.LeaveTypeId, lr => lr.LeaveTypeId, (eldlt, lr) => new { eldlt, lr })
                 .Join(dbContext.LeaveEntitlement, eldltlr => eldltlr.eldlt.eld.LeaveTypeID, le => le.LeaveTypeId, (eldltlr, le) => new { eldltlr, le })
                 .Where(x => x.eldltlr.eldlt.eld.EmployeeID == employeeId && x.eldltlr.eldlt.lt.IsActive == true && x.eldltlr.eldlt.lt.EffectiveFromDate <= ToDate && (x.eldltlr.eldlt.lt.EffectiveToDate == null || x.eldltlr.eldlt.lt.EffectiveToDate >= FromDate))
                 .Select(x => new EmployeeAvailableLeaveDetails
                 {
                     EmployeeID = x.eldltlr.eldlt.eld.EmployeeID,
                     LeaveTypeID = x.eldltlr.eldlt.eld.LeaveTypeID,
                     BalanceLeave = x.eldltlr.eldlt.eld.BalanceLeave,
                     ActualBalanceLeave = (x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate != null) ? x.eldltlr.eldlt.eld.AdjustmentBalanceLeave : x.eldltlr.eldlt.eld.BalanceLeave,
                     DisplayBalanceLeave= (x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate != null) ? x.eldltlr.eldlt.eld.AdjustmentBalanceLeave : x.eldltlr.eldlt.eld.BalanceLeave,
                     BookedLeaveCount = dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, al => al.LeaveId, ald => ald.LeaveId, (al, ald) => new { al, ald }).Where(rs => rs.al.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && rs.al.EmployeeId == x.eldltlr.eldlt.eld.EmployeeID && rs.al.Status != "Cancelled" && rs.ald.AppliedLeaveStatus != false && rs.al.FromDate >= FromDate && rs.al.ToDate <= ToDate).Sum(rs => (rs.ald.IsFirstHalf == true || rs.ald.IsSecondHalf == true) ? 0.5m : 1),
                     LeaveType = x.eldltlr.eldlt.lt.LeaveType,
                     LeaveCode = x.eldltlr.eldlt.lt.LeaveCode,
                     LeaveAccruedType = x.eldltlr.eldlt.lt.LeaveAccruedType,
                     LeaveAccruedDay = x.eldltlr.eldlt.lt.LeaveAccruedDay,
                     LeaveAccruedNoOfDays = x.eldltlr.eldlt.lt.LeaveAccruedNoOfDays,
                     LeaveAccruedTypeName = dbContext.AppConstants.Where(y => y.AppConstantId == x.eldltlr.eldlt.lt.LeaveAccruedType).Select(x => x.AppConstantValue).FirstOrDefault(),
                     LeaveTypesId = x.eldltlr.eldlt.lt.LeaveTypesId,
                     ProRate = x.eldltlr.eldlt.lt.ProRate,
                     EffectiveFromDate = x.eldltlr.eldlt.lt.EffectiveFromDate,
                     EffectiveToDate = x.eldltlr.eldlt.lt.EffectiveToDate,
                     ExceedLeaveBalance = x.eldltlr.lr.ExceedLeaveBalance,
                     AllowUsersViewId = x.eldltlr.lr.AllowUsersViewId,
                     BalanceDisplayedId = x.eldltlr.lr.BalanceDisplayedId,
                     DaysInAdvance = x.eldltlr.lr.DaysInAdvance,
                     AllowRequestDates = x.eldltlr.lr.AllowRequestDates,
                     AllowRequestNextDays = x.eldltlr.lr.AllowRequestNextDays,
                     DatesAppliedAdvance = x.eldltlr.lr.DatesAppliedAdvance,
                     MaximumLeavePerApplication = x.eldltlr.lr.MaximumLeavePerApplication,
                     MinimumGapTwoApplication = x.eldltlr.lr.MinimumGapTwoApplication,
                     MaximumConsecutiveDays = x.eldltlr.lr.MaximumConsecutiveDays,
                     EnableFileUpload = x.eldltlr.lr.EnableFileUpload,
                     MinimumNoOfApplicationsPeriod = x.eldltlr.lr.MinimumNoOfApplicationsPeriod,
                     AllowRequestPeriodId = x.eldltlr.lr.AllowRequestPeriodId,
                     MaximumLeave = x.eldltlr.lr.MaximumLeave,
                     MinimumGap = x.eldltlr.lr.MinimumGap,
                     MaximumConsecutive = x.eldltlr.lr.MaximumConsecutive,
                     EnableFile = x.eldltlr.lr.EnableFile,
                     CannotBeTakenTogether = x.eldltlr.lr.CannotBeTakenTogether,
                     AllowPastDates = x.eldltlr.lr.AllowPastDates,
                     AllowFutureDates = x.eldltlr.lr.AllowFutureDates,
                     IsAllowRequestNextDays = x.eldltlr.lr.IsAllowRequestNextDays,
                     IsToBeApplied = x.eldltlr.lr.IsToBeApplied,
                     Weekendsbetweenleaveperiod = x.eldltlr.lr.Weekendsbetweenleaveperiod,
                     Holidaybetweenleaveperiod = x.eldltlr.lr.Holidaybetweenleaveperiod,
                     DurationsAllowedDetails = dbContext.LeaveDuration.Join(dbContext.AppConstants, ld => ld.DurationId, ac => ac.AppConstantId, (ld, ac) => new { ld, ac }).Where(rs => rs.ld.LeaveTypeId == x.eldltlr.lr.LeaveTypeId).Select(rs => new DurationsAllowedDetails { LeaveTypeId = rs.ld.LeaveTypeId, DurationId = rs.ac.AppConstantId, DisplayName = rs.ac.DisplayName, AppConstantValue = rs.ac.AppConstantValue }).ToList(),
                     activeLeaveType = dbContext.LeaveTakenTogether.Join(dbContext.LeaveTypes, ltt => ltt.LeaveOrHolidayId, lt => lt.LeaveTypeId, (ltt, lt) => new { ltt, lt }).Where(rs => rs.ltt.LeaveTakenType == "Leave" && rs.ltt.LeaveTypeId == x.eldltlr.lr.LeaveTypeId).Select(rs => new ActiveLeaveList { leaveTypeId = rs.ltt.LeaveOrHolidayId, leaveType = rs.lt.LeaveType }).ToList(),
                     activeHoliday = dbContext.LeaveTakenTogether.Join(dbContext.Holiday, ltt => ltt.LeaveOrHolidayId, h => h.HolidayID, (ltt, h) => new { ltt, h }).Where(rs => rs.ltt.LeaveTakenType == "Holiday" && rs.ltt.LeaveTypeId == x.eldltlr.lr.LeaveTypeId).Select(rs => new ActiveHolidayList { holidayID = rs.ltt.LeaveOrHolidayId, holidayName = rs.h.HolidayName }).ToList(),
                     CarryForwardId = x.le.CarryForwardId,
                     MaximumCarryForwardDays = x.le.MaximumCarryForwardDays,
                     ReimbursementId = x.le.ReimbursementId,
                     MaximumReimbursementDays = x.le.MaximumReimbursementDays,
                     ResetYear = x.le.ResetYear,
                     ResetMonth = x.le.ResetMonth,
                     ResetDay = x.le.ResetDay,
                     SpecificEmployeeDetailLeaveList = dbContext.SpecificEmployeeDetailLeave.Join(dbContext.AppConstants, sedl => sedl.EmployeeDetailLeaveId, ac => ac.AppConstantId, (sedl, ac) => new { sedl, ac }).Where(rs => rs.sedl.LeaveTypeId == x.eldltlr.lr.LeaveTypeId).Select(rs => new SpecificEmployeeDetailLeaveView
                     {
                         LeaveTypeId = rs.sedl.LeaveTypeId,
                         EmployeeDetailLeaveId = rs.sedl.EmployeeDetailLeaveId,
                         EmployeeDetailLeaveText = rs.ac.DisplayName
                     }).ToList(),
                     AdjustmentEffectiveFromDate = x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate,
                     AdjustmentBalanceLeave = x.eldltlr.eldlt.eld.AdjustmentBalanceLeave,
                     BalanceBasedOn = dbContext.AppConstants.Where(rs => rs.AppConstantId == x.eldltlr.eldlt.lt.BalanceBasedOn).Select(rs => new BalanceBasedOnDetails { BalanceBasedOnId = rs.AppConstantId, BalanceBasedOnText = rs.DisplayName, BalanceBasedOnValue = rs.AppConstantValue }).ToList(),
                     GrantMinimumNoOfRequestDay = x.eldltlr.lr.GrantMinimumNoOfRequestDay,
                     GrantMaximumNoOfRequestDay = x.eldltlr.lr.GrantMaximumNoOfRequestDay,
                     GrantMaximumNoOfPeriod = x.eldltlr.lr.GrantMaximumNoOfPeriod,
                     GrantMaximumNoOfDay = x.eldltlr.lr.GrantMaximumNoOfDay,
                     GrantMinimumGapTwoApplicationDay = x.eldltlr.lr.GrantMinimumGapTwoApplicationDay,
                     GrantUploadDocumentSpecificPeriodDay = x.eldltlr.lr.GrantUploadDocumentSpecificPeriodDay,
                     IsGrantRequestPastDay = x.eldltlr.lr.IsGrantRequestPastDay,
                     GrantRequestPastDay = x.eldltlr.lr.GrantRequestPastDay,
                     IsGrantRequestFutureDay = x.eldltlr.lr.IsGrantRequestFutureDay,
                     GrantRequestFutureDay = x.eldltlr.lr.GrantRequestFutureDay,
                     GrantEffectiveFromDate = dbContext.LeaveGrantRequestDetails.Where(rs => rs.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && rs.EmployeeID == x.eldltlr.eldlt.eld.EmployeeID && rs.Status == "Approved" && rs.IsActive == true).Select(rs => rs.EffectiveFromDate).FirstOrDefault(),
                     ResetPeriod = dbContext.AppConstants.Where(y => y.AppConstantId == x.le.ResetYear).Select(z => z.AppConstantValue).FirstOrDefault(),
                     LeaveAdjustmentDetails = dbContext.LeaveAdjustmentDetails.Where(y => y.EmployeeId == employeeId && y.LeavetypeId == x.eldltlr.eldlt.eld.LeaveTypeID).OrderByDescending(z => z.EffectiveFromDate).ToList(),
                     LeaveCarryForward = dbContext.LeaveCarryForward.Where(y => y.EmployeeID == employeeId && y.LeaveTypeID == x.eldltlr.eldlt.eld.LeaveTypeID).OrderByDescending(z => z.ResetDate).ToList(),
                     ToBeAdvanced = x.eldltlr.lr.ToBeAdvanced,
                     ResetLeaveAfter = x.eldltlr.lr.GrantResetLeaveAfterDays,
                     LeaveGrantRequestDetails = dbContext.LeaveGrantRequestDetails.Where(y => y.EmployeeID == employeeId &&
                       y.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && y.Status == "Approved" && y.BalanceDay !=0).OrderBy(x=>x.EffectiveFromDate).ToList(),
                     AppliedLeaveDates = (from leave in dbContext.ApplyLeave
                                          join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                                          where leave.EmployeeId == employeeId && leave.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && ald.Date >= FromDate.AddYears(-1) && ald.Date <= ToDate.AddYears(1) && (leave.Status != "Cancelled" &&
                                          (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true)))
                                          select new AppliedLeaveTypeDetails
                                          {
                                              Date = ald.Date,
                                              IsFirstHalf = ald.IsFirstHalf,
                                              IsSecondHalf = ald.IsSecondHalf,
                                              IsFullDay = ald.IsFullDay,
                                              LeaveId = ald.LeaveId
                                          }).ToList()

                 }).OrderByDescending(x=>x.BalanceLeave).ToList();
            //if (FromDate.Year != currrentFinanceYear)            
            //{
                
            //    foreach (var item in EmployeeAvailableLeaveListDetails)
            //    {
                    
            //        decimal balanceLeave = 0;
            //        DateTime? leaveAccruedDate;
            //        decimal actualBalance = item?.BalanceLeave==null?0:(decimal)item.BalanceLeave;
            //        int duration = 0;
            //        if(item.AdjustmentEffectiveFromDate !=null)
            //        {
            //            actualBalance= item?.AdjustmentBalanceLeave == null ? 0 : (decimal)item.AdjustmentBalanceLeave; 
            //        }
            //        if (dateOfJoining != null && dateOfJoining > item.EffectiveFromDate)
            //        {
            //            leaveAccruedDate = item.EffectiveFromDate;
            //        }
            //        else
            //        {
            //            leaveAccruedDate = dateOfJoining;
            //        }
            //        string appConstants = dbContext.AppConstants.Where(x => x.AppConstantId == item.LeaveAccruedType).Select(x => x.AppConstantValue).FirstOrDefault();
            //        if (appConstants?.ToLower() == "monthly")
            //        {                                           
            //            if(leaveAccruedDate <= FromDate)
            //            {
            //                duration = 12;
            //            }
            //            else
            //            {
            //                duration = 12 - (leaveAccruedDate==null?0: leaveAccruedDate.Value.Month);
            //                if(leaveAccruedDate?.Month>3)
            //                {
            //                    duration = duration + 3;
            //                }
            //            }
                        
            //        }
            //        else if (appConstants?.ToLower() == "halfyearly")
            //        {                        
            //            if (leaveAccruedDate <= FromDate)
            //            {
            //                duration = 2;
            //            }
            //            else
            //            {
            //                if(leaveAccruedDate != null && leaveAccruedDate?.Month<10 && leaveAccruedDate?.Month > 4)
            //                {
            //                    duration = 2;
            //                }
            //                else
            //                {
            //                    duration = 1;
            //                }
                            
            //            }
            //            balanceLeave = item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays * duration;
            //        }
            //        else if (appConstants?.ToLower() == "quarterly")
            //        {
                        
            //            if (leaveAccruedDate <= FromDate)
            //            {
            //                duration = 4;
            //            }
            //            else
            //            {
            //                if (leaveAccruedDate != null && leaveAccruedDate?.Month >=4 && leaveAccruedDate?.Month <= 6)
            //                {
            //                    duration = 4;
            //                }
            //                else if(leaveAccruedDate != null && leaveAccruedDate?.Month >= 7 && leaveAccruedDate?.Month <= 9)
            //                {
            //                    duration = 3;
            //                }
            //                else if (leaveAccruedDate != null && leaveAccruedDate?.Month >= 10 && leaveAccruedDate?.Month <= 12)
            //                {
            //                    duration = 2;
            //                }
            //                else if (leaveAccruedDate != null && leaveAccruedDate?.Month >= 1 && leaveAccruedDate?.Month <= 3)
            //                {
            //                    duration = 1;
            //                }
            //            }
                        
            //        }
            //        else if (appConstants?.ToLower() == "yearly")
            //        {
            //            duration = 1;
            //        }
            //        if (appConstants?.ToLower() != "onetime")
            //        {
            //            balanceLeave = item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays * duration;
            //            //balanceLeave = item.BalanceLeave==null?0:(decimal)item.BalanceLeave;
            //        }
            //        //else
            //        //{
            //        //    balanceLeave = item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays * duration;
            //        //}

            //        decimal finalBalance = 0;
            //        finalBalance = balanceLeave - (item.BookedLeaveCount == null ? 0 : (decimal)item.BookedLeaveCount);
            //        if(item.MaximumCarryForwardDays !=null && item?.MaximumCarryForwardDays > 0)
            //        {
            //            decimal financialYearEndBalance = 0;
                         
            //            duration = 0;
            //            if (appConstants?.ToLower() == "monthly")
            //            {
            //                duration = 12 - (DateTime.Now.Month);
            //                if (leaveAccruedDate?.Month > 3)
            //                {
            //                    duration = duration + 3;
            //                }
                            
            //            }
            //            else if (appConstants?.ToLower() == "halfyearly")
            //            {
            //                if (DateTime.Now.Month < 10 && DateTime.Now.Month > 4)
            //                {
            //                    duration = 2;
            //                }
            //                else
            //                {
            //                    duration = 1;
            //                }
                            
            //            }
            //            else if (appConstants?.ToLower() == "quarterly")
            //            {
            //                if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 6)
            //                {
            //                    duration = 4;
            //                }
            //                else if (DateTime.Now.Month >= 7 && DateTime.Now.Month <= 9)
            //                {
            //                    duration = 3;
            //                }
            //                else if (DateTime.Now.Month >= 10 && DateTime.Now.Month <= 12)
            //                {
            //                    duration = 2;
            //                }
            //                else if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 3)
            //                {
            //                    duration = 1;
            //                }
                            
            //            }
            //            else if (appConstants?.ToLower() == "yearly")
            //            {
            //                duration = 1;
                            
            //            }
                        
            //            if (appConstants?.ToLower() == "onetime")
            //            {
            //                finalBalance = actualBalance;
            //            }
            //            else
            //            {
            //                financialYearEndBalance = (actualBalance) + (item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays * duration);
            //                if (financialYearEndBalance > item?.MaximumCarryForwardDays)
            //                {
            //                    finalBalance = finalBalance + (item.MaximumCarryForwardDays == null ? 0 : (decimal)item.MaximumCarryForwardDays);
            //                }
            //                else
            //                {
            //                    finalBalance = finalBalance + financialYearEndBalance;
            //                }
            //            }
            //            item.AdjustmentEffectiveFromDate = null;
            //        }
            //        else
            //        {
            //            finalBalance = finalBalance + (actualBalance);
            //        }
            //        item.DisplayBalanceLeave = finalBalance;
            //    }
            //}

            return EmployeeAvailableLeaveListDetails;
        }
        public decimal? GetGrantLeaveExpireDate(int leaveTypeId)
        {
            return dbContext.LeaveRestrictions.Where(x => x.LeaveTypeId == leaveTypeId).Select(x=>x.GrantResetLeaveAfterDays).FirstOrDefault();
        }


        public List<EmployeeAvailableLeaveDetails> GetEmployeeLeavesBalanceDetails(int employeeId, DateTime FromDate, DateTime ToDate, DateTime? dateOfJoining)
        {
            int currrentFinanceYear = DateTime.Now.Year;
            List<EmployeeAvailableLeaveDetails> EmployeeAvailableLeaveListDetails = new List<EmployeeAvailableLeaveDetails>();
            if (DateTime.Now.Month > 3)
                currrentFinanceYear = DateTime.Now.Year;
            else
                currrentFinanceYear = DateTime.Now.Year - 1;

            EmployeeAvailableLeaveListDetails = dbContext.EmployeeLeaveDetails
                .Join(dbContext.LeaveTypes, eld => eld.LeaveTypeID, lt => lt.LeaveTypeId, (eld, lt) => new { eld, lt })
                .Where(x => x.eld.EmployeeID == employeeId && x.lt.IsActive == true && x.lt.EffectiveFromDate <= ToDate && (x.lt.EffectiveToDate == null || x.lt.EffectiveToDate >= FromDate)).Select(x =>
                 new EmployeeAvailableLeaveDetails
                 {
                     EmployeeID = x.eld.EmployeeID,
                     LeaveTypeID = x.eld.LeaveTypeID,
                     BalanceLeave = x.eld.BalanceLeave,
                     ActualBalanceLeave = (x.eld.AdjustmentEffectiveFromDate != null) ? x.eld.AdjustmentBalanceLeave : x.eld.BalanceLeave,
                     DisplayBalanceLeave = (x.eld.AdjustmentEffectiveFromDate != null) ? x.eld.AdjustmentBalanceLeave : x.eld.BalanceLeave,
                     BookedLeaveCount = dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, al => al.LeaveId, ald => ald.LeaveId, (al, ald) => new { al, ald }).Where(rs => rs.al.LeaveTypeId == x.eld.LeaveTypeID && rs.al.EmployeeId == x.eld.EmployeeID && rs.al.Status != "Cancelled" && rs.ald.AppliedLeaveStatus != false && rs.al.FromDate >= FromDate && rs.al.ToDate <= ToDate).Sum(rs => (rs.ald.IsFirstHalf == true || rs.ald.IsSecondHalf == true) ? 0.5m : 1),
                     LeaveType = x.lt.LeaveType,
                     BalanceBasedOn = dbContext.AppConstants.Where(rs => rs.AppConstantId == x.lt.BalanceBasedOn).Select(rs => new BalanceBasedOnDetails { BalanceBasedOnId = rs.AppConstantId, BalanceBasedOnText = rs.DisplayName, BalanceBasedOnValue = rs.AppConstantValue }).ToList(),
                 }).ToList();
            return EmployeeAvailableLeaveListDetails;
        }

        public EmployeeAvailableLeaveDetails GetLeaveDetailsByEmployeeIdAndLeaveId(int employeeId, DateTime FromDate, DateTime ToDate,int leaveId)
        {
            int currrentFinanceYear = DateTime.Now.Year;
            EmployeeAvailableLeaveDetails EmployeeAvailableLeaveListDetails = new EmployeeAvailableLeaveDetails();
            if (DateTime.Now.Month > 3)
                currrentFinanceYear = DateTime.Now.Year;
            else
                currrentFinanceYear = DateTime.Now.Year - 1;

            EmployeeAvailableLeaveListDetails = dbContext.EmployeeLeaveDetails
                 .Join(dbContext.LeaveTypes, eld => eld.LeaveTypeID, lt => lt.LeaveTypeId, (eld, lt) => new { eld, lt })
                 .Join(dbContext.LeaveRestrictions, eldlt => eldlt.lt.LeaveTypeId, lr => lr.LeaveTypeId, (eldlt, lr) => new { eldlt, lr })
                 .Join(dbContext.LeaveEntitlement, eldltlr => eldltlr.eldlt.eld.LeaveTypeID, le => le.LeaveTypeId, (eldltlr, le) => new { eldltlr, le })
                 .Where(x => x.eldltlr.eldlt.eld.EmployeeID == employeeId && x.eldltlr.eldlt.lt.IsActive == true && x.eldltlr.eldlt.lt.EffectiveFromDate <= ToDate && (x.eldltlr.eldlt.lt.EffectiveToDate == null || x.eldltlr.eldlt.lt.EffectiveToDate >= FromDate) && x.eldltlr.eldlt.lt.LeaveTypeId == leaveId)
                 .Select(x => new EmployeeAvailableLeaveDetails
                 {
                     EmployeeID = x.eldltlr.eldlt.eld.EmployeeID,
                     LeaveTypeID = x.eldltlr.eldlt.eld.LeaveTypeID,
                     BalanceLeave = x.eldltlr.eldlt.eld.BalanceLeave,
                     ActualBalanceLeave = (x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate != null) ? x.eldltlr.eldlt.eld.AdjustmentBalanceLeave : x.eldltlr.eldlt.eld.BalanceLeave,
                     DisplayBalanceLeave = (x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate != null) ? x.eldltlr.eldlt.eld.AdjustmentBalanceLeave : x.eldltlr.eldlt.eld.BalanceLeave,
                     BookedLeaveCount = dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, al => al.LeaveId, ald => ald.LeaveId, (al, ald) => new { al, ald }).Where(rs => rs.al.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && rs.al.EmployeeId == x.eldltlr.eldlt.eld.EmployeeID && rs.al.Status != "Cancelled" && rs.ald.AppliedLeaveStatus != false && rs.al.FromDate >= FromDate && rs.al.ToDate <= ToDate).Sum(rs => (rs.ald.IsFirstHalf == true || rs.ald.IsSecondHalf == true) ? 0.5m : 1),
                     LeaveType = x.eldltlr.eldlt.lt.LeaveType,
                     LeaveCode = x.eldltlr.eldlt.lt.LeaveCode,
                     LeaveAccruedType = x.eldltlr.eldlt.lt.LeaveAccruedType,
                     LeaveAccruedDay = x.eldltlr.eldlt.lt.LeaveAccruedDay,
                     LeaveAccruedNoOfDays = x.eldltlr.eldlt.lt.LeaveAccruedNoOfDays,
                     LeaveAccruedTypeName = dbContext.AppConstants.Where(y => y.AppConstantId == x.eldltlr.eldlt.lt.LeaveAccruedType).Select(x => x.AppConstantValue).FirstOrDefault(),
                     LeaveTypesId = x.eldltlr.eldlt.lt.LeaveTypesId,
                     ProRate = x.eldltlr.eldlt.lt.ProRate,
                     EffectiveFromDate = x.eldltlr.eldlt.lt.EffectiveFromDate,
                     EffectiveToDate = x.eldltlr.eldlt.lt.EffectiveToDate,
                     ExceedLeaveBalance = x.eldltlr.lr.ExceedLeaveBalance,
                     AllowUsersViewId = x.eldltlr.lr.AllowUsersViewId,
                     BalanceDisplayedId = x.eldltlr.lr.BalanceDisplayedId,
                     DaysInAdvance = x.eldltlr.lr.DaysInAdvance,
                     AllowRequestDates = x.eldltlr.lr.AllowRequestDates,
                     AllowRequestNextDays = x.eldltlr.lr.AllowRequestNextDays,
                     DatesAppliedAdvance = x.eldltlr.lr.DatesAppliedAdvance,
                     MaximumLeavePerApplication = x.eldltlr.lr.MaximumLeavePerApplication,
                     MinimumGapTwoApplication = x.eldltlr.lr.MinimumGapTwoApplication,
                     MaximumConsecutiveDays = x.eldltlr.lr.MaximumConsecutiveDays,
                     EnableFileUpload = x.eldltlr.lr.EnableFileUpload,
                     MinimumNoOfApplicationsPeriod = x.eldltlr.lr.MinimumNoOfApplicationsPeriod,
                     AllowRequestPeriodId = x.eldltlr.lr.AllowRequestPeriodId,
                     MaximumLeave = x.eldltlr.lr.MaximumLeave,
                     MinimumGap = x.eldltlr.lr.MinimumGap,
                     MaximumConsecutive = x.eldltlr.lr.MaximumConsecutive,
                     EnableFile = x.eldltlr.lr.EnableFile,
                     CannotBeTakenTogether = x.eldltlr.lr.CannotBeTakenTogether,
                     AllowPastDates = x.eldltlr.lr.AllowPastDates,
                     AllowFutureDates = x.eldltlr.lr.AllowFutureDates,
                     IsAllowRequestNextDays = x.eldltlr.lr.IsAllowRequestNextDays,
                     IsToBeApplied = x.eldltlr.lr.IsToBeApplied,
                     Weekendsbetweenleaveperiod = x.eldltlr.lr.Weekendsbetweenleaveperiod,
                     Holidaybetweenleaveperiod = x.eldltlr.lr.Holidaybetweenleaveperiod,
                     DurationsAllowedDetails = dbContext.LeaveDuration.Join(dbContext.AppConstants, ld => ld.DurationId, ac => ac.AppConstantId, (ld, ac) => new { ld, ac }).Where(rs => rs.ld.LeaveTypeId == x.eldltlr.lr.LeaveTypeId).Select(rs => new DurationsAllowedDetails { LeaveTypeId = rs.ld.LeaveTypeId, DurationId = rs.ac.AppConstantId, DisplayName = rs.ac.DisplayName, AppConstantValue = rs.ac.AppConstantValue }).ToList(),
                     activeLeaveType = dbContext.LeaveTakenTogether.Join(dbContext.LeaveTypes, ltt => ltt.LeaveOrHolidayId, lt => lt.LeaveTypeId, (ltt, lt) => new { ltt, lt }).Where(rs => rs.ltt.LeaveTakenType == "Leave" && rs.ltt.LeaveTypeId == x.eldltlr.lr.LeaveTypeId).Select(rs => new ActiveLeaveList { leaveTypeId = rs.ltt.LeaveOrHolidayId, leaveType = rs.lt.LeaveType }).ToList(),
                     activeHoliday = dbContext.LeaveTakenTogether.Join(dbContext.Holiday, ltt => ltt.LeaveOrHolidayId, h => h.HolidayID, (ltt, h) => new { ltt, h }).Where(rs => rs.ltt.LeaveTakenType == "Holiday" && rs.ltt.LeaveTypeId == x.eldltlr.lr.LeaveTypeId).Select(rs => new ActiveHolidayList { holidayID = rs.ltt.LeaveOrHolidayId, holidayName = rs.h.HolidayName }).ToList(),
                     CarryForwardId = x.le.CarryForwardId,
                     MaximumCarryForwardDays = x.le.MaximumCarryForwardDays,
                     ReimbursementId = x.le.ReimbursementId,
                     MaximumReimbursementDays = x.le.MaximumReimbursementDays,
                     ResetYear = x.le.ResetYear,
                     ResetMonth = x.le.ResetMonth,
                     ResetDay = x.le.ResetDay,
                     SpecificEmployeeDetailLeaveList = dbContext.SpecificEmployeeDetailLeave.Join(dbContext.AppConstants, sedl => sedl.EmployeeDetailLeaveId, ac => ac.AppConstantId, (sedl, ac) => new { sedl, ac }).Where(rs => rs.sedl.LeaveTypeId == x.eldltlr.lr.LeaveTypeId).Select(rs => new SpecificEmployeeDetailLeaveView
                     {
                         LeaveTypeId = rs.sedl.LeaveTypeId,
                         EmployeeDetailLeaveId = rs.sedl.EmployeeDetailLeaveId,
                         EmployeeDetailLeaveText = rs.ac.DisplayName
                     }).ToList(),
                     AdjustmentEffectiveFromDate = x.eldltlr.eldlt.eld.AdjustmentEffectiveFromDate,
                     AdjustmentBalanceLeave = x.eldltlr.eldlt.eld.AdjustmentBalanceLeave,
                     BalanceBasedOn = dbContext.AppConstants.Where(rs => rs.AppConstantId == x.eldltlr.eldlt.lt.BalanceBasedOn).Select(rs => new BalanceBasedOnDetails { BalanceBasedOnId = rs.AppConstantId, BalanceBasedOnText = rs.DisplayName, BalanceBasedOnValue = rs.AppConstantValue }).ToList(),
                     GrantMinimumNoOfRequestDay = x.eldltlr.lr.GrantMinimumNoOfRequestDay,
                     GrantMaximumNoOfRequestDay = x.eldltlr.lr.GrantMaximumNoOfRequestDay,
                     GrantMaximumNoOfPeriod = x.eldltlr.lr.GrantMaximumNoOfPeriod,
                     GrantMaximumNoOfDay = x.eldltlr.lr.GrantMaximumNoOfDay,
                     GrantMinimumGapTwoApplicationDay = x.eldltlr.lr.GrantMinimumGapTwoApplicationDay,
                     GrantUploadDocumentSpecificPeriodDay = x.eldltlr.lr.GrantUploadDocumentSpecificPeriodDay,
                     IsGrantRequestPastDay = x.eldltlr.lr.IsGrantRequestPastDay,
                     GrantRequestPastDay = x.eldltlr.lr.GrantRequestPastDay,
                     IsGrantRequestFutureDay = x.eldltlr.lr.IsGrantRequestFutureDay,
                     GrantRequestFutureDay = x.eldltlr.lr.GrantRequestFutureDay,
                     GrantEffectiveFromDate = dbContext.LeaveGrantRequestDetails.Where(rs => rs.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && rs.EmployeeID == x.eldltlr.eldlt.eld.EmployeeID && rs.Status == "Approved" && rs.IsActive == true).Select(rs => rs.EffectiveFromDate).FirstOrDefault(),
                     ResetPeriod = dbContext.AppConstants.Where(y => y.AppConstantId == x.le.ResetYear).Select(z => z.AppConstantValue).FirstOrDefault(),
                     LeaveAdjustmentDetails = dbContext.LeaveAdjustmentDetails.Where(y => y.EmployeeId == employeeId && y.LeavetypeId == x.eldltlr.eldlt.eld.LeaveTypeID).OrderByDescending(z => z.EffectiveFromDate).ToList(),
                     LeaveCarryForward = dbContext.LeaveCarryForward.Where(y => y.EmployeeID == employeeId && y.LeaveTypeID == x.eldltlr.eldlt.eld.LeaveTypeID).OrderByDescending(z => z.ResetDate).ToList(),
                     ToBeAdvanced = x.eldltlr.lr.ToBeAdvanced,
                     ResetLeaveAfter = x.eldltlr.lr.GrantResetLeaveAfterDays,
                     LeaveGrantRequestDetails = dbContext.LeaveGrantRequestDetails.Where(y => y.EmployeeID == employeeId &&
                       y.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && y.Status == "Approved" && y.BalanceDay != 0).OrderBy(x => x.EffectiveFromDate).ToList(),
                     AppliedLeaveDates = (from leave in dbContext.ApplyLeave
                                          join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                                          where leave.EmployeeId == employeeId && leave.LeaveTypeId == x.eldltlr.eldlt.eld.LeaveTypeID && ald.Date >= FromDate.AddYears(-1) && ald.Date <= ToDate.AddYears(1) && (leave.Status != "Cancelled" &&
                                          (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true)))
                                          select new AppliedLeaveTypeDetails
                                          {
                                              Date = ald.Date,
                                              IsFirstHalf = ald.IsFirstHalf,
                                              IsSecondHalf = ald.IsSecondHalf,
                                              IsFullDay = ald.IsFullDay,
                                              LeaveId = ald.LeaveId
                                          }).ToList()

                 }).FirstOrDefault();
            return EmployeeAvailableLeaveListDetails;
        }


    }
}
