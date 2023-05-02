using IAM.DAL.DBContext;
using IAM.DAL.Models;
using System.Linq;

namespace IAM.DAL.Repository
{
    public interface IUserAccessBagRepository
    {
        UserAccessBag GetUserAccessBag(string pUserEmailAddress);
    }
    public class UserAccessBagRepository : BaseRepository<UserAccessBag>, IUserAccessBagRepository
    {
        private readonly IAMDBContext dbContext;
        public UserAccessBagRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public UserAccessBag GetUserAccessBag(string pUserEmailAddress)
        {
            return dbContext.UserAccessesbag.Where(x => x.User.EmailAddress == pUserEmailAddress).SingleOrDefault();
        }
    }
}