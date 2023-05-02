using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharedLibraries.ViewModels.Appraisal;

namespace Appraisal.DAL.Repository
{
    public interface IVersionDepartmentRoleRepository : IBaseRepository<VersionDepartmentRoleMapping>
    {
        List<VersionDepartmentRoleMapping> GetByDepartmentID(int versionId,int DepartmentId);
        VersionDepartmentRoleMapping GetByRoleID(int versionId, int DepartmentId,int RoleId);
        List<DepartmentList> GetVersionDepartmentRoleMapping(int versionId);
    }
    public class VersionDepartmentRoleRepository : BaseRepository<VersionDepartmentRoleMapping>, IVersionDepartmentRoleRepository
    {
        private readonly AppraisalDBContext dbContext;
        public VersionDepartmentRoleRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<VersionDepartmentRoleMapping> GetByDepartmentID(int versionId, int DepartmentId)
        {
            return dbContext.VersionDepartmentRoleMapping.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == DepartmentId).ToList();
        }
        public VersionDepartmentRoleMapping GetByRoleID(int versionId, int DepartmentId,int RoleId)
        {
            return dbContext.VersionDepartmentRoleMapping.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == DepartmentId && x.ROLE_ID == RoleId).FirstOrDefault();
        }
        public List<DepartmentList> GetVersionDepartmentRoleMapping(int versionId)
        {
            List<DepartmentList> departmentList = new List<DepartmentList>();
            foreach (int departmentId in dbContext.VersionDepartmentRoleMapping.Where(x => x.VERSION_ID == versionId).GroupBy(x => x.DEPT_ID).OrderBy(x => x.Key).Select(x => x.Key).ToList())
            {
                DepartmentList department = new DepartmentList();
                department.DepartmentId = departmentId;
                department.Roles = dbContext.VersionDepartmentRoleMapping.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == departmentId).Select(x =>
                 new RolesList { RoleId = x.ROLE_ID }).ToList();
                departmentList.Add(department);
            }
            return departmentList;                              
        }
    }
}
