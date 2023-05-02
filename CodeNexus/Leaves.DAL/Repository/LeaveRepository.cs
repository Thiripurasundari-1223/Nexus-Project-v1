using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SharedLibraries.Models.Notifications;
using System.Globalization;
using log4net.Core;
using System.Security.Cryptography;
using SharedLibraries.ViewModels;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Appraisal;

namespace Leaves.DAL.Repository
{
    public interface ILeaveRepository : IBaseRepository<LeaveTypes>
    {
        LeaveTypes GetByID(int pLeaveTypeId);
        List<LeaveView> GetAllLeaves();
        LeaveTypes GetByleaveId(int leaveId);
        List<LeaveMaxLimitAction> GetMaxlimitAction();
        //List<ProbationStatus> GetProbationStatus();
        List<MonthList> GetMonthsList();
        List<DaysList> GetDaysList();
        List<AllowUser> GetAllowUserToViewList();
        List<BalanceToBeDisplay> GetBalanceToBeDisplay();
        LeaveDetailsView GetLeaveDetailsViewById(int leaveId);
        LeaveEntitlementView GetLeaveEntitlementById(int leaveId);
        LeaveApplicableView GetLeaveApplicableById(int leaveId);
        LeaveRestrictionsView GetLeaveRestrictionsById(int leaveId);
        List<TeamLeaveView> GetTeamLeave(ReportingManagerTeamLeaveView managerTeamLeaveView);
        List<LeaveTypesView> GetLeaveTypes(int departmentId);
        List<EmployeeLeaveAdjustment> GetEmployeeLeaveAdjustmentDetails(int employeeId, DateTime fromDate, DateTime toDate);
        decimal GetLeaveByEmployeeId(int employeeId, int LeaveTypeId, DateTime fromDate, DateTime toDate, int leaveId, bool isEdit);
        List<AppliedLeaveTypeDetails> GetEmployeeLeaves(int employeeId, DateTime fromDate, DateTime toDate);
        LeaveTypeRestrictionsView GetLeaveRestrictionsDetailsById(int leaveId);
        List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndBackwardToDate(int employeeId, int LeaveTypeId, DateTime toDate);
        ApplyLeavesView GetAppliedLeaveByLeaveIds(int employeeId, List<int?> activeLeaveTypeId, DateTime previousDate);
        //List<HolidayViewDetails> GetCurrentFinancialYearHolidayListById(List<int?> activeHolidayId, DateTime previousDate, DateTime nextDate);
        List<ApplyLeavesView> GetAppliedLeaveByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate);
        List<ApplyLeavesView> GetAppliedLeaveDetailsByEmployeeIdAndLeaveId(int employeeId, int leaveId);
        bool GetByleaveType(string leaveType,int leaveTypeId);
        List<AppliedLeaveDetailsView> GetEmployeeExistsLeaves(int employeeId, DateTime fromDate);
        List<AppliedLeaveTypeDetails> GetEmployeeLeaveDetails(int employeeId, DateTime fromDate, DateTime toDate);
        List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndFromDate(int employeeId, int leaveTypeId,int LeaveId, DateTime fromDate);
        List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndToDate(int employeeId, int leaveTypeId,int LeaveId, DateTime toDate);
        List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndForwardToDate(int employeeId, int LeaveTypeId, DateTime toDate);
        List<ApplyLeaves> GetLeaveRequestByEmployeeId(int employeeId, int LeaveTypeId, DateTime fromDate, DateTime toDate, int leaveId, bool isEdit);
        //List<ApplyLeavesView> GetAppliedResLeaveByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate, int? leaveTypeId);
        List<AppliedLeaveTypeDetails> GetAppliedLeaveByLeaveTypeAndDate(int employeeId, DateTime fromDate, DateTime toDate, int leaveTypeId);
        bool checkAppliedLeaveByDate(int employeeId, int leaveTypeId, DateTime leaveDate, bool isFullday, bool isFirstHalf, bool isSecondHalf);
        bool checkAppliedLeaveByLeaveId(int employeeId, int leaveTypeId, DateTime leaveDate, bool isFullday, bool isFirstHalf, bool isSecondHalf, int leaveId);
        LeaveExceptionView GetLeaveExceptionById(int leaveId);
        List<AppliedLeaveTypeDetails> GetAllAppliedLeaveByLeaveTypeAndDate(int employeeId, DateTime fromDate, DateTime toDate, int leaveTypeId);
        List<AppliedLeaveTypeDetails> GetAppliedLeaveByLeaveTypeAndEmployee(int employeeId, int leaveTypeId);
        List<string> GetLeaveStatusToDisplay();
        List<ApplyLeavesView> GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestrictiond);
        List<EmployeeRequestCount> GetPendingLeaveCount(EmployeeListByDepartment employeeList);
        IndividualLeaveList GetEmployeeLeavesByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction);
    }
    public class LeaveRepository : BaseRepository<LeaveTypes>, ILeaveRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        #region Get By Leave Id
        public LeaveTypes GetByleaveId(int leaveId)
        {
            LeaveTypes LeaveDetail = new LeaveTypes();
            LeaveDetail = dbContext.LeaveTypes.Where(x => x.LeaveTypeId == leaveId).FirstOrDefault();
            if (LeaveDetail == null)
            {
                LeaveTypes AddLeaveDetail = new LeaveTypes()
                {
                    LeaveTypeId = 0
                };
                return AddLeaveDetail;
            }
            return LeaveDetail;
        }
        #endregion

        public LeaveDetailsView GetLeaveDetailsViewById(int leaveId)
        {
            LeaveDetailsView LeaveDetailsView = new LeaveDetailsView();
            LeaveTypes leaveDetails = dbContext.LeaveTypes.Where(x => x.LeaveTypeId == leaveId).FirstOrDefault();
            if (leaveDetails != null)
            {
                LeaveDetailsView.LeaveTypeId = leaveDetails.LeaveTypeId;
                LeaveDetailsView.LeaveType = leaveDetails.LeaveType;
                LeaveDetailsView.LeaveCode = leaveDetails.LeaveCode;
                LeaveDetailsView.LeaveAccruedType = leaveDetails.LeaveAccruedType;
                LeaveDetailsView.LeaveAccruedDay = leaveDetails.LeaveAccruedDay;
                LeaveDetailsView.LeaveAccruedNoOfDays = leaveDetails.LeaveAccruedNoOfDays;
                LeaveDetailsView.LeaveDescription = leaveDetails.LeaveDescription;
                //LeaveDetailsView.IsActive = leaveDetails.IsActive;
                LeaveDetailsView.LeaveTypesId = leaveDetails.LeaveTypesId;
                LeaveDetailsView.ProRate = leaveDetails.ProRate;
                LeaveDetailsView.EffectiveFromDate = leaveDetails.EffectiveFromDate;
                LeaveDetailsView.EffectiveToDate = leaveDetails.EffectiveToDate;
                LeaveDetailsView.BalanceBasedOn = leaveDetails.BalanceBasedOn;
                LeaveDetailsView.ProRateMonthList = dbContext.ProRateMonthDetails.Where(rs => rs.LeaveTypeId == leaveDetails.LeaveTypeId).Select(rs => new ProRateMonthList { Fromday = rs.Fromday, Today = rs.Today, Count = rs.Count }).ToList();
                LeaveDetailsView.AllowTimesheet = leaveDetails.AllowTimesheet;
            }
            return LeaveDetailsView;
        }
        public LeaveEntitlementView GetLeaveEntitlementById(int leaveId)
        {
            LeaveEntitlementView LeaveEntitlementView = new LeaveEntitlementView();
            LeaveEntitlement LeaveEntitlement = dbContext.LeaveEntitlement.Where(x => x.LeaveTypeId == leaveId).FirstOrDefault();
            if (LeaveEntitlement != null)
            {
                LeaveEntitlementView.LeaveEntitlementId = LeaveEntitlement.LeaveEntitlementId;
                LeaveEntitlementView.MaxLeaveAvailedYearId = LeaveEntitlement.MaxLeaveAvailedYearId;
                LeaveEntitlementView.MaxLeaveAvailedDays = LeaveEntitlement.MaxLeaveAvailedDays;
                LeaveEntitlementView.CarryForwardId = LeaveEntitlement.CarryForwardId;
                LeaveEntitlementView.MaximumCarryForwardDays = LeaveEntitlement.MaximumCarryForwardDays;
                LeaveEntitlementView.ReimbursementId = LeaveEntitlement.ReimbursementId;
                LeaveEntitlementView.MaximumReimbursementDays = LeaveEntitlement.MaximumReimbursementDays;
                LeaveEntitlementView.ResetYear = LeaveEntitlement.ResetYear;
                LeaveEntitlementView.ResetMonth = LeaveEntitlement.ResetMonth;
                LeaveEntitlementView.ResetDay = LeaveEntitlement.ResetDay;
                ////LeaveEntitlementView.RestrictLeaveApplicationDays = LeaveEntitlement.RestrictLeaveApplicationDays;
                ////LeaveEntitlementView.DocumentMandatoryDays = LeaveEntitlement.DocumentMandatoryDays;
                //LeaveEntitlementView.AllowEncashmentCarryForward = LeaveEntitlement.AllowEncashmentCarryForward;
                //LeaveEntitlementView.NoOfDays = LeaveEntitlement.NoOfDays;
                //LeaveEntitlementView.LeaveMaxLimitActionId = LeaveEntitlement.LeaveMaxLimitActionId;
                LeaveEntitlementView.LeaveTypeId = LeaveEntitlement.LeaveTypeId;
            }
            return LeaveEntitlementView;
        }
        public LeaveApplicableView GetLeaveApplicableById(int leaveId)
        {
            LeaveApplicableView LeaveApplicableView = new LeaveApplicableView();
            LeaveApplicable leaveApplicable = dbContext.LeaveApplicable.Where(x => x.LeaveTypeId == leaveId && x.Type == "LeaveApplicable").FirstOrDefault();
            if (leaveApplicable != null)
            {
                LeaveApplicableView.LeaveApplicableId = leaveApplicable.LeaveApplicableId;
                LeaveApplicableView.Gender_Male = leaveApplicable.Gender_Male;
                LeaveApplicableView.Gender_Female = leaveApplicable.Gender_Female;
                LeaveApplicableView.Gender_Others = leaveApplicable.Gender_Others;
                LeaveApplicableView.MaritalStatus_Single = leaveApplicable.MaritalStatus_Single;
                LeaveApplicableView.MaritalStatus_Married = leaveApplicable.MaritalStatus_Married;
                //LeaveApplicableView.ApplicableDepartmentId = leaveApplicable.ApplicableDepartmentId;
                //LeaveApplicableView.ApplicableDesginationId = leaveApplicable.ApplicableDesginationId;
                //LeaveApplicableView.ApplicableLocationId = leaveApplicable.ApplicableLocationId;
                //LeaveApplicableView.ApplicableRoleId = leaveApplicable.ApplicableRoleId;
                LeaveApplicableView.EmployeeTypeId = leaveApplicable.EmployeeTypeId;
                LeaveApplicableView.ProbationStatus = leaveApplicable.ProbationStatus;
                //LeaveApplicableView.ExceptionsDepartmentId = leaveApplicable.ExceptionsDepartmentId;
                //LeaveApplicableView.ExceptionsDesginationId = leaveApplicable.ExceptionsDesginationId;
                //LeaveApplicableView.ExceptionsLocationId = leaveApplicable.ExceptionsLocationId;
                //LeaveApplicableView.ExceptionsRoleId = leaveApplicable.ExceptionsRoleId;
                LeaveApplicableView.LeaveTypeId = leaveApplicable.LeaveTypeId;
            }
            return LeaveApplicableView;
        }
        public LeaveRestrictionsView GetLeaveRestrictionsById(int leaveId)
        {
            LeaveRestrictionsView LeaveRestrictionsView = new LeaveRestrictionsView();
            LeaveRestrictions LeaveRestrictions = dbContext.LeaveRestrictions.Where(x => x.LeaveTypeId == leaveId).FirstOrDefault();
            if (LeaveRestrictions != null)
            {
                LeaveRestrictionsView.LeaveRestrictionsId = LeaveRestrictions.LeaveRestrictionsId;
                //WeekendCountAfterDays = x.lr.WeekendCountAfterDays,
                //HolidayCountAfterDays = x.lr.HolidayCountAfterDays,
                LeaveRestrictionsView.ExceedLeaveBalance = LeaveRestrictions.ExceedLeaveBalance;
                LeaveRestrictionsView.AllowUsersViewId = LeaveRestrictions.AllowUsersViewId;
                LeaveRestrictionsView.BalanceDisplayedId = LeaveRestrictions.BalanceDisplayedId;
                LeaveRestrictionsView.DaysInAdvance = LeaveRestrictions.DaysInAdvance;
                LeaveRestrictionsView.AllowRequestNextDays = LeaveRestrictions.AllowRequestNextDays;
                LeaveRestrictionsView.DatesAppliedAdvance = LeaveRestrictions.DatesAppliedAdvance;
                LeaveRestrictionsView.MaximumLeavePerApplication = LeaveRestrictions.MaximumLeavePerApplication;
                LeaveRestrictionsView.MinimumGapTwoApplication = LeaveRestrictions.MinimumGapTwoApplication;
                LeaveRestrictionsView.MaximumConsecutiveDays = LeaveRestrictions.MaximumConsecutiveDays;
                LeaveRestrictionsView.EnableFileUpload = LeaveRestrictions.EnableFileUpload;
                LeaveRestrictionsView.MinimumNoOfApplicationsPeriod = LeaveRestrictions.MinimumNoOfApplicationsPeriod;
                LeaveRestrictionsView.AllowRequestPeriodId = LeaveRestrictions.AllowRequestPeriodId;
                LeaveRestrictionsView.MaximumLeave = LeaveRestrictions.MaximumLeave;
                LeaveRestrictionsView.MinimumGap = LeaveRestrictions.MinimumGap;
                LeaveRestrictionsView.MaximumConsecutive = LeaveRestrictions.MaximumConsecutive;
                LeaveRestrictionsView.EnableFile = LeaveRestrictions.EnableFile;
                LeaveRestrictionsView.CannotBeTakenTogether = LeaveRestrictions.CannotBeTakenTogether;
                LeaveRestrictionsView.AllowRequestDates = LeaveRestrictions.AllowRequestDates;
                LeaveRestrictionsView.LeaveTypeId = LeaveRestrictions.LeaveTypeId;
                LeaveRestrictionsView.AllowPastDates = LeaveRestrictions.AllowPastDates;
                LeaveRestrictionsView.AllowFutureDates = LeaveRestrictions.AllowFutureDates;
                LeaveRestrictionsView.IsAllowRequestNextDays = LeaveRestrictions.IsAllowRequestNextDays;
                LeaveRestrictionsView.IsToBeApplied = LeaveRestrictions.IsToBeApplied;
                LeaveRestrictionsView.Weekendsbetweenleaveperiod = LeaveRestrictions.Weekendsbetweenleaveperiod;
                LeaveRestrictionsView.Holidaybetweenleaveperiod = LeaveRestrictions.Holidaybetweenleaveperiod;
                LeaveRestrictionsView.DurationsAllowedId = dbContext.LeaveDuration.Join(dbContext.AppConstants, ld => ld.DurationId, ac => ac.AppConstantId, (ld, ac) => new { ld, ac }).Where(rs => rs.ld.LeaveTypeId == LeaveRestrictions.LeaveTypeId).Select(rs =>new DurationAllowed {DurationId= rs.ac.AppConstantId,DurationValue=rs.ac.DisplayName }).ToList();
                LeaveRestrictionsView.activeLeaveType = dbContext.LeaveTakenTogether.Join(dbContext.LeaveTypes, ltt => ltt.LeaveOrHolidayId, lt => lt.LeaveTypeId, (ltt, lt) => new { ltt, lt }).Where(rs => rs.ltt.LeaveTypeId == leaveId && rs.ltt.LeaveTakenType == "Leave").Select(rs => new ActiveLeaveList { leaveTypeId = rs.ltt.LeaveOrHolidayId, leaveType = rs.lt.LeaveType }).ToList();
                LeaveRestrictionsView.activeHoliday = dbContext.LeaveTakenTogether.Join(dbContext.Holiday, ltt => ltt.LeaveOrHolidayId, h => h.HolidayID, (ltt, h) => new { ltt, h }).Where(rs => rs.ltt.LeaveTypeId == leaveId && rs.ltt.LeaveTakenType == "Holiday").Select(rs => new ActiveHolidayList { holidayID = rs.ltt.LeaveOrHolidayId, holidayName = rs.h.HolidayName }).ToList();
                LeaveRestrictionsView.SpecificEmployeeDetailLeaveList = dbContext.SpecificEmployeeDetailLeave.Join(dbContext.AppConstants, sedl => sedl.EmployeeDetailLeaveId, ac => ac.AppConstantId, (sedl, ac) => new { sedl, ac }).Where(rs => rs.sedl.LeaveTypeId == leaveId).Select(rs => new SpecificEmployeeDetailLeaveList
                {
                    EmployeeDetailLeaveId = rs.sedl.EmployeeDetailLeaveId,
                    EmployeeDetailLeaveValue = rs.ac.DisplayName
                }).ToList();
                LeaveRestrictionsView.GrantMinimumNoOfRequestDay = LeaveRestrictions.GrantMinimumNoOfRequestDay;
                LeaveRestrictionsView.GrantMaximumNoOfRequestDay = LeaveRestrictions.GrantMaximumNoOfRequestDay;
                LeaveRestrictionsView.GrantMaximumNoOfPeriod = LeaveRestrictions.GrantMaximumNoOfPeriod;
                LeaveRestrictionsView.GrantMaximumNoOfDay = LeaveRestrictions.GrantMaximumNoOfDay;
                LeaveRestrictionsView.GrantMinimumGapTwoApplicationDay = LeaveRestrictions.GrantMinimumGapTwoApplicationDay;
                LeaveRestrictionsView.GrantUploadDocumentSpecificPeriodDay = LeaveRestrictions.GrantUploadDocumentSpecificPeriodDay;
                LeaveRestrictionsView.IsGrantRequestPastDay = LeaveRestrictions.IsGrantRequestPastDay;
                LeaveRestrictionsView.GrantRequestPastDay = LeaveRestrictions.GrantRequestPastDay;
                LeaveRestrictionsView.IsGrantRequestFutureDay = LeaveRestrictions.IsGrantRequestFutureDay;
                LeaveRestrictionsView.GrantRequestFutureDay = LeaveRestrictions.GrantRequestFutureDay;
                LeaveRestrictionsView.GrantResetLeaveAfterDays = LeaveRestrictions.GrantResetLeaveAfterDays;
                LeaveRestrictionsView.ToBeAdvanced = LeaveRestrictions.ToBeAdvanced;
            }
            return LeaveRestrictionsView;
        }

        #region Get Max limit Action List
        public List<LeaveMaxLimitAction> GetMaxlimitAction()
        {
            return dbContext.LeaveMaxLimitAction.ToList();
        }
        #endregion
        public LeaveTypes GetByID(int pLeaveTypeId)
        {
            return dbContext.LeaveTypes.Where(x => x.LeaveTypeId == pLeaveTypeId).FirstOrDefault();
        }
        public List<LeaveView> GetAllLeaves()
        {
            List<LeaveView> leaveView = new List<LeaveView>();
            leaveView = (from leaveDetails in dbContext.LeaveTypes
                         select new LeaveView
                         {
                             LeaveTypeId = leaveDetails.LeaveTypeId,
                             LeaveType = leaveDetails.LeaveType,
                             LeaveCode = leaveDetails.LeaveCode,
                             EmployeesTypeId = dbContext.LeaveApplicable.Where(x=>x.LeaveTypeId==leaveDetails.LeaveTypeId).Select(x=>x.EmployeeTypeId).FirstOrDefault(),
                             ProRate = leaveDetails.ProRate,
                             EffectiveFromDate = leaveDetails.EffectiveFromDate,
                             EffectiveToDate = leaveDetails.EffectiveToDate,
                             ModifiedOn = leaveDetails.ModifiedOn,
                             IsActive=leaveDetails.IsActive,
                             CreatedOn= leaveDetails.CreatedOn
                         }).OrderByDescending(x =>x.CreatedOn).ToList();
            return leaveView;
        }
        //#region Get Probation Status List
        //public List<ProbationStatus> GetProbationStatus()
        //{
        //    int i = 1;
        //    List<ProbationStatus> probationStatuse = new List<ProbationStatus>();
        //    List<SharedLibraries.ProbationStatus> probation = Enum.GetValues(typeof(SharedLibraries.ProbationStatus)).Cast<SharedLibraries.ProbationStatus>().ToList();
        //    foreach (SharedLibraries.ProbationStatus enumValue in (List<SharedLibraries.ProbationStatus>)probation)
        //    {
        //        List<ProbationStatus> statuses = new List<ProbationStatus>() {
        //        new ProbationStatus(){ Id = i, ProbastionStatus=enumValue.ToString()}
        //        };
        //        probationStatuse.AddRange(statuses);
        //        i++;
        //    }
        //    return probationStatuse;
        //}
        //#endregion
        #region Get Months List
        public List<MonthList> GetMonthsList()
        {
            int i = 1;
            List<MonthList> monthList = new List<MonthList>();
            List<SharedLibraries.Months> months = Enum.GetValues(typeof(SharedLibraries.Months)).Cast<SharedLibraries.Months>().ToList();
            foreach (SharedLibraries.Months enumValue in (List<SharedLibraries.Months>)months)
            {
                List<MonthList> statuses = new List<MonthList>() {
                new MonthList(){ Id = i, Month=enumValue.ToString()}
                };
                monthList.AddRange(statuses);
                i++;
            }
            return monthList;
        }
        #endregion
        #region Get Days List
        public List<DaysList> GetDaysList()
        {
            int i = 1;
            List<DaysList> daysList = new List<DaysList>();
            List<SharedLibraries.Days> days = Enum.GetValues(typeof(SharedLibraries.Days)).Cast<SharedLibraries.Days>().ToList();
            foreach (SharedLibraries.Days enumValue in (List<SharedLibraries.Days>)days)
            {
                List<DaysList> statuses = new List<DaysList>() {
                new DaysList(){ Id = i, Day=enumValue.ToString()}
                };
                daysList.AddRange(statuses);
                i++;
            }
            return daysList;
        }
        #endregion
        #region Get Allow Users To View
        public List<AllowUser> GetAllowUserToViewList()
        {
            int i = 1;
            List<AllowUser> allowUser = new List<AllowUser>();
            List<SharedLibraries.AllowUsersToView> days = Enum.GetValues(typeof(SharedLibraries.AllowUsersToView)).Cast<SharedLibraries.AllowUsersToView>().ToList();
            foreach (SharedLibraries.AllowUsersToView enumValue in (List<SharedLibraries.AllowUsersToView>)days)
            {
                List<AllowUser> statuses = new List<AllowUser>() {
                new AllowUser(){ Id = i, AllowUsers=enumValue.ToString()}
                };
                allowUser.AddRange(statuses);
                i++;
            }
            return allowUser;
        }
        #endregion
        #region Get balance to display
        public List<BalanceToBeDisplay> GetBalanceToBeDisplay()
        {
            int i = 1;
            List<BalanceToBeDisplay> balanceToBeDisplay = new List<BalanceToBeDisplay>();
            List<SharedLibraries.AllowUsersToView> days = Enum.GetValues(typeof(SharedLibraries.AllowUsersToView)).Cast<SharedLibraries.AllowUsersToView>().ToList();
            foreach (SharedLibraries.AllowUsersToView enumValue in (List<SharedLibraries.AllowUsersToView>)days)
            {
                List<BalanceToBeDisplay> statuses = new List<BalanceToBeDisplay>() {
                new BalanceToBeDisplay(){ Id = i, Balance=enumValue.ToString()}
                };
                balanceToBeDisplay.AddRange(statuses);
                i++;
            }
            return balanceToBeDisplay;
        }
        #endregion

        #region Get Leave status to display
        public List<string> GetLeaveStatusToDisplay()
        {
            List<string> teamLeaveStatus = (from leave in dbContext.ApplyLeave select leave.Status).Distinct().ToList();
            if (teamLeaveStatus == null)
            {
                teamLeaveStatus = new List<string>();
            }
            List<string> teamGrantLeaveStatus = (from emp in dbContext.EmployeeGrantLeaveApproval  select emp.Status).Distinct().ToList();
            if (teamGrantLeaveStatus?.Count > 0)
            {
                teamLeaveStatus = teamLeaveStatus?.Concat(teamGrantLeaveStatus).Distinct().ToList();
            }
            return teamLeaveStatus;
        }
        #endregion
        public List<TeamLeaveView> GetTeamLeave(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            List<TeamLeaveView> teamLeave = (from leave in dbContext.ApplyLeave
                                             join details in dbContext.LeaveTypes on leave.LeaveTypeId equals details.LeaveTypeId
                                             //where managerTeamLeaveView.ResourceId.Contains(leave.EmployeeId) && leave.FromDate >= managerTeamLeaveView.FromDate && leave.ToDate <= managerTeamLeaveView.ToDate && leave.IsActive == true
                                             where leave.ManagerId == (managerTeamLeaveView.ManagerId == 0 ? leave.ManagerId : managerTeamLeaveView.ManagerId) 
                                             && leave.FromDate >= managerTeamLeaveView.FromDate && leave.FromDate <= managerTeamLeaveView.ToDate
                                             && leave.LeaveTypeId == (managerTeamLeaveView.LeaveTypeId == 0 ? leave.LeaveTypeId : managerTeamLeaveView.LeaveTypeId)
                                             && leave.NoOfDays == (managerTeamLeaveView.NoOfDays == 0 ? leave.NoOfDays : managerTeamLeaveView.NoOfDays)
                                             && leave.Status == (string.IsNullOrEmpty(managerTeamLeaveView.LeaveStatus) ? leave.Status : managerTeamLeaveView.LeaveStatus)
                                             //&& leave.IsActive == true
                                             select new TeamLeaveView
                                             {
                                                 EmployeeId = leave.EmployeeId,
                                                 LeaveTypeId = leave.LeaveTypeId,
                                                 FromDate = leave.FromDate,
                                                 ToDate = leave.ToDate,
                                                 NoOfDays = leave.NoOfDays,
                                                 Reason = leave.Reason,
                                                 Status = leave.Status,
                                                 Feedback = leave.Feedback == "" ? dbContext.LeaveRejectionReason.Where(x => x.LeaveRejectionReasonId == leave.LeaveRejectionReasonId).Select(x => x.LeaveRejectionReasons).FirstOrDefault() : leave.Feedback,
                                                 LeaveId = leave.LeaveId,
                                                 LeaveType = details.LeaveType,
                                                 IsGrantLeaveRequest = false,
                                                 CreatedOn = leave.CreatedOn,
                                                 ManagerId = leave.ManagerId,
                                                 AppliedLeaveDetails = dbContext.AppliedLeaveDetails.Where(x => x.LeaveId == leave.LeaveId).Select(x =>
                                                                                                          new AppliedLeaveDetailsView
                                                                                                          {
                                                                                                              AppliedLeaveDetailsID = x.AppliedLeaveDetailsID,
                                                                                                              LeaveId = x.LeaveId,
                                                                                                              Date = x.Date,
                                                                                                              IsFullDay = x.IsFullDay,
                                                                                                              IsFirstHalf = x.IsFirstHalf,
                                                                                                              IsSecondHalf = x.IsSecondHalf,
                                                                                                              CompensatoryOffId = x.CompensatoryOffId,
                                                                                                              AppliedLeaveStatus = x.AppliedLeaveStatus,
                                                                                                              CreatedBy = x.CreatedBy,
                                                                                                          }
                                                                                                        ).ToList()
                                             }).OrderByDescending(x => x.CreatedOn).Skip(managerTeamLeaveView.NoOfRecord * (managerTeamLeaveView.PageNumber)).Take(managerTeamLeaveView.NoOfRecord).ToList(); 
            if (teamLeave == null)
            {
                teamLeave = new List<TeamLeaveView>();
            }
            List<TeamLeaveView> teamGrantLeave = (from re in dbContext.LeaveGrantRequestDetails
                                                  join emp in dbContext.EmployeeGrantLeaveApproval on re.LeaveGrantDetailId equals emp.LeaveGrantDetailId
                                                  where re.CreatedOn >= managerTeamLeaveView.FromDate && re.CreatedOn <= managerTeamLeaveView.ToDate && re.IsActive == true
                                                  && emp.ApproverEmployeeId == (managerTeamLeaveView.ManagerEmployeeId == 0 ? emp.ApproverEmployeeId : managerTeamLeaveView.ManagerEmployeeId) && emp.Status != null
                                                  && re.LeaveTypeId == (managerTeamLeaveView.LeaveTypeId == 0 ? re.LeaveTypeId : managerTeamLeaveView.LeaveTypeId)
                                                  && re.NumberOfDay == (managerTeamLeaveView.NoOfDays == 0 ? re.NumberOfDay : managerTeamLeaveView.NoOfDays)
                                                  && emp.Status == (string.IsNullOrEmpty(managerTeamLeaveView.LeaveStatus) ? emp.Status : managerTeamLeaveView.LeaveStatus)
                                                  select new TeamLeaveView
                                                  {
                                                      EmployeeId = re.EmployeeID,
                                                      LeaveTypeId = re.LeaveTypeId,
                                                      FromDate = re.EffectiveFromDate,
                                                      ToDate = re.EffectiveFromDate.Value.AddDays(re.NumberOfDay == null ? 0 : (double)re.NumberOfDay),
                                                      NoOfDays = re.NumberOfDay == null ? 0 : GetRoundOff((decimal)re.NumberOfDay),
                                                      Reason = re.Reason,
                                                      Status = emp.Status,
                                                      Feedback = emp.Comments,
                                                      LeaveId = 0,
                                                      LeaveGrantDetailId = re.LeaveGrantDetailId,
                                                      LeaveType = dbContext.LeaveTypes.Where(rs => rs.LeaveTypeId == re.LeaveTypeId).Select(rs => rs.LeaveType).FirstOrDefault(),
                                                      AppliedLeaveDetails = new List<AppliedLeaveDetailsView>(),
                                                      IsGrantLeaveRequest = true,
                                                      LevelId = emp.LevelId,
                                                      ManagerId = emp.ApproverEmployeeId,
                                                      CreatedOn = re.CreatedOn,
                                                      ListOfDocuments = dbContext.LeaveGrantDocumentDetails.Where(x => x.LeaveGrantDetailId == re.LeaveGrantDetailId).Select(x => new SupportingDocuments
                                                      {
                                                          DocumentId = x.LeaveGrantDocumentDetailId,
                                                          DocumentName = x.DocumentName,
                                                          DocumentSize = 0,
                                                          DocumentCategory = "",
                                                          IsApproved = x.IsActive == null ? false : (bool)x.IsActive,
                                                          DocumentType = x.DocumentType
                                                      }).ToList()
                                                  }).OrderByDescending(x => x.CreatedOn).Skip(managerTeamLeaveView.NoOfRecord * (managerTeamLeaveView.PageNumber)).Take(managerTeamLeaveView.NoOfRecord).ToList();
            if (teamGrantLeave?.Count > 0)
            {
                teamLeave = teamLeave?.Concat(teamGrantLeave).OrderByDescending(x => x.CreatedOn).Skip(managerTeamLeaveView.NoOfRecord * (managerTeamLeaveView.PageNumber)).Take(managerTeamLeaveView.NoOfRecord).ToList();
            }            
            return teamLeave;

        }
        public List<LeaveTypesView> GetLeaveTypes(int departmentId)
        {
            return (from leave in dbContext.LeaveTypes
                    join applicable in dbContext.LeaveDepartment on leave.LeaveTypeId equals applicable.LeaveTypeId
                    join restriction in dbContext.LeaveRestrictions on applicable.LeaveTypeId equals restriction.LeaveTypeId
                    where (applicable.LeaveApplicableDepartmentId == departmentId || applicable.LeaveApplicableDepartmentId == 0)
                    && applicable.LeaveExceptionDepartmentId != departmentId //&& leave.IsActive == true
                    select new LeaveTypesView
                    {
                        LeaveTypeId = leave.LeaveTypeId,
                        LeaveType = leave.LeaveType,
                        MandatoryDays = restriction.EnableFileUpload,
                    }).ToList();
        }
        public List<EmployeeLeaveAdjustment> GetEmployeeLeaveAdjustmentDetails(int employeeId ,DateTime fromDate , DateTime toDate)
        {
            List<EmployeeLeaveAdjustment> employeeLeaveAdjustment = new();
            employeeLeaveAdjustment = (from eld in dbContext.EmployeeLeaveDetails
                                       join ldetail in dbContext.LeaveTypes on eld.LeaveTypeID equals ldetail.LeaveTypeId
                                       join ent in dbContext.LeaveEntitlement on ldetail.LeaveTypeId equals ent.LeaveTypeId
                                       where eld.EmployeeID == employeeId && ldetail.EffectiveFromDate <= toDate && (ldetail.EffectiveToDate == null || ldetail.EffectiveToDate >= fromDate) &&
                                       ldetail.IsActive == true
                                       select new EmployeeLeaveAdjustment
                                       {
                                           EmployeeId = employeeId,
                                           LeaveTypeId = eld.LeaveTypeID,
                                           LeaveType = ldetail.LeaveType,
                                           AdjustmentEffectiveFromDate = eld.AdjustmentEffectiveFromDate,
                                           AdjustmentLeaveBalance = eld.AdjustmentBalanceLeave,
                                           LeaveBalance = eld.BalanceLeave,
                                           ActualBalanceLeave = (eld.AdjustmentEffectiveFromDate != null ) ? eld.AdjustmentBalanceLeave : eld.BalanceLeave,
                                           EffectiveFromDate = ldetail.EffectiveFromDate,
                                           EffectiveToDate = ldetail.EffectiveToDate,
                                           LeaveAccruedType = ldetail.LeaveAccruedType,
                                           LeaveAccruedTypeName = dbContext.AppConstants.Where(x => x.AppConstantId == ldetail.LeaveAccruedType).Select(x => x.AppConstantValue).FirstOrDefault(),
                                           LeaveAccruedDay = ldetail.LeaveAccruedDay,
                                           LeaveAccruedNoOfDays = ldetail.LeaveAccruedNoOfDays,
                                           CarryForwardId = ent.CarryForwardId,
                                           CarryForwardIdName = dbContext.AppConstants.Where(x => x.AppConstantId == ent.CarryForwardId).Select(x => x.AppConstantValue).FirstOrDefault(),
                                           MaximumCarryForwardDays = ent.MaximumCarryForwardDays,
                                           ReimbursementId = ent.ReimbursementId,
                                           ReimbursementIdName = dbContext.AppConstants.Where(x => x.AppConstantId == ent.ReimbursementId).Select(x => x.AppConstantValue).FirstOrDefault(),
                                           MaximumReimbursementDays = ent.MaximumReimbursementDays,
                                           ResetYear = ent.ResetYear,
                                           ResetYearName = dbContext.AppConstants.Where(x => x.AppConstantId == ent.ResetYear).Select(x => x.AppConstantValue).FirstOrDefault(),
                                           ResetMonth = ent.ResetMonth,
                                           ResetDay = ent.ResetDay,
                                           BalanceBasedOn = dbContext.AppConstants.Where(rs => rs.AppConstantId == ldetail.BalanceBasedOn).Select(rs => new BalanceBasedOnDetails { BalanceBasedOnId = rs.AppConstantId, BalanceBasedOnText = rs.DisplayName, BalanceBasedOnValue = rs.AppConstantValue }).FirstOrDefault(),
                                           AdjustmentDays = dbContext.LeaveAdjustmentDetails.Where(x => x.EmployeeId == employeeId && x.LeavetypeId == eld.LeaveTypeID && x.EffectiveFromDate>= fromDate && x.EffectiveFromDate<=toDate).Sum(x=>x.NoOfDays==null?0:(decimal)x.NoOfDays),
                                           LeaveAdjustmentDetails = dbContext.LeaveAdjustmentDetails.Where(y => y.EmployeeId == employeeId && y.LeavetypeId == eld.LeaveTypeID).OrderByDescending(z => z.EffectiveFromDate).ToList(),
                                           LeaveCarryForward =dbContext.LeaveCarryForward.Where(x => x.EmployeeID == employeeId && x.LeaveTypeID == eld.LeaveTypeID).OrderByDescending(x=>x.ResetDate).ToList(),
                                           LeaveGrantRequestDetails = dbContext.LeaveGrantRequestDetails.Where(y => y.EmployeeID == employeeId &&
                       y.LeaveTypeId == eld.LeaveTypeID && y.Status == "Approved" && y.BalanceDay != 0).OrderBy(x => x.EffectiveFromDate).ToList(),
                                           AppliedLeaveDates = (from leave in dbContext.ApplyLeave
                                                                join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                                                                where leave.EmployeeId == employeeId && leave.LeaveTypeId == eld.LeaveTypeID && ald.Date >= fromDate.AddYears(-1) && ald.Date <= toDate.AddYears(1) && (leave.Status != "Cancelled" &&
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
            return employeeLeaveAdjustment;
        }
        
        public decimal GetLeaveByEmployeeId(int employeeId, int LeaveTypeId, DateTime fromDate, DateTime toDate, int leaveId, bool isEdit)
        {
            decimal totalLeave = 0;
            List<AppliedLeaveDetails> AppliedLeaveDetailsList = new List<AppliedLeaveDetails>();
            if (isEdit)
            {
                AppliedLeaveDetailsList= dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails,al=>al.LeaveId,ald=>ald.LeaveId,(al,ald)=>new {al,ald }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == LeaveTypeId
                && x.al.LeaveId!= leaveId && x.ald.AppliedLeaveStatus!=false
                && x.al.FromDate.Date >= fromDate.Date && x.al.FromDate <= toDate.Date).Select(x=>new AppliedLeaveDetails { 
                AppliedLeaveDetailsID=x.ald.AppliedLeaveDetailsID,
                Date=x.ald.Date,
                IsFullDay=x.ald.IsFullDay,
                IsFirstHalf=x.ald.IsFirstHalf,
                IsSecondHalf=x.ald.IsSecondHalf,
                AppliedLeaveStatus=x.ald.AppliedLeaveStatus,
                CompensatoryOffId=x.ald.CompensatoryOffId,
                LeaveId=x.ald.LeaveId,
                CreatedOn=x.ald.CreatedOn,
                CreatedBy=x.ald.CreatedBy,
                ModifiedBy=x.ald.ModifiedBy,
                ModifiedOn=x.ald.ModifiedOn
                }).ToList();
            }
            else
            {
                AppliedLeaveDetailsList = dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, al => al.LeaveId, ald => ald.LeaveId, (al, ald) => new { al, ald }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == LeaveTypeId
                             && x.ald.AppliedLeaveStatus != false
                             && x.al.FromDate.Date >= fromDate.Date && x.al.FromDate <= toDate.Date).Select(x => new AppliedLeaveDetails
                             {
                                 AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,
                                 Date = x.ald.Date,
                                 IsFullDay = x.ald.IsFullDay,
                                 IsFirstHalf = x.ald.IsFirstHalf,
                                 IsSecondHalf = x.ald.IsSecondHalf,
                                 AppliedLeaveStatus = x.ald.AppliedLeaveStatus,
                                 CompensatoryOffId = x.ald.CompensatoryOffId,
                                 LeaveId = x.ald.LeaveId,
                                 CreatedOn = x.ald.CreatedOn,
                                 CreatedBy = x.ald.CreatedBy,
                                 ModifiedBy = x.ald.ModifiedBy,
                                 ModifiedOn = x.ald.ModifiedOn
                             }).ToList();
            }
            if(AppliedLeaveDetailsList!=null&& AppliedLeaveDetailsList?.Count>0)
            {
                foreach(AppliedLeaveDetails items in AppliedLeaveDetailsList)
                {
                    if(items.IsFullDay)
                    {
                        totalLeave += 1;
                    }
                    else if(items.IsFirstHalf)
                    {
                        totalLeave += 0.5m;
                    }
                    else if (items.IsSecondHalf)
                    {
                        totalLeave += 0.5m;
                    }
                }
            }
            return totalLeave;
        }
        public List<AppliedLeaveTypeDetails> GetEmployeeLeaves(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<AppliedLeaveTypeDetails> leaves = new List<AppliedLeaveTypeDetails>();
            leaves = (from leave in dbContext.ApplyLeave
                      join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                      //where leave.EmployeeId == employeeId && leave.FromDate >= fromDate && leave.ToDate <= toDate && leave.Status != "Rejected"
                      where leave.EmployeeId == employeeId && ald.Date >= fromDate && ald.Date <= toDate && (leave.Status != "Cancelled" &&
                      (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true)))
                      select new AppliedLeaveTypeDetails
                      {
                          Date = ald.Date,
                          IsFirstHalf = ald.IsFirstHalf,
                          IsSecondHalf = ald.IsSecondHalf,
                          IsFullDay = ald.IsFullDay,
                          LeaveId = ald.LeaveId
                      }).ToList();
            return leaves;
        }
        public LeaveTypeRestrictionsView GetLeaveRestrictionsDetailsById(int leaveId)
        {
            LeaveTypeRestrictionsView LeaveTypeRestrictionsView = new LeaveTypeRestrictionsView();
            LeaveRestrictions LeaveRestrictions = dbContext.LeaveRestrictions.Where(x => x.LeaveTypeId == leaveId).FirstOrDefault();
            LeaveEntitlement leaveEntitlement = dbContext.LeaveEntitlement.Where(x => x.LeaveTypeId == leaveId).FirstOrDefault();
            if (LeaveRestrictions != null)
            {
                LeaveTypeRestrictionsView.LeaveRestrictionsId = LeaveRestrictions.LeaveRestrictionsId;
                LeaveTypeRestrictionsView.MaximumLeavePerApplication = LeaveRestrictions.MaximumLeavePerApplication;
                LeaveTypeRestrictionsView.MinimumGapTwoApplication = LeaveRestrictions.MinimumGapTwoApplication;
                LeaveTypeRestrictionsView.MinimumNoOfApplicationsPeriod = LeaveRestrictions.MinimumNoOfApplicationsPeriod;
                LeaveTypeRestrictionsView.AllowRequestPeriodId = LeaveRestrictions.AllowRequestPeriodId;
                LeaveTypeRestrictionsView.MaximumLeave = LeaveRestrictions.MaximumLeave;
                LeaveTypeRestrictionsView.MinimumGap = LeaveRestrictions.MinimumGap;
                LeaveTypeRestrictionsView.AppConstantsView = GetAppconstantDetailsById(LeaveRestrictions.AllowRequestPeriodId);
                LeaveTypeRestrictionsView.activeLeaveType = dbContext.LeaveTakenTogether.Join(dbContext.LeaveTypes, ltt => ltt.LeaveOrHolidayId, lt => lt.LeaveTypeId, (ltt, lt) => new { ltt, lt }).Where(rs => rs.ltt.LeaveTypeId == leaveId && rs.ltt.LeaveTakenType == "Leave").Select(rs => new ActiveLeaveList { leaveTypeId = rs.ltt.LeaveOrHolidayId, leaveType = rs.lt.LeaveType }).ToList();
                LeaveTypeRestrictionsView.activeHoliday = dbContext.LeaveTakenTogether.Join(dbContext.Holiday, ltt => ltt.LeaveOrHolidayId, h => h.HolidayID, (ltt, h) => new { ltt, h }).Where(rs => rs.ltt.LeaveTypeId == leaveId && rs.ltt.LeaveTakenType == "Holiday").Select(rs => new ActiveHolidayList { holidayID = rs.ltt.LeaveOrHolidayId, holidayName = rs.h.HolidayName }).ToList();
                LeaveTypeRestrictionsView.MaxLeaveAvailedYearId = leaveEntitlement.MaxLeaveAvailedYearId;
                LeaveTypeRestrictionsView.MaxLeaveAvailedDays = leaveEntitlement.MaxLeaveAvailedDays;
                LeaveTypeRestrictionsView.EntitlementAppConstantsView = GetAppconstantDetailsById(leaveEntitlement.MaxLeaveAvailedYearId);
                LeaveTypeRestrictionsView.GrantMaximumPeriodAppConstantsView = GetAppconstantDetailsById(LeaveRestrictions.GrantMaximumNoOfPeriod);
                LeaveTypeRestrictionsView.GrantMaximumNoOfDay = LeaveRestrictions.GrantMaximumNoOfDay;
                LeaveTypeRestrictionsView.GrantMinimumGapTwoApplicationDay = LeaveRestrictions.GrantMinimumGapTwoApplicationDay;
                LeaveTypeRestrictionsView.MaximumConsecutiveDays = LeaveRestrictions.MaximumConsecutiveDays;
                LeaveTypeRestrictionsView.Weekendsbetweenleaveperiod = LeaveRestrictions.Weekendsbetweenleaveperiod;
                LeaveTypeRestrictionsView.GrantRequestFutureDay = LeaveRestrictions.GrantRequestFutureDay;
                LeaveTypeRestrictionsView.Holidaybetweenleaveperiod = LeaveRestrictions.Holidaybetweenleaveperiod;
                LeaveTypeRestrictionsView.GrantResetLeaveAfterDays = LeaveRestrictions.GrantResetLeaveAfterDays;
            }
            return LeaveTypeRestrictionsView;
        }
        #region Get Appconstant Type List
        public List<AppConstantsView> GetAppconstantDetailsById(int? appConstantId)
        {
            List<AppConstantsView> appConstantList = dbContext.AppConstants.Where(x => x.AppConstantId == appConstantId).Select(x => new AppConstantsView { AppConstantId = x.AppConstantId, AppConstantType = x.AppConstantType, DisplayName = x.DisplayName, AppConstantValue = x.AppConstantValue }).ToList();
            return appConstantList;
        }
        #endregion
        #region Get Employee Apply Leave Details By EmployeeId
        public List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndBackwardToDate(int employeeId, int LeaveTypeId, DateTime toDate)
        {
            return dbContext.ApplyLeave.Where(x => x.EmployeeId == employeeId && x.LeaveTypeId== LeaveTypeId
            && x.Status != "Rejected" && x.ToDate<= toDate).OrderByDescending(x => x.LeaveId).Take(1).ToList();
        }
        #endregion
        #region Get Active Leaves by Previous Date
        public ApplyLeavesView GetAppliedLeaveByLeaveIds(int employeeId,List<int?> activeLeaveTypeId, DateTime previousDate)
        {
          return  (from leave in dbContext.ApplyLeave
             join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId             
             where leave.EmployeeId == employeeId && ald.Date == previousDate && (leave.Status != "Cancelled" &&
             (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && activeLeaveTypeId.Contains(leave.LeaveTypeId) 
             select new ApplyLeavesView
             {
                 LeaveId = leave.LeaveId,
                 LeaveType= dbContext.LeaveTypes.Where(y => y.LeaveTypeId == leave.LeaveTypeId).Select(z => z.LeaveType).FirstOrDefault()

             }).FirstOrDefault();
        }
        #endregion
        #region Get Current Financial year Holiday List By Holid Id
        public List<HolidayViewDetails> GetCurrentFinancialYearHolidayListById(List<int?> activeHolidayId, DateTime previousDate, DateTime nextDate)
        {
            List<HolidayViewDetails> holidayList = dbContext.Holiday.Where(x => (x.HolidayDate == previousDate || x.HolidayDate == nextDate) && activeHolidayId.Contains(x.HolidayID) && x.IsActive == true)
                .Select(x => new HolidayViewDetails
                { HolidayID = x.HolidayID, Year = x.Year, HolidayName = x.HolidayName, HolidayCode = x.HolidayCode, HolidayDescription = x.HolidayDescription, HolidayDate = x.HolidayDate, IsActive = x.IsActive, IsRestrictHoliday = x.IsRestrictHoliday }).ToList();
            return holidayList;
        }
        #endregion
        #region Get Employee Applied Leave by EmployeeId
        public List<ApplyLeavesView> GetAppliedLeaveByEmployeeId(int employeeId,DateTime fromDate, DateTime toDate)
        {
            List<ApplyLeavesView> appliedLeave = dbContext.ApplyLeave.Where(x => x.EmployeeId == employeeId 
                       && x.Status!= "Cancelled"
                      && x.FromDate.Date >= fromDate && x.FromDate.Date <= toDate).Select(x => new ApplyLeavesView
                      {
                          LeaveId = x.LeaveId,
                          EmployeeId = x.EmployeeId,
                          LeaveTypeId = x.LeaveTypeId,
                          FromDate = x.FromDate,
                          ToDate = x.ToDate,
                          NoOfDays = x.NoOfDays,
                          Reason = x.Reason,
                          Status = x.Status,
                          Feedback = x.Feedback,
                          LeaveRejectionReasonId = x.LeaveRejectionReasonId,
                          IsActive = x.IsActive,
                          LeaveType = dbContext.LeaveTypes.Where(y=>y.LeaveTypeId==x.LeaveTypeId).Select(z=>z.LeaveType).FirstOrDefault(),
                          AppliedLeaveDetails = dbContext.AppliedLeaveDetails.Where(rs => rs.LeaveId == x.LeaveId).Select(rs => new AppliedLeaveDetailsView { AppliedLeaveDetailsID = rs.AppliedLeaveDetailsID, Date = rs.Date, IsFullDay = rs.IsFullDay, IsFirstHalf = rs.IsFirstHalf, IsSecondHalf = rs.IsSecondHalf, LeaveId = rs.LeaveId, CompensatoryOffId = rs.CompensatoryOffId, CreatedBy = rs.CreatedBy, AppliedLeaveStatus = rs.AppliedLeaveStatus }).ToList(),
                          CreatedBy = x.CreatedBy,
                          IsGrantLeave=false,
                          CreatedOn=x.CreatedOn

                      }).OrderByDescending(x => x.CreatedOn).ToList();
            if (appliedLeave == null)
            {
                appliedLeave = new List<ApplyLeavesView>();
            }
            List<ApplyLeavesView> appliedGrantLeave = dbContext.LeaveGrantRequestDetails.Where(x => x.EmployeeID == employeeId && x.Status != "Cancelled"
             && x.CreatedOn >= fromDate && x.CreatedOn <= toDate && x.IsActive == true && x.IsLeaveAdjustment != true).Select(x => new ApplyLeavesView
             {
                 LeaveGrantDetailId = x.LeaveGrantDetailId,
                 EmployeeId = x.EmployeeID,
                 LeaveTypeId = x.LeaveTypeId,
                 LeaveType=dbContext.LeaveTypes.Where(rs=>rs.LeaveTypeId==x.LeaveTypeId).Select(rs=>rs.LeaveType).FirstOrDefault(),
                 FromDate = (DateTime)x.EffectiveFromDate,
                 ToDate = x.EffectiveFromDate.Value.AddDays(x.NumberOfDay == null ? 0 : (double)x.NumberOfDay),
                 NoOfDays = x.NumberOfDay == null ? 0 : GetRoundOff((decimal)x.NumberOfDay),
                 Reason = x.Reason,
                 Status=x.Status,
                 IsActive=x.IsActive==null?false:(bool)x.IsActive,
                 CreatedBy=x.CreatedBy,
                 IsGrantLeave = true,
                 CreatedOn=x.CreatedOn,
                 GrantLeaveApprovalStatus = dbContext.EmployeeGrantLeaveApproval.Where(y => y.LeaveGrantDetailId == x.LeaveGrantDetailId).Select(z =>
                     new GrantLeaveApprovalView { LevelId = z.LevelId, LevelApprovalEmployeeId = z.ApproverEmployeeId,                         
                         LevelApproverName = "", Comments=z.Comments, Status=z.Status }).ToList()
             }).ToList();
            if(appliedGrantLeave != null)
            {
                appliedLeave = appliedLeave.Concat(appliedGrantLeave).OrderByDescending(x => x.CreatedOn).ToList();
            }
            return appliedLeave;
        }
        #endregion
        #region Get Employee Applied Leave by EmployeeId
        public List<ApplyLeavesView> GetAppliedLeaveDetailsByEmployeeIdAndLeaveId(int employeeId, int leaveId)
        {
            return dbContext.ApplyLeave
                .Join(dbContext.LeaveTypes, al => al.LeaveTypeId, lt => lt.LeaveTypeId, (al, lt) => new { al, lt }).Where(x => x.al.EmployeeId == employeeId
                      //&& x.al.Status != "Rejected"
                      && x.al.LeaveId == leaveId).Select(x => new ApplyLeavesView
                      {
                          LeaveId = x.al.LeaveId,
                          EmployeeId = x.al.EmployeeId,
                          LeaveTypeId = x.al.LeaveTypeId,
                          FromDate = x.al.FromDate,
                          ToDate = x.al.ToDate,
                          NoOfDays = x.al.NoOfDays,
                          Reason = x.al.Reason,
                          Status = x.al.Status,
                          Feedback = x.al.Feedback,
                          LeaveRejectionReasonId = x.al.LeaveRejectionReasonId,
                          IsActive = x.al.IsActive,
                          LeaveType = x.lt.LeaveType,
                          AppliedLeaveDetails = dbContext.AppliedLeaveDetails.Where(rs => rs.LeaveId == x.al.LeaveId).Select(rs => new AppliedLeaveDetailsView { AppliedLeaveDetailsID = rs.AppliedLeaveDetailsID, Date = rs.Date, IsFullDay = rs.IsFullDay, IsFirstHalf = rs.IsFirstHalf, IsSecondHalf = rs.IsSecondHalf, LeaveId = rs.LeaveId, CompensatoryOffId = rs.CompensatoryOffId, CreatedBy = rs.CreatedBy, AppliedLeaveStatus = rs.AppliedLeaveStatus }).ToList(),
                          CreatedBy = x.al.CreatedBy

                      }).OrderByDescending(x => x.FromDate).ToList();
        }
        #endregion
        #region Get By Leave Id
        public bool GetByleaveType(string leaveType,int leaveTypeId)
        {
            bool isExists = false;
            List<LeaveTypes> LeaveDetail = new List<LeaveTypes>();
            LeaveDetail=dbContext.LeaveTypes.Where(x => x.LeaveType == leaveType).ToList();
            if(LeaveDetail?.Count>0)
            {
                if (LeaveDetail[0].LeaveTypeId == leaveTypeId)
                {
                    isExists = false;
                }
                else
                {
                    isExists = true;
                }
            }
            return isExists;
        }
        #endregion
        #region Get Exist Leave By EmployeeId
        public List<AppliedLeaveDetailsView> GetEmployeeExistsLeaves(int employeeId, DateTime fromDate)
        {
            List<AppliedLeaveDetailsView> leaves = new List<AppliedLeaveDetailsView>();
            leaves = (from leave in dbContext.ApplyLeave
                      join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                      //where leave.EmployeeId == employeeId && ald.Date == fromDate && (ald.AppliedLeaveStatus==null || ald.AppliedLeaveStatus==true)
                      where leave.EmployeeId == employeeId && ald.Date == fromDate && (leave.Status != "Cancelled" &&
                      (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true)))
                      select new AppliedLeaveDetailsView
                      {
                          Date = ald.Date,
                          IsFirstHalf = ald.IsFirstHalf,
                          IsSecondHalf = ald.IsSecondHalf,
                          IsFullDay = ald.IsFullDay
                      }).ToList();
            return leaves;
        }
        #endregion

       
        public List<AppliedLeaveTypeDetails> GetEmployeeLeaveDetails(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<AppliedLeaveTypeDetails> leaves = new List<AppliedLeaveTypeDetails>();
            leaves = (from leave in dbContext.ApplyLeave
                      join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                      //join lt in dbContext.LeaveTypes on leave.LeaveTypeId equals lt.LeaveTypeId
                      where leave.EmployeeId == employeeId && ald.Date >= fromDate && ald.Date <= toDate && (leave.Status != "Cancelled" &&
                      (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true)))
                      select new AppliedLeaveTypeDetails
                      {
                          LeaveId = ald.LeaveId,
                          LeaveTypeId = leave.LeaveTypeId,
                          LeaveType = dbContext.LeaveTypes.Where(x => x.LeaveTypeId == leave.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault(),
                          Date = ald.Date,
                          IsFirstHalf = ald.IsFirstHalf,
                          IsFullDay = ald.IsFullDay,
                          IsSecondHalf = ald.IsSecondHalf,
                          AppliedLeaveStatus = ald.AppliedLeaveStatus,
                          NoOfDays = leave.NoOfDays,
                          reason = leave.Reason,
                          Status = leave.Status,
                          FromDate=leave.FromDate,
                          ToDate=leave.ToDate,
                          appliedLeaveDetailId = ald.AppliedLeaveDetailsID,
                          //LeaveTypeName=dbContext.AppConstants.Where(x=>x.AppConstantId== lt.LeaveTypesId).Select(x=>x.AppConstantValue).FirstOrDefault()
                          LeaveTypeName = dbContext.AppConstants.Join(dbContext.LeaveTypes, al => al.AppConstantId, ald => ald.LeaveTypesId, (al, ald) => new { al, ald }).Where(x => x.ald.LeaveTypeId == leave.LeaveTypeId).Select(x => x.al.AppConstantValue).FirstOrDefault()
                      }).ToList();
            return leaves;
        }
        #region Get Employee Edit Apply Leave Details By EmployeeId and FromDate
        public List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndFromDate(int employeeId, int leaveTypeId,int LeaveId,DateTime fromDate)
        {
            return dbContext.ApplyLeave.Where(x => x.EmployeeId == employeeId &&x.LeaveTypeId== leaveTypeId &&x.LeaveId!= LeaveId && x.FromDate<= fromDate
            && x.Status != "Rejected").OrderByDescending(x => x.LeaveId).Take(1).ToList();
        }
        #endregion
        #region Get Employee Edit Apply Leave Details By EmployeeId and ToDate
        public List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndToDate(int employeeId, int leaveTypeId,int LeaveId, DateTime toDate)
        {
            return dbContext.ApplyLeave.Where(x => x.EmployeeId == employeeId && x.LeaveTypeId== leaveTypeId &&x.LeaveId!= LeaveId && x.FromDate > toDate
            && x.Status != "Rejected").OrderBy(x => x.LeaveId).Take(1).ToList();
        }
        #endregion

        #region Get Employee Apply Leave Details By EmployeeId And FromDate
        public List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndForwardToDate(int employeeId, int LeaveTypeId, DateTime fromDate)
        {
            return dbContext.ApplyLeave.Where(x => x.EmployeeId == employeeId && x.LeaveTypeId == LeaveTypeId
            && x.Status != "Rejected" && x.FromDate >= fromDate).OrderBy(x => x.LeaveId).Take(1).ToList();
        }
        #endregion

        public List<ApplyLeaves> GetLeaveRequestByEmployeeId(int employeeId, int LeaveTypeId, DateTime fromDate, DateTime toDate, int leaveId, bool isEdit)
        {
            if (isEdit)
            {
                return dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, al => al.LeaveId, ald => ald.LeaveId, (al, ald) => new { al, ald }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == LeaveTypeId
                             && x.ald.AppliedLeaveStatus != false && x.al.LeaveId != leaveId
                             && x.al.FromDate.Date >= fromDate.Date && x.al.FromDate <= toDate.Date).Select(x => new ApplyLeaves
                             {
                                 LeaveId = x.al.LeaveId,
                                 EmployeeId = x.al.EmployeeId,
                                 LeaveTypeId = x.al.LeaveTypeId,
                                 FromDate = x.al.FromDate,
                                 ToDate = x.al.ToDate,
                                 NoOfDays = x.al.NoOfDays,
                                 Reason = x.al.Reason,
                                 Status = x.al.Status,
                                 Feedback = x.al.Feedback,
                                 LeaveRejectionReasonId = x.al.LeaveRejectionReasonId,
                                 IsActive = x.al.IsActive,
                                 CreatedOn = x.al.CreatedOn,
                                 CreatedBy = x.al.CreatedBy,
                                 ModifiedOn = x.al.ModifiedOn,
                                 ModifiedBy = x.al.ModifiedBy
                             }).Distinct().ToList();
            }
            else
            {
                return dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, al => al.LeaveId, ald => ald.LeaveId, (al, ald) => new { al, ald }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == LeaveTypeId
                            && x.ald.AppliedLeaveStatus != false
                            && x.al.FromDate.Date >= fromDate.Date && x.al.FromDate <= toDate.Date).Select(x => new ApplyLeaves
                            {
                                LeaveId = x.al.LeaveId,
                                EmployeeId = x.al.EmployeeId,
                                LeaveTypeId = x.al.LeaveTypeId,
                                FromDate = x.al.FromDate,
                                ToDate = x.al.ToDate,
                                NoOfDays = x.al.NoOfDays,
                                Reason = x.al.Reason,
                                Status = x.al.Status,
                                Feedback = x.al.Feedback,
                                LeaveRejectionReasonId = x.al.LeaveRejectionReasonId,
                                IsActive = x.al.IsActive,
                                CreatedOn = x.al.CreatedOn,
                                CreatedBy = x.al.CreatedBy,
                                ModifiedOn = x.al.ModifiedOn,
                                ModifiedBy = x.al.ModifiedBy
                            }).Distinct().ToList();
            }
        }
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
        //public List<ApplyLeavesView> GetAppliedResLeaveByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate, int? leaveTypeId)
        //{
        //    List<ApplyLeavesView> appliedLeave = dbContext.ApplyLeave
        //        .Join(dbContext.LeaveTypes, al => al.LeaveTypeId, lt => lt.LeaveTypeId, (al, lt) => new { al, lt }).Where(x => x.al.EmployeeId == employeeId
        //               && x.al.Status != "Cancelled" && x.al.Status != "Rejected" && x.al.LeaveTypeId == leaveTypeId
        //              && x.al.FromDate.Date >= fromDate && x.al.FromDate.Date <= toDate).Select(x => new ApplyLeavesView
        //              {
        //                  LeaveId = x.al.LeaveId,
        //                  EmployeeId = x.al.EmployeeId,
        //                  LeaveTypeId = x.al.LeaveTypeId,
        //                  FromDate = x.al.FromDate,
        //                  ToDate = x.al.ToDate,
        //                  NoOfDays = x.al.NoOfDays,
        //                  Reason = x.al.Reason,
        //                  Status = x.al.Status,
        //                  Feedback = x.al.Feedback,
        //                  LeaveRejectionReasonId = x.al.LeaveRejectionReasonId,
        //                  IsActive = x.al.IsActive,
        //                  LeaveType = x.lt.LeaveType,
        //                  AppliedLeaveDetails = dbContext.AppliedLeaveDetails.Where(rs => rs.LeaveId == x.al.LeaveId).Select(rs => new AppliedLeaveDetailsView { AppliedLeaveDetailsID = rs.AppliedLeaveDetailsID, Date = rs.Date, IsFullDay = rs.IsFullDay, IsFirstHalf = rs.IsFirstHalf, IsSecondHalf = rs.IsSecondHalf, LeaveId = rs.LeaveId, CompensatoryOffId = rs.CompensatoryOffId, CreatedBy = rs.CreatedBy, AppliedLeaveStatus = rs.AppliedLeaveStatus }).ToList(),
        //                  CreatedBy = x.al.CreatedBy,
        //                  IsGrantLeave = false,
        //                  CreatedOn = x.al.CreatedOn

        //              }).OrderByDescending(x => x.CreatedOn).ToList();
        //    if (appliedLeave == null)
        //    {
        //        appliedLeave = new List<ApplyLeavesView>();
        //    }
        //    return appliedLeave;
        //}

        public List<AppliedLeaveTypeDetails> GetAppliedLeaveByLeaveTypeAndDate(int employeeId, DateTime fromDate, DateTime toDate, int leaveTypeId)
        {
            List<AppliedLeaveTypeDetails> leaves = new List<AppliedLeaveTypeDetails>();
            leaves = (from leave in dbContext.ApplyLeave
                      join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                      where leave.EmployeeId == employeeId && ald.Date >= fromDate && ald.Date <= toDate && (leave.Status != "Cancelled" &&
                      (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId== leaveTypeId
                      select new AppliedLeaveTypeDetails
                      {
                          LeaveId = ald.LeaveId,
                          LeaveType = dbContext.LeaveTypes.Where(x => x.LeaveTypeId == leave.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault(),
                          Date = ald.Date,
                          IsFirstHalf = ald.IsFirstHalf,
                          IsFullDay = ald.IsFullDay,
                          IsSecondHalf = ald.IsSecondHalf,
                          AppliedLeaveStatus = ald.AppliedLeaveStatus,
                          CreatedOn = ald.CreatedOn,
                          LeaveTypeName = dbContext.AppConstants.Join(dbContext.LeaveTypes, al => al.AppConstantId, ald => ald.LeaveTypesId, (al, ald) => new { al, ald }).Where(x => x.ald.LeaveTypeId == leave.LeaveTypeId).Select(x => x.al.AppConstantValue).FirstOrDefault()
                      }).ToList();
            return leaves;
        }
        public List<AppliedLeaveTypeDetails> GetAppliedLeaveByLeaveTypeAndEmployee(int employeeId, int leaveTypeId)
        {
            List<AppliedLeaveTypeDetails> leaves = new List<AppliedLeaveTypeDetails>();
            leaves = (from leave in dbContext.ApplyLeave
                      join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                      where leave.EmployeeId == employeeId && (leave.Status != "Cancelled" &&
                      (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId
                      select new AppliedLeaveTypeDetails
                      {
                          LeaveId = ald.LeaveId,
                          LeaveType = dbContext.LeaveTypes.Where(x => x.LeaveTypeId == leave.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault(),
                          Date = ald.Date,
                          IsFirstHalf = ald.IsFirstHalf,
                          IsFullDay = ald.IsFullDay,
                          IsSecondHalf = ald.IsSecondHalf,
                          AppliedLeaveStatus = ald.AppliedLeaveStatus,
                          CreatedOn = ald.CreatedOn
                      }).ToList();
            return leaves;
        }
        public List<AppliedLeaveTypeDetails> GetAllAppliedLeaveByLeaveTypeAndDate(int employeeId, DateTime fromDate, DateTime toDate, int leaveTypeId)
        {
            List<AppliedLeaveTypeDetails> leaves = new List<AppliedLeaveTypeDetails>();
            leaves = (from leave in dbContext.ApplyLeave
                      join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                      where leave.EmployeeId == employeeId && ald.Date >= fromDate && ald.Date <= toDate && leave.LeaveTypeId == leaveTypeId
                      select new AppliedLeaveTypeDetails
                      {
                          LeaveId = ald.LeaveId,
                          LeaveType = dbContext.LeaveTypes.Where(x => x.LeaveTypeId == leave.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault(),
                          Date = ald.Date,
                          IsFirstHalf = ald.IsFirstHalf,
                          IsFullDay = ald.IsFullDay,
                          IsSecondHalf = ald.IsSecondHalf,
                          AppliedLeaveStatus = ald.AppliedLeaveStatus,
                          Status= leave.Status,
                          CreatedOn=ald.CreatedOn,
                          LeaveTypeName = dbContext.AppConstants.Join(dbContext.LeaveTypes, al => al.AppConstantId, ald => ald.LeaveTypesId, (al, ald) => new { al, ald }).Where(x => x.ald.LeaveTypeId == leave.LeaveTypeId).Select(x => x.al.AppConstantValue).FirstOrDefault()
                      }).ToList();
            return leaves;
        }
        #region Get Active Leaves by Previous Date
        public bool checkAppliedLeaveByDate(int employeeId, int leaveTypeId, DateTime leaveDate, bool isFullday, bool isFirstHalf, bool isSecondHalf)
        {
            if(isFullday)
            {
                return (from leave in dbContext.ApplyLeave
                        join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                        where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                        (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId== leaveTypeId
                        select new ApplyLeavesView
                        {
                            LeaveId = leave.LeaveId

                        }).Any();
            }
            else if(isFirstHalf)
            {
                return (from leave in dbContext.ApplyLeave
                        join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                        where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                        (ald.IsFirstHalf==true || ald.IsFullDay == true) &&
                        (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId
                        select new ApplyLeavesView
                        {
                            LeaveId = leave.LeaveId

                        }).Any();
            }
            else if(isSecondHalf)
            {
                return (from leave in dbContext.ApplyLeave
                        join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                        where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                        (ald.IsSecondHalf == true || ald.IsFullDay == true) &&
                        (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId
                        select new ApplyLeavesView
                        {
                            LeaveId = leave.LeaveId

                        }).Any();
            }
            return false;
            
        }
        #endregion
        #region Get Active Leaves by Previous Date
        public bool checkAppliedLeaveByLeaveId(int employeeId, int leaveTypeId, DateTime leaveDate, bool isFullday, bool isFirstHalf, bool isSecondHalf, int leaveId)
        {
            if (isFullday)
            {
                return (from leave in dbContext.ApplyLeave
                        join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                        where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                        (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId && leaveId != ald.LeaveId
                        select new ApplyLeavesView
                        {
                            LeaveId = leave.LeaveId

                        }).Any();
            }
            else if (isFirstHalf)
            {
                return (from leave in dbContext.ApplyLeave
                        join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                        where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                        (ald.IsFirstHalf == true || ald.IsFullDay == true) &&
                        (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId && leaveId != ald.LeaveId
                        select new ApplyLeavesView
                        {
                            LeaveId = leave.LeaveId

                        }).Any();
            }
            else if (isSecondHalf)
            {
                return (from leave in dbContext.ApplyLeave
                        join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                        where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                        (ald.IsSecondHalf == true || ald.IsFullDay == true) &&
                        (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId && leaveId != ald.LeaveId
                        select new ApplyLeavesView
                        {
                            LeaveId = leave.LeaveId

                        }).Any();
            }
            return false;

        }
        #endregion
        public LeaveExceptionView GetLeaveExceptionById(int leaveId)
        {
            LeaveExceptionView LeaveExceptionView = new();
            LeaveApplicable leaveException = dbContext.LeaveApplicable.Where(x => x.LeaveTypeId == leaveId && x.Type== "LeaveException").FirstOrDefault();
            if (leaveException != null)
            {
                LeaveExceptionView.LeaveApplicableId = leaveException.LeaveApplicableId;
                LeaveExceptionView.Gender_Male_Exception = leaveException.Gender_Male_Exception;
                LeaveExceptionView.Gender_Female_Exception = leaveException.Gender_Female_Exception;
                LeaveExceptionView.Gender_Others_Exception = leaveException.Gender_Others_Exception;
                LeaveExceptionView.MaritalStatus_Single_Exception = leaveException.MaritalStatus_Single_Exception;
                LeaveExceptionView.MaritalStatus_Married_Exception = leaveException.MaritalStatus_Married_Exception;
                LeaveExceptionView.LeaveTypeId = leaveException.LeaveTypeId;
            }
            return LeaveExceptionView;
        }
        #region Get Employee Applied Leave by EmployeeId By Grouping
        public List<ApplyLeavesView> GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestrictiond)
        {
            if(employeeLeaveandRestrictiond.FromDate != null && employeeLeaveandRestrictiond.ToDate != null)
            {
                return (from lt in dbContext.AppliedLeaveDetails
                        join al in dbContext.ApplyLeave on lt.LeaveId equals al.LeaveId
                        where al.EmployeeId == employeeLeaveandRestrictiond.EmployeeId && al.LeaveTypeId == employeeLeaveandRestrictiond.LeaveId
                        && lt.Date >= employeeLeaveandRestrictiond.FromDate && lt.Date <= employeeLeaveandRestrictiond.ToDate
                        select new ApplyLeavesView
                        {
                            LeaveId = al.LeaveId,
                            EmployeeId = al.EmployeeId,
                            LeaveTypeId = al.LeaveTypeId,
                            LeaveDate = lt.Date,
                            Reason = al.Reason,
                            Status = al.Status,
                            Feedback = al.Feedback,
                            LeaveRejectionReasonId = al.LeaveRejectionReasonId,
                            LeaveType = dbContext.LeaveTypes.Where(y => y.LeaveTypeId == al.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault(),
                            CreatedBy = al.CreatedBy,
                            CreatedOn = lt.CreatedOn,
                            ApproveRejectName = al.ApproveRejectName,
                            ApproveRejectDate = al.ApproveRejectOn,
                            isFirstHalf = lt.IsFirstHalf,
                            isSecondHalf = lt.IsSecondHalf,
                            isFullDay = lt.IsFullDay
                        }).OrderByDescending(x => x.CreatedOn).Skip(employeeLeaveandRestrictiond.NoOfRecord * employeeLeaveandRestrictiond.PageNumber).Take(employeeLeaveandRestrictiond.NoOfRecord).OrderByDescending(x => x.LeaveDate).ToList();

            }
            else
            {
                return (from lt in dbContext.AppliedLeaveDetails
                        join al in dbContext.ApplyLeave on lt.LeaveId equals al.LeaveId
                        where al.EmployeeId == employeeLeaveandRestrictiond.EmployeeId && al.LeaveTypeId == employeeLeaveandRestrictiond.LeaveId
                        select new ApplyLeavesView
                        {
                            LeaveId = al.LeaveId,
                            EmployeeId = al.EmployeeId,
                            LeaveTypeId = al.LeaveTypeId,
                            LeaveDate = lt.Date,
                            Reason = al.Reason,
                            Status = al.Status,
                            Feedback = al.Feedback,
                            LeaveRejectionReasonId = al.LeaveRejectionReasonId,
                            LeaveType = dbContext.LeaveTypes.Where(y => y.LeaveTypeId == al.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault(),
                            CreatedBy = al.CreatedBy,
                            CreatedOn = lt.CreatedOn,
                            ApproveRejectName = al.ApproveRejectName,
                            ApproveRejectDate = al.ApproveRejectOn,
                            isFirstHalf = lt.IsFirstHalf,
                            isSecondHalf = lt.IsSecondHalf,
                            isFullDay = lt.IsFullDay
                        }).OrderByDescending(x=>x.CreatedOn).Skip(employeeLeaveandRestrictiond.NoOfRecord * employeeLeaveandRestrictiond.PageNumber).Take(employeeLeaveandRestrictiond.NoOfRecord).OrderByDescending(x => x.LeaveDate).ToList();

            }

            //return dbContext.AppliedLeaveDetails
            //    .Join(dbContext.ApplyLeave, lt => lt.LeaveId, al => al.LeaveId, (lt, al) => new { lt, al }).Where(x => x.al.EmployeeId == employeeLeaveandRestrictiond.EmployeeId && x.al.LeaveTypeId == employeeLeaveandRestrictiond.LeaveId
            //          //&& x.al.Status != "Rejected"
            //          && x.al.LeaveId == employeeLeaveandRestrictiond.LeaveId).ToList().Select(x => new ApplyLeavesView
            //          {
            //              LeaveId = x.al.LeaveId,
            //              EmployeeId = x.al.EmployeeId,
            //              LeaveTypeId = x.al.LeaveTypeId,
            //              LeaveDate = x.lt.Date,
            //              Reason = x.al.Reason,
            //              Status = x.al.Status,
            //              Feedback = x.al.Feedback,
            //              LeaveRejectionReasonId = x.al.LeaveRejectionReasonId,
            //              IsActive = x.al.IsActive,
            //              LeaveType = dbContext.LeaveTypes.Where(y => y.LeaveTypeId == x.al.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault(),
            //              CreatedBy = x.al.CreatedBy,
            //              CreatedOn = x.lt.CreatedOn
            //          }).OrderByDescending(x => x.LeaveDate).ToList();
        }
        #endregion

        public IndividualLeaveList GetEmployeeLeavesByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeave = new();
            FilterData filterData = employeeLeaveandRestriction.FilterData;
            if (employeeLeaveandRestriction.PageNumber == 0)
            {
                var grantLeaveCount = (from re in dbContext.LeaveGrantRequestDetails
                                       join emp in dbContext.EmployeeGrantLeaveApproval on re.LeaveGrantDetailId equals emp.LeaveGrantDetailId
                                       where re.IsActive == true
                                       && emp.ApproverEmployeeId == (employeeLeaveandRestriction.managerId == 0 ? emp.ApproverEmployeeId : employeeLeaveandRestriction.managerId) && emp.Status != null
                                        && (employeeLeaveandRestriction.isFiltered == false || ((filterData.statusList == null || filterData.statusList.Contains(re.Status)) &&
              (filterData.LeaveTypeIdList == null || filterData.LeaveTypeIdList.Contains(re.LeaveTypeId))
              && (filterData.NoOfDays == null || ((filterData.NoOfDaysCondition == ">" ? (re.NumberOfDay <= filterData.NoOfDays) : filterData.NoOfDaysCondition == "<" ? (re.NumberOfDay >= filterData.NoOfDays) : (re.NumberOfDay >= filterData.NoOfDays && re.NumberOfDay <= filterData.NoOfDays)))
             ) && ((filterData.FromDate == null || filterData.ToDate == null) || (re.EffectiveFromDate >= filterData.FromDate && re.EffectiveFromDate <= filterData.ToDate))
             ))
                                       select new AppliedLeaveTypeDetails
                                       {
                                           Date = re.EffectiveFromDate ?? DateTime.UtcNow,
                                       }).Count();

                var leavCount = (from leave in dbContext.ApplyLeave
                                 join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                                 //where leave.EmployeeId == employeeId && leave.FromDate >= fromDate && leave.ToDate <= toDate && leave.Status != "Rejected"
                                 where (leave.EmployeeId == employeeLeaveandRestriction.EmployeeId
                                 && leave.ManagerId == (employeeLeaveandRestriction.managerId == 0 ? leave.ManagerId : employeeLeaveandRestriction.managerId)
                                  && (employeeLeaveandRestriction.isFiltered == false || ((filterData.statusList == null || filterData.statusList.Contains(leave.Status)) &&
                                (filterData.LeaveTypeIdList == null || filterData.LeaveTypeIdList.Contains(leave.LeaveTypeId))
                                && (filterData.NoOfDays == null || ((filterData.NoOfDaysCondition == ">" ? (leave.NoOfDays <= filterData.NoOfDays) : filterData.NoOfDaysCondition == "<" ? (leave.NoOfDays >= filterData.NoOfDays) : (leave.NoOfDays >= filterData.NoOfDays && leave.NoOfDays <= filterData.NoOfDays)))
                                ) && ((filterData.FromDate == null || filterData.ToDate == null) || (leave.FromDate >= filterData.FromDate && leave.ToDate <= filterData.ToDate))
                                )))
                                 //&& ald.Date >= fromDate && ald.Date <= toDate
                                 //&& (leave.Status != "Cancelled" &&
                                 //(leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true)))
                                 select new AppliedLeaveTypeDetails
                                 {
                                     Date = ald.Date,
                                 }).Count();
                individualLeave.TotalCount = grantLeaveCount + leavCount;
            }
            if (employeeLeaveandRestriction.isExport == true)
            {
                individualLeave.AppliedLeaveDetails = dbContext.ApplyLeave
                     .Where(rs => rs.EmployeeId == employeeLeaveandRestriction.EmployeeId &&
                     rs.ManagerId == (employeeLeaveandRestriction.managerId == 0 ? rs.ManagerId : employeeLeaveandRestriction.managerId)
                     && (employeeLeaveandRestriction.isFiltered == false || ((filterData.statusList == null || filterData.statusList.Contains(rs.Status)) &&
                         (filterData.LeaveTypeIdList == null || filterData.LeaveTypeIdList.Contains(rs.LeaveTypeId))
                         && (filterData.NoOfDays == null || ((filterData.NoOfDaysCondition == ">" ? (rs.NoOfDays <= filterData.NoOfDays) : filterData.NoOfDaysCondition == "<" ? (rs.NoOfDays >= filterData.NoOfDays) : (rs.NoOfDays >= filterData.NoOfDays && rs.NoOfDays <= filterData.NoOfDays)))
                        ) && ((filterData.FromDate == null || filterData.ToDate == null) || (rs.FromDate >= filterData.FromDate && rs.ToDate <= filterData.ToDate))
                        ))
                      ).Select(rs => new AppliedLeaveTypeDetails
                      {
                          FromDate = rs.FromDate,
                          ToDate = rs.ToDate,
                          LeaveId = rs.LeaveId,
                          Status = rs.Status,
                          reason = rs.Reason,
                          NoOfDays = rs.NoOfDays,
                          LeaveTypeId = rs.LeaveTypeId,
                          AppliedLeaveDetails = dbContext.AppliedLeaveDetails.Where(x => x.LeaveId == rs.LeaveId).Select(x =>
                                                                                                      new AppliedLeaveDetailsView
                                                                                                      {
                                                                                                          AppliedLeaveDetailsID = x.AppliedLeaveDetailsID,
                                                                                                          LeaveId = x.LeaveId,
                                                                                                          Date = x.Date,
                                                                                                          IsFullDay = x.IsFullDay,
                                                                                                          IsFirstHalf = x.IsFirstHalf,
                                                                                                          IsSecondHalf = x.IsSecondHalf,
                                                                                                          CompensatoryOffId = x.CompensatoryOffId,
                                                                                                          AppliedLeaveStatus = x.AppliedLeaveStatus,
                                                                                                          CreatedBy = x.CreatedBy,
                                                                                                      }).ToList(),
                          LeaveTypeName = dbContext.LeaveTypes.Where(x => x.LeaveTypeId == rs.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault()
                      }).ToList().OrderByDescending(x => x.CreatedOn).ToList();
                if (individualLeave.AppliedLeaveDetails == null)
                {
                    individualLeave.AppliedLeaveDetails = new List<AppliedLeaveTypeDetails>();
                }
                List<AppliedLeaveTypeDetails> teamGrantLeave = (from re in dbContext.LeaveGrantRequestDetails
                                                                join emp in dbContext.EmployeeGrantLeaveApproval on re.LeaveGrantDetailId equals emp.LeaveGrantDetailId
                                                                where re.IsActive == true
                                                                && emp.ApproverEmployeeId == (employeeLeaveandRestriction.managerId == 0 ? emp.ApproverEmployeeId : employeeLeaveandRestriction.managerId) && emp.Status != null
                                                                 && (employeeLeaveandRestriction.isFiltered == false || ((filterData.statusList == null || filterData.statusList.Contains(re.Status)) &&
                                       (filterData.LeaveTypeIdList == null || filterData.LeaveTypeIdList.Contains(re.LeaveTypeId))
                                       && (filterData.NoOfDays == null || ((filterData.NoOfDaysCondition == ">" ? (re.NumberOfDay <= filterData.NoOfDays) : filterData.NoOfDaysCondition == "<" ? (re.NumberOfDay >= filterData.NoOfDays) : (re.NumberOfDay >= filterData.NoOfDays && re.NumberOfDay <= filterData.NoOfDays)))
                                      ) && ((filterData.FromDate == null || filterData.ToDate == null) || (re.EffectiveFromDate >= filterData.FromDate && re.EffectiveFromDate <= filterData.ToDate))
                                      ))
                                                                select new AppliedLeaveTypeDetails
                                                                {
                                                                    EmployeeId = re.EmployeeID,
                                                                    LeaveTypeId = re.LeaveTypeId,
                                                                    FromDate = re.EffectiveFromDate ?? DateTime.UtcNow,
                                                                    ToDate = re.EffectiveFromDate.Value.AddDays(re.NumberOfDay == null ? 0 : (double)re.NumberOfDay),
                                                                    NoOfDays = re.NumberOfDay == null ? 0 : GetRoundOff((decimal)re.NumberOfDay),
                                                                    reason = re.Reason,
                                                                    Status = emp.Status,
                                                                    Feedback = emp.Comments,
                                                                    LeaveId = 0,
                                                                    LeaveGrantDetailId = re.LeaveGrantDetailId,
                                                                    LeaveType = dbContext.LeaveTypes.Where(rs => rs.LeaveTypeId == re.LeaveTypeId).Select(rs => rs.LeaveType).FirstOrDefault(),
                                                                    AppliedLeaveDetails = new List<AppliedLeaveDetailsView>(),
                                                                    IsGrantLeaveRequest = true,
                                                                    LevelId = emp.LevelId,
                                                                    ManagerId = emp.ApproverEmployeeId,
                                                                    CreatedOn = re.CreatedOn,
                                                                    ListOfDocuments = dbContext.LeaveGrantDocumentDetails.Where(x => x.LeaveGrantDetailId == re.LeaveGrantDetailId).Select(x => new SupportingDocuments
                                                                    {
                                                                        DocumentId = x.LeaveGrantDocumentDetailId,
                                                                        DocumentName = x.DocumentName,
                                                                        DocumentSize = 0,
                                                                        DocumentCategory = "",
                                                                        IsApproved = x.IsActive == null ? false : (bool)x.IsActive,
                                                                        DocumentType = x.DocumentType
                                                                    }).ToList()
                                                                }).OrderByDescending(x => x.CreatedOn).ToList();
                if (teamGrantLeave?.Count > 0)
                {
                    individualLeave.AppliedLeaveDetails = individualLeave.AppliedLeaveDetails?.Concat(teamGrantLeave).OrderByDescending(x => x.CreatedOn).ToList();
                }
            }
            else
            {
                List<AppliedLeaveTypeDetails> leaveList = dbContext.ApplyLeave
                     .Where(rs => rs.EmployeeId == employeeLeaveandRestriction.EmployeeId &&
                     rs.ManagerId == (employeeLeaveandRestriction.managerId == 0 ? rs.ManagerId : employeeLeaveandRestriction.managerId)
                     && (employeeLeaveandRestriction.isFiltered == false || ((filterData.statusList == null || filterData.statusList.Contains(rs.Status)) &&
                         (filterData.LeaveTypeIdList == null || filterData.LeaveTypeIdList.Contains(rs.LeaveTypeId))
                         && (filterData.NoOfDays == null || ((filterData.NoOfDaysCondition == ">" ? (rs.NoOfDays <= filterData.NoOfDays) : filterData.NoOfDaysCondition == "<" ? (rs.NoOfDays >= filterData.NoOfDays) : (rs.NoOfDays >= filterData.NoOfDays && rs.NoOfDays <= filterData.NoOfDays)))
                        ) && ((filterData.FromDate == null || filterData.ToDate == null) || (rs.FromDate >= filterData.FromDate && rs.ToDate <= filterData.ToDate))
                        ))
                      ).Select(rs => new AppliedLeaveTypeDetails
                      {
                          FromDate = rs.FromDate,
                          ToDate = rs.ToDate,
                          LeaveId = rs.LeaveId,
                          Status = rs.Status,
                          reason = rs.Reason,
                          NoOfDays = rs.NoOfDays,
                          LeaveTypeId = rs.LeaveTypeId,
                          AppliedLeaveDetails = dbContext.AppliedLeaveDetails.Where(x => x.LeaveId == rs.LeaveId).Select(x =>
                                                                                                       new AppliedLeaveDetailsView
                                                                                                       {
                                                                                                           AppliedLeaveDetailsID = x.AppliedLeaveDetailsID,
                                                                                                           LeaveId = x.LeaveId,
                                                                                                           Date = x.Date,
                                                                                                           IsFullDay = x.IsFullDay,
                                                                                                           IsFirstHalf = x.IsFirstHalf,
                                                                                                           IsSecondHalf = x.IsSecondHalf,
                                                                                                           CompensatoryOffId = x.CompensatoryOffId,
                                                                                                           AppliedLeaveStatus = x.AppliedLeaveStatus,
                                                                                                           CreatedBy = x.CreatedBy,
                                                                                                       }).ToList(),
                          LeaveTypeName = dbContext.LeaveTypes.Where(x => x.LeaveTypeId == rs.LeaveTypeId).Select(x => x.LeaveType).FirstOrDefault()
                      }).ToList().OrderByDescending(x => x.CreatedOn).Skip((employeeLeaveandRestriction.PageNumber) * (employeeLeaveandRestriction.NoOfRecord)).Take(employeeLeaveandRestriction.NoOfRecord).ToList();
                if (leaveList == null)
                {
                    leaveList = new List<AppliedLeaveTypeDetails>();
                }
                List<AppliedLeaveTypeDetails> teamGrantLeave = (from re in dbContext.LeaveGrantRequestDetails
                                                                join emp in dbContext.EmployeeGrantLeaveApproval on re.LeaveGrantDetailId equals emp.LeaveGrantDetailId
                                                                where re.IsActive == true
                                                                && emp.ApproverEmployeeId == (employeeLeaveandRestriction.managerId == 0 ? emp.ApproverEmployeeId : employeeLeaveandRestriction.managerId) && emp.Status != null
                                                                 && (employeeLeaveandRestriction.isFiltered == false || ((filterData.statusList == null || filterData.statusList.Contains(re.Status)) &&
                                       (filterData.LeaveTypeIdList == null || filterData.LeaveTypeIdList.Contains(re.LeaveTypeId))
                                       && (filterData.NoOfDays == null || ((filterData.NoOfDaysCondition == ">" ? (re.NumberOfDay <= filterData.NoOfDays) : filterData.NoOfDaysCondition == "<" ? (re.NumberOfDay >= filterData.NoOfDays) : (re.NumberOfDay >= filterData.NoOfDays && re.NumberOfDay <= filterData.NoOfDays)))
                                      ) && ((filterData.FromDate == null || filterData.ToDate == null) || (re.EffectiveFromDate >= filterData.FromDate && re.EffectiveFromDate <= filterData.ToDate))
                                      ))
                                                                select new AppliedLeaveTypeDetails
                                                                {
                                                                    EmployeeId = re.EmployeeID,
                                                                    LeaveTypeId = re.LeaveTypeId,
                                                                    FromDate = re.EffectiveFromDate ?? DateTime.UtcNow,
                                                                    ToDate = re.EffectiveFromDate.Value.AddDays(re.NumberOfDay == null ? 0 : (double)re.NumberOfDay),
                                                                    NoOfDays = re.NumberOfDay == null ? 0 : GetRoundOff((decimal)re.NumberOfDay),
                                                                    reason = re.Reason,
                                                                    Status = emp.Status,
                                                                    Feedback = emp.Comments,
                                                                    LeaveId = 0,
                                                                    LeaveGrantDetailId = re.LeaveGrantDetailId,
                                                                    LeaveTypeName = dbContext.LeaveTypes.Where(rs => rs.LeaveTypeId == re.LeaveTypeId).Select(rs => rs.LeaveType).FirstOrDefault(),
                                                                    IsGrantLeaveRequest = true,
                                                                    LevelId = emp.LevelId,
                                                                    ManagerId = emp.ApproverEmployeeId,
                                                                    CreatedOn = re.CreatedOn,
                                                                    ListOfDocuments = dbContext.LeaveGrantDocumentDetails.Where(x => x.LeaveGrantDetailId == re.LeaveGrantDetailId).Select(x => new SupportingDocuments
                                                                    {
                                                                        DocumentId = x.LeaveGrantDocumentDetailId,
                                                                        DocumentName = x.DocumentName,
                                                                        DocumentSize = 0,
                                                                        DocumentCategory = "",
                                                                        IsApproved = x.IsActive == null ? false : (bool)x.IsActive,
                                                                        DocumentType = x.DocumentType
                                                                    }).ToList()
                                                                }).OrderByDescending(x => x.CreatedOn).Skip(employeeLeaveandRestriction.NoOfRecord * (employeeLeaveandRestriction.PageNumber)).Take(employeeLeaveandRestriction.NoOfRecord).ToList();

                individualLeave.AppliedLeaveDetails = teamGrantLeave.Count > 0 ? leaveList?.Concat(teamGrantLeave).OrderByDescending(x => x.CreatedOn).ToList() : leaveList;
            }

            return individualLeave;
        }

        public List<EmployeeRequestCount> GetPendingLeaveCount(EmployeeListByDepartment employeeList)
        {
            List<int> employeeIdList = employeeList?.EmployeeId;
            List<int> grantleaveList = dbContext.LeaveGrantRequestDetails.Join(dbContext.EmployeeGrantLeaveApproval, leave => leave.LeaveGrantDetailId, ald => ald.LeaveGrantDetailId, (leave, ald) => new { leave, ald })
                       .Where(rs => employeeIdList.Contains(rs.leave.EmployeeID) && rs.leave.Status == "Pending" && (employeeList.managerId == 0 || ( rs.ald.ApproverEmployeeId == employeeList.managerId && rs.ald.Status == "Pending"))).Select(x => x.leave.EmployeeID).ToList();
            if (grantleaveList == null)
            {
                grantleaveList = new List<int>();
            }
            List<int> leaveList = dbContext.ApplyLeave.Join(dbContext.AppliedLeaveDetails, leave => leave.LeaveId, ald => ald.LeaveId, (leave, ald) => new { leave, ald })
                    .Where(rs => employeeIdList.Contains(rs.leave.EmployeeId) && rs.leave.Status == "Pending" && (employeeList.managerId == 0 || rs.leave.ManagerId == employeeList.managerId)).Select(x => x.leave.EmployeeId).ToList();
            if (leaveList == null)
            {
                leaveList = new List<int>();
            }
            List<EmployeeRequestCount> employeeRequests = leaveList.Concat(grantleaveList).GroupBy(x => x).Select(x => new EmployeeRequestCount
            {
                EmployeeId = x.Key,
                RequestCount = x.Count()
            }).ToList(); ;

            return employeeRequests;

        }
    }
}
