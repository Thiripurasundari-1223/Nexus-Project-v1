using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface IAppliedLeaveDetailsRepository : IBaseRepository<AppliedLeaveDetails>
    {
        List<AppliedLeaveDetails> GetByID(int leaveId);
        public bool ApplyLeaveDatesDupilication(ApplyLeavesView ApplyLeavesView);
        public AppliedLeaveDetails GetAppliedLeaveByID(int appliedLeaveDetailsID);
        List<EmployeeLeavesForTimeSheetView> GetEmployeeLeavesForTimesheet(int employeeID, DateTime fromDate, DateTime toDate);
        //AppliedLeaveDetails GetConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date);
        bool GetPreviousConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date,int leaveId);
        bool GetNextConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date,int leaveId);
        bool CheckNextConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date, int leaveId);
        bool CheckPreviousConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date, int leaveId);
        List<AppliedLeaveDetails> GetConsecutiveAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date, int leaveId);
        List<AppliedLeaveDetails> GetAppliedLeaveByDate(DateTime date);
        bool checkAppliedLeaveByDate(int employeeId, int leaveTypeId, DateTime leaveDate, bool isFullday, bool isFirstHalf, bool isSecondHalf, int leaveId);

    }
    public class AppliedLeaveDetailsRepository : BaseRepository<AppliedLeaveDetails>, IAppliedLeaveDetailsRepository
    {
        private readonly LeaveDBContext dbContext;
        public AppliedLeaveDetailsRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<AppliedLeaveDetails> GetByID(int leaveId)
        {
            if (leaveId > 0)
            {
                return dbContext.AppliedLeaveDetails.Where(x => x.LeaveId == leaveId).ToList();
            }
            return null;
        }
        public bool ApplyLeaveDatesDupilication(ApplyLeavesView ApplyLeavesView)
        {


            if (ApplyLeavesView?.AppliedLeaveDetails?.Count > 0)
            {
                if (ApplyLeavesView?.LeaveId == 0)
                {
                    foreach (AppliedLeaveDetailsView appliedLeaveDetailsView in ApplyLeavesView?.AppliedLeaveDetails)
                    {
                        bool isAppliedLeave = false;
                        if (appliedLeaveDetailsView.IsFullDay)
                        {
                            isAppliedLeave = dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.ald.Date == appliedLeaveDetailsView.Date && x.al.EmployeeId == ApplyLeavesView.EmployeeId && x.ald.AppliedLeaveStatus != false
                            && (x.ald.IsFirstHalf == true || x.ald.IsFullDay == true || x.ald.IsSecondHalf))
                        .Select(x => new AppliedLeaveDetails
                        {
                            AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,

                        }).Any();
                        }
                        if (appliedLeaveDetailsView.IsFirstHalf)
                        {
                            isAppliedLeave = dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.ald.Date == appliedLeaveDetailsView.Date 
                            && x.al.EmployeeId == ApplyLeavesView.EmployeeId && x.ald.AppliedLeaveStatus != false && 
                            (x.ald.IsFirstHalf==true || x.ald.IsFullDay==true))
                        .Select(x => new AppliedLeaveDetails
                        {
                            AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,

                        }).Any();
                        }
                        if (appliedLeaveDetailsView.IsSecondHalf)
                        {
                            isAppliedLeave = dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.ald.Date == appliedLeaveDetailsView.Date && x.al.EmployeeId == ApplyLeavesView.EmployeeId && x.ald.AppliedLeaveStatus != false
                            && (x.ald.IsSecondHalf == true || x.ald.IsFullDay == true))
                        .Select(x => new AppliedLeaveDetails
                        {
                            AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,

                        }).Any();
                        }
                        if(isAppliedLeave)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    foreach (AppliedLeaveDetailsView appliedLeaveDetailsView in ApplyLeavesView?.AppliedLeaveDetails)
                    {
                        bool isAppliedLeave = false;
                        if (appliedLeaveDetailsView.IsFullDay)
                        {
                            isAppliedLeave = dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.ald.Date == appliedLeaveDetailsView.Date && x.al.EmployeeId == ApplyLeavesView.EmployeeId && x.ald.AppliedLeaveStatus != false
                           && (x.ald.IsFirstHalf == true || x.ald.IsFullDay == true || x.ald.IsSecondHalf) && x.al.LeaveId != ApplyLeavesView.LeaveId)
                        .Select(x => new AppliedLeaveDetails
                        {
                            AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,

                        }).Any();
                        }
                        if (appliedLeaveDetailsView.IsFirstHalf)
                        {
                            isAppliedLeave = dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.ald.Date == appliedLeaveDetailsView.Date
                            && x.al.EmployeeId == ApplyLeavesView.EmployeeId && x.ald.AppliedLeaveStatus != false &&
                            (x.ald.IsFirstHalf == true || x.ald.IsFullDay == true) && x.al.LeaveId != ApplyLeavesView.LeaveId)
                        .Select(x => new AppliedLeaveDetails
                        {
                            AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,

                        }).Any();
                        }
                        if (appliedLeaveDetailsView.IsSecondHalf)
                        {
                            isAppliedLeave = dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.ald.Date == appliedLeaveDetailsView.Date && x.al.EmployeeId == ApplyLeavesView.EmployeeId && x.ald.AppliedLeaveStatus != false
                            && (x.ald.IsSecondHalf == true || x.ald.IsFullDay == true) && x.al.LeaveId != ApplyLeavesView.LeaveId )
                        .Select(x => new AppliedLeaveDetails
                        {
                            AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,

                        }).Any();
                        }
                        if (isAppliedLeave)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public AppliedLeaveDetails GetAppliedLeaveByID(int appliedLeaveDetailsID)
        {
            return dbContext.AppliedLeaveDetails.Where(x => x.AppliedLeaveDetailsID == appliedLeaveDetailsID).FirstOrDefault();
        }

        #region Get Employee Leaves For Timesheet
        public List<EmployeeLeavesForTimeSheetView> GetEmployeeLeavesForTimesheet(int employeeID, DateTime fromDate, DateTime toDate)
        {

            List<EmployeeLeavesForTimeSheetView> leaves = new List<EmployeeLeavesForTimeSheetView>();
            leaves = (from AppLeaveDet in dbContext.AppliedLeaveDetails
                      join Leave in dbContext.ApplyLeave on AppLeaveDet.LeaveId equals Leave.LeaveId
                      join Types in dbContext.LeaveTypes on Leave.LeaveTypeId equals Types.LeaveTypeId
                      where Leave.EmployeeId == employeeID && AppLeaveDet.Date >= fromDate && AppLeaveDet.Date <= toDate && (AppLeaveDet.AppliedLeaveStatus == null || AppLeaveDet.AppliedLeaveStatus == true)
                      select new EmployeeLeavesForTimeSheetView
                      {
                          Date = AppLeaveDet.Date,
                          IsFullDay = AppLeaveDet.IsFullDay,
                          IsFirstHalf = AppLeaveDet.IsFirstHalf,
                          IsSecondHalf = AppLeaveDet.IsSecondHalf,
                          AppliedLeaveStatus = AppLeaveDet.AppliedLeaveStatus,
                          IsAllowTimesheet = Types.AllowTimesheet
                      }).ToList();
            return leaves;
        }
        #endregion
        //public AppliedLeaveDetails GetConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date)
        //{
        //    return dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == leaveTypeId && x.ald.AppliedLeaveStatus != false && x.ald.Date == date )
        //        .Select(x => new AppliedLeaveDetails
        //        {
        //            AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,
        //        }).FirstOrDefault();
        //}
        public bool CheckPreviousConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date,int leaveId)
        {
            return dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == leaveTypeId && x.ald.AppliedLeaveStatus != false && x.ald.Date == date
            && x.ald.LeaveId !=leaveId)
                .Select(x => new AppliedLeaveDetails
                {
                    AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,
                    IsFullDay = x.ald.IsFullDay,
                    IsFirstHalf = x.ald.IsFirstHalf,
                    IsSecondHalf = x.ald.IsSecondHalf
                }).Any();
            //if(data==null || data?.Count == 0)
            //{
            //    return true;
            //}
            //else if(data?.Count>1)
            //{
            //    return false;
            //}
            //else if(data?.Count==1)
            //{
            //    if(data[0].IsFirstHalf==true)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
        public bool GetPreviousConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date,int leaveId)
        {
            return dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == leaveTypeId && x.ald.AppliedLeaveStatus != false && x.ald.Date == date &&
             x.ald.LeaveId !=leaveId)
                .Select(x => new AppliedLeaveDetails
                {
                    AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,
                    IsFullDay = x.ald.IsFullDay,
                    IsFirstHalf = x.ald.IsFirstHalf,
                    IsSecondHalf = x.ald.IsSecondHalf
                }).Any();
            //if(data?.Count==2)
            //{
            //    return true;
            //}
            //else if(data?.Count==1)
            //{
            //    if(data[0].IsFullDay==true || data[0].IsSecondHalf==true)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
        public bool CheckNextConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date, int leaveId)
        {
            return dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == leaveTypeId && x.ald.AppliedLeaveStatus != false && x.ald.Date == date
            && x.ald.LeaveId != leaveId)
                .Select(x => new AppliedLeaveDetails
                {
                    AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,
                    IsFullDay = x.ald.IsFullDay,
                    IsFirstHalf = x.ald.IsFirstHalf,
                    IsSecondHalf = x.ald.IsSecondHalf
                }).Any();
            //if (data == null || data?.Count==0)
            //{
            //    return true;
            //}
            //else if (data?.Count > 1)
            //{
            //    return false;
            //}
            //else if (data?.Count == 1)
            //{
            //    if (data[0].IsSecondHalf == true)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
        public bool GetNextConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date,int leaveId)
        {
            return dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x => x.al.EmployeeId == employeeId && x.al.LeaveTypeId == leaveTypeId && x.ald.AppliedLeaveStatus != false && x.ald.Date == date 
             && x.ald.LeaveId !=leaveId)
                .Select(x => new AppliedLeaveDetails
                {
                    AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,
                    IsFullDay= x.ald.IsFullDay,
                    IsFirstHalf=x.ald.IsFirstHalf,
                    IsSecondHalf=x.ald.IsSecondHalf
                }).Any();
            //if (data?.Count == 2)
            //{
            //    return true;
            //}
            //else if (data?.Count == 1)
            //{
            //    if (data[0].IsFullDay == true || data[0].IsFirstHalf == true)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
        public List<AppliedLeaveDetails> GetConsecutiveAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime date, int leaveId)
        {
            if(leaveId>0)
            {
                return dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x =>
            x.al.EmployeeId == employeeId && x.al.LeaveTypeId == leaveTypeId
            && x.ald.AppliedLeaveStatus != false && x.ald.Date == date
            &&  x.al.LeaveId != leaveId )
                .Select(x => new AppliedLeaveDetails
                {
                    AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,
                    IsFullDay = x.ald.IsFullDay,
                    IsFirstHalf = x.ald.IsFirstHalf,
                    IsSecondHalf = x.ald.IsSecondHalf
                }).ToList();
            }
            else
            {
                return dbContext.AppliedLeaveDetails.Join(dbContext.ApplyLeave, ald => ald.LeaveId, al => al.LeaveId, (ald, al) => new { ald, al }).Where(x =>
            x.al.EmployeeId == employeeId && x.al.LeaveTypeId == leaveTypeId
            && x.ald.AppliedLeaveStatus != false && x.ald.Date == date
            )
                .Select(x => new AppliedLeaveDetails
                {
                    AppliedLeaveDetailsID = x.ald.AppliedLeaveDetailsID,
                    IsFullDay = x.ald.IsFullDay,
                    IsFirstHalf = x.ald.IsFirstHalf,
                    IsSecondHalf = x.ald.IsSecondHalf
                }).ToList();
            }
            
        }
        public List<AppliedLeaveDetails> GetAppliedLeaveByDate(DateTime date)
        {
            return (from leave in dbContext.ApplyLeave
                    join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                    where ald.Date.Date == date.Date && (leave.Status != "Cancelled" &&
                    (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true)))
                    select ald).ToList();
        }
        #region Get Active Leaves by Previous Date
        public bool checkAppliedLeaveByDate(int employeeId, int leaveTypeId, DateTime leaveDate, bool isFullday, bool isFirstHalf, bool isSecondHalf, int leaveId)
        {
            if (isFullday)
            {
                if(leaveId>0)
                {
                    return (from leave in dbContext.ApplyLeave
                            join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                            where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                            (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId &&
                            leave.LeaveId != leaveId 
                            select new ApplyLeavesView
                            {
                                LeaveId = leave.LeaveId

                            }).Any();
                }
                else
                {
                    return (from leave in dbContext.ApplyLeave
                            join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                            where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                            (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId
                            
                            select new ApplyLeavesView
                            {
                                LeaveId = leave.LeaveId

                            }).Any();
                }
                
            }
            else if (isFirstHalf)
            {
                if (leaveId > 0)
                {
                    return (from leave in dbContext.ApplyLeave
                            join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                            where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                            (ald.IsFirstHalf == true || ald.IsFullDay == true) &&
                            (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId &&
                            leave.LeaveId != leaveId
                            select new ApplyLeavesView
                            {
                                LeaveId = leave.LeaveId

                            }).Any();
                }
                else
                {
                    return (from leave in dbContext.ApplyLeave
                            join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                            where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                            (ald.IsFirstHalf == true || ald.IsFullDay == true) &&
                            (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId 
                            
                            select new ApplyLeavesView
                            {
                                LeaveId = leave.LeaveId

                            }).Any();
                }
            }
            else if (isSecondHalf)
            {
                if (leaveId > 0)
                {
                    return (from leave in dbContext.ApplyLeave
                            join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                            where leave.EmployeeId == employeeId && ald.Date == leaveDate && (leave.Status != "Cancelled" &&
                            (ald.IsSecondHalf == true || ald.IsFullDay == true) &&
                            (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId &&
                            leave.LeaveId != leaveId
                            select new ApplyLeavesView
                            {
                                LeaveId = leave.LeaveId

                            }).Any();
                }
                else
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
            }
            return false;

        }
        #endregion

    }
}
