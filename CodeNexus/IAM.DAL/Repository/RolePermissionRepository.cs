using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System.Collections.Generic;
using System.Linq;

namespace IAM.DAL.Repository
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermissions>
    {
        List<RolesDetail> GetRolePermissionsByEmail(string email, int pRoleId);
        List<RolePermissions> GetRolePermissionsByRoleId(int pRoleId);
        List<ModuleWiseFeatureDetails> GetModuleWiseFeatureDetails();
        List<RoleFeatureList> GetRoleFeatureList(int roleId);
        List<Modules> GetModuleDescription();
    }
    public class RolePermissionRepository : BaseRepository<RolePermissions>, IRolePermissionRepository
    {
        private readonly IAMDBContext dbContext;
        public RolePermissionRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<RolesDetail> GetRolePermissionsByEmail(string email, int pRoleId)
        {
            List<RolesDetail> rolesDetails = new List<RolesDetail>();
            List<RolePermissionView> rolePermission = new List<RolePermissionView>();
            if (!string.IsNullOrEmpty(email))
            {
                pRoleId = dbContext.Employees.Where(x => x.EmailAddress == email).Select(x => x.SystemRoleId == null ? 0 : (int)x.SystemRoleId).FirstOrDefault();
            }
            List<SystemRoles> lstRoles = new List<SystemRoles>();
            rolePermission = (from rolePermissions in dbContext.RolePermissions
                              join modules in dbContext.Modules on rolePermissions.ModuleId equals modules.ModuleId
                              join features in dbContext.Features on rolePermissions.FeatureId equals features.FeatureId
                              where rolePermissions.IsEnabled == true
                              select new RolePermissionView
                              {
                                  RolePermissionId = rolePermissions.RolePermissionId,
                                  ModuleId = rolePermissions.ModuleId,
                                  ModuleName = modules.ModuleName,
                                  FeatureId = rolePermissions.FeatureId,
                                  FeatureName = features.FeatureName,
                                  RoleId = rolePermissions.RoleId,
                                  IsEnabled = rolePermissions.IsEnabled
                              }).ToList();
            if (pRoleId > 0)
            {
                lstRoles = dbContext.SystemRoles.Where(x => x.RoleId == pRoleId).ToList();
            }
            else
            {
                lstRoles = dbContext.SystemRoles.OrderBy(x => x.RoleName).ToList();
            }
            if (lstRoles?.Count > 0)
            {
                foreach (var item in lstRoles)
                {
                    RolesDetail roleDetail = new RolesDetail();
                    roleDetail.RoleId = item?.RoleId;
                    roleDetail.RoleName = item?.RoleName;
                    roleDetail.RolePermission = rolePermission.Where(x => x.RoleId == item?.RoleId).Select(x => new RolePermission { FeatureName = x.FeatureName, ModuleName = x.ModuleName, ModuleId = x.ModuleId, FeatureId = x.FeatureId }).ToList();
                    rolesDetails.Add(roleDetail);
                }

            }
            return rolesDetails;
        }
        public List<RolePermissions> GetRolePermissionsByRoleId(int pRoleId)
        {
            return dbContext.RolePermissions.Where(x => x.RoleId == pRoleId).ToList();
        }
        public List<ModuleWiseFeatureDetails> GetModuleWiseFeatureDetails()
        {
            var moduleFeatures = (from moduleFeatureMapping in dbContext.ModuleFeatureMapping
                                  join modules in dbContext.Modules on moduleFeatureMapping.ModuleId equals modules.ModuleId
                                  join features in dbContext.Features on moduleFeatureMapping.FeatureId equals features.FeatureId
                                  select new
                                  {
                                      moduleFeatureMapping.ModuleFeatureMappingId,
                                      moduleFeatureMapping.ModuleId,
                                      modules.ModuleName,
                                      features.FeatureId,
                                      features.FeatureName
                                  }
                                  ).OrderBy(x => x.ModuleName).ToList();
            List<ModuleWiseFeatureDetails> moduleWiseFeatureDetails = new List<ModuleWiseFeatureDetails>();
            foreach (var item in moduleFeatures?.GroupBy(x => x.ModuleId))
            {
                ModuleWiseFeatureDetails moduleWiseFeatureDetail = new ModuleWiseFeatureDetails
                {
                    ModuleId = moduleFeatures.Where(x => x.ModuleId == item?.Key.Value).Select(x => x.ModuleId).FirstOrDefault(),
                    ModuleName = moduleFeatures.Where(x => x.ModuleId == item?.Key.Value).Select(x => x.ModuleName).FirstOrDefault(),
                    FeatureDetails = moduleFeatures.Where(x => x.ModuleId == item?.Key.Value).Select(x => new FeatureDetails { FeatureId = x.FeatureId, FeatureName = x.FeatureName }).ToList()
                };
                moduleWiseFeatureDetails.Add(moduleWiseFeatureDetail);
            }
            return moduleWiseFeatureDetails;
        }
        public List<RoleFeatureList> GetRoleFeatureList(int roleId)
        {
            return (from role in dbContext.RolePermissions
                    join module in dbContext.Modules on role.ModuleId equals module.ModuleId
                    join feature in dbContext.Features on role.FeatureId equals feature.FeatureId
                    where role.RoleId == roleId && role.IsEnabled == true
                    select new RoleFeatureList
                    {
                        RoleFeatureId = feature.FeatureId,
                        FeatureId = feature.FeatureId,
                        FeatureName = feature.FeatureName,
                        ModuleId = module.ModuleId,
                        ModuleName = module.ModuleName,
                        IsMenu = module.IsMenu,
                        NavigationURL = module.NavigationURL,
                        FeatureNavigationURL=feature.NavigationURL,
                        ModuleIcon=module.ModuleIcon
                    }).ToList();
        }
        public List<Modules> GetModuleDescription()
        {
            return dbContext.Modules.Where(x => x.IsActive == true).ToList();
        }
    }
}