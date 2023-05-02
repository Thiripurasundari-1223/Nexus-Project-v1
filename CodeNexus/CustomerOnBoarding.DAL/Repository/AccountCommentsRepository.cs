using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOnBoarding.DAL.Repository
{
    public interface IAccountCommentsRepository : IBaseRepository<AccountComments>
    {
        AccountComments GetByID(int pID);
        List<AccountCommentsView> GetByAccountID(int pAccountID);
    }
    public class AccountCommentsRepository : BaseRepository<AccountComments>, IAccountCommentsRepository
    {
        private readonly COBDBContext dbContext;
        public AccountCommentsRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public AccountComments GetByID(int pID)
        {
            return dbContext.AccountComments.Where(x => x.AccountCommentId == pID).FirstOrDefault();
        }
        public List<AccountCommentsView> GetByAccountID(int pAccountID)
        {
            return dbContext.AccountComments.Where(x => x.AccountId == pAccountID).Select(accComments=>
            new AccountCommentsView
            {
                AccountCommentId = accComments.AccountCommentId,
                AccountId = accComments.AccountId,
                CreatedByName = accComments.CreatedByName,
                Comments = accComments.Comments,
                CreatedBy = accComments.CreatedBy,
                CreatedOn = accComments.CreatedOn,
                ModifiedBy = accComments.ModifiedBy,
                ModifiedOn = accComments.ModifiedOn
            }).ToList();
        }
    }
}