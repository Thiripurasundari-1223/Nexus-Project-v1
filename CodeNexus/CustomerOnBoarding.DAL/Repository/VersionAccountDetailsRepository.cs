using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnBoarding.DAL.Repository
{

    public interface IVersionAccountDetailsRepository : IBaseRepository<VersionAccountDetails>
    {
        List<KeyWithValue> GetVersionNameByAccountId(int accountId);
    }
    public class VersionAccountDetailsRepository : BaseRepository<VersionAccountDetails>, IVersionAccountDetailsRepository
    {
        private readonly COBDBContext dbContext;
        public VersionAccountDetailsRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<KeyWithValue> GetVersionNameByAccountId(int accountId)
        {
            return dbContext.VersionAccountDetails.Where(x => x.AccountId == accountId).Select(x => new KeyWithValue { Key = x.VersionId, Value = x.VersionName }).OrderByDescending(x => x.Key).ToList();
        }
    }
}
