using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOnBoarding.DAL.Repository
{
    public interface IAccountTypeRepository : IBaseRepository<AccountType>
    {
        AccountType GetByName(string pAccountType, int pAccountTypeId = 0);
        AccountType GetByID(int pAccountTypeId);
        List<AccountType> GetAccountType();
    }
    public class AccountTypeRepository : BaseRepository<AccountType>, IAccountTypeRepository
    {
        private readonly COBDBContext dbContext;
        public AccountTypeRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public AccountType GetByName(string pAccountType, int pAccountTypeId = 0)
        {
            if (pAccountTypeId > 0)
            {
                return dbContext.AccountType.Where(x => x.AccountTypeName == pAccountType && x.AccountTypeId == pAccountTypeId).FirstOrDefault();
            }
            return dbContext.AccountType.Where(x => x.AccountTypeId == pAccountTypeId).FirstOrDefault();
        }
        public AccountType GetByID(int pAccountTypeId)
        {
            return dbContext.AccountType.Where(x => x.AccountTypeId == pAccountTypeId).FirstOrDefault();
        }
        public List<AccountType> GetAccountType()
        {
            return dbContext.AccountType.ToList();
        }
    }
}