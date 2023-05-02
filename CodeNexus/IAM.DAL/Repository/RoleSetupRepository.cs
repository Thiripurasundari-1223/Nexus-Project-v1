using IAM.DAL.DBContext;
using IAM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAM.DAL.Repository
{
    public interface IRoleSetupRepository : IBaseRepository<RoleSetup>
    {
        //IEnumerable<RoleSetup> GetByRoleNameAndRoleID(string pRoleName, int pRoleID = 0);
        RoleSetup GetByID(int pRoleSetupID);
    }
    public class RoleSetupRepository : BaseRepository<RoleSetup>, IRoleSetupRepository
    {
        private readonly IAMDBContext dbContext;
        public RoleSetupRepository(IAMDBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public RoleSetup GetByID(int pRoleSetupID)
        {
            return dbContext.RoleSetup.Where(x => x.RoleSetupID == pRoleSetupID).FirstOrDefault();
        }
        //public IEnumerable<RoleSetup> GetByRoleNameAndRoleID(string pRoleName, int pRoleID = 0)
        //{
        //    int[] lstRole = dbContext.Role.Where(r => r.RoleId == pRoleID && r.RoleName.Equals(pRoleName)).Select(x => x.RoleId).ToArray();
        //    return dbContext.RoleSetup.Where(x => lstRole.Contains(x.RoleID)).ToList();
        //}
    }
}