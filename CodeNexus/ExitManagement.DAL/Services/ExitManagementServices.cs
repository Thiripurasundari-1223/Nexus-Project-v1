using ExitManagement.DAL.Repository;
using SharedLibraries.Common;
using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.ExitManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Services
{
    public class ExitManagementServices
    {
        private readonly IEmployeeResignationDetailsRepository _employeeResignationDetailsRepository;
        private readonly IResignationReasonRepository _resignationReasonRepository;
        private readonly IResignationApprovalRepository _resignationApprovalRepository;
        private readonly IResignationApprovalStatusRepository _resignationApprovalStatusRepository;
        private readonly IAppConstantsRepository _appConstantsRepository;
        private readonly IExitManagementEmailTemplateRepository _exitManagementEmailTemplateRepository;
        private readonly IResignationInterviewRepository _resignationInterviewRepository;
        private readonly IResignationFeedbackToManagementRepository _resignationFeedbackToManagementRepository;
        private readonly IReasonLeavingPositionRepository _reasonLeavingPositionRepository;
        private readonly IResignationChecklistRepository _resignationChecklistRepository;
        private readonly IManagerCheckListRepository _managerCheckListRepository;
        private readonly IPMOCheckListRepository _pmoCheckListRepository;
        private readonly IITCheckListRepository _itCheckListRepository;
        private readonly IAdminCheckListRepository _adminCheckListRepository;
        private readonly IFinanceCheckListRepository _financeCheckListRepository;
        private readonly IHRCheckListRepository _hrCheckListRepository;
        private readonly ICheckListViewRepository _checkListViewRepository;
        #region Constructor
        public ExitManagementServices(IEmployeeResignationDetailsRepository employeeResignationDetailsRepository, IResignationReasonRepository resignationReasonRepository, IResignationApprovalRepository resignationApprovalRepository, IResignationApprovalStatusRepository resignationApprovalStatusRepository, IAppConstantsRepository appConstantsRepository, IExitManagementEmailTemplateRepository exitManagementEmailTemplateRepository, IResignationInterviewRepository resignationInterviewRepository, IResignationFeedbackToManagementRepository resignationFeedbackToManagementRepository, IReasonLeavingPositionRepository reasonLeavingPositionRepository, IResignationChecklistRepository resignationChecklistRepository,
            IManagerCheckListRepository managerCheckListRepository, IPMOCheckListRepository pmoCheckListRepository, IITCheckListRepository itCheckListRepository, IAdminCheckListRepository adminCheckListRepository, IFinanceCheckListRepository financeCheckListRepository, IHRCheckListRepository hrCheckListRepository, ICheckListViewRepository checkListViewRepository)
        {
            _employeeResignationDetailsRepository = employeeResignationDetailsRepository;
            _resignationReasonRepository = resignationReasonRepository;
            _resignationApprovalRepository = resignationApprovalRepository;
            _resignationApprovalStatusRepository = resignationApprovalStatusRepository;
            _appConstantsRepository = appConstantsRepository;
            _exitManagementEmailTemplateRepository = exitManagementEmailTemplateRepository;
            _resignationInterviewRepository = resignationInterviewRepository;
            _resignationFeedbackToManagementRepository = resignationFeedbackToManagementRepository;
            _reasonLeavingPositionRepository = reasonLeavingPositionRepository;
            _resignationChecklistRepository = resignationChecklistRepository;
            _managerCheckListRepository = managerCheckListRepository;
            _pmoCheckListRepository = pmoCheckListRepository;
            _itCheckListRepository = itCheckListRepository;
            _adminCheckListRepository = adminCheckListRepository;
            _financeCheckListRepository = financeCheckListRepository;
            _hrCheckListRepository = hrCheckListRepository;
            _checkListViewRepository = checkListViewRepository;
        }
        #endregion
        #region 
        public async Task<EmployeeResignationDetailsView> InsertAndUpdateResignationDetails(EmployeeResignationDetailsView employeeResignationDetails)
        {
            try
            {
                int EmployeeResignationDetailsId = 0;
                int? firstLevelApproverId = 0;
                EmployeeResignationDetails resignationDetails = new();
                if (employeeResignationDetails.EmployeeResignationDetailsId != 0)
                    resignationDetails = _employeeResignationDetailsRepository.GetByResignationID(employeeResignationDetails.EmployeeResignationDetailsId);
                if (resignationDetails != null)
                {
                    resignationDetails.EmployeeId = employeeResignationDetails.EmployeeId;
                    resignationDetails.EmployeeName = employeeResignationDetails?.EmployeeName;
                    resignationDetails.FormattedEmployeeId = employeeResignationDetails?.FormattedEmployeeId;
                    resignationDetails.EmployeeDesignation = employeeResignationDetails?.EmployeeDesignation;
                    resignationDetails.MobileNumber = employeeResignationDetails?.MobileNumber;
                    resignationDetails.PersonalEmailAddress = employeeResignationDetails?.PersonalEmailAddress;
                    resignationDetails.AddressLine1 = employeeResignationDetails?.AddressLine1;
                    resignationDetails.AddressLine2 = employeeResignationDetails?.AddressLine2;
                    if (employeeResignationDetails.EmployeeResignationDetailsId == 0)
                    {
                        resignationDetails.ResignationReasonId = 0; // employeeResignationDetails.ResignationReasonId;
                        resignationDetails.ResignationReason = ""; //employeeResignationDetails?.ResignationReason;
                    }
                    //resignationDetails.ResignationReasonId = employeeResignationDetails.ResignationReasonId;
                    //resignationDetails.ResignationReason = employeeResignationDetails?.ResignationReason;
                    //resignationDetails.ResignationDate = employeeResignationDetails?.ResignationDate;
                    //if (employeeResignationDetails.EmployeeResignationDetailsId == 0)
                    //{
                    //    employeeResignationDetails.RelievingDate = DateTime.Now.AddDays(employeeResignationDetails?.ResignationApprover?.NoticePeriod == null ? 0 : (int)employeeResignationDetails.ResignationApprover.NoticePeriod - 1);
                    //    employeeResignationDetails.ActualRelievingDate = employeeResignationDetails.RelievingDate;
                    //}
                    //resignationDetails.RelievingDate = employeeResignationDetails?.RelievingDate;
                    //resignationDetails.ActualRelievingDate = employeeResignationDetails?.ActualRelievingDate;
                    resignationDetails.City = employeeResignationDetails?.City;
                    resignationDetails.State = employeeResignationDetails.State;
                    resignationDetails.Country = employeeResignationDetails.Country;
                    resignationDetails.ZipCode = employeeResignationDetails?.ZipCode;
                    resignationDetails.Department = employeeResignationDetails.DepartmentName;
                    resignationDetails.ReportingManager = employeeResignationDetails.ReportingManager;
                    resignationDetails.Location = employeeResignationDetails.Location;
                    resignationDetails.ResignationType = employeeResignationDetails.ResignationType;
                    resignationDetails.Remarks = employeeResignationDetails.Remarks;
                    resignationDetails.ProfilePicture = employeeResignationDetails.ProfilePicture;
                    resignationDetails.ReportingManagerId = employeeResignationDetails.ReportingManagerID;
                    if (employeeResignationDetails.EmployeeResignationDetailsId == 0 || employeeResignationDetails?.IsWithdrawal == true)
                    {
                        resignationDetails.ResignationStatus = employeeResignationDetails?.ResignationStatus;
                    }
                    resignationDetails.WithdrawalReason = employeeResignationDetails?.WithdrawalReason;
                    if(employeeResignationDetails?.IsWithdrawal == true)
                    {
                        resignationDetails.WithdrawalSubmmitedDate = DateTime.UtcNow;
                    }
                    if (employeeResignationDetails.EmployeeResignationDetailsId == 0)
                    {
                        resignationDetails.CreatedOn = DateTime.UtcNow;
                        resignationDetails.CreatedBy = employeeResignationDetails?.CreatedBy;
                        await _employeeResignationDetailsRepository.AddAsync(resignationDetails);
                        await _employeeResignationDetailsRepository.SaveChangesAsync();
                        EmployeeResignationDetailsId = resignationDetails.EmployeeResignationDetailsId;
                        if (employeeResignationDetails?.ResignationApprover != null)
                        {
                            List<ResignationApproval> resignationApprovalList = _resignationApprovalRepository.GetResignationApproval();
                            if (resignationApprovalList?.Count > 0)
                            {
                                foreach (ResignationApproval approval in resignationApprovalList)
                                {

                                    int? approverId = 0;
                                    string approverType = "";
                                    if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("ResignationApproval", "ReportingTo"))
                                    {
                                        approverId = employeeResignationDetails?.ResignationApprover?.ReportingManagerEmployeeId;
                                        approverType = "Reporting Manager";
                                    }
                                    else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("ResignationApproval", "DepartmentBUHead"))
                                    {
                                        approverId = employeeResignationDetails?.ResignationApprover?.DepartmentHeadEmployeeId;
                                        approverType = "Department BU Head";
                                    }
                                    else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("ResignationApproval", "HRBUHead"))
                                    {
                                        approverId = employeeResignationDetails?.ResignationApprover?.HRHeadEmployeeId;
                                        approverType = "HR BU Head";
                                    }
                                    else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("ResignationApproval", "Others"))
                                    {
                                        approverId = approval.LevelApprovalEmployeeId;
                                        approverType = "Others";
                                    }
                                    if (approverId > 0)
                                    {
                                        if (approval.LevelId == 1)
                                        {
                                            firstLevelApproverId = approverId;
                                        }
                                        ResignationApprovalStatus approvalStatus = new ResignationApprovalStatus();
                                        approvalStatus.EmployeeResignationDetailsId = EmployeeResignationDetailsId;
                                        approvalStatus.ApproverEmployeeId = approverId;
                                        approvalStatus.LevelId = approval.LevelId;
                                        approvalStatus.FeedBack = "";
                                        approvalStatus.Status = approvalStatus.LevelId == 1 ? "Pending" : null;
                                        approvalStatus.ApprovalType = "Approval";
                                        approvalStatus.CreatedOn = DateTime.UtcNow;
                                        approvalStatus.CreatedBy = employeeResignationDetails?.CreatedBy;
                                        approvalStatus.ApproverType = approverType;
                                        await _resignationApprovalStatusRepository.AddAsync(approvalStatus);
                                        await _resignationApprovalStatusRepository.SaveChangesAsync();
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        resignationDetails.ModifiedOn = DateTime.UtcNow;
                        resignationDetails.ModifiedBy = employeeResignationDetails?.ModifiedBy;
                        _employeeResignationDetailsRepository.Update(resignationDetails);
                        EmployeeResignationDetailsId = resignationDetails.EmployeeResignationDetailsId;
                        await _employeeResignationDetailsRepository.SaveChangesAsync();
                    }
                    if (employeeResignationDetails?.IsWithdrawal == true)
                    {
                        List<ResignationApprovalStatus> resignationApprovalList = _resignationApprovalStatusRepository.GetResignationApprovalStatusById(employeeResignationDetails.EmployeeResignationDetailsId, "Approval");

                        foreach (ResignationApprovalStatus item in resignationApprovalList)
                        {
                            if (item.LevelId == 1)
                            {
                                firstLevelApproverId = item.ApproverEmployeeId;
                            }
                            ResignationApprovalStatus approvalStatus = new ResignationApprovalStatus();
                            approvalStatus.EmployeeResignationDetailsId = EmployeeResignationDetailsId;
                            approvalStatus.ApproverEmployeeId = item.ApproverEmployeeId;
                            approvalStatus.LevelId = item.LevelId;
                            approvalStatus.FeedBack = "";
                            approvalStatus.Status = approvalStatus.LevelId == 1 ? "Pending" : null;
                            approvalStatus.ApprovalType = "Withdrawal";
                            approvalStatus.CreatedOn = DateTime.UtcNow;
                            approvalStatus.CreatedBy = employeeResignationDetails?.CreatedBy;
                            approvalStatus.ApproverType = item.ApproverType;
                            await _resignationApprovalStatusRepository.AddAsync(approvalStatus);
                            await _resignationApprovalStatusRepository.SaveChangesAsync();
                        }
                    }
                }
                employeeResignationDetails.EmployeeResignationDetailsId = resignationDetails?.EmployeeResignationDetailsId == null ? 0 : (int)resignationDetails?.EmployeeResignationDetailsId;
                employeeResignationDetails.ApprovalEmailTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("ResignationApprovalManager");
                employeeResignationDetails.WithdrawalEmailTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("WithdrawalResignationApprovalManager");
                employeeResignationDetails.ApproverId = firstLevelApproverId;
                employeeResignationDetails.ResignationStatus = resignationDetails.ResignationStatus;
                return employeeResignationDetails;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion
        #region  insert or update Resignation Reason     
        public async Task<EmployeeResignationDetailsView> InsertAndUpdateResignationReason(EmployeeResignationDetailsView employeeResignationDetails)
        {
            try
            {
                EmployeeResignationDetails resignationDetails = new();
                if (employeeResignationDetails.EmployeeResignationDetailsId != 0)
                    resignationDetails = _employeeResignationDetailsRepository.GetByResignationID(employeeResignationDetails.EmployeeResignationDetailsId);
                if (resignationDetails != null && resignationDetails.EmployeeResignationDetailsId == employeeResignationDetails.EmployeeResignationDetailsId)
                {
                    resignationDetails.ResignationReasonId = employeeResignationDetails.ResignationReasonId;
                    resignationDetails.ResignationReason = employeeResignationDetails.ResignationReason != null ? employeeResignationDetails.ResignationReason : GetResignationReasonsById(employeeResignationDetails.ResignationReasonId == null ? 0 : (int)employeeResignationDetails.ResignationReasonId);
                    resignationDetails.ResignationDate = DateTime.Now;
                    //if (employeeResignationDetails.EmployeeResignationDetailsId == 0)
                    {
                        employeeResignationDetails.RelievingDate = DateTime.Now.AddDays(employeeResignationDetails?.ResignationApprover?.NoticePeriod == null ? 0 : (int)employeeResignationDetails.ResignationApprover.NoticePeriod - 1);
                        employeeResignationDetails.ActualRelievingDate = employeeResignationDetails.RelievingDate;
                    }
                    resignationDetails.RelievingDate = employeeResignationDetails?.RelievingDate;
                    resignationDetails.ActualRelievingDate = employeeResignationDetails?.ActualRelievingDate;
                    resignationDetails.ModifiedOn = DateTime.UtcNow;
                    resignationDetails.ModifiedBy = employeeResignationDetails?.ModifiedBy;
                    resignationDetails.ResignationStatus = employeeResignationDetails.ResignationStatus;
                    int? firstLevelApproverId = 0;
                    if (employeeResignationDetails?.ResignationApprover != null)
                    {
                        List<ResignationApproval> resignationApprovalList = _resignationApprovalRepository.GetResignationApproval();
                        if (resignationApprovalList?.Count > 0)
                        {
                            foreach (ResignationApproval approval in resignationApprovalList)
                            {

                                int? approverId = 0;
                                string approverType = "";
                                if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("ResignationApproval", "ReportingTo"))
                                {
                                    approverId = employeeResignationDetails?.ResignationApprover?.ReportingManagerEmployeeId;
                                    approverType = "Reporting Manager";
                                }
                                else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("ResignationApproval", "DepartmentBUHead"))
                                {
                                    approverId = employeeResignationDetails?.ResignationApprover?.DepartmentHeadEmployeeId;
                                    approverType = "Department BU Head";
                                }
                                else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("ResignationApproval", "HRBUHead"))
                                {
                                    approverId = employeeResignationDetails?.ResignationApprover?.HRHeadEmployeeId;
                                    approverType = "HR BU Head";
                                }
                                else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("ResignationApproval", "Others"))
                                {
                                    approverId = approval.LevelApprovalEmployeeId;
                                    approverType = "Others";
                                }
                                if (approverId > 0)
                                {
                                    if (approval.LevelId == 1)
                                    {
                                        firstLevelApproverId = approverId;
                                    }
                                    ResignationApprovalStatus approvalStatus = new ResignationApprovalStatus();
                                    approvalStatus.EmployeeResignationDetailsId = resignationDetails.EmployeeResignationDetailsId;
                                    approvalStatus.ApproverEmployeeId = approverId;
                                    approvalStatus.LevelId = approval.LevelId;
                                    approvalStatus.FeedBack = "";
                                    approvalStatus.Status = approvalStatus.LevelId == 1 ? "Pending" : null;
                                    approvalStatus.ApprovalType = "Approval";
                                    approvalStatus.CreatedOn = DateTime.UtcNow;
                                    approvalStatus.CreatedBy = employeeResignationDetails?.CreatedBy;
                                    approvalStatus.ApproverType = approverType;
                                    await _resignationApprovalStatusRepository.AddAsync(approvalStatus);
                                    await _resignationApprovalStatusRepository.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    _employeeResignationDetailsRepository.Update(resignationDetails);
                    await _employeeResignationDetailsRepository.SaveChangesAsync();
                    employeeResignationDetails.ApprovalEmailTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("ResignationApprovalManager");
                    employeeResignationDetails.WithdrawalEmailTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("WithdrawalResignationApprovalManager");
                    employeeResignationDetails.ApproverId = firstLevelApproverId;
                    employeeResignationDetails.ResignationReason = resignationDetails.ResignationReason;
                    employeeResignationDetails.ResignationDate = resignationDetails.ResignationDate;
                }
                return employeeResignationDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region
        public List<EmployeeResignationDetailsView> GetAllResignationDetails(AllResignationInputView resignationInputView)
        {
            return _employeeResignationDetailsRepository.GetAllResignationDetails(resignationInputView);
        }
        #endregion
        #region
        public List<ResignationReason> GetResignationReasons()
        {
            return _resignationReasonRepository.GetResignationReasons();
        }
        #endregion
        #region
        public string GetResignationReasonsById(int reasonId)
        {
            return _resignationReasonRepository.GetResignationReasonsById(reasonId);
        }
        #endregion

        #region
        public EmployeeResignationDetailsView GetResignationDetailsByResignationId(int resignationId)
        {
            return _employeeResignationDetailsRepository.GetResignationDetailByResignationId(resignationId);
        }
        #endregion
        #region Insert or update approvel level
        public async Task<bool> InsertOrUpdateResignationApproval(List<ResignationApproval> resignationApproval)
        {
            try
            {
                List<ResignationApproval> resignationApprovalList = _resignationApprovalRepository.GetResignationApproval();
                if (resignationApprovalList?.Count > 0)
                {
                    foreach (ResignationApproval item in resignationApprovalList)
                    {
                        _resignationApprovalRepository.Delete(item);
                        await _resignationApprovalRepository.SaveChangesAsync();
                    }
                }
                foreach (ResignationApproval item in resignationApproval)
                {
                    ResignationApproval employeeResignApproval = new ResignationApproval();
                    employeeResignApproval.LevelId = item.LevelId;
                    employeeResignApproval.LevelApprovalId = item.LevelApprovalId;
                    employeeResignApproval.LevelApprovalEmployeeId = item.LevelApprovalEmployeeId;
                    employeeResignApproval.CreatedOn = DateTime.UtcNow;
                    employeeResignApproval.CreatedBy = item.CreatedBy;
                    await _resignationApprovalRepository.AddAsync(employeeResignApproval);
                    await _resignationApprovalRepository.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
        #region Get resign approvel level
        public ApprovalConfiguration GetResignationApproval()
        {
            ApprovalConfiguration appConfiguration = new();
            appConfiguration.ApprovalType = _appConstantsRepository.GetAppConstantByType("ResignationApproval");
            appConfiguration.ResignationApproval = _resignationApprovalRepository.GetResignationApproval();
            return appConfiguration;

        }
        #endregion
        #region Approve Or Reject Or Cancel Resignation
        public async Task<ApproveResignationView> ApproveOrRejectOrCancelResignation(ApproveResignationView resignationDetails)
        {
            string approvalType = "Approval";
            string overallStatus = "";
            EmployeeResignationDetails employeeResignation = _employeeResignationDetailsRepository.GetByResignationID(resignationDetails.EmployeeResignationDetailsId);
            if (employeeResignation.ResignationStatus == "Withdrawal Pending" || employeeResignation.ResignationStatus == "Withdrawal Approved")
            {
                approvalType = "Withdrawal";
            }
            ResignationApprovalStatus resignation = _resignationApprovalStatusRepository.GetResignationApprovalStatus(resignationDetails.EmployeeResignationDetailsId, resignationDetails.LevelId, approvalType);
            if (resignation != null && (resignationDetails?.Status == "Approved" || resignationDetails?.Status == "Rejected"))
            {
                resignation.FeedBack = resignationDetails.Feedback;
                resignation.Status = resignationDetails.Status;
                resignation.ApprovedBy = resignationDetails.CreatedBy;
                resignation.ModifiedBy = resignationDetails.CreatedBy;
                resignation.ModifiedOn = DateTime.UtcNow;
                _resignationApprovalStatusRepository.Update(resignation);
                await _resignationApprovalStatusRepository.SaveChangesAsync();

            }

            employeeResignation.RelievingDate = resignationDetails.RelievingDate;
            if (resignationDetails?.Status == "Approved")
            {
                if (_resignationApprovalStatusRepository.GetResignationApprovalLastLevel(resignationDetails.EmployeeResignationDetailsId) == resignationDetails.LevelId)
                {
                    employeeResignation.ResignationStatus = employeeResignation.ResignationStatus == "Withdrawal Pending" ? "Withdrawal Approved" : resignationDetails?.Status;
                    resignationDetails.NextLevelApproverId = 0;
                }
                else
                {
                    ResignationApprovalStatus resignationDetail = _resignationApprovalStatusRepository.GetResignationApprovalStatus(resignationDetails.EmployeeResignationDetailsId, resignationDetails.LevelId + 1, approvalType);
                    resignationDetails.NextLevelApproverId = resignationDetail.ApproverEmployeeId;
                    if (resignationDetail != null)
                    {
                        resignationDetail.Status = "Pending";
                        resignationDetail.ModifiedBy = resignationDetails.CreatedBy;
                        resignationDetail.ModifiedOn = DateTime.UtcNow;
                        _resignationApprovalStatusRepository.Update(resignationDetail);
                        await _resignationApprovalStatusRepository.SaveChangesAsync();

                    }
                }
            }
            else
            {
                if (employeeResignation.ResignationStatus == "Withdrawal Pending" && resignationDetails?.Status == "Rejected")
                {
                    ResignationApprovalStatus resignationLastApprovalStatus = _resignationApprovalStatusRepository.GetResignationLastApprovalStatus(resignationDetails.EmployeeResignationDetailsId, "Approval");
                    employeeResignation.ResignationStatus = resignationLastApprovalStatus?.Status == "Approved" ? "Approved" : "Pending";
                    overallStatus = "Withdrawal Rejected";
                }
                else
                {
                    employeeResignation.ResignationStatus = resignationDetails?.Status;
                }
            }
            employeeResignation.ModifiedBy = resignationDetails.CreatedBy;
            employeeResignation.ModifiedOn = DateTime.UtcNow;
            _employeeResignationDetailsRepository.Update(employeeResignation);
            await _employeeResignationDetailsRepository.SaveChangesAsync();
            resignationDetails.EmailTemplateList = _exitManagementEmailTemplateRepository.GetAllEmailTemplate();
            resignationDetails.OverAllStatus = overallStatus != "" ? overallStatus : employeeResignation.ResignationStatus;
            resignationDetails.ResignDate = employeeResignation.ResignationDate;
            resignationDetails.WithdrawalReason = employeeResignation.WithdrawalReason;
            resignationDetails.ResignReason = employeeResignation.ResignationReason != null ? employeeResignation.ResignationReason : GetResignationReasonsById(employeeResignation.ResignationReasonId == null ? 0 : (int)employeeResignation.ResignationReasonId);
            return resignationDetails;
        }
        #endregion
        #region 
        public async Task<ResignationInterviewDetailView> InsertAndUpdateResignationInterviewDetails(ResignationInterviewDetailView employeeResignationInterviewDetail)
        {
            try
            {
                int EmployeeResignationInterviewId = 0;
                ResignationInterview resignationInterview = new();
                ResignationFeedbackToManagement resignationFeedbackToManagement = new();
                if (employeeResignationInterviewDetail.ResignationInterviewId != 0)
                {
                    resignationInterview = _resignationInterviewRepository.GetByResignationInterviewID(employeeResignationInterviewDetail.ResignationInterviewId);
                    resignationFeedbackToManagement = _resignationFeedbackToManagementRepository.GetByResignationInterviewFeedbackByID(employeeResignationInterviewDetail.ResignationInterviewId);
                    List<ReasonLeavingPosition> positionIdList = _reasonLeavingPositionRepository.GetLeavingPositionByInterviewId(employeeResignationInterviewDetail.ResignationInterviewId);
                    foreach (ReasonLeavingPosition item in positionIdList)
                    {
                        _reasonLeavingPositionRepository.Delete(item);
                        await _reasonLeavingPositionRepository.SaveChangesAsync();
                    }
                }
                if (resignationInterview != null && resignationFeedbackToManagement != null)
                {
                    resignationInterview.EmployeeID = employeeResignationInterviewDetail.EmployeeID;
                    resignationInterview.OverallView = employeeResignationInterviewDetail.OverallView;
                    //resignationInterview.ReasonOfRelievingPositionId = employeeResignationInterviewDetail.ReasonOfRelievingPositionId;
                    resignationInterview.ReasonOfRelieving = employeeResignationInterviewDetail.ReasonOfRelieving;
                    resignationInterview.ShareProspectiveEmployer = employeeResignationInterviewDetail.ShareProspectiveEmployer;
                    resignationInterview.AttractedProspectiveEmployer = employeeResignationInterviewDetail.AttractedProspectiveEmployer;
                    resignationInterview.EventTriggeredForAlternativeJob = employeeResignationInterviewDetail.EventTriggeredForAlternativeJob;
                    resignationInterview.EnjoyDislike = employeeResignationInterviewDetail.EnjoyDislike;
                    resignationInterview.UnresolvedIssues = employeeResignationInterviewDetail.UnresolvedIssues;
                    resignationInterview.FeelAboutManagement = employeeResignationInterviewDetail.FeelAboutManagement;
                    resignationInterview.FeedbackOnPerformance = employeeResignationInterviewDetail.FeedbackOnPerformance;
                    resignationInterview.SufficientSupportTraining = employeeResignationInterviewDetail.SufficientSupportTraining;
                    resignationInterview.PreventFromLeaving = employeeResignationInterviewDetail.PreventFromLeaving;
                    resignationInterview.RejoinORRecommend = employeeResignationInterviewDetail.RejoinORRecommend;
                    resignationInterview.Suggestion = employeeResignationInterviewDetail.Suggestion;
                    resignationFeedbackToManagement.TrainingId = employeeResignationInterviewDetail.TrainingId;
                    resignationFeedbackToManagement.TrainingRemark = employeeResignationInterviewDetail.TrainingRemark;
                    resignationFeedbackToManagement.NatureOfWorkId = employeeResignationInterviewDetail.NatureOfWorkId;
                    resignationFeedbackToManagement.NatureOfWorkRemark = employeeResignationInterviewDetail.NatureOfWorkRemark;
                    resignationFeedbackToManagement.ImmediateSupervisorInvolmentId = employeeResignationInterviewDetail.ImmediateSupervisorInvolmentId;
                    resignationFeedbackToManagement.ImmediateSupervisorInvolmentRemark = employeeResignationInterviewDetail.ImmediateSupervisorInvolmentRemark;
                    resignationFeedbackToManagement.JobRecognitionId = employeeResignationInterviewDetail.JobRecognitionId;
                    resignationFeedbackToManagement.JobRecognitionRemark = employeeResignationInterviewDetail.JobRecognitionRemark;
                    resignationFeedbackToManagement.PerformanceFeedbackId = employeeResignationInterviewDetail.PerformanceFeedbackId;
                    resignationFeedbackToManagement.PerformanceFeedbackRemark = employeeResignationInterviewDetail.PerformanceFeedbackRemark;
                    resignationFeedbackToManagement.GrowthOpportunityId = employeeResignationInterviewDetail.GrowthOpportunityId;
                    resignationFeedbackToManagement.GrowthOpportunityRemark = employeeResignationInterviewDetail.GrowthOpportunityRemark;
                    resignationFeedbackToManagement.NewSkillsOpportunityId = employeeResignationInterviewDetail.NewSkillsOpportunityId;
                    resignationFeedbackToManagement.NewSkillsOpportunityRemark = employeeResignationInterviewDetail.NewSkillsOpportunityRemark;
                    resignationFeedbackToManagement.CompensationId = employeeResignationInterviewDetail.CompensationId;
                    resignationFeedbackToManagement.CompensationRemark = employeeResignationInterviewDetail.CompensationRemark;
                    resignationFeedbackToManagement.AnnualIncrementId = employeeResignationInterviewDetail.AnnualIncrementId;
                    resignationFeedbackToManagement.AnnualIncrementRemark = employeeResignationInterviewDetail.AnnualIncrementRemark;
                    resignationFeedbackToManagement.InformationSharingId = employeeResignationInterviewDetail.InformationSharingId;
                    resignationFeedbackToManagement.InformationSharingRemark = employeeResignationInterviewDetail.InformationSharingRemark;
                    resignationFeedbackToManagement.Other = employeeResignationInterviewDetail.Other;
                    resignationFeedbackToManagement.OtherRemark = employeeResignationInterviewDetail.OtherRemark;
                    resignationFeedbackToManagement.OrgPoliciesId = employeeResignationInterviewDetail.OrgPoliciesId;
                    resignationFeedbackToManagement.OrgPoliciesRemark = employeeResignationInterviewDetail.OrgPoliciesRemark;
                    if (employeeResignationInterviewDetail.ResignationInterviewId == 0)
                    {
                        resignationInterview.CreatedBy = employeeResignationInterviewDetail.CreatedBy;
                        resignationInterview.CreatedOn = DateTime.UtcNow;
                        resignationInterview.ResignationDetailsId = _employeeResignationDetailsRepository.GetLastResignationIdByEmployeeId(employeeResignationInterviewDetail.EmployeeID);
                        await _resignationInterviewRepository.AddAsync(resignationInterview);
                        await _resignationInterviewRepository.SaveChangesAsync();
                        resignationFeedbackToManagement.ResignationInterviewId = resignationInterview.ResignationInterviewId;
                        resignationFeedbackToManagement.CreatedBy = employeeResignationInterviewDetail.CreatedBy;
                        resignationFeedbackToManagement.CreatedOn = DateTime.UtcNow;
                        await _resignationFeedbackToManagementRepository.AddAsync(resignationFeedbackToManagement);
                        await _resignationFeedbackToManagementRepository.SaveChangesAsync();
                        employeeResignationInterviewDetail.EmailTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("ExitInterview");
                    }
                    else
                    {
                        resignationInterview.ModifiedBy = employeeResignationInterviewDetail.ModifiedBy;
                        resignationInterview.ModifiedOn = DateTime.UtcNow;
                        _resignationInterviewRepository.Update(resignationInterview);
                        await _resignationInterviewRepository.SaveChangesAsync();
                        resignationFeedbackToManagement.ModifiedBy = employeeResignationInterviewDetail.ModifiedBy;
                        resignationFeedbackToManagement.ModifiedOn = DateTime.UtcNow;
                        _resignationFeedbackToManagementRepository.Update(resignationFeedbackToManagement);
                        await _resignationFeedbackToManagementRepository.SaveChangesAsync();
                    }
                    //insert reason for leaving position
                    foreach (int item in employeeResignationInterviewDetail.ReasonOfRelievingPositionId)
                    {
                        ReasonLeavingPosition position = new ReasonLeavingPosition();
                        position.ResignationInterviewId = resignationInterview.ResignationInterviewId;
                        position.LeavingPositionId = item;
                        position.CreatedBy = employeeResignationInterviewDetail.CreatedBy;
                        position.CreatedOn = DateTime.UtcNow;
                        await _reasonLeavingPositionRepository.AddAsync(position);
                        await _reasonLeavingPositionRepository.SaveChangesAsync();
                    }
                    employeeResignationInterviewDetail.ResignationInterviewId = resignationInterview.ResignationInterviewId;
                }
                return employeeResignationInterviewDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get resignation interview details by employee id
        public ResignationInterviewListView GetResignationInterviewDetailByEmployeeId(int employeeId)
        {
            ResignationInterviewListView details = new ResignationInterviewListView();
            details.ResignationInterviewDetailList = _resignationInterviewRepository.GetResignationInterviewDetailByEmployeeId(employeeId);
            details.ResignationInterview = _appConstantsRepository.GetAppConstantByType("ResignationInterview");
            details.ReasonRelievingPosition = _appConstantsRepository.GetAppConstantByType("ReasonRelievingPosition");
            return details;
        }
        #endregion
        #region Get resignation interview details by interview id
        public ResignationInterviewDetailView GetResignationInterviewDetailById(int resignationInterviewId)
        {
            ResignationInterviewDetailView detail = _resignationInterviewRepository.GetResignationInterviewDetailById(resignationInterviewId);
            if (detail != null)
            {
                detail.ResignationInterview = _appConstantsRepository.GetAppConstantByType("ResignationInterview");
                detail.ReasonRelievingPosition = _appConstantsRepository.GetAppConstantByType("ReasonRelievingPosition");
            }
            return detail;
        }
        #endregion
        #region Get resignation interview master data
        public ResignationInterviewMasterData GetResignationInterviewMasterData(List<ResignationEmployeeMasterView> employeeList)
        {
            ResignationInterviewMasterData detail = new ResignationInterviewMasterData();
            detail.ResignationInterview = _appConstantsRepository.GetAppConstantByType("ResignationInterview");
            detail.ReasonRelievingPosition = _appConstantsRepository.GetAppConstantByType("ReasonRelievingPosition");
            detail.EmployeeList = _resignationInterviewRepository.GetLastResignationInterviewByEmployeeList(employeeList);
            return detail;
        }
        #endregion

        #region Delete the Exit Interview
        public async Task<bool> DeleteExitInterviewByInterviewId(int exitInterviewId)
        {
            ResignationInterview resignationInterview = _resignationInterviewRepository.GetByResignationInterviewID(exitInterviewId);
            ResignationFeedbackToManagement resignationFeedbackToManagement = _resignationFeedbackToManagementRepository.GetByResignationInterviewFeedbackByID(exitInterviewId);
            if (resignationInterview != null && resignationFeedbackToManagement != null)
            {
                _resignationInterviewRepository.Delete(resignationInterview);
                await _resignationInterviewRepository.SaveChangesAsync();
                _resignationFeedbackToManagementRepository.Delete(resignationFeedbackToManagement);
                await _resignationFeedbackToManagementRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion

        #region Delete the Exit Interview
        public async Task<bool> GetCheckExitInterviewEnable(ExitInterviewEnable exitInterview)
        {
            ResignationInterview resignationInterview = _resignationInterviewRepository.GetCheckExitInterviewEnable(exitInterview.EmployeeId, exitInterview.ResignationDate);
            if (resignationInterview == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region Get resignation checklist master data
        public ResignationChecklistMasterData GetResignationChecklistMasterData(List<ResignationEmployeeMasterView> employeeList)
        {
            ResignationChecklistMasterData detail = new ResignationChecklistMasterData();
            detail.ExitCheckListLetter = _appConstantsRepository.GetAppConstantByType("ExitCheckListLetter");
            detail.ExitCheckList = _appConstantsRepository.GetAppConstantByType("ExitCheckList");
            detail.ManagerMailData = _appConstantsRepository.GetAppConstantByType("ManagerMailData");
            detail.ITMailData = _appConstantsRepository.GetAppConstantByType("ITMailData");
            detail.ELPayData = _appConstantsRepository.GetAppConstantByType("ELPayData");
            detail.CheckListSubmission = _appConstantsRepository.GetAppConstantByType("CheckListSubmission");
            detail.NoticePayData = _appConstantsRepository.GetAppConstantByType("NoticePayData");
            detail.EmployeeList = _resignationChecklistRepository.GetResignationCheckListEmployee(employeeList);
            detail.CheckListView = _checkListViewRepository.GetAllChecklistView();
            return detail;
        }
        #endregion
        #region inser or update resignation checklist details
        public async Task<ResignationChecklistView> InsertOrUpdateResignationChecklist(ResignationChecklistView resignationDetails)
        {
            try
            {
                ResignationChecklist checklist = new ResignationChecklist();
                int checkListId = 0;
                string managerStatus = "", pmoStatus = "", itStatus = "", adminStatus = "", financeStatus = "", hrStatus = "";
                if (resignationDetails.ResignationChecklistId > 0)
                {
                    checklist = _resignationChecklistRepository.GetResignationChecklistByID(resignationDetails.ResignationChecklistId);

                }
                else
                {
                    checklist.EmployeeID = resignationDetails.EmployeeID;
                    checklist.ManagerId = resignationDetails.ManagerId;
                    checklist.IsAgreeCheckList = resignationDetails.IsAgreeCheckList;
                    checklist.ManagerStatus = "Pending";
                    checklist.PMOStatus = ""; //"Yet to Start";
                    checklist.ITStatus = "Yet to Start";
                    checklist.AdminStatus = "Yet to Start";
                    checklist.FinanceStatus = "Yet to Start";
                    checklist.HRStatus = "Yet to Start";
                    checklist.CreatedBy = resignationDetails.CreatedBy;
                    checklist.CreatedOn = DateTime.UtcNow;
                    checklist.ResignationDetailsId = _employeeResignationDetailsRepository.GetLastResignationIdByEmployeeId(resignationDetails.EmployeeID);
                    await _resignationChecklistRepository.AddAsync(checklist);
                    await _resignationChecklistRepository.SaveChangesAsync();
                    //resignationDetails.ResignationChecklistId = checklist.ResignationChecklistId;

                }
                checkListId = resignationDetails.ResignationChecklistId == 0 ? checklist.ResignationChecklistId : resignationDetails.ResignationChecklistId;
                //Manager Checklist
                if (resignationDetails.ManagerCheckList != null && resignationDetails.CheckListType == "manager")
                {
                    ManagerCheckList manager = new ManagerCheckList();

                    if (resignationDetails?.ManagerCheckList?.ManagerCheckListId > 0)
                    {
                        manager = _managerCheckListRepository.GetManagerResignationChecklistByID(checkListId);

                    }
                    resignationDetails.IsManagerSubmited = manager.Status == "Completed" ? true : false;
                    manager.ResignationChecklistId = checkListId;
                    manager.ApprovedBy = resignationDetails?.ManagerCheckList?.ApprovedBy;
                    manager.Status = resignationDetails?.IsSubmit == true ? "Completed" : manager.ManagerCheckListId == 0 && resignationDetails?.IsSubmit == false ? "In-Progress" : manager.Status;
                    manager.KnowledgeTransferId = resignationDetails?.ManagerCheckList?.KnowledgeTransferId;
                    manager.KnowledgeTransferRemark = resignationDetails?.ManagerCheckList?.KnowledgeTransferRemark;
                    manager.MailID = resignationDetails?.ManagerCheckList?.MailID;
                    manager.RoutedTo = resignationDetails?.ManagerCheckList?.RoutedTo;
                    manager.RoutedToRemark = resignationDetails?.ManagerCheckList?.RoutedToRemark;
                    manager.ProjectDocumentsReturnedId = resignationDetails?.ManagerCheckList?.ProjectDocumentsReturnedId;
                    manager.ProjectDocumentsReturnedRemark = resignationDetails?.ManagerCheckList?.ProjectDocumentsReturnedRemark;
                    manager.RecoverPayNoticeId = resignationDetails?.ManagerCheckList?.RecoverPayNoticeId;
                    manager.RecoverPayNoticeRemark = resignationDetails?.ManagerCheckList?.RecoverPayNoticeRemark;
                    manager.RouteReporteesTo = resignationDetails?.ManagerCheckList?.RouteReporteesTo;
                    manager.WaivingOffNoticePeriodReason = resignationDetails?.ManagerCheckList?.WaivingOffNoticePeriodReason;
                    manager.TimesheetsId = resignationDetails?.ManagerCheckList?.TimesheetsId;
                    manager.TimesheetsRemark = resignationDetails?.ManagerCheckList?.TimesheetsRemark;
                    if (resignationDetails?.ManagerCheckList?.ManagerCheckListId > 0)
                    {
                        manager.ModifiedBy = resignationDetails?.ManagerCheckList?.ModifiedBy;
                        manager.ModifiedOn = DateTime.UtcNow;
                        _managerCheckListRepository.Update(manager);
                        await _managerCheckListRepository.SaveChangesAsync();

                    }
                    else
                    {
                        manager.CreatedBy = resignationDetails?.ManagerCheckList?.CreatedBy;
                        manager.CreatedOn = DateTime.UtcNow;
                        await _managerCheckListRepository.AddAsync(manager);
                        await _managerCheckListRepository.SaveChangesAsync();
                    }
                    if (manager.Status == "Completed")
                    {
                        managerStatus = "Completed";
                        itStatus = checklist.ITStatus == "Yet to Start" ? "Pending" : "";
                    }
                    else if (manager.Status == "In-Progress")
                    {
                        managerStatus = "In-Progress";
                    }
                }
                //else
                //{
                //    ManagerCheckList manager = new ManagerCheckList();
                //    manager = _managerCheckListRepository.GetManagerResignationChecklistByID(checkListId);
                //    if (manager != null)
                //    {
                //        manager.KnowledgeTransferId = null;
                //        manager.KnowledgeTransferRemark = null;
                //        manager.MailID = null;
                //        manager.RoutedTo = null;
                //        manager.RoutedToRemark = null;
                //        manager.ProjectDocumentsReturnedId = null;
                //        manager.ProjectDocumentsReturnedRemark = null;
                //        manager.RecoverPayNoticeId = null;
                //        manager.RecoverPayNoticeRemark = null;
                //        manager.RouteReporteesTo = null;
                //        manager.WaivingOffNoticePeriodReason = null;
                //        manager.TimesheetsId = null;
                //        manager.TimesheetsRemark = null;
                //        if (manager?.ManagerCheckListId > 0)
                //        {
                //            manager.ModifiedBy = resignationDetails?.ResignationChecklist?.ModifiedBy;
                //            manager.ModifiedOn = DateTime.UtcNow;
                //            _managerCheckListRepository.Update(manager);
                //            await _managerCheckListRepository.SaveChangesAsync();

                //        }
                //    }
                //}
                //PMO checklist

                /*
                    if (resignationDetails.PMOCheckList != null)
                    {
                        PMOCheckList pmo = new PMOCheckList();
                        if (resignationDetails?.PMOCheckList?.PMOCheckListId > 0)
                        {
                            pmo = _pmoCheckListRepository.GetPMOResignationChecklistByID(checkListId);
                        }
                        resignationDetails.IsPMOSubmited = pmo.Status == "Completed" ? true : false;
                        pmo.ResignationChecklistId = checkListId;
                        pmo.ApprovedBy = resignationDetails?.PMOCheckList?.ApprovedBy;
                        pmo.Status = resignationDetails?.IsSubmit == true ? "Completed" : pmo.PMOCheckListId == 0 && resignationDetails?.IsSubmit == false ? "In-Progress" : pmo.Status;
                        pmo.TimesheetsId = resignationDetails?.PMOCheckList?.TimesheetsId;
                        pmo.TimesheetsRemark = resignationDetails?.PMOCheckList?.TimesheetsRemark;

                        if (resignationDetails?.PMOCheckList?.PMOCheckListId > 0)
                        {
                            pmo.ModifiedBy = resignationDetails?.PMOCheckList?.ModifiedBy;
                            pmo.ModifiedOn = DateTime.UtcNow;
                            _pmoCheckListRepository.Update(pmo);
                            await _pmoCheckListRepository.SaveChangesAsync();
                        }
                        else
                        {
                            pmo.CreatedBy = resignationDetails?.PMOCheckList?.CreatedBy;
                            pmo.CreatedOn = DateTime.UtcNow;
                            await _pmoCheckListRepository.AddAsync(pmo);
                            await _pmoCheckListRepository.SaveChangesAsync();
                        }
                        if (pmo.Status == "Completed")
                        {
                            pmoStatus = "Completed";
                            itStatus = checklist.ITStatus == "Yet to Start" ? "Pending" : "";
                        }
                        else if (pmo.Status == "In-Progress")
                        {
                            pmoStatus = "In-Progress";
                        }
                    }
                    else
                    {

                        PMOCheckList pmo = new PMOCheckList();
                        pmo = _pmoCheckListRepository.GetPMOResignationChecklistByID(checkListId);
                        if (pmo != null)
                        {
                            pmo.TimesheetsId = null;
                            pmo.TimesheetsRemark = null;

                            if (pmo?.PMOCheckListId > 0)
                            {
                                pmo.ModifiedBy = resignationDetails?.ResignationChecklist?.ModifiedBy;
                                pmo.ModifiedOn = DateTime.UtcNow;
                                _pmoCheckListRepository.Update(pmo);
                                await _pmoCheckListRepository.SaveChangesAsync();
                            }
                        }

                    }
                */
                //IT checklist
                if (resignationDetails.ITCheckList != null && resignationDetails.CheckListType == "it")
                {
                    ITCheckList it = new ITCheckList();
                    if (resignationDetails?.ITCheckList?.ITCheckListId > 0)
                    {
                        it = _itCheckListRepository.GetITResignationChecklistByID(checkListId);
                    }
                    resignationDetails.IsITSubmited = it.Status == "Completed" ? true : false;
                    it.ResignationChecklistId = checkListId;
                    it.ApprovedBy = resignationDetails?.ITCheckList?.ApprovedBy;
                    it.Status = resignationDetails?.IsSubmit == true ? "Completed" : it.ITCheckListId == 0 && resignationDetails?.IsSubmit == false ? "In-Progress" : it.Status;
                    it.LoginDisabledId = resignationDetails?.ITCheckList?.LoginDisabledId;
                    it.LoginDisabledRemark = resignationDetails?.ITCheckList?.LoginDisabledRemark;
                    it.MailID = resignationDetails?.ITCheckList?.MailID;
                    it.RoutedToRemark = resignationDetails?.ITCheckList?.RoutedToRemark;
                    it.BiometricAccessTerminationId = resignationDetails?.ITCheckList?.BiometricAccessTerminationId;
                    it.BiometricAccessTerminationRemark = resignationDetails?.ITCheckList?.BiometricAccessTerminationRemark;
                    it.SystemAssetsRecoveredId = resignationDetails?.ITCheckList?.SystemAssetsRecoveredId;
                    it.SystemAssetsRecoveredRemark = resignationDetails?.ITCheckList?.SystemAssetsRecoveredRemark;
                    it.DATAcardReturnedId = resignationDetails?.ITCheckList?.DATAcardReturnedId;
                    it.DATAcardReturnedRemark = resignationDetails?.ITCheckList?.DATAcardReturnedRemark;
                    it.DamageRecoveryId = resignationDetails?.ITCheckList?.DamageRecoveryId;
                    it.DamageRecoveryRemark = resignationDetails?.ITCheckList?.DamageRecoveryRemark;
                    it.MacAddressRemovalId = resignationDetails?.ITCheckList?.MacAddressRemovalId;
                    it.MacAddressRemovalRemark = resignationDetails?.ITCheckList?.MacAddressRemovalRemark;
                    it.DataBackUpId = resignationDetails?.ITCheckList?.DataBackUpId;
                    it.DataBackUpRemark = resignationDetails?.ITCheckList?.DataBackUpRemark;
                    it.UserLaptopDataSize = resignationDetails?.ITCheckList?.UserLaptopDataSize;
                    it.UserLaptopDataSizeRemark = resignationDetails?.ITCheckList?.UserLaptopDataSizeRemark;
                    if (resignationDetails?.ITCheckList?.ITCheckListId > 0)
                    {
                        it.ModifiedBy = resignationDetails?.ITCheckList?.ModifiedBy;
                        it.ModifiedOn = DateTime.UtcNow;
                        _itCheckListRepository.Update(it);
                        await _itCheckListRepository.SaveChangesAsync();
                    }
                    else
                    {
                        it.CreatedBy = resignationDetails?.ITCheckList?.CreatedBy;
                        it.CreatedOn = DateTime.UtcNow;
                        await _itCheckListRepository.AddAsync(it);
                        await _itCheckListRepository.SaveChangesAsync();


                    }
                    if (it.Status == "Completed")
                    {
                        itStatus = "Completed";
                        adminStatus = checklist.AdminStatus == "Yet to Start" ? "Pending" : "";
                    }
                    else if (it.Status == "In-Progress")
                    {
                        itStatus = "In-Progress";
                    }
                }
                //else
                //{
                //    ITCheckList it = new ITCheckList();
                //    it = _itCheckListRepository.GetITResignationChecklistByID(checkListId);
                //    if (it != null)
                //    {
                //        it.LoginDisabledId = null;
                //        it.LoginDisabledRemark = null;
                //        it.MailID = null;
                //        it.RoutedToRemark = null;
                //        it.BiometricAccessTerminationId = null;
                //        it.BiometricAccessTerminationRemark = null;
                //        it.SystemAssetsRecoveredId = null;
                //        it.SystemAssetsRecoveredRemark = null;
                //        it.DATAcardReturnedId = null;
                //        it.DATAcardReturnedRemark = null;
                //        it.DamageRecoveryId = null;
                //        it.DamageRecoveryRemark = null;
                //        it.MacAddressRemovalId = null;
                //        it.MacAddressRemovalRemark = null;
                //        it.DataBackUpId = null;
                //        it.DataBackUpRemark = null;
                //        it.UserLaptopDataSize = null;
                //        it.UserLaptopDataSizeRemark = null;

                //        if (it?.ITCheckListId > 0)
                //        {
                //            it.ModifiedBy = resignationDetails?.ResignationChecklist?.ModifiedBy;
                //            it.ModifiedOn = DateTime.UtcNow;
                //            _itCheckListRepository.Update(it);
                //            await _itCheckListRepository.SaveChangesAsync();
                //        }
                //    }
                //}
                ////Admin checklist
                if (resignationDetails.AdminCheckList != null && resignationDetails.CheckListType == "admin")
                {
                    AdminCheckList admin = new AdminCheckList();
                    if (resignationDetails?.AdminCheckList?.AdminCheckListId > 0)
                    {
                        admin = _adminCheckListRepository.GetAdminResignationChecklistByID(checkListId);
                    }
                    resignationDetails.IsAdminSubmited = admin.Status == "Completed" ? true : false;
                    admin.ResignationChecklistId = checkListId;
                    admin.ApprovedBy = resignationDetails?.AdminCheckList?.ApprovedBy;
                    admin.Status = resignationDetails?.IsSubmit == true ? "Completed" : admin.AdminCheckListId == 0 && resignationDetails?.IsSubmit == false ? "In-Progress" : admin.Status;
                    admin.IdentityCardId = resignationDetails?.AdminCheckList?.IdentityCardId;
                    admin.IdentityCardRemark = resignationDetails?.AdminCheckList?.IdentityCardRemark;
                    admin.CabinKeysID = resignationDetails?.AdminCheckList?.CabinKeysID;
                    admin.CabinKeysRemark = resignationDetails?.AdminCheckList?.CabinKeysRemark;
                    admin.TravelCardId = resignationDetails?.AdminCheckList?.TravelCardId;
                    admin.TravelCardRemark = resignationDetails?.AdminCheckList?.TravelCardRemark;
                    admin.BusinessCardsId = resignationDetails?.AdminCheckList?.BusinessCardsId;
                    admin.BusinessCardsRemark = resignationDetails?.AdminCheckList?.BusinessCardsRemark;
                    admin.LibraryBooksId = resignationDetails?.AdminCheckList?.LibraryBooksId;
                    admin.LibraryBooksRemark = resignationDetails?.AdminCheckList?.LibraryBooksRemark;
                    admin.CompanyMobileId = resignationDetails?.AdminCheckList?.CompanyMobileId;
                    admin.CompanyMobileRemark = resignationDetails?.AdminCheckList?.CompanyMobileRemark;
                    admin.OtherRecovery = resignationDetails?.AdminCheckList?.OtherRecovery;
                    admin.BiometricAccessTerminationId = resignationDetails?.AdminCheckList?.BiometricAccessTerminationId;
                    admin.BiometricAccessTerminationRemark = resignationDetails?.AdminCheckList?.BiometricAccessTerminationRemark;

                    if (resignationDetails?.AdminCheckList?.AdminCheckListId > 0)
                    {
                        admin.ModifiedBy = resignationDetails?.AdminCheckList?.ModifiedBy;
                        admin.ModifiedOn = DateTime.UtcNow;
                        _adminCheckListRepository.Update(admin);
                        await _adminCheckListRepository.SaveChangesAsync();
                    }
                    else
                    {
                        admin.CreatedBy = resignationDetails?.AdminCheckList?.CreatedBy;
                        admin.CreatedOn = DateTime.UtcNow;
                        await _adminCheckListRepository.AddAsync(admin);
                        await _adminCheckListRepository.SaveChangesAsync();


                    }
                    if (admin.Status == "Completed")
                    {
                        adminStatus = "Completed";
                        financeStatus = checklist.FinanceStatus == "Yet to Start" ? "Pending" : "";
                    }
                    else if (admin.Status == "In-Progress")
                    {
                        adminStatus = "In-Progress";
                    }
                }
                //else
                //{
                //    AdminCheckList admin = new AdminCheckList();
                //    admin = _adminCheckListRepository.GetAdminResignationChecklistByID(checkListId);
                //    if (admin != null)
                //    {
                //        admin.IdentityCardId = null;
                //        admin.IdentityCardRemark = null;
                //        admin.CabinKeysID = null;
                //        admin.CabinKeysRemark = null;
                //        admin.TravelCardId = null;
                //        admin.TravelCardRemark = null;
                //        admin.BusinessCardsId = null;
                //        admin.BusinessCardsRemark = null;
                //        admin.LibraryBooksId = null;
                //        admin.LibraryBooksRemark = null;
                //        admin.CompanyMobileId = null;
                //        admin.CompanyMobileRemark = null;
                //        admin.OtherRecovery = null;
                //        admin.BiometricAccessTerminationId = null;
                //        admin.BiometricAccessTerminationRemark = null;
                //        if (admin?.AdminCheckListId > 0)
                //        {
                //            admin.ModifiedBy = resignationDetails?.ResignationChecklist?.ModifiedBy;
                //            admin.ModifiedOn = DateTime.UtcNow;
                //            _adminCheckListRepository.Update(admin);
                //            await _adminCheckListRepository.SaveChangesAsync();
                //        }
                //    }
                //}
                //Finance checklist
                if (resignationDetails.FinanceCheckList != null && resignationDetails.CheckListType == "finance")
                {
                    FinanceCheckList finance = new FinanceCheckList();
                    if (resignationDetails?.FinanceCheckList?.FinanceCheckListId > 0)
                    {
                        finance = _financeCheckListRepository.GetFinanceResignationChecklistByID(checkListId);
                    }
                    resignationDetails.IsFinanceSubmited = finance.Status == "Completed" ? true : false;
                    finance.ResignationChecklistId = checkListId;
                    finance.ApprovedBy = resignationDetails?.FinanceCheckList?.ApprovedBy;
                    finance.Status = resignationDetails?.IsSubmit == true ? "Completed" : finance.FinanceCheckListId == 0 && resignationDetails?.IsSubmit == false ? "In-Progress" : finance.Status;
                    finance.JoiningBonus = resignationDetails?.FinanceCheckList?.JoiningBonus;
                    finance.JoiningBonusRemark = resignationDetails?.FinanceCheckList?.JoiningBonusRemark;
                    finance.RetentionBonus = resignationDetails?.FinanceCheckList?.RetentionBonus;
                    finance.RetentionBonusRemark = resignationDetails?.FinanceCheckList?.RetentionBonusRemark;
                    finance.SalaryAdvance = resignationDetails?.FinanceCheckList?.SalaryAdvance;
                    finance.SalaryAdvanceRemark = resignationDetails?.FinanceCheckList?.SalaryAdvanceRemark;
                    finance.Loans = resignationDetails?.FinanceCheckList?.Loans;
                    finance.LoansRemark = resignationDetails?.FinanceCheckList?.LoansRemark;
                    finance.TravelAdvance = resignationDetails?.FinanceCheckList?.TravelAdvance;
                    finance.TravelAdvanceRemark = resignationDetails?.FinanceCheckList?.TravelAdvanceRemark;
                    finance.TravelCardReturned = resignationDetails?.FinanceCheckList?.TravelCardReturned;
                    finance.TravelCardReturnedRemark = resignationDetails?.FinanceCheckList?.TravelCardReturnedRemark;
                    finance.RelocationCost = resignationDetails?.FinanceCheckList?.RelocationCost;
                    finance.RelocationCostRemark = resignationDetails?.FinanceCheckList?.RelocationCostRemark;
                    finance.TravelKitAllowance = resignationDetails?.FinanceCheckList?.TravelKitAllowance;
                    finance.TravelKitAllowanceRemark = resignationDetails?.FinanceCheckList?.TravelKitAllowanceRemark;
                    finance.ITProofsId = resignationDetails?.FinanceCheckList?.ITProofsId;
                    finance.ITProofsRemark = resignationDetails?.FinanceCheckList?.ITProofsRemark;
                    finance.TrainingBond = resignationDetails?.FinanceCheckList?.TrainingBond;
                    finance.TrainingBondRemark = resignationDetails?.FinanceCheckList?.TrainingBondRemark;
                    finance.ITRecovery = resignationDetails?.FinanceCheckList?.ITRecovery;
                    finance.ITRecoveryRemark = resignationDetails?.FinanceCheckList?.ITRecoveryRemark;
                    finance.AdministrationRecovery = resignationDetails?.FinanceCheckList?.AdministrationRecovery;
                    finance.AdministrationRecoveryRemark = resignationDetails?.FinanceCheckList?.AdministrationRecoveryRemark;
                    finance.GratuityEligibilityId = resignationDetails?.FinanceCheckList?.GratuityEligibilityId;
                    finance.GratuityEligibilityRemark = resignationDetails?.FinanceCheckList?.GratuityEligibilityRemark;

                    if (resignationDetails?.FinanceCheckList?.FinanceCheckListId > 0)
                    {
                        finance.ModifiedBy = resignationDetails?.FinanceCheckList?.ModifiedBy;
                        finance.ModifiedOn = DateTime.UtcNow;
                        _financeCheckListRepository.Update(finance);
                        await _financeCheckListRepository.SaveChangesAsync();
                    }
                    else
                    {
                        finance.CreatedBy = resignationDetails?.FinanceCheckList?.CreatedBy;
                        finance.CreatedOn = DateTime.UtcNow;
                        await _financeCheckListRepository.AddAsync(finance);
                        await _financeCheckListRepository.SaveChangesAsync();

                    }
                    if (finance.Status == "Completed")
                    {
                        financeStatus = "Completed";
                        hrStatus = checklist.HRStatus == "Yet to Start" ? "Pending" : "";
                    }
                    else if (finance.Status == "In-Progress")
                    {
                        financeStatus = "In-Progress";
                    }
                }
                //else
                //{
                //    FinanceCheckList finance = new FinanceCheckList();
                //    finance = _financeCheckListRepository.GetFinanceResignationChecklistByID(checkListId);
                //    if (finance != null)
                //    {
                //        finance.JoiningBonus = null;
                //        finance.JoiningBonusRemark = null;
                //        finance.RetentionBonus = null;
                //        finance.RetentionBonusRemark = null;
                //        finance.SalaryAdvance = null;
                //        finance.SalaryAdvanceRemark = null;
                //        finance.Loans = null;
                //        finance.LoansRemark = null;
                //        finance.TravelAdvance = null;
                //        finance.TravelAdvanceRemark = null;
                //        finance.TravelCardReturned = null;
                //        finance.TravelCardReturnedRemark = null;
                //        finance.RelocationCost = null;
                //        finance.RelocationCostRemark = null;
                //        finance.TravelKitAllowance = null;
                //        finance.TravelKitAllowanceRemark = null;
                //        finance.ITProofsId = null;
                //        finance.ITProofsRemark = null;
                //        finance.TrainingBond = null;
                //        finance.TrainingBondRemark = null;
                //        finance.ITRecovery = null;
                //        finance.ITRecoveryRemark = null;
                //        finance.AdministrationRecovery = null;
                //        finance.AdministrationRecoveryRemark = null;
                //        finance.GratuityEligibilityId = null;
                //        finance.GratuityEligibilityRemark = null;

                //        if (finance.FinanceCheckListId > 0)
                //        {
                //            finance.ModifiedBy = resignationDetails?.ResignationChecklist?.ModifiedBy;
                //            finance.ModifiedOn = DateTime.UtcNow;
                //            _financeCheckListRepository.Update(finance);
                //            await _financeCheckListRepository.SaveChangesAsync();
                //        }
                //    }
                //}
                //HR checklist
                if (resignationDetails.HRCheckList != null && resignationDetails.CheckListType == "hr")
                {
                    HRCheckList hr = new HRCheckList();
                    if (resignationDetails?.HRCheckList?.HRCheckListId > 0)
                    {
                        hr = _hrCheckListRepository.GetHRResignationChecklistByID(checkListId);
                    }
                    hr.ResignationChecklistId = checkListId;
                    hr.ApprovedBy = resignationDetails?.HRCheckList?.ApprovedBy;
                    hr.Status = resignationDetails?.IsSubmit == true ? "Completed" : hr.HRCheckListId == 0 && resignationDetails?.IsSubmit == false ? "In-Progress" : hr.Status;
                    hr.NoticePayId = resignationDetails?.HRCheckList?.NoticePayId;
                    hr.NoticePayDay = resignationDetails?.HRCheckList?.NoticePayDay;
                    hr.NoticePayRemark = resignationDetails?.HRCheckList?.NoticePayRemark;
                    hr.ELBalanceId = resignationDetails?.HRCheckList?.ELBalanceId;
                    hr.ELBalanceDay = resignationDetails?.HRCheckList?.ELBalanceDay;
                    hr.ELBalanceRemark = resignationDetails?.HRCheckList?.ELBalanceRemark;
                    hr.NoticePeriodWaiverRequestId = resignationDetails?.HRCheckList?.NoticePeriodWaiverRequestId;
                    hr.NoticePeriodWaiverRequestRemark = resignationDetails?.HRCheckList?.NoticePeriodWaiverRequestRemark;
                    hr.LeaveBalanceSummaryId = resignationDetails?.HRCheckList?.LeaveBalanceSummaryId;
                    hr.LeaveBalanceSummaryRemark = resignationDetails?.HRCheckList?.LeaveBalanceSummaryRemark;
                    hr.RehireEligibleId = resignationDetails?.HRCheckList?.RehireEligibleId;
                    hr.Comments = resignationDetails?.HRCheckList?.Comments;

                    if (resignationDetails?.HRCheckList?.HRCheckListId > 0)
                    {
                        hr.ModifiedBy = resignationDetails?.HRCheckList?.ModifiedBy;
                        hr.ModifiedOn = DateTime.UtcNow;
                        _hrCheckListRepository.Update(hr);
                        await _hrCheckListRepository.SaveChangesAsync();
                    }
                    else
                    {
                        hr.CreatedBy = resignationDetails?.HRCheckList?.CreatedBy;
                        hr.CreatedOn = DateTime.UtcNow;
                        await _hrCheckListRepository.AddAsync(hr);
                        await _hrCheckListRepository.SaveChangesAsync();

                    }
                    if (hr.Status == "Completed")
                    {
                        hrStatus = "Completed";
                    }
                    else if (hr.Status == "In-Progress")
                    {
                        hrStatus = "In-Progress";
                    }
                }
                //else
                //{
                //    HRCheckList hr = new HRCheckList();
                //    hr = _hrCheckListRepository.GetHRResignationChecklistByID(checkListId);
                //    if (hr != null)
                //    {
                //        hr.NoticePayId = null;
                //        hr.NoticePayDay = null;
                //        hr.NoticePayRemark = null;
                //        hr.ELBalanceId = null;
                //        hr.ELBalanceDay = null;
                //        hr.ELBalanceRemark = null;
                //        hr.NoticePeriodWaiverRequestId = null;
                //        hr.NoticePeriodWaiverRequestRemark = null;
                //        hr.LeaveBalanceSummaryId = null;
                //        hr.LeaveBalanceSummaryRemark = null;
                //        hr.RehireEligibleId = null;
                //        hr.Comments = null;

                //        if (hr.HRCheckListId > 0)
                //        {
                //            hr.ModifiedBy = resignationDetails?.ResignationChecklist?.ModifiedBy;
                //            hr.ModifiedOn = DateTime.UtcNow;
                //            _hrCheckListRepository.Update(hr);
                //            await _hrCheckListRepository.SaveChangesAsync();
                //        }
                //    }
                //}
                if (managerStatus != "" || pmoStatus != "" || itStatus != "" || adminStatus != ""
                    || financeStatus != "" || hrStatus != "")
                {
                    ResignationChecklist updateStatus = _resignationChecklistRepository.GetResignationChecklistByID(resignationDetails.ResignationChecklistId == 0 ? checklist.ResignationChecklistId : resignationDetails.ResignationChecklistId);
                    updateStatus.ManagerStatus = managerStatus != "" ? managerStatus : updateStatus.ManagerStatus;
                    updateStatus.PMOStatus = pmoStatus != "" ? pmoStatus : updateStatus.PMOStatus;
                    updateStatus.ITStatus = itStatus != "" ? itStatus : updateStatus.ITStatus;
                    updateStatus.AdminStatus = adminStatus != "" ? adminStatus : updateStatus.AdminStatus;
                    updateStatus.FinanceStatus = financeStatus != "" ? financeStatus : updateStatus.FinanceStatus;
                    updateStatus.HRStatus = hrStatus != "" ? hrStatus : updateStatus.HRStatus;
                    _resignationChecklistRepository.Update(updateStatus);
                    await _resignationChecklistRepository.SaveChangesAsync();
                }
                resignationDetails.ResignationChecklistId = checkListId;
                resignationDetails.ExitCheckListTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("ExitCheckList");
                return resignationDetails;

            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion
        #region Get resignation checklist details by id
        public async Task<ResignationChecklistView> GetResignationChecklistById(ResignationChecklistView checkList)
        {
            ResignationChecklistView resignationDetails = new ResignationChecklistView();
            try
            {
                resignationDetails.ManagerCheckList = _managerCheckListRepository.GetManagerResignationChecklistByID(checkList.ResignationChecklistId);
                resignationDetails.PMOCheckList = _pmoCheckListRepository.GetPMOResignationChecklistByID(checkList.ResignationChecklistId);
                resignationDetails.ITCheckList = _itCheckListRepository.GetITResignationChecklistByID(checkList.ResignationChecklistId);
                resignationDetails.AdminCheckList = _adminCheckListRepository.GetAdminResignationChecklistByID(checkList.ResignationChecklistId);
                resignationDetails.FinanceCheckList = _financeCheckListRepository.GetFinanceResignationChecklistByID(checkList.ResignationChecklistId);
                resignationDetails.HRCheckList = _hrCheckListRepository.GetHRResignationChecklistByID(checkList.ResignationChecklistId);
                //resignationDetails.CheckListView = _checkListViewRepository.GetCheckListViewByRole(checkList.CheckListEdit);
                resignationDetails.ResignationChecklist = _resignationChecklistRepository.GetResignationChecklistByID(checkList.ResignationChecklistId);
                resignationDetails.employeeResignationChecklistDetails = _employeeResignationDetailsRepository.GetEmployeeDetailByResignationID(resignationDetails?.ResignationChecklist?.ResignationDetailsId == null ? 0 : (int)resignationDetails?.ResignationChecklist?.ResignationDetailsId);
                return resignationDetails;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion
        #region Get my checklist details
        public List<ResignationChecklistDetails> GetMyCheckListDetails(AllResignationInputView resignationInputView)
        {
            return _resignationChecklistRepository.GetMyCheckListDetails(resignationInputView);

        }
        #endregion
        #region Get reportees checklist details
        public List<ChecklistEmployeeView> GetReporteesCheckListDetails(AllResignationInputView resignationInputView)
        {
            return _resignationChecklistRepository.GetReporteesCheckListDetails(resignationInputView);
        }
        #endregion
        #region Get delete checklist details by id
        public async Task<bool> DeleteChecklistById(int checklistId)
        {
            ResignationChecklist checklist = _resignationChecklistRepository.GetResignationChecklistByID(checklistId);
            if (checklist != null)
            {
                _resignationChecklistRepository.Delete(checklist);
                _resignationChecklistRepository.SaveChangesAsync();

                ManagerCheckList managerCheckList = _managerCheckListRepository.GetManagerResignationChecklistByID(checklistId);
                if (managerCheckList != null)
                {
                    _managerCheckListRepository.Delete(managerCheckList);
                    _managerCheckListRepository.SaveChangesAsync();
                }
                PMOCheckList pmoCheckList = _pmoCheckListRepository.GetPMOResignationChecklistByID(checklistId);
                if (pmoCheckList != null)
                {
                    _pmoCheckListRepository.Delete(pmoCheckList);
                    _pmoCheckListRepository.SaveChangesAsync();
                }
                ITCheckList itCheckList = _itCheckListRepository.GetITResignationChecklistByID(checklistId);
                if (itCheckList != null)
                {
                    _itCheckListRepository.Delete(itCheckList);
                    _itCheckListRepository.SaveChangesAsync();
                }
                AdminCheckList adminCheckList = _adminCheckListRepository.GetAdminResignationChecklistByID(checklistId);
                if (adminCheckList != null)
                {
                    _adminCheckListRepository.Delete(adminCheckList);
                    _adminCheckListRepository.SaveChangesAsync();
                }
                FinanceCheckList financeCheckList = _financeCheckListRepository.GetFinanceResignationChecklistByID(checklistId);
                if (financeCheckList != null)
                {
                    _financeCheckListRepository.Delete(financeCheckList);
                    _financeCheckListRepository.SaveChangesAsync();
                }
                HRCheckList hrCheckList = _hrCheckListRepository.GetHRResignationChecklistByID(checklistId);
                if (hrCheckList != null)
                {
                    _hrCheckListRepository.Delete(hrCheckList);
                    _hrCheckListRepository.SaveChangesAsync();
                }
            }
            return true;
        }
        #endregion
        #region Get checklist details by id
        public int GetResignationChecklistByEmployeeId(int employeeId)
        {
            return _resignationChecklistRepository.GetResignationChecklistByEmployeeId(employeeId);
        }
        #endregion 
        #region Get employee resignation details
        public List<KeyWithIntValue> GetLastResignationIdByEmployeeList(List<int> employeeIdList)
        {
            return _employeeResignationDetailsRepository.GetLastResignationIdByEmployeeList(employeeIdList);
        }
        #endregion
        #region Get interview details by id
        public int GetResignationInterviewByEmployeeId(int employeeId)
        {
            return _resignationInterviewRepository.GetResignationInterviewByEmployeeId(employeeId);
        }
        #endregion

        #region Get interview details by id
        public NotificationMasterData GetResignationInterviewNotification(int days)
        {
            NotificationMasterData interviewNotification = new NotificationMasterData();
            interviewNotification.EmployeeList = _employeeResignationDetailsRepository.GetResignationInterviewNotification(days);
            interviewNotification.EmailTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("ExitInterviewNotification");
            return interviewNotification;
        }
        #endregion 
        #region Get Checklist Submission Notification Employee
        public NotificationMasterData GetChecklistSubmissionNotificationEmployee(int days)
        {
            NotificationMasterData checkLisNotification = new NotificationMasterData();
            checkLisNotification.EmployeeList = _employeeResignationDetailsRepository.GetChecklistSubmissionNotificationEmployee(days);
            checkLisNotification.EmailTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("ExitCheckListNotification");
            return checkLisNotification;
        }
        #endregion 
        #region Get Checklist Complete Notification Employee
        public NotificationMasterData GetChecklistCompleteNotificationEmployee(int days)
        {
            NotificationMasterData checkLisNotification = new NotificationMasterData();
            checkLisNotification.ChecklistEmployeeList = _resignationChecklistRepository.GetChecklistCompleteNotificationEmployee(days);
            checkLisNotification.EmailTemplate = _exitManagementEmailTemplateRepository.GetEmailTemplateByName("ExitCheckListCompleteNotification");
            return checkLisNotification;
        }
        #endregion 
        #region Get resignation employee Filter list
        //public List<int> GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter)
        //{
        //    return _resignationChecklistRepository.GetResignationEmployeeListByFilter(resignationEmployeeFilter);
        //}
        public List<ResignationEmployeeMasterView> GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            List<ResignationEmployeeMasterView> resEmpList = _resignationChecklistRepository.GetResignationEmployeeListByFilter(resignationEmployeeFilter);
            //if (resignationEmployeeFilter.IsCheckList)
            //{
            //    List<ChecklistEmployeeView> resEmpCheckList = _resignationChecklistRepository.GetReporteesCheckListDetails(resignationEmployeeFilter.ResignationInputFilter);
            //    foreach (ResignationEmployeeMasterView item in resEmpList)
            //    {
            //        item.RoleName = resEmpCheckList.Where(x => x.EmployeeID == item.EmployeeID && x.ResignationChecklistId == (int)item.ResignationChecklistId).Select(r => r.RoleName).FirstOrDefault();
            //    }
            //}
            return resEmpList;
        }
        #endregion 

        #region Get Exit Check List Letter data
        public List<KeyWithValue> GetExitCheckListLetter()
        {
            return _appConstantsRepository.GetAppConstantByType("ExitCheckListLetter");
        }
        #endregion
        #region
        public int GetResignationEmployeeListByFilterCount(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            int count = _resignationChecklistRepository.GetResignationEmployeeListByFilterCount(resignationEmployeeFilter);
            //if (resignationEmployeeFilter.IsCheckList)
            //{
            //    List<ChecklistEmployeeView> resEmpCheckList = _resignationChecklistRepository.GetReporteesCheckListDetails(resignationEmployeeFilter.ResignationInputFilter);
            //    foreach (ResignationEmployeeMasterView item in resEmpList)
            //    {
            //        item.RoleName = resEmpCheckList.Where(x => x.EmployeeID == item.EmployeeID && x.ResignationChecklistId == (int)item.ResignationChecklistId).Select(r => r.RoleName).FirstOrDefault();
            //    }
            //}
            return count;
        }
       #endregion
    }
}
