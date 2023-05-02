using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;

namespace Appraisal.DAL.Repository
{
    public interface IAppraisalKeyResultRepository : IBaseRepository<KeyResultMaster>
    {
        KeyResultMaster GetByID(int keyResultId);
        List<KeyResultMaster> GetAllKeyResultDetails();
        List<VersionKRABenchmarkGridDetails> GetVersionKRAGridviewData(int versionId);
        List<VersionKRABenchmarkGridDetails> GetVersionBenchmarkGridData(int versionId);
        bool KRANameDuplication(string keyResultName, int kraId);
        int GetKRAIdByName(string keyResultName);
        KeyResultMaster GetByName(string keyResultName);
        List<AppraisalWorkDayView> GetEmployeeWorkDayDetailsByFilter(AppraisalWorkDayFilterView appraisalWorkDayFilterView);
    }
    public class AppraisalKeyResultRepository : BaseRepository<KeyResultMaster>, IAppraisalKeyResultRepository
    {
        private readonly AppraisalDBContext dbContext;
        public AppraisalKeyResultRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public KeyResultMaster GetByID(int keyResultId)
        {
            return dbContext.KeyResultMaster.Where(x => x.KEY_RESULT_ID == keyResultId).FirstOrDefault();
        }
        public KeyResultMaster GetByName(string keyResultName)
        {
            return keyResultName==null?null:dbContext.KeyResultMaster.Where(x => x.KEY_RESULT_NAME.ToLower() == keyResultName.ToLower()).FirstOrDefault();
        }
        public List<KeyResultMaster> GetAllKeyResultDetails()
        {
            return dbContext.KeyResultMaster.OrderByDescending(x => x.CREATED_DATE).ToList();
        }
        public List<VersionKRABenchmarkGridDetails> GetVersionKRAGridviewData(int versionId)
        {
            List<VersionKRABenchmarkGridDetails> VersionKRAGridResult = (from ver in dbContext.VersionKeyResults
                                                                         join vermas in dbContext.VersionMaster on ver.VERSION_ID equals vermas.VERSION_ID
                                                                         join obj in dbContext.ObjectiveMaster on ver.OBJECTIVE_ID equals obj.OBJECTIVE_ID
                                                                         join kra in dbContext.KeyResultMaster on ver.KEY_RESULT_ID equals kra.KEY_RESULT_ID
                                                                         where ver.VERSION_ID == versionId
                                                                         select new VersionKRABenchmarkGridDetails
                                                                         {
                                                                             VersionId = ver.VERSION_ID,
                                                                             VersionName = vermas.VERSION_NAME,
                                                                             VersionCode = vermas.VERSION_CODE,
                                                                             VersionDesciption = vermas.VERSION_DESC,
                                                                             DepartmentId = ver.DEPT_ID,
                                                                             RoleId = ver.ROLE_ID,
                                                                             ObjectiveId = obj.OBJECTIVE_ID,
                                                                             ObjectiveName = obj.OBJECTIVE_NAME,
                                                                             KeyResultId = kra.KEY_RESULT_ID,
                                                                             KeyResultName = kra.KEY_RESULT_NAME
                                                                         }).Distinct().ToList();
            return VersionKRAGridResult == null ? new List<VersionKRABenchmarkGridDetails>() : VersionKRAGridResult;
        }
        public List<VersionKRABenchmarkGridDetails> GetVersionBenchmarkGridData(int versionId)
        {
            List<VersionKRABenchmarkGridDetails> VersionKRAGridResult = (from ver in dbContext.VersionKeyResults
                                                                         join vermas in dbContext.VersionMaster on ver.VERSION_ID equals vermas.VERSION_ID
                                                                         join obj in dbContext.ObjectiveMaster on ver.OBJECTIVE_ID equals obj.OBJECTIVE_ID
                                                                         join kra in dbContext.KeyResultMaster on ver.KEY_RESULT_ID equals kra.KEY_RESULT_ID
                                                                         where ver.VERSION_ID == versionId
                                                                         select new VersionKRABenchmarkGridDetails
                                                                         {
                                                                             VersionId = ver.VERSION_ID,
                                                                             VersionName = vermas.VERSION_NAME,
                                                                             VersionCode = vermas.VERSION_CODE,
                                                                             VersionDesciption = vermas.VERSION_DESC,
                                                                             DepartmentId = ver.DEPT_ID,
                                                                             RoleId = ver.ROLE_ID,
                                                                             ObjectiveId = obj.OBJECTIVE_ID,
                                                                             ObjectiveName = obj.OBJECTIVE_NAME,
                                                                             KeyResultId = kra.KEY_RESULT_ID,
                                                                             KeyResultName = kra.KEY_RESULT_NAME
                                                                         }).Distinct().ToList();
            return VersionKRAGridResult == null ? new List<VersionKRABenchmarkGridDetails>() : VersionKRAGridResult;
        }
        public bool KRANameDuplication(string keyResultName,int kraId)
        {
            //int kraId = dbContext.KeyResultMaster.Where(x => x.KEY_RESULT_NAME == keyResultName).Select(x=>x.KEY_RESULT_ID).FirstOrDefault();
            //if (kraId !=0)
            //    return true;
            //return false;
            bool isDuplicateName = false;
            string existName = dbContext.KeyResultMaster.Where(x => x.KEY_RESULT_NAME.ToLower() == keyResultName.ToLower() && (x.KEY_RESULT_ID == kraId || kraId == 0)).Select(x => x.KEY_RESULT_NAME).FirstOrDefault();
            if (kraId == 0 && existName != null)
            {
                isDuplicateName = true;
            }
            else if (kraId != 0 && existName?.ToLower() != keyResultName?.ToLower())
            {
                string newName = dbContext.KeyResultMaster.Where(x => x.KEY_RESULT_NAME.ToLower() == keyResultName.ToLower()).Select(x => x.KEY_RESULT_NAME).FirstOrDefault();
                if (newName != null)
                {
                    isDuplicateName = true;
                }
            }
            return isDuplicateName;
        }
        public int GetKRAIdByName(string keyResultName)
        {
            return keyResultName==null?0: dbContext.KeyResultMaster.Where(x => x.KEY_RESULT_NAME.ToLower() == keyResultName.ToLower()).Select(x => x.KEY_RESULT_ID).FirstOrDefault();
        }

        public List<AppraisalWorkDayView> GetEmployeeWorkDayDetailsByFilter(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
        {
            DateTime startDate = appraisalWorkDayFilterView.StartDate.Date;
            DateTime endDate = appraisalWorkDayFilterView.EndDate.Date;
            List<AppraisalWorkDayView> appraisalWorkDayViewResult = new List<AppraisalWorkDayView>();
            appraisalWorkDayViewResult = (from app in dbContext.AppraisalMaster
                                        join emp in dbContext.EmployeeAppraisalMaster on app.APP_CYCLE_ID equals emp.APP_CYCLE_ID
                                        where app.APP_CYCLE_START_DATE <= startDate
                                        && app.APP_CYCLE_END_DATE >= endDate
                                        && emp.EMPLOYEE_ID == appraisalWorkDayFilterView.EmployeeId
                                        //&& emp.EMPLOYEE_DEPT_ID == appraisalWorkDayFilterView.DepartmentId
                                        //&& emp.EMPLOYEE_ROLE_ID == appraisalWorkDayFilterView.RoleId
                                        select new AppraisalWorkDayView
                                        {
                                            AppCycleId = app.APP_CYCLE_ID, StartDate = app.APP_CYCLE_START_DATE, EndDate = app.APP_CYCLE_END_DATE,
                                            VersionId = (int)app.VERSION_ID, EmployeeId = emp.EMPLOYEE_ID,
                                            //DepartmentId = ver.DEPT_ID,
                                            //RoleId = ver.ROLE_ID,
                                            EmployeeObjectiveList = (from vrole in dbContext.VersionDepartmentRoleObjective.Where(vx =>  vx.VERSION_ID == app.VERSION_ID && vx.DEPT_ID == emp.EMPLOYEE_DEPT_ID && vx.ROLE_ID == emp.EMPLOYEE_ROLE_ID)
                                                                    join obj in dbContext.ObjectiveMaster on vrole.OBJECTIVE_ID equals obj.OBJECTIVE_ID
                                                                    select new AppraisalWorkDayObjectiveView
                                                                    {
                                                                        ObjectiveId = obj.OBJECTIVE_ID, ObjectiveName = obj.OBJECTIVE_NAME,
                                                                        EmployeeKRAList = (from verin in dbContext.VersionKeyResults.Where(vkx => vkx.KEY_RESULT_WEIGHTAGE > 0 && vkx.VERSION_ID == vrole.VERSION_ID && vkx.DEPT_ID == vrole.DEPT_ID && vkx.ROLE_ID == vrole.ROLE_ID && vkx.OBJECTIVE_ID == obj.OBJECTIVE_ID)
                                                                                           join kra in dbContext.KeyResultMaster on verin.KEY_RESULT_ID equals kra.KEY_RESULT_ID
                                                                                           select new AppraisalWorkDayKRAView
                                                                                            {
                                                                                                KeyResultId = kra.KEY_RESULT_ID, KeyResultName = kra.KEY_RESULT_NAME
                                                                                            }).ToList(),
                                                                        GroupKRAList = (from kragrp in dbContext.VersionKeyResultsGroup.Where(vgx =>  vgx.VERSION_ID == vrole.VERSION_ID && vgx.DEPT_ID == vrole.DEPT_ID && vgx.ROLE_ID == vrole.ROLE_ID && vgx.OBJECTIVE_ID == vrole.OBJECTIVE_ID)
                                                                                        select new AppraisalWorkDayKRAGroupView
                                                                                        {
                                                                                            KeyResultGroupId = kragrp.KEY_RESULTS_GROUP_ID,
                                                                                            KeyResultGroupName = kragrp.KEY_RESULTS_GROUP_NAME,
                                                                                            GroupKRADetailList = (from grpdet in dbContext.VersionKeyResultsGroupDetails.Where(vgdx => kragrp.KEY_RESULTS_GROUP_ID == vgdx.KEY_RESULTS_GROUP_ID)
                                                                                                                join kra in dbContext.KeyResultMaster on grpdet.KEY_RESULT_ID equals kra.KEY_RESULT_ID
                                                                                                                    select new AppraisalWorkDayKRAView
                                                                                                                    {
                                                                                                                        KeyResultId = kra.KEY_RESULT_ID,
                                                                                                                        KeyResultName = kra.KEY_RESULT_NAME
                                                                                                                    }).ToList()
                                                                                        }).ToList()
                                                                    }).ToList()
                                        }).ToList();

            appraisalWorkDayViewResult = appraisalWorkDayViewResult == null ? new List<AppraisalWorkDayView>() : appraisalWorkDayViewResult;
            
            return appraisalWorkDayViewResult;
        }
    }
}
