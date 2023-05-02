using IAM.DAL.DBContext;
using IAM.DAL.Models;
using SharedLibraries.ViewModels.Employees;
using System.Linq;

namespace IAM.DAL.Repository
{
    public interface IUserTokenRepository : IBaseRepository<UserToken>
    {
        bool CheckAccessToken(string pEmailAddress);
        UserToken GetByEmailAddress(string pEmailAddress);
        UserToken CheckAccessToken(string pEmailAddress, string AccessToken);
    }
    public class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
    {
        private readonly IAMDBContext dbContext;
        public UserTokenRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public bool CheckAccessToken(string pEmailAddress)
        {
            return dbContext.UserTokens.Any(x => x.EmailAddress == pEmailAddress);
        }
        public UserToken CheckAccessToken(string pEmailAddress, string AccessToken)
        {
            return dbContext.UserTokens.Where(x => x.EmailAddress == pEmailAddress && x.AccessToken== AccessToken).FirstOrDefault();
        }
        public UserToken GetByEmailAddress(string pEmailAddress)
        {
            return dbContext.UserTokens.Where(x => x.EmailAddress == pEmailAddress).FirstOrDefault();
        }
    }
}