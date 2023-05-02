using Appraisal.DAL.Repository;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appraisal.DAL.Services
{
    public class WorkDayDetailServices
    {
        private readonly IWorkDayDetailRepository _workDayDetailRepository;
        private readonly IWorkDayRepository _workDayRepository;
        private readonly IWorkdayKRARepository _workdayKRARepository;
        private readonly IWorkdayObjectiveRepository _workdayObjectiveRepository;

        public WorkDayDetailServices(IWorkDayDetailRepository workDayDetailRepository, IWorkDayRepository workDayRepository,
            IWorkdayKRARepository workdayKRARepository, IWorkdayObjectiveRepository workdayObjectiveRepository)
        {
            _workDayDetailRepository = workDayDetailRepository;
            _workDayRepository = workDayRepository;
            _workdayKRARepository = workdayKRARepository;
            _workdayObjectiveRepository = workdayObjectiveRepository;
        }

        #region Save or Update WorkDay Detail
        /// <summary>
        /// Save or Update workDay Detail
        /// </summary>
        /// <param name="detailView"></param>
        /// <returns></returns>
        public async Task<WorkdayInputView> SaveOrUpdateWorkDayDetail(WorkdayInputView detailView)
        {
            int WorkDayDetailId = 0;
            string[] WorkHours = detailView.WorkHours?.Split(":");
            //if (detailView == null) return detailView;
            if (detailView.WorkDayDetailId > 0)
            {
                WorkDayDetail updateworkDay = _workDayDetailRepository.GetByID(detailView.WorkDayDetailId);
                if (updateworkDay != null)
                {
                    updateworkDay.ModifiedOn = DateTime.UtcNow;
                    updateworkDay.ModifiedBy = detailView.ModifiedBy;
                    updateworkDay.WorkHours = new TimeSpan(WorkHours.Length > 0 ? Convert.ToInt32(WorkHours[0]) : 0, WorkHours.Length > 1 ? Convert.ToInt32(WorkHours[1]) : 0, WorkHours.Length > 2 ? Convert.ToInt32(WorkHours[2]) : 0);
                    updateworkDay.EmployeeRemark = detailView.EmployeeRemark;
                    updateworkDay.ProjectId = detailView.ProjectId;
                    updateworkDay.ProjectName = detailView.ProjectName;
                    updateworkDay.WorkdayKRAId = detailView.KRAId ?? 0;
                    updateworkDay.WorkDayId = detailView.WorkDayId;
                    _workDayDetailRepository.Update(updateworkDay);
                    await _workDayDetailRepository.SaveChangesAsync();
                }
                WorkDayDetailId = updateworkDay.WorkDayDetailId;
            }
            else
            {
                if (detailView.WorkDayId == 0)
                {
                    WorkDay workDay = new()
                    {
                        WorkDate = detailView.WorkDate,
                        CreatedBy = detailView.CreatedBy,
                        CreatedOn = DateTime.UtcNow.Date,
                        EmployeeId = detailView.EmployeeId,
                        EmployeeName = detailView.EmployeeName
                    };
                    await _workDayRepository.AddAsync(workDay);
                    await _workDayRepository.SaveChangesAsync();
                    detailView.WorkDayId = workDay.WorkDayId;
                }
                if (detailView.ObjectiveId == 0)
                {
                    WorkdayObjective workDayObjective = new()
                    {
                        WorkDayId = detailView.WorkDayId,
                        ObjectiveName = detailView.ObjectiveName
                    };
                    await _workdayObjectiveRepository.AddAsync(workDayObjective);
                    await _workdayObjectiveRepository.SaveChangesAsync();
                    detailView.ObjectiveId = workDayObjective.WorkdayObjectiveId;
                }
                if (detailView.KRAId == 0)
                {
                    WorkdayKRA workDayKRA = new()
                    {
                        KRAName = detailView.KRAName,
                        WorkDayId = detailView.WorkDayId,
                        WorkdayObjectiveId = detailView.ObjectiveId ?? 0
                    };
                    await _workdayKRARepository.AddAsync(workDayKRA);
                    await _workdayKRARepository.SaveChangesAsync();
                    detailView.KRAId = workDayKRA.WorkdayKRAId;
                }
                WorkDayDetail addworkDay = new()
                {
                    ProjectId = detailView.ProjectId,
                    ProjectName = detailView.ProjectName,
                    CreatedBy = detailView.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    EmployeeRemark = detailView.EmployeeRemark,
                    WorkDayId = detailView.WorkDayId,
                    WorkdayKRAId = detailView.KRAId ?? 0,
                    WorkDate = detailView.WorkDate,
                    WorkHours = new TimeSpan(WorkHours.Length > 0 ? Convert.ToInt32(WorkHours[0]) : 0, WorkHours.Length > 1 ? Convert.ToInt32(WorkHours[1]) : 0, WorkHours.Length > 2 ? Convert.ToInt32(WorkHours[2]) : 0),
                    Status = string.IsNullOrEmpty(detailView.Status) ? "Pending" : detailView.Status
                };
                await _workDayDetailRepository.AddAsync(addworkDay);
                await _workDayDetailRepository.SaveChangesAsync();
                WorkDayDetailId = addworkDay.WorkDayDetailId;
                detailView.WorkDayDetailId = addworkDay.WorkDayDetailId;
            }
            return detailView;
        }
        #endregion

        #region Approve Or Reject Workday Detail View
        /// <summary>
        /// Approve Or Reject Workday Detail View
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ApproveOrRejectWorkDayDetail(ApproveOrRejectWorkdayDetailView approveOrRejectWorkdayDetail)
        {
            if (approveOrRejectWorkdayDetail == null) return false;
            List<WorkDayDetail> currentlstWorkDayDetail = new();
            if (approveOrRejectWorkdayDetail.WorkDayDetailId.Count() > 0)
            {
                currentlstWorkDayDetail = _workDayDetailRepository.GetAll().
                                            Where(x => approveOrRejectWorkdayDetail.WorkDayDetailId.Contains(x.WorkDayDetailId)).ToList();
            }
            else
            {
                currentlstWorkDayDetail = _workDayDetailRepository.GetAll().
                                            Where(x => x.WorkDayId == approveOrRejectWorkdayDetail.WorkDayId).ToList();
            }
            foreach (WorkDayDetail workDayDetail in currentlstWorkDayDetail)
            {
                workDayDetail.Status = approveOrRejectWorkdayDetail.Status;
                workDayDetail.ApproverId = approveOrRejectWorkdayDetail.ApproverId;
                workDayDetail.ApproverName = approveOrRejectWorkdayDetail.ApproverName;
                workDayDetail.ApproverRemark = approveOrRejectWorkdayDetail.ApproverRemark;
                workDayDetail.ApprovedDate = DateTime.UtcNow;
                _workDayDetailRepository.Update(workDayDetail);
            }
            await _workDayDetailRepository.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Approve Or Reject Workday List View
        /// <summary>
        /// Approve Or Reject Workday Detail View
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ApproveOrRejectWorkDayListView(ApproveOrRejectWorkdayListView approveOrRejectWorkdayDetail)
        {
            if (approveOrRejectWorkdayDetail == null) return false;
            List<WorkDayDetail> currentlstWorkDayDetail = new();
            currentlstWorkDayDetail = _workDayDetailRepository.GetAll().
                                            Where(x => approveOrRejectWorkdayDetail.WorkDayIds.Contains(x.WorkDayId)).ToList();
            foreach (WorkDayDetail workDayDetail in currentlstWorkDayDetail)
            {
                workDayDetail.Status = approveOrRejectWorkdayDetail.Status;
                workDayDetail.ApproverId = approveOrRejectWorkdayDetail.ApproverId;
                workDayDetail.ApproverName = approveOrRejectWorkdayDetail.ApproverName;
                workDayDetail.ApproverRemark = approveOrRejectWorkdayDetail.ApproverRemark;
                workDayDetail.ApprovedDate = DateTime.UtcNow;
                _workDayDetailRepository.Update(workDayDetail);
            }
            await _workDayDetailRepository.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Delete WorkDay Detail
        public async Task<bool> DeleteWorkDayDetail(int WorkDayDetailId)
        {
            WorkDayDetail workDayDetail = _workDayDetailRepository.Get(WorkDayDetailId);
            if (workDayDetail == null) return false;
            _workDayDetailRepository.Delete(workDayDetail);
            await _workDayDetailRepository.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get WorkDay Detail List
        public List<WorkdayListView> GetWorkDayDetailList(WorkdayFilterView workdayFilter)
        {
            return _workDayRepository.GetEmployeeWorkDayDetailByEmployeeId(workdayFilter);
        }
        #endregion

        #region Workday Detail Count
        public List<EmployeeRequestCount> WorkdayDetailCount(List<int> employeeIdLists)
        {
            return _workDayDetailRepository.WorkdayDetailCount(employeeIdLists);
        }
        #endregion
    }
}