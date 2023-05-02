using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{
    public interface IAuditRepository : IBaseRepository<ProjectAudit>
    {


        //void AddProjectChangesToAudit(ProjectAudit projectAudit);
        bool GetProjectId(int projectId);
    }

    public class AuditRepository : BaseRepository<ProjectAudit>, IAuditRepository
    {
        private readonly PMDBContext dbContext;
    

        public AuditRepository(PMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public bool GetProjectId(int projectID)
        {
            if (projectID > 0)
            {
                ProjectAudit objProjectAudit = dbContext.ProjectAudit.Where(r => r.ProjectID == projectID).FirstOrDefault();
                if (objProjectAudit != null) {
                if (objProjectAudit.ProjectID != null) return true;
                }
            }
            return false;







        }


    }
}