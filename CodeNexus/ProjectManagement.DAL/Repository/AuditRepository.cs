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
        ProjectAudit findByProjectId(int projectId);
        bool GetProjectId(int projectID);


    }



    public class AuditRepository : BaseRepository<ProjectAudit>, IAuditRepository

    {

        private readonly PMDBContext _dbContext;




        public AuditRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ProjectAudit findByProjectId(int projectId)
        {
            return _dbContext.ProjectAudit.Where(r => r.ProjectID == projectId).FirstOrDefault();
        }

        public bool GetProjectId(int projectID)
        {
            if (projectID > 0)
            {
                ProjectAudit objProjectAudit =  _dbContext.ProjectAudit.Where(r => r.ProjectID == projectID).FirstOrDefault();
                if (objProjectAudit != null && objProjectAudit.ProjectID > 0)  return true; 
            }
            return false;

           

        }
    }

}


