using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{
    public interface IVersionBenchmarksRepository : IBaseRepository<VersionBenchMarks>
    {
        VersionBenchMarks GetByBenchmarkId(int BenchmarkId, int versionId, int DepartmentId, int RoleId, int ObjectiveId, int KRAId);
        List<VersionBenchObjKRAView> GetVersionBenchmarkObjectiveKRA(int versionId, int departmentId, int roleId);
        List<VersionBenchMarks> GetVersionKRABenchmarkRange(int versionId, int departmentId, int roleId, int objectiveId, int kraId);
    }
    public class VersionBenchmarksRepository : BaseRepository<VersionBenchMarks>, IVersionBenchmarksRepository
    {
        private readonly AppraisalDBContext dbContext;
        public VersionBenchmarksRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public VersionBenchMarks GetByBenchmarkId(int BenchmarkId, int versionId, int DepartmentId, int RoleId, int ObjectiveId, int KRAId)
        {
            return dbContext.VersionBenchMarks.Where(x => x.VERSION_ID == versionId && x.DEPT_ID == DepartmentId && x.ROLE_ID == RoleId && x.OBJECTIVE_ID == ObjectiveId && x.KEY_RESULT_ID == KRAId && x.BENCHMARK_ID == BenchmarkId).FirstOrDefault();
        }
        public List<VersionBenchObjKRAView> GetVersionBenchmarkObjectiveKRA(int versionId, int departmentId, int roleId)
        {
            List<VersionBenchObjKRAView> versionBenchmark = new List<VersionBenchObjKRAView>();
            List<int> objectiveIdList = dbContext.VersionDepartmentRoleObjective.Where(x => x.VERSION_ID == versionId &&
              x.DEPT_ID == departmentId && x.ROLE_ID == roleId).Select(x => x.OBJECTIVE_ID).ToList();
            foreach (int objectiveId in objectiveIdList)
            {

                VersionBenchObjKRAView ObjectiveRolemapping = new VersionBenchObjKRAView();
                ObjectiveRolemapping.ObjectiveId = objectiveId;
                ObjectiveRolemapping.ObjectiveName = dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_ID == objectiveId).Select(x => x.OBJECTIVE_NAME).FirstOrDefault();
                ObjectiveRolemapping.ObjectiveDescription = dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_ID == objectiveId).Select(x => x.OBJECTIVE_DESCRIPTION).FirstOrDefault();
                ObjectiveRolemapping.ObjectiveWeightage = dbContext.VersionDepartmentRoleObjective.Where(x => x.VERSION_ID==versionId && x.DEPT_ID==departmentId && x.ROLE_ID==roleId && x.OBJECTIVE_ID == objectiveId).Select(x => x.OBJECTIVE_WEIGHTAGE).FirstOrDefault();
                List<int> groupKRAList = dbContext.VersionKeyResultsGroupDetails.Where(vkr => vkr.VERSION_ID == versionId && vkr.DEPT_ID == departmentId && vkr.ROLE_ID == roleId && vkr.OBJECTIVE_ID == objectiveId).Select(x => x.KEY_RESULT_ID).ToList();
                List<KeyResultDetailView> KRAsViews = (from mas in dbContext.KeyResultMaster
                                                       join
                 vkr in dbContext.VersionKeyResults on mas.KEY_RESULT_ID equals vkr.KEY_RESULT_ID
                                                       where vkr.VERSION_ID == versionId && vkr.DEPT_ID == departmentId && vkr.ROLE_ID == roleId && vkr.OBJECTIVE_ID == objectiveId

                                                       select new KeyResultDetailView
                                                       {
                                                           VersionId = vkr.VERSION_ID,
                                                           Dept_Id = vkr.DEPT_ID,
                                                           Role_Id = vkr.ROLE_ID,
                                                           Objective_Id = vkr.OBJECTIVE_ID,
                                                           KeyResult_Id = vkr.KEY_RESULT_ID,
                                                           KeyResult_Name = mas.KEY_RESULT_NAME,
                                                           Key_Result_Weightage = vkr.KEY_RESULT_WEIGHTAGE,
                                                           Benchmark_Duration = vkr.BENCHMARK_DURATION,
                                                           Benchmark_From_Value = vkr.BENCHMARK_FROM_VALUE,
                                                           Benchmark_To_Value = vkr.BENCHMARK_TO_VALUE,
                                                           Benchmark_Operator = vkr.BENCHMARK_OPERATOR,
                                                           Benchmark_Type = vkr.BENCHMARK_TYPE,
                                                           Benchmark_UIType = vkr.BENCHMARK_UITYPE,
                                                           Benchmark_Value = vkr.BENCHMARK_VALUE,
                                                           Is_Document_Mandatory = vkr.IS_DOCUMENT_MANDATORY,
                                                           KRABenchmark = dbContext.VersionBenchMarks.Where(x => x.KEY_RESULT_ID == vkr.KEY_RESULT_ID &&
                                                            x.VERSION_ID == versionId && x.DEPT_ID == departmentId &&
                                                            x.ROLE_ID == roleId && x.OBJECTIVE_ID == objectiveId).Select(x =>
                                      new KRABenchmark
                                      {
                                          BenchmarkId = x.BENCHMARK_ID,
                                          BenchmarkValue = x.BENCHMARK_VALUE,
                                          BenchmarkWeightage = x.BENCHMARK_WEIGHTAGE,
                                          RangeFrom = x.RANGE_FROM,
                                          RangeTo = x.RANGE_TO
                                      }).ToList()
                                                       }).ToList();

                ObjectiveRolemapping.KRAsViews = KRAsViews.Where(x => !groupKRAList.Contains(x.KeyResult_Id)).Select(x => x).ToList();
                ObjectiveRolemapping.VersionBenchmarkKeyResultGroup = new List<VersionBenchmarkKeyResultGroup>();
                List<int> groupIdList = dbContext.VersionKeyResultsGroup.Where(x => x.VERSION_ID == versionId &&
               x.DEPT_ID == departmentId && x.ROLE_ID == roleId && x.OBJECTIVE_ID == objectiveId).Select(x => x.KEY_RESULTS_GROUP_ID).ToList();
                foreach (int groupId in groupIdList)
                {
                    VersionBenchmarkKeyResultGroup groupDetails = (from kraGroup in dbContext.VersionKeyResultsGroup
                                                                   join detail in dbContext.VersionKeyResultsGroupDetails on kraGroup.KEY_RESULTS_GROUP_ID equals detail.KEY_RESULTS_GROUP_ID
                                                                   where detail.VERSION_ID == versionId && detail.DEPT_ID == departmentId &&
                                                                   detail.ROLE_ID == roleId && detail.OBJECTIVE_ID == objectiveId &&
                                                                   detail.KEY_RESULTS_GROUP_ID == groupId
                                                                   select new VersionBenchmarkKeyResultGroup
                                                                   {
                                                                       VersionId = kraGroup.VERSION_ID,
                                                                       DeptId = kraGroup.DEPT_ID,
                                                                       RoleId = kraGroup.ROLE_ID,
                                                                       ObjectiveId = kraGroup.OBJECTIVE_ID,
                                                                       KeyResultGroupId = kraGroup.KEY_RESULTS_GROUP_ID,
                                                                       KeyResultGroupName = kraGroup.KEY_RESULTS_GROUP_NAME,
                                                                       KeyResultGroupWeightage = kraGroup.KEY_RESULT_GROUP_WEIGHTAGE,
                                                                       MandatoryKeyResultOption = kraGroup.MANDATORY_KEY_RESULT_OPTIONS
                                                                   }
                                  ).FirstOrDefault();
                    if (groupDetails != null)
                    {
                        List<int> groupKRA = dbContext.VersionKeyResultsGroupDetails.Where(x => x.KEY_RESULTS_GROUP_ID == groupId &&
                           x.VERSION_ID == versionId && x.ROLE_ID == roleId && x.DEPT_ID == departmentId &&
                           x.OBJECTIVE_ID == objectiveId).Select(x => x.KEY_RESULT_ID).ToList();
                        groupDetails.KeyResultDetail = KRAsViews.Where(x => groupKRA.Contains(x.KeyResult_Id) &&
                        x.VersionId == versionId && x.Role_Id == roleId && x.Dept_Id == departmentId &&
                           x.Objective_Id == objectiveId).Select(kra => new KeyResultDetailView
                           {
                               KeyResult_Id = kra.KeyResult_Id,
                               KeyResult_Name = kra.KeyResult_Name,
                               Key_Result_Weightage = kra.Key_Result_Weightage,
                               KRABenchmark = kra.KRABenchmark,
                               Benchmark_Duration = kra.Benchmark_Duration,
                               Benchmark_From_Value = kra.Benchmark_From_Value,
                               Benchmark_Operator = kra.Benchmark_Operator,
                               Benchmark_To_Value = kra.Benchmark_To_Value,
                               Benchmark_Type = kra.Benchmark_Type,
                               Benchmark_UIType = kra.Benchmark_UIType,
                               Benchmark_Value = kra.Benchmark_Value,
                               Dept_Id = kra.Dept_Id,
                               Objective_Id = kra.Objective_Id,
                               Role_Id = kra.Role_Id,
                               Updated_By = kra.Updated_By,
                               VersionId = kra.VersionId,
                               Is_Document_Mandatory=kra.Is_Document_Mandatory
                           }).ToList();
                    }

                    ObjectiveRolemapping.VersionBenchmarkKeyResultGroup.Add(groupDetails);
                }
                versionBenchmark.Add(ObjectiveRolemapping);
            }
            return versionBenchmark;
        }
        public List<VersionBenchMarks> GetVersionKRABenchmarkRange(int versionId, int departmentId, int roleId, int objectiveId, int kraId)
        {
            return dbContext.VersionBenchMarks.Where(x => x.VERSION_ID == versionId &&
            x.DEPT_ID == departmentId && x.ROLE_ID == roleId && x.OBJECTIVE_ID == objectiveId &&
            x.KEY_RESULT_ID == kraId).Select(x => x).ToList();
        }
    }
}
