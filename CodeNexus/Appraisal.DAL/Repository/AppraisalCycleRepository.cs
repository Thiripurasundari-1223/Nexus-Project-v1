using Appraisal.DAL.DBContext;
using SharedLibraries.Common;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{
    public interface IAppraisalCycleRepository : IBaseRepository<AppraisalMaster>
    {
        AppraisalMaster GetByID(int appCycleId);
        List<AppraisalMaster> GetAllAppraisalCycle();
        List<AppraisalMasterView> GetAllAppraisalCycleDetails();
        List<AppraisalCycleMasterData> GetIndividualAppraisalDropdownList(int employeeID);
        List<IndividualAppraisalObjKRAView> GetIndividualAppraisalDetailsByAppCycleId(int appcycleId, int departmentId, int roleId, int employeeId);
        public bool AppraisalCycleNameDuplication(string appCycleName, int appCycleId);
        bool checkEntityUsedAppCycle(int entityId);
        bool checkVersionUsedAppCycle(int versionId);
        List<KeyWithValue> GetAppraisalDurationList();
        AppraisalMaster GetByName(string appCycleName);
        int GetActiveAppCycleId();
        //public bool AppraisalCycleNameDuplicationByExcel(string appCycleName, int appCycleId);
    }
    public class AppraisalCycleRepository : BaseRepository<AppraisalMaster>, IAppraisalCycleRepository
    {
        private readonly AppraisalDBContext dbContext;
        public AppraisalCycleRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public AppraisalMaster GetByID(int appCycleId)
        {
            return dbContext.AppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleId).FirstOrDefault();
        }
        public AppraisalMaster GetByName(string appCycleName)
        {
            return appCycleName==null? null: dbContext.AppraisalMaster.Where(x => x.APP_CYCLE_NAME.ToLower() == appCycleName.ToLower()).FirstOrDefault();
        }
        public List<AppraisalMaster> GetAllAppraisalCycle()
        {
            return dbContext.AppraisalMaster.ToList();
        }
        public List<AppraisalMasterView> GetAllAppraisalCycleDetails()
        {
            List<AppraisalMasterView> lstAppraisalMasterView = new List<AppraisalMasterView>();
            lstAppraisalMasterView = (from app in dbContext.AppraisalMaster
                                      join entity in dbContext.EntityMaster on app.ENTITY_ID equals entity.ENTITY_ID
                                      join version in dbContext.VersionMaster on app.VERSION_ID equals version.VERSION_ID
                                      orderby app.CREATED_DATE descending
                                      select new AppraisalMasterView
                                      {
                                          APP_CYCLE_ID = app.APP_CYCLE_ID,
                                          ENTITY_ID = app.ENTITY_ID,
                                          ENTITY_NAME = entity.ENTITY_NAME,
                                          VERSION_ID = app.VERSION_ID,
                                          VERSION_NAME = version.VERSION_NAME,
                                          APP_CYCLE_NAME = app.APP_CYCLE_NAME,
                                          APP_CYCLE_DESC = app.APP_CYCLE_DESC,
                                          APP_CYCLE_START_DATE = app.APP_CYCLE_START_DATE,
                                          APP_CYCLE_END_DATE = app.APP_CYCLE_END_DATE,
                                          APPRAISEE_REVIEW_START_DATE = app.APPRAISEE_REVIEW_START_DATE,
                                          APPRAISEE_REVIEW_END_DATE = app.APPRAISEE_REVIEW_END_DATE,
                                          APPRAISER_REVIEW_START_DATE = app.APPRAISER_REVIEW_START_DATE,
                                          APPRAISER_REVIEW_END_DATE = app.APPRAISER_REVIEW_END_DATE,
                                          MGMT_REVIEW_START_DATE = app.MGMT_REVIEW_START_DATE,
                                          MGMT_REVIEW_END_DATE = app.MGMT_REVIEW_END_DATE,
                                          APPRAISAL_STATUS = app.APPRAISAL_STATUS == 1 ? true : false,
                                          DateOfJoining = app.DateOfJoining,
                                          EmployeesTypeId = app.EmployeesTypeId,
                                          DURATION_ID = app.DURATION_ID,
                                          CREATED_BY = app.CREATED_BY,
                                          CREATED_DATE = app.CREATED_DATE,
                                          UPDATED_BY = app.UPDATED_BY,
                                          UPDATED_DATE = app.UPDATED_DATE
                                      }
                      ).ToList();
            return lstAppraisalMasterView;
        }
        public List<AppraisalCycleMasterData> GetIndividualAppraisalDropdownList(int employeeID)
        {
            List<AppraisalCycleMasterData> appraisal = new List<AppraisalCycleMasterData>();
            appraisal = (from a in dbContext.AppraisalMaster
                         select new AppraisalCycleMasterData
                         {
                             APP_CYCLE_ID = a.APP_CYCLE_ID,
                             APP_CYCLE_NAME = a.APP_CYCLE_NAME,
                             ISACTIVE_APPRAISAL = a.APPRAISAL_STATUS == 1 ? true : false,
                             APPRAISEE_REVIEW_START_DATE = a.APPRAISEE_REVIEW_START_DATE,
                             APPRAISEE_REVIEW_END_DATE = a.APPRAISEE_REVIEW_END_DATE,
                             APPRAISER_REVIEW_START_DATE = a.APPRAISER_REVIEW_START_DATE,
                             APPRAISER_REVIEW_END_DATE = a.APPRAISER_REVIEW_END_DATE,
                             MGMT_REVIEW_START_DATE = a.MGMT_REVIEW_START_DATE,
                             MGMT_REVIEW_END_DATE = a.MGMT_REVIEW_END_DATE,
                             EMPLOYEE_DEPARTMENT_ID = dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == a.APP_CYCLE_ID && x.EMPLOYEE_ID == employeeID).Select(x => x.EMPLOYEE_DEPT_ID).FirstOrDefault(),
                             EMPLOYEE_ROLE_ID = dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == a.APP_CYCLE_ID && x.EMPLOYEE_ID == employeeID).Select(x => x.EMPLOYEE_ROLE_ID).FirstOrDefault(),
                         }).ToList();
                return appraisal;
        }

        public List<IndividualAppraisalObjKRAView> GetIndividualAppraisalDetailsByAppCycleId(int appcycleId, int departmentId, int roleId, int employeeId)
        {
            List<IndividualAppraisalObjKRAView> individualappraisalbenchmark = new List<IndividualAppraisalObjKRAView>();
            AppraisalMaster appmaster = new AppraisalMaster();
            appmaster = dbContext.AppraisalMaster.Where(x => x.APP_CYCLE_ID == appcycleId).Select(x => x).FirstOrDefault();
            EmployeeObjectiveRating employeeObjectiveRating = new EmployeeObjectiveRating();
            EmployeeKeyResultRating employeeKeyResultRating = new EmployeeKeyResultRating();
            List<int> objectiveList = dbContext.VersionKeyResults.Where(x => x.VERSION_ID == appmaster.VERSION_ID &&
            x.DEPT_ID == departmentId && x.ROLE_ID == roleId).GroupBy(x => x.OBJECTIVE_ID).OrderBy(x => x.Key).Select(x => x.Key).ToList();
            objectiveList = objectiveList == null ? new List<int>() : objectiveList;
            foreach (int objectiveId in objectiveList)
            {
                IndividualAppraisalObjKRAView ObjectiveRolemapping = new IndividualAppraisalObjKRAView();
                ObjectiveRolemapping.EntityId = appmaster?.ENTITY_ID;
                ObjectiveRolemapping.EntityName = dbContext.EntityMaster.Where(x => x.ENTITY_ID == appmaster.ENTITY_ID).Select(x => x.ENTITY_NAME).FirstOrDefault();
                ObjectiveRolemapping.ObjectiveId = objectiveId;
                ObjectiveRolemapping.ObjectiveName = dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_ID == objectiveId).Select(x => x.OBJECTIVE_NAME).FirstOrDefault();
                ObjectiveRolemapping.ObjectiveWeightage = dbContext.VersionDepartmentRoleObjective.Where(x => x.OBJECTIVE_ID == objectiveId && x.VERSION_ID == appmaster.VERSION_ID && x.ROLE_ID == roleId && x.DEPT_ID == departmentId).Select(x => x.OBJECTIVE_WEIGHTAGE).FirstOrDefault();
                ObjectiveRolemapping.ObjectiveDescription = dbContext.ObjectiveMaster.Where(x => x.OBJECTIVE_ID == objectiveId).Select(x => x.OBJECTIVE_DESCRIPTION).FirstOrDefault();

                /************************************************ Get Count Of Approved And Rejected Kra's *********************************************/
                employeeKeyResultRating = dbContext.EmployeeKeyResultRating.Where(x => x.OBJECTIVE_ID == objectiveId && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId).Select(x => x).FirstOrDefault();
                int ApprovedKeyResultCount = (from ekr in dbContext.EmployeeKeyResultRating
                                              join appConstants in dbContext.AppConstants on ekr.KEY_RESULT_STATUS equals appConstants.APP_CONSTANT_ID
                                              where ekr.OBJECTIVE_ID == objectiveId && ekr.EMPLOYEE_ID == employeeId && ekr.APP_CYCLE_ID == appcycleId && appConstants.APP_CONSTANT_TYPE_VALUE == "Approved"
                                              select new { ekr.KEY_RESULT_ID }).Distinct().Count();
                int RejectedKeyResultCount = (from ekr in dbContext.EmployeeKeyResultRating
                                              join appConstants in dbContext.AppConstants on ekr.KEY_RESULT_STATUS equals appConstants.APP_CONSTANT_ID
                                              where ekr.OBJECTIVE_ID == objectiveId && ekr.EMPLOYEE_ID == employeeId && ekr.APP_CYCLE_ID == appcycleId && appConstants.APP_CONSTANT_TYPE_VALUE == "Rejected"
                                              select new { ekr.KEY_RESULT_ID }).Distinct().Count();

                int ApprovedGroupKeyResultCount = (from egs in dbContext.EmployeeGroupSelection
                                                   join appConstants in dbContext.AppConstants on egs.INDIVIDUAL_KEYRES_STATUS equals appConstants.APP_CONSTANT_ID
                                                   where egs.OBJECTIVE_ID == objectiveId && egs.EMPLOYEE_ID == employeeId && egs.APP_CYCLE_ID == appcycleId && appConstants.APP_CONSTANT_TYPE_VALUE == "Approved"
                                                   select new { egs.KEY_RESULT_ID }).Distinct().Count();
                int RejectedGroupKeyResultCount = (from egs in dbContext.EmployeeGroupSelection
                                                   join appConstants in dbContext.AppConstants on egs.INDIVIDUAL_KEYRES_STATUS equals appConstants.APP_CONSTANT_ID
                                                   where egs.OBJECTIVE_ID == objectiveId && egs.EMPLOYEE_ID == employeeId && egs.APP_CYCLE_ID == appcycleId && appConstants.APP_CONSTANT_TYPE_VALUE == "Rejected"
                                                   select new { egs.KEY_RESULT_ID }).Distinct().Count();
                ObjectiveRolemapping.ApprovedKeyResultCount = ApprovedKeyResultCount + ApprovedGroupKeyResultCount;
                ObjectiveRolemapping.RejectedKeyResultCount = RejectedKeyResultCount + RejectedGroupKeyResultCount;

                /************************************************ Get Employee Self Appraisal Objective Ratings *********************************************/
                employeeObjectiveRating = dbContext.EmployeeObjectiveRating.Where(x => x.OBJECTIVE_ID == objectiveId && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId).Select(x => x).FirstOrDefault();
                ObjectiveRolemapping.ObjectiveRating = employeeObjectiveRating?.OBJECTIVE_RATING;
                ObjectiveRolemapping.ObjectiveMaxRating = employeeObjectiveRating?.OBJECTIVE_MAX_RATING;

                List<int> groupKRAList = dbContext.VersionKeyResultsGroupDetails.Where(vkr => vkr.VERSION_ID == appmaster.VERSION_ID && vkr.DEPT_ID == departmentId && vkr.ROLE_ID == roleId && vkr.OBJECTIVE_ID == objectiveId).Select(x => x.KEY_RESULT_ID).ToList();
                groupKRAList = groupKRAList == null ? new List<int>() : groupKRAList;
                List<IndividualKRAView> KRAsViews = (from mas in dbContext.KeyResultMaster
                                                     join vkr in dbContext.VersionKeyResults on mas.KEY_RESULT_ID equals vkr.KEY_RESULT_ID
                                                     where vkr.VERSION_ID == appmaster.VERSION_ID && vkr.DEPT_ID == departmentId && vkr.ROLE_ID == roleId && vkr.OBJECTIVE_ID == objectiveId
                                                     select new IndividualKRAView
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
                                                         Benchmark_Operator_Name = dbContext.AppConstants.Where(x => x.APP_CONSTANT_ID == vkr.BENCHMARK_OPERATOR).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
                                                         Benchmark_Type = vkr.BENCHMARK_TYPE,
                                                         Benchmark_Type_Name = dbContext.AppConstants.Where(x => x.APP_CONSTANT_ID == vkr.BENCHMARK_TYPE).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
                                                         Benchmark_UIType = vkr.BENCHMARK_UITYPE,
                                                         Benchmark_UIType_Name = dbContext.AppConstants.Where(x => x.APP_CONSTANT_ID == vkr.BENCHMARK_UITYPE).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
                                                         Benchmark_Value = vkr.BENCHMARK_VALUE,

                                                         /************************************************Get Employee Self Appraisal Group Key_Results Status*********************************************/
                                                         KEY_RESULT_STATUS = dbContext.EmployeeKeyResultRating.Where(x => x.OBJECTIVE_ID == vkr.OBJECTIVE_ID && x.KEY_RESULT_ID == vkr.KEY_RESULT_ID && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId).Select(x => x.KEY_RESULT_STATUS).FirstOrDefault(),
                                                         //KEY_RESULT_STATUS_NAME = dbContext.AppConstants.Where(x=>x.APP_CONSTANT_ID== dbContext.EmployeeKeyResultRating.Where(x => x.OBJECTIVE_ID == vkr.OBJECTIVE_ID && x.KEY_RESULT_ID == vkr.KEY_RESULT_ID && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId).Select(x => x.KEY_RESULT_STATUS).FirstOrDefault()).Select(x=>x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),                                           
                                                         KEY_RESULT_STATUS_NAME = dbContext.AppConstants.Join(dbContext.EmployeeKeyResultRating, e => e.APP_CONSTANT_ID, d => d.KEY_RESULT_STATUS, (x, y) => new { x, y }).Where(z => z.y.OBJECTIVE_ID == vkr.OBJECTIVE_ID && z.y.KEY_RESULT_ID == vkr.KEY_RESULT_ID && z.y.EMPLOYEE_ID == employeeId && z.y.APP_CYCLE_ID == appcycleId).Select(a => a.x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),

                                                         /************************************************ Get Employee Self Appraisal KRA Ratings *********************************************/
                                                         KEY_RESULT_ACTUAL_VALUE = dbContext.EmployeeKeyResultRating.Where(x => x.APP_CYCLE_ID == appcycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == vkr.OBJECTIVE_ID && x.KEY_RESULT_ID == vkr.KEY_RESULT_ID).Select(x => x.KEY_RESULT_ACTUAL_VALUE).FirstOrDefault(),
                                                         KEY_RESULT_MAX_RATING = dbContext.EmployeeKeyResultRating.Where(x => x.APP_CYCLE_ID == appcycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == vkr.OBJECTIVE_ID && x.KEY_RESULT_ID == vkr.KEY_RESULT_ID).Select(x => x.KEY_RESULT_MAX_RATING).FirstOrDefault(),
                                                         KEY_RESULT_RATING = dbContext.EmployeeKeyResultRating.Where(x => x.APP_CYCLE_ID == appcycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == vkr.OBJECTIVE_ID && x.KEY_RESULT_ID == vkr.KEY_RESULT_ID).Select(x => x.KEY_RESULT_RATING).FirstOrDefault(),

                                                         IndividualKRABenchmark = dbContext.VersionBenchMarks.Where(x => x.KEY_RESULT_ID == vkr.KEY_RESULT_ID &&
                                                          x.VERSION_ID == appmaster.VERSION_ID && x.DEPT_ID == departmentId &&
                                                          x.ROLE_ID == roleId && x.OBJECTIVE_ID == objectiveId).Select(x =>
                                                                 new IndividualKRABenchmark
                                                                 {
                                                                     BenchmarkId = x.BENCHMARK_ID,
                                                                     BenchmarkValue = x.BENCHMARK_VALUE,
                                                                     BenchmarkValue_Name = dbContext.AppConstants.Where(y => y.APP_CONSTANT_ID == x.BENCHMARK_VALUE).Select(y => y.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
                                                                     BenchmarkWeightage = x.BENCHMARK_WEIGHTAGE,
                                                                     RangeFrom = x.RANGE_FROM,
                                                                     RangeTo = x.RANGE_TO,
                                                                     BenchmarkSubjectiveName = x.BENCHMARK_SUBJECTIVE_NAME
                                                                 }).ToList(),

                                                         /************************************************Get Employee Uploaded Doc List*********************************************/
                                                         DocList = dbContext.EmployeeKeyResultAttachments.Where(x => x.OBJECTIVE_ID == vkr.OBJECTIVE_ID && x.KEY_RESULT_ID == vkr.KEY_RESULT_ID
                                                         && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId).Select(x => new DocList { KEY_RESULTS_DOC_ID = x.DOC_ID, KEY_RESULTS_DOC_NAME = x.DOC_NAME, DOC_TYPE = x.DOC_TYPE }).ToList(),

                                                         /************************************************Get Last Comment*********************************************/
                                                         kraComments = dbContext.EmployeeKeyResultConversation.Where(x => x.OBJECTIVE_ID == vkr.OBJECTIVE_ID && x.KEY_RESULT_ID == vkr.KEY_RESULT_ID
                                                         && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId && x.COMMENT != null).OrderByDescending(x => x.COMMENT_ID).Select(x => new KRAComments
                                                         { KEY_RESULTS_COMMENT = x.COMMENT, KEY_RESULTS_COMMENT_CREATEDBY = x.CREATED_BY }).FirstOrDefault(),

                                                         IS_DOCUMENT_MANDATORY = vkr.IS_DOCUMENT_MANDATORY == true ? true : false
                                                     }).ToList();
                ObjectiveRolemapping.IndividualKRAView = KRAsViews?.Where(x => !groupKRAList.Contains(x.KeyResult_Id)).Select(x => x).ToList();
                ObjectiveRolemapping.IndividualBenchmarkKeyResultGroup = new List<IndividualBenchmarkKeyResultGroup>();
                List<int> groupList = dbContext.VersionKeyResultsGroup.Where(x => x.VERSION_ID == appmaster.VERSION_ID &&
                x.DEPT_ID == departmentId && x.ROLE_ID == roleId && x.OBJECTIVE_ID == objectiveId).Select(x => x.KEY_RESULTS_GROUP_ID).ToList();
                groupList = groupList == null ? new List<int>() : groupList;
                foreach (int groupId in groupList)
                {
                    IndividualBenchmarkKeyResultGroup groupDetails = (from kraGroup in dbContext.VersionKeyResultsGroup
                                                                      join detail in dbContext.VersionKeyResultsGroupDetails on kraGroup.KEY_RESULTS_GROUP_ID equals detail.KEY_RESULTS_GROUP_ID
                                                                      join vkr in dbContext.VersionKeyResults on detail.KEY_RESULT_ID equals vkr.KEY_RESULT_ID
                                                                      where detail.VERSION_ID == appmaster.VERSION_ID && detail.DEPT_ID == departmentId &&
                                                                   detail.ROLE_ID == roleId && detail.OBJECTIVE_ID == objectiveId &&
                                                                   detail.KEY_RESULTS_GROUP_ID == groupId
                                                                      select new IndividualBenchmarkKeyResultGroup
                                                                      {
                                                                          VersionId = kraGroup.VERSION_ID,
                                                                          DeptId = kraGroup.DEPT_ID,
                                                                          RoleId = kraGroup.ROLE_ID,
                                                                          ObjectiveId = kraGroup.OBJECTIVE_ID,
                                                                          KeyResultGroupId = kraGroup.KEY_RESULTS_GROUP_ID,
                                                                          KeyResultGroupName = kraGroup.KEY_RESULTS_GROUP_NAME,
                                                                          KeyResultGroupWeightage = kraGroup.KEY_RESULT_GROUP_WEIGHTAGE,
                                                                          MandatoryKeyResultOption = kraGroup.MANDATORY_KEY_RESULT_OPTIONS,
                                                                          Benchmark_Type = kraGroup.MANDATORY_KEY_RESULT_OPTIONS,
                                                                          Benchmark_Type_Name = dbContext.AppConstants.Where(x => x.APP_CONSTANT_ID == kraGroup.MANDATORY_KEY_RESULT_OPTIONS).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
                                                                      }
                                  ).FirstOrDefault();
                    if (groupDetails != null)
                    {
                        List<int> groupKRA = new List<int>();
                        AppraisalEmployeeStatusView appraisalEmployeeStatusView = (from apm in dbContext.EmployeeAppraisalMaster
                                                                                   join ac in dbContext.AppConstants on apm.APPRAISAL_STATUS equals ac.APP_CONSTANT_ID
                                                                                   where apm.APP_CYCLE_ID == appcycleId && apm.EMPLOYEE_ID == employeeId && apm.EMPLOYEE_DEPT_ID == departmentId
                                                                                    && apm.EMPLOYEE_ROLE_ID == roleId
                                                                                   select new AppraisalEmployeeStatusView
                                                                                   {
                                                                                       ApprisalStatusID = apm.APPRAISAL_STATUS,
                                                                                       ApprisalStatus = ac.APP_CONSTANT_TYPE_VALUE,
                                                                                   }).FirstOrDefault();

                        if (appraisalEmployeeStatusView?.ApprisalStatus == "Self Appraisal Not Started" || appraisalEmployeeStatusView?.ApprisalStatus == "Self Appraisal In Progress" || appraisalEmployeeStatusView?.ApprisalStatus == "Self Appraisal Review Sent Back" || appraisalEmployeeStatusView?.ApprisalStatus == "Self-Appraisal in Progress-Feedback")
                        {
                            groupKRA = dbContext.VersionKeyResultsGroupDetails.Where(x => x.KEY_RESULTS_GROUP_ID == groupId &&
                           x.VERSION_ID == appmaster.VERSION_ID && x.ROLE_ID == roleId && x.DEPT_ID == departmentId &&
                           x.OBJECTIVE_ID == objectiveId).Select(x => x.KEY_RESULT_ID).ToList();
                            groupKRA = groupKRA == null ? new List<int>() : groupKRA;
                        }
                        else
                        {
                            groupKRA = dbContext.EmployeeGroupSelection.Where(x => x.KEY_RESULTS_GROUP_ID == groupId &&
                         x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId &&
                         x.OBJECTIVE_ID == objectiveId).Select(x => x.KEY_RESULT_ID).ToList();
                            groupKRA = groupKRA == null ? new List<int>() : groupKRA;
                        }

                        groupDetails.KeyResultDetail = KRAsViews?.Where(x => groupKRA.Contains(x.KeyResult_Id) &&
                        x.VersionId == appmaster.VERSION_ID && x.Role_Id == roleId && x.Dept_Id == departmentId &&
                           x.Objective_Id == objectiveId).Select(kra => new IndividualKRAView
                           {

                               KeyResult_Id = kra.KeyResult_Id,
                               KeyResult_Name = kra.KeyResult_Name,
                               Key_Result_Weightage = kra.Key_Result_Weightage,
                               IndividualKRABenchmark = kra.IndividualKRABenchmark,
                               Benchmark_Duration = kra.Benchmark_Duration,
                               Benchmark_From_Value = kra.Benchmark_From_Value,
                               Benchmark_Operator = kra.Benchmark_Operator,
                               Benchmark_Operator_Name = dbContext.AppConstants.Where(x => x.APP_CONSTANT_ID == kra.Benchmark_Operator).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
                               Benchmark_To_Value = kra.Benchmark_To_Value,
                               Benchmark_Type = kra.Benchmark_Type,
                               Benchmark_Type_Name = dbContext.AppConstants.Where(x => x.APP_CONSTANT_ID == kra.Benchmark_Type).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
                               Benchmark_Value = kra.Benchmark_Value,
                               Dept_Id = kra.Dept_Id,
                               Objective_Id = kra.Objective_Id,
                               Role_Id = kra.Role_Id,
                               Updated_By = kra.Updated_By,
                               VersionId = kra.VersionId,
                               Benchmark_UIType = kra.Benchmark_UIType,
                               IsSelected = dbContext.EmployeeGroupSelection.Where(x => x.APP_CYCLE_ID == appcycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == kra.Objective_Id && x.KEY_RESULT_ID == kra.KeyResult_Id).Select(x => x.KEY_RESULT_ID)?.Count() > 0 ? true : false,
                               Benchmark_UIType_Name = dbContext.AppConstants.Where(x => x.APP_CONSTANT_ID == kra.Benchmark_UIType).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),

                               /************************************************Get Employee Self Appraisal Group Key_Results Status*********************************************/
                               KEY_RESULT_STATUS = dbContext.EmployeeGroupSelection.Where(x => x.OBJECTIVE_ID == kra.Objective_Id && x.KEY_RESULT_ID == kra.KeyResult_Id && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId && x.KEY_RESULTS_GROUP_ID == groupId).Select(x => x.INDIVIDUAL_KEYRES_STATUS).FirstOrDefault(),
                               KEY_RESULT_STATUS_NAME = dbContext.AppConstants.Join(dbContext.EmployeeGroupSelection, e => e.APP_CONSTANT_ID, d => d.INDIVIDUAL_KEYRES_STATUS, (x, y) => new { x, y }).Where(z => z.y.OBJECTIVE_ID == kra.Objective_Id && z.y.KEY_RESULT_ID == kra.KeyResult_Id && z.y.EMPLOYEE_ID == employeeId && z.y.APP_CYCLE_ID == appcycleId && z.y.KEY_RESULTS_GROUP_ID == groupId).Select(a => a.x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),

                               /************************************************Get Employee Self Appraisal Key_Results Group Ratings*********************************************/
                               GRP_KEYRES_ACTUAL_VALUE = dbContext.EmployeeGroupSelection.Where(x => x.APP_CYCLE_ID == appcycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == kra.Objective_Id && x.KEY_RESULT_ID == kra.KeyResult_Id).Select(x => x.GRP_KEYRES_ACTUAL_VALUE).FirstOrDefault(),
                               INDIVIDUAL_GRPITEM_RATING = dbContext.EmployeeGroupSelection.Where(x => x.APP_CYCLE_ID == appcycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == kra.Objective_Id && x.KEY_RESULT_ID == kra.KeyResult_Id).Select(x => x.INDIVIDUAL_GRPITEM_RATING).FirstOrDefault(),
                               KEY_RESULTS_GROUP_RATING = dbContext.EmployeeGroupRating.Where(x => x.APP_CYCLE_ID == appcycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == kra.Objective_Id && x.KEY_RESULTS_GROUP_ID == groupId).Select(x => x.KEY_RESULTS_GROUP_RATING).FirstOrDefault(),
                               KEY_RESULTS_GROUP_MAX_RATING = dbContext.EmployeeGroupRating.Where(x => x.APP_CYCLE_ID == appcycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == kra.Objective_Id && x.KEY_RESULTS_GROUP_ID == groupId).Select(x => x.KEY_RESULTS_GROUP_MAX_RATING).FirstOrDefault(),

                               /************************************************Get Employee Uploaded Doc List*********************************************/
                               DocList = dbContext.EmployeeKeyResultAttachments.Where(x => x.OBJECTIVE_ID == kra.Objective_Id && x.KEY_RESULT_ID == kra.KeyResult_Id
                               && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId).Select(x => new DocList { KEY_RESULTS_DOC_ID = x.DOC_ID, KEY_RESULTS_DOC_NAME = x.DOC_NAME, DOC_TYPE = x.DOC_TYPE }).ToList(),

                               /************************************************Get Last Comment*********************************************/
                               kraComments = dbContext.EmployeeKeyResultConversation.Where(x => x.OBJECTIVE_ID == kra.Objective_Id && x.KEY_RESULT_ID == kra.KeyResult_Id
                               && x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == appcycleId && x.COMMENT != null).OrderByDescending(x => x.COMMENT_ID).Select(x => new KRAComments
                               { KEY_RESULTS_COMMENT = x.COMMENT, KEY_RESULTS_COMMENT_CREATEDBY = x.CREATED_BY }).FirstOrDefault(),

                               IS_DOCUMENT_MANDATORY = kra.IS_DOCUMENT_MANDATORY == true ? true : false
                           }).ToList();
                    }

                    ObjectiveRolemapping?.IndividualBenchmarkKeyResultGroup?.Add(groupDetails);
                }
                individualappraisalbenchmark?.Add(ObjectiveRolemapping);
            }
            individualappraisalbenchmark = individualappraisalbenchmark?.OrderByDescending(x => x.ObjectiveWeightage).Select(x => x).ToList();
            return individualappraisalbenchmark;
        }
        public bool AppraisalCycleNameDuplication(string appCycleName, int appCycleId)
        {
            //int appCycleId = dbContext.AppraisalMaster.Where(x => x.APP_CYCLE_NAME == appCycleName).Select(x=>x.APP_CYCLE_ID).FirstOrDefault();
            //if (appCycleId !=0)
            //    return true;
            //return false;
            bool isDuplicateName = false;
            string existName = dbContext.AppraisalMaster.Where(x => x.APP_CYCLE_NAME.ToLower() == appCycleName.ToLower() && (x.APP_CYCLE_ID == appCycleId || appCycleId == 0)).Select(x => x.APP_CYCLE_NAME).FirstOrDefault();
            if (appCycleId == 0 && existName != null)
            {
                isDuplicateName = true;
            }
            else if (appCycleId != 0 && existName?.ToLower() != appCycleName?.ToLower())
            {
                string newName = dbContext.AppraisalMaster.Where(x => x.APP_CYCLE_NAME.ToLower() == appCycleName.ToLower()).Select(x => x.APP_CYCLE_NAME).FirstOrDefault();
                if (newName != null)
                {
                    isDuplicateName = true;
                }
            }
            return isDuplicateName;
        }
        public bool checkEntityUsedAppCycle(int entityId)
        {
            AppraisalMaster appMaster = dbContext.AppraisalMaster.Where(x => x.ENTITY_ID == entityId).FirstOrDefault();
            return appMaster == null ? false : true;
        }
        public bool checkVersionUsedAppCycle(int versionId)
        {
            AppraisalMaster appMaster = dbContext.AppraisalMaster.Where(x => x.VERSION_ID == versionId).FirstOrDefault();
            return appMaster == null ? false : true;
        }
        public List<KeyWithValue> GetAppraisalDurationList()
        {
            return (from apptype in dbContext.AppConstantType
                    join appconstant in dbContext.AppConstants on apptype.APP_CONSTANT_TYPE_ID equals appconstant.APP_CONSTANT_TYPE_ID
                    where apptype.APP_CONSTANT_TYPE_DESC == "Duration"
                    select new KeyWithValue
                    {
                        Key = appconstant.APP_CONSTANT_ID,
                        Value = appconstant.APP_CONSTANT_TYPE_VALUE
                    }).ToList();
        }
        public int GetActiveAppCycleId()
        {
            return dbContext.AppraisalMaster.Where(x => x.APPRAISAL_STATUS == 1).Select(x=>x.APP_CYCLE_ID).FirstOrDefault();
        }
        //public bool AppraisalCycleNameDuplicationByExcel(string appCycleName, int appCycleId)
        //{
        //    List<AppraisalMaster> appCycleView = new List<AppraisalMaster>();
        //    if (appCycleId > 0)
        //    {
        //        appCycleView = dbContext.AppraisalMaster.Where(x => x.APP_CYCLE_NAME == appCycleName && x.APP_CYCLE_ID != appCycleId).ToList();
        //    }
        //    if (appCycleView.Count > 0)
        //        return true;
        //    return false;
        //}
    }
}
