using ExitManagement.DAL.DBContext;
using SharedLibraries.Common;
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
    public interface IResignationChecklistRepository : IBaseRepository<ResignationChecklist>
    {
        ResignationChecklist GetResignationChecklistByID(int checklistId);
        List<ResignationEmployeeMasterView> GetResignationCheckListEmployee(List<ResignationEmployeeMasterView> employeeList);
        List<ResignationChecklistDetails> GetMyCheckListDetails(AllResignationInputView resignationInputView);
        List<ChecklistEmployeeView> GetReporteesCheckListDetails(AllResignationInputView resignationInputView);
        int GetResignationChecklistByEmployeeId(int employeeId);
        List<ResignationChecklist> GetChecklistCompleteNotificationEmployee(int days);
        List<ResignationEmployeeMasterView> GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter);
        int GetResignationEmployeeListByFilterCount(ResignationEmployeeFilterView resignationEmployeeFilter);
    }


    public class ResignationChecklistRepository : BaseRepository<ResignationChecklist>, IResignationChecklistRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ResignationChecklistRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public ResignationChecklist GetResignationChecklistByID(int checklistId)
        {
            return dbContext.ResignationChecklist.Where(x => x.ResignationChecklistId == checklistId).FirstOrDefault();
        }
        public List<ResignationEmployeeMasterView> GetResignationCheckListEmployee(List<ResignationEmployeeMasterView> employeeList)
        {
            List<ResignationEmployeeMasterView> result = new List<ResignationEmployeeMasterView>();
            foreach (ResignationEmployeeMasterView item in employeeList)
            {
                int? resignId = dbContext.EmployeeResignationDetails.Where(x => x.EmployeeId == item.EmployeeID && x.ResignationStatus != "Rejected" && x.ResignationStatus != "Cancelled" && x.ResignationStatus != "Withdrawal Approved").OrderByDescending(x => x.CreatedOn).Select(x => x.EmployeeResignationDetailsId).FirstOrDefault();
                if (resignId > 0)
                {
                    ResignationChecklist data = dbContext.ResignationChecklist.Where(x => x.ResignationDetailsId == resignId).FirstOrDefault();
                    if (data == null)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
            //var resignationChecklist = dbContext.ResignationChecklist.ToArray();
            //List<ResignationEmployeeMasterView> employees = (from a in resignationChecklist
            //                                                 join b in employeeList.ToArray() on a.EmployeeID equals b.EmployeeID
            //                                                 where a.CreatedOn >= b.ResignationDate
            //                                                 select b).ToList();
            //return employeeList.Except(employees).ToList();
        }
        public List<ResignationChecklistDetails> GetMyCheckListDetails(AllResignationInputView resignationInputView)
        {
            if (resignationInputView.IsAdmin == true)
            {
                return dbContext.ResignationChecklist.Select(x =>
                new ResignationChecklistDetails
                {
                    ResignationChecklistId = x.ResignationChecklistId,
                    EmployeeID = x.EmployeeID,
                    ManagerId = x.ManagerId,
                    IsAgreeCheckList = x.IsAgreeCheckList,
                    ManagerStatus = x.ManagerStatus,
                    PMOStatus = x.PMOStatus,
                    ITStatus = x.ITStatus,
                    AdminStatus = x.AdminStatus,
                    FinanceStatus = x.FinanceStatus,
                    HRStatus = x.HRStatus,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    Status= x.HRStatus=="Completed"?"Completed":"In-Progress",
                    ResignationDetailsId=x.ResignationDetailsId
                }).OrderByDescending(x => x.CreatedOn).ToList();

            }
            else
            {
                return dbContext.ResignationChecklist.Where(x => x.EmployeeID == resignationInputView.EmployeeId).Select(x =>
                new ResignationChecklistDetails
                {
                    ResignationChecklistId = x.ResignationChecklistId,
                    EmployeeID = x.EmployeeID,
                    ManagerId = x.ManagerId,
                    IsAgreeCheckList = x.IsAgreeCheckList,
                    ManagerStatus = x.ManagerStatus,
                    PMOStatus = x.PMOStatus,
                    ITStatus = x.ITStatus,
                    AdminStatus = x.AdminStatus,
                    FinanceStatus = x.FinanceStatus,
                    HRStatus = x.HRStatus,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    Status = x.HRStatus == "Completed" ? "Completed" : "In-Progress",
                    ResignationDetailsId = x.ResignationDetailsId
                }).OrderByDescending(x => x.CreatedOn).ToList();

            }

        }
        public List<ChecklistEmployeeView> GetReporteesCheckListDetails(AllResignationInputView resignationInputView)
        {
          
            List<ChecklistEmployeeView> managerCheckList = new List<ChecklistEmployeeView>();
            List<ChecklistEmployeeView> allCheckList = new List<ChecklistEmployeeView>();
            foreach(string item in resignationInputView?.EmployeeRole)
            {
                List<ChecklistEmployeeView> approvalcheckList = new List<ChecklistEmployeeView>();
                //if (item?.ToLower() == resignationInputView?.PMORole?.ToLower())
                //{
                //    approvalcheckList = dbContext.ResignationChecklist.Where(x => x.PMOStatus != "Yet to Start").Select(x =>
                //                                                                      new ChecklistEmployeeView
                //                                                                      {
                //                                                                          EmployeeID = x.EmployeeID,
                //                                                                          ResignationChecklistId = x.ResignationChecklistId,
                //                                                                          CreatedOn = x.CreatedOn,
                //                                                                          Status=x.PMOStatus,
                //                                                                          RoleName= item,
                //                                                                          ResignationDetailsId = x.ResignationDetailsId,
                //                                                                          ManagerStatus = x.ManagerStatus,
                //                                                                          AdminStatus = x.AdminStatus,
                //                                                                          ITStatus = x.ITStatus,
                //                                                                          HRStatus = x.HRStatus,
                //                                                                          FinanceStatus=x.FinanceStatus,
                //                                                                          IsAgreeCheckList = x.IsAgreeCheckList
                //                                                                      }).OrderByDescending(x => x.CreatedOn).ToList();
                //}
                if (item?.ToLower() == resignationInputView?.ITRole?.ToLower())
                {
                    approvalcheckList = dbContext.ResignationChecklist.Where(x => x.ITStatus != "Yet to Start").Select(x =>
                                                                                   new ChecklistEmployeeView
                                                                                   {
                                                                                       EmployeeID = x.EmployeeID,
                                                                                       ResignationChecklistId = x.ResignationChecklistId,
                                                                                       CreatedOn = x.CreatedOn,
                                                                                       Status = x.ITStatus,
                                                                                       RoleName = item,
                                                                                       ResignationDetailsId = x.ResignationDetailsId,
                                                                                       ManagerStatus = x.ManagerStatus,
                                                                                       AdminStatus = x.AdminStatus,
                                                                                       ITStatus = x.ITStatus,
                                                                                       HRStatus = x.HRStatus,
                                                                                       FinanceStatus = x.FinanceStatus,
                                                                                       IsAgreeCheckList = x.IsAgreeCheckList
                                                                                   }).OrderByDescending(x => x.CreatedOn).ToList();
                }
                else if (item?.ToLower() == resignationInputView?.AdminRole?.ToLower())
                {
                    approvalcheckList = dbContext.ResignationChecklist.Where(x => x.AdminStatus != "Yet to Start").Select(x =>
                                                                                   new ChecklistEmployeeView
                                                                                   {
                                                                                       EmployeeID = x.EmployeeID,
                                                                                       ResignationChecklistId = x.ResignationChecklistId,
                                                                                       CreatedOn = x.CreatedOn,
                                                                                       Status = x.AdminStatus,
                                                                                       RoleName = item,
                                                                                       ResignationDetailsId = x.ResignationDetailsId,
                                                                                       ManagerStatus = x.ManagerStatus,
                                                                                       AdminStatus = x.AdminStatus,
                                                                                       ITStatus = x.ITStatus,
                                                                                       HRStatus = x.HRStatus,
                                                                                       FinanceStatus = x.FinanceStatus,
                                                                                       IsAgreeCheckList = x.IsAgreeCheckList
                                                                                   }).OrderByDescending(x => x.CreatedOn).ToList();
                }
                else if (item?.ToLower() == resignationInputView?.FinanceRole?.ToLower())
                {
                    approvalcheckList = dbContext.ResignationChecklist.Where(x => x.FinanceStatus != "Yet to Start").Select(x =>
                                                                                   new ChecklistEmployeeView
                                                                                   {
                                                                                       EmployeeID = x.EmployeeID,
                                                                                       ResignationChecklistId = x.ResignationChecklistId,
                                                                                       CreatedOn = x.CreatedOn,
                                                                                       Status = x.FinanceStatus,
                                                                                       RoleName = item,
                                                                                       ResignationDetailsId = x.ResignationDetailsId,
                                                                                       ManagerStatus = x.ManagerStatus,
                                                                                       AdminStatus = x.AdminStatus,
                                                                                       ITStatus = x.ITStatus,
                                                                                       HRStatus = x.HRStatus,
                                                                                       FinanceStatus = x.FinanceStatus,
                                                                                       IsAgreeCheckList = x.IsAgreeCheckList
                                                                                   }).OrderByDescending(x => x.CreatedOn).ToList();
                }
                else if (item?.ToLower() == resignationInputView?.HRRole?.ToLower())
                {
                    approvalcheckList = dbContext.ResignationChecklist.Where(x => x.HRStatus != "Yet to Start").Select(x =>
                                                                                   new ChecklistEmployeeView
                                                                                   {
                                                                                       EmployeeID = x.EmployeeID,
                                                                                       ResignationChecklistId = x.ResignationChecklistId,
                                                                                       CreatedOn = x.CreatedOn,
                                                                                       Status = x.HRStatus,
                                                                                       RoleName = item,
                                                                                       ResignationDetailsId = x.ResignationDetailsId,
                                                                                       ManagerStatus = x.ManagerStatus,
                                                                                       AdminStatus = x.AdminStatus,
                                                                                       ITStatus = x.ITStatus,
                                                                                       HRStatus = x.HRStatus,
                                                                                       FinanceStatus = x.FinanceStatus,
                                                                                       IsAgreeCheckList = x.IsAgreeCheckList
                                                                                   }).OrderByDescending(x => x.CreatedOn).ToList();
                }
               
                if(approvalcheckList?.Count>0)
                    allCheckList = allCheckList.Concat(approvalcheckList).ToList();
            }
            
            managerCheckList = dbContext.ResignationChecklist.Where(x => resignationInputView.ReporteesList.Contains(x.EmployeeID == null ? 0 : (int)x.EmployeeID)).Select(x =>
                                                                               new ChecklistEmployeeView
                                                                               {
                                                                                   EmployeeID = x.EmployeeID,
                                                                                   ResignationChecklistId = x.ResignationChecklistId,
                                                                                   CreatedOn = x.CreatedOn,
                                                                                   Status = x.ManagerStatus,
                                                                                   RoleName = "manager",
                                                                                   ResignationDetailsId = x.ResignationDetailsId,
                                                                                   ManagerStatus = x.ManagerStatus,
                                                                                   AdminStatus = x.AdminStatus,
                                                                                   ITStatus = x.ITStatus,
                                                                                   HRStatus = x.HRStatus,
                                                                                   FinanceStatus = x.FinanceStatus,
                                                                                   IsAgreeCheckList = x.IsAgreeCheckList
                                                                               }).OrderByDescending(x => x.CreatedOn).ToList();
            if (managerCheckList?.Count > 0)
            {
                allCheckList = allCheckList.Concat(managerCheckList).OrderByDescending(x=>x.Status).Distinct().ToList();
            }
            //if (allCheckList?.Count > 0)
            //{
            //    foreach (ChecklistEmployeeView item in allCheckList)
            //    {
            //        string status = "";
            //        ResignationChecklist checklist = dbContext.ResignationChecklist.Where(x => x.ResignationChecklistId == item.ResignationChecklistId).FirstOrDefault();
            //        if (managerCheckList.Where(x => x.ResignationChecklistId == item.ResignationChecklistId).Any())
            //        {
            //            status = checklist.ManagerStatus;
            //        }
            //        if (status == "" || status == "Completed")
            //        {

            //            if (resignationInputView?.EmployeeRole?.ToLower() == resignationInputView?.PMORole)
            //            {
            //                status = checklist.PMOStatus;
            //            }
            //            else if (resignationInputView?.EmployeeRole?.ToLower() == resignationInputView?.ITRole)
            //            {
            //                status = checklist.ITStatus;
            //            }
            //            else if (resignationInputView?.EmployeeRole?.ToLower() == resignationInputView?.AdminRole)
            //            {
            //                status = checklist.AdminStatus;
            //            }
            //            else if (resignationInputView?.EmployeeRole?.ToLower() == resignationInputView?.FinanceRole)
            //            {
            //                status = checklist.FinanceStatus;
            //            }
            //            else if (resignationInputView?.EmployeeRole?.ToLower() == resignationInputView?.HRRole)
            //            {
            //                status = checklist.HRStatus;
            //            }
            //        }
            //        item.Status = status;
            //    }
            //}
            return allCheckList;
        }
        public int GetResignationChecklistByEmployeeId(int employeeId)
        {
            int? resignId=dbContext.EmployeeResignationDetails.Where(x => x.EmployeeId == employeeId).OrderByDescending(x => x.CreatedOn).Select(x => x.EmployeeResignationDetailsId).FirstOrDefault();
            if(resignId !=null && resignId>0)
            {
               if(dbContext.ResignationChecklist.Where(x => x.EmployeeID == employeeId && x.ResignationDetailsId == resignId).Any())
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
        public List<ResignationChecklist> GetChecklistCompleteNotificationEmployee(int days)
        {            
           return (from a in dbContext.EmployeeResignationDetails
                   join b in dbContext.ResignationChecklist on a.EmployeeResignationDetailsId equals b.ResignationDetailsId
                   where
           ((a.ResignationStatus == "Pending" || a.ResignationStatus == "Approved") && a.RelievingDate != null && a.RelievingDate.Value.AddDays(days).Date <= DateTime.Now.Date && a.RelievingDate.Value.Date >=  DateTime.Now.Date) && (b.ManagerStatus == "Pending" || b.ManagerStatus == "In-Progress" ||
           b.PMOStatus == "Pending" || b.PMOStatus == "In-Progress" ||
           b.ITStatus == "Pending" || b.ITStatus == "In-Progress" ||
           b.AdminStatus == "Pending" || b.AdminStatus == "In-Progress" ||
           b.FinanceStatus == "Pending" || b.FinanceStatus == "In-Progress" ||
           b.HRStatus == "Pending" || b.HRStatus == "In-Progress")
           select b).ToList();
        }

        public List<int> GetResignationEmployeeListByFilter_Backup(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            if (resignationEmployeeFilter.IsCheckList)
                return (from users in dbContext.EmployeeResignationDetails
                        join b in dbContext.ResignationChecklist on users.EmployeeResignationDetailsId equals b.ResignationDetailsId
                        where users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Approved" : resignationEmployeeFilter.ResignationStatus)
                        && users.ReportingManagerId == (resignationEmployeeFilter.EmployeeId == 0 ? (int)users.ReportingManagerId : resignationEmployeeFilter.EmployeeId)
                        && b.IsAgreeCheckList == true
                        select users.EmployeeId).ToList();
            else
                return (from users in dbContext.EmployeeResignationDetails
                        where users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Pending" : resignationEmployeeFilter.ResignationStatus)
                        && users.ReportingManagerId == (resignationEmployeeFilter.EmployeeId == 0 ? (int)users.ReportingManagerId : resignationEmployeeFilter.EmployeeId)
                        select users.EmployeeId).ToList();
        }
        
        public List<ResignationEmployeeMasterView> GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            List<string> chekListStatus = new List<string> {"",""};
            List<string> resignListStatus = new List<string> {"Pending","Withdrawal Pending"};
           
            if (!string.IsNullOrEmpty(resignationEmployeeFilter?.ResignationStatus))
            {
                resignListStatus =  new List<string> { resignationEmployeeFilter?.ResignationStatus };
            }
            if (resignationEmployeeFilter.IsAdmin == true)
            {
                if (resignationEmployeeFilter.IsCheckList)
                    return (from users in dbContext.EmployeeResignationDetails
                            join chk in dbContext.ResignationChecklist on users.EmployeeResignationDetailsId equals chk.ResignationDetailsId
                            where users.RelievingDate != null && chk.IsAgreeCheckList == true
                            && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Approved" : resignationEmployeeFilter.ResignationStatus)
                            && users.EmployeeId != (resignationEmployeeFilter.EmployeeId)
                            && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                            && true == (resignationEmployeeFilter.IsActive == true ? users.RelievingDate >= DateTime.Today : users.RelievingDate < DateTime.Today)
                            select new ResignationEmployeeMasterView
                            {
                                EmployeeID = users.EmployeeId,
                                EmployeeName = users.EmployeeName,
                                FormattedEmployeeID = users.FormattedEmployeeId,
                                RelievingDate = users.RelievingDate,
                                ResignationDetailsId = users.EmployeeResignationDetailsId,
                                ResignationStatus = users.ResignationStatus,
                                ResignationChecklistId = chk.ResignationChecklistId,
                                ManagerStatus = chk.ManagerStatus,
                                ITStatus = chk.ITStatus,
                                AdminStatus = chk.AdminStatus,
                                FinanceStatus = chk.FinanceStatus,
                                HRStatus = chk.HRStatus,
                                ProfilePic = users.ProfilePicture
                            }).OrderByDescending(x => x.RelievingDate).Skip(resignationEmployeeFilter.NoOfRecord * (resignationEmployeeFilter.PageNumber))
                        .Take(resignationEmployeeFilter.NoOfRecord).Distinct().ToList();
                else
                    return (from users in dbContext.EmployeeResignationDetails
                            where users.RelievingDate != null
                            && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? users.ResignationStatus : resignationEmployeeFilter.ResignationStatus)
                            && users.EmployeeId != (resignationEmployeeFilter.EmployeeId)
                            && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                            && true == (resignationEmployeeFilter.IsActive == true ? users.RelievingDate >= DateTime.Today : users.RelievingDate < DateTime.Today)
                            select new ResignationEmployeeMasterView
                            {
                                EmployeeID = users.EmployeeId,
                                EmployeeName = users.EmployeeName,
                                FormattedEmployeeID = users.FormattedEmployeeId,
                                RelievingDate = users.RelievingDate,
                                ResignationDetailsId = users.EmployeeResignationDetailsId,
                                ResignationStatus = users.ResignationStatus,
                                ProfilePic = users.ProfilePicture
                            }).OrderByDescending(x => x.RelievingDate).Skip(resignationEmployeeFilter.NoOfRecord * (resignationEmployeeFilter.PageNumber))
                        .Take(resignationEmployeeFilter.NoOfRecord).Distinct().ToList();
            }
            else 
            {
                if (resignationEmployeeFilter.IsCheckList)
                {
                    var resEmpCheckList = GetReporteesCheckListDetails(resignationEmployeeFilter.ResignationInputFilter).ToArray();
                    var resignationdetail = dbContext.EmployeeResignationDetails.ToArray();
                    return (from users in resignationdetail
                            join chk in resEmpCheckList on users.EmployeeResignationDetailsId equals chk.ResignationDetailsId
                            //join approver in dbContext.ResignationApprovalStatus on users.EmployeeResignationDetailsId equals approver.EmployeeResignationDetailsId
                            where users.RelievingDate != null && chk.IsAgreeCheckList == true
                            //&& resignationEmployeeFilter.ReporteesList.Contains(chk.EmployeeID == null ? 0 : (int)chk.EmployeeID)
                            && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Approved" : resignationEmployeeFilter.ResignationStatus)
                            //&& resignationEmployeeFilter.ReporteesList.Contains(users.ReportingManagerId == null ? 0 : (int)users.ReportingManagerId)
                            && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                            && true == (resignationEmployeeFilter.IsActive == true ? users.RelievingDate >= DateTime.Today : users.RelievingDate < DateTime.Today)
                            select new ResignationEmployeeMasterView
                            {
                                EmployeeID = users.EmployeeId,
                                EmployeeName = users.EmployeeName,
                                FormattedEmployeeID = users.FormattedEmployeeId,
                                RelievingDate = users.RelievingDate,
                                ResignationDetailsId = users.EmployeeResignationDetailsId,
                                ResignationStatus = users.ResignationStatus,
                                ResignationChecklistId = chk.ResignationChecklistId,
                                ManagerStatus = chk.ManagerStatus,
                                ITStatus = chk.ITStatus,
                                AdminStatus = chk.AdminStatus,
                                FinanceStatus = chk.FinanceStatus,
                                HRStatus = chk.HRStatus,
                                ProfilePic = users.ProfilePicture,
                                RoleName=chk.RoleName
                            }).OrderByDescending(x => x.RelievingDate).Skip(resignationEmployeeFilter.NoOfRecord * (resignationEmployeeFilter.PageNumber))
                      .Take(resignationEmployeeFilter.NoOfRecord).Distinct().ToList();
                }
                  
                else
                    return (from users in dbContext.EmployeeResignationDetails
                            join approver in dbContext.ResignationApprovalStatus on users.EmployeeResignationDetailsId equals approver.EmployeeResignationDetailsId
                            where users.RelievingDate != null && resignationEmployeeFilter.ReporteesList.Contains(approver.ApproverEmployeeId == null ? 0 : (int)approver.ApproverEmployeeId)
                           // && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Pending" : resignationEmployeeFilter.ResignationStatus)
                            && resignListStatus.Contains(users.ResignationStatus)
                            //&& resignationEmployeeFilter.ReporteesList.Contains(users.ReportingManagerId == null ? 0 : (int)users.ReportingManagerId)
                            && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                            && true == (resignationEmployeeFilter.IsActive == true ? users.RelievingDate >= DateTime.Today : users.RelievingDate < DateTime.Today)
                            select new ResignationEmployeeMasterView
                            {
                                EmployeeID = users.EmployeeId,
                                EmployeeName = users.EmployeeName,
                                FormattedEmployeeID = users.FormattedEmployeeId,
                                RelievingDate = users.RelievingDate,
                                ResignationDetailsId = users.EmployeeResignationDetailsId,
                                ResignationStatus = users.ResignationStatus,
                                ProfilePic = users.ProfilePicture
                            }).OrderByDescending(x => x.RelievingDate).Skip(resignationEmployeeFilter.NoOfRecord * (resignationEmployeeFilter.PageNumber))
                        .Take(resignationEmployeeFilter.NoOfRecord).Distinct().ToList();
            }
            //(from users in dbContext.EmployeeResignationDetails
            // join chk in dbContext.ResignationChecklist on users.EmployeeResignationDetailsId equals chk.ResignationDetailsId
            // join approver in dbContext.ResignationApprovalStatus on users.EmployeeResignationDetailsId equals approver.EmployeeResignationDetailsId
            // where resignationEmployeeFilter.ReporteesList.Contains(approver.ApproverEmployeeId == null ? 0 : (int)approver.ApproverEmployeeId)
            // && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Approved" : resignationEmployeeFilter.ResignationStatus)
            // && users.ReportingManagerId == (resignationEmployeeFilter.EmployeeId == 0 ? (int)users.ReportingManagerId : resignationEmployeeFilter.EmployeeId)
            // && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
            // && chk.IsAgreeCheckList == true && true == ((users.RelievingDate >= ((resignationEmployeeFilter.IsActive == true) ? DateTime.Today.AddDays(1) : users.RelievingDate))
            // || (users.RelievingDate < ((resignationEmployeeFilter.IsActive == false) ? DateTime.Today : users.RelievingDate)))
            // select users).ToList();
            //into jnChkListDetails
            //    from chk in jnChkListDetails.DefaultIfEmpty()
        }
        public int GetResignationEmployeeListByFilterCount(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            List<string> chekListStatus = new List<string> { "", "" };
            List<string> resignListStatus = new List<string> { "Pending", "Withdrawal Pending" };
            List<ChecklistEmployeeView> resEmpCheckList = new List<ChecklistEmployeeView>();
            if (!string.IsNullOrEmpty(resignationEmployeeFilter?.ResignationStatus))
            {
                resignListStatus = new List<string> { resignationEmployeeFilter?.ResignationStatus };
            }
            if (resignationEmployeeFilter.IsAdmin == true)
            {

                if (resignationEmployeeFilter.IsCheckList)
                {
                    return (from users in dbContext.EmployeeResignationDetails
                            join chk in dbContext.ResignationChecklist on users.EmployeeResignationDetailsId equals chk.ResignationDetailsId
                            where users.RelievingDate != null && chk.IsAgreeCheckList == true
                            && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Approved" : resignationEmployeeFilter.ResignationStatus)
                            && users.EmployeeId != (resignationEmployeeFilter.EmployeeId)
                            && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                            && true == (resignationEmployeeFilter.IsActive == true ? users.RelievingDate >= DateTime.Today : users.RelievingDate < DateTime.Today)
                            select new ResignationEmployeeMasterView
                            {
                                EmployeeID = users.EmployeeId,
                                EmployeeName = users.EmployeeName,
                                FormattedEmployeeID = users.FormattedEmployeeId,
                                RelievingDate = users.RelievingDate,
                                ResignationDetailsId = users.EmployeeResignationDetailsId,
                                ResignationStatus = users.ResignationStatus,
                                ResignationChecklistId = chk.ResignationChecklistId,
                                ManagerStatus = chk.ManagerStatus,
                                ITStatus = chk.ITStatus,
                                AdminStatus = chk.AdminStatus,
                                FinanceStatus = chk.FinanceStatus,
                                HRStatus = chk.HRStatus,
                                ProfilePic = users.ProfilePicture,
                            }).Distinct().Count();
                }
                else
                    return (from users in dbContext.EmployeeResignationDetails
                            where users.RelievingDate != null
                            && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? users.ResignationStatus : resignationEmployeeFilter.ResignationStatus)
                            && users.EmployeeId != (resignationEmployeeFilter.EmployeeId)
                            && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                            && true == (resignationEmployeeFilter.IsActive == true ? users.RelievingDate >= DateTime.Today : users.RelievingDate < DateTime.Today)
                            select new ResignationEmployeeMasterView
                            {
                                EmployeeID = users.EmployeeId,
                                EmployeeName = users.EmployeeName,
                                FormattedEmployeeID = users.FormattedEmployeeId,
                                RelievingDate = users.RelievingDate,
                                ResignationDetailsId = users.EmployeeResignationDetailsId,
                                ResignationStatus = users.ResignationStatus,
                                ProfilePic = users.ProfilePicture
                            }).Distinct().Count();
            }
            else
            {
                if (resignationEmployeeFilter.IsCheckList)
                {
                    resEmpCheckList = GetReporteesCheckListDetails(resignationEmployeeFilter.ResignationInputFilter);
                    var resignationdetail = dbContext.EmployeeResignationDetails.ToArray();
                    return (from users in resignationdetail
                            join chk in resEmpCheckList on users.EmployeeResignationDetailsId equals chk.ResignationDetailsId
                            //join approver in dbContext.ResignationApprovalStatus on users.EmployeeResignationDetailsId equals approver.EmployeeResignationDetailsId
                            where users.RelievingDate != null && chk.IsAgreeCheckList == true
                            //&& resignationEmployeeFilter.ReporteesList.Contains(chk.EmployeeID == null ? 0 : (int)chk.EmployeeID)
                            && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Approved" : resignationEmployeeFilter.ResignationStatus)
                            //&& resignationEmployeeFilter.ReporteesList.Contains(users.ReportingManagerId == null ? 0 : (int)users.ReportingManagerId)
                            && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                            && true == (resignationEmployeeFilter.IsActive == true ? users.RelievingDate >= DateTime.Today : users.RelievingDate < DateTime.Today)
                            select new ResignationEmployeeMasterView
                            {
                                EmployeeID = users.EmployeeId,
                                EmployeeName = users.EmployeeName,
                                FormattedEmployeeID = users.FormattedEmployeeId,
                                RelievingDate = users.RelievingDate,
                                ResignationDetailsId = users.EmployeeResignationDetailsId,
                                ResignationStatus = users.ResignationStatus,
                                ResignationChecklistId = chk.ResignationChecklistId,
                                ManagerStatus = chk.ManagerStatus,
                                ITStatus = chk.ITStatus,
                                AdminStatus = chk.AdminStatus,
                                FinanceStatus = chk.FinanceStatus,
                                HRStatus = chk.HRStatus,
                                ProfilePic = users.ProfilePicture,
                                RoleName =chk.RoleName
                            }).Distinct().Count();
                }
               
                else
                    return (from users in dbContext.EmployeeResignationDetails
                            join approver in dbContext.ResignationApprovalStatus on users.EmployeeResignationDetailsId equals approver.EmployeeResignationDetailsId
                            where users.RelievingDate != null && resignationEmployeeFilter.ReporteesList.Contains(approver.ApproverEmployeeId == null ? 0 : (int)approver.ApproverEmployeeId)
                            // && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Pending" : resignationEmployeeFilter.ResignationStatus)
                            && resignListStatus.Contains(users.ResignationStatus)
                            //&& resignationEmployeeFilter.ReporteesList.Contains(users.ReportingManagerId == null ? 0 : (int)users.ReportingManagerId)
                            && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                            && true == (resignationEmployeeFilter.IsActive == true ? users.RelievingDate >= DateTime.Today : users.RelievingDate < DateTime.Today)
                            select new ResignationEmployeeMasterView
                            {
                                EmployeeID = users.EmployeeId,
                                EmployeeName = users.EmployeeName,
                                FormattedEmployeeID = users.FormattedEmployeeId,
                                RelievingDate = users.RelievingDate,
                                ResignationDetailsId = users.EmployeeResignationDetailsId,
                                ResignationStatus = users.ResignationStatus,
                                ProfilePic = users.ProfilePicture
                            }).Distinct().Count();
            }
            //(from users in dbContext.EmployeeResignationDetails
            // join chk in dbContext.ResignationChecklist on users.EmployeeResignationDetailsId equals chk.ResignationDetailsId
            // join approver in dbContext.ResignationApprovalStatus on users.EmployeeResignationDetailsId equals approver.EmployeeResignationDetailsId
            // where resignationEmployeeFilter.ReporteesList.Contains(approver.ApproverEmployeeId == null ? 0 : (int)approver.ApproverEmployeeId)
            // && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Approved" : resignationEmployeeFilter.ResignationStatus)
            // && users.ReportingManagerId == (resignationEmployeeFilter.EmployeeId == 0 ? (int)users.ReportingManagerId : resignationEmployeeFilter.EmployeeId)
            // && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
            // && chk.IsAgreeCheckList == true && true == ((users.RelievingDate >= ((resignationEmployeeFilter.IsActive == true) ? DateTime.Today.AddDays(1) : users.RelievingDate))
            // || (users.RelievingDate < ((resignationEmployeeFilter.IsActive == false) ? DateTime.Today : users.RelievingDate)))
            // select users).ToList();
            //into jnChkListDetails
            //    from chk in jnChkListDetails.DefaultIfEmpty()
        }


    }
}
