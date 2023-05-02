using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;

namespace Appraisal.DAL.Repository
{
    public interface IVersionKeyResultsRepository : IBaseRepository<VersionKeyResults>
    {
        List<VersionKeyResults> GetByObjectiveId(int versionId,int departmentId,int RoleId,int ObjectiveId);
        VersionKeyResults GetByKRAId(int versionId, int DepartmentId, int RoleId, int ObjectiveId, int KRAId);
        List<VersionKeyResults> GetAllVersionKeyResults();
        List<VersionKeyResults> GetKRAVersiondetails(int versionId);
        bool CheckObjectiveUsedVersion(int objectiveId);
        bool CheckKRAUsedVersion(int KRAId);

    }
    public class VersionKeyResultsRepository : BaseRepository<VersionKeyResults>, IVersionKeyResultsRepository
    {
        private readonly AppraisalDBContext dbContext;
        public VersionKeyResultsRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<VersionKeyResults> GetByObjectiveId(int versionId,int departmentId, int RoleId, int ObjectiveId)
        {
            return dbContext.VersionKeyResults.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == departmentId && x.ROLE_ID == RoleId && x.OBJECTIVE_ID == ObjectiveId).ToList();
        }
        public VersionKeyResults GetByKRAId(int versionId, int DepartmentId, int RoleId, int ObjectiveId, int KRAId)
        {
            return dbContext.VersionKeyResults.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == DepartmentId && x.ROLE_ID == RoleId && x.OBJECTIVE_ID == ObjectiveId && x.KEY_RESULT_ID == KRAId).FirstOrDefault();
        }
        public List<VersionKeyResults> GetAllVersionKeyResults()
        {
            return dbContext.VersionKeyResults.ToList();
        }
        public List<VersionKeyResults> GetKRAVersiondetails(int versionId)
        {
            return dbContext.VersionKeyResults.Where(x => x.VERSION_ID == versionId).ToList();
        }
        public bool CheckObjectiveUsedVersion(int objectiveId)
        {
            VersionKeyResults kra= dbContext.VersionKeyResults.Where(x => x.OBJECTIVE_ID == objectiveId).FirstOrDefault();
            return kra != null ? true : false;
        }
        public bool CheckKRAUsedVersion(int KRAId)
        {
            VersionKeyResults kra = dbContext.VersionKeyResults.Where(x => x.KEY_RESULT_ID == KRAId).FirstOrDefault();
            return kra != null ? true : false;
        }


    }
}
