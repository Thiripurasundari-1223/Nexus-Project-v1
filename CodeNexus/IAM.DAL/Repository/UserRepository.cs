using IAM.DAL.DBContext;
using IAM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAM.DAL.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        bool ValidateRegisteredUser(User user);
        bool ValidateUsername(User user);
        int GetLoggedUserID(User user);
        User GetLoggedUserByName(string pEmailAddress);
        User GetByName(string pCustomerName, int pCustomerID = 0);
        User GetByID(int pCustomerID);
        List<User> GetByName(string[] pCustomerNames);
    }
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IAMDBContext dbContext;
        public UserRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public int GetLoggedUserID(User registeruser)
        {
            User user = dbContext.User.Where(x => x.EmailAddress == registeruser.EmailAddress && x.Password == registeruser.Password).FirstOrDefault();
            if (user != null)
                return user.UserId;
            return 0;
        }
        public User GetLoggedUserByName(string pEmailAddress)
        {
            return dbContext.User.Where(x => x.EmailAddress == pEmailAddress).FirstOrDefault();
        }
        public bool ValidateRegisteredUser(User registeruser)
        {
            int userCount = dbContext.User.Where(x => x.EmailAddress == registeruser.EmailAddress && x.Password == registeruser.Password).Count();
            if (userCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ValidateUsername(User registeruser)
        {
            int userCount = dbContext.User.Where(x => x.EmailAddress == registeruser.EmailAddress).Count();
            if (userCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public User GetByName(string pUserName, int pUserID = 0)
        {
            if (pUserID > 0)
            {
                return dbContext.User.Where(x => x.EmailAddress == pUserName && x.UserId == pUserID).FirstOrDefault();
            }
            return dbContext.User.Where(x => x.UserId == pUserID).FirstOrDefault();
        }
        public List<User> GetByName(string[] pUserNames)
        {
            return dbContext.User.Where(x => pUserNames.Contains(x.EmailAddress)).ToList();
        }
        public User GetByID(int pUserID)
        {
            return dbContext.User.Where(x => x.UserId == pUserID).FirstOrDefault();
        }
    }
}