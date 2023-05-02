using System;
using System.Collections.Generic;
using System.Text;
using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System.Linq;
using SharedLibraries.ViewModels.Appraisal;

namespace Appraisal.DAL.Repository
{
    public interface IEmpObjectiveRatingRepository : IBaseRepository<EmployeeObjectiveRating>
    {
        List<EmployeeObjectiveRating> GetObjectiveByID(int appCycleId,int employeeId);
        AppraisalReport GetAppraisalObjectiveRatingDetails(int employeeId);
        EmployeeObjectiveRating GetObjectiveRating(int appCycleId, int employeeId, int ObjectiveId);
    }
   public class EmpObjectiveRatingRepository : BaseRepository<EmployeeObjectiveRating>, IEmpObjectiveRatingRepository
    {
        private readonly AppraisalDBContext dbContext;
        public EmpObjectiveRatingRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<EmployeeObjectiveRating> GetObjectiveByID(int appCycleId, int employeeId)
        {
            return dbContext.EmployeeObjectiveRating.Where(x => x.APP_CYCLE_ID == appCycleId && x.EMPLOYEE_ID == employeeId).ToList();
        }
        public AppraisalReport GetAppraisalObjectiveRatingDetails(int employeeId)
        {
            AppraisalReport appraisalReport = new AppraisalReport();
            appraisalReport.objectiveRating = (from versionObj in dbContext.VersionDepartmentRoleObjective
                              //join objRating in dbContext.EmployeeObjectiveRating on versionObj.OBJECTIVE_ID equals objRating.OBJECTIVE_ID
                              join objMaster in dbContext.ObjectiveMaster on versionObj.OBJECTIVE_ID equals objMaster.OBJECTIVE_ID
                              join appMaster in dbContext.AppraisalMaster on versionObj.VERSION_ID equals appMaster.VERSION_ID
                              join empmaster in dbContext.EmployeeAppraisalMaster on appMaster.APP_CYCLE_ID equals empmaster.APP_CYCLE_ID
                              where empmaster.EMPLOYEE_ID==employeeId && appMaster.APPRAISAL_STATUS==1 &&
                              versionObj.DEPT_ID==empmaster.EMPLOYEE_DEPT_ID && versionObj.ROLE_ID == empmaster.EMPLOYEE_ROLE_ID
                                               select new EmployeeObjectiveRatingView
                              {
                                  VersionId = versionObj.VERSION_ID,
                                  ObjectiveId = versionObj.OBJECTIVE_ID,
                                  ObjectiveName = objMaster.OBJECTIVE_NAME,
                                  Rating = dbContext.EmployeeObjectiveRating.Where(x=>x.EMPLOYEE_ID==employeeId && x.APP_CYCLE_ID==empmaster.APP_CYCLE_ID
                                  && x.OBJECTIVE_ID== versionObj.OBJECTIVE_ID).Select(x=>x.OBJECTIVE_RATING).FirstOrDefault(),
                                  MaxRating = dbContext.EmployeeObjectiveRating.Where(x => x.EMPLOYEE_ID == employeeId && x.APP_CYCLE_ID == empmaster.APP_CYCLE_ID
                                  && x.OBJECTIVE_ID == versionObj.OBJECTIVE_ID).Select(x => x.OBJECTIVE_MAX_RATING).FirstOrDefault()
                                               }).ToList();
           
            List<AppraisalMilestonedetails> appraisalmilestone = new List<AppraisalMilestonedetails>();
            AppraisalMaster milestone = dbContext.AppraisalMaster.Where(am => am.APPRAISAL_STATUS == 1).FirstOrDefault();
            if(milestone !=null)
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
                decimal? finalRating= dbContext.EmployeeAppraisalMaster.Where(x => x.APP_CYCLE_ID == milestone.APP_CYCLE_ID && x.EMPLOYEE_ID == employeeId).Select(x => x.EMPLOYEE_SELF_RATING).FirstOrDefault();
                appraisalReport.TotalScore = finalRating == null ? 0 : (int)finalRating;
            }
            appraisalReport.appraisalMilestonedetails = appraisalmilestone;
            //var score = (from versionObj in dbContext.VersionDepartmentRoleObjective
            //                  join objRating in dbContext.EmployeeObjectiveRating on versionObj.OBJECTIVE_ID equals objRating.OBJECTIVE_ID
            //                  join objMaster in dbContext.ObjectiveMaster on versionObj.OBJECTIVE_ID equals objMaster.OBJECTIVE_ID
            //                  where versionObj.VERSION_ID == versionId && objRating.EMPLOYEE_ID == employeeId
            //                  group objRating by objRating.OBJECTIVE_ID into totalScore
            //                  select new
            //                  {
            //                      total = (totalScore.Select(x => x.OBJECTIVE_RATING).Sum())
            //                  }).FirstOrDefault();
            //appraisalReport.TotalScore = score.total.Value;

            return appraisalReport;
        }

        public EmployeeObjectiveRating GetObjectiveRating(int appCycleId, int employeeId, int ObjectiveId)
        {
            return dbContext.EmployeeObjectiveRating.Where(x => x.APP_CYCLE_ID == appCycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == ObjectiveId ).FirstOrDefault();
        }
    }
}
