using ExitManagement.DAL.DBContext;
using SharedLibraries.Common;
using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
    public interface IEmployeeResignationDetailsRepository : IBaseRepository<EmployeeResignationDetails>
    {
        EmployeeResignationDetails GetByResignationID(int EmployeeResignationDetailsId);
        List<EmployeeResignationDetailsView> GetAllResignationDetails(AllResignationInputView resignationInputView);
        EmployeeResignationDetailsView GetResignationDetailByResignationId(int resignationId);
        int? GetLastResignationIdByEmployeeId(int employeeId);
        List<KeyWithIntValue> GetLastResignationIdByEmployeeList(List<int> employeeIdList);
        List<int> GetChecklistSubmissionNotificationEmployee(int days);
        List<int> GetResignationInterviewNotification(int days);
        EmployeeResignationChecklistDetailsView GetEmployeeDetailByResignationID(int EmployeeResignationDetailsId);
    }
    public class EmployeeResignationDetailsRepository : BaseRepository<EmployeeResignationDetails>, IEmployeeResignationDetailsRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public EmployeeResignationDetailsRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public EmployeeResignationDetails GetByResignationID(int EmployeeResignationDetailsId)
        {
            return dbContext.EmployeeResignationDetails.Where(x => x.EmployeeResignationDetailsId == EmployeeResignationDetailsId).FirstOrDefault();
        }

        public List<EmployeeResignationDetailsView> GetAllResignationDetails(AllResignationInputView resignationInputView)
        {
            List<string> resStatus = new List<string> { "Rejected" , "Withdrawal Approved" };
            return (from resignationDetails in dbContext.EmployeeResignationDetails
                    where resignationDetails.EmployeeId == resignationInputView.EmployeeId
                    && true == (resignationInputView.IsMyData ? !resStatus.Contains(resignationDetails.ResignationStatus) : true)
                    select new EmployeeResignationDetailsView
                    {
                        EmployeeResignationDetailsId = resignationDetails.EmployeeResignationDetailsId,
                        EmployeeId = resignationDetails.EmployeeId,
                        EmployeeName = resignationDetails.EmployeeName,
                        FormattedEmployeeId = resignationDetails.FormattedEmployeeId,
                        EmployeeDesignation = resignationDetails.EmployeeDesignation,
                        MobileNumber = resignationDetails.MobileNumber,
                        PersonalEmailAddress = resignationDetails.PersonalEmailAddress,
                        AddressLine1 = resignationDetails.AddressLine1,
                        AddressLine2 = resignationDetails.AddressLine2,
                        ResignationReasonId = resignationDetails.ResignationReasonId,
                        ResignationStatus = resignationDetails.ResignationStatus,
                        ResignationDate = resignationDetails.ResignationDate,
                        ActualRelievingDate = resignationDetails.ActualRelievingDate,
                        RelievingDate = resignationDetails.RelievingDate,
                        CreatedOn = resignationDetails.CreatedOn,
                        CreatedBy = resignationDetails.CreatedBy,
                        ModifiedBy = resignationDetails.ModifiedBy,
                        ModifiedOn = resignationDetails.ModifiedOn,
                        ResignationReason = resignationDetails.ResignationReason == null ? dbContext.ResignationReason.Where(x => x.ResignationReasonId == resignationDetails.ResignationReasonId).Select(x => x.ResignationReasonName).FirstOrDefault() : resignationDetails.ResignationReason,
                        WithdrawalReason = resignationDetails.WithdrawalReason,
                        ZipCode = resignationDetails.ZipCode,
                        City = resignationDetails.City,
                        State = resignationDetails.State,
                        Country = resignationDetails.Country,
                        DepartmentName = resignationDetails.Department,
                        Location = resignationDetails.Location,
                        ResignationType = resignationDetails.ResignationType,
                        Remarks = resignationDetails.Remarks,
                        ProfilePicture = resignationDetails.ProfilePicture,
                        ReportingManager = resignationDetails.ReportingManager,
                        ReportingManagerID = resignationDetails.ReportingManagerId,
                        IsAgreeCheckList = resignationDetails.ResignationStatus != "Pending" ? dbContext.ResignationChecklist.Where(x => x.ResignationDetailsId == resignationDetails.EmployeeResignationDetailsId).Select(x => x.IsAgreeCheckList).FirstOrDefault() : false,
                        ResignationApproverList = (from a in dbContext.ResignationApproval
                                                   join b in dbContext.ResignationApprovalStatus
                                                   on a.LevelId equals b.LevelId
                                                   join c in dbContext.AppConstants on a.LevelApprovalId equals c.AppConstantId
                                                   where b.EmployeeResignationDetailsId == resignationDetails.EmployeeResignationDetailsId
                                                   select new ResignationApprovalStatusView { LevelId = a.LevelId, ApproverType = c.DisplayName, FeedBack = b.FeedBack, Status = b.Status, ApproverEmployeeId = b.ApproverEmployeeId, ApprovalType = b.ApprovalType }).ToList(),
                        ResignationInterviewDetailId = (from rin in dbContext.ResignationInterview.Where(ri=> ri.ResignationDetailsId == resignationDetails.EmployeeResignationDetailsId) select rin.ResignationInterviewId).FirstOrDefault(),
                        WithdrawalSubmmitedDate = resignationDetails.WithdrawalSubmmitedDate
                    }).OrderByDescending(x => x.CreatedOn).ToList();
        }
        public List<EmployeeResignationDetailsView> GetAllResignationDetails_Backup(AllResignationInputView resignationInputView)
        {
            //List<EmployeeResignationDetailsView> resignationList = new List<EmployeeResignationDetailsView>();
            if (resignationInputView.IsAdmin == true)
            {
                return (from resignationDetails in dbContext.EmployeeResignationDetails
                        select new EmployeeResignationDetailsView
                        {
                            EmployeeResignationDetailsId = resignationDetails.EmployeeResignationDetailsId,
                            EmployeeId = resignationDetails.EmployeeId,
                            EmployeeDesignation = resignationDetails.EmployeeDesignation,
                            MobileNumber = resignationDetails.MobileNumber,
                            PersonalEmailAddress = resignationDetails.PersonalEmailAddress,
                            AddressLine1 = resignationDetails.AddressLine1,
                            AddressLine2 = resignationDetails.AddressLine2,
                            ResignationReasonId = resignationDetails.ResignationReasonId,
                            ResignationStatus = resignationDetails.ResignationStatus,
                            ResignationDate = resignationDetails.ResignationDate,
                            ActualRelievingDate = resignationDetails.ActualRelievingDate,
                            RelievingDate = resignationDetails.RelievingDate,
                            CreatedOn = resignationDetails.CreatedOn,
                            CreatedBy = resignationDetails.CreatedBy,
                            ModifiedBy = resignationDetails.ModifiedBy,
                            ModifiedOn = resignationDetails.ModifiedOn,
                            ResignationReason = resignationDetails.ResignationReason == null ? dbContext.ResignationReason.Where(x => x.ResignationReasonId == resignationDetails.ResignationReasonId).Select(x => x.ResignationReasonName).FirstOrDefault() : resignationDetails.ResignationReason,
                            WithdrawalReason = resignationDetails.WithdrawalReason,
                            ZipCode = resignationDetails.ZipCode,
                            City = resignationDetails.City,
                            State = resignationDetails.State,
                            Country = resignationDetails.Country,
                            DepartmentName = resignationDetails.Department,
                            Location = resignationDetails.Location,
                            ResignationType = resignationDetails.ResignationType,
                            Remarks = resignationDetails.Remarks,
                            ProfilePicture = resignationDetails.ProfilePicture,
                            ReportingManager = resignationDetails.ReportingManager,
                            ReportingManagerID = resignationDetails.ReportingManagerId,
                            IsAgreeCheckList = resignationDetails.ResignationStatus != "Pending" ? dbContext.ResignationChecklist.Where(x => x.ResignationDetailsId == resignationDetails.EmployeeResignationDetailsId).Select(x => x.IsAgreeCheckList).FirstOrDefault() : false,
                            ResignationApproverList = (from a in dbContext.ResignationApproval
                                                       join b in dbContext.ResignationApprovalStatus
                                                       on a.LevelId equals b.LevelId
                                                       join c in dbContext.AppConstants on a.LevelApprovalId equals c.AppConstantId
                                                       where b.EmployeeResignationDetailsId == resignationDetails.EmployeeResignationDetailsId
                                                       select new ResignationApprovalStatusView { LevelId = a.LevelId, ApproverType = c.DisplayName, FeedBack = b.FeedBack, Status = b.Status, ApproverEmployeeId = b.ApproverEmployeeId, ApprovalType = b.ApprovalType }).ToList()
                        }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            else
            {
                List<EmployeeResignationDetailsView> employeeResignation = (from resignationDetails in dbContext.EmployeeResignationDetails
                                                                            where resignationInputView.ReporteesList.Contains(resignationDetails.EmployeeId)
                                                                            select new EmployeeResignationDetailsView
                                                                            {
                                                                                EmployeeResignationDetailsId = resignationDetails.EmployeeResignationDetailsId,
                                                                                EmployeeId = resignationDetails.EmployeeId,
                                                                                EmployeeDesignation = resignationDetails.EmployeeDesignation,
                                                                                //ResidanceContactNumber = resignationDetails.ResidanceContactNumber,
                                                                                MobileNumber = resignationDetails.MobileNumber,
                                                                                PersonalEmailAddress = resignationDetails.PersonalEmailAddress,
                                                                                AddressLine1 = resignationDetails.AddressLine1,
                                                                                AddressLine2 = resignationDetails.AddressLine2,
                                                                                ResignationReasonId = resignationDetails.ResignationReasonId,
                                                                                ResignationStatus = resignationDetails.ResignationStatus,
                                                                                ResignationDate = resignationDetails.ResignationDate,
                                                                                ActualRelievingDate = resignationDetails.ActualRelievingDate,
                                                                                RelievingDate = resignationDetails.RelievingDate,
                                                                                CreatedOn = resignationDetails.CreatedOn,
                                                                                CreatedBy = resignationDetails.CreatedBy,
                                                                                ModifiedBy = resignationDetails.ModifiedBy,
                                                                                ModifiedOn = resignationDetails.ModifiedOn,
                                                                                ResignationReason = resignationDetails.ResignationReason == null ? dbContext.ResignationReason.Where(x => x.ResignationReasonId == resignationDetails.ResignationReasonId).Select(x => x.ResignationReasonName).FirstOrDefault() : resignationDetails.ResignationReason,
                                                                                WithdrawalReason = resignationDetails.WithdrawalReason,
                                                                                ZipCode = resignationDetails.ZipCode,
                                                                                City = resignationDetails.City,
                                                                                State = resignationDetails.State,
                                                                                Country = resignationDetails.Country,
                                                                                DepartmentName = resignationDetails.Department,
                                                                                Location = resignationDetails.Location,
                                                                                ResignationType = resignationDetails.ResignationType,
                                                                                Remarks = resignationDetails.Remarks,
                                                                                ProfilePicture = resignationDetails.ProfilePicture,
                                                                                ReportingManager = resignationDetails.ReportingManager,
                                                                                ReportingManagerID = resignationDetails.ReportingManagerId,
                                                                                IsAgreeCheckList = resignationDetails.ResignationStatus != "Pending" ? dbContext.ResignationChecklist.Where(x => x.ResignationDetailsId == resignationDetails.EmployeeResignationDetailsId).Select(x => x.IsAgreeCheckList).FirstOrDefault() : false,
                                                                                ResignationApproverList = (from a in dbContext.ResignationApproval
                                                                                                           join b in dbContext.ResignationApprovalStatus
                                                                                                           on a.LevelId equals b.LevelId
                                                                                                           join c in dbContext.AppConstants on a.LevelApprovalId equals c.AppConstantId
                                                                                                           where b.EmployeeResignationDetailsId == resignationDetails.EmployeeResignationDetailsId
                                                                                                           select new ResignationApprovalStatusView { LevelId = a.LevelId, ApproverType = c.DisplayName, FeedBack = b.FeedBack, Status = b.Status, ApproverEmployeeId = b.ApproverEmployeeId, ApprovalType = b.ApprovalType }).ToList()
                                                                            }).OrderByDescending(x => x.CreatedOn).ToList();
                List<EmployeeResignationDetailsView> approverResignation = new List<EmployeeResignationDetailsView>();
                if (resignationInputView.IsMyData == false)
                {
                    if (resignationInputView.ReporteesList == null)
                    {
                        resignationInputView.ReporteesList = new List<int>();
                    } 
                    if(resignationInputView.IsAllData==false)
                    {
                        resignationInputView.ReporteesList = new List<int>();
                        resignationInputView.ReporteesList.Add(resignationInputView.EmployeeId);
                    }
                    else
                    {
                        resignationInputView.ReporteesList.Add(resignationInputView.EmployeeId);
                    }
                    
                    approverResignation = (from resignationDetails in dbContext.EmployeeResignationDetails
                                           join approver in dbContext.ResignationApprovalStatus on resignationDetails.EmployeeResignationDetailsId equals approver.EmployeeResignationDetailsId
                                           where resignationInputView.ReporteesList.Contains(approver.ApproverEmployeeId == null ? 0 : (int)approver.ApproverEmployeeId) && approver.Status !=null
                                           select new EmployeeResignationDetailsView
                                           {
                                               EmployeeResignationDetailsId = resignationDetails.EmployeeResignationDetailsId,
                                               EmployeeId = resignationDetails.EmployeeId,
                                               EmployeeDesignation = resignationDetails.EmployeeDesignation,
                                               //ResidanceContactNumber = resignationDetails.ResidanceContactNumber,
                                               MobileNumber = resignationDetails.MobileNumber,
                                               PersonalEmailAddress = resignationDetails.PersonalEmailAddress,
                                               AddressLine1 = resignationDetails.AddressLine1,
                                               AddressLine2 = resignationDetails.AddressLine2,
                                               ResignationReasonId = resignationDetails.ResignationReasonId,
                                               ResignationStatus = resignationDetails.ResignationStatus,
                                               ResignationDate = resignationDetails.ResignationDate,
                                               ActualRelievingDate = resignationDetails.ActualRelievingDate,
                                               RelievingDate = resignationDetails.RelievingDate,
                                               CreatedOn = resignationDetails.CreatedOn,
                                               CreatedBy = resignationDetails.CreatedBy,
                                               ModifiedBy = resignationDetails.ModifiedBy,
                                               ModifiedOn = resignationDetails.ModifiedOn,
                                               ResignationReason = resignationDetails.ResignationReason == null ? dbContext.ResignationReason.Where(x => x.ResignationReasonId == resignationDetails.ResignationReasonId).Select(x => x.ResignationReasonName).FirstOrDefault() : resignationDetails.ResignationReason,
                                               WithdrawalReason = resignationDetails.WithdrawalReason,
                                               ZipCode = resignationDetails.ZipCode,
                                               City = resignationDetails.City,
                                               State = resignationDetails.State,
                                               Country = resignationDetails.Country,
                                               DepartmentName = resignationDetails.Department,
                                               Location = resignationDetails.Location,
                                               ResignationType = resignationDetails.ResignationType,
                                               Remarks = resignationDetails.Remarks,                                               
                                               ProfilePicture = resignationDetails.ProfilePicture,
                                               ReportingManager = resignationDetails.ReportingManager,
                                               ReportingManagerID = resignationDetails.ReportingManagerId,
                                               IsAgreeCheckList = resignationDetails.ResignationStatus != "Pending" ? dbContext.ResignationChecklist.Where(x => x.ResignationDetailsId == resignationDetails.EmployeeResignationDetailsId).Select(x => x.IsAgreeCheckList).FirstOrDefault() : false,
                                               ResignationApproverList = (from a in dbContext.ResignationApproval
                                                                          join b in dbContext.ResignationApprovalStatus
                                                                          on a.LevelId equals b.LevelId
                                                                          join c in dbContext.AppConstants on a.LevelApprovalId equals c.AppConstantId
                                                                          where b.EmployeeResignationDetailsId == resignationDetails.EmployeeResignationDetailsId
                                                                          select new ResignationApprovalStatusView { LevelId = a.LevelId, ApproverType = c.DisplayName, FeedBack = b.FeedBack, Status = b.Status, ApproverEmployeeId = b.ApproverEmployeeId, ApprovalType = b.ApprovalType }).ToList()
                                           }).OrderByDescending(x => x.CreatedOn).ToList();
                }
                List<EmployeeResignationDetailsView> allData = employeeResignation?.Concat(approverResignation == null ? new List<EmployeeResignationDetailsView>() : approverResignation).Distinct().ToList();
                return allData.GroupBy(x => x.EmployeeResignationDetailsId).Select(x => x.FirstOrDefault()).ToList();
            }

        }

        public EmployeeResignationDetailsView GetResignationDetailByResignationId(int resignationId)
        {
            return (from resignationDetails in dbContext.EmployeeResignationDetails
                    where resignationDetails.EmployeeResignationDetailsId == resignationId
                    select new EmployeeResignationDetailsView
                    {
                        EmployeeResignationDetailsId = resignationDetails.EmployeeResignationDetailsId,
                        EmployeeId = resignationDetails.EmployeeId,
                        EmployeeDesignation = resignationDetails.EmployeeDesignation,
                        //ResidanceContactNumber = resignationDetails.ResidanceContactNumber,
                        MobileNumber = resignationDetails.MobileNumber,
                        PersonalEmailAddress = resignationDetails.PersonalEmailAddress,
                        AddressLine1 = resignationDetails.AddressLine1,
                        AddressLine2 = resignationDetails.AddressLine2,
                        ResignationReasonId = resignationDetails.ResignationReasonId,
                        ResignationStatus = resignationDetails.ResignationStatus,
                        ResignationDate = resignationDetails.ResignationDate,
                        ActualRelievingDate = resignationDetails.ActualRelievingDate,
                        RelievingDate = resignationDetails.RelievingDate,
                        CreatedOn = resignationDetails.CreatedOn,
                        CreatedBy = resignationDetails.CreatedBy,
                        ModifiedBy = resignationDetails.ModifiedBy,
                        ModifiedOn = resignationDetails.ModifiedOn,
                        ResignationReason = resignationDetails.ResignationReason == null ? dbContext.ResignationReason.Where(x => x.ResignationReasonId == resignationDetails.ResignationReasonId).Select(x => x.ResignationReasonName).FirstOrDefault() : resignationDetails.ResignationReason,
                        WithdrawalReason = resignationDetails.WithdrawalReason,
                        ZipCode = resignationDetails.ZipCode,
                        City = resignationDetails.City,
                        State = resignationDetails.State,
                        Country = resignationDetails.Country,
                        DepartmentName = resignationDetails.Department,
                        Location = resignationDetails.Location,
                        ResignationType = resignationDetails.ResignationType,
                        Remarks = resignationDetails.Remarks,
                        ProfilePicture = resignationDetails.ProfilePicture,
                        ReportingManager = resignationDetails.ReportingManager,
                        ReportingManagerID = resignationDetails.ReportingManagerId,
                        IsAgreeCheckList = resignationDetails.ResignationStatus != "Pending" ? dbContext.ResignationChecklist.Where(x => x.ResignationDetailsId == resignationDetails.EmployeeResignationDetailsId).Select(x => x.IsAgreeCheckList).FirstOrDefault() : false,
                        ResignationApproverList = dbContext.ResignationApprovalStatus.Where(x => x.EmployeeResignationDetailsId == resignationId).Select(b=>
                                                   new ResignationApprovalStatusView { LevelId = b.LevelId, 
                                                       ApproverType = b.ApproverType, 
                                                       FeedBack = b.FeedBack, 
                                                       Status = b.Status, 
                                                       ApproverEmployeeId = b.ApproverEmployeeId, 
                                                       ApprovalType = b.ApprovalType,
                                                       ApproveById = b.ApprovedBy}).ToList(),
                        ResignationInterviewDetailId = (from rin in dbContext.ResignationInterview.Where(ri => ri.ResignationDetailsId == resignationDetails.EmployeeResignationDetailsId) select rin.ResignationInterviewId).FirstOrDefault()
                    }).FirstOrDefault();
        }
        public int? GetLastResignationIdByEmployeeId(int employeeId)
        {
            return dbContext.EmployeeResignationDetails.Where(x => x.EmployeeId == employeeId).OrderByDescending(x=>x.CreatedOn).Select(x=>x.EmployeeResignationDetailsId).FirstOrDefault();
        }
        public List<KeyWithIntValue> GetLastResignationIdByEmployeeList(List<int> employeeIdList)
        {
            List<KeyWithIntValue> data = new List<KeyWithIntValue>();
            foreach(int empId in employeeIdList)
            {
               int resignId= dbContext.EmployeeResignationDetails.Where(x => x.EmployeeId == empId && x.ResignationStatus!="Rejected" && x.ResignationStatus != "Cancelled" && x.ResignationStatus != "Withdrawal Approved").OrderByDescending(x => x.CreatedOn).Select(x=>x.EmployeeResignationDetailsId).FirstOrDefault();
                if (resignId > 0)
                {
                    data.Add(
                        new KeyWithIntValue()
                        {
                            Key = empId,
                            Value = resignId
                        });
                }
            }
            return data;
        }
        public List<int> GetChecklistSubmissionNotificationEmployee(int days)
        {
            List<int> checklistResignId = dbContext.ResignationChecklist.Select(x => x.ResignationDetailsId == null ? 0 : (int)x.ResignationDetailsId).ToList();
            return dbContext.EmployeeResignationDetails.Where(x => !checklistResignId.Contains(x.EmployeeResignationDetailsId) && (x.ResignationStatus == "Pending" || x.ResignationStatus == "Approved") && x.RelievingDate != null && x.RelievingDate.Value.AddDays(days).Date <= DateTime.Now.Date && x.RelievingDate.Value.Date >= DateTime.Now.Date).Select(x=>x.EmployeeId).ToList();
        }
        public List<int> GetResignationInterviewNotification(int days)
        {
            List<int> employeeList = dbContext.EmployeeResignationDetails?.Where(x => !dbContext.ResignationInterview.Any(b => b.ResignationDetailsId == x.EmployeeResignationDetailsId) && (x.ResignationStatus == "Pending" || x.ResignationStatus == "Approved") && x.RelievingDate != null && x.RelievingDate.Value.AddDays(days).Date <= DateTime.Now.Date && x.RelievingDate.Value.Date >= DateTime.Now.Date).Select(x => x.EmployeeId).ToList();
            return employeeList;
        }

        public EmployeeResignationChecklistDetailsView GetEmployeeDetailByResignationID(int EmployeeResignationDetailsId)
        {
            return dbContext.EmployeeResignationDetails.Where(x => x.EmployeeResignationDetailsId == EmployeeResignationDetailsId).Select(x=> new EmployeeResignationChecklistDetailsView
            {
                EmployeeResignationDetailsId = x.EmployeeResignationDetailsId,
                EmployeeId = x.EmployeeId,
                EmployeeName=x.EmployeeName,
                FormattedEmployeeId =x.FormattedEmployeeId
            }
            ).FirstOrDefault();
        }
    }
}