using Appraisal.DAL.Repository;
using ExcelDataReader;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Appraisal.DAL.Services
{
    public class AppraisalServices
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAppraisalRepository _appraisalRepository;
        private readonly IAppraisalCycleRepository _appraisalCycleRepository;
        private readonly IAppraisalObjectiveRepository _appraisalObjectiveRepository;
        private readonly IAppraisalKeyResultRepository _appraisalKeyResultRepository;
        public readonly IVersionRepository _versionRepository;
        public readonly IVersionDepartmentRoleRepository _versionDepartmentRoleRepository;
        public readonly IVersionKeyResultsRepository _versionKeyResultsRepository;
        public readonly IVersionBenchmarksRepository _versionBenchmarksRepository;
        public readonly IEmployeeKeyResultsRatingRepository _employeeKeyResultsRatingRepository;
        public readonly IEmpObjectiveRatingRepository _empObjectiveRatingRepository;
        public readonly IEmployeeKResultCommentRepository _employeeKResultCommentRepository;
        public readonly IEmployeeAppraisalCommentRepository _employeeAppraisalCommentRepository;
        public readonly IAppConstantTypeRepository _appConstantTypeRepository;
        public readonly IAppConstantsRepository _appConstantsRepository;
        public readonly IVersionKeyResultsGroupRepository _versionKeyResultsGroupRepository;
        public readonly IVersionKeyResultsGroupDetailsRepository _versionKeyResultsGroupDetailsRepository;
        public readonly IEmployeeAppraisalMasterRepository _employeeAppraisalMasterRepository;
        public readonly IEmployeeKeyResultAttachmentRepository _employeekeyresultattachmentRepository;
        public readonly IVersionDepartmentRoleObjectiveRepository _versionDepartmentRoleObjectiveRepository;
        public readonly IEmployeeGroupSelectionRepository _employeeGroupSelectionRepository;
        public readonly IEmployeeGroupRatingRepository _employeeGroupRatingRepository;
        public readonly IAppBUHeadCommentsRepository _appBUHeadCommentsRepository;
        private readonly IWorkDayDetailRepository _workDayDetailRepository;

        #region Constructor
        public AppraisalServices(IAppraisalRepository appraisalRepository,
                                 IAppraisalCycleRepository appraisalCycleRepository,
                                 IAppraisalObjectiveRepository appraisalObjectiveRepository,
                                 IAppraisalKeyResultRepository appraisalKeyResultRepository,
                                 IVersionRepository versionRepository,
                                 IVersionDepartmentRoleRepository versionDepartmentRoleRepository,
                                 IVersionKeyResultsRepository versionKeyResultsRepository,
                                 IVersionBenchmarksRepository versionBenchmarksRepository,
                                 IEmployeeKeyResultsRatingRepository employeeKeyResultsRatingRepository,
                                 IEmpObjectiveRatingRepository empObjectiveRatingRepository,
                                 IEmployeeKResultCommentRepository employeeKResultCommentRepository,
                                 IEmployeeAppraisalCommentRepository employeeAppraisalCommentRepository,
                                 IAppConstantTypeRepository appConstantTypeRepository,
                                 IAppConstantsRepository appConstantsRepository,
                                 IVersionKeyResultsGroupRepository versionKeyResultsGroupRepository,
                                 IVersionKeyResultsGroupDetailsRepository versionKeyResultsGroupDetailsRepository,
                                 IEmployeeAppraisalMasterRepository employeeAppraisalMasterRepository,
                                 IEmployeeKeyResultAttachmentRepository empkeyresultattachmentRepository,
                                 IVersionDepartmentRoleObjectiveRepository versionDepartmentRoleObjectiveRepository,
                                 IEmployeeGroupSelectionRepository employeeGroupSelectionRepository,
                                 IEmployeeGroupRatingRepository employeeGroupRatingRepository,
                                 IAppBUHeadCommentsRepository appBUHeadCommentsRepository,
                                 IWorkDayDetailRepository workDayDetailRepository)
        {
            _appraisalRepository = appraisalRepository;
            _appraisalCycleRepository = appraisalCycleRepository;
            _appraisalObjectiveRepository = appraisalObjectiveRepository;
            _appraisalKeyResultRepository = appraisalKeyResultRepository;
            _versionRepository = versionRepository;
            _versionDepartmentRoleRepository = versionDepartmentRoleRepository;
            _versionKeyResultsRepository = versionKeyResultsRepository;
            _versionBenchmarksRepository = versionBenchmarksRepository;
            _employeeKeyResultsRatingRepository = employeeKeyResultsRatingRepository;
            _empObjectiveRatingRepository = empObjectiveRatingRepository;
            _employeeKResultCommentRepository = employeeKResultCommentRepository;
            _employeeAppraisalCommentRepository = employeeAppraisalCommentRepository;
            _appConstantTypeRepository = appConstantTypeRepository;
            _appConstantsRepository = appConstantsRepository;
            _versionKeyResultsGroupRepository = versionKeyResultsGroupRepository;
            _versionKeyResultsGroupDetailsRepository = versionKeyResultsGroupDetailsRepository;
            _employeeAppraisalMasterRepository = employeeAppraisalMasterRepository;
            _employeekeyresultattachmentRepository = empkeyresultattachmentRepository;
            _versionDepartmentRoleObjectiveRepository = versionDepartmentRoleObjectiveRepository;
            _employeeGroupSelectionRepository = employeeGroupSelectionRepository;
            _employeeGroupRatingRepository = employeeGroupRatingRepository;
            _appBUHeadCommentsRepository = appBUHeadCommentsRepository;
            _workDayDetailRepository = workDayDetailRepository;
        }
        #endregion

        #region Add or Update Entity
        public async Task<int> AddorUpdateEntity(EntityView entityView)
        {
            try
            {
                int length = 4;
                int entityId = 0;
                EntityMaster entityDetails = new EntityMaster();
                if (entityView.EntityId != 0) entityDetails = _appraisalRepository.GetByID(entityView.EntityId);
                if (entityDetails != null)
                {
                    entityDetails.ENTITY_NAME = entityView.EntityName;
                    //entityDetails.ENTITY_SHORT_NAME = entityView.EntityCode;
                    entityDetails.ENTITY_DESCRIPTION = entityView.Description;

                    if (entityView.EntityId == 0)
                    {
                        entityDetails.CREATED_DATE = DateTime.UtcNow;
                        entityDetails.CREATED_BY = entityView.CreatedBy;
                        await _appraisalRepository.AddAsync(entityDetails);
                        await _appraisalRepository.SaveChangesAsync();
                        entityId = entityDetails.ENTITY_ID;

                        entityDetails.ENTITY_SHORT_NAME = "Entity-" + entityId.ToString().PadLeft(length, '0');
                        _appraisalRepository.Update(entityDetails);
                        await _appraisalRepository.SaveChangesAsync();
                    }
                    else
                    {
                        entityDetails.UPDATED_DATE = DateTime.UtcNow;
                        entityDetails.UPDATED_BY = entityView.ModifiedBy;
                        _appraisalRepository.Update(entityDetails);
                        entityId = entityDetails.ENTITY_ID;
                        await _appraisalRepository.SaveChangesAsync();
                    }
                }
                return entityId;
            }
            catch (Exception)
            {
                //logger.Error(ex.Message.ToString());
                throw; ;
            }
        }
        #endregion

        #region Get All Entities
        public List<EntityMaster> GetAllEntityDetails()
        {
            return _appraisalRepository.GetAllEntityDetails();
        }
        #endregion

        #region Delete Entity
        public async Task<string> DeleteEntity(int entityId)
        {
            try
            {
                if (entityId > 0)
                {
                    if (_appraisalCycleRepository.checkEntityUsedAppCycle(entityId))
                    {
                        return "This entity mapped with appraisal cycle so system not allowed this operation";
                    }
                    else
                    {
                        EntityMaster entityMaster = _appraisalRepository.GetByID(entityId);
                        if (entityMaster != null && entityMaster.ENTITY_ID > 0)
                        {
                            _appraisalRepository.Delete(entityMaster);
                            await _appraisalRepository.SaveChangesAsync();
                            return "SUCCESS";
                        }
                    }
                }
            }
            catch (Exception)
            {
                //logger.Error(ex.Message.ToString());
                throw; ;
            }
            return "Unexpected error occurred. Try again!";
        }
        #endregion

        #region Add or Update Version
        public async Task<int> AddOrUpdateVersion(VersionView versionView)
        {
            try
            {
                int length = 4;
                int versionId = 0;
                VersionMaster versionDetails = new VersionMaster();
                if (versionView.VersionId != 0) versionDetails = _versionRepository.GetByID(versionView.VersionId);
                if (versionDetails != null)
                {
                    versionDetails.VERSION_NAME = versionView.VersionName;
                    //versionDetails.VERSION_CODE = versionView.VersionCode;
                    versionDetails.VERSION_DESC = versionView.Description;

                    if (versionView.VersionId == 0)
                    {
                        versionDetails.CREATED_DATE = DateTime.UtcNow;
                        versionDetails.CREATED_BY = versionView.CreatedBy;
                        await _versionRepository.AddAsync(versionDetails);
                        await _versionRepository.SaveChangesAsync();
                        versionId = versionDetails.VERSION_ID;

                        versionDetails.VERSION_CODE = "Version-" + versionId.ToString().PadLeft(length, '0');
                        _versionRepository.Update(versionDetails);
                        await _versionRepository.SaveChangesAsync();
                    }
                    else
                    {
                        versionDetails.UPDATED_DATE = DateTime.UtcNow;
                        versionDetails.UPDATED_BY = versionView.CreatedBy;
                        _versionRepository.Update(versionDetails);
                        versionId = versionDetails.VERSION_ID;
                        await _versionRepository.SaveChangesAsync();
                    }
                }
                return versionId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get All Versions
        public List<VersionMaster> GetAllVersionDetails()
        {
            return _versionRepository.GetAllVersionDetails();
        }
        #endregion

        #region Delete Version
        public async Task<string> DeleteVersion(int versionId)
        {
            try
            {
                if (versionId > 0)
                {
                    if (_appraisalCycleRepository.checkVersionUsedAppCycle(versionId))
                    {
                        return "This version mapped with appraisal cycle so system not allowed this operation";
                    }
                    else
                    {
                        VersionMaster versionMaster = _versionRepository.GetByID(versionId);
                        if (versionMaster != null && versionMaster.VERSION_ID > 0)
                        {
                            _versionRepository.Delete(versionMaster);
                            await _versionRepository.SaveChangesAsync();
                            return "SUCCESS";
                        }
                    }
                }
            }
            catch (Exception)
            {
                //logger.Error(ex.Message.ToString());
                throw; ;
            }
            return "Unexpected error occurred. Try again.";
        }
        #endregion

        #region Add or Update AppraisalCycle
        public async Task<int> AddorUpdateAppraisalCycle(AppraisalCycleView appraisalCycleView)
        {
            try
            {
                AppraisalMaster appraisalMaster = new AppraisalMaster();
                if (appraisalCycleView.AppCycleId != 0) appraisalMaster = _appraisalCycleRepository.GetByID(appraisalCycleView.AppCycleId);
                if (appraisalMaster != null)
                {
                    appraisalMaster.ENTITY_ID = appraisalCycleView.EntityId;
                    appraisalMaster.VERSION_ID = appraisalCycleView.VersionId;
                    appraisalMaster.APP_CYCLE_NAME = appraisalCycleView.AppCycleName;
                    appraisalMaster.APP_CYCLE_DESC = appraisalCycleView.AppCycleDesc;
                    appraisalMaster.APP_CYCLE_START_DATE = appraisalCycleView.AppCycleStartDate;
                    appraisalMaster.APP_CYCLE_END_DATE = appraisalCycleView.AppCycleEndDate;
                    appraisalMaster.APPRAISEE_REVIEW_START_DATE = appraisalCycleView.AppraiseeReviewStartDate;
                    appraisalMaster.APPRAISEE_REVIEW_END_DATE = appraisalCycleView.AppraiseeReviewEndDate;
                    appraisalMaster.APPRAISER_REVIEW_START_DATE = appraisalCycleView.AppraiserReviewStartDate;
                    appraisalMaster.APPRAISER_REVIEW_END_DATE = appraisalCycleView.AppraiserReviewEndDate;
                    appraisalMaster.MGMT_REVIEW_START_DATE = appraisalCycleView.MgmtReviewStartDate;
                    appraisalMaster.MGMT_REVIEW_END_DATE = appraisalCycleView.MgmtReviewEndDate;
                    appraisalMaster.DateOfJoining = appraisalCycleView.DateOfJoining;
                    appraisalMaster.EmployeesTypeId = appraisalCycleView.EmployeesTypeId;
                    appraisalMaster.DURATION_ID = appraisalCycleView.DurationId;
                }
                if (appraisalCycleView.AppCycleId == 0)
                {
                    appraisalMaster.APPRAISAL_STATUS = 0;
                    appraisalMaster.CREATED_DATE = DateTime.UtcNow;
                    appraisalMaster.CREATED_BY = appraisalCycleView.CreatedBy;
                    appraisalMaster.UPDATED_DATE = DateTime.UtcNow;
                    appraisalMaster.UPDATED_BY = appraisalCycleView.ModifiedBy;
                    await _appraisalCycleRepository.AddAsync(appraisalMaster);
                    await _appraisalCycleRepository.SaveChangesAsync();
                }
                else
                {
                    appraisalMaster.UPDATED_DATE = DateTime.UtcNow;
                    appraisalMaster.UPDATED_BY = appraisalCycleView.ModifiedBy;
                    _appraisalCycleRepository.Update(appraisalMaster);
                    await _appraisalCycleRepository.SaveChangesAsync();
                    if (appraisalCycleView.AppCycleId == _appraisalCycleRepository.GetActiveAppCycleId())
                    {
                        List<EmployeeAppraisalMaster> employeeList = _employeeAppraisalMasterRepository.GetAppCycleEmployeeListByAppCycleId(appraisalCycleView.AppCycleId);
                        foreach (EmployeeAppraisalMaster employeeMaster in employeeList)
                        {
                            employeeMaster.ENTITY_ID = appraisalCycleView.EntityId;
                            employeeMaster.UPDATED_BY = appraisalCycleView.ModifiedBy;
                            employeeMaster.UPDATED_DATE = DateTime.UtcNow;
                            _employeeAppraisalMasterRepository.Update(employeeMaster);
                            await _employeeAppraisalMasterRepository.SaveChangesAsync();
                        }
                    }
                }
                return appraisalMaster.APP_CYCLE_ID;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get All AppraisalCycle
        public List<AppraisalMasterView> GetAllAppraisalCycleDetails()
        {
            return _appraisalCycleRepository.GetAllAppraisalCycleDetails();
        }
        #endregion

        #region Delete AppraisalCycle
        public async Task<string> DeleteAppraisalCycle(int appCycleId)
        {
            try
            {
                if (appCycleId > 0)
                {
                    if (_employeeAppraisalMasterRepository.checkAppCycleUsedEmployeeMaster(appCycleId))
                    {
                        return "This appraisal cycle mapped with employees so system not allowed this operation";
                    }
                    else
                    {
                        AppraisalMaster appraisalMaster = _appraisalCycleRepository.GetByID(appCycleId);
                        if (appraisalMaster != null && appraisalMaster.APP_CYCLE_ID > 0)
                        {
                            _appraisalCycleRepository.Delete(appraisalMaster);
                            await _appraisalCycleRepository.SaveChangesAsync();
                            return "SUCCESS";
                        }
                    }
                }
            }
            catch (Exception)
            {
                //logger.Error(ex.Message.ToString());
                throw; ;
            }
            return "Unexpected error occurred. Try again.";
        }
        #endregion

        #region Update AppraisalCycle Status
        public async Task<int> UpdateAppraisalCycleStatus(UpdateAppraisalStatusView appraisalCycleView)
        {
            try
            {
                List<AppraisalMaster> appraisalMasterList = new List<AppraisalMaster>();
                if (appraisalCycleView.AppCycleId != 0) appraisalMasterList = _appraisalCycleRepository.GetAllAppraisalCycle();
                if (appraisalMasterList != null)
                {
                    if (appraisalCycleView.AppCycleId > 0)
                    {
                        foreach (AppraisalMaster item in appraisalMasterList)
                        {
                            item.APPRAISAL_STATUS = 0;
                            _appraisalCycleRepository.Update(item);
                            await _appraisalCycleRepository.SaveChangesAsync();
                        }
                        AppraisalMaster appraisalMaster = _appraisalCycleRepository.GetByID(appraisalCycleView.AppCycleId);
                        appraisalMaster.APPRAISAL_STATUS = appraisalCycleView.AppraisalStatus;
                        _appraisalCycleRepository.Update(appraisalMaster);
                        await _appraisalRepository.SaveChangesAsync();

                        if (appraisalCycleView?.EmployeeDetails?.Count > 0)
                        {
                            foreach (Employees employee in appraisalCycleView?.EmployeeDetails)
                            {
                                if (employee.DateOfJoining <= appraisalMaster.DateOfJoining && employee.EmployeeTypeId == appraisalMaster.EmployeesTypeId && employee.DateOfRelieving == null && employee.IsActive == true)
                                {
                                    EmployeeAppraisalMaster appCycleEmployee = _employeeAppraisalMasterRepository.GetAppCycleEmployee(appraisalMaster.APP_CYCLE_ID, employee.EmployeeID);
                                    if (appCycleEmployee == null)
                                    {
                                        appCycleEmployee = new EmployeeAppraisalMaster();
                                        appCycleEmployee.APPRAISAL_STATUS = _appConstantsRepository.GetAppraisalStatusId("Self Appraisal Not Started");
                                        appCycleEmployee.APP_CYCLE_ID = appraisalMaster.APP_CYCLE_ID;
                                        appCycleEmployee.EMPLOYEE_APPRAISER_RATING = 0;
                                        appCycleEmployee.EMPLOYEE_DEPT_ID = employee.DepartmentId == null ? 0 : (int)employee.DepartmentId;
                                        appCycleEmployee.EMPLOYEE_FINAL_RATING = 0;
                                        appCycleEmployee.EMPLOYEE_ID = employee.EmployeeID;
                                        appCycleEmployee.EMPLOYEE_MANAGER_ID = employee.ReportingManagerId == null ? 0 : (int)employee.ReportingManagerId;
                                        appCycleEmployee.EMPLOYEE_ROLE_ID = employee.RoleId == null ? 0 : (int)employee.RoleId;
                                        appCycleEmployee.EMPLOYEE_SELF_RATING = 0;
                                        appCycleEmployee.ENTITY_ID = appraisalMaster.ENTITY_ID == null ? 0 : (int)appraisalMaster.ENTITY_ID;
                                        appCycleEmployee.CREATED_BY = appraisalCycleView.UpdatedBy;
                                        appCycleEmployee.CREATED_DATE = DateTime.UtcNow;
                                        await _employeeAppraisalMasterRepository.AddAsync(appCycleEmployee);
                                        await _employeeAppraisalMasterRepository.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        appCycleEmployee.EMPLOYEE_DEPT_ID = employee.DepartmentId == null ? 0 : (int)employee.DepartmentId;
                                        appCycleEmployee.EMPLOYEE_MANAGER_ID = employee.ReportingManagerId == null ? 0 : (int)employee.ReportingManagerId;
                                        appCycleEmployee.EMPLOYEE_ROLE_ID = employee.RoleId == null ? 0 : (int)employee.RoleId;
                                        appCycleEmployee.ENTITY_ID = appraisalMaster.ENTITY_ID == null ? 0 : (int)appraisalMaster.ENTITY_ID;
                                        appCycleEmployee.UPDATED_BY = appraisalCycleView.UpdatedBy;
                                        appCycleEmployee.UPDATED_DATE = DateTime.UtcNow;
                                        _employeeAppraisalMasterRepository.Update(appCycleEmployee);
                                        await _employeeAppraisalMasterRepository.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
                return appraisalCycleView.AppCycleId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Add or Update Objective
        public async Task<int> AddorUpdateObjective(ObjectiveView objectiveView)
        {
            try
            {
                int length = 4;
                int objectiveId = 0;
                ObjectiveMaster objectiveDetails = new ObjectiveMaster();
                if (objectiveView.ObjectiveId != 0) objectiveDetails = _appraisalObjectiveRepository.GetByID(objectiveView.ObjectiveId);
                if (objectiveDetails != null)
                {
                    objectiveDetails.OBJECTIVE_NAME = objectiveView.ObjectiveName;
                    //objectiveDetails.OBJECTIVE_SHORT_NAME = objectiveView.ObjectiveCode;
                    objectiveDetails.OBJECTIVE_DESCRIPTION = objectiveView.ObjectiveDescription;

                    if (objectiveView.ObjectiveId == 0)
                    {
                        objectiveDetails.CREATED_DATE = DateTime.UtcNow;
                        objectiveDetails.CREATED_BY = objectiveView.CreatedBy;
                        await _appraisalObjectiveRepository.AddAsync(objectiveDetails);
                        await _appraisalObjectiveRepository.SaveChangesAsync();
                        objectiveId = objectiveDetails.OBJECTIVE_ID;

                        objectiveDetails.OBJECTIVE_SHORT_NAME = "Objective-" + objectiveId.ToString().PadLeft(length, '0');
                        _appraisalObjectiveRepository.Update(objectiveDetails);
                        await _appraisalObjectiveRepository.SaveChangesAsync();
                    }
                    else
                    {
                        objectiveDetails.UPDATED_DATE = DateTime.UtcNow;
                        objectiveDetails.UPDATED_BY = objectiveView.ModifiedBy;
                        _appraisalObjectiveRepository.Update(objectiveDetails);
                        objectiveId = objectiveDetails.OBJECTIVE_ID;
                        await _appraisalObjectiveRepository.SaveChangesAsync();
                    }
                }
                return objectiveId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get All Objective Details
        public List<ObjectiveMaster> GetAllObjectiveDetails()
        {
            return _appraisalObjectiveRepository.GetAllObjectiveDetails();
        }
        #endregion

        #region Delete Objective
        public async Task<string> DeleteObjective(int objectiveId)
        {
            try
            {
                if (objectiveId > 0)
                {
                    if (_versionKeyResultsRepository.CheckObjectiveUsedVersion(objectiveId))
                    {
                        return "This objective mapped with version so system not allowed this operation";
                    }
                    else
                    {
                        ObjectiveMaster objectiveMaster = _appraisalObjectiveRepository.GetByID(objectiveId);
                        if (objectiveMaster != null && objectiveMaster.OBJECTIVE_ID > 0)
                        {
                            _appraisalObjectiveRepository.Delete(objectiveMaster);
                            await _appraisalObjectiveRepository.SaveChangesAsync();
                            return "SUCCESS";
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw; ;
                //logger.Error(ex.Message.ToString());
            }
            return "Unexpected error occurred. Try again!";
        }
        #endregion

        #region Add or Update KeyResultMaster
        public async Task<int> AddOrUpdateKeyResultMaster(KeyResultMasterView keyResultMasterView)
        {
            try
            {
                int length = 4;
                int keyResultId = 0;
                KeyResultMaster keyResultDetails = new KeyResultMaster();
                if (keyResultMasterView.KeyResultId != 0) keyResultDetails = _appraisalKeyResultRepository.GetByID(keyResultMasterView.KeyResultId);
                if (keyResultDetails != null)
                {
                    keyResultDetails.KEY_RESULT_NAME = keyResultMasterView.KeyResultName;
                    //keyResultDetails.KEY_RESULT_SHORT_NAME = keyResultMasterView.KeyResultCode;
                    keyResultDetails.KEY_RESULT_DESCRIPTION = keyResultMasterView.KeyResultDescription;

                    if (keyResultMasterView.KeyResultId == 0)
                    {
                        keyResultDetails.CREATED_DATE = DateTime.UtcNow;
                        keyResultDetails.CREATED_BY = keyResultMasterView.CreatedBy;
                        await _appraisalKeyResultRepository.AddAsync(keyResultDetails);
                        await _appraisalKeyResultRepository.SaveChangesAsync();
                        keyResultId = keyResultDetails.KEY_RESULT_ID;

                        keyResultDetails.KEY_RESULT_SHORT_NAME = "KRA-" + keyResultId.ToString().PadLeft(length, '0');
                        _appraisalKeyResultRepository.Update(keyResultDetails);
                        await _appraisalKeyResultRepository.SaveChangesAsync();
                    }
                    else
                    {
                        keyResultDetails.UPDATED_DATE = DateTime.UtcNow;
                        keyResultDetails.UPDATED_BY = keyResultMasterView.ModifiedBy;
                        _appraisalKeyResultRepository.Update(keyResultDetails);
                        keyResultId = keyResultDetails.KEY_RESULT_ID;
                        await _appraisalKeyResultRepository.SaveChangesAsync();
                    }
                }
                return keyResultId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get All Key Result Details
        public List<KeyResultMaster> GetAllKeyResultDetails()
        {
            return _appraisalKeyResultRepository.GetAllKeyResultDetails();
        }
        #endregion

        #region Delete Key Result Master
        public async Task<string> DeleteKeyResultMaster(int keyResultId)
        {
            try
            {
                if (keyResultId > 0)
                {
                    if (_versionKeyResultsRepository.CheckKRAUsedVersion(keyResultId))
                    {
                        return "This KRA mapped with version so system not allowed this operation";
                    }
                    else
                    {
                        KeyResultMaster keyResultMaster = _appraisalKeyResultRepository.GetByID(keyResultId);
                        if (keyResultMaster != null && keyResultMaster.KEY_RESULT_ID > 0)
                        {
                            _appraisalKeyResultRepository.Delete(keyResultMaster);
                            await _appraisalKeyResultRepository.SaveChangesAsync();
                            return "SUCCESS";
                        }
                    }
                }
            }
            catch (Exception)
            {
                //logger.Error(ex.Message.ToString());
                throw; ;
            }
            return "Unexpected error occurred. Try again!";
        }
        #endregion

        #region Add or Update Department Role
        public async Task<int> AddOrUpdateDepartmentRoles(List<DepartmentRoleView> departmentRoleViews)
        {
            try
            {
                int versionId = 0;
                VersionDepartmentRoleMapping VersionDepartmentRole = new VersionDepartmentRoleMapping();
                if (departmentRoleViews != null)
                {
                    foreach (DepartmentRoleView departmentRole in departmentRoleViews)
                    {
                        foreach (var roles in departmentRole.RoleIds)
                        {
                            var roleexist = _versionDepartmentRoleRepository.GetByRoleID(departmentRole.VersionId, departmentRole.DepartmentId, roles.RoleId);
                            if (roleexist == null)
                            {
                                VersionDepartmentRole.VERSION_ID = departmentRole.VersionId;
                                VersionDepartmentRole.DEPT_ID = departmentRole.DepartmentId;
                                VersionDepartmentRole.ROLE_ID = roles.RoleId;
                                VersionDepartmentRole.CREATED_BY = departmentRole.CreatedBy;
                                VersionDepartmentRole.CREATED_DATE = DateTime.UtcNow;
                                await _versionDepartmentRoleRepository.AddAsync(VersionDepartmentRole);
                                await _versionDepartmentRoleRepository.SaveChangesAsync();
                            }
                        }
                        versionId = departmentRole.VersionId;
                    }
                }
                return versionId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get All Version Department & Role Mapping
        public VersionKRAMasterdata GetAllVersionDepartmentRoleMapping(int versionId)
        {
            VersionKRAMasterdata KRAMasterdata = new VersionKRAMasterdata()
            {
                VersionDepartmentRoleMapping = _versionDepartmentRoleRepository.GetVersionDepartmentRoleMapping(versionId),
                KeyResultMasters = _appraisalKeyResultRepository.GetAllKeyResultDetails(),
                ObjectiveMaster = _appraisalObjectiveRepository.GetAllObjectiveDetails()
            };
            return KRAMasterdata;
        }
        #endregion
        
        #region Get All Version Department & Role KRA Mapping
        public List<ObjectiveKRA> GetVersionDepartmentRoleKRAMapping(int versionId, int departmentId, int roleId)
        {
            return _appraisalObjectiveRepository.GetVersionDepartmentRoleKRAMapping(versionId, departmentId, roleId);
        }
        #endregion
        
        #region Add or Update Version Objective KRA
        public async Task<bool> AddOrUpdateVersionObjectiveKRA(List<VersionKeyResultsView> versionKeyResultsViews)
        {
            try
            {

                if (versionKeyResultsViews != null)
                {
                    foreach (var objectiveKRA in versionKeyResultsViews)
                    {

                        if (objectiveKRA?.ObjectiveId > 0)
                        {
                            VersionDepartmentRoleObjective objective = _versionDepartmentRoleObjectiveRepository.GetVersionDepartmentRoleObjectiveById(objectiveKRA.VersionId, objectiveKRA.DepartmentId, objectiveKRA.RoleId, objectiveKRA.ObjectiveId);
                            if (objective == null)
                            {
                                objective = new VersionDepartmentRoleObjective();
                                objective.VERSION_ID = objectiveKRA.VersionId;
                                objective.DEPT_ID = objectiveKRA.DepartmentId;
                                objective.ROLE_ID = objectiveKRA.RoleId;
                                objective.OBJECTIVE_ID = objectiveKRA.ObjectiveId;
                                objective.OBJECTIVE_WEIGHTAGE = objectiveKRA.ObjectiveWeightage;
                                objective.CREATED_BY = objectiveKRA.CreatedBy;
                                objective.CREATED_DATE = DateTime.UtcNow;
                                await _versionDepartmentRoleObjectiveRepository.AddAsync(objective);
                                await _versionDepartmentRoleObjectiveRepository.SaveChangesAsync();
                            }
                        }

                        foreach (var KRAIds in objectiveKRA.KeyResultIds)
                        {
                            VersionKeyResults VersionObjectiveKRA = _versionKeyResultsRepository.GetByKRAId(objectiveKRA.VersionId, objectiveKRA.DepartmentId, objectiveKRA.RoleId, objectiveKRA.ObjectiveId, KRAIds.KeyResultId);
                            if (VersionObjectiveKRA != null)
                            {
                                VersionObjectiveKRA.VERSION_ID = objectiveKRA.VersionId;
                                VersionObjectiveKRA.DEPT_ID = objectiveKRA.DepartmentId;
                                VersionObjectiveKRA.ROLE_ID = objectiveKRA.RoleId;
                                VersionObjectiveKRA.OBJECTIVE_ID = objectiveKRA.ObjectiveId;
                                VersionObjectiveKRA.KEY_RESULT_ID = KRAIds.KeyResultId;
                                VersionObjectiveKRA.UPDATED_BY = objectiveKRA.CreatedBy;
                                VersionObjectiveKRA.UPDATED_DATE = DateTime.UtcNow;
                                _versionKeyResultsRepository.Update(VersionObjectiveKRA);
                                await _versionKeyResultsRepository.SaveChangesAsync();
                            }
                            else
                            {
                                VersionObjectiveKRA = new VersionKeyResults();
                                VersionObjectiveKRA.VERSION_ID = objectiveKRA.VersionId;
                                VersionObjectiveKRA.DEPT_ID = objectiveKRA.DepartmentId;
                                VersionObjectiveKRA.ROLE_ID = objectiveKRA.RoleId;
                                VersionObjectiveKRA.OBJECTIVE_ID = objectiveKRA.ObjectiveId;
                                VersionObjectiveKRA.KEY_RESULT_ID = KRAIds.KeyResultId;
                                VersionObjectiveKRA.CREATED_BY = objectiveKRA.CreatedBy;
                                VersionObjectiveKRA.CREATED_DATE = DateTime.UtcNow;
                                VersionObjectiveKRA.UPDATED_DATE = DateTime.UtcNow;
                                VersionObjectiveKRA.UPDATED_BY = objectiveKRA.CreatedBy;
                                await _versionKeyResultsRepository.AddAsync(VersionObjectiveKRA);
                                await _versionKeyResultsRepository.SaveChangesAsync();
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion
        
        #region Get Version KRA Gridview Data
        public List<VersionKRABenchmarkGridDetails> GetVersionKRAGridviewData(int versionId)
        {
            return _appraisalKeyResultRepository.GetVersionKRAGridviewData(versionId);
        }
        #endregion

        #region Add or Update Version Benchmark Objective KRA
        public async Task<bool> AddOrUpdateVersionBenchmarkKRA(AddVersionBenchmarkView versionBenchmarkInsertViews)
        {
            try
            {
                if (versionBenchmarkInsertViews != null)
                {
                    if (versionBenchmarkInsertViews?.KeyResultDetail?.Count > 0)
                    {
                        foreach (var objectiveKRA in versionBenchmarkInsertViews?.KeyResultDetail)
                        {
                            VersionKeyResults VersionObjectiveKRA = new VersionKeyResults();
                            if (objectiveKRA != null) VersionObjectiveKRA = _versionKeyResultsRepository.GetByKRAId(objectiveKRA.VersionId, objectiveKRA.Dept_Id, objectiveKRA.Role_Id, objectiveKRA.Objective_Id, objectiveKRA.KeyResult_Id);
                            if (VersionObjectiveKRA != null)
                            {
                                VersionObjectiveKRA.VERSION_ID = objectiveKRA.VersionId;
                                VersionObjectiveKRA.DEPT_ID = objectiveKRA.Dept_Id;
                                VersionObjectiveKRA.ROLE_ID = objectiveKRA.Role_Id;
                                VersionObjectiveKRA.OBJECTIVE_ID = objectiveKRA.Objective_Id;
                                VersionObjectiveKRA.KEY_RESULT_ID = objectiveKRA.KeyResult_Id;
                                VersionObjectiveKRA.KEY_RESULT_WEIGHTAGE = objectiveKRA.Key_Result_Weightage;
                                VersionObjectiveKRA.BENCHMARK_TYPE = objectiveKRA.Benchmark_Type;
                                VersionObjectiveKRA.BENCHMARK_UITYPE = objectiveKRA.Benchmark_UIType;
                                VersionObjectiveKRA.BENCHMARK_DURATION = objectiveKRA.Benchmark_Duration;
                                VersionObjectiveKRA.BENCHMARK_OPERATOR = objectiveKRA.Benchmark_Operator;
                                VersionObjectiveKRA.BENCHMARK_VALUE = objectiveKRA.Benchmark_Value;
                                VersionObjectiveKRA.BENCHMARK_FROM_VALUE = objectiveKRA.Benchmark_From_Value;
                                VersionObjectiveKRA.BENCHMARK_TO_VALUE = objectiveKRA.Benchmark_To_Value;
                                VersionObjectiveKRA.UPDATED_BY = objectiveKRA.Updated_By;
                                VersionObjectiveKRA.UPDATED_DATE = DateTime.UtcNow;
                                VersionObjectiveKRA.IS_DOCUMENT_MANDATORY = objectiveKRA.Is_Document_Mandatory;
                                _versionKeyResultsRepository.Update(VersionObjectiveKRA);
                                await _versionKeyResultsRepository.SaveChangesAsync();
                            }
                        }
                    }
                    if (versionBenchmarkInsertViews?.VersionBenchmarkKeyResultGroup?.Count > 0)
                    {
                        foreach (VersionBenchmarkKeyResultGroup keyResultGroupDetail in versionBenchmarkInsertViews?.VersionBenchmarkKeyResultGroup)
                        {
                            VersionKeyResultsGroup keyResultGroup = new VersionKeyResultsGroup();
                            if (keyResultGroupDetail?.KeyResultGroupId > 0)
                            {
                                keyResultGroup = _versionKeyResultsGroupRepository.GetVersionKeyResultGroupById(keyResultGroupDetail.KeyResultGroupId);
                            }
                            if (keyResultGroup == null)
                            {
                                keyResultGroup = new VersionKeyResultsGroup();
                            }
                            keyResultGroup.VERSION_ID = keyResultGroupDetail.VersionId;
                            keyResultGroup.DEPT_ID = keyResultGroupDetail.DeptId;
                            keyResultGroup.ROLE_ID = keyResultGroupDetail.RoleId;
                            keyResultGroup.OBJECTIVE_ID = keyResultGroupDetail.ObjectiveId;
                            keyResultGroup.KEY_RESULTS_GROUP_NAME = keyResultGroupDetail.KeyResultGroupName;
                            keyResultGroup.MANDATORY_KEY_RESULT_OPTIONS = keyResultGroupDetail.MandatoryKeyResultOption;
                            keyResultGroup.KEY_RESULT_GROUP_WEIGHTAGE = keyResultGroupDetail.KeyResultGroupWeightage;
                            if (keyResultGroupDetail?.KeyResultGroupId > 0)
                            {
                                keyResultGroup.UPDATED_BY = keyResultGroupDetail.UpdatedBy == null ? 0 : keyResultGroupDetail.UpdatedBy;
                                keyResultGroup.UPDATED_DATE = DateTime.UtcNow;
                                _versionKeyResultsGroupRepository.Update(keyResultGroup);
                                await _versionKeyResultsGroupRepository.SaveChangesAsync();
                            }
                            else
                            {
                                keyResultGroup.CREATED_BY = keyResultGroupDetail.CreatedBy;
                                keyResultGroup.CREATED_DATE = DateTime.UtcNow;
                                keyResultGroup.UPDATED_DATE = null;
                                await _versionKeyResultsGroupRepository.AddAsync(keyResultGroup);
                                await _versionKeyResultsGroupRepository.SaveChangesAsync();
                            }
                            foreach (KeyResultDetailView keyResultDetail in keyResultGroupDetail?.KeyResultDetail)
                            {
                                VersionKeyResults VersionObjectiveKRA = new VersionKeyResults();
                                if (keyResultDetail != null) VersionObjectiveKRA = _versionKeyResultsRepository.GetByKRAId(keyResultDetail.VersionId, keyResultDetail.Dept_Id, keyResultDetail.Role_Id, keyResultDetail.Objective_Id, keyResultDetail.KeyResult_Id);
                                if (VersionObjectiveKRA != null)
                                {
                                    VersionObjectiveKRA.VERSION_ID = keyResultDetail.VersionId;
                                    VersionObjectiveKRA.DEPT_ID = keyResultDetail.Dept_Id;
                                    VersionObjectiveKRA.ROLE_ID = keyResultDetail.Role_Id;
                                    VersionObjectiveKRA.OBJECTIVE_ID = keyResultDetail.Objective_Id;
                                    VersionObjectiveKRA.KEY_RESULT_ID = keyResultDetail.KeyResult_Id;
                                    VersionObjectiveKRA.KEY_RESULT_WEIGHTAGE = keyResultDetail.Key_Result_Weightage;
                                    VersionObjectiveKRA.BENCHMARK_TYPE = keyResultDetail.Benchmark_Type;
                                    VersionObjectiveKRA.BENCHMARK_UITYPE = keyResultDetail.Benchmark_UIType;
                                    VersionObjectiveKRA.BENCHMARK_DURATION = keyResultDetail.Benchmark_Duration;
                                    VersionObjectiveKRA.BENCHMARK_OPERATOR = keyResultDetail.Benchmark_Operator;
                                    VersionObjectiveKRA.BENCHMARK_VALUE = keyResultDetail.Benchmark_Value;
                                    VersionObjectiveKRA.BENCHMARK_FROM_VALUE = keyResultDetail.Benchmark_From_Value;
                                    VersionObjectiveKRA.BENCHMARK_TO_VALUE = keyResultDetail.Benchmark_To_Value;
                                    VersionObjectiveKRA.UPDATED_BY = keyResultGroupDetail.UpdatedBy == null ? 0 : keyResultGroupDetail.UpdatedBy; //keyResultDetail.Updated_By;
                                    VersionObjectiveKRA.UPDATED_DATE = DateTime.UtcNow;
                                    VersionObjectiveKRA.IS_DOCUMENT_MANDATORY = keyResultDetail.Is_Document_Mandatory;
                                    _versionKeyResultsRepository.Update(VersionObjectiveKRA);
                                    await _versionKeyResultsRepository.SaveChangesAsync();
                                }
                                VersionKeyResultsGroupDetails keyResultGroupDetails = new VersionKeyResultsGroupDetails();
                                if (keyResultDetail != null)
                                {
                                    keyResultGroupDetails = _versionKeyResultsGroupDetailsRepository.GetVersionKeyResultGroupDetailsById(keyResultDetail.VersionId, keyResultDetail.Dept_Id, keyResultDetail.Role_Id, keyResultDetail.Objective_Id, keyResultGroup.KEY_RESULTS_GROUP_ID, keyResultDetail.KeyResult_Id);
                                    if (keyResultGroupDetails == null)
                                    {
                                        keyResultGroupDetails = new VersionKeyResultsGroupDetails();
                                        keyResultGroupDetails.VERSION_ID = keyResultDetail.VersionId;
                                        keyResultGroupDetails.DEPT_ID = keyResultDetail.Dept_Id;
                                        keyResultGroupDetails.ROLE_ID = keyResultDetail.Role_Id;
                                        keyResultGroupDetails.OBJECTIVE_ID = keyResultDetail.Objective_Id;
                                        keyResultGroupDetails.KEY_RESULTS_GROUP_ID = keyResultGroup.KEY_RESULTS_GROUP_ID;
                                        keyResultGroupDetails.KEY_RESULT_ID = keyResultDetail.KeyResult_Id;
                                        keyResultGroupDetails.CREATED_BY = keyResultGroupDetail.CreatedBy;
                                        keyResultGroupDetails.CREATED_DATE = DateTime.UtcNow;
                                        keyResultGroupDetails.UPDATED_DATE = null;
                                        await _versionKeyResultsGroupDetailsRepository.AddAsync(keyResultGroupDetails);
                                        await _versionKeyResultsGroupDetailsRepository.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Add or Update Version Benchmark KRA Range
        public async Task<bool> AddOrUpdateVersionBenchmarkKRARange(VersionBenchmarkRangeView benchmarkKRARangeViews)
        {
            try
            {
                if (benchmarkKRARangeViews != null)
                {
                    //Delete exising benchmark range
                    List<VersionBenchMarks> benchmarkList = _versionBenchmarksRepository.GetVersionKRABenchmarkRange(
                        benchmarkKRARangeViews.VersionId, benchmarkKRARangeViews.DepartmentId, benchmarkKRARangeViews.RoleId,
                        benchmarkKRARangeViews.ObjectiveId, benchmarkKRARangeViews.KeyResultId);
                    if (benchmarkList?.Count > 0)
                    {
                        foreach (VersionBenchMarks item in benchmarkList)
                        {
                            _versionBenchmarksRepository.Delete(item);
                            await _versionBenchmarksRepository.SaveChangesAsync();
                        }
                    }
                    //Add new benchmark range
                    foreach (KRABenchmark kraBenchmark in benchmarkKRARangeViews?.BenchmarkRange)
                    {
                        VersionBenchMarks versionObjKRA = new VersionBenchMarks();
                        versionObjKRA.VERSION_ID = benchmarkKRARangeViews.VersionId;
                        versionObjKRA.DEPT_ID = benchmarkKRARangeViews.DepartmentId;
                        versionObjKRA.ROLE_ID = benchmarkKRARangeViews.RoleId;
                        versionObjKRA.OBJECTIVE_ID = benchmarkKRARangeViews.ObjectiveId;
                        versionObjKRA.KEY_RESULT_ID = benchmarkKRARangeViews.KeyResultId;
                        versionObjKRA.RANGE_FROM = kraBenchmark.RangeFrom;
                        versionObjKRA.RANGE_TO = kraBenchmark.RangeTo;
                        versionObjKRA.BENCHMARK_VALUE = kraBenchmark.BenchmarkValue;
                        versionObjKRA.BENCHMARK_WEIGHTAGE = kraBenchmark.BenchmarkWeightage;
                        versionObjKRA.CREATED_BY = kraBenchmark.CreatedBy;
                        versionObjKRA.CREATED_DATE = DateTime.UtcNow;
                        await _versionBenchmarksRepository.AddAsync(versionObjKRA);
                        await _versionBenchmarksRepository.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get Ind appraisal objective and KRAs
        public AppraisalObjectiveandKRAListView GetIndAppraisalObjandKRAs(int appCycleId, int employeeId)
        {
            AppraisalObjectiveandKRAListView AppraisalObjectiveandKRAListView = new AppraisalObjectiveandKRAListView()
            {
                employeeObjectiveRatingsList = _empObjectiveRatingRepository.GetObjectiveByID(appCycleId, employeeId),
                employeeKeyResultRatingsList = _employeeKeyResultsRatingRepository.GetAllObjectiveKeyResults(appCycleId, employeeId)
            };
            return AppraisalObjectiveandKRAListView;
        }
        #endregion
        
        #region Add or Update Self appraisal KRA ratings
        public async Task<int> AddOrUpdateSelfAppraisalRating(List<EmployeeKRRatingView> employeeKRRatingViews)
        {
            try
            {
                int appCycleId = 0;
                EmployeeKeyResultRating KRARatings = new EmployeeKeyResultRating();

                if (employeeKRRatingViews != null)
                {
                    foreach (var objectiveKRA in employeeKRRatingViews)
                    {
                        if (objectiveKRA.Key_Result_Id != 0) KRARatings = _employeeKeyResultsRatingRepository.GetKeyResultdetail(objectiveKRA.APPCycle_Id, objectiveKRA.Employee_Id, objectiveKRA.Objective_Id, objectiveKRA.Key_Result_Id);
                        if (KRARatings != null)
                        {
                            KRARatings.APP_CYCLE_ID = objectiveKRA.APPCycle_Id;
                            KRARatings.EMPLOYEE_ID = objectiveKRA.Employee_Id;
                            KRARatings.OBJECTIVE_ID = objectiveKRA.Objective_Id;
                            KRARatings.KEY_RESULT_ID = objectiveKRA.Key_Result_Id;
                            KRARatings.KEY_RESULT_ACTUAL_VALUE = objectiveKRA.Key_Result_Actual_Value;
                            KRARatings.KEY_RESULT_MAX_RATING = objectiveKRA.Key_Result_Max_Rating;
                            KRARatings.KEY_RESULT_RATING = objectiveKRA.Key_Result_Rating;
                            KRARatings.KEY_RESULT_STATUS = objectiveKRA.Key_Result_Status;
                            KRARatings.IS_ADDRESSED = objectiveKRA.Is_Addressed;
                            if (objectiveKRA.Key_Result_Id == 0)
                            {
                                KRARatings.CREATED_BY = objectiveKRA.Created_By;
                                KRARatings.CREATED_DATE = DateTime.UtcNow;
                                await _employeeKeyResultsRatingRepository.AddAsync(KRARatings);
                                await _employeeKeyResultsRatingRepository.SaveChangesAsync();
                                appCycleId = KRARatings.APP_CYCLE_ID;
                            }
                            else
                            {
                                KRARatings.UPDATED_BY = objectiveKRA.Created_By;
                                KRARatings.UPDATED_DATE = DateTime.UtcNow;
                                _employeeKeyResultsRatingRepository.Update(KRARatings);
                                await _employeeKeyResultsRatingRepository.SaveChangesAsync();
                                appCycleId = KRARatings.KEY_RESULT_ID;
                            }
                        }
                    }
                }
                return appCycleId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Add Self appraisal KRA Comments
        public async Task<int> AddSelfAppraisalKRAComment(IndividualComments individualComments)
        {
            try
            {
                int commentId = 0;
                EmployeeKeyResultConversation KRAComment = new EmployeeKeyResultConversation();
                if (individualComments != null && individualComments.Comment != null)
                {
                    KRAComment.APP_CYCLE_ID = individualComments.AppCycleId;
                    KRAComment.EMPLOYEE_ID = individualComments.EmployeeId;
                    KRAComment.OBJECTIVE_ID = individualComments.ObjectiveId;
                    KRAComment.KEY_RESULT_ID = individualComments.KeyResultId;
                    KRAComment.COMMENT = individualComments.Comment;
                    if (individualComments.CommentId == 0)
                    {
                        KRAComment.CREATED_BY = individualComments.Created_By; //individualComments.EmployeeId;
                        KRAComment.CREATED_DATE = DateTime.UtcNow;
                        await _employeeKResultCommentRepository.AddAsync(KRAComment);
                        await _employeeKResultCommentRepository.SaveChangesAsync();
                        commentId = KRAComment.COMMENT_ID;
                    }
                }
                else
                {
                    return 0;
                }
                return commentId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Add appraisal Comments
        public async Task<int> AddSelfAppraisalComment(EmployeeAppraisalComment employeeAppraisalComment)
        {
            try
            {
                int commentId = 0;
                EmployeeAppraisalConversation appraisalComment = new EmployeeAppraisalConversation();

                if (employeeAppraisalComment.Comment != null && employeeAppraisalComment.Comment != string.Empty)
                {
                    if (employeeAppraisalComment.Comment_Id != 0) appraisalComment = _employeeAppraisalCommentRepository.GetAppraisalCommentById(employeeAppraisalComment.Comment_Id);
                    if (appraisalComment != null)
                    {
                        appraisalComment.APP_CYCLE_ID = employeeAppraisalComment.App_Cycle_Id;
                        appraisalComment.EMPLOYEE_ID = employeeAppraisalComment.Employee_Id;
                        appraisalComment.COMMENT = employeeAppraisalComment.Comment;

                        if (employeeAppraisalComment.Comment_Id == 0)
                        {
                            appraisalComment.CREATED_BY = employeeAppraisalComment.Created_By; //employeeAppraisalComment.Employee_Id;
                            appraisalComment.CREATED_DATE = DateTime.UtcNow;
                            await _employeeAppraisalCommentRepository.AddAsync(appraisalComment);
                            await _employeeAppraisalCommentRepository.SaveChangesAsync();
                            commentId = appraisalComment.COMMENT_ID;
                        }
                    }
                }
                else
                {
                    return 0;
                }
                return commentId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Update Self appraisal Manager KRA ratings
        public async Task<int> UpdateAppraisalManagerRating(ManagerRatingView managerRatingView)
        {
            try
            {
                int appCycleId = 0;
                List<EmployeeKeyResultRating> employeeKeyResultRating = new List<EmployeeKeyResultRating>();
                EmployeeKeyResultRating employeeKeyResults = new EmployeeKeyResultRating();
                if (managerRatingView != null)
                {
                    employeeKeyResultRating = _employeeKeyResultsRatingRepository.GetKeyResults(managerRatingView.APPCycle_Id, managerRatingView.Employee_Id, managerRatingView.Objective_Id);
                    if (employeeKeyResultRating != null)
                    {
                        foreach (var KRA in employeeKeyResultRating)
                        {
                            KRA.APP_CYCLE_ID = managerRatingView.APPCycle_Id;
                            KRA.EMPLOYEE_ID = managerRatingView.Employee_Id;
                            KRA.OBJECTIVE_ID = managerRatingView.Objective_Id;
                            KRA.KEY_RESULT_ID = KRA.KEY_RESULT_ID;
                            KRA.KEY_RESULT_STATUS = managerRatingView.Key_Result_Status;
                            KRA.UPDATED_BY = managerRatingView.Created_By;
                            KRA.UPDATED_DATE = DateTime.UtcNow;
                            KRA.IS_ADDRESSED = managerRatingView.Is_Addressed;
                            _employeeKeyResultsRatingRepository.Update(KRA);
                            await _employeeKeyResultsRatingRepository.SaveChangesAsync();
                            appCycleId = KRA.APP_CYCLE_ID;
                        }
                    }
                }
                return appCycleId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get Version Benchmark Master Data
        public BenchmarkMasterDataView GetVersionBenchmarkMasterData(int versionId)
        {
            BenchmarkMasterDataView benchmarkMasterData = _appConstantsRepository.GetAppConstants();
            benchmarkMasterData.VersionDepartmentRoleMapping = _versionDepartmentRoleRepository.GetVersionDepartmentRoleMapping(versionId);
            return benchmarkMasterData;
        }
        #endregion 
        
        #region Get Version role grid data
        public List<VersionRoleGridDetails> GetVersionRolesGridviewData(int versionId)
        {
            return _versionRepository.GetVersionRolesGridviewData(versionId);
        }
        #endregion 
        
        #region Get Version details by versionid
        public VersionDetailsView GetVersionDetailsById(int versionId)
        {
            return _versionRepository.GetVersionDetailsById(versionId);
        }
        #endregion
        
        #region Get Version KRA master data
        public VersionKRAMasterdata GetVersionKRAMasterData(int versionId)
        {
            VersionKRAMasterdata KRAMasterdata = new VersionKRAMasterdata()
            {
                VersionDepartmentRoleMapping = _versionDepartmentRoleRepository.GetVersionDepartmentRoleMapping(versionId),
                KeyResultMasters = _appraisalKeyResultRepository.GetAllKeyResultDetails(),
                ObjectiveMaster = _appraisalObjectiveRepository.GetAllObjectiveDetails()
            };
            return KRAMasterdata;
        }
        #endregion
        
        #region Get Version benchmark objective KRA
        public List<VersionBenchObjKRAView> GetVersionBenchmarkObjectiveKRA(int versionId, int departmentId, int roleId)
        {
            return _versionBenchmarksRepository.GetVersionBenchmarkObjectiveKRA(versionId, departmentId, roleId);
        }
        #endregion
        
        #region Get Version benchmark Gridview Data
        public List<VersionKRABenchmarkGridDetails> GetVersionBenchmarkGridData(int versionId)
        {
            return _appraisalKeyResultRepository.GetVersionBenchmarkGridData(versionId);
        }
        #endregion

        #region Get Department Wise Appraisal Details
        public AppraisalStatusReport getDepartmentWiseAppraisalDetails(int DepartmentID)
        {
            return _employeeAppraisalMasterRepository.getDepartmentWiseAppraisalDetails(DepartmentID);
        }
        #endregion

        #region Get Appraisal Status Report Count
        public AppraisalStatusReportCount getAppraisalStatusReportCount()
        {
            return _employeeAppraisalMasterRepository.getAppraisalStatusReportCount();
        }
        #endregion

        #region Get Employee Appraisal Details by Employee id
        public EmployeeAppraisalListView GetEmployeeAppraisalListByEmployeeId(EmployeeListAndDepartment empListDepartment)
        {
            EmployeeAppraisalListView empAppraislList = new EmployeeAppraisalListView();
            empAppraislList.EmployeeAppraisalMasterDetailView = _employeeAppraisalMasterRepository.GetEmployeeAppraisalListByEmployeeId(empListDepartment.employeeids);
            empAppraislList.AppraisalMilestonedetails = _employeeAppraisalMasterRepository.GetAppraisalMilestonedetails(0);
            empAppraislList.AppraisalBUHeadCommentsView = _appBUHeadCommentsRepository.GetAllComments(empListDepartment.departmentId);
            return empAppraislList;
        }
        #endregion

        #region delete benchmark KRA group
        public async Task<bool> deleteBenchmarkKRAGroup(int groupId)
        {
            List<VersionKeyResultsGroupDetails> benchmarkGroupDetails = _versionKeyResultsGroupDetailsRepository.GetVersionKeyResultGroupDetailsByGroupId(groupId);
            if (benchmarkGroupDetails?.Count > 0)
            {
                foreach (VersionKeyResultsGroupDetails groupDetail in benchmarkGroupDetails)
                {
                    _versionKeyResultsGroupDetailsRepository.Delete(groupDetail);
                    await _versionKeyResultsGroupDetailsRepository.SaveChangesAsync();
                }
            }
            VersionKeyResultsGroup benchmarkGroup = _versionKeyResultsGroupRepository.GetVersionKeyResultGroupById(groupId);
            if (benchmarkGroup != null)
            {
                _versionKeyResultsGroupRepository.Delete(benchmarkGroup);
                await _versionKeyResultsGroupRepository.SaveChangesAsync();
            }
            return true;
        }
        #endregion
        
        #region Get version KRA benchmark range
        public List<VersionBenchMarks> GetVersionKRABenchmarkRange(int versionId, int departmentId, int roleId, int objectiveId, int kraId)
        {
            return _versionBenchmarksRepository.GetVersionKRABenchmarkRange(versionId, departmentId, roleId, objectiveId, kraId);
        }
        #endregion
        
        #region Get KRA Comments By Id
        public List<KraComments> GetKRACommentsById(int appcycleId, int employeeId, int ObjId, int KraId)
        {
            return _employeeKResultCommentRepository.GetKRACommentsById(appcycleId, employeeId, ObjId, KraId);
        }
        #endregion

        #region Get Individual Appraisal DropdownList
        public List<AppraisalCycleMasterData> GetIndividualAppraisalDropdownList(int employeeID)
        {
            return _appraisalCycleRepository.GetIndividualAppraisalDropdownList(employeeID);
        }
        #endregion

        #region Get Individual Appraisal Details By AppCycle Id
        public IndividualAppraisalView GetIndividualAppraisalDetailsByAppCycleId(int appcycleId, int departmentId, int roleId, int employeeId)
        {
            IndividualAppraisalView individualAppraisalView = new IndividualAppraisalView();

            individualAppraisalView.IndividualAppraisalObjKRAView = _appraisalCycleRepository.GetIndividualAppraisalDetailsByAppCycleId(appcycleId, departmentId, roleId, employeeId);
            individualAppraisalView.AppraisalEmployeeStatusView = _employeeAppraisalMasterRepository.GetEmployeeAppraisalStatusById(appcycleId, departmentId, roleId, employeeId);
            individualAppraisalView.appraisalMilestonedetails = _employeeAppraisalMasterRepository.GetAppraisalMilestonedetails(appcycleId);
            individualAppraisalView.individualAppraisalCommentsViews = _employeeAppraisalCommentRepository.GetAppraisalCommentsById(appcycleId, employeeId);
            return individualAppraisalView;
        }
        #endregion

        #region Add Individual Appraisal Documents
        public async Task<bool> AddIndividualAppraisalDocuments(EmployeeKeyResultAttachmentsView IndividualDocuments)
        {
            try
            {
                string directoryPath = Path.Combine(IndividualDocuments.BaseDirectory, IndividualDocuments.SourceType, IndividualDocuments.APP_CYCLE_NAME.ToString(), IndividualDocuments.EMPLOYEE_ID.ToString(), IndividualDocuments.OBJECTIVE_NAME.ToString(), IndividualDocuments.KEY_RESULT_ID.ToString());
                //Upload files
                foreach (var document in IndividualDocuments.ListOfDocuments)
                {
                    EmployeeKeyResultAttachments Document = new EmployeeKeyResultAttachments
                    {
                        APP_CYCLE_ID = IndividualDocuments.APP_CYCLE_ID,
                        EMPLOYEE_ID = IndividualDocuments.EMPLOYEE_ID,
                        OBJECTIVE_ID = IndividualDocuments.OBJECTIVE_ID,
                        KEY_RESULT_ID = IndividualDocuments.KEY_RESULT_ID,
                        DOC_NAME = document.DOC_NAME,
                        DOC_TYPE = document.DOC_TYPE,
                        DOC_URL = directoryPath,
                        DOC_UPLOADED_BY = IndividualDocuments.EMPLOYEE_ID,
                        CREATED_BY = IndividualDocuments.EMPLOYEE_ID,
                        CREATED_DATE = DateTime.UtcNow,
                    };
                    await _employeekeyresultattachmentRepository.AddAsync(Document);
                    await _employeekeyresultattachmentRepository.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                //logger.Error(ex.Message.ToString());
                throw; ;
            }
            // return false;
        }
        #endregion

        #region Delete Individual Appraisal Document

        public async Task<bool> DeleteIndividualAppraisalDocument(int documentId)
        {
            try
            {
                if (documentId > 0)
                {
                    EmployeeKeyResultAttachments supDocument = _employeekeyresultattachmentRepository.GetByID(documentId);
                    if (supDocument?.DOC_ID > 0)
                    {
                        _employeekeyresultattachmentRepository.Delete(supDocument);
                        await _employeekeyresultattachmentRepository.SaveChangesAsync();

                        //Delete file from physical directory
                        string filePath = Path.Combine(supDocument.DOC_URL, supDocument.DOC_NAME);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                //logger.Error(ex.Message.ToString());
                throw; ;
            }
            return false;
        }
        #endregion
        
        #region Get appraisal documents by Id
        public List<EmployeeKeyResultAttachments> GetDocumentByObjAndKraId(AppraisalSourceDocuments sourceDocuments)
        {
            return _employeekeyresultattachmentRepository.GetDocumentByObjAndKraId(sourceDocuments);
        }
        #endregion

        #region Add or Update Individual Appraisal Rating
        public async Task<bool> AddorUpdateIndividualAppraisalRating(IndividualAppraisalAddView appraisalDetailsView)
        {
            try
            {
                EmployeeKeyResultRating EmployeeKeys = new EmployeeKeyResultRating();
                var details = appraisalDetailsView.EmployeeObjectiveRatings.FirstOrDefault();
                if (appraisalDetailsView != null)
                {
                    if (appraisalDetailsView?.EmployeeKeyResultRatings?.Count > 0)
                    {
                        foreach (var empkeyresult in appraisalDetailsView?.EmployeeKeyResultRatings)
                        {
                            EmployeeKeyResultRating EmployeeKey = new EmployeeKeyResultRating();

                            EmployeeKeyResultRating EmployeeKeyval = new EmployeeKeyResultRating();
                            if (empkeyresult != null) EmployeeKey = _employeeKeyResultsRatingRepository.GetKeyResultdetail(empkeyresult.APP_CYCLE_ID, empkeyresult.EMPLOYEE_ID, empkeyresult.OBJECTIVE_ID, empkeyresult.KEY_RESULT_ID);
                            if (EmployeeKey == null)
                            {
                                EmployeeKey = new EmployeeKeyResultRating();
                                EmployeeKey.APP_CYCLE_ID = empkeyresult.APP_CYCLE_ID;
                                EmployeeKey.EMPLOYEE_ID = empkeyresult.EMPLOYEE_ID;
                                EmployeeKey.OBJECTIVE_ID = empkeyresult.OBJECTIVE_ID;
                                EmployeeKey.KEY_RESULT_ID = empkeyresult.KEY_RESULT_ID;
                                EmployeeKey.KEY_RESULT_ACTUAL_VALUE = empkeyresult.KEY_RESULT_ACTUAL_VALUE;
                                EmployeeKey.KEY_RESULT_MAX_RATING = empkeyresult.KEY_RESULT_MAX_RATING;
                                EmployeeKey.KEY_RESULT_RATING = empkeyresult.KEY_RESULT_RATING;
                                EmployeeKey.CREATED_BY = empkeyresult.EMPLOYEE_ID;
                                EmployeeKey.CREATED_DATE = DateTime.UtcNow;
                                await _employeeKeyResultsRatingRepository.AddAsync(EmployeeKey);
                                await _employeeKeyResultsRatingRepository.SaveChangesAsync();
                            }
                            else
                            {
                                int? status = 0;
                                var KeyResultStatus = _appConstantsRepository.GetAppraisalStatusName(EmployeeKey.KEY_RESULT_STATUS);
                                if (KeyResultStatus == "Rejected" && appraisalDetailsView.IsSubmit == true)
                                {
                                    status = null;
                                }
                                else
                                {
                                    status = EmployeeKey.KEY_RESULT_STATUS;
                                }
                                EmployeeKey.APP_CYCLE_ID = empkeyresult.APP_CYCLE_ID;
                                EmployeeKey.EMPLOYEE_ID = empkeyresult.EMPLOYEE_ID;
                                EmployeeKey.OBJECTIVE_ID = empkeyresult.OBJECTIVE_ID;
                                EmployeeKey.KEY_RESULT_ID = empkeyresult.KEY_RESULT_ID;
                                EmployeeKey.KEY_RESULT_ACTUAL_VALUE = empkeyresult.KEY_RESULT_ACTUAL_VALUE;
                                EmployeeKey.KEY_RESULT_MAX_RATING = empkeyresult.KEY_RESULT_MAX_RATING;
                                EmployeeKey.KEY_RESULT_RATING = empkeyresult.KEY_RESULT_RATING;
                                EmployeeKey.KEY_RESULT_STATUS = status;
                                EmployeeKey.UPDATED_BY = empkeyresult.EMPLOYEE_ID;
                                EmployeeKey.UPDATED_DATE = DateTime.UtcNow;
                                _employeeKeyResultsRatingRepository.Update(EmployeeKey);
                                await _employeeKeyResultsRatingRepository.SaveChangesAsync();
                            }
                        }
                    }
                    if (appraisalDetailsView?.EmployeeObjectiveRatings?.Count > 0)
                    {
                        foreach (var empobjresult in appraisalDetailsView?.EmployeeObjectiveRatings)
                        {
                            EmployeeObjectiveRating employeeobj = new EmployeeObjectiveRating();
                            if (empobjresult != null) employeeobj = _empObjectiveRatingRepository.GetObjectiveRating(empobjresult.APP_CYCLE_ID, empobjresult.EMPLOYEE_ID, empobjresult.OBJECTIVE_ID);
                            if (employeeobj == null)
                            {
                                employeeobj = new EmployeeObjectiveRating();
                                employeeobj.APP_CYCLE_ID = empobjresult.APP_CYCLE_ID;
                                employeeobj.EMPLOYEE_ID = empobjresult.EMPLOYEE_ID;
                                employeeobj.OBJECTIVE_ID = empobjresult.OBJECTIVE_ID;
                                employeeobj.OBJECTIVE_MAX_RATING = empobjresult.OBJECTIVE_MAX_RATING;
                                employeeobj.OBJECTIVE_RATING = empobjresult.OBJECTIVE_RATING;
                                employeeobj.CREATED_BY = empobjresult.EMPLOYEE_ID;
                                employeeobj.CREATED_DATE = DateTime.UtcNow;
                                await _empObjectiveRatingRepository.AddAsync(employeeobj);
                                await _empObjectiveRatingRepository.SaveChangesAsync();
                            }
                            else
                            {
                                employeeobj.APP_CYCLE_ID = empobjresult.APP_CYCLE_ID;
                                employeeobj.EMPLOYEE_ID = empobjresult.EMPLOYEE_ID;
                                employeeobj.OBJECTIVE_ID = empobjresult.OBJECTIVE_ID;
                                employeeobj.OBJECTIVE_MAX_RATING = empobjresult.OBJECTIVE_MAX_RATING;
                                employeeobj.OBJECTIVE_RATING = empobjresult.OBJECTIVE_RATING;
                                employeeobj.UPDATED_BY = empobjresult.EMPLOYEE_ID;
                                employeeobj.UPDATED_DATE = DateTime.UtcNow;
                                _empObjectiveRatingRepository.Update(employeeobj);
                                await _empObjectiveRatingRepository.SaveChangesAsync();
                            }
                        }
                    }
                    if (appraisalDetailsView.EmployeeGroupSelections != null)
                    {
                        List<EmployeeGroupSelection> employeeGroupSelections = new List<EmployeeGroupSelection>();
                        if (appraisalDetailsView.EmployeeGroupSelections != null) employeeGroupSelections = _employeeGroupSelectionRepository.GetEmployeeGroupSelection(details.APP_CYCLE_ID, details.EMPLOYEE_ID);
                        if (employeeGroupSelections == null)
                        {
                            if (appraisalDetailsView?.EmployeeGroupSelections?.Count > 0)
                            {
                                foreach (var empgroupselresult in appraisalDetailsView?.EmployeeGroupSelections)
                                {
                                    EmployeeGroupSelection empgroupsel = new EmployeeGroupSelection
                                    {
                                        APP_CYCLE_ID = empgroupselresult.APP_CYCLE_ID,
                                        EMPLOYEE_ID = empgroupselresult.EMPLOYEE_ID,
                                        OBJECTIVE_ID = empgroupselresult.OBJECTIVE_ID,
                                        KEY_RESULT_ID = empgroupselresult.KEY_RESULT_ID,
                                        KEY_RESULTS_GROUP_ID = empgroupselresult.KEY_RESULTS_GROUP_ID,
                                        GRP_KEYRES_ACTUAL_VALUE = empgroupselresult.GRP_KEYRES_ACTUAL_VALUE,
                                        INDIVIDUAL_GRPITEM_RATING = empgroupselresult.INDIVIDUAL_GRPITEM_RATING,
                                        CREATED_BY = empgroupselresult.EMPLOYEE_ID,
                                        CREATED_DATE = DateTime.UtcNow,
                                        //IsSelected = empgroupselresult.IsSelected
                                    };
                                    await _employeeGroupSelectionRepository.AddAsync(empgroupsel);
                                    await _employeeGroupSelectionRepository.SaveChangesAsync();
                                }
                            }
                        }

                        else
                        {
                            List<int> oldkRAID = employeeGroupSelections.Select(x => x.KEY_RESULT_ID).ToList();
                            oldkRAID = oldkRAID == null ? new List<int>() : oldkRAID;
                            List<int> newkRAID = appraisalDetailsView?.EmployeeGroupSelections.Select(x => x.KEY_RESULT_ID).ToList();
                            newkRAID = newkRAID == null ? new List<int>() : newkRAID;
                            var results = employeeGroupSelections?.Where(m => !newkRAID.Contains(m.KEY_RESULT_ID)).ToList();
                            if (results?.Count > 0)
                            {
                                foreach (var item in results)
                                {
                                    List<EmployeeKeyResultAttachments> supDocument = _employeekeyresultattachmentRepository.GetDocumentByKraId(item.APP_CYCLE_ID, item.EMPLOYEE_ID, item.OBJECTIVE_ID, item.KEY_RESULT_ID);
                                    if (supDocument != null && supDocument.Count > 0)
                                    {
                                        foreach (var doc in supDocument)
                                        {
                                            _employeekeyresultattachmentRepository.Delete(doc);
                                            await _employeekeyresultattachmentRepository.SaveChangesAsync();
                                        }
                                    }
                                    List<EmployeeKeyResultConversation> kraComments = _employeeKResultCommentRepository.GetKRACommentsByKRAId(item.APP_CYCLE_ID, item.EMPLOYEE_ID, item.OBJECTIVE_ID, item.KEY_RESULT_ID);
                                    if (kraComments != null && kraComments.Count > 0)
                                    {
                                        foreach (var comment in kraComments)
                                        {
                                            _employeeKResultCommentRepository.Delete(comment);
                                            await _employeeKResultCommentRepository.SaveChangesAsync();
                                        }
                                    }

                                }
                            }
                            else
                            {

                            }

                            foreach (var item in employeeGroupSelections)
                            {
                                EmployeeGroupSelection groupSelection = _employeeGroupSelectionRepository.GetEmployeeGroupSelectionbyID(item.APP_CYCLE_ID, item.EMPLOYEE_ID, item.OBJECTIVE_ID, item.KEY_RESULT_ID, item.KEY_RESULTS_GROUP_ID);
                                if (groupSelection != null && groupSelection.KEY_RESULTS_GROUP_ID > 0)
                                {
                                    _employeeGroupSelectionRepository.Delete(groupSelection);
                                    await _employeeGroupSelectionRepository.SaveChangesAsync();
                                }
                            }
                            if (appraisalDetailsView?.EmployeeGroupSelections?.Count > 0)
                            {
                                foreach (var item in appraisalDetailsView.EmployeeGroupSelections)
                                {
                                    int? status = 0;
                                    var KeyResultStatus = _appConstantsRepository.GetAppraisalStatusName(item.INDIVIDUAL_KEYRES_STATUS);
                                    if (KeyResultStatus == "Rejected" && appraisalDetailsView.IsSubmit == true)
                                    {
                                        status = null;
                                    }
                                    else
                                    {
                                        status = item.INDIVIDUAL_KEYRES_STATUS;
                                    }
                                    EmployeeGroupSelection selection = new EmployeeGroupSelection
                                    {
                                        APP_CYCLE_ID = item.APP_CYCLE_ID,
                                        EMPLOYEE_ID = item.EMPLOYEE_ID,
                                        OBJECTIVE_ID = item.OBJECTIVE_ID,
                                        KEY_RESULT_ID = item.KEY_RESULT_ID,
                                        KEY_RESULTS_GROUP_ID = item.KEY_RESULTS_GROUP_ID,
                                        GRP_KEYRES_ACTUAL_VALUE = item.GRP_KEYRES_ACTUAL_VALUE,
                                        INDIVIDUAL_GRPITEM_RATING = item.INDIVIDUAL_GRPITEM_RATING,
                                        INDIVIDUAL_KEYRES_STATUS = status,
                                        CREATED_BY = item.EMPLOYEE_ID,
                                        CREATED_DATE = DateTime.UtcNow,
                                        //IsSelected = item.IsSelected
                                    };
                                    await _employeeGroupSelectionRepository.AddAsync(selection);
                                    await _employeeGroupSelectionRepository.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    if (appraisalDetailsView.EmployeeGroupRatings != null)
                    {
                        List<EmployeeGroupRating> employeeGroupRatings = new List<EmployeeGroupRating>();
                        if (appraisalDetailsView.EmployeeGroupRatings != null) employeeGroupRatings = _employeeGroupRatingRepository.GetEmployeeGroupRating(details.APP_CYCLE_ID, details.EMPLOYEE_ID);
                        if (employeeGroupRatings == null)
                        {
                            if (appraisalDetailsView?.EmployeeGroupRatings?.Count > 0)
                            {
                                foreach (var empgroupratingresult in appraisalDetailsView?.EmployeeGroupRatings)
                                {
                                    EmployeeGroupRating empgrouprating = new EmployeeGroupRating
                                    {
                                        APP_CYCLE_ID = empgroupratingresult.APP_CYCLE_ID,
                                        EMPLOYEE_ID = empgroupratingresult.EMPLOYEE_ID,
                                        OBJECTIVE_ID = empgroupratingresult.OBJECTIVE_ID,
                                        KEY_RESULTS_GROUP_ID = empgroupratingresult.KEY_RESULTS_GROUP_ID,
                                        KEY_RESULTS_GROUP_RATING = empgroupratingresult.KEY_RESULTS_GROUP_RATING,
                                        KEY_RESULTS_GROUP_MAX_RATING = empgroupratingresult.KEY_RESULTS_GROUP_MAX_RATING,
                                        CREATED_BY = empgroupratingresult.EMPLOYEE_ID,
                                        CREATED_DATE = DateTime.UtcNow,
                                    };
                                    await _employeeGroupRatingRepository.AddAsync(empgrouprating);
                                    await _employeeGroupRatingRepository.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in employeeGroupRatings)
                            {
                                EmployeeGroupRating employeeGroup = _employeeGroupRatingRepository.GetEmployeeGroupRatingbyID(item.APP_CYCLE_ID, item.EMPLOYEE_ID, item.OBJECTIVE_ID, item.KEY_RESULTS_GROUP_ID);
                                if (employeeGroup != null)
                                {
                                    _employeeGroupRatingRepository.Delete(employeeGroup);
                                    await _employeeGroupRatingRepository.SaveChangesAsync();
                                }
                            }
                            if (appraisalDetailsView?.EmployeeGroupRatings?.Count > 0)
                            {
                                foreach (var item in appraisalDetailsView.EmployeeGroupRatings)
                                {
                                    EmployeeGroupRating groupRating = new EmployeeGroupRating
                                    {

                                        APP_CYCLE_ID = item.APP_CYCLE_ID,
                                        EMPLOYEE_ID = item.EMPLOYEE_ID,
                                        OBJECTIVE_ID = item.OBJECTIVE_ID,
                                        KEY_RESULTS_GROUP_ID = item.KEY_RESULTS_GROUP_ID,
                                        KEY_RESULTS_GROUP_RATING = item.KEY_RESULTS_GROUP_RATING,
                                        KEY_RESULTS_GROUP_MAX_RATING = item.KEY_RESULTS_GROUP_MAX_RATING,
                                        UPDATED_BY = item.EMPLOYEE_ID,
                                        UPDATED_DATE = DateTime.UtcNow,
                                    };
                                    await _employeeGroupRatingRepository.AddAsync(groupRating);
                                    await _employeeGroupRatingRepository.SaveChangesAsync();
                                }
                            }

                        }
                    }

                    EmployeeAppraisalMaster AppraisalMaster = new EmployeeAppraisalMaster();
                    if (appraisalDetailsView.EntityID != 0 && details.EMPLOYEE_ID != 0 && details.APP_CYCLE_ID != 0)
                        AppraisalMaster = _employeeAppraisalMasterRepository.GetEmployeeAppraisalMasterByIDs(details.APP_CYCLE_ID, details.EMPLOYEE_ID, appraisalDetailsView.EntityID);
                    if (AppraisalMaster != null)
                    {

                        AppraisalMaster.APPRAISAL_STATUS = _appConstantsRepository.GetAppraisalStatusId(appraisalDetailsView.Status); //appraisalDetailsView.IsSubmit ? _appConstantsRepository.GetAppraisalStatusId("Self Appraisal Completed") : _appConstantsRepository.GetAppraisalStatusId("Self Appraisal In Progress");

                        AppraisalMaster.EMPLOYEE_SELF_RATING = appraisalDetailsView.overAllRating;
                        AppraisalMaster.EMPLOYEE_FINAL_RATING = appraisalDetailsView.overAllRating;
                        AppraisalMaster.EMPLOYEE_APPRAISER_RATING = appraisalDetailsView.overAllRating;
                        AppraisalMaster.UPDATED_BY = details.EMPLOYEE_ID;
                        AppraisalMaster.UPDATED_DATE = DateTime.UtcNow;
                        _employeeAppraisalMasterRepository.Update(AppraisalMaster);
                        await _employeeAppraisalMasterRepository.SaveChangesAsync();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get Appraisal Milestone Details
        public List<AppraisalMilestonedetails> GetAppraisalMilestonedetails(int appCycleId)
        {
            return _employeeAppraisalMasterRepository.GetAppraisalMilestonedetails(appCycleId);
        }
        #endregion

        #region Get Appraisal Objective Rating Details
        public AppraisalReport GetAppraisalObjectiveRatingDetails(int employeeId)
        {
            return _empObjectiveRatingRepository.GetAppraisalObjectiveRatingDetails(employeeId);
        }
        #endregion
        
        #region Get app cycle employee list
        public List<EmployeeAppraisalMasterDetailView> GetAllAppCycleEmployee(int appCycleId)
        {
            return _employeeAppraisalMasterRepository.GetAllAppCycleEmployee(appCycleId);
        }
        #endregion
        
        #region Delete app cycle employee
        public async Task<bool> DeleteAppCycleEmployee(int appCycleId, int employeeId)
        {
            EmployeeAppraisalMaster employeeMaster = _employeeAppraisalMasterRepository.GetAppCycleEmployee(appCycleId, employeeId);
            if (employeeMaster != null)
            {
                _employeeAppraisalMasterRepository.Delete(employeeMaster);
                await _employeeAppraisalMasterRepository.SaveChangesAsync();
            }
            return true;
        }
        #endregion
        
        #region Add app cycle employee
        public async Task<bool> AddAppCycleEmployee(List<EmployeeAppraisalMaster> appEmployeeList)
        {
            foreach (EmployeeAppraisalMaster appEmployee in appEmployeeList)
            {
                EmployeeAppraisalMaster appCycleEmployee = _employeeAppraisalMasterRepository.GetAppCycleEmployee(appEmployee.APP_CYCLE_ID, appEmployee.EMPLOYEE_ID);
                if (appCycleEmployee == null)
                {
                    appCycleEmployee = new EmployeeAppraisalMaster();
                    appCycleEmployee.APPRAISAL_STATUS = _appConstantsRepository.GetAppraisalStatusId("Self Appraisal Not Started");
                    appCycleEmployee.APP_CYCLE_ID = appEmployee.APP_CYCLE_ID;
                    appCycleEmployee.EMPLOYEE_APPRAISER_RATING = 0;
                    appCycleEmployee.EMPLOYEE_DEPT_ID = appEmployee.EMPLOYEE_DEPT_ID;
                    appCycleEmployee.EMPLOYEE_FINAL_RATING = 0;
                    appCycleEmployee.EMPLOYEE_ID = appEmployee.EMPLOYEE_ID;
                    appCycleEmployee.EMPLOYEE_MANAGER_ID = appEmployee.EMPLOYEE_MANAGER_ID;
                    appCycleEmployee.EMPLOYEE_ROLE_ID = appEmployee.EMPLOYEE_ROLE_ID;
                    appCycleEmployee.EMPLOYEE_SELF_RATING = 0;
                    appCycleEmployee.ENTITY_ID = appEmployee.ENTITY_ID;
                    appCycleEmployee.CREATED_BY = appEmployee.CREATED_BY;
                    appCycleEmployee.CREATED_DATE = DateTime.UtcNow;
                    await _employeeAppraisalMasterRepository.AddAsync(appCycleEmployee);
                    await _employeeAppraisalMasterRepository.SaveChangesAsync();
                }
            }
            return true;
        }
        #endregion
        
        #region Get Appraisal Comments By Id
        public List<IndividualAppraisalCommentsView> GetAppraisalCommentsById(int appcycleId, int employeeId)
        {
            return _employeeAppraisalCommentRepository.GetAppraisalCommentsById(appcycleId, employeeId);
        }
        #endregion

        #region Get App cycle detail by Id
        public AppraisalMaster GetAppCycleDetailById(int appCycleId)
        {
            return _appraisalCycleRepository.GetByID(appCycleId);
        }
        #endregion
        
        #region Check Entity name duplication
        public bool EntityNameDuplication(EntityView entityView)
        {
            return _appraisalRepository.EntityNameDuplication(entityView.EntityName, entityView.EntityId);
        }
        #endregion
        
        #region Check Objective name duplication
        public bool ObjectiveNameDuplication(ObjectiveView objectiveView)
        {
            return _appraisalObjectiveRepository.ObjectiveNameDuplication(objectiveView.ObjectiveName, objectiveView.ObjectiveId);
        }
        #endregion
        
        #region Check KRA name duplication
        public bool KRANameDuplication(KeyResultMasterView keyResultMasterView)
        {
            return _appraisalKeyResultRepository.KRANameDuplication(keyResultMasterView.KeyResultName, keyResultMasterView.KeyResultId);
        }
        #endregion
        
        #region Check Version name duplication
        public bool VersionNameDuplication(VersionView versionView)
        {
            return _versionRepository.VersionNameDuplication(versionView.VersionName, versionView.VersionId);
        }
        #endregion
        
        #region Check AppraisalCycle name duplication
        public bool AppraisalCycleNameDuplication(string appCycleName, int appcycleId)
        {
            return _appraisalCycleRepository.AppraisalCycleNameDuplication(appCycleName, appcycleId);
        }
        #endregion
        
        #region Delete Objective KRA Mapping
        public async Task<string> DeleteObjectiveKRAMapping(int versionId, int departmentId, int roleId, int objectiveId, int kRAId)
        {
            try
            {
                if (_appraisalCycleRepository.checkVersionUsedAppCycle(versionId))
                {
                    return "This version mapped with appraisal cycle so system not allowed this operation";
                }
                else
                {
                    //Delete KRA benchmark range
                    List<VersionBenchMarks> benchmarkList = _versionBenchmarksRepository.GetVersionKRABenchmarkRange(versionId, departmentId, roleId, objectiveId, kRAId);
                    if (benchmarkList?.Count > 0)
                    {
                        foreach (VersionBenchMarks item in benchmarkList)
                        {
                            _versionBenchmarksRepository.Delete(item);
                            await _versionBenchmarksRepository.SaveChangesAsync();
                        }
                    }
                    //Delete KRA mapping
                    VersionKeyResults versionKeyResul = _versionKeyResultsRepository.GetByKRAId(versionId, departmentId, roleId, objectiveId, kRAId);
                    if (versionKeyResul != null)
                    {
                        _versionKeyResultsRepository.Delete(versionKeyResul);
                        await _versionKeyResultsRepository.SaveChangesAsync();
                    }
                    return "SUCCESS";
                }
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion
        
        #region Delete Role Objective Mapping
        public async Task<string> DeleteRoleObjectiveMapping(int versionId, int departmentId, int roleId, int objectiveId)
        {
            try
            {
                if (_appraisalCycleRepository.checkVersionUsedAppCycle(versionId))
                {
                    return "This version mapped with appraisal cycle so system not allowed this operation";
                }
                else
                {
                    List<VersionKeyResults> KRAList = _versionKeyResultsRepository.GetByObjectiveId(versionId, departmentId, roleId, objectiveId);
                    if (KRAList?.Count > 0)
                    {
                        foreach (VersionKeyResults KRADetail in KRAList)
                        {
                            //Delete KRA benchmark range
                            List<VersionBenchMarks> benchmarkList = _versionBenchmarksRepository.GetVersionKRABenchmarkRange(versionId, departmentId, roleId, objectiveId, KRADetail.KEY_RESULT_ID);
                            if (benchmarkList?.Count > 0)
                            {
                                foreach (VersionBenchMarks item in benchmarkList)
                                {
                                    _versionBenchmarksRepository.Delete(item);
                                    await _versionBenchmarksRepository.SaveChangesAsync();
                                }
                            }
                            //Delete KRA mapping
                            VersionKeyResults versionKeyResul = _versionKeyResultsRepository.GetByKRAId(versionId, departmentId, roleId, objectiveId, KRADetail.KEY_RESULT_ID);
                            if (versionKeyResul != null)
                            {
                                _versionKeyResultsRepository.Delete(versionKeyResul);
                                await _versionKeyResultsRepository.SaveChangesAsync();
                            }
                        }
                        //Delete role objective mapping
                        VersionDepartmentRoleObjective versionDepartment = _versionDepartmentRoleObjectiveRepository.GetVersionDepartmentRoleObjectiveById(versionId, departmentId, roleId, objectiveId);
                        if (versionDepartment != null)
                        {
                            _versionDepartmentRoleObjectiveRepository.Delete(versionDepartment);
                            await _versionDepartmentRoleObjectiveRepository.SaveChangesAsync();
                        }
                    }
                    return "SUCCESS";
                }
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion
        
        #region Delete Department Role Mapping
        public async Task<string> DeleteDepartmentRoleMapping(int versionId, int departmentId, int roleId)
        {
            try
            {
                if (_appraisalCycleRepository.checkVersionUsedAppCycle(versionId))
                {
                    return "This version mapped with appraisal cycle so system not allowed this operation";
                }
                else
                {
                    List<VersionDepartmentRoleObjective> versionDepartmentObjectiveList = _versionDepartmentRoleObjectiveRepository.GetVersionDepartmentRoleById(versionId, departmentId, roleId);
                    if (versionDepartmentObjectiveList?.Count > 0)
                    {
                        foreach (VersionDepartmentRoleObjective versionDepartmentObjective in versionDepartmentObjectiveList)
                        {
                            List<VersionKeyResults> KRAList = _versionKeyResultsRepository.GetByObjectiveId(versionId, departmentId, roleId, versionDepartmentObjective.OBJECTIVE_ID);
                            if (KRAList?.Count > 0)
                            {
                                foreach (VersionKeyResults KRADetail in KRAList)
                                {
                                    //Delete KRA benchmark range
                                    List<VersionBenchMarks> benchmarkList = _versionBenchmarksRepository.GetVersionKRABenchmarkRange(versionId, departmentId, roleId, versionDepartmentObjective.OBJECTIVE_ID, KRADetail.KEY_RESULT_ID);
                                    if (benchmarkList?.Count > 0)
                                    {
                                        foreach (VersionBenchMarks item in benchmarkList)
                                        {
                                            _versionBenchmarksRepository.Delete(item);
                                            await _versionBenchmarksRepository.SaveChangesAsync();
                                        }
                                    }
                                    //Delete KRA mapping
                                    VersionKeyResults versionKeyResul = _versionKeyResultsRepository.GetByKRAId(versionId, departmentId, roleId, versionDepartmentObjective.OBJECTIVE_ID, KRADetail.KEY_RESULT_ID);
                                    if (versionKeyResul != null)
                                    {
                                        _versionKeyResultsRepository.Delete(versionKeyResul);
                                        await _versionKeyResultsRepository.SaveChangesAsync();
                                    }
                                }
                                //Delete role objective mapping
                                VersionDepartmentRoleObjective versionDepartmentObjectiveDetail = _versionDepartmentRoleObjectiveRepository.GetVersionDepartmentRoleObjectiveById(versionId, departmentId, roleId, versionDepartmentObjective.OBJECTIVE_ID);
                                if (versionDepartmentObjectiveDetail != null)
                                {
                                    _versionDepartmentRoleObjectiveRepository.Delete(versionDepartmentObjectiveDetail);
                                    await _versionDepartmentRoleObjectiveRepository.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    //Delete department role mapping
                    VersionDepartmentRoleMapping versionDepartmentRole = _versionDepartmentRoleRepository.GetByRoleID(versionId, departmentId, roleId);
                    if (versionDepartmentRole != null)
                    {
                        _versionDepartmentRoleRepository.Delete(versionDepartmentRole);
                        await _versionDepartmentRoleRepository.SaveChangesAsync();
                    }
                    return "SUCCESS";
                }
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion
        
        #region Delete Version Department Mapping
        public async Task<string> DeleteVersionDepartmentMapping(int versionId, int departmentId)
        {
            try
            {
                if (_appraisalCycleRepository.checkVersionUsedAppCycle(versionId))
                {
                    return "This version mapped with appraisal cycle so system not allowed this operation";
                }
                else
                {
                    List<VersionDepartmentRoleMapping> versionDepartmentRoleList = _versionDepartmentRoleRepository.GetByDepartmentID(versionId, departmentId);
                    if (versionDepartmentRoleList?.Count > 0)
                    {
                        foreach (VersionDepartmentRoleMapping versionDepartmentRoleDetail in versionDepartmentRoleList)
                        {
                            List<VersionDepartmentRoleObjective> versionDepartmentObjectiveList = _versionDepartmentRoleObjectiveRepository.GetVersionDepartmentRoleById(versionId, departmentId, versionDepartmentRoleDetail.ROLE_ID);
                            if (versionDepartmentObjectiveList?.Count > 0)
                            {
                                foreach (VersionDepartmentRoleObjective versionDepartmentObjective in versionDepartmentObjectiveList)
                                {
                                    List<VersionKeyResults> KRAList = _versionKeyResultsRepository.GetByObjectiveId(versionId, departmentId, versionDepartmentRoleDetail.ROLE_ID, versionDepartmentObjective.OBJECTIVE_ID);
                                    if (KRAList?.Count > 0)
                                    {
                                        foreach (VersionKeyResults KRADetail in KRAList)
                                        {
                                            //Delete KRA benchmark range
                                            List<VersionBenchMarks> benchmarkList = _versionBenchmarksRepository.GetVersionKRABenchmarkRange(versionId, departmentId, versionDepartmentRoleDetail.ROLE_ID, versionDepartmentObjective.OBJECTIVE_ID, KRADetail.KEY_RESULT_ID);
                                            if (benchmarkList?.Count > 0)
                                            {
                                                foreach (VersionBenchMarks item in benchmarkList)
                                                {
                                                    _versionBenchmarksRepository.Delete(item);
                                                    await _versionBenchmarksRepository.SaveChangesAsync();
                                                }
                                            }
                                            //Delete KRA mapping
                                            VersionKeyResults versionKeyResul = _versionKeyResultsRepository.GetByKRAId(versionId, departmentId, versionDepartmentRoleDetail.ROLE_ID, versionDepartmentObjective.OBJECTIVE_ID, KRADetail.KEY_RESULT_ID);
                                            if (versionKeyResul != null)
                                            {
                                                _versionKeyResultsRepository.Delete(versionKeyResul);
                                                await _versionKeyResultsRepository.SaveChangesAsync();
                                            }
                                        }
                                        //Delete role objective mapping
                                        VersionDepartmentRoleObjective versionDepartmentObjectiveDetail = _versionDepartmentRoleObjectiveRepository.GetVersionDepartmentRoleObjectiveById(versionId, departmentId, versionDepartmentRoleDetail.ROLE_ID, versionDepartmentObjective.OBJECTIVE_ID);
                                        if (versionDepartmentObjectiveDetail != null)
                                        {
                                            _versionDepartmentRoleObjectiveRepository.Delete(versionDepartmentObjectiveDetail);
                                            await _versionDepartmentRoleObjectiveRepository.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                            //Delete department role mapping
                            VersionDepartmentRoleMapping versionDepartmentRole = _versionDepartmentRoleRepository.GetByRoleID(versionId, departmentId, versionDepartmentRoleDetail.ROLE_ID);
                            if (versionDepartmentRole != null)
                            {
                                _versionDepartmentRoleRepository.Delete(versionDepartmentRole);
                                await _versionDepartmentRoleRepository.SaveChangesAsync();
                            }
                        }
                    }

                    return "SUCCESS";
                }
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get Employee Appraisal Master By ManagerID
        public List<EmployeeAppraisalByManager> GetEmployeeAppraisalMasterByManagerID(int appCycleID, int managerID)
        {
            return _employeeAppraisalMasterRepository.GetEmployeeAppraisalMasterByManagerID(appCycleID, managerID);
        }
        #endregion
        
        #region Get All App Constants
        public List<KeyWithValue> GetAppraisalDurationList()
        {
            return _appraisalCycleRepository.GetAppraisalDurationList();
        }
        #endregion
        
        #region Add or Update update Appraisal BU Head Comments
        public async Task<int> AddOrUpdateBUHeadComments(AppBUHeadCommentsView appBUHeadCommentsView)
        {
            try
            {
                int commentId = 0;
                AppraisalBUHeadComments appraisalBUHeadComments = new AppraisalBUHeadComments();
                int appCycleId = _employeeAppraisalMasterRepository.GetCurrentAppcycleId();
                if (appBUHeadCommentsView.AppraisalBUHeadCommentsId != 0) appraisalBUHeadComments = _appBUHeadCommentsRepository.GetByID(appBUHeadCommentsView.AppraisalBUHeadCommentsId);
                if (appraisalBUHeadComments != null && appCycleId>0)
                {
                    appraisalBUHeadComments.AppCycle_Id = appCycleId;
                    appraisalBUHeadComments.Department_Id = appBUHeadCommentsView.Department_Id;
                    appraisalBUHeadComments.Employee_Id = appBUHeadCommentsView.Employee_Id;
                    appraisalBUHeadComments.Comment = appBUHeadCommentsView.Comment;

                    if (appBUHeadCommentsView.AppraisalBUHeadCommentsId == 0)
                    {
                        appraisalBUHeadComments.Created_By = appBUHeadCommentsView.CreatedBy;
                        appraisalBUHeadComments.Created_On = DateTime.UtcNow;
                        await _appBUHeadCommentsRepository.AddAsync(appraisalBUHeadComments);
                        await _appBUHeadCommentsRepository.SaveChangesAsync();
                        commentId = appraisalBUHeadComments.AppraisalBUHeadCommentsId;
                    }
                    else
                    {
                        appraisalBUHeadComments.Updated_By = appBUHeadCommentsView.UpdatedBy;
                        appraisalBUHeadComments.Updated_On = DateTime.UtcNow;
                        _appBUHeadCommentsRepository.Update(appraisalBUHeadComments);
                        commentId = appraisalBUHeadComments.AppraisalBUHeadCommentsId;
                        await _appBUHeadCommentsRepository.SaveChangesAsync();
                    }
                }
                List<EmployeeAppraisalMaster> EmployeeList = new List<EmployeeAppraisalMaster>();
                if (commentId > 0 && appCycleId > 0)
                {
                    EmployeeList = _employeeAppraisalMasterRepository.GetEmployeeByDepartment(appCycleId, appBUHeadCommentsView.Department_Id);
                    if (EmployeeList?.Count > 0)
                    {
                        foreach (var item in EmployeeList)
                        {
                            if(appBUHeadCommentsView?.Employee_Id != item.EMPLOYEE_ID)
                            {
                                int? status = 0;
                                var employeeStatus = _appConstantsRepository.GetAppraisalStatusName(item.APPRAISAL_STATUS);
                                if (employeeStatus == "Appraiser Review Completed")
                                {
                                    status = _appConstantsRepository.GetAppraisalStatusId("Management Review Completed");
                                }
                                else
                                {
                                    status = item.APPRAISAL_STATUS;
                                }
                                item.APPRAISAL_STATUS = status;
                                item.IsBUHeadApproved = true;
                                item.UPDATED_BY = appBUHeadCommentsView.UpdatedBy;
                                item.UPDATED_DATE = DateTime.UtcNow;
                                _employeeAppraisalMasterRepository.Update(item);
                                await _employeeAppraisalMasterRepository.SaveChangesAsync();
                            }                            
                        }
                    }
                }
                return commentId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion
        
        #region Get Appraisal BU Head Comments
        public List<AppraisalBUHeadCommentsView> GetAppraisalBUHeadComments(int departmentId)
        {
            return _appBUHeadCommentsRepository.GetAllComments(departmentId);
        }
        #endregion
        
        #region Copy and Add Version Details
        public int CopyandAddVersionDetails(int versionId, int createdBy)
        {
            try
            {
                int version_Id = 0;
                if (versionId != 0) version_Id = _versionRepository.GetByVersionIdToCopy(versionId, createdBy);

                return version_Id;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get Employee Appraisal Status
        public AppraisalEmployeeStatusView GetEmployeeAppraisalStatusById(int appcycleId, int departmentId, int roleId, int employeeId)
        {
            return _employeeAppraisalMasterRepository.GetEmployeeAppraisalStatusById(appcycleId, departmentId, roleId, employeeId);
        }
        #endregion

        #region Approve Or Reject By Manager
        public async Task<string> ApproveOrRejectByManager(AddApproveandRejectByManagerView DetailsView)
        {
            EmployeeAppraisalMaster AppraisalMaster = new EmployeeAppraisalMaster();
            try
            {
                if (DetailsView != null)
                {
                    foreach (var empkeyresult in DetailsView?.EmployeeKeyResultRatingStatus)
                    {
                        EmployeeKeyResultRating EmployeeKey = new EmployeeKeyResultRating();
                        if (empkeyresult != null) EmployeeKey = _employeeKeyResultsRatingRepository.GetKeyResultdetail(empkeyresult.APP_CYCLE_ID, empkeyresult.EMPLOYEE_ID, empkeyresult.OBJECTIVE_ID, empkeyresult.KEY_RESULT_ID);
                        if (EmployeeKey != null)
                        {
                            //EmployeeKey = new EmployeeKeyResultRating();
                            EmployeeKey.APP_CYCLE_ID = empkeyresult.APP_CYCLE_ID;
                            EmployeeKey.EMPLOYEE_ID = empkeyresult.EMPLOYEE_ID;
                            EmployeeKey.OBJECTIVE_ID = empkeyresult.OBJECTIVE_ID;
                            EmployeeKey.KEY_RESULT_ID = empkeyresult.KEY_RESULT_ID;
                            EmployeeKey.KEY_RESULT_STATUS = empkeyresult.IsApproved ? _appConstantsRepository.GetAppraisalStatusId("Approved") : _appConstantsRepository.GetAppraisalStatusId("Rejected"); ;
                            EmployeeKey.UPDATED_BY = DetailsView.ManagerID;
                            EmployeeKey.UPDATED_DATE = DateTime.UtcNow;
                            _employeeKeyResultsRatingRepository.Update(EmployeeKey);
                            await _employeeKeyResultsRatingRepository.SaveChangesAsync();
                        }
                        else
                        {

                        }
                    }

                    foreach (var empgroupselect in DetailsView?.EmployeeGroupSelectionStatus)
                    {

                        EmployeeGroupSelection employeeGroup = new EmployeeGroupSelection();

                        if (empgroupselect != null) employeeGroup = _employeeGroupSelectionRepository.GetEmployeeGroupSelectionbyID(empgroupselect.APP_CYCLE_ID, empgroupselect.EMPLOYEE_ID, empgroupselect.OBJECTIVE_ID, empgroupselect.KEY_RESULT_ID, empgroupselect.KEY_RESULTS_GROUP_ID);
                        if (employeeGroup != null)
                        {
                            //EmployeeKey = new EmployeeKeyResultRating();
                            employeeGroup.APP_CYCLE_ID = empgroupselect.APP_CYCLE_ID;
                            employeeGroup.EMPLOYEE_ID = empgroupselect.EMPLOYEE_ID;
                            employeeGroup.OBJECTIVE_ID = empgroupselect.OBJECTIVE_ID;
                            employeeGroup.KEY_RESULT_ID = empgroupselect.KEY_RESULT_ID;
                            employeeGroup.INDIVIDUAL_KEYRES_STATUS = empgroupselect.IsApproved ? _appConstantsRepository.GetAppraisalStatusId("Approved") : _appConstantsRepository.GetAppraisalStatusId("Rejected"); ;
                            employeeGroup.UPDATED_BY = DetailsView.ManagerID;
                            employeeGroup.UPDATED_DATE = DateTime.UtcNow;
                            _employeeGroupSelectionRepository.Update(employeeGroup);
                            await _employeeKeyResultsRatingRepository.SaveChangesAsync();
                        }
                        else
                        {

                        }
                    }

                }

                //if (!DetailsView.IsSubmit)
                //{
                //    Status = _appConstantsRepository.GetAppraisalStatusId("Appraiser Review In Progress");
                //    StatusMsg = "Appraiser Review In Progress";
                //}

                //if (DetailsView.IsSubmit && employeeresultfalsecount == 0 && employeegroupfalsecount == 0)
                //{
                //    Status = _appConstantsRepository.GetAppraisalStatusId("Appraiser Review Completed");
                //    StatusMsg = "Appraiser Review Completed";
                //}
                //else if (DetailsView.IsSubmit && (employeeresultfalsecount != 0 || employeegroupfalsecount != 0))
                //{
                //    Status = _appConstantsRepository.GetAppraisalStatusId("Self Appraisal Review Sent Back");
                //    StatusMsg = "Self Appraisal Review Sent Back";
                //}

                //var values = DetailsView?.EmployeeKeyResultRatingStatus.FirstOrDefault();
                //if (values != null)
                //{
                //    AppraisalMaster = _employeeAppraisalMasterRepository.GetEmployeeAppraisalMasterByIDs(values.APP_CYCLE_ID, values.EMPLOYEE_ID, DetailsView.EntityID);
                //    AppraisalMaster.APPRAISAL_STATUS = _appConstantsRepository.GetAppraisalStatusId(DetailsView.Status); //Status;
                //    AppraisalMaster.UPDATED_BY = DetailsView.ManagerID;
                //    AppraisalMaster.UPDATED_DATE = DateTime.UtcNow;
                //    _employeeAppraisalMasterRepository.Update(AppraisalMaster);
                //    await _employeeAppraisalMasterRepository.SaveChangesAsync();
                //}
                AppraisalMaster = _employeeAppraisalMasterRepository.GetEmployeeAppraisalMasterByIDs(DetailsView.AppCycleId, DetailsView.EmployeeId, DetailsView.EntityID);
                if (AppraisalMaster != null)
                {
                    if (DetailsView.Status == "Appraiser Review Completed")
                    {
                        AppraisalMaster.IsRevertRating = false;
                    }
                    AppraisalMaster.APPRAISAL_STATUS = _appConstantsRepository.GetAppraisalStatusId(DetailsView.Status); //Status;
                    AppraisalMaster.UPDATED_BY = DetailsView.ManagerID;
                    AppraisalMaster.UPDATED_DATE = DateTime.UtcNow;
                    _employeeAppraisalMasterRepository.Update(AppraisalMaster);
                    await _employeeAppraisalMasterRepository.SaveChangesAsync();
                }
                return DetailsView.Status;
            }

            catch (Exception)
            {
                throw; ;
            }

        }
        #endregion
        
        #region Get Manager Details
        public List<ManagerDetail> GetManagerDetails(int appCycleID, int managerID)
        {
            return _employeeAppraisalMasterRepository.GetManagerDetails(appCycleID, managerID);
        }
        #endregion

        #region Add or Update AppraisalCycle
        public async Task<string> AddorUpdateAppraisalCycleByExcel(ImportExcelView import)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            if (!string.IsNullOrEmpty(import.Base64Format))
            {
                byte[] bytes = Convert.FromBase64String(import.Base64Format);
                MemoryStream stream = new MemoryStream(bytes);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet dataset = reader?.AsDataSet();
                    if (dataset?.Tables?.Count > 0 && dataset?.Tables[0]?.Rows?.Count > 1)
                    {
                        for (int i = 1; i < dataset?.Tables[0]?.Rows?.Count; i++)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim()))
                                {
                                    AppraisalMaster appraisalMaster = new AppraisalMaster();
                                    appraisalMaster = _appraisalCycleRepository.GetByName(dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim());
                                    if (appraisalMaster == null)
                                    {
                                        appraisalMaster = new AppraisalMaster();
                                        appraisalMaster.APP_CYCLE_NAME = dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim();
                                        appraisalMaster.ENTITY_ID = _appraisalRepository.GetEntityIdByName(dataset?.Tables[0]?.Rows[i][1]?.ToString()?.Trim());
                                        appraisalMaster.VERSION_ID = _versionRepository.GetVersionIdByName(dataset?.Tables[0]?.Rows[i][2]?.ToString()?.Trim());
                                        appraisalMaster.APP_CYCLE_START_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][3]?.ToString()?.Trim()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][3]?.ToString()?.Trim());
                                        appraisalMaster.APP_CYCLE_END_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][4]?.ToString()?.Trim()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][4]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISEE_REVIEW_START_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][5]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][5]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISEE_REVIEW_END_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][6]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][6]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISER_REVIEW_START_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][7]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][7]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISER_REVIEW_END_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][8]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][8]?.ToString()?.Trim());
                                        appraisalMaster.MGMT_REVIEW_START_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][9]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][9]?.ToString()?.Trim());
                                        appraisalMaster.MGMT_REVIEW_END_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][10]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][10]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISAL_STATUS = 0;
                                        appraisalMaster.DateOfJoining = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][11]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][11]?.ToString()?.Trim());
                                        int? employeeTypeId = dataset?.Tables[0]?.Rows[i][12] == null ? 0 : dataset?.Tables[0]?.Rows[i][12]?.ToString()?.Trim() == "" ? 0 : import.EmployeesTypes?.Where(x => x.EmployeesType.ToLower() == dataset?.Tables[0]?.Rows[i][12]?.ToString()?.Trim()?.ToLower()).Select(x => x.EmployeesTypeId).FirstOrDefault();
                                        appraisalMaster.EmployeesTypeId = employeeTypeId == null ? 0 : (int)employeeTypeId;
                                        appraisalMaster.DURATION_ID = dataset?.Tables[0]?.Rows[i][13] == null ? 0 : dataset?.Tables[0]?.Rows[i][13]?.ToString()?.Trim() == "" ? 0 : _appConstantsRepository.GetAppraisalStatusId(dataset?.Tables[0]?.Rows[i][13]?.ToString()?.Trim());
                                        appraisalMaster.APP_CYCLE_DESC = dataset?.Tables[0]?.Rows[i][14]?.ToString()?.Trim();
                                        appraisalMaster.CREATED_BY = 1;
                                        appraisalMaster.CREATED_DATE = DateTime.UtcNow;
                                        appraisalMaster.UPDATED_DATE = DateTime.UtcNow;
                                        appraisalMaster.UPDATED_BY = 1;
                                        await _appraisalCycleRepository.AddAsync(appraisalMaster);
                                        await _appraisalCycleRepository.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        //appraisalMaster.APP_CYCLE_NAME = dataset?.Tables[0]?.Rows[i][0]?.ToString();
                                        appraisalMaster.ENTITY_ID = _appraisalRepository.GetEntityIdByName(dataset?.Tables[0]?.Rows[i][1]?.ToString()?.Trim());
                                        appraisalMaster.VERSION_ID = _versionRepository.GetVersionIdByName(dataset?.Tables[0]?.Rows[i][2]?.ToString()?.Trim());
                                        appraisalMaster.APP_CYCLE_START_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][3]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][3]?.ToString()?.Trim());
                                        appraisalMaster.APP_CYCLE_END_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][4]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][4]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISEE_REVIEW_START_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][5]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][5]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISEE_REVIEW_END_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][6]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][6]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISER_REVIEW_START_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][7]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][7]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISER_REVIEW_END_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][8]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][8]?.ToString()?.Trim());
                                        appraisalMaster.MGMT_REVIEW_START_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][9]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][9]?.ToString()?.Trim());
                                        appraisalMaster.MGMT_REVIEW_END_DATE = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][10]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][10]?.ToString()?.Trim());
                                        appraisalMaster.APPRAISAL_STATUS = 0;
                                        appraisalMaster.DateOfJoining = string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][11]?.ToString()?.Trim()) ? null : Convert.ToDateTime(dataset?.Tables[0]?.Rows[i][11]?.ToString()?.Trim());
                                        int? employeeTypeId = dataset?.Tables[0]?.Rows[i][12] == null ? 0 : dataset?.Tables[0]?.Rows[i][12]?.ToString()?.Trim() == "" ? 0 : import.EmployeesTypes?.Where(x => x.EmployeesType.ToLower() == dataset?.Tables[0]?.Rows[i][12]?.ToString()?.Trim().ToLower()).Select(x => x.EmployeesTypeId).FirstOrDefault();
                                        appraisalMaster.EmployeesTypeId = employeeTypeId == null ? 0 : (int)employeeTypeId;
                                        appraisalMaster.DURATION_ID = dataset?.Tables[0]?.Rows[i][13] == null ? 0 : dataset?.Tables[0]?.Rows[i][13]?.ToString()?.Trim() == "" ? 0 : _appConstantsRepository.GetAppraisalStatusId(dataset?.Tables[0]?.Rows[i][13]?.ToString()?.Trim());
                                        appraisalMaster.APP_CYCLE_DESC = dataset?.Tables[0]?.Rows[i][14]?.ToString()?.Trim();
                                        appraisalMaster.UPDATED_BY = 1;
                                        appraisalMaster.UPDATED_DATE = DateTime.UtcNow;
                                        _appraisalCycleRepository.Update(appraisalMaster);
                                        await _appraisalRepository.SaveChangesAsync();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateAppraisalCycleByExcel");
                            }
                        }
                        return "SUCCESS";

                    }
                }
            }
            return "Import data's are empty, Please import valid data.";
        }
        #endregion

        #region Individual Appraisal Document Download By ID
        public EmployeeKeyResultAttachments IndividualAppraisalDocumentDownloadByID(int documentID)
        {
            EmployeeKeyResultAttachments documentDetails = new EmployeeKeyResultAttachments();
            try
            {
                if (documentID > 0)
                {
                    documentDetails = _employeekeyresultattachmentRepository.GetByID(documentID);
                }
            }
            catch (Exception)
            {
                throw; ;

            }
            return documentDetails;
        }
        #endregion

        #region Get Employee Appraisal Details by Manager
        public List<EmployeeAppraisalMasterDetailView> GetEmployeeAppraisalListByManager(List<int> employeeids)
        {
            return _employeeAppraisalMasterRepository.GetEmployeeAppraisalListByEmployeeId(employeeids);
        }
        #endregion

        #region Get Employee By Department
        public List<EmployeeAppraisalMaster> GetEmployeeByDepartment(int AppCycle_Id, int Department_Id)
        {
            return _employeeAppraisalMasterRepository.GetEmployeeByDepartment(AppCycle_Id, Department_Id);
        }
        #endregion

        #region Get Employee By Status
        public List<EmployeeAppraisalByManager> GetEmployeeByStatus(int AppCycle_Id, int Department_Id)
        {
            return _employeeAppraisalMasterRepository.GetEmployeeByStatus(AppCycle_Id, Department_Id);
        }
        #endregion

        #region BU Head Revert By Employee
        public async Task<string> BUHeadRevertByEmployee(BuHeadRevertByEmployeeView buHeadRevertByEmployee)
        {
            try
            {
                EmployeeAppraisalMaster employeeAppraisalMaster = new();
                employeeAppraisalMaster = _employeeAppraisalMasterRepository.GetEmployeeAppraisalMasterByAppandEmpandManIDs(buHeadRevertByEmployee.AppCycleID, buHeadRevertByEmployee.EmployeeID, buHeadRevertByEmployee.ManagerID);
                if(employeeAppraisalMaster != null)
                {
                    employeeAppraisalMaster.IsBUHeadRevert = true;
                    employeeAppraisalMaster.IsRevertRating = true;
                    _employeeAppraisalMasterRepository.Update(employeeAppraisalMaster);
                    await _employeeAppraisalMasterRepository.SaveChangesAsync();
                    return "SUCCESS";
                }
                else
                {
                    return "FAILURE";
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add or Update Version Using Excel
        public async Task<string> AddorUpdateVersionByExcel(ImportExcelView import)
        {
            if (!string.IsNullOrEmpty(import.Base64Format))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                byte[] bytes = Convert.FromBase64String(import.Base64Format);
                MemoryStream stream = new MemoryStream(bytes);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet dataset = reader?.AsDataSet();
                    if (dataset?.Tables?.Count > 0)
                    {
                        //Insert Version

                        DataTable versionTable = dataset?.Tables["version"];
                        if (versionTable?.Rows?.Count > 1)
                        {
                            for (int i = 1; i < versionTable?.Rows?.Count; i++)
                            {
                                try
                                {
                                    VersionView versionView = new VersionView();
                                    versionView.VersionName = versionTable.Rows[i][0]?.ToString()?.Trim();
                                    versionView.Description = versionTable.Rows[i][1]?.ToString()?.Trim();
                                    versionView.CreatedBy = 1;
                                    if (_versionRepository.VersionNameDuplication(versionView.VersionName, versionView.VersionId) == false)
                                    {
                                        int versionId = AddOrUpdateVersion(versionView).Result;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel", "Insert Version");
                                }

                            }
                        }
                        //Insert department role mapping
                        DataTable roleTable = dataset?.Tables["department_role_mapping"];
                        if (roleTable?.Rows?.Count > 1)
                        {
                            for (int i = 1; i < roleTable?.Rows?.Count; i++)
                            {
                                try
                                {
                                    int versionId = _versionRepository.GetVersionIdByName(roleTable.Rows[i][0]?.ToString()?.Trim());
                                    int? depId = import?.Department?.Where(x => x.DepartmentName?.ToLower() == roleTable.Rows[i][1]?.ToString()?.Trim()?.ToLower()).Select(x => x.DepartmentId).FirstOrDefault();
                                    int departmentId = depId == null ? 0 : (int)depId;
                                    int? role = import?.Roles?.Where(x => x.RoleName?.ToLower() == roleTable.Rows[i][2]?.ToString()?.Trim()?.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                                    int roleId = role == null ? 0 : (int)role;
                                    if (versionId > 0 && departmentId > 0 && roleId > 0)
                                    {
                                        VersionDepartmentRoleMapping existRole = _versionDepartmentRoleRepository.GetByRoleID(versionId, departmentId, roleId);
                                        if (existRole == null)
                                        {
                                            VersionDepartmentRoleMapping VersionDepartmentRole = new VersionDepartmentRoleMapping();
                                            VersionDepartmentRole.VERSION_ID = versionId;
                                            VersionDepartmentRole.DEPT_ID = departmentId;
                                            VersionDepartmentRole.ROLE_ID = roleId;
                                            VersionDepartmentRole.CREATED_BY = 1;
                                            VersionDepartmentRole.CREATED_DATE = DateTime.UtcNow;
                                            await _versionDepartmentRoleRepository.AddAsync(VersionDepartmentRole);
                                            await _versionDepartmentRoleRepository.SaveChangesAsync();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel", "Insert department role mapping");
                                }


                            }
                        }
                        //Insert objective mapping
                        DataTable objectiveTable = dataset?.Tables["objective_mapping"];
                        if (objectiveTable?.Rows?.Count > 1)
                        {
                            for (int i = 1; i < objectiveTable?.Rows?.Count; i++)
                            {
                                try
                                {
                                    int versionId = _versionRepository.GetVersionIdByName(objectiveTable.Rows[i][0]?.ToString()?.Trim());
                                    int? depId = import?.Department?.Where(x => x.DepartmentName?.ToLower() == objectiveTable.Rows[i][1]?.ToString()?.Trim()?.ToLower()).Select(x => x.DepartmentId).FirstOrDefault();
                                    int departmentId = depId == null ? 0 : (int)depId;
                                    int? role = import?.Roles?.Where(x => x.RoleName?.ToLower() == objectiveTable.Rows[i][2]?.ToString()?.Trim()?.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                                    int roleId = role == null ? 0 : (int)role;
                                    int objectiveId = _appraisalObjectiveRepository.GetObjectiveIdByName(objectiveTable.Rows[i][3]?.ToString()?.Trim());
                                    if (objectiveId == 0)
                                    {
                                        ObjectiveMaster objectiveDetails = new ObjectiveMaster();
                                        objectiveDetails.OBJECTIVE_NAME = objectiveTable.Rows[i][3]?.ToString()?.Trim();
                                        objectiveDetails.OBJECTIVE_DESCRIPTION = "";
                                        objectiveDetails.CREATED_DATE = DateTime.UtcNow;
                                        objectiveDetails.CREATED_BY = 1;
                                        await _appraisalObjectiveRepository.AddAsync(objectiveDetails);
                                        await _appraisalObjectiveRepository.SaveChangesAsync();
                                        objectiveId = objectiveDetails.OBJECTIVE_ID;
                                        objectiveDetails.OBJECTIVE_SHORT_NAME = "Objective-" + objectiveDetails.OBJECTIVE_ID.ToString().PadLeft(4, '0');
                                        _appraisalObjectiveRepository.Update(objectiveDetails);
                                        await _appraisalObjectiveRepository.SaveChangesAsync();
                                    }
                                    decimal weightage = 0;
                                    if (!string.IsNullOrEmpty(objectiveTable.Rows[i][4]?.ToString()?.Trim()))
                                    {
                                        weightage = Convert.ToDecimal(objectiveTable.Rows[i][4]?.ToString()?.Trim());
                                    }
                                    if (versionId > 0 && departmentId > 0 && roleId > 0 && objectiveId > 0 && weightage > 0)
                                    {
                                        VersionDepartmentRoleObjective objectiveDetail = _versionDepartmentRoleObjectiveRepository.GetVersionDepartmentRoleObjectiveById(versionId, departmentId, roleId, objectiveId);
                                        if (objectiveDetail == null)
                                        {
                                            VersionDepartmentRoleObjective objective = new VersionDepartmentRoleObjective();
                                            objective.VERSION_ID = versionId;
                                            objective.DEPT_ID = departmentId;
                                            objective.ROLE_ID = roleId;
                                            objective.OBJECTIVE_ID = objectiveId;
                                            objective.OBJECTIVE_WEIGHTAGE = weightage;
                                            objective.CREATED_BY = 1;
                                            objective.CREATED_DATE = DateTime.UtcNow;
                                            await _versionDepartmentRoleObjectiveRepository.AddAsync(objective);
                                            await _versionDepartmentRoleObjectiveRepository.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            objectiveDetail.OBJECTIVE_WEIGHTAGE = weightage;
                                            objectiveDetail.UPDATED_BY = 1;
                                            objectiveDetail.UPDATED_DATE = DateTime.UtcNow;
                                            _versionDepartmentRoleObjectiveRepository.Update(objectiveDetail);
                                            await _versionDepartmentRoleObjectiveRepository.SaveChangesAsync();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel", "Insert objective mapping");
                                }

                            }
                        }
                        //Insert KRA mapping
                        DataTable kraTable = dataset?.Tables["kra_mapping"];
                        if (kraTable?.Rows?.Count > 1)
                        {
                            for (int i = 1; i < kraTable?.Rows?.Count; i++)
                            {
                                try
                                {
                                    int versionId = _versionRepository.GetVersionIdByName(kraTable.Rows[i][0]?.ToString()?.Trim());
                                    int? depId = import?.Department?.Where(x => x.DepartmentName?.ToLower() == kraTable.Rows[i][1]?.ToString()?.Trim()?.ToLower()).Select(x => x.DepartmentId).FirstOrDefault();
                                    int departmentId = depId == null ? 0 : (int)depId;
                                    int? role = import?.Roles?.Where(x => x.RoleName?.ToLower() == kraTable.Rows[i][2]?.ToString()?.Trim()?.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                                    int roleId = role == null ? 0 : (int)role;
                                    int objectiveId = _appraisalObjectiveRepository.GetObjectiveIdByName(kraTable.Rows[i][3]?.ToString()?.Trim());
                                    int kraId = _appraisalKeyResultRepository.GetKRAIdByName(kraTable.Rows[i][4]?.ToString()?.Trim());
                                    if (kraId == 0)
                                    {
                                        KeyResultMaster keyResultDetails = new KeyResultMaster();
                                        keyResultDetails.KEY_RESULT_NAME = kraTable.Rows[i][4]?.ToString()?.Trim();
                                        keyResultDetails.KEY_RESULT_DESCRIPTION = "";
                                        keyResultDetails.CREATED_DATE = DateTime.UtcNow;
                                        keyResultDetails.CREATED_BY = 1;
                                        await _appraisalKeyResultRepository.AddAsync(keyResultDetails);
                                        await _appraisalKeyResultRepository.SaveChangesAsync();
                                        kraId = keyResultDetails.KEY_RESULT_ID;
                                        keyResultDetails.KEY_RESULT_SHORT_NAME = "KRA-" + kraId.ToString().PadLeft(4, '0');
                                        _appraisalKeyResultRepository.Update(keyResultDetails);
                                        await _appraisalKeyResultRepository.SaveChangesAsync();
                                    }
                                    if (versionId > 0 && departmentId > 0 && roleId > 0 && objectiveId > 0 && kraId > 0)
                                    {
                                        VersionKeyResults VersionObjectiveKRA = new VersionKeyResults();
                                        VersionObjectiveKRA = _versionKeyResultsRepository.GetByKRAId(versionId, departmentId, roleId, objectiveId, kraId);
                                        if (VersionObjectiveKRA == null)
                                        {
                                            VersionObjectiveKRA = new VersionKeyResults();
                                            VersionObjectiveKRA.VERSION_ID = versionId;
                                            VersionObjectiveKRA.DEPT_ID = departmentId;
                                            VersionObjectiveKRA.ROLE_ID = roleId;
                                            VersionObjectiveKRA.OBJECTIVE_ID = objectiveId;
                                            VersionObjectiveKRA.KEY_RESULT_ID = kraId;
                                            VersionObjectiveKRA.KEY_RESULT_WEIGHTAGE = kraTable.Rows[i][5] == null ? 0 : kraTable.Rows[i][5]?.ToString()?.Trim() == "" ? 0 : Convert.ToDecimal(kraTable.Rows[i][5]?.ToString()?.Trim());
                                            VersionObjectiveKRA.BENCHMARK_TYPE = kraTable.Rows[i][6] == null ? 0 : kraTable.Rows[i][6]?.ToString()?.Trim() == "" ? 0 : _appConstantsRepository.GetAppraisalStatusId(kraTable.Rows[i][6]?.ToString()?.Trim());
                                            VersionObjectiveKRA.BENCHMARK_UITYPE = kraTable.Rows[i][7] == null ? 0 : kraTable.Rows[i][7]?.ToString()?.Trim() == "" ? 0 : _appConstantsRepository.GetAppraisalStatusId(kraTable.Rows[i][7]?.ToString()?.Trim());
                                            VersionObjectiveKRA.IS_DOCUMENT_MANDATORY = kraTable.Rows[i][8] == null ? false : kraTable.Rows[i][8]?.ToString()?.Trim() == "" ? false : Convert.ToInt32(kraTable.Rows[i][8]?.ToString()?.Trim()) == 0 ? false : Convert.ToInt32(kraTable.Rows[i][8]?.ToString()?.Trim()) == 1 ? true : false;
                                            VersionObjectiveKRA.BENCHMARK_DURATION = 0;
                                            VersionObjectiveKRA.BENCHMARK_OPERATOR = 0;
                                            VersionObjectiveKRA.BENCHMARK_VALUE = 0;
                                            VersionObjectiveKRA.BENCHMARK_FROM_VALUE = 0;
                                            VersionObjectiveKRA.BENCHMARK_TO_VALUE = 0;
                                            VersionObjectiveKRA.CREATED_BY = 1;
                                            VersionObjectiveKRA.CREATED_DATE = DateTime.UtcNow;
                                            VersionObjectiveKRA.UPDATED_DATE = DateTime.UtcNow;
                                            VersionObjectiveKRA.UPDATED_BY = 1;
                                            await _versionKeyResultsRepository.AddAsync(VersionObjectiveKRA);
                                            await _versionKeyResultsRepository.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            VersionObjectiveKRA.KEY_RESULT_WEIGHTAGE = kraTable.Rows[i][5] == null ? 0 : kraTable.Rows[i][5]?.ToString()?.Trim() == "" ? 0 : Convert.ToDecimal(kraTable.Rows[i][5]?.ToString()?.Trim());
                                            VersionObjectiveKRA.BENCHMARK_TYPE = kraTable.Rows[i][6] == null ? 0 : kraTable.Rows[i][6]?.ToString()?.Trim() == "" ? 0 : _appConstantsRepository.GetAppraisalStatusId(kraTable.Rows[i][6]?.ToString()?.Trim());
                                            VersionObjectiveKRA.BENCHMARK_UITYPE = kraTable.Rows[i][7] == null ? 0 : kraTable.Rows[i][7]?.ToString()?.Trim() == "" ? 0 : _appConstantsRepository.GetAppraisalStatusId(kraTable.Rows[i][7]?.ToString()?.Trim());
                                            VersionObjectiveKRA.IS_DOCUMENT_MANDATORY = kraTable.Rows[i][8] == null ? false : kraTable.Rows[i][8]?.ToString()?.Trim() == "" ? false : Convert.ToInt32(kraTable.Rows[i][8]?.ToString()?.Trim()) == 0 ? false : Convert.ToInt32(kraTable.Rows[i][8]?.ToString()?.Trim()) == 1 ? true : false;
                                            VersionObjectiveKRA.BENCHMARK_DURATION = 0;
                                            VersionObjectiveKRA.BENCHMARK_OPERATOR = 0;
                                            VersionObjectiveKRA.BENCHMARK_VALUE = 0;
                                            VersionObjectiveKRA.BENCHMARK_FROM_VALUE = 0;
                                            VersionObjectiveKRA.BENCHMARK_TO_VALUE = 0;
                                            VersionObjectiveKRA.UPDATED_BY = 1;
                                            VersionObjectiveKRA.UPDATED_DATE = DateTime.UtcNow;
                                            _versionKeyResultsRepository.Update(VersionObjectiveKRA);
                                            await _versionKeyResultsRepository.SaveChangesAsync();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel", "Insert KRA mapping");
                                }

                            }
                        }
                        //Insert benchmark mapping
                        DataTable benchmarkTable = dataset?.Tables["benchmark_mapping"];
                        if (benchmarkTable?.Rows?.Count > 1)
                        {
                            List<VersionKRABenchmarkGridDetails> benchmarkRangeList = new List<VersionKRABenchmarkGridDetails>();
                            for (int i = 1; i < benchmarkTable?.Rows?.Count; i++)
                            {
                                VersionKRABenchmarkGridDetails benchmarkRange = new VersionKRABenchmarkGridDetails();
                                try
                                {
                                    benchmarkRange.VersionName = benchmarkTable.Rows[i][0]?.ToString()?.Trim();
                                    benchmarkRange.DepartmentName = benchmarkTable.Rows[i][1]?.ToString()?.Trim()?.ToLower();
                                    benchmarkRange.RoleName = benchmarkTable.Rows[i][2]?.ToString()?.Trim()?.ToLower();
                                    benchmarkRange.ObjectiveName = benchmarkTable.Rows[i][3]?.ToString()?.Trim();
                                    benchmarkRange.KeyResultName = benchmarkTable.Rows[i][4]?.ToString()?.Trim();
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel", "Insert benchmark values");
                                }
                                benchmarkRangeList.Add(benchmarkRange);
                            }
                            if (benchmarkRangeList?.Count > 0)
                            {
                                benchmarkRangeList = benchmarkRangeList.GroupBy(x => new { x.VersionName, x.DepartmentName, x.RoleName, x.ObjectiveName, x.KeyResultName }).Select(x => x.First()).ToList();
                                foreach (VersionKRABenchmarkGridDetails item in benchmarkRangeList)
                                {
                                    int versionId = _versionRepository.GetVersionIdByName(item.VersionName);
                                    int? depId = import?.Department?.Where(x => x.DepartmentName?.ToLower() == item.DepartmentName).Select(x => x.DepartmentId).FirstOrDefault();
                                    int departmentId = depId == null ? 0 : (int)depId;
                                    int? role = import?.Roles?.Where(x => x.RoleName?.ToLower() == item.RoleName).Select(x => x.RoleId).FirstOrDefault();
                                    int roleId = role == null ? 0 : (int)role;
                                    int objectiveId = _appraisalObjectiveRepository.GetObjectiveIdByName(item.ObjectiveName);
                                    int kraId = _appraisalKeyResultRepository.GetKRAIdByName(item.KeyResultName);
                                    List<VersionBenchMarks> benchmarkList = _versionBenchmarksRepository.GetVersionKRABenchmarkRange(versionId, departmentId, roleId, objectiveId, kraId);
                                    if (benchmarkList?.Count > 0)
                                    {
                                        foreach (VersionBenchMarks benchmark in benchmarkList)
                                        {
                                            _versionBenchmarksRepository.Delete(benchmark);
                                            await _versionBenchmarksRepository.SaveChangesAsync();
                                        }
                                    }
                                }
                            }

                            for (int i = 1; i < benchmarkTable?.Rows?.Count; i++)
                            {
                                try
                                {
                                    int versionId = _versionRepository.GetVersionIdByName(benchmarkTable.Rows[i][0]?.ToString()?.Trim());
                                    int? depId = import?.Department?.Where(x => x.DepartmentName?.ToLower() == benchmarkTable.Rows[i][1]?.ToString()?.Trim()?.ToLower()).Select(x => x.DepartmentId).FirstOrDefault();
                                    int departmentId = depId == null ? 0 : (int)depId;
                                    int? role = import?.Roles?.Where(x => x.RoleName?.ToLower() == benchmarkTable.Rows[i][2]?.ToString()?.Trim()?.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                                    int roleId = role == null ? 0 : (int)role;
                                    int objectiveId = _appraisalObjectiveRepository.GetObjectiveIdByName(benchmarkTable.Rows[i][3]?.ToString()?.Trim());
                                    int kraId = _appraisalKeyResultRepository.GetKRAIdByName(benchmarkTable.Rows[i][4]?.ToString()?.Trim());
                                    decimal benchmarkValue = 0;
                                    if (benchmarkTable.Rows[i][7] != null && benchmarkTable.Rows[i][7]?.ToString()?.Trim() != "")
                                    {
                                        if (IsNumeric(benchmarkTable.Rows[i][7]?.ToString()?.Trim()))
                                        {
                                            benchmarkValue = Convert.ToDecimal(benchmarkTable.Rows[i][7]?.ToString()?.Trim());
                                        }
                                        else
                                        {
                                            benchmarkValue = _appConstantsRepository.GetAppraisalStatusId(benchmarkTable.Rows[i][7]?.ToString()?.Trim());
                                        }

                                    }

                                    if (versionId > 0 && departmentId > 0 && roleId > 0 && objectiveId > 0 && kraId > 0)
                                    {
                                        VersionBenchMarks versionObjKRA = new VersionBenchMarks();
                                        versionObjKRA.VERSION_ID = versionId;
                                        versionObjKRA.DEPT_ID = departmentId;
                                        versionObjKRA.ROLE_ID = roleId;
                                        versionObjKRA.OBJECTIVE_ID = objectiveId;
                                        versionObjKRA.KEY_RESULT_ID = kraId;
                                        versionObjKRA.RANGE_FROM = benchmarkTable.Rows[i][5] == null ? 0 : benchmarkTable.Rows[i][5]?.ToString()?.Trim() == "" ? 0 : Convert.ToDecimal(benchmarkTable.Rows[i][5]?.ToString()?.Trim());
                                        versionObjKRA.RANGE_TO = benchmarkTable.Rows[i][6] == null ? 0 : benchmarkTable.Rows[i][6]?.ToString()?.Trim() == "" ? 0 : Convert.ToDecimal(benchmarkTable.Rows[i][6]?.ToString()?.Trim());
                                        versionObjKRA.BENCHMARK_VALUE = benchmarkValue;
                                        versionObjKRA.BENCHMARK_WEIGHTAGE = benchmarkTable.Rows[i][8] == null ? 0 : benchmarkTable.Rows[i][8]?.ToString()?.Trim() == "" ? 0 : Convert.ToDecimal(benchmarkTable.Rows[i][8]?.ToString()?.Trim());
                                        versionObjKRA.CREATED_BY = 1;
                                        versionObjKRA.CREATED_DATE = DateTime.UtcNow;
                                        await _versionBenchmarksRepository.AddAsync(versionObjKRA);
                                        await _versionBenchmarksRepository.SaveChangesAsync();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel", "Insert benchmark values");
                                }


                            }
                        }

                        //Create group
                        DataTable groupTable = dataset?.Tables["create_group"];
                        if (groupTable?.Rows?.Count > 1)
                        {
                            for (int i = 1; i < groupTable?.Rows?.Count; i++)
                            {
                                try
                                {
                                    int versionId = _versionRepository.GetVersionIdByName(groupTable.Rows[i][0]?.ToString()?.Trim());
                                    int? depId = import?.Department?.Where(x => x.DepartmentName?.ToLower() == groupTable.Rows[i][1]?.ToString()?.Trim()?.ToLower()).Select(x => x.DepartmentId).FirstOrDefault();
                                    int departmentId = depId == null ? 0 : (int)depId;
                                    int? role = import?.Roles?.Where(x => x.RoleName?.ToLower() == groupTable.Rows[i][2]?.ToString()?.Trim()?.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                                    int roleId = role == null ? 0 : (int)role;
                                    int objectiveId = _appraisalObjectiveRepository.GetObjectiveIdByName(groupTable.Rows[i][3]?.ToString()?.Trim());
                                    VersionKeyResultsGroup keyResultGroup = new VersionKeyResultsGroup();
                                    keyResultGroup = _versionKeyResultsGroupRepository.GetVersionKeyResultGroupByObjectiveId(versionId, departmentId, roleId, objectiveId, groupTable.Rows[i][4]?.ToString()?.Trim());
                                    if (versionId > 0 && departmentId > 0 && roleId > 0 && objectiveId > 0 && groupTable.Rows[i][4]?.ToString()?.Trim() != "")
                                    {
                                        if (keyResultGroup == null)
                                        {
                                            keyResultGroup = new VersionKeyResultsGroup();
                                            keyResultGroup.VERSION_ID = versionId;
                                            keyResultGroup.DEPT_ID = departmentId;
                                            keyResultGroup.ROLE_ID = roleId;
                                            keyResultGroup.OBJECTIVE_ID = objectiveId;
                                            keyResultGroup.KEY_RESULTS_GROUP_NAME = groupTable.Rows[i][4]?.ToString()?.Trim();
                                            keyResultGroup.KEY_RESULT_GROUP_WEIGHTAGE = groupTable.Rows[i][5] == null ? 0 : groupTable.Rows[i][5]?.ToString()?.Trim() == "" ? 0 : Convert.ToDecimal(groupTable.Rows[i][5]?.ToString()?.Trim());
                                            keyResultGroup.MANDATORY_KEY_RESULT_OPTIONS = groupTable.Rows[i][6] == null ? 0 : groupTable.Rows[i][6]?.ToString()?.Trim() == "" ? 0 : Convert.ToInt32(groupTable.Rows[i][6]?.ToString()?.Trim());

                                            keyResultGroup.CREATED_BY = 1;
                                            keyResultGroup.CREATED_DATE = DateTime.UtcNow;
                                            keyResultGroup.UPDATED_DATE = null;
                                            await _versionKeyResultsGroupRepository.AddAsync(keyResultGroup);
                                            await _versionKeyResultsGroupRepository.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            keyResultGroup.KEY_RESULT_GROUP_WEIGHTAGE = groupTable.Rows[i][5] == null ? 0 : groupTable.Rows[i][5]?.ToString()?.Trim() == "" ? 0 : Convert.ToDecimal(groupTable.Rows[i][5]?.ToString()?.Trim());
                                            keyResultGroup.MANDATORY_KEY_RESULT_OPTIONS = groupTable.Rows[i][6] == null ? 0 : groupTable.Rows[i][6]?.ToString()?.Trim() == "" ? 0 : Convert.ToInt32(groupTable.Rows[i][6]?.ToString()?.Trim());
                                            keyResultGroup.UPDATED_BY = 1;
                                            keyResultGroup.UPDATED_DATE = DateTime.UtcNow;
                                            _versionKeyResultsGroupRepository.Update(keyResultGroup);
                                            await _versionKeyResultsGroupRepository.SaveChangesAsync();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel", "Create benchmark group");
                                }


                            }
                        }
                        //KRA group mapping
                        DataTable groupDetailTable = dataset?.Tables["kra_group_mapping"];
                        if (groupDetailTable?.Rows?.Count > 1)
                        {
                            for (int i = 1; i < groupDetailTable?.Rows?.Count; i++)
                            {
                                try
                                {
                                    int versionId = _versionRepository.GetVersionIdByName(groupDetailTable.Rows[i][0]?.ToString()?.Trim());
                                    int? depId = import?.Department?.Where(x => x.DepartmentName?.ToLower() == groupDetailTable.Rows[i][1]?.ToString()?.Trim()?.ToLower()).Select(x => x.DepartmentId).FirstOrDefault();
                                    int departmentId = depId == null ? 0 : (int)depId;
                                    int? role = import?.Roles?.Where(x => x.RoleName?.ToLower() == groupDetailTable.Rows[i][2]?.ToString()?.Trim()?.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                                    int roleId = role == null ? 0 : (int)role;
                                    int objectiveId = _appraisalObjectiveRepository.GetObjectiveIdByName(groupDetailTable.Rows[i][3]?.ToString()?.Trim());
                                    VersionKeyResultsGroup keyResultGroup = _versionKeyResultsGroupRepository.GetVersionKeyResultGroupByObjectiveId(versionId, departmentId, roleId, objectiveId, groupDetailTable.Rows[i][4]?.ToString()?.Trim());
                                    int kraId = _appraisalKeyResultRepository.GetKRAIdByName(groupDetailTable.Rows[i][5]?.ToString()?.Trim());
                                    if (versionId > 0 && departmentId > 0 && roleId > 0 && objectiveId > 0 && kraId > 0 && keyResultGroup?.KEY_RESULTS_GROUP_ID > 0)
                                    {
                                        VersionKeyResultsGroupDetails keyResultGroupDetails = new VersionKeyResultsGroupDetails();
                                        keyResultGroupDetails = _versionKeyResultsGroupDetailsRepository.GetVersionKeyResultGroupDetailsById(versionId, departmentId, roleId, objectiveId, keyResultGroup.KEY_RESULTS_GROUP_ID, kraId);
                                        if (keyResultGroupDetails == null)
                                        {
                                            keyResultGroupDetails = new VersionKeyResultsGroupDetails();
                                            keyResultGroupDetails.VERSION_ID = versionId;
                                            keyResultGroupDetails.DEPT_ID = departmentId;
                                            keyResultGroupDetails.ROLE_ID = roleId;
                                            keyResultGroupDetails.OBJECTIVE_ID = objectiveId;
                                            keyResultGroupDetails.KEY_RESULTS_GROUP_ID = keyResultGroup.KEY_RESULTS_GROUP_ID;
                                            keyResultGroupDetails.KEY_RESULT_ID = kraId;
                                            keyResultGroupDetails.CREATED_BY = 1;
                                            keyResultGroupDetails.CREATED_DATE = DateTime.UtcNow;
                                            keyResultGroupDetails.UPDATED_DATE = null;
                                            await _versionKeyResultsGroupDetailsRepository.AddAsync(keyResultGroupDetails);
                                            await _versionKeyResultsGroupDetailsRepository.SaveChangesAsync();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel", "Kra group mapping");
                                }
                            }
                        }
                        return "Version Imported Successfully.";
                    }
                }
            }
            return "Import data's are empty, Please import valid data.";
        }
        #endregion
        
        #region Add or Update entity Master using excel
        public async Task<string> AddorUpdateEntityByExcel(ImportExcelView import)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            if (!string.IsNullOrEmpty(import.Base64Format))
            {
                byte[] bytes = Convert.FromBase64String(import.Base64Format);
                MemoryStream stream = new MemoryStream(bytes);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet dataset = reader?.AsDataSet();
                    if (dataset?.Tables?.Count > 0 && dataset?.Tables[0]?.Rows?.Count > 1)
                    {
                        for (int i = 1; i < dataset?.Tables[0]?.Rows?.Count; i++)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim()))
                                {
                                    int length = 4;
                                    EntityMaster entityDetails = new EntityMaster();

                                    entityDetails = _appraisalRepository.GetByName(dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim());
                                    if (entityDetails == null)
                                    {
                                        entityDetails = new EntityMaster();
                                        entityDetails.ENTITY_NAME = dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim();
                                        entityDetails.ENTITY_DESCRIPTION = dataset?.Tables[0]?.Rows[i][1]?.ToString()?.Trim();
                                        entityDetails.CREATED_DATE = DateTime.UtcNow;
                                        entityDetails.CREATED_BY = 1;
                                        await _appraisalRepository.AddAsync(entityDetails);
                                        await _appraisalRepository.SaveChangesAsync();
                                        entityDetails.ENTITY_SHORT_NAME = "Entity-" + entityDetails.ENTITY_ID.ToString().PadLeft(length, '0');
                                        _appraisalRepository.Update(entityDetails);
                                        await _appraisalRepository.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        //entityDetails.ENTITY_NAME = dataset?.Tables[0]?.Rows[i][0]?.ToString();
                                        entityDetails.ENTITY_DESCRIPTION = dataset?.Tables[0]?.Rows[i][1]?.ToString()?.Trim();
                                        entityDetails.UPDATED_DATE = DateTime.UtcNow;
                                        entityDetails.UPDATED_BY = 1;
                                        _appraisalRepository.Update(entityDetails);
                                        await _appraisalRepository.SaveChangesAsync();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateEntityByExcel");
                            }
                        }
                        return "SUCCESS";

                    }
                }
            }
            return "Import data's are empty, Please import valid data.";
        }
        #endregion
        
        #region Add or Update objective Master using excel
        public async Task<string> AddorUpdateObjectiveByExcel(ImportExcelView import)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            if (!string.IsNullOrEmpty(import.Base64Format))
            {
                byte[] bytes = Convert.FromBase64String(import.Base64Format);
                MemoryStream stream = new MemoryStream(bytes);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet dataset = reader?.AsDataSet();
                    if (dataset?.Tables?.Count > 0 && dataset?.Tables[0]?.Rows?.Count > 1)
                    {
                        for (int i = 1; i < dataset?.Tables[0]?.Rows?.Count; i++)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim()))
                                {
                                    int length = 4;
                                    ObjectiveMaster objectiveDetails = new ObjectiveMaster();

                                    objectiveDetails = _appraisalObjectiveRepository.GetByName(dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim());
                                    if (objectiveDetails == null)
                                    {
                                        objectiveDetails = new ObjectiveMaster();
                                        objectiveDetails.OBJECTIVE_NAME = dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim();
                                        objectiveDetails.OBJECTIVE_DESCRIPTION = dataset?.Tables[0]?.Rows[i][1]?.ToString()?.Trim();
                                        objectiveDetails.CREATED_DATE = DateTime.UtcNow;
                                        objectiveDetails.CREATED_BY = 1;
                                        await _appraisalObjectiveRepository.AddAsync(objectiveDetails);
                                        await _appraisalObjectiveRepository.SaveChangesAsync();
                                        objectiveDetails.OBJECTIVE_SHORT_NAME = "Objective-" + objectiveDetails.OBJECTIVE_ID.ToString().PadLeft(length, '0');
                                        _appraisalObjectiveRepository.Update(objectiveDetails);
                                        await _appraisalObjectiveRepository.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        //objectiveDetails.OBJECTIVE_NAME = dataset?.Tables[0]?.Rows[i][0]?.ToString();
                                        objectiveDetails.OBJECTIVE_DESCRIPTION = dataset?.Tables[0]?.Rows[i][1]?.ToString()?.Trim();
                                        objectiveDetails.UPDATED_DATE = DateTime.UtcNow;
                                        objectiveDetails.UPDATED_BY = 1;
                                        _appraisalObjectiveRepository.Update(objectiveDetails);
                                        await _appraisalObjectiveRepository.SaveChangesAsync();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateObjectiveByExcel");
                            }
                        }
                        return "SUCCESS";

                    }
                }
            }
            return "Import data's are empty, Please import valid data.";
        }
        #endregion
        
        #region Add or Update KeyResultMaster using excel
        public async Task<string> AddorUpdateKeyResultByExcel(ImportExcelView import)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            if (!string.IsNullOrEmpty(import.Base64Format))
            {
                byte[] bytes = Convert.FromBase64String(import.Base64Format);
                MemoryStream stream = new MemoryStream(bytes);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet dataset = reader?.AsDataSet();
                    if (dataset?.Tables?.Count > 0 && dataset?.Tables[0]?.Rows?.Count > 1)
                    {
                        for (int i = 1; i < dataset?.Tables[0]?.Rows?.Count; i++)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim()))
                                {
                                    int length = 4;
                                    KeyResultMaster keyResultDetails = new KeyResultMaster();

                                    keyResultDetails = _appraisalKeyResultRepository.GetByName(dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim());
                                    if (keyResultDetails == null)
                                    {
                                        keyResultDetails = new KeyResultMaster();
                                        keyResultDetails.KEY_RESULT_NAME = dataset?.Tables[0]?.Rows[i][0]?.ToString()?.Trim();
                                        keyResultDetails.KEY_RESULT_DESCRIPTION = dataset?.Tables[0]?.Rows[i][1]?.ToString()?.Trim();
                                        keyResultDetails.CREATED_DATE = DateTime.UtcNow;
                                        keyResultDetails.CREATED_BY = 1;
                                        await _appraisalKeyResultRepository.AddAsync(keyResultDetails);
                                        await _appraisalKeyResultRepository.SaveChangesAsync();
                                        keyResultDetails.KEY_RESULT_SHORT_NAME = "KRA-" + keyResultDetails.KEY_RESULT_ID.ToString().PadLeft(length, '0');
                                        _appraisalKeyResultRepository.Update(keyResultDetails);
                                        await _appraisalKeyResultRepository.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        //keyResultDetails.KEY_RESULT_NAME = dataset?.Tables[0]?.Rows[i][0]?.ToString();
                                        keyResultDetails.KEY_RESULT_DESCRIPTION = dataset?.Tables[0]?.Rows[i][1]?.ToString()?.Trim();
                                        keyResultDetails.UPDATED_DATE = DateTime.UtcNow;
                                        keyResultDetails.UPDATED_BY = 1;
                                        _appraisalKeyResultRepository.Update(keyResultDetails);
                                        await _appraisalKeyResultRepository.SaveChangesAsync();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateKeyResultByExcel");
                            }
                        }
                        return "SUCCESS";
                    }
                }
            }
            return "Import data's are empty, Please import valid data.";
        }
        #endregion
        
        #region GetIndividualAppraisalRating Details
        public IndividualEmployeeAppraisalRating GetEmployeeAppraisalRating(int employeeID)
        {
            return _employeeAppraisalMasterRepository.GetEmployeeAppraisalRating(employeeID);
        }
        #endregion

        #region Is Numeric
        public bool IsNumeric(string input)
        {
            decimal numeric;
            return decimal.TryParse(input, out numeric);
        }
        #endregion
        #region Add appraisal Comments
        public async Task<int> AddSelfAppraisalBUHeadComment(EmployeeAppraisalBUHeadComment employeeAppraisalComment)
        {
            try
            {
                int commentId = 0;
                EmployeeAppraisalConversation appraisalComment = new EmployeeAppraisalConversation();

                if (employeeAppraisalComment.Comment != null && employeeAppraisalComment.Comment != string.Empty)
                {
                    if (employeeAppraisalComment.Comment_Id != 0) appraisalComment = _employeeAppraisalCommentRepository.GetAppraisalCommentById(employeeAppraisalComment.Comment_Id);
                    if (appraisalComment != null)
                    {
                        appraisalComment.APP_CYCLE_ID = employeeAppraisalComment.App_Cycle_Id;
                        appraisalComment.EMPLOYEE_ID = employeeAppraisalComment.Employee_Id;
                        appraisalComment.COMMENT = employeeAppraisalComment.Comment;

                        if (employeeAppraisalComment.Comment_Id == 0)
                        {
                            appraisalComment.CREATED_BY = employeeAppraisalComment.Created_By; //employeeAppraisalComment.Employee_Id;
                            appraisalComment.CREATED_DATE = DateTime.UtcNow;
                            await _employeeAppraisalCommentRepository.AddAsync(appraisalComment);
                            await _employeeAppraisalCommentRepository.SaveChangesAsync();
                            commentId = appraisalComment.COMMENT_ID;
                        }
                    }
                }
                else
                {
                    return 0;
                }
                return commentId;
            }
            catch (Exception)
            {
                throw; ;
            }
        }
        #endregion

        #region Get Employee Appraisal Details by Employee id For detail review
        public List<EmployeeAppraisalMasterDetailView> GetEmployeeAppraisalListByEmployeeIdForReview(EmployeeListAndDepartment empListDepartment)
        {
            return _employeeAppraisalMasterRepository.GetEmployeeAppraisalListByEmployeeId(empListDepartment.employeeids);
        }
        #endregion

        #region Get Employee WorkDay Details by Filter
        public List<AppraisalWorkDayView> GetEmployeeWorkDayDetailsByFilter(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
        {
            List<AppraisalWorkDayView> workDayViewList = _appraisalKeyResultRepository.GetEmployeeWorkDayDetailsByFilter(appraisalWorkDayFilterView);
            if(workDayViewList?.Count > 0)
            {
                List<WorkDayDetail> subWorkDayDetailList = GetWorkDayDetailByFilter(appraisalWorkDayFilterView);
                DateTime startDate = appraisalWorkDayFilterView.StartDate.Date;
                DateTime endDate = appraisalWorkDayFilterView.EndDate.Date;

                var workDateList = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                                    .Select(x => startDate.AddDays(x)).ToList();
                int totalDays = Convert.ToInt32((endDate - startDate).TotalDays);
                if (totalDays > 0)
                {
                    List<AppraisalWorkDayView> appraisalWorkDayViewList = new List<AppraisalWorkDayView>();
                    for (var workDate = startDate; workDate <= endDate; workDate = workDate.AddDays(1))
                    {
                        //var workDateDetail = workDayViewList.Where(x => x.StartDate <= appraisalWorkDayFilterView.StartDate
                        //                                                 && x.EndDate >= appraisalWorkDayFilterView.EndDate).FirstOrDefault();

                        //var workDateDetail = workDayViewList.Where(x => x.StartDate <= appraisalWorkDayFilterView.StartDate
                        //                                                 && x.EndDate >= appraisalWorkDayFilterView.EndDate)
                        //    .Select(rs => new AppraisalWorkDayView
                        //    {
                        //        AppCycleId = rs.AppCycleId,
                        //        StartDate = rs.StartDate,
                        //        EndDate = rs.EndDate,
                        //        VersionId = rs.VersionId,
                        //        EmployeeId = rs.EmployeeId,
                        //        EmployeeObjectiveList = rs.EmployeeObjectiveList
                        //    }).FirstOrDefault();

                        List<AppraisalWorkDayView> newworkDayViewList = JsonConvert.DeserializeObject<List<AppraisalWorkDayView>>(JsonConvert.SerializeObject(workDayViewList));
                        var workDateDetail = newworkDayViewList.Where(x => x.StartDate <= appraisalWorkDayFilterView.StartDate
                                                                         && x.EndDate >= appraisalWorkDayFilterView.EndDate).FirstOrDefault();

                        if (workDateDetail != null && workDateDetail.AppCycleId > 0)
                        {
                            workDateDetail.WorkDate = workDate;
                            if(workDateDetail?.EmployeeObjectiveList.Count > 0 && subWorkDayDetailList?.Count > 0)
                            foreach (AppraisalWorkDayObjectiveView item in workDateDetail?.EmployeeObjectiveList)
                            {
                                if (item.EmployeeKRAList?.Count > 0)
                                        foreach (AppraisalWorkDayKRAView KRAitem in item?.EmployeeKRAList)
                                        {
                                            KRAitem.SubmittedWorkDayDetailList = subWorkDayDetailList.Where(x => 
                                                                                 //workDateDetail.EmployeeId == x.EmployeeId &&
                                                                                 KRAitem.KeyResultId == x.WorkdayKRAId).ToList();
                                        }
                                    //foreach (AppraisalWorkDayKRAGroupView grpItem in item.GroupKRAList)
                                    //{
                                    //    if (grpItem.GroupKRADetailList?.Count > 0)
                                    //        foreach (AppraisalWorkDayKRAView KRAitem in grpItem?.GroupKRADetailList)
                                    //        {
                                    //            KRAitem.SubmittedWorkDayDetailList = subWorkDayDetailList.Where(x => x.WorkDate == workDate && workDateDetail.EmployeeId == x.EmployeeId
                                    //                                                && grpItem.KeyResultGroupId == x.GroupId && KRAitem.KeyResultId == x.KRAId).ToList();
                                    //        }
                                    //}
                                }
                            appraisalWorkDayViewList.Add(workDateDetail);
                        }
                    }
                    workDayViewList = appraisalWorkDayViewList == null ? new List<AppraisalWorkDayView>() : appraisalWorkDayViewList;
                }
            }
            return workDayViewList;
        }
        #endregion

        #region Get WorkDayDetail By Filter
        /// <summary>
        /// Get WorkDayDetail By Filter
        /// </summary>
        /// <param name="lstWorkDayDetail"></param>
        /// <returns></returns>
        public List<WorkDayDetail> GetWorkDayDetailByFilter(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
        {
            List<WorkDayDetail> lstWorkDayDetailResponse = new();// _workDayDetailRepository.GetWorkDayDetailByFilter(appraisalWorkDayFilterView);
            //lstWorkDayDetailResponse = _workDayDetailRepository.GetWorkDayDetailByEmployeeId(appraisalWorkDayFilterView.EmployeeId);
            return lstWorkDayDetailResponse;
        }

        #endregion

    }
}