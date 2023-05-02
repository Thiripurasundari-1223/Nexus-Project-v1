using IAM.DAL.DBContext;
using IAM.DAL.Repository;
using System.Linq;

namespace IAM.DAL.Models
{
    public interface ILoginHistoryRepository : IBaseRepository<LoginHistory>
    {
        LoginHistory GetByID(int pCustomerID);
        public LoginHistory GetLoggedUserByName(string pEmailAddress);
    }
    public class LoginHistoryRepository : BaseRepository<LoginHistory>, ILoginHistoryRepository
    {
        private readonly IAMDBContext dbContext;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public LoginHistoryRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        #endregion

        #region Login History
        /// <summary>
        /// Login History
        /// </summary>
        /// <param name="pUserID"></param>
        /// <returns></returns>
        public LoginHistory GetByID(int pUserID)
        {
            return dbContext.LoginHistories.Where(x => x.UserID == pUserID).FirstOrDefault();
        }
        #endregion

        #region Get Logged User By Name
        /// <summary>
        /// Get Logged User By Name
        /// </summary>
        /// <param name="pEmailAddress"></param>
        /// <returns></returns>
        public LoginHistory GetLoggedUserByName(string pEmailAddress)
        {
            return dbContext.LoginHistories.Where(x => x.Username == pEmailAddress).FirstOrDefault();
        }
        #endregion
    }
}