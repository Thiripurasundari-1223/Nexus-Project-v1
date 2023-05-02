using Appraisal.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using SharedLibraries;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{
    public interface IVersionRepository : IBaseRepository<VersionMaster>
    {
        VersionMaster GetByID(int versionId);
        List<VersionMaster> GetAllVersionDetails();
        List<VersionRoleGridDetails> GetVersionRolesGridviewData(int versionId);
        VersionDetailsView GetVersionDetailsById(int versionId);
        bool VersionNameDuplication(string versionName,int versionId);
        int GetByVersionIdToCopy(int versionId, int createdBy);
        int GetVersionIdByName(string name);
    }
    public class VersionRepository : BaseRepository<VersionMaster>, IVersionRepository
    {
        private readonly AppraisalDBContext dbContext;
        public VersionRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public VersionMaster GetByID(int versionId)
        {
            return dbContext.VersionMaster.Where(x => x.VERSION_ID == versionId).FirstOrDefault();
        }
        public List<VersionMaster> GetAllVersionDetails()
        {
            return dbContext.VersionMaster.OrderByDescending(x => x.CREATED_DATE).ToList();
        }
        public List<VersionRoleGridDetails> GetVersionRolesGridviewData(int versionId)
        {
            List<VersionRoleGridDetails> VersionRoleGridResult = (from ver in dbContext.VersionMaster
                                                                  join role in dbContext.VersionDepartmentRoleMapping on ver.VERSION_ID equals role.VERSION_ID
                                                                  where ver.VERSION_ID == versionId
                                                                  select new VersionRoleGridDetails
                                                                  {
                                                                      VersionId = ver.VERSION_ID,
                                                                      VersionName = ver.VERSION_NAME,
                                                                      VersionCode = ver.VERSION_CODE,
                                                                      VersionDesciption = ver.VERSION_DESC,
                                                                      DepartmentId = role.DEPT_ID,
                                                                      RoleId = role.ROLE_ID,
                                                                  }).Distinct().ToList();
            return VersionRoleGridResult == null ? new List<VersionRoleGridDetails>() : VersionRoleGridResult;
        }
        public VersionDetailsView GetVersionDetailsById(int versionId)
        {
            VersionDetailsView versionDetails = new VersionDetailsView();
            versionDetails = dbContext.VersionMaster.Where(x => x.VERSION_ID == versionId).Select(x => new VersionDetailsView
            {
                VersionId = x.VERSION_ID,
                VersionCode = x.VERSION_CODE,
                VersionName = x.VERSION_NAME,
                VersionDescription = x.VERSION_DESC
            }).FirstOrDefault();
            //Version role mapping
            versionDetails.VersionRoleMapping = new List<VersionRoleView>();
            foreach (int departmentId in dbContext.VersionDepartmentRoleMapping.Where(x => x.VERSION_ID == versionId).GroupBy(x => x.DEPT_ID).OrderBy(x => x.Key).Select(x => x.Key).ToList())
            {
                VersionRoleView rolemapping = new VersionRoleView();
                rolemapping.DepartmentId = departmentId;
                rolemapping.Roles = dbContext.VersionDepartmentRoleMapping.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == departmentId).Select(x =>
                 new RolesList { RoleId = x.ROLE_ID }).ToList();
                versionDetails.VersionRoleMapping.Add(rolemapping);
            }
            return versionDetails == null ? new VersionDetailsView() : versionDetails;
        }
        public bool VersionNameDuplication(string versionName,int versionId)
        {
            //return dbContext.VersionMaster.Where(x => x.VERSION_NAME == versionName).Select(x=>x.VERSION_ID).FirstOrDefault();
            bool isDuplicateName = false;
            string existName = dbContext.VersionMaster.Where(x => x.VERSION_NAME.ToLower() == versionName.ToLower() && (x.VERSION_ID == versionId || versionId == 0)).Select(x => x.VERSION_NAME).FirstOrDefault();
            if (versionId == 0 && existName != null)
            {
                isDuplicateName = true;
            }
            else if (versionId != 0 && existName?.ToLower() != versionName?.ToLower())
            {
                string newName = dbContext.VersionMaster.Where(x => x.VERSION_NAME.ToLower() == versionName.ToLower()).Select(x => x.VERSION_NAME).FirstOrDefault();
                if (newName != null)
                {
                    isDuplicateName = true;
                }
            }
            return isDuplicateName;
        }
        public int GetByVersionIdToCopy(int versionId, int createdBy)
        {
            //var version = dbContext.VersionMaster.FromSqlRaw("EXEC dbo.CopyandCreateVersionDetails @VersionId,@CreatedBy", parms.ToArray()).ToList();
            int result = dbContext.Database.ExecuteSqlRaw("EXEC dbo.CopyandCreateVersionDetails @VersionId={0},@CreatedBy={1}", versionId, createdBy);
            return result;
        }
        public int GetVersionIdByName(string name)
        {
            return name==null?0:dbContext.VersionMaster.Where(x => x.VERSION_NAME.ToLower() == name.ToLower()).Select(x => x.VERSION_ID).FirstOrDefault();
        }
    }
}
