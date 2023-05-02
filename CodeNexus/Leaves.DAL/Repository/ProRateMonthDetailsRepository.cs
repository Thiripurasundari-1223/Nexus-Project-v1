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
    public interface IProRateMonthDetailsRepository : IBaseRepository<ProRateMonthDetails>
    {
        List<ProRateMonthDetailsView> GetProRateMonthDetailsByID(int leaveTypeId);
        List<ProRateMonthDetails> GetByID(int leaveTypeId);

    }
    public class ProRateMonthDetailsRepository : BaseRepository<ProRateMonthDetails>, IProRateMonthDetailsRepository
    {
        private readonly LeaveDBContext dbContext;
        public ProRateMonthDetailsRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<ProRateMonthDetailsView> GetProRateMonthDetailsByID(int leaveTypeId)
        {

            if (leaveTypeId > 0)
            {
                return dbContext.ProRateMonthDetails.Where(x => x.LeaveTypeId == leaveTypeId)
                    .Select(x=>new ProRateMonthDetailsView
                    { 
                        ProRateMonthDetailId=x.ProRateMonthDetailId,
                        LeaveTypeId=x.LeaveTypeId,
                        Fromday=x.Fromday,
                        Today=x.Today,
                        Count=x.Count
                    }).ToList();
            }
            return null;
        }
        public List<ProRateMonthDetails> GetByID(int leaveTypeId)
        {
            if (leaveTypeId > 0)
            {
                return dbContext.ProRateMonthDetails.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
            }
            return null;
        }
    }
}
