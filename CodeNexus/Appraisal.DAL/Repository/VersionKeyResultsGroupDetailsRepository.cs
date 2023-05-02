using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{
    public interface IVersionKeyResultsGroupDetailsRepository : IBaseRepository<VersionKeyResultsGroupDetails>
    {
        VersionKeyResultsGroupDetails GetVersionKeyResultGroupDetailsById(int versionId, int departmentId, int roleId, int objectiveId, int keyResultGroupId, int keyResultId);
        List<VersionKeyResultsGroupDetails> GetVersionKeyResultGroupDetailsByGroupId(int groupId);
    }
    public class VersionKeyResultsGroupDetailsRepository : BaseRepository<VersionKeyResultsGroupDetails>, IVersionKeyResultsGroupDetailsRepository
    {
        private readonly AppraisalDBContext dbContext;
        public VersionKeyResultsGroupDetailsRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public VersionKeyResultsGroupDetails GetVersionKeyResultGroupDetailsById(int versionId, int departmentId, int roleId, int objectiveId, int keyResultGroupId, int keyResultId)
        {
            return dbContext.VersionKeyResultsGroupDetails.Where(x => x.VERSION_ID == versionId &&
            x.DEPT_ID==departmentId &&
            x.ROLE_ID==roleId &&
            x.OBJECTIVE_ID==objectiveId &&
            x.KEY_RESULTS_GROUP_ID==keyResultGroupId &&
            x.KEY_RESULT_ID==keyResultId).FirstOrDefault();
        }
        public List<VersionKeyResultsGroupDetails> GetVersionKeyResultGroupDetailsByGroupId(int groupId)
        {
            return dbContext.VersionKeyResultsGroupDetails.Where(x => x.KEY_RESULTS_GROUP_ID == groupId).ToList();
        }
    }
}
