using IAM.DAL.DBContext;
using IAM.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace IAM.DAL.Repository
{
    public interface IUserRolesRepository : IBaseRepository<UserRoles>
    {
        UserRoles GetByUserID(int pUserID = 0);
        IEnumerable<UserRoles> GetByRoleName(string pRoleName);
        UserRoles GetByID(int pUserRoleId = 0);
    }
    public class UserRolesRepository : BaseRepository<UserRoles>, IUserRolesRepository
    {
        private readonly IAMDBContext dbContext;
        public UserRolesRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public UserRoles GetByID(int pUserRoleId = 0)
        {
            return dbContext.UserRoles.Where(x => x.UserRoleId== pUserRoleId).FirstOrDefault();
        }
        public UserRoles GetByUserID(int pUserID = 0)
        {
            return dbContext.UserRoles.Where(x => x.UserID == pUserID).FirstOrDefault();
        }
        IEnumerable<UserRoles> IUserRolesRepository.GetByRoleName(string pRoleName)
        {
            int roleID = dbContext.Role.Where(r => r.RoleName == pRoleName).Select(r => r.RoleId).FirstOrDefault();
            return dbContext.UserRoles.Where(x =>  x.RoleId== roleID).ToList();
        }
    }
}