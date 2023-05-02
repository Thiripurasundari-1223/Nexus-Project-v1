using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{
    public interface IVersionDepartmentRoleObjectiveRepository : IBaseRepository<VersionDepartmentRoleObjective>
    {
        VersionDepartmentRoleObjective GetVersionDepartmentRoleObjectiveById(int versionId, int departmentId, int roleId, int objectiveId);
        List<VersionDepartmentRoleObjective> GetVersionDepartmentRoleById(int versionId, int departmentId, int roleId);
    }
    public class VersionDepartmentRoleObjectiveRepository : BaseRepository<VersionDepartmentRoleObjective>, IVersionDepartmentRoleObjectiveRepository
    {
        private readonly AppraisalDBContext dbContext;
        public VersionDepartmentRoleObjectiveRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<VersionDepartmentRoleObjective> GetVersionDepartmentRoleById(int versionId, int departmentId, int roleId)
        {
            return dbContext.VersionDepartmentRoleObjective.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == departmentId && x.ROLE_ID == roleId ).ToList();
        }
        public VersionDepartmentRoleObjective GetVersionDepartmentRoleObjectiveById(int versionId, int departmentId, int roleId, int objectiveId)
        {
            return dbContext.VersionDepartmentRoleObjective.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == departmentId && x.ROLE_ID == roleId && x.OBJECTIVE_ID== objectiveId).Select(x => x).FirstOrDefault();
        }
    }
}
