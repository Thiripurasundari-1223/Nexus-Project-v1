using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOnBoarding.DAL.Repository
{
    public interface IBillingCycleRepository : IBaseRepository<BillingCycle>
    {
        BillingCycle GetByName(string pBillingCycle, int pBillingCycleId = 0);
        BillingCycle GetByID(int pBillingCycleId);
        List<BillingCycle> GetAllBillingCycle();
    }
    public class BillingCycleRepository : BaseRepository<BillingCycle>, IBillingCycleRepository
    {
        private readonly COBDBContext dbContext;
        public BillingCycleRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public BillingCycle GetByName(string pBillingCycle, int pBillingCycleId = 0)
        {
            if (pBillingCycleId > 0)
            {
                return dbContext.BillingCycle.Where(x => x.BillingCycleDescription == pBillingCycle && x.BillingCycleId == pBillingCycleId).FirstOrDefault();
            }
            return dbContext.BillingCycle.Where(x => x.BillingCycleId == pBillingCycleId).FirstOrDefault();
        }
        public BillingCycle GetByID(int pBillingCycleId)
        {
            return dbContext.BillingCycle.Where(x => x.BillingCycleId == pBillingCycleId).FirstOrDefault();
        }
        public List<BillingCycle> GetAllBillingCycle()
        {
            return dbContext.BillingCycle.ToList();
        }
    }
}