using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface ISystemRoleRepository : IBaseRepository<SystemRoles>
    {
        bool RoleNameDuplication(string pRoleName);
        string GetSystemRoleNameByRoleId(int? roleId);
    }
    public class SystemRoleRepository : BaseRepository<SystemRoles> , ISystemRoleRepository
    {
        private readonly IAMDBContext dbContext;
        public SystemRoleRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public bool RoleNameDuplication(string pRoleName)
        {
            SystemRoles roles = dbContext.SystemRoles.Where(x => x.RoleName == pRoleName).FirstOrDefault();
            if (roles?.RoleId > 0)
                return true;
            return false;
        }
        public string GetSystemRoleNameByRoleId(int? roleId)
        {
            return dbContext.SystemRoles.Where(y => y.RoleId == roleId).Select(x => x.RoleName).FirstOrDefault();
        }
    }
}
