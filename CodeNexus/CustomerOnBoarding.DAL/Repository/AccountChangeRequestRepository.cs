using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOnBoarding.DAL.Repository
{
    public interface IAccountChangeRequestRepository : IBaseRepository<AccountChangeRequest>
    {
        AccountChangeRequest GetByID(int pID);
        List<AccountChangeRequestView> GetAccountChangeRequestListByAccountID(int pAccountID);
        List<AccountChangeRequest> GetByAccountID(int pAccountID);
    }
    public class AccountChangeRequestRepository : BaseRepository<AccountChangeRequest>, IAccountChangeRequestRepository
    {
        private readonly COBDBContext dbContext;
        public AccountChangeRequestRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public AccountChangeRequest GetByID(int pID)
        {
            return dbContext.AccountChangeRequest.Where(x => x.AccountChangeRequestId == pID).FirstOrDefault();
        }
        public List<AccountChangeRequest> GetByAccountID(int pAccountID)
        {
            return dbContext.AccountChangeRequest.Where(x => x.AccountId == pAccountID).ToList();
        }
        public List<AccountChangeRequestView> GetAccountChangeRequestListByAccountID(int pAccountID)
        {
            return dbContext.AccountChangeRequest.Where(x => x.AccountId == pAccountID && x.IsActive == true).Select(accountChangeRequest=>
            new AccountChangeRequestView
            {
                AccountChangeRequestId = accountChangeRequest.AccountChangeRequestId,
                AccountId = accountChangeRequest.AccountId,
                CreatedByName = accountChangeRequest.CreatedByName,
                AccountRelatedIssue = dbContext.AccountRelatedIssue.Where(x => x.AccountRelatedIssueId == accountChangeRequest.AccountRelatedIssueId).Select(x => x.AccountRelatedIssueReason).FirstOrDefault(),
                AccountRelatedIssueId = accountChangeRequest.AccountRelatedIssueId,
                Comments = accountChangeRequest.Comments,
                CreatedBy = accountChangeRequest.CreatedBy,
                CreatedOn = accountChangeRequest.CreatedOn,
                ModifiedBy = accountChangeRequest.ModifiedBy,
                ModifiedOn = accountChangeRequest.ModifiedOn
            }
            ).ToList();
        }
    }
}