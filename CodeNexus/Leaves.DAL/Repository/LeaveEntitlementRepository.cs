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
    public interface ILeaveEntitlementRepository : IBaseRepository<LeaveEntitlement>
    {
        public LeaveEntitlement GetEntitlementByLeaveId(int leaveTypeId);
        LeaveEntitlement GetByID(int pLeaveTypeId);
        List<LeaveCarryForwardListView> GetCarryForwardList();
        LeaveCarryForwardListView GetLeaveTypeCarryForwardDetails(int leavetypeId);
    }
    public class LeaveEntitlementRepository : BaseRepository<LeaveEntitlement>, ILeaveEntitlementRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveEntitlementRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public LeaveEntitlement GetEntitlementByLeaveId(int leaveTypeId)
        {
            LeaveEntitlement Entitlement = new LeaveEntitlement();
            Entitlement = dbContext.LeaveEntitlement.Where(x => x.LeaveTypeId == leaveTypeId).FirstOrDefault();
            if (Entitlement == null)
            {
                LeaveEntitlement AddEntitlement = new LeaveEntitlement()
                {
                    LeaveTypeId = 0
                };
                return AddEntitlement;
            }
            return Entitlement;
        }
        public LeaveEntitlement GetByID(int pLeaveTypeId)
        {
            return dbContext.LeaveEntitlement.Where(x => x.LeaveTypeId == pLeaveTypeId).FirstOrDefault();
        }
      
        public List<LeaveCarryForwardListView> GetCarryForwardList()
        {
            List<LeaveCarryForwardListView> leaveCarryForwardLists = new();
            leaveCarryForwardLists = (from LeaveEntitle in dbContext.LeaveEntitlement
                                      join appconst in dbContext.AppConstants on LeaveEntitle.ResetYear equals appconst.AppConstantId
                                      select new LeaveCarryForwardListView
                                      {
                                          LeaveTypeID= (int)LeaveEntitle.LeaveTypeId,
                                          CarryForwardID= LeaveEntitle.CarryForwardId,
                                          CarryForwardName= dbContext.AppConstants.Where(x => x.AppConstantId == LeaveEntitle.CarryForwardId).Select(x => x.AppConstantValue).FirstOrDefault(),
                                          MaximumCarryForwardDays= (decimal)LeaveEntitle.MaximumCarryForwardDays,
                                          ReimbursementID = LeaveEntitle.ReimbursementId,
                                          ReimbursementName= dbContext.AppConstants.Where(x => x.AppConstantId == LeaveEntitle.ReimbursementId).Select(x => x.AppConstantValue).FirstOrDefault(),
                                          MaximumReimbursementDays= (decimal)LeaveEntitle.MaximumReimbursementDays,
                                          ResetYear =LeaveEntitle.ResetYear,
                                          ResetMonth=LeaveEntitle.ResetMonth,
                                          ResetDay=LeaveEntitle.ResetDay,
                                          Period= dbContext.AppConstants.Where(x => x.AppConstantId == LeaveEntitle.ResetYear).Select(x => x.AppConstantValue).FirstOrDefault(),

                                      }).ToList();


            return leaveCarryForwardLists;
        }
        public LeaveCarryForwardListView GetLeaveTypeCarryForwardDetails(int leavetypeId)
        {
            return (from LeaveEntitle in dbContext.LeaveEntitlement
                                      join appconst in dbContext.AppConstants on LeaveEntitle.ResetYear equals appconst.AppConstantId
                                      where LeaveEntitle.LeaveTypeId== leavetypeId
                                      select new LeaveCarryForwardListView
                                      {
                                          LeaveTypeID = (int)LeaveEntitle.LeaveTypeId,
                                          CarryForwardID = LeaveEntitle.CarryForwardId,
                                          CarryForwardName = dbContext.AppConstants.Where(x => x.AppConstantId == LeaveEntitle.CarryForwardId).Select(x => x.AppConstantValue).FirstOrDefault(),
                                          MaximumCarryForwardDays = (decimal)LeaveEntitle.MaximumCarryForwardDays,
                                          ReimbursementID = LeaveEntitle.ReimbursementId,
                                          ReimbursementName = dbContext.AppConstants.Where(x => x.AppConstantId == LeaveEntitle.ReimbursementId).Select(x => x.AppConstantValue).FirstOrDefault(),
                                          MaximumReimbursementDays = (decimal)LeaveEntitle.MaximumReimbursementDays,
                                          ResetYear = LeaveEntitle.ResetYear,
                                          ResetMonth = LeaveEntitle.ResetMonth,
                                          ResetDay = LeaveEntitle.ResetDay,
                                          Period = dbContext.AppConstants.Where(x => x.AppConstantId == LeaveEntitle.ResetYear).Select(x => x.AppConstantValue).FirstOrDefault(),

                                      }).FirstOrDefault();
        }

    }
}
