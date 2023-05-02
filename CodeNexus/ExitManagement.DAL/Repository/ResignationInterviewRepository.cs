using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
    public interface IResignationInterviewRepository : IBaseRepository<ResignationInterview>
    {
        ResignationInterview GetByResignationInterviewID(int EmployeeResignationInterviewId);
        ResignationInterviewDetailView GetResignationInterviewDetailById(int resignationInterviewId);
        List<ResignationInterviewDetailView> GetResignationInterviewDetailByEmployeeId(int employeeId);

        ResignationInterview GetCheckExitInterviewEnable(int employeeId , DateTime? resignationDate);
        List<ResignationEmployeeMasterView> GetLastResignationInterviewByEmployeeList(List<ResignationEmployeeMasterView> employeeList);
        int GetResignationInterviewByEmployeeId(int employeeId);
    }
    public class ResignationInterviewRepository : BaseRepository<ResignationInterview>, IResignationInterviewRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ResignationInterviewRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public ResignationInterview GetByResignationInterviewID(int EmployeeResignationInterviewId)
        {
            return dbContext.ResignationInterview.Where(x => x.ResignationInterviewId == EmployeeResignationInterviewId).FirstOrDefault();
        }
        public ResignationInterviewDetailView GetResignationInterviewDetailById(int resignationInterviewId)
        {
            return (from interview in dbContext.ResignationInterview join feedback in dbContext.ResignationFeedbackToManagement
                    on interview.ResignationInterviewId equals feedback.ResignationInterviewId
                    where interview.ResignationInterviewId == resignationInterviewId
                    select new ResignationInterviewDetailView
                    {
                        ResignationInterviewId = interview.ResignationInterviewId,
                        EmployeeID = interview.EmployeeID,
                        OverallView = interview.OverallView,
                        ReasonOfRelievingPositionId = dbContext.ReasonLeavingPosition.Where(x => x.ResignationInterviewId == interview.ResignationInterviewId).Select(x => x.LeavingPositionId).ToList(),
                        ReasonOfRelieving = interview.ReasonOfRelieving,
                        EventTriggeredForAlternativeJob = interview.EventTriggeredForAlternativeJob,
                        ShareProspectiveEmployer = interview.ShareProspectiveEmployer,
                        AttractedProspectiveEmployer = interview.AttractedProspectiveEmployer,
                        UnresolvedIssues = interview.UnresolvedIssues,
                        EnjoyDislike = interview.EnjoyDislike,
                        FeelAboutManagement = interview.FeelAboutManagement,
                        FeedbackOnPerformance = interview.FeedbackOnPerformance,
                        SufficientSupportTraining = interview.SufficientSupportTraining,
                        PreventFromLeaving = interview.PreventFromLeaving,
                        RejoinORRecommend = interview.RejoinORRecommend,
                        Suggestion = interview.Suggestion,
                        TrainingId = feedback.TrainingId,
                        TrainingRemark = feedback.TrainingRemark,
                        NatureOfWorkId = feedback.NatureOfWorkId,
                        NatureOfWorkRemark = feedback.NatureOfWorkRemark,
                        ImmediateSupervisorInvolmentId = feedback.ImmediateSupervisorInvolmentId,
                        ImmediateSupervisorInvolmentRemark = feedback.ImmediateSupervisorInvolmentRemark,
                        JobRecognitionId = feedback.JobRecognitionId,
                        JobRecognitionRemark = feedback.JobRecognitionRemark,
                        PerformanceFeedbackId = feedback.PerformanceFeedbackId,
                        PerformanceFeedbackRemark = feedback.PerformanceFeedbackRemark,
                        GrowthOpportunityId = feedback.GrowthOpportunityId,
                        GrowthOpportunityRemark = feedback.GrowthOpportunityRemark,
                        NewSkillsOpportunityId = feedback.NewSkillsOpportunityId,
                        NewSkillsOpportunityRemark = feedback.NewSkillsOpportunityRemark,
                        CompensationId = feedback.CompensationId,
                        CompensationRemark = feedback.CompensationRemark,
                        AnnualIncrementId = feedback.AnnualIncrementId,
                        AnnualIncrementRemark = feedback.AnnualIncrementRemark,
                        InformationSharingId = feedback.InformationSharingId,
                        InformationSharingRemark = feedback.InformationSharingRemark,
                        Other = feedback.Other,
                        OtherRemark = feedback.OtherRemark,
                        OrgPoliciesId = feedback.OrgPoliciesId,
                        OrgPoliciesRemark = feedback.OrgPoliciesRemark,
                        CreatedOn = feedback.CreatedOn,
                        CreatedBy = feedback.CreatedBy,
                        ModifiedOn = feedback.ModifiedOn,
                        ModifiedBy = feedback.ModifiedBy

                    }).FirstOrDefault();
        }
        public List<ResignationInterviewDetailView> GetResignationInterviewDetailByEmployeeId(int employeeId)
        {
            if (employeeId > 0)
            {
                return (from interview in dbContext.ResignationInterview
                        join feedback in dbContext.ResignationFeedbackToManagement
                        on interview.ResignationInterviewId equals feedback.ResignationInterviewId
                        where interview.EmployeeID == employeeId
                        select new ResignationInterviewDetailView
                        {
                            ResignationInterviewId = interview.ResignationInterviewId,
                            EmployeeID = interview.EmployeeID,
                            OverallView = interview.OverallView,
                            ReasonOfRelievingPositionId = dbContext.ReasonLeavingPosition.Where(x => x.ResignationInterviewId == interview.ResignationInterviewId).Select(x => x.LeavingPositionId).ToList(),
                            ReasonOfRelieving = interview.ReasonOfRelieving,
                            EventTriggeredForAlternativeJob = interview.EventTriggeredForAlternativeJob,
                            ShareProspectiveEmployer = interview.ShareProspectiveEmployer,
                            AttractedProspectiveEmployer = interview.AttractedProspectiveEmployer,
                            UnresolvedIssues = interview.UnresolvedIssues,
                            EnjoyDislike = interview.EnjoyDislike,
                            FeelAboutManagement = interview.FeelAboutManagement,
                            FeedbackOnPerformance = interview.FeedbackOnPerformance,
                            SufficientSupportTraining = interview.SufficientSupportTraining,
                            PreventFromLeaving = interview.PreventFromLeaving,
                            RejoinORRecommend = interview.RejoinORRecommend,
                            Suggestion = interview.Suggestion,
                            TrainingId = feedback.TrainingId,
                            TrainingRemark = feedback.TrainingRemark,
                            NatureOfWorkId = feedback.NatureOfWorkId,
                            NatureOfWorkRemark = feedback.NatureOfWorkRemark,
                            ImmediateSupervisorInvolmentId = feedback.ImmediateSupervisorInvolmentId,
                            ImmediateSupervisorInvolmentRemark = feedback.ImmediateSupervisorInvolmentRemark,
                            JobRecognitionId = feedback.JobRecognitionId,
                            JobRecognitionRemark = feedback.JobRecognitionRemark,
                            PerformanceFeedbackId = feedback.PerformanceFeedbackId,
                            PerformanceFeedbackRemark = feedback.PerformanceFeedbackRemark,
                            GrowthOpportunityId = feedback.GrowthOpportunityId,
                            GrowthOpportunityRemark = feedback.GrowthOpportunityRemark,
                            NewSkillsOpportunityId = feedback.NewSkillsOpportunityId,
                            NewSkillsOpportunityRemark = feedback.NewSkillsOpportunityRemark,
                            CompensationId = feedback.CompensationId,
                            CompensationRemark = feedback.CompensationRemark,
                            AnnualIncrementId = feedback.AnnualIncrementId,
                            AnnualIncrementRemark = feedback.AnnualIncrementRemark,
                            InformationSharingId = feedback.InformationSharingId,
                            InformationSharingRemark = feedback.InformationSharingRemark,
                            Other = feedback.Other,
                            OtherRemark = feedback.OtherRemark,
                            OrgPoliciesId = feedback.OrgPoliciesId,
                            OrgPoliciesRemark = feedback.OrgPoliciesRemark,
                            CreatedOn = feedback.CreatedOn,
                            CreatedBy = feedback.CreatedBy,
                            ModifiedOn = feedback.ModifiedOn,
                            ModifiedBy = feedback.ModifiedBy,
                            ResignationDetailsId= interview.ResignationDetailsId
                        }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            else
            {
                return (from interview in dbContext.ResignationInterview
                        join feedback in dbContext.ResignationFeedbackToManagement
on interview.ResignationInterviewId equals feedback.ResignationInterviewId
                        select new ResignationInterviewDetailView
                        {
                            ResignationInterviewId = interview.ResignationInterviewId,
                            EmployeeID = interview.EmployeeID,
                            OverallView = interview.OverallView,
                            ReasonOfRelievingPositionId = dbContext.ReasonLeavingPosition.Where(x => x.ResignationInterviewId == interview.ResignationInterviewId).Select(x => x.LeavingPositionId).ToList(),
                            ReasonOfRelieving = interview.ReasonOfRelieving,
                            EventTriggeredForAlternativeJob = interview.EventTriggeredForAlternativeJob,
                            ShareProspectiveEmployer = interview.ShareProspectiveEmployer,
                            AttractedProspectiveEmployer = interview.AttractedProspectiveEmployer,
                            UnresolvedIssues = interview.UnresolvedIssues,
                            EnjoyDislike = interview.EnjoyDislike,
                            FeelAboutManagement = interview.FeelAboutManagement,
                            FeedbackOnPerformance = interview.FeedbackOnPerformance,
                            SufficientSupportTraining = interview.SufficientSupportTraining,
                            PreventFromLeaving = interview.PreventFromLeaving,
                            RejoinORRecommend = interview.RejoinORRecommend,
                            Suggestion = interview.Suggestion,
                            TrainingId = feedback.TrainingId,
                            TrainingRemark = feedback.TrainingRemark,
                            NatureOfWorkId = feedback.NatureOfWorkId,
                            NatureOfWorkRemark = feedback.NatureOfWorkRemark,
                            ImmediateSupervisorInvolmentId = feedback.ImmediateSupervisorInvolmentId,
                            ImmediateSupervisorInvolmentRemark = feedback.ImmediateSupervisorInvolmentRemark,
                            JobRecognitionId = feedback.JobRecognitionId,
                            JobRecognitionRemark = feedback.JobRecognitionRemark,
                            PerformanceFeedbackId = feedback.PerformanceFeedbackId,
                            PerformanceFeedbackRemark = feedback.PerformanceFeedbackRemark,
                            GrowthOpportunityId = feedback.GrowthOpportunityId,
                            GrowthOpportunityRemark = feedback.GrowthOpportunityRemark,
                            NewSkillsOpportunityId = feedback.NewSkillsOpportunityId,
                            NewSkillsOpportunityRemark = feedback.NewSkillsOpportunityRemark,
                            CompensationId = feedback.CompensationId,
                            CompensationRemark = feedback.CompensationRemark,
                            AnnualIncrementId = feedback.AnnualIncrementId,
                            AnnualIncrementRemark = feedback.AnnualIncrementRemark,
                            InformationSharingId = feedback.InformationSharingId,
                            InformationSharingRemark = feedback.InformationSharingRemark,
                            Other = feedback.Other,
                            OtherRemark = feedback.OtherRemark,
                            OrgPoliciesId = feedback.OrgPoliciesId,
                            OrgPoliciesRemark = feedback.OrgPoliciesRemark,
                            CreatedOn = feedback.CreatedOn,
                            CreatedBy = feedback.CreatedBy,
                            ModifiedOn = feedback.ModifiedOn,
                            ModifiedBy = feedback.ModifiedBy,
                            ResignationDetailsId = interview.ResignationDetailsId

                        }).OrderByDescending(x => x.CreatedOn).ToList();
            }
        }

        public List<ResignationEmployeeMasterView> GetLastResignationInterviewByEmployeeList(List<ResignationEmployeeMasterView> employeeList)
        {
            List<ResignationEmployeeMasterView> result = new List<ResignationEmployeeMasterView>();
            foreach (ResignationEmployeeMasterView item in employeeList)
            {
                int? resignId= dbContext.EmployeeResignationDetails.Where(x => x.EmployeeId == item.EmployeeID && x.ResignationStatus != "Rejected" && x.ResignationStatus != "Cancelled" && x.ResignationStatus != "Withdrawal Approved").OrderByDescending(x => x.CreatedOn).Select(x => x.EmployeeResignationDetailsId).FirstOrDefault();
                if(resignId>0)
                {
                    ResignationInterview data = dbContext.ResignationInterview.Where(x => x.ResignationDetailsId == resignId).FirstOrDefault();
                    if(data==null)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
            //var resignationInterview = dbContext.ResignationInterview.ToArray();
            //List<ResignationEmployeeMasterView> employees = (from a in resignationInterview
            //                                                 join b in employeeList.ToArray() on a.EmployeeID equals b.EmployeeID
            //                                                 where a.CreatedOn >= b.ResignationDate
            //                                                 select b).ToList();
            //return employeeList.Except(employees).ToList();
        }

        public  ResignationInterview GetCheckExitInterviewEnable(int employeeId, DateTime? resignationDate)
        {
             return dbContext.ResignationInterview.Where(x => x.EmployeeID == employeeId && x.CreatedOn >= resignationDate).FirstOrDefault();
        }
        public int GetResignationInterviewByEmployeeId(int employeeId)
        {
            int? resignId = dbContext.EmployeeResignationDetails.Where(x => x.EmployeeId == employeeId).OrderByDescending(x => x.CreatedOn).Select(x => x.EmployeeResignationDetailsId).FirstOrDefault();
            if (resignId != null && resignId > 0)
            {
                 if(dbContext.ResignationInterview.Where(x => x.EmployeeID == employeeId && x.ResignationDetailsId == resignId).Any())
                {
                    return 1;
                }
            }
            else
            {
                return 2;
            }
            return 0;
        }

    }
}
