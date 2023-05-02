using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{    
    public interface IVersionKeyResultsGroupRepository : IBaseRepository<VersionKeyResultsGroup>
    {
        VersionKeyResultsGroup GetVersionKeyResultGroupById(int keyResultGroupId);
        VersionKeyResultsGroup GetVersionKeyResultGroupByObjectiveId(int versionId, int departmentId, int roleId, int objectiveId, string groupName);
    }
    public class VersionKeyResultsGroupRepository : BaseRepository<VersionKeyResultsGroup>, IVersionKeyResultsGroupRepository
    {
        private readonly AppraisalDBContext dbContext;
        public VersionKeyResultsGroupRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public VersionKeyResultsGroup GetVersionKeyResultGroupById(int keyResultGroupId)
        {
            return dbContext.VersionKeyResultsGroup.Where(x => x.KEY_RESULTS_GROUP_ID == keyResultGroupId).FirstOrDefault();
        }
        public VersionKeyResultsGroup GetVersionKeyResultGroupByObjectiveId(int versionId,int departmentId, int roleId,int objectiveId, string groupName)
        {
            return groupName==null?null: dbContext.VersionKeyResultsGroup.Where(x => x.VERSION_ID == versionId && x.DEPT_ID==departmentId && x.ROLE_ID==roleId &&
            x.OBJECTIVE_ID==objectiveId && x.KEY_RESULTS_GROUP_NAME.ToLower() == groupName.ToLower()).FirstOrDefault();
        }
    }
}
