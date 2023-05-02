using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{
    public interface IAppraisalObjectiveRepository : IBaseRepository<ObjectiveMaster>
    {
        ObjectiveMaster GetByID(int objectiveId);
        ObjectiveMaster GetByName(string objectiveName);
        List<ObjectiveMaster> GetAllObjectiveDetails();
        List<ObjectiveKRA> GetVersionDepartmentRoleKRAMapping(int versionId, int departmentId, int roleId);
        bool ObjectiveNameDuplication(string objectiveName, int objectiveId);
        int GetObjectiveIdByName(string objectiveName);
    }
    public class AppraisalObjectiveRepository : BaseRepository<ObjectiveMaster>, IAppraisalObjectiveRepository
    {
        private readonly AppraisalDBContext dbContext;
        public AppraisalObjectiveRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public ObjectiveMaster GetByID(int objectiveId)
        {
            return dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_ID == objectiveId).FirstOrDefault();
        }
        public ObjectiveMaster GetByName(string objectiveName)
        {
            return objectiveName==null?null: dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_NAME.ToLower() == objectiveName.ToLower()).FirstOrDefault();
        }
        public List<ObjectiveMaster> GetAllObjectiveDetails()
        {
            return dbContext.ObjectiveMaster.OrderByDescending(x => x.CREATED_DATE).ToList();
        }
        public List<ObjectiveKRA> GetVersionDepartmentRoleKRAMapping(int versionId, int departmentId, int roleId)
        {
            List<ObjectiveKRA> objectiveList = new List<ObjectiveKRA>();
            List<int> objectiveIdList = dbContext.VersionDepartmentRoleObjective.Where(x => x.VERSION_ID == versionId &&
              x.DEPT_ID == departmentId && x.ROLE_ID == roleId).Select(x => x.OBJECTIVE_ID).ToList();
            foreach (int objectiveId in objectiveIdList)
            {
                ObjectiveKRA ObjectiveRolemapping = new ObjectiveKRA();
                ObjectiveRolemapping.ObjectiveId = objectiveId;
                ObjectiveRolemapping.ObjectiveName = dbContext.ObjectiveMaster.Where(x=>x.OBJECTIVE_ID==objectiveId).Select(x=>x.OBJECTIVE_NAME).FirstOrDefault();
                ObjectiveRolemapping.ObjectiveWeightage = dbContext.VersionDepartmentRoleObjective.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == departmentId && x.ROLE_ID == roleId && x.OBJECTIVE_ID == objectiveId).Select(x => x.OBJECTIVE_WEIGHTAGE).FirstOrDefault();
                ObjectiveRolemapping.kRADetails = (from mas in dbContext.KeyResultMaster
                                                   join
             vkr in dbContext.VersionKeyResults on mas.KEY_RESULT_ID equals vkr.KEY_RESULT_ID
                                                   where vkr.VERSION_ID == versionId && vkr.DEPT_ID == departmentId && vkr.ROLE_ID == roleId && vkr.OBJECTIVE_ID== objectiveId
                                                   select new KRADetails { KRAId = vkr.KEY_RESULT_ID, KRAName = mas.KEY_RESULT_NAME }).ToList();

                objectiveList.Add(ObjectiveRolemapping);
            }
            return objectiveList;
        }
        public bool ObjectiveNameDuplication(string objectiveName,int objectiveId)
        {
            //int objectiveId = dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_NAME == objectiveName).Select(x=>x.OBJECTIVE_ID).FirstOrDefault();
            //if (objectiveId !=0)
            //    return true;
            //return false;
            bool isDuplicateName = false;
            string existName = dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_NAME.ToLower() == objectiveName.ToLower() && (x.OBJECTIVE_ID == objectiveId || objectiveId == 0)).Select(x => x.OBJECTIVE_NAME).FirstOrDefault();
            if (objectiveId == 0 && existName != null)
            {
                isDuplicateName = true;
            }
            else if (objectiveId != 0 && existName?.ToLower() != objectiveName?.ToLower())
            {
                string newName = dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_NAME.ToLower() == objectiveName.ToLower()).Select(x => x.OBJECTIVE_NAME).FirstOrDefault();
                if (newName != null)
                {
                    isDuplicateName = true;
                }
            }
            return isDuplicateName;
        }
        public int GetObjectiveIdByName(string objectiveName)
        {
           return objectiveName==null?0:dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_NAME.ToLower()== objectiveName.ToLower()).Select(x => x.OBJECTIVE_ID).FirstOrDefault();
        }
    }
}
