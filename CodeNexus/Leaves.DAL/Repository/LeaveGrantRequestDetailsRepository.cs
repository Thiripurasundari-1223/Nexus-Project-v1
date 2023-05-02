using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaves.DAL.Repository
{
    public interface ILeaveGrantRequestDetailsRepository:IBaseRepository<LeaveGrantRequestDetails>
    {
        LeaveGrantRequestDetails GetByID(int LeaveGrantDetailId);
        //List<LeaveGrantRequestDetails> GetByLeaveTypeAndEmployeeID(int leaveTypeId,int employeeId);
        List<LeaveGrantRequestAndDocumentView> GetGrantLeaveByEmployeeId(int employeeId);
        List<LeaveGrantRequestDetails> GetGrantLeaveListByTypeAndEmployeeID(int leaveTypeId, int employeeId, DateTime fromDate, DateTime toDate, int leaveGrantDetailId, bool isEdit);
        List<LeaveGrantRequestDetails> GetBackwardLeaveGrantGapByTypeIdAndEmployeeID(int leaveTypeId, int employeeId, int leaveGrantDetailId, DateTime fromDate, bool isEdit);
        List<LeaveGrantRequestDetails> GetForwardLeaveGrantGapByTypeIdAndEmployeeID(int leaveTypeId, int employeeId, int leaveGrantDetailId, DateTime fromDate, bool isEdit);
        List<LeaveGrantRequestAndDocumentView> GetGrantLeaveByEmployeeIdAndLeaveGrantId(int employeeId, int leaveGrantDetailId);
        bool ApplyLeaveGrantDatesDupilication(LeaveGrantRequestAndDocumentView leaveGrantRequestView);
        //List<LeaveGrantRequestDetails> GetGrantLeaveRequestByIsActive();
        List<LeaveGrantRequestDetails> GetByLeaveTypeID(int leaveTypeId);
        List<LeaveGrantRequestDetails> GetGrantLeaveRequestByDate(int employeeId, int leaveTypeId, DateTime? fromDate, DateTime? toDate, List<AppliedLeaveDetailsView> appliedLeaveDetails);
        List<LeaveGrantRequestDetails> GetGrantLeaveRequestBalance(int employeeId, int leaveTypeId, DateTime? fromDate, DateTime? toDate, List<AppliedLeaveDetailsView> appliedLeaveDetails);
        List<LeaveGrantRequestDetails> GetGrantLeaveListByEmployeeID(int leaveTypeId, int employeeId, DateTime fromDate, DateTime toDate);
        decimal GetAppliedGrantLeaveBalance(int leaveTypeId, int employeeId, DateTime date);
        List<LeaveGrantRequestDetails> GetGrantRequestDetailsByLeaveTypeId(int leaveTypeId, int employeeId, DateTime date,int grantRequestId);
        List<LeaveGrantRequestDetails> CheckGrantRequestExpiry(int leaveTypeId, int employeeId, DateTime date);
        List<LeaveGrantRequestDetails> GetGrantRequestCardBalance(int leaveTypeId, int employeeId, DateTime fromDate, DateTime toDate);
        decimal GetGrantRequestCardBalanceByDate(int leaveTypeId, int employeeId, DateTime date);
        List<int> GetGrantLeaveByManagerId(int managerId);
    }
    public class LeaveGrantRequestDetailsRepository : BaseRepository<LeaveGrantRequestDetails>, ILeaveGrantRequestDetailsRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveGrantRequestDetailsRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        //public List<LeaveGrantRequestDetails> GetByLeaveTypeAndEmployeeID(int leaveTypeId, int employeeId)
        //{
        //    if (leaveTypeId > 0)
        //    {
        //        return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId).OrderByDescending(x => x.LeaveGrantDetailId).Take(1).ToList();
        //    }
        //    return null;
        //}
        public LeaveGrantRequestDetails GetByID(int LeaveGrantDetailId)
        {
            return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveGrantDetailId == LeaveGrantDetailId).FirstOrDefault();
        }
        public List<LeaveGrantRequestAndDocumentView> GetGrantLeaveByEmployeeId(int employeeId)
        {


            List<LeaveGrantRequestDetails> leaveGrantRequestList = new();
            leaveGrantRequestList = dbContext.LeaveGrantRequestDetails.Where(x => x.EmployeeID == employeeId).ToList();
            List<LeaveGrantRequestAndDocumentView> leaveGrantRequestAndDocumentLists = new List<LeaveGrantRequestAndDocumentView>();
            foreach (LeaveGrantRequestDetails item in leaveGrantRequestList)
            {

                leaveGrantRequestAndDocumentLists.Add(new LeaveGrantRequestAndDocumentView
                {
                    leaveGrantRequestDetail = dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveGrantDetailId == item.LeaveGrantDetailId).Select(x => new LeaveGrantRequestDetailsView
                    {
                        LeaveGrantDetailId = x.LeaveGrantDetailId,
                        LeaveTypeId = x.LeaveTypeId,
                        EmployeeID = x.EmployeeID,
                        NumberOfDay = x.NumberOfDay,
                        Reason = x.Reason,
                        EffectiveFromDate = x.EffectiveFromDate,
                        EffectiveToDate = x.EffectiveToDate,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy
                    }).FirstOrDefault(),

                    leaveGrantDocument = dbContext.LeaveGrantDocumentDetails.Where(x => x.LeaveGrantDetailId == item.LeaveGrantDetailId).Select(x => new LeaveGrantDocument
                    {
                        LeaveGrantDocumentDetailId = x.LeaveGrantDocumentDetailId,
                        DocumentName = x.DocumentName,
                        DocumentPath = x.DocumentPath,
                        DocumentType = x.DocumentType,
                        IsActive = x.IsActive
                    }).ToList()
                });

            }
            return leaveGrantRequestAndDocumentLists;
        }

        public List<LeaveGrantRequestDetails> GetGrantLeaveListByTypeAndEmployeeID(int leaveTypeId, int employeeId, DateTime fromDate, DateTime toDate, int leaveGrantDetailId, bool isEdit)
        {
            if (isEdit)
            {
                return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId && x.Status != "Rejected" &&  x.Status != "Cancelled" && x.LeaveGrantDetailId != leaveGrantDetailId && x.EffectiveFromDate >= fromDate.Date && x.EffectiveFromDate <= toDate.Date).ToList();
            }
            else
            {
                return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId && x.Status != "Rejected" && x.Status != "Cancelled" && x.EffectiveFromDate >= fromDate.Date && x.EffectiveFromDate <= toDate.Date).ToList();
            }
        }
        public List<LeaveGrantRequestDetails> GetBackwardLeaveGrantGapByTypeIdAndEmployeeID(int leaveTypeId, int employeeId, int leaveGrantDetailId, DateTime fromDate, bool isEdit)
        {
            if (isEdit)
            {
                return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId && x.Status != "Rejected" && x.Status != "Cancelled" && x.LeaveGrantDetailId != leaveGrantDetailId && x.EffectiveFromDate <= fromDate).OrderByDescending(x => x.EffectiveFromDate).Take(1).ToList();
            }
            else
            {
                return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId && x.Status != "Rejected" && x.Status != "Cancelled" && x.EffectiveFromDate <= fromDate).OrderByDescending(x => x.EffectiveFromDate).Take(1).ToList();
            }
        }
        public List<LeaveGrantRequestDetails> GetForwardLeaveGrantGapByTypeIdAndEmployeeID(int leaveTypeId, int employeeId, int leaveGrantDetailId, DateTime fromDate, bool isEdit)
        {
            if (isEdit)
            {
                return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId && x.Status != "Rejected" && x.Status != "Cancelled" && x.LeaveGrantDetailId != leaveGrantDetailId && x.EffectiveFromDate >= fromDate).OrderBy(x => x.EffectiveFromDate).Take(1).ToList();
            }
            else
            {
                return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId && x.Status != "Rejected" && x.Status != "Cancelled" && x.EffectiveFromDate > fromDate).OrderBy(x => x.EffectiveFromDate).Take(1).ToList();
            }
        }
        public List<LeaveGrantRequestAndDocumentView> GetGrantLeaveByEmployeeIdAndLeaveGrantId(int employeeId, int leaveGrantDetailId)
        {


            List<LeaveGrantRequestDetails> leaveGrantRequestList = new();
            leaveGrantRequestList = dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveGrantDetailId == leaveGrantDetailId && x.EmployeeID == employeeId).ToList();
            List<LeaveGrantRequestAndDocumentView> leaveGrantRequestAndDocumentLists = new List<LeaveGrantRequestAndDocumentView>();
            foreach (LeaveGrantRequestDetails item in leaveGrantRequestList)
            {

                leaveGrantRequestAndDocumentLists.Add(new LeaveGrantRequestAndDocumentView
                {
                    leaveGrantRequestDetail = dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveGrantDetailId == item.LeaveGrantDetailId).Select(x => new LeaveGrantRequestDetailsView
                    {
                        LeaveGrantDetailId = x.LeaveGrantDetailId,
                        LeaveTypeId = x.LeaveTypeId,
                        EmployeeID = x.EmployeeID,
                        NumberOfDay = x.NumberOfDay,
                        Reason = x.Reason,
                        EffectiveFromDate = x.EffectiveFromDate,
                        EffectiveToDate = x.EffectiveToDate,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy
                    }).FirstOrDefault(),

                    leaveGrantDocument = dbContext.LeaveGrantDocumentDetails.Where(x => x.LeaveGrantDetailId == item.LeaveGrantDetailId).Select(x => new LeaveGrantDocument
                    {
                        LeaveGrantDocumentDetailId = x.LeaveGrantDocumentDetailId,
                        DocumentName = x.DocumentName,
                        DocumentPath = x.DocumentPath,
                        DocumentType = x.DocumentType,
                        IsActive = x.IsActive
                    }).ToList()
                });

            }
            return leaveGrantRequestAndDocumentLists;
        }
        public bool ApplyLeaveGrantDatesDupilication(LeaveGrantRequestAndDocumentView leaveGrantRequestView)
        {

            bool isCount = false;
            DateTime fromdate = new DateTime((int)leaveGrantRequestView.leaveGrantRequestDetail.EffectiveFromDate?.Year,(int) leaveGrantRequestView.leaveGrantRequestDetail.EffectiveFromDate?.Month, (int)leaveGrantRequestView.leaveGrantRequestDetail.EffectiveFromDate?.Day);
            if (leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId==0)
            {
                List<LeaveGrantRequestDetails> leaveGrantRequestList = new List<LeaveGrantRequestDetails>();
                leaveGrantRequestList = dbContext.LeaveGrantRequestDetails.Where(x => x.EmployeeID == leaveGrantRequestView.leaveGrantRequestDetail.EmployeeID && x.Status != "Rejected" && x.Status != "Cancelled" && x.EffectiveFromDate == fromdate).ToList();
                if (leaveGrantRequestList != null && leaveGrantRequestList?.Count > 0)
                {
                    isCount = true;
                }
            }
            else
            {
                List<LeaveGrantRequestDetails> leaveGrantRequestList = new List<LeaveGrantRequestDetails>();
                leaveGrantRequestList = dbContext.LeaveGrantRequestDetails.Where(x => x.EmployeeID == leaveGrantRequestView.leaveGrantRequestDetail.EmployeeID && x.LeaveGrantDetailId== leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId && x.Status != "Rejected" && x.Status != "Cancelled").ToList();
                List<LeaveGrantRequestDetails> appliedleaveGrantRequestList = new();
                if (leaveGrantRequestList.Count>0)
                {
                    appliedleaveGrantRequestList = leaveGrantRequestList.Where(rs => rs.EffectiveFromDate == fromdate).ToList();
                }
                if(appliedleaveGrantRequestList.Count==0)
                {
                    List<LeaveGrantRequestDetails> leaveGrantRequestDetailList = new List<LeaveGrantRequestDetails>();
                    leaveGrantRequestDetailList = dbContext.LeaveGrantRequestDetails.Where(x => x.EmployeeID == leaveGrantRequestView.leaveGrantRequestDetail.EmployeeID && x.Status != "Rejected" && x.Status != "Cancelled" && x.EffectiveFromDate == fromdate).ToList();
                    if (leaveGrantRequestDetailList != null && leaveGrantRequestDetailList?.Count > 0)
                    {
                        isCount = true;
                    }
                }
            }
            return isCount;
        }
        
        //public List<LeaveGrantRequestDetails> GetGrantLeaveRequestByIsActive()
        //{
        //    return dbContext.LeaveGrantRequestDetails.Where(x => x.IsActive == true && x.EffectiveToDate !=null && x.BalanceDay>0).ToList();
        //}
        public List<LeaveGrantRequestDetails> GetByLeaveTypeID(int leaveTypeId)
        {
            return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
        }
        public List<LeaveGrantRequestDetails> GetGrantLeaveRequestByDate(int employeeId, int leaveTypeId,DateTime? fromDate, DateTime? toDate, List<AppliedLeaveDetailsView> appliedLeaveDetails)
        {
            List<LeaveGrantRequestDetails> leaveDetails = new List<LeaveGrantRequestDetails>();
            foreach(var item in appliedLeaveDetails)
            {
                List<LeaveGrantRequestDetails> requestList = dbContext.LeaveGrantRequestDetails.Where(x => x.Status == "Approved" && x.EmployeeID == employeeId && x.LeaveTypeId == leaveTypeId && x.EffectiveFromDate <= item.Date
              && (x.EffectiveToDate >= item.Date || x.EffectiveToDate == null) && x.NumberOfDay != x.BalanceDay).OrderBy(x => x.EffectiveFromDate).ToList();
                foreach(var data in requestList)
                {
                    if(!leaveDetails.Any(x=>x.LeaveGrantDetailId==data.LeaveGrantDetailId))
                    {
                        leaveDetails.Add(data);
                    }
                }
            }
            return leaveDetails.OrderByDescending(x=>x.EffectiveFromDate).ToList();
            //return dbContext.LeaveGrantRequestDetails.Where(x => x.Status=="Approved" && x.EmployeeID==employeeId && x.LeaveTypeId==leaveTypeId && x.EffectiveFromDate <= fromDate
            //&& (x.EffectiveToDate >= toDate || x.EffectiveToDate==null)).OrderBy(x=>x.EffectiveFromDate).ToList();
        }
        public List<LeaveGrantRequestDetails> GetGrantLeaveRequestBalance(int employeeId, int leaveTypeId, DateTime? fromDate, DateTime? toDate, List<AppliedLeaveDetailsView> appliedLeaveDetails)
        {
            List<LeaveGrantRequestDetails> leaveDetails = new List<LeaveGrantRequestDetails>();
            foreach (var item in appliedLeaveDetails)
            {
                List<LeaveGrantRequestDetails> requestList = dbContext.LeaveGrantRequestDetails.Where(x => x.Status == "Approved" && x.EmployeeID == employeeId && x.LeaveTypeId == leaveTypeId && x.EffectiveFromDate <= item.Date
              && (x.EffectiveToDate >= item.Date || x.EffectiveToDate == null) && x.BalanceDay>0).OrderBy(x => x.EffectiveFromDate).ToList();
                foreach (var data in requestList)
                {
                    if (!leaveDetails.Any(x => x.LeaveGrantDetailId == data.LeaveGrantDetailId))
                    {
                        leaveDetails.Add(data);
                    }
                }
            }
            return leaveDetails.OrderBy(x => x.EffectiveFromDate).ToList();
        }
        public List<LeaveGrantRequestDetails> GetGrantLeaveListByEmployeeID(int leaveTypeId, int employeeId, DateTime fromDate, DateTime toDate)
        {
            return dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId && x.Status != "Rejected" && x.Status != "Cancelled" && x.EffectiveFromDate >= fromDate.Date && x.EffectiveFromDate <= toDate.Date).ToList();
        }
        public decimal GetAppliedGrantLeaveBalance(int leaveTypeId, int employeeId, DateTime date)
        {
            decimal BalanceDay = 0;
            LeaveGrantRequestDetails previousRequest= dbContext.LeaveGrantRequestDetails.Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeID == employeeId && x.Status != "Rejected" && x.Status != "Cancelled" && x.EffectiveFromDate <= date.Date).OrderByDescending(x => x.EffectiveFromDate).FirstOrDefault();            
           decimal? leaves = (from leave in dbContext.ApplyLeave
                      join ald in dbContext.AppliedLeaveDetails on leave.LeaveId equals ald.LeaveId
                      where leave.EmployeeId == employeeId && ald.Date >= previousRequest.EffectiveFromDate && ald.Date <= date && (leave.Status != "Cancelled" &&
                      (leave.Status != "Rejected" || (leave.Status == "Rejected" && ald.AppliedLeaveStatus == true))) && leave.LeaveTypeId == leaveTypeId
                      select new LeaveGrantRequestDetails
                      {
                          BalanceDay = ald.IsFullDay == true ? 1 : (decimal)0.5
                      }).Sum(x => x.BalanceDay);
            if(leaves !=null)
            {
                BalanceDay = (previousRequest?.NumberOfDay == null ? 0 : (decimal)previousRequest.NumberOfDay) - (leaves==null?0:(decimal)leaves);
            }
            else
            {
                BalanceDay = previousRequest?.NumberOfDay==null?0:(decimal)previousRequest.NumberOfDay;
            }
            return BalanceDay;
        }
        public List<LeaveGrantRequestDetails> GetGrantRequestDetailsByLeaveTypeId(int leaveTypeId, int employeeId,DateTime date,int grantRequestId)
        {
            return dbContext.LeaveGrantRequestDetails.Where(x => x.Status == "Approved" && x.EmployeeID == employeeId && x.LeaveTypeId == leaveTypeId && x.EffectiveFromDate <= date
            && x.LeaveGrantDetailId != grantRequestId).OrderBy(x => x.EffectiveFromDate).ToList();
        }
        public List<LeaveGrantRequestDetails> CheckGrantRequestExpiry(int leaveTypeId, int employeeId, DateTime date)
        {
            return dbContext.LeaveGrantRequestDetails.Where(x => x.Status == "Approved" && x.EmployeeID == employeeId && x.LeaveTypeId == leaveTypeId && x.EffectiveToDate !=null &&
            (x.EffectiveToDate==null ? DateTime.MaxValue.Date : ((DateTime)x.EffectiveToDate).Date) == date.Date && x.BalanceDay !=0).OrderBy(x => x.EffectiveFromDate).ToList();
        }
        public List<LeaveGrantRequestDetails> GetGrantRequestCardBalance(int leaveTypeId, int employeeId, DateTime fromDate, DateTime toDate)
        {
            return dbContext.LeaveGrantRequestDetails.Where(x => x.Status == "Approved" && x.EmployeeID == employeeId && x.LeaveTypeId == leaveTypeId && x.EffectiveFromDate <= toDate.Date
            && (x.EffectiveToDate>= fromDate.Date || x.EffectiveToDate==null)).OrderBy(x => x.EffectiveFromDate).ToList();
        }
        public decimal GetGrantRequestCardBalanceByDate(int employeeId, int leaveTypeId, DateTime date)
        {
            decimal balance = 0;
            List<LeaveGrantRequestDetails>  result=dbContext.LeaveGrantRequestDetails.Where(x => x.Status == "Approved" && x.EmployeeID == employeeId && x.LeaveTypeId == leaveTypeId && x.EffectiveFromDate <= date
            && (x.EffectiveToDate >= date || x.EffectiveToDate == null)).OrderBy(x => x.EffectiveFromDate).ToList();
            if(result?.Count>0)
            {
                foreach(LeaveGrantRequestDetails item in result)
                {
                    balance = balance + (item?.BalanceDay == null ? 0 : (decimal)item.BalanceDay);
                }
            }
            return balance;
        }

        public List<int> GetGrantLeaveByManagerId(int managerId)
        {
            return  dbContext.LeaveGrantRequestDetails.Join(dbContext.EmployeeGrantLeaveApproval, leave => leave.LeaveGrantDetailId, ald => ald.LeaveGrantDetailId, (leave, ald) => new { leave, ald })
                      .Where(rs => rs.ald.ApproverEmployeeId == managerId && rs.leave.Status == "Pending" && rs.ald.Status == "Pending").Select(x =>
                         x.leave.EmployeeID
                      ).ToList();
        }
    }
}
