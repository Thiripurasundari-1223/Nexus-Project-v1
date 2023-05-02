using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Appraisal.DAL.Repository
{
    public interface IEmployeeAppraisalMasterRepository : IBaseRepository<EmployeeAppraisalMaster>
    {
        public AppraisalStatusReport getDepartmentWiseAppraisalDetails(int DepartmentID);
        public AppraisalStatusReportCount getAppraisalStatusReportCount();
        List<EmployeeAppraisalMasterDetailView> GetEmployeeAppraisalListByEmployeeId(List<int> employeeId);
        public List<AppraisalMilestonedetails> GetAppraisalMilestonedetails(int appCycleId);
        EmployeeAppraisalMaster GetAppCycleEmployee(int appCycleId, int employeeId);
        List<EmployeeAppraisalMasterDetailView> GetAllAppCycleEmployee(int appCycleId);
        bool checkAppCycleUsedEmployeeMaster(int appCycleId);
        EmployeeAppraisalMaster GetEmployeeAppraisalMasterByIDs(int appCycleId, int employeeId, int entityId);
        List<EmployeeAppraisalByManager> GetEmployeeAppraisalMasterByManagerID(int appCycleID, int managerID);
        AppraisalEmployeeStatusView GetEmployeeAppraisalStatusById(int appcycleId, int departmentId, int roleId, int employeeId);
        List<ManagerDetail> GetManagerDetails(int appCycleID, int managerID);
        public int GetCurrentAppcycleId();
        public List<EmployeeAppraisalMaster> GetEmployeeByDepartment(int appCycleID, int departmentId);
        public List<EmployeeAppraisalByManager> GetEmployeeByStatus(int appCycleID, int departmentId);
        EmployeeAppraisalMaster GetEmployeeAppraisalMasterByAppandEmpandManIDs(int appCycleID, int employeeID, int managerID);
        public IndividualEmployeeAppraisalRating GetEmployeeAppraisalRating(int employeeID);
        List<EmployeeAppraisalMaster> GetAppCycleEmployeeListByAppCycleId(int appCycleId);
        //List<EmployeeAppraisalMasterDetailView> GetEmployeeAppraisalListByEmployeeId(List<int> employeeId, bool isAll, bool isAdmin);

    }
    public class EmployeeAppraisalMasterRepository : BaseRepository<EmployeeAppraisalMaster>, IEmployeeAppraisalMasterRepository
    {
        private readonly AppraisalDBContext dbContext;
        public EmployeeAppraisalMasterRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public AppraisalStatusReport getDepartmentWiseAppraisalDetails(int DepartmentID)
        {
            List<EmployeeAppraisalMasterView> employeeAppraisalMasterView = new List<EmployeeAppraisalMasterView>();
            List<TeamRatingSummary> teamRatingSummaries = new List<TeamRatingSummary>();
            List<AppraisalStatusGridview> appraisalStatusGridviews = new List<AppraisalStatusGridview>();
            var appraisalMasterData = (from EmpMaster in dbContext.EmployeeAppraisalMaster
                                       join AppMaster in dbContext.AppraisalMaster on EmpMaster.APP_CYCLE_ID equals AppMaster.APP_CYCLE_ID
                                       where AppMaster.APPRAISAL_STATUS == 1 && (DepartmentID == 0 || EmpMaster.EMPLOYEE_DEPT_ID == DepartmentID)
                                       group EmpMaster by EmpMaster.APPRAISAL_STATUS into status
                                       select new
                                       {
                                           Appraisal_Status_Id = status.Key,
                                           EmployeeCount = status.Select(x => x.EMPLOYEE_ID).Count()
                                       }).ToList();
            int totalemployee = dbContext.EmployeeAppraisalMaster.Join(dbContext.AppraisalMaster, ea => ea.APP_CYCLE_ID, am => am.APP_CYCLE_ID, (ea, am) => new { ea, am })
              .Where(rs => rs.am.APPRAISAL_STATUS == 1 && (DepartmentID == 0 || rs.ea.EMPLOYEE_DEPT_ID == DepartmentID)).Count();
            foreach (var appStatus in appraisalMasterData)
            {
                var employeeAppraisalMaster = (from stsId in dbContext.EmployeeAppraisalMaster
                                               join AppMaster in dbContext.AppraisalMaster on stsId.APP_CYCLE_ID equals AppMaster.APP_CYCLE_ID
                                               join app in dbContext.AppConstants on stsId.APPRAISAL_STATUS equals app.APP_CONSTANT_ID
                                               where appStatus.Appraisal_Status_Id == app.APP_CONSTANT_ID && AppMaster.APPRAISAL_STATUS == 1 && (DepartmentID == 0 || stsId.EMPLOYEE_DEPT_ID == DepartmentID)
                                               select new
                                               {
                                                   Employee_Dept_Id = stsId.EMPLOYEE_DEPT_ID,
                                                   Appraisal_Status_Id = appStatus.Appraisal_Status_Id,
                                                   Apraisal_Status = app.APP_CONSTANT_TYPE_VALUE,
                                                   EmployeeCount = appStatus.EmployeeCount
                                               }).ToList();
                EmployeeAppraisalMasterView employeeAppraisalMasterView1 = new EmployeeAppraisalMasterView()
                {
                    Employee_Dept_Id = employeeAppraisalMaster.Select(x => x.Employee_Dept_Id).FirstOrDefault(),
                    Appraisal_Status_Id = employeeAppraisalMaster.Select(x => x.Appraisal_Status_Id).FirstOrDefault(),
                    Apraisal_Status = employeeAppraisalMaster.Select(x => x.Apraisal_Status + "-" + appStatus.EmployeeCount).FirstOrDefault(),
                    EmployeeCount = employeeAppraisalMaster.Select(x => x.EmployeeCount).FirstOrDefault(),
                    Appraisal_Percentage = (decimal.Divide(appStatus.EmployeeCount, totalemployee) * 100)
                };
                employeeAppraisalMasterView.Add(employeeAppraisalMasterView1);
            }

            var teamRatings = (from EmpMaster in dbContext.EmployeeAppraisalMaster
                               join AppMaster in dbContext.AppraisalMaster on EmpMaster.APP_CYCLE_ID equals AppMaster.APP_CYCLE_ID
                               where AppMaster.APPRAISAL_STATUS == 1 && (DepartmentID == 0 || EmpMaster.EMPLOYEE_DEPT_ID == DepartmentID)
                               group EmpMaster by EmpMaster.EMPLOYEE_SELF_RATING into rating
                               select new
                               {
                                   Rating = rating.Key,
                                   EmployeeCount = rating.Select(x => x.EMPLOYEE_ID).Count()
                               }).ToList();

            foreach (var rat in teamRatings)
            {
                var ratingsDetail = (from stsId in dbContext.EmployeeAppraisalMaster
                                     join AppMaster in dbContext.AppraisalMaster on stsId.APP_CYCLE_ID equals AppMaster.APP_CYCLE_ID
                                     where stsId.EMPLOYEE_SELF_RATING == rat.Rating && AppMaster.APPRAISAL_STATUS == 1
                                     select new
                                     {
                                         Employee_Dept_Id = stsId.EMPLOYEE_DEPT_ID,
                                         Rating = rat.Rating,
                                         EmployeeCount = rat.EmployeeCount
                                     }).ToList();
                TeamRatingSummary teamRatingSummary = new TeamRatingSummary()
                {
                    Employee_Dept_Id = ratingsDetail.Select(x => x.Employee_Dept_Id).FirstOrDefault(),
                    Employee_Rating = ratingsDetail.Select(x => x.Rating).FirstOrDefault(),
                    Employee_Count = ratingsDetail.Select(x => x.EmployeeCount).FirstOrDefault()
                };
                teamRatingSummaries.Add(teamRatingSummary);
            }

            appraisalStatusGridviews = (from appraisalMaster in dbContext.EmployeeAppraisalMaster
                                        join app in dbContext.AppConstants on appraisalMaster.APPRAISAL_STATUS equals app.APP_CONSTANT_ID
                                        join AppMaster in dbContext.AppraisalMaster on appraisalMaster.APP_CYCLE_ID equals AppMaster.APP_CYCLE_ID
                                        where AppMaster.APPRAISAL_STATUS == 1 && (DepartmentID == 0 || appraisalMaster.EMPLOYEE_DEPT_ID == DepartmentID)
                                        select new AppraisalStatusGridview
                                        {
                                            Employee_Id = appraisalMaster.EMPLOYEE_ID,
                                            Manager_Id = appraisalMaster.EMPLOYEE_MANAGER_ID,
                                            Department_Id = appraisalMaster.EMPLOYEE_DEPT_ID,
                                            Role_Id = appraisalMaster.EMPLOYEE_ROLE_ID,
                                            Rating = appraisalMaster.EMPLOYEE_FINAL_RATING,
                                            AppraisalStatus = app.APP_CONSTANT_TYPE_VALUE,
                                            AppraisalStatusId = appraisalMaster.APPRAISAL_STATUS,
                                            AppCycleID=appraisalMaster.APP_CYCLE_ID,
                                            IsBuheadApproved = appraisalMaster.IsBUHeadApproved == null ? false : appraisalMaster.IsBUHeadApproved
                                        }).Distinct().ToList();

            AppraisalStatusReport appraisalStatus = new AppraisalStatusReport()
            {
                AppraisalStatus = employeeAppraisalMasterView,
                TeamRatingSummary = teamRatingSummaries,
                appraisalStatusGridviews = appraisalStatusGridviews
            };
            return appraisalStatus;
        }

        public AppraisalStatusReportCount getAppraisalStatusReportCount()
        {
            var Pending = (from statuscount in dbContext.EmployeeAppraisalMaster
                           join AppMaster in dbContext.AppraisalMaster on statuscount.APP_CYCLE_ID equals AppMaster.APP_CYCLE_ID
                           where statuscount.APPRAISAL_STATUS == 10 && AppMaster.APPRAISAL_STATUS == 1
                           group statuscount by statuscount.APPRAISAL_STATUS into count
                           select new
                           {
                               PendingCount = count.Select(x => x.EMPLOYEE_ID).Count()
                           }).FirstOrDefault();
            var Approved = (from statuscount in dbContext.EmployeeAppraisalMaster
                            join AppMaster in dbContext.AppraisalMaster on statuscount.APP_CYCLE_ID equals AppMaster.APP_CYCLE_ID
                            where statuscount.APPRAISAL_STATUS == 12 && AppMaster.APPRAISAL_STATUS == 1
                            group statuscount by statuscount.APPRAISAL_STATUS into count
                            select new
                            {
                                ApprovedCount = count.Select(x => x.EMPLOYEE_ID).Count()
                            }).FirstOrDefault();
            var AppraisalCycle = (from statuscount in dbContext.EmployeeAppraisalMaster
                                  join AppMaster in dbContext.AppraisalMaster on statuscount.APP_CYCLE_ID equals AppMaster.APP_CYCLE_ID
                                  where AppMaster.APPRAISAL_STATUS == 1
                                  select new
                                  {
                                      AppraisalCycle = AppMaster.APP_CYCLE_NAME
                                  }).FirstOrDefault();

            AppraisalStatusReportCount appraisalStatusReportCount = new AppraisalStatusReportCount()
            {
                AppraisalCycle = AppraisalCycle.AppraisalCycle.ToString(),
                Pending = (Pending == null) ? 0 : Pending.PendingCount,
                Approved = (Approved == null) ? 0 : Approved.ApprovedCount
            };
            return appraisalStatusReportCount;
        }

        public List<EmployeeAppraisalMasterDetailView> GetEmployeeAppraisalListByEmployeeId(List<int> employeeId)
        {
            List<EmployeeAppraisalMasterDetailView> EmployeeAppraisalList = new List<EmployeeAppraisalMasterDetailView>();
            int appCycleId = dbContext.AppraisalMaster.Where(am => am.APPRAISAL_STATUS == 1).Select(x => x.APP_CYCLE_ID).FirstOrDefault();
            if (appCycleId > 0)
            {
                EmployeeAppraisalList = dbContext.EmployeeAppraisalMaster
                .Join(dbContext.EntityMaster, ea => ea.ENTITY_ID, em => em.ENTITY_ID, (ea, em) => new { ea, em })
                .Join(dbContext.AppConstants, eaem => eaem.ea.APPRAISAL_STATUS, ac => ac.APP_CONSTANT_ID, (eaem, ac) => new { eaem, ac })
                .Where(rs => rs.eaem.ea.APP_CYCLE_ID == appCycleId && employeeId.Contains(rs.eaem.ea.EMPLOYEE_ID))
                .Select(rs => new EmployeeAppraisalMasterDetailView
                {
                    APP_CYCLE_ID = rs.eaem.ea.APP_CYCLE_ID,
                    EMPLOYEE_ID = rs.eaem.ea.EMPLOYEE_ID,
                    ENTITY_ID = rs.eaem.ea.ENTITY_ID,
                    ENTITY_NAME = rs.eaem.em.ENTITY_NAME,
                    ENTITY_SHORT_NAME = rs.eaem.em.ENTITY_SHORT_NAME,
                    EMPLOYEE_ROLE_ID = rs.eaem.ea.EMPLOYEE_ROLE_ID,
                    EMPLOYEE_DEPT_ID = rs.eaem.ea.EMPLOYEE_DEPT_ID,
                    EMPLOYEE_MANAGER_ID = rs.eaem.ea.EMPLOYEE_MANAGER_ID,
                    EMPLOYEE_SELF_RATING = rs.eaem.ea.EMPLOYEE_SELF_RATING,
                    EMPLOYEE_APPRAISER_RATING = rs.eaem.ea.EMPLOYEE_APPRAISER_RATING,
                    EMPLOYEE_FINAL_RATING = rs.eaem.ea.EMPLOYEE_FINAL_RATING,
                    APPRAISAL_STATUS = rs.eaem.ea.APPRAISAL_STATUS,
                    APPRAISAL_STATUS_NAME = rs.ac.APP_CONSTANT_TYPE_VALUE,
                    CREATED_BY = rs.eaem.ea.CREATED_BY,
                    CREATED_DATE = rs.eaem.ea.CREATED_DATE,
                    UPDATED_BY = rs.eaem.ea.UPDATED_BY,
                    UPDATED_DATE = rs.eaem.ea.UPDATED_DATE,
                    IsBUHeadRevert = rs.eaem.ea.IsBUHeadRevert,
                    IsRevertRating = rs.eaem.ea.IsRevertRating,
                    IsBuheadApproved = rs.eaem.ea.IsBUHeadApproved == null ? false : rs.eaem.ea.IsBUHeadApproved
                }).ToList();
            }
            return EmployeeAppraisalList;
        }
        //public List<EmployeeAppraisalMasterDetailView> GetEmployeeAppraisalListByEmployeeId(List<int> employeeId, bool isAll, bool isAdmin)
        //{
        //    List<EmployeeAppraisalMasterDetailView> EmployeeAppraisalList = new List<EmployeeAppraisalMasterDetailView>();
        //    int appCycleId = dbContext.AppraisalMaster.Where(am => am.APPRAISAL_STATUS == 1).Select(x => x.APP_CYCLE_ID).FirstOrDefault();
        //    if (appCycleId > 0)
        //    {
        //        if (isAdmin)
        //        {
        //            EmployeeAppraisalList = dbContext.EmployeeAppraisalMaster
        //            .Join(dbContext.EntityMaster, ea => ea.ENTITY_ID, em => em.ENTITY_ID, (ea, em) => new { ea, em })
        //            .Join(dbContext.AppConstants, eaem => eaem.ea.APPRAISAL_STATUS, ac => ac.APP_CONSTANT_ID, (eaem, ac) => new { eaem, ac })
        //            .Where(rs => rs.eaem.ea.APP_CYCLE_ID == appCycleId )
        //            .Select(rs => new EmployeeAppraisalMasterDetailView
        //            {
        //                APP_CYCLE_ID = rs.eaem.ea.APP_CYCLE_ID,
        //                EMPLOYEE_ID = rs.eaem.ea.EMPLOYEE_ID,
        //                ENTITY_ID = rs.eaem.ea.ENTITY_ID,
        //                ENTITY_NAME = rs.eaem.em.ENTITY_NAME,
        //                ENTITY_SHORT_NAME = rs.eaem.em.ENTITY_SHORT_NAME,
        //                EMPLOYEE_ROLE_ID = rs.eaem.ea.EMPLOYEE_ROLE_ID,
        //                EMPLOYEE_DEPT_ID = rs.eaem.ea.EMPLOYEE_DEPT_ID,
        //                EMPLOYEE_MANAGER_ID = rs.eaem.ea.EMPLOYEE_MANAGER_ID,
        //                EMPLOYEE_SELF_RATING = rs.eaem.ea.EMPLOYEE_SELF_RATING,
        //                EMPLOYEE_APPRAISER_RATING = rs.eaem.ea.EMPLOYEE_APPRAISER_RATING,
        //                EMPLOYEE_FINAL_RATING = rs.eaem.ea.EMPLOYEE_FINAL_RATING,
        //                APPRAISAL_STATUS = rs.eaem.ea.APPRAISAL_STATUS,
        //                APPRAISAL_STATUS_NAME = rs.ac.APP_CONSTANT_TYPE_VALUE,
        //                CREATED_BY = rs.eaem.ea.CREATED_BY,
        //                CREATED_DATE = rs.eaem.ea.CREATED_DATE,
        //                UPDATED_BY = rs.eaem.ea.UPDATED_BY,
        //                UPDATED_DATE = rs.eaem.ea.UPDATED_DATE,
        //                IsBUHeadRevert = rs.eaem.ea.IsBUHeadRevert,
        //                IsRevertRating = rs.eaem.ea.IsRevertRating,
        //                IsBuheadApproved = rs.eaem.ea.IsBUHeadApproved == null ? false : rs.eaem.ea.IsBUHeadApproved
        //            }).ToList();
        //        }
        //        else
        //        {
        //            //employeeIds = employeeId;
        //            //GetAllEmployeesForManagerRecursively(employeeIds, isAll, appCycleId);
        //            EmployeeAppraisalList = dbContext.EmployeeAppraisalMaster
        //            .Join(dbContext.EntityMaster, ea => ea.ENTITY_ID, em => em.ENTITY_ID, (ea, em) => new { ea, em })
        //            .Join(dbContext.AppConstants, eaem => eaem.ea.APPRAISAL_STATUS, ac => ac.APP_CONSTANT_ID, (eaem, ac) => new { eaem, ac })
        //            .Where(rs => rs.eaem.ea.APP_CYCLE_ID == appCycleId && employeeId.Contains(rs.eaem.ea.EMPLOYEE_ID))
        //            .Select(rs => new EmployeeAppraisalMasterDetailView
        //            {
        //                APP_CYCLE_ID = rs.eaem.ea.APP_CYCLE_ID,
        //                EMPLOYEE_ID = rs.eaem.ea.EMPLOYEE_ID,
        //                ENTITY_ID = rs.eaem.ea.ENTITY_ID,
        //                ENTITY_NAME = rs.eaem.em.ENTITY_NAME,
        //                ENTITY_SHORT_NAME = rs.eaem.em.ENTITY_SHORT_NAME,
        //                EMPLOYEE_ROLE_ID = rs.eaem.ea.EMPLOYEE_ROLE_ID,
        //                EMPLOYEE_DEPT_ID = rs.eaem.ea.EMPLOYEE_DEPT_ID,
        //                EMPLOYEE_MANAGER_ID = rs.eaem.ea.EMPLOYEE_MANAGER_ID,
        //                EMPLOYEE_SELF_RATING = rs.eaem.ea.EMPLOYEE_SELF_RATING,
        //                EMPLOYEE_APPRAISER_RATING = rs.eaem.ea.EMPLOYEE_APPRAISER_RATING,
        //                EMPLOYEE_FINAL_RATING = rs.eaem.ea.EMPLOYEE_FINAL_RATING,
        //                APPRAISAL_STATUS = rs.eaem.ea.APPRAISAL_STATUS,
        //                APPRAISAL_STATUS_NAME = rs.ac.APP_CONSTANT_TYPE_VALUE,
        //                CREATED_BY = rs.eaem.ea.CREATED_BY,
        //                CREATED_DATE = rs.eaem.ea.CREATED_DATE,
        //                UPDATED_BY = rs.eaem.ea.UPDATED_BY,
        //                UPDATED_DATE = rs.eaem.ea.UPDATED_DATE,
        //                IsBUHeadRevert = rs.eaem.ea.IsBUHeadRevert,
        //                IsRevertRating = rs.eaem.ea.IsRevertRating,
        //                IsBuheadApproved = rs.eaem.ea.IsBUHeadApproved == null ? false : rs.eaem.ea.IsBUHeadApproved
        //            }).ToList();

        //        }
        //    }
        //    return EmployeeAppraisalList;
        //}
        //private List<int> GetAllEmployeesForManagerRecursively(List<int> employeeId, bool isAll,int appCycleId)
        //{
        //    List<int> recList = dbContext.EmployeeAppraisalMaster.Where(x => x.EMPLOYEE_ID != (int)x.EMPLOYEE_MANAGER_ID && x.APP_CYCLE_ID== appCycleId
        //                       && employeeId.Contains((int)x.EMPLOYEE_MANAGER_ID)).Select(x => x.EMPLOYEE_ID).Distinct().ToList();
        //    if (recList != null && recList?.Count > 0)
        //    {
        //        if (i == 0)
        //        {
        //            employeeIds = new List<int>();
        //            employeeIds = employeeIds.Concat(recList).ToList();
        //        }
        //        else
        //        {
        //            employeeIds = employeeIds.Concat(recList).ToList();
        //        }
        //        i++;
        //        if (isAll)
        //        {
        //            GetAllEmployeesForManagerRecursively(recList, isAll, appCycleId);
        //        }
        //    }
        //    if (i == 0)
        //    {
        //        employeeIds = new List<int> { 0 };
        //    }
        //    return employeeIds;
        //}
        public List<AppraisalMilestonedetails> GetAppraisalMilestonedetails(int appCycleId)
        {
            List<AppraisalMilestonedetails> appraisalmilestone = new List<AppraisalMilestonedetails>();
            AppraisalMaster milestone = new AppraisalMaster();
            if (appCycleId > 0)
            {
                milestone = dbContext.AppraisalMaster.Where(am => am.APP_CYCLE_ID == appCycleId).FirstOrDefault();
            }
            else
            {
                milestone = dbContext.AppraisalMaster.Where(am => am.APPRAISAL_STATUS == 1).FirstOrDefault();
            }
            if (milestone != null)
            {
                bool isCompleted = false;
                if (milestone.APPRAISEE_REVIEW_START_DATE <= DateTime.Now)
                {
                    isCompleted = true;
                }
                appraisalmilestone.Add(new AppraisalMilestonedetails { MilestoneName = "Start Your Appraisal", MilestoneDate = milestone.APPRAISEE_REVIEW_START_DATE, MilestoneStaus = isCompleted });
                if (isCompleted && milestone.APPRAISEE_REVIEW_END_DATE <= DateTime.Now)
                {
                    isCompleted = true;
                }
                else
                {
                    isCompleted = false;
                }
                appraisalmilestone.Add(new AppraisalMilestonedetails { MilestoneName = "Submit Your Appraisal", MilestoneDate = milestone.APPRAISEE_REVIEW_END_DATE, MilestoneStaus = isCompleted });
                if (isCompleted && milestone.APPRAISER_REVIEW_START_DATE <= DateTime.Now)
                {
                    isCompleted = true;
                }
                else
                {
                    isCompleted = false;
                }
                appraisalmilestone.Add(new AppraisalMilestonedetails { MilestoneName = "Start of Review", MilestoneDate = milestone.APPRAISER_REVIEW_START_DATE, MilestoneStaus = isCompleted });
                if (isCompleted && milestone.APPRAISER_REVIEW_END_DATE <= DateTime.Now)
                {
                    isCompleted = true;
                }
                else
                {
                    isCompleted = false;
                }
                appraisalmilestone.Add(new AppraisalMilestonedetails { MilestoneName = "Finish of Review", MilestoneDate = milestone.APPRAISER_REVIEW_END_DATE, MilestoneStaus = isCompleted });
                if (isCompleted && milestone.MGMT_REVIEW_START_DATE <= DateTime.Now)
                {
                    isCompleted = true;
                }
                else
                {
                    isCompleted = false;
                }
                appraisalmilestone.Add(new AppraisalMilestonedetails { MilestoneName = "Get Final Approval", MilestoneDate = milestone.MGMT_REVIEW_START_DATE, MilestoneStaus = isCompleted });
                if (isCompleted && milestone.MGMT_REVIEW_END_DATE <= DateTime.Now)
                {
                    isCompleted = true;
                }
                else
                {
                    isCompleted = false;
                }
                appraisalmilestone.Add(new AppraisalMilestonedetails { MilestoneName = "Finish Your Appraisal", MilestoneDate = milestone.MGMT_REVIEW_END_DATE, MilestoneStaus = isCompleted });

            }
            return appraisalmilestone;
        }

        public EmployeeAppraisalMaster GetAppCycleEmployee(int appCycleId, int employeeId)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleId && x.EMPLOYEE_ID == employeeId).Select(x => x).FirstOrDefault();
        }
        public List<EmployeeAppraisalMasterDetailView> GetAllAppCycleEmployee(int appCycleId)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleId).Select(x =>
            new EmployeeAppraisalMasterDetailView
            {
                APP_CYCLE_ID = x.APP_CYCLE_ID,
                APPRAISAL_STATUS_NAME = dbContext.AppConstants.Where(y => y.APP_CONSTANT_ID == x.APPRAISAL_STATUS).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
                EMPLOYEE_APPRAISER_RATING = x.EMPLOYEE_APPRAISER_RATING,
                APPRAISAL_STATUS = x.APPRAISAL_STATUS,
                CREATED_BY = x.CREATED_BY,
                EMPLOYEE_DEPT_ID = x.EMPLOYEE_DEPT_ID,
                EMPLOYEE_ROLE_ID = x.EMPLOYEE_ROLE_ID,
                EMPLOYEE_FINAL_RATING = x.EMPLOYEE_FINAL_RATING,
                EMPLOYEE_MANAGER_ID = x.EMPLOYEE_MANAGER_ID,
                EMPLOYEE_ID = x.EMPLOYEE_ID,
                ENTITY_NAME = dbContext.EntityMaster.Where(y => y.ENTITY_ID == x.ENTITY_ID).Select(y => y.ENTITY_NAME).FirstOrDefault(),
                EMPLOYEE_SELF_RATING = x.EMPLOYEE_SELF_RATING
            }).ToList();
        }
        public bool checkAppCycleUsedEmployeeMaster(int appCycleId)
        {
            EmployeeAppraisalMaster appMaster = dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleId).FirstOrDefault();
            return appMaster == null ? false : true;
        }
        public EmployeeAppraisalMaster GetEmployeeAppraisalMasterByIDs(int appCycleId, int employeeId, int entityId)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleId && x.EMPLOYEE_ID == employeeId && x.ENTITY_ID == entityId).Select(x => x).FirstOrDefault();
        }

        public List<EmployeeAppraisalByManager> GetEmployeeAppraisalMasterByManagerID(int appCycleID, int managerID)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_MANAGER_ID == managerID).Select(x =>
             new EmployeeAppraisalByManager
             {
                 APP_CYCLE_ID = x.APP_CYCLE_ID,
                 EMPLOYEE_ID = x.EMPLOYEE_ID,
                 ENTITY_ID = x.ENTITY_ID,
                 EMPLOYEE_DEPT_ID = x.EMPLOYEE_DEPT_ID,
                 EMPLOYEE_ROLE_ID = x.EMPLOYEE_ROLE_ID,
                 //EMPLOYEE_MANAGER_ID = x.EMPLOYEE_MANAGER_ID,
                 APPRAISAL_STATUS = x.APPRAISAL_STATUS,
                 APPRAISAL_STATUS_NAME = dbContext.AppConstants.Where(y => y.APP_CONSTANT_ID == x.APPRAISAL_STATUS).Select(y => y.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
             }).ToList();
        }
        public AppraisalEmployeeStatusView GetEmployeeAppraisalStatusById(int appcycleId, int departmentId, int roleId, int employeeId)
        {
            AppraisalEmployeeStatusView appraisal = (from apm in dbContext.EmployeeAppraisalMaster
                                                     join ac in dbContext.AppConstants on apm.APPRAISAL_STATUS equals ac.APP_CONSTANT_ID
                                                     where apm.APP_CYCLE_ID == appcycleId && apm.EMPLOYEE_ID == employeeId && apm.EMPLOYEE_DEPT_ID == departmentId
                                                      && apm.EMPLOYEE_ROLE_ID == roleId
                                                     select new AppraisalEmployeeStatusView
                                                     {
                                                         ApprisalStatusID = apm.APPRAISAL_STATUS,
                                                         ApprisalStatus = ac.APP_CONSTANT_TYPE_VALUE,
                                                         EmployeeSelfRating = apm.EMPLOYEE_SELF_RATING,
                                                         IsBUHeadRevert = apm.IsBUHeadRevert,
                                                         IsRevertRating = apm.IsRevertRating

                                                     }).FirstOrDefault();
            if (appraisal != null)
            {
                bool? isApproved = dbContext.AppraisalBUHeadComments.Where(x => x.AppCycle_Id == appcycleId && x.Department_Id == departmentId)?.Select(x => x)?.ToList()?.Count > 0 ? true : false;
                appraisal.IsBUHeadApproved = isApproved == null ? false : isApproved;
            }
            return appraisal;
        }
        public int GetCurrentAppcycleId()
        {
            return dbContext.AppraisalMaster.Where(am => am.APPRAISAL_STATUS == 1).Select(x => x.APP_CYCLE_ID).FirstOrDefault();
        }

        public List<ManagerDetail> GetManagerDetails(int appCycleID, int managerID)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_ID == managerID).Select(x =>
               new ManagerDetail
               {
                   APP_CYCLE_ID = x.APP_CYCLE_ID,
                   ENTITY_ID = x.ENTITY_ID,
                   MANAGER_ROLE_ID = x.EMPLOYEE_ROLE_ID,
                   MANAGER_DEPARTMENT_ID = x.EMPLOYEE_DEPT_ID,
                   MANAGER_ID = x.EMPLOYEE_ID,
               }).ToList();
        }
        public List<EmployeeAppraisalMaster> GetEmployeeByDepartment(int appCycleID, int departmentId)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_DEPT_ID == departmentId).Select(x => x).ToList();
        }
        public List<EmployeeAppraisalByManager> GetEmployeeByStatus(int appCycleID, int departmentId)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_DEPT_ID == departmentId).Select(x =>
             new EmployeeAppraisalByManager
             {
                 APP_CYCLE_ID = x.APP_CYCLE_ID,
                 EMPLOYEE_ID = x.EMPLOYEE_ID,
                 ENTITY_ID = x.ENTITY_ID,
                 EMPLOYEE_DEPT_ID = x.EMPLOYEE_DEPT_ID,
                 EMPLOYEE_ROLE_ID = x.EMPLOYEE_ROLE_ID,
                 APPRAISAL_STATUS = x.APPRAISAL_STATUS,
                 APPRAISAL_STATUS_NAME = dbContext.AppConstants.Where(y => y.APP_CONSTANT_ID == x.APPRAISAL_STATUS).Select(y => y.APP_CONSTANT_TYPE_VALUE).FirstOrDefault(),
             }).ToList();
        }

        public EmployeeAppraisalMaster GetEmployeeAppraisalMasterByAppandEmpandManIDs(int appCycleID, int employeeID, int managerID)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_ID == employeeID && x.EMPLOYEE_MANAGER_ID == managerID).Select(x => x).FirstOrDefault();
        }
        public IndividualEmployeeAppraisalRating GetEmployeeAppraisalRating(int employeeID)
        {
            IndividualEmployeeAppraisalRating EmployeeAppraisalRating = new IndividualEmployeeAppraisalRating();
            List<EmployeeAppraisalRatingView> employeeAppraisal = new List<EmployeeAppraisalRatingView>();
            //int? appCycleId = Convert.ToInt32(dbContext.AppraisalMaster.Where(am => am.APPRAISAL_STATUS == 1).FirstOrDefault().APP_CYCLE_ID);
            EmployeeAppraisalRating.Employee_Id = employeeID;

            employeeAppraisal =(from ea in dbContext.EmployeeAppraisalMaster
                                join am in dbContext.AppraisalMaster on ea.APP_CYCLE_ID equals am.APP_CYCLE_ID
                                where ea.EMPLOYEE_ID == employeeID
                                select new EmployeeAppraisalRatingView { AppCycleId=ea.APP_CYCLE_ID, FinalRating=ea.EMPLOYEE_FINAL_RATING, AppraisalStartDate=am.APP_CYCLE_START_DATE,AppraisalEndDate=am.APP_CYCLE_END_DATE}
                                ).OrderByDescending(rs => rs.AppraisalStartDate).Take(2).ToList();
            if (employeeAppraisal != null && employeeAppraisal.Count > 0)
            {
                for (int i = 0; i < employeeAppraisal.Count; i++)
                {
                    if (i == 0)
                    {
                        EmployeeAppraisalRating.Current_AppcycleId = employeeAppraisal[i].AppCycleId;
                        EmployeeAppraisalRating.Current_Rating = employeeAppraisal[i].FinalRating;
                        //var appcycle = dbContext.AppraisalMaster.Where(am => am.APP_CYCLE_ID == employeeAppraisal[i].APP_CYCLE_ID).Select(rs => new { rs.APP_CYCLE_START_DATE, rs.APP_CYCLE_END_DATE }).FirstOrDefault();
                        //if (appcycle != null)
                        //{
                        //    EmployeeAppraisalRating.CurrentAppcycle_monthyear = appcycle.APP_CYCLE_START_DATE?.ToString("MMM YYYY") + "-" + appcycle.APP_CYCLE_END_DATE?.ToString("MMM YYYY");
                        //}
                        EmployeeAppraisalRating.CurrentAppcycle_monthyear = employeeAppraisal[i]?.AppraisalStartDate?.ToString("MMM yyyy") + "-" + employeeAppraisal[i]?.AppraisalEndDate?.ToString("MMM yyyy");
                    }
                    else
                    {
                        EmployeeAppraisalRating.Previous_AppcycleId = employeeAppraisal[i].AppCycleId;
                        EmployeeAppraisalRating.Previous_Rating = employeeAppraisal[i].FinalRating;
                        //var appcycle = dbContext.AppraisalMaster.Where(am => am.APP_CYCLE_ID == employeeAppraisal[i].APP_CYCLE_ID).Select(rs => new { rs.APP_CYCLE_START_DATE, rs.APP_CYCLE_END_DATE }).FirstOrDefault();
                        //if (appcycle != null)
                        //{
                        //    EmployeeAppraisalRating.PreviousAppcycle_monthyear = appcycle.APP_CYCLE_START_DATE?.ToString("MMM YYYY") + "-" + appcycle.APP_CYCLE_END_DATE?.ToString("MMM YYYY");
                        //}
                        EmployeeAppraisalRating.PreviousAppcycle_monthyear = employeeAppraisal[i]?.AppraisalStartDate?.ToString("MMM yyyy") + "-" + employeeAppraisal[i]?.AppraisalEndDate?.ToString("MMM yyyy");
                    }
                }
            }

            return EmployeeAppraisalRating;
        }
        public List<EmployeeAppraisalMaster> GetAppCycleEmployeeListByAppCycleId(int appCycleId)
        {
            return dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == appCycleId).ToList();
        }
    }
}
