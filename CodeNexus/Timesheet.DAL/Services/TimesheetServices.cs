using SharedLibraries.Common;
using SharedLibraries.Models.Timesheet;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.DAL.Repository;

namespace Timesheet.DAL.Services
{
    public class TimesheetServices
    {
        //private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly ITimesheetLogRepository _timesheetLogRepository;
        private readonly ITimesheetCommentsRepository _timesheetCommentsRepository;
        private readonly ITimesheetConfigurationRepository _timesheetConfigurationRepository;

        public TimesheetServices(ITimesheetRepository timesheetRepository, ITimesheetLogRepository timesheetLogRepository,
            ITimesheetCommentsRepository timesheetCommentsRepository, ITimesheetConfigurationRepository timesheetConfigurationRepository)
        {
            _timesheetRepository = timesheetRepository;
            _timesheetLogRepository = timesheetLogRepository;
            _timesheetCommentsRepository = timesheetCommentsRepository;
            _timesheetConfigurationRepository = timesheetConfigurationRepository;
        }

        #region Save or Update Timesheet
        /// <summary>
        /// Save or Update timesheet
        /// </summary>
        /// <param name="pTimesheet"></param>
        /// <returns></returns>
        public async Task<List<TimesheetLogView>> SaveOrUpdateTimesheetLog(List<TimesheetLogView> lstTimesheetLog, Guid? weekTimesheetId = null)
        {
            List<TimesheetLogView> lstTimesheetLogResponse = new List<TimesheetLogView>();
            try
            {
                if (lstTimesheetLog?.Count > 0)
                {
                    foreach (TimesheetLogView timesheet in lstTimesheetLog.OrderBy(x => x.PeriodSelection))
                    {
                        if (timesheet.TimesheetLogId > 0)
                        {
                            TimesheetLog updateTimesheet = _timesheetLogRepository.GetByID(timesheet.TimesheetLogId);
                            updateTimesheet.ModifiedOn = DateTime.UtcNow;
                            updateTimesheet.ModifiedBy = timesheet.ResourceId;
                            string[] clockedHours = timesheet.ClockedHours.Split(":");
                            updateTimesheet.ClockedHours = new TimeSpan(clockedHours?.Length > 0 ? Convert.ToInt32(clockedHours[0]) : 0, clockedHours.Length > 1 ? Convert.ToInt32(clockedHours[1]) : 0, clockedHours.Length > 2 ? Convert.ToInt32(clockedHours[2]) : 0);
                            string[] requiredHours = timesheet.RequiredHours.Split(":");
                            updateTimesheet.RequiredHours = new TimeSpan(requiredHours?.Length > 0 ? Convert.ToInt32(requiredHours[0]) : 0, requiredHours.Length > 1 ? Convert.ToInt32(requiredHours[1]) : 0, requiredHours.Length > 2 ? Convert.ToInt32(requiredHours[2]) : 0);
                            updateTimesheet.Comments = weekTimesheetId != null ? "" : timesheet.Comments;
                            updateTimesheet.ProjectId = timesheet.ProjectId;
                            updateTimesheet.WeekTimesheetId = weekTimesheetId;
                            updateTimesheet.WorkItem = timesheet.WorkItem;
                            _timesheetLogRepository.Update(updateTimesheet);
                            await _timesheetLogRepository.SaveChangesAsync();
                        }
                        else
                        {
                            TimesheetLog addTimesheetLog = new TimesheetLog();
                            string[] clockedHours = timesheet.ClockedHours.Split(":");
                            addTimesheetLog.ClockedHours = new TimeSpan(clockedHours?.Length > 0 ? Convert.ToInt32(clockedHours[0]) : 0, clockedHours.Length > 1 ? Convert.ToInt32(clockedHours[1]) : 0, clockedHours.Length > 2 ? Convert.ToInt32(clockedHours[2]) : 0);
                            string[] requiredHours = timesheet.RequiredHours.Split(":");
                            addTimesheetLog.RequiredHours = new TimeSpan(requiredHours?.Length > 0 ? Convert.ToInt32(requiredHours[0]) : 0, requiredHours.Length > 1 ? Convert.ToInt32(requiredHours[1]) : 0, requiredHours.Length > 2 ? Convert.ToInt32(requiredHours[2]) : 0);
                            addTimesheetLog.Comments = timesheet.Comments;
                            addTimesheetLog.PeriodSelection = timesheet.PeriodSelection;
                            addTimesheetLog.ProjectId = timesheet.ProjectId;
                            addTimesheetLog.ResourceId = timesheet.ResourceId;
                            addTimesheetLog.TimesheetId = timesheet.TimesheetId;
                            addTimesheetLog.ModifiedBy = timesheet.ModifiedBy;
                            addTimesheetLog.ModifiedOn = timesheet.ModifiedOn;
                            addTimesheetLog.CreatedOn = DateTime.UtcNow;
                            addTimesheetLog.CreatedBy = timesheet.ResourceId;
                            addTimesheetLog.IsSubmitted = false;
                            addTimesheetLog.IsRejected = false;
                            addTimesheetLog.IsApproved = false;
                            addTimesheetLog.WeekTimesheetId = weekTimesheetId;
                            addTimesheetLog.WorkItem = timesheet.WorkItem;
                            await _timesheetLogRepository.AddAsync(addTimesheetLog);
                            await _timesheetLogRepository.SaveChangesAsync();
                            timesheet.IsSubmitted = false;
                            timesheet.IsRejected = false;
                            timesheet.IsApproved = false;
                            timesheet.TimesheetLogId = addTimesheetLog.TimesheetLogId;
                            timesheet.WeekTimesheetId = weekTimesheetId;
                        }
                        lstTimesheetLogResponse.Add(timesheet);
                    }

                    return lstTimesheetLogResponse;
                }
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
            }
            return lstTimesheetLogResponse;
        }

        #endregion

        #region Delete TimesheetLog
        /// <summary>
        /// Delete TimesheetLog
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteTimesheetLog(List<int> timesheetLogId)
        {
            try
            {
                if (timesheetLogId?.Count > 0)
                {
                    foreach (var timesheetId in timesheetLogId)
                    {
                        TimesheetLog pTimesheetLog = _timesheetLogRepository.GetByID(timesheetId);
                        _timesheetLogRepository.Delete(pTimesheetLog);
                    }
                    await _timesheetLogRepository.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return false;
            }
        }
        #endregion 

        #region Submit Timesheet
        /// <summary>
        /// Submit Timesheet
        /// </summary>
        /// <param name="submitTimesheet"></param>
        /// <returns></returns>
        public async Task<bool> SubmitTimesheet(SubmitTimesheet submitTimesheet)
        {
            try
            {
                Guid weekTimesheetId = Guid.NewGuid();
                bool duplicateComments = false;
                await SaveOrUpdateTimesheetLog(submitTimesheet.TimesheetLog, weekTimesheetId);
                var grpTimesheet = submitTimesheet.TimesheetLog.GroupBy(x => new { x.ProjectId, x.ResourceId })
                .OrderBy(x => x.Key.ProjectId)
                .Select(x => new
                {
                    x.Key.ResourceId,
                    x.Key.ProjectId,
                    TotalClockedHours = new TimeSpan(x.Sum(y => y == null ? 0 : new TimeSpan(y.ClockedHours.Split(":").Length > 0 ? Convert.ToInt32(y.ClockedHours.Split(":")[0]) : 0, y.ClockedHours.Split(":").Length > 1 ? Convert.ToInt32(y.ClockedHours.Split(":")[1]) : 0, y.ClockedHours.Split(":").Length > 2 ? Convert.ToInt32(y.ClockedHours.Split(":")[2]) : 0).Ticks))
                }
                ).ToList();
                foreach (var item in grpTimesheet)
                {
                    SharedLibraries.Models.Timesheet.Timesheet timesheet = new SharedLibraries.Models.Timesheet.Timesheet()
                    {
                        ReportingPersonId = submitTimesheet.ListOfProjectSPOC.Where(x => x.projectId == item.ProjectId).Select(x => x.SPOCId == null ? 0 : (int)x.SPOCId).FirstOrDefault(), //(int)_timesheetRepository.GetProjectSPOCByProjectId((int)item.ProjectId),
                        TotalClockedHours = string.Format("{0}:{1}", (item.TotalClockedHours.Days * 24 + item.TotalClockedHours.Hours), item.TotalClockedHours.Minutes),
                        TotalRequiredHours = submitTimesheet.TotalRequiredHours,
                        WeekTimesheetId = weekTimesheetId,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = item.ResourceId
                    };
                    await _timesheetRepository.AddAsync(timesheet);
                    await _timesheetRepository.SaveChangesAsync();
                    List<int> lstTimesheetLog = submitTimesheet.TimesheetLog.Where(y => (y.ProjectId == item.ProjectId) && (y.ResourceId == item.ResourceId)).Select(x => x.TimesheetLogId).ToList();
                    _timesheetLogRepository.UpdateTimesheetId(timesheet.TimesheetId, lstTimesheetLog);
                    await _timesheetLogRepository.SaveChangesAsync();
                    if (submitTimesheet.Comments != "" && submitTimesheet.Comments != null && duplicateComments == false)
                    {
                        TimesheetComments timesheetComments = new TimesheetComments()
                        {
                            TimesheetId = timesheet.TimesheetId,
                            Comments = submitTimesheet.Comments,
                            WeekTimesheetId = weekTimesheetId,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = item.ResourceId
                        };
                        await _timesheetCommentsRepository.AddAsync(timesheetComments);
                        await _timesheetCommentsRepository.SaveChangesAsync();
                        duplicateComments = true;
                    }
                }
                if (submitTimesheet?.TimesheetLog?.Count > 0)
                {
                    DeleteSavedTimesheetLogs(submitTimesheet?.TimesheetLog[0]);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
            }
            //return false;
        }
        #endregion 

        #region Get timesheet by timesheetId
        /// <summary>
        /// Get timesheet by teimesheetId
        /// </summary>
        /// <returns></returns>
        public List<TimesheetLogView> GetTimesheetByTimesheetId(int timesheetId)
        {
            try
            {
                return _timesheetLogRepository.GetTimesheetByTimesheetId(timesheetId);
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return new List<TimesheetLogView>();
            }
        }
        #endregion 

        #region Approve timesheet by reporting manager
        /// <summary>
        /// Approve timesheet by reporting manager
        /// </summary>
        /// <returns></returns>
        public async Task<string> ApproveOrRejectTimesheet(List<TimesheetStatusView> lstTimesheets)
        {
            try
            {
                string status = "approved";
                Guid? weekTimesheetId = null;
                if (lstTimesheets != null && lstTimesheets.Count > 0)
                {
                    foreach (TimesheetStatusView lstTimesheet in lstTimesheets)
                    {
                        bool isApprove = true;
                        int resourceId = 0;//, reportingPersonId = 0;
                        List<TimesheetLogView> lstTimesheetLog = _timesheetLogRepository.GetTimesheetByTimesheetId(lstTimesheet.TimesheetId);
                        foreach (TimesheetLogView timesheetLog in lstTimesheetLog)
                        {
                            TimesheetLog pTimesheetLog = _timesheetLogRepository.GetByTimesheetLogId(timesheetLog.TimesheetLogId);
                            if (pTimesheetLog != null)
                            {
                                if (lstTimesheet.IsApproved == true)
                                {
                                    pTimesheetLog.IsApproved = true;
                                    timesheetLog.IsApproved = true;
                                }
                                else
                                {
                                    pTimesheetLog.IsRejected = true;
                                    timesheetLog.IsRejected = true;
                                    isApprove = false;
                                }
                            }
                            resourceId = (int)timesheetLog.ResourceId;
                            weekTimesheetId = timesheetLog.WeekTimesheetId;
                            _timesheetLogRepository.Update(pTimesheetLog);
                            await _timesheetLogRepository.SaveChangesAsync();
                        }
                        var totalApprovedHours = lstTimesheetLog.Where(x => x.IsApproved == true).Select(x => x).ToList<TimesheetLogView>().GroupBy(x => new { x.TimesheetId })
                            .OrderBy(x => x.Key.TimesheetId)
                            .Select(x => new
                            {
                                x.Key.TimesheetId,
                                TotalClockedHours = new TimeSpan(x.Sum(r => r == null ? 0 : new TimeSpan(r.ClockedHours.Split(":").Length > 0 ? Convert.ToInt32(r.ClockedHours.Split(":")[0]) : 0,
                                r.ClockedHours.Split(":").Length > 1 ? Convert.ToInt32(r.ClockedHours.Split(":")[1]) : 0, 0).Ticks))
                            }
                            ).ToList();
                        foreach (var item in totalApprovedHours)
                        {
                            if (item.TimesheetId != null)
                            {
                                SharedLibraries.Models.Timesheet.Timesheet pTimesheet = _timesheetRepository.GetByID((int)item.TimesheetId);
                                if (pTimesheet != null)
                                {
                                    pTimesheet.TotalApprovedHours = string.Format("{0}:{1}", (item.TotalClockedHours.Days * 24 + item.TotalClockedHours.Hours), item.TotalClockedHours.Minutes);
                                    pTimesheet.IsBillable = lstTimesheet.IsBillable;
                                    //reportingPersonId = pTimesheet.ReportingPersonId;
                                    _timesheetRepository.Update(pTimesheet);
                                    await _timesheetRepository.SaveChangesAsync();
                                }
                            }
                        }
                        var totalRejectedHours = lstTimesheetLog.Where(x => x.IsRejected == true).Select(x => x).ToList<TimesheetLogView>().GroupBy(x => new { x.TimesheetId })
                            .OrderBy(x => x.Key.TimesheetId)
                            .Select(x => new
                            {
                                x.Key.TimesheetId
                            }
                            ).ToList();
                        foreach (var item in totalRejectedHours)
                        {
                            if (item.TimesheetId != null)
                            {
                                SharedLibraries.Models.Timesheet.Timesheet pTimesheet = _timesheetRepository.GetByID((int)item.TimesheetId);
                                if (pTimesheet != null)
                                {
                                    pTimesheet.TotalApprovedHours = null;
                                    pTimesheet.IsBillable = lstTimesheet.IsBillable;
                                    pTimesheet.OtherReasonForRejection = lstTimesheet.Reason;
                                    pTimesheet.RejectionReasonId = lstTimesheet.RejectionReasonId;
                                    //reportingPersonId = pTimesheet.ReportingPersonId;
                                    _timesheetRepository.Update(pTimesheet);
                                    await _timesheetRepository.SaveChangesAsync();
                                }
                            }
                        }
                       
                        if (lstTimesheet.Comments != null && lstTimesheet.Comments != "")
                        {
                            TimesheetComments timesheetComments = new TimesheetComments
                            {
                                CreatedBy = lstTimesheet.ResourceId,
                                CreatedOn = DateTime.UtcNow,
                                TimesheetId = lstTimesheet.TimesheetId,
                                Comments = lstTimesheet.Comments,
                                WeekTimesheetId = weekTimesheetId
                            };
                            await _timesheetCommentsRepository.AddAsync(timesheetComments);
                            await _timesheetCommentsRepository.SaveChangesAsync();
                        }
                        if (!isApprove)
                        {
                            status = "rejected";
                        }
                    }
                }
                return status;
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return "";
            }
        }
        #endregion 

        #region Get resource timesheet by week
        /// <summary>
        /// Get timesheet by teimesheetId
        /// </summary>
        /// <returns></returns>
        public ResourceWeeklyTimesheet GetResourceTimesheetByWeek(ProjectTimesheet projectTimesheet)
        {
            try
            {
                ResourceWeeklyTimesheet resourceWeeklyTimesheet = new ResourceWeeklyTimesheet();
                resourceWeeklyTimesheet = _timesheetLogRepository.GetResourceTimesheetByWeek(projectTimesheet);
                int currentDay = (int)DateTime.Now.DayOfWeek;
                TimesheetConfigurationView timesheetConfigurationVeiw = new TimesheetConfigurationView();
                timesheetConfigurationVeiw = _timesheetConfigurationRepository.GetConfigurationDetails();
                bool isEnableApprove = false;
                bool isEnableSubmit = false;
                DateTime previousWeekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek).AddDays(-7);
                //Is approve button enable
                if (resourceWeeklyTimesheet?.Status != "Yet to submit")
                {
                    if (timesheetConfigurationVeiw !=null && timesheetConfigurationVeiw?.TimesheetApprovalFromDayId>0 && timesheetConfigurationVeiw?.TimesheetApprovalToDayId > 0)
                    {

                        if (previousWeekStartDate.Date == projectTimesheet.WeekStartDate?.Date)
                        {
                            if (timesheetConfigurationVeiw?.TimesheetApprovalFromDayId == 7)
                            {
                                timesheetConfigurationVeiw.TimesheetApprovalFromDayId = 0;
                            }
                            if (timesheetConfigurationVeiw?.TimesheetApprovalToDayId == 7)
                            {
                                timesheetConfigurationVeiw.TimesheetApprovalToDayId = 0;
                            }
                            if (timesheetConfigurationVeiw?.TimesheetApprovalFromDayId <= currentDay && timesheetConfigurationVeiw?.TimesheetApprovalToDayId >= currentDay)
                            {
                                isEnableApprove = true;
                            }
                        }
                    }
                    else
                    {
                        isEnableApprove = true;
                    }
                }
                if (resourceWeeklyTimesheet?.Status == "Yet to submit")
                {
                    
                    if (timesheetConfigurationVeiw != null && timesheetConfigurationVeiw?.TimesheetSubmissionDayId > 0 && !string.IsNullOrEmpty(timesheetConfigurationVeiw?.TimesheetSubmissionTime) )
                    {
                        resourceWeeklyTimesheet.CurrentDayId = currentDay;
                        if (previousWeekStartDate.Date == projectTimesheet.WeekStartDate?.Date)
                        {
                            if (timesheetConfigurationVeiw?.TimesheetSubmissionDayId == 7)
                            {
                                timesheetConfigurationVeiw.TimesheetSubmissionDayId = 0;
                            }
                            resourceWeeklyTimesheet.TimesheetSubmissionDayId = timesheetConfigurationVeiw?.TimesheetSubmissionDayId;
                            if (timesheetConfigurationVeiw?.TimesheetSubmissionDayId >= currentDay )
                            {
                                isEnableSubmit = true;
                            }
                        }
                    }
                    else
                    {
                        isEnableSubmit = true;
                    }
                }
                resourceWeeklyTimesheet.IsEnableApprove = isEnableApprove;
                resourceWeeklyTimesheet.IsEnableSubmit = isEnableSubmit;
                resourceWeeklyTimesheet.TimesheetSubmissionTime = string.IsNullOrEmpty(timesheetConfigurationVeiw?.TimesheetSubmissionTime) == false ? timesheetConfigurationVeiw?.TimesheetSubmissionTime : "00:00:00";
                if (resourceWeeklyTimesheet?.Status == "Yet to submit")
                {
                    DateTime previouWeekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek).AddDays(-7);
                    string[] savedTime = timesheetConfigurationVeiw?.TimesheetSubmissionTime?.ToString().Split(':');
                    DateTime timeheetSubmissionDay = new DateTime();
                    if (savedTime?.Count() > 0)
                    {
                        timeheetSubmissionDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(savedTime[0]), savedTime?.Count() > 1 ? Convert.ToInt32(savedTime[1]) : 0, 0);
                    }
                    if (projectTimesheet.WeekStartDate.Value.Date < previouWeekStartDate.Date)
                    {
                        resourceWeeklyTimesheet.Status = "Timesheet Due";
                    }
                    else if (projectTimesheet.WeekStartDate.Value.Date == previouWeekStartDate.Date)
                    {
                        if ((int)DateTime.Now.DayOfWeek > Convert.ToInt32(timesheetConfigurationVeiw?.TimesheetSubmissionDayId))
                        {
                            resourceWeeklyTimesheet.Status = "Timesheet Due";
                        }
                        else if ((int)DateTime.Now.DayOfWeek == Convert.ToInt32(timesheetConfigurationVeiw?.TimesheetSubmissionDayId))
                        {
                            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, timeheetSubmissionDay.TimeOfDay) == 0 || TimeSpan.Compare(DateTime.Now.TimeOfDay, timeheetSubmissionDay.TimeOfDay) == 1)
                            {
                                resourceWeeklyTimesheet.Status = "Timesheet Due";
                            }
                        }
                    }
                }
                return resourceWeeklyTimesheet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 

        #region Get First day of week
        public static DateTime FirstDayOfWeek(DateTime date)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var dateDiff = date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (dateDiff < 0)
                dateDiff += 7;
            return date.AddDays(-dateDiff).Date;
        }
        #endregion 

        #region Add or Update comments
        /// <summary>
        /// Add or Update comments
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateComments(TimesheetComments timesheetComments)
        {
            try
            {
                if (timesheetComments.TimesheetCommentsId > 0)
                {
                    timesheetComments.ModifiedOn = DateTime.UtcNow;
                    _timesheetCommentsRepository.Update(timesheetComments);
                }
                else
                {
                    timesheetComments.CreatedOn = DateTime.UtcNow;
                    await _timesheetCommentsRepository.AddAsync(timesheetComments);
                }
                await _timesheetCommentsRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return false;
            }
        }
        #endregion

        #region Delete comments
        /// <summary>
        /// Delete comments
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteComments(int timesheetCommentsId)
        {
            try
            {
                TimesheetComments pTimesheetComments = _timesheetCommentsRepository.GetByID(timesheetCommentsId);
                _timesheetCommentsRepository.Delete(pTimesheetComments);
                await _timesheetCommentsRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return false;
            }
        }
        #endregion 

        #region Delete Timesheet Logs
        /// <summary>
        /// Delete Timesheet Logs
        /// </summary>
        /// <param name="lstTimesheetLogIds"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTimesheetLogs(List<int> lstTimesheetLogIds)
        {
            try
            {
                foreach (int timesheetLogId in lstTimesheetLogIds)
                {
                    TimesheetLog pTimesheetLog = _timesheetLogRepository.GetByID(timesheetLogId);
                    if (pTimesheetLog != null)
                        _timesheetLogRepository.Delete(pTimesheetLog);
                }
                await _timesheetLogRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return false;
            }
        }
        #endregion 

        #region Get Resource Team Timesheet
        public List<TeamTimesheet> GetResourceTeamTimesheet(TeamMember teamMember)
        {
            //try
            //{
                return _timesheetLogRepository.GetResourceTeamTimesheet(teamMember);
            //}
            //catch (Exception)
            //{
            //    _logger.Error(ex.Message.ToString());
            //    return new List<TeamTimesheet>();
            //}
        }
        #endregion

        #region Get Rejection Reason List
        public List<RejectionReason> GetRejectionReasonList()
        {
            //try
            //{
                return _timesheetRepository.GetRejectionReasonList();
            //}
            //catch (Exception)
            //{
            //    _logger.Error(ex.Message.ToString());
            //    return new List<RejectionReason>();
            //}
        }
        #endregion

        #region Get timesheet log by project id
        /// <summary>
        /// Get timesheet log by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<TimesheetLogView> GetTimesheetLogByProjectId(List<int?> lstProjectId)
        {
            try
            {
                return _timesheetLogRepository.GetTimesheetLogByProjectId(lstProjectId);
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return new List<TimesheetLogView>();
            }
        }           
        #endregion

        #region Get Resource Id By Timesheet Id
        /// <summary>
        /// Get Resource Id By Timesheet Id
        /// </summary>
        /// <param name="pTimesheetId"></param>
        /// <returns></returns>
        public int? GetReportingPersonIdByTimesheetId(int? pTimesheetId)
        {
            try
            {
                return _timesheetLogRepository.GetResourceIdByTimesheetId(pTimesheetId);
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return 0;
            }
        }
        #endregion

        #region Insert Or Update Timesheet Configuration
        /// <summary>
        /// Insert Or Update Timesheet Configuration
        /// </summary>
        /// <param name="timesheetConfigurationView"></param>
        /// <returns></returns>
        public async Task<int> InsertOrUpdateTimesheetConfiguration(TimesheetConfigurationView timesheetConfigurationView)
        {
            try
            {
                int TimesheetConfigurationId = 0;
                TimesheetConfigurationDetails timesheetConfigurationDetails = new TimesheetConfigurationDetails();
                if (timesheetConfigurationView.TimesheetConfigurationId != 0) timesheetConfigurationDetails = _timesheetConfigurationRepository.GetByConfigurationID(timesheetConfigurationView.TimesheetConfigurationId);
                if (timesheetConfigurationDetails != null)
                {
                    timesheetConfigurationDetails.TimesheetSubmissionDayId = timesheetConfigurationView.TimesheetSubmissionDayId;
                    string[] TimesheetSubmissionTime = timesheetConfigurationView.TimesheetSubmissionTime?.Split(":");
                    timesheetConfigurationDetails.TimesheetSubmissionTime = new TimeSpan(TimesheetSubmissionTime?.Length > 0 ? Convert.ToInt32(TimesheetSubmissionTime?[0]) : 0, TimesheetSubmissionTime?.Length > 1 ? Convert.ToInt32(TimesheetSubmissionTime?[1]) : 0, TimesheetSubmissionTime?.Length > 2 ? Convert.ToInt32(TimesheetSubmissionTime?[2]) : 0);
                    timesheetConfigurationDetails.TimesheetAlertSubmissionFromDayId = timesheetConfigurationView.TimesheetAlertSubmissionFromDayId;
                    timesheetConfigurationDetails.TimesheetAlertSubmissionToDayId = timesheetConfigurationView.TimesheetAlertSubmissionToDayId;
                    timesheetConfigurationDetails.TimesheetApprovalFromDayId = timesheetConfigurationView.TimesheetApprovalFromDayId;
                    timesheetConfigurationDetails.TimesheetApprovalToDayId = timesheetConfigurationView.TimesheetApprovalToDayId;
                    timesheetConfigurationDetails.TimesheetAlertApprovalFromDayId = timesheetConfigurationView.TimesheetAlertApprovalFromDayId;
                    timesheetConfigurationDetails.TimesheetAlertApprovalToDayId = timesheetConfigurationView.TimesheetAlertApprovalToDayId;
                    if (timesheetConfigurationView.TimesheetConfigurationId == 0)
                    {
                        timesheetConfigurationDetails.CreatedOn = DateTime.UtcNow;
                        timesheetConfigurationDetails.CreatedBy = timesheetConfigurationView.CreatedBy;
                        await _timesheetConfigurationRepository.AddAsync(timesheetConfigurationDetails);
                        await _timesheetConfigurationRepository.SaveChangesAsync();
                        TimesheetConfigurationId = timesheetConfigurationDetails.TimesheetConfigurationId;
                    }
                    else
                    {
                        timesheetConfigurationDetails.ModifiedOn = DateTime.UtcNow;
                        timesheetConfigurationDetails.ModifiedBy = timesheetConfigurationView.ModifiedBy;
                        _timesheetConfigurationRepository.Update(timesheetConfigurationDetails);
                        TimesheetConfigurationId = timesheetConfigurationDetails.TimesheetConfigurationId;
                        await _timesheetConfigurationRepository.SaveChangesAsync();
                    }
                }
                return TimesheetConfigurationId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Configuration Details
        /// <summary>
        /// Get Configuration Details
        /// </summary>
        /// <returns></returns>
        public TimesheetConfigurationView GetConfigurationDetails()
        {
            try
            {
                return _timesheetConfigurationRepository.GetConfigurationDetails();
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return new TimesheetConfigurationView();
            }
        }
        #endregion

        #region Get Timesheet Alert For Submission
        /// <summary>
        /// Get Timesheet Alert For Submission
        /// </summary>
        /// <returns></returns>
        public Tuple<List<int>, DateTime, bool> GetTimesheetAlertForSubmission()
        {
            bool notification = false;
            try
            {
                TimesheetConfigurationView timesheetConfigurationVeiw = _timesheetConfigurationRepository.GetConfigurationDetails();
                DateTime weekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek).AddDays(-7);
                if (timesheetConfigurationVeiw != null && timesheetConfigurationVeiw.TimesheetAlertSubmissionFromDayId >0 && timesheetConfigurationVeiw.TimesheetAlertSubmissionToDayId>0)
                {
                    int currentDay = (int)DateTime.Now.DayOfWeek;
                    if (timesheetConfigurationVeiw.TimesheetAlertSubmissionFromDayId == 6)
                    {
                        weekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek);
                    }
                    for (int i = 0; i <= 14; i++)
                    {
                        if (timesheetConfigurationVeiw.TimesheetAlertSubmissionFromDayId == 7)
                        {
                            timesheetConfigurationVeiw.TimesheetAlertSubmissionFromDayId = 0;
                        }
                        if (timesheetConfigurationVeiw.TimesheetAlertSubmissionToDayId == 7)
                        {
                            timesheetConfigurationVeiw.TimesheetAlertSubmissionToDayId = 0;
                        }
                        if (timesheetConfigurationVeiw.TimesheetAlertSubmissionFromDayId == currentDay)
                        {
                            notification = true;
                        }
                        if (timesheetConfigurationVeiw.TimesheetAlertSubmissionFromDayId == timesheetConfigurationVeiw.TimesheetAlertSubmissionToDayId)
                        {
                            break;
                        }
                        timesheetConfigurationVeiw.TimesheetAlertSubmissionFromDayId += 1;
                    }
                    if (notification)
                        return Tuple.Create(_timesheetLogRepository.GetTimesheetAlertForSubmission(weekStartDate), weekStartDate, notification);
                }
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
            }
            return new Tuple<List<int>, DateTime,bool>(new List<int>(), DateTime.Now, notification);
        }
        #endregion

        #region Get Timesheet Alert For Approval
        /// <summary>
        /// Get Timesheet Alert For Approval
        /// </summary>
        /// <returns></returns>
        public Tuple<List<TimesheetAlertApproval>, DateTime,bool> GetTimesheetAlertForApproval()
        {
            bool notification = false;
            try
            {
               
                TimesheetConfigurationView timesheetConfigurationVeiw = _timesheetConfigurationRepository.GetConfigurationDetails();
                DateTime weekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek).AddDays(-7);
                
                if (timesheetConfigurationVeiw != null && timesheetConfigurationVeiw.TimesheetAlertApprovalFromDayId>0 && timesheetConfigurationVeiw.TimesheetAlertApprovalToDayId>0)
                {
                    int currentDay = (int)DateTime.Now.DayOfWeek;
                    if (timesheetConfigurationVeiw.TimesheetAlertApprovalFromDayId == 6)
                    {
                        weekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek);
                    }
                    for (int i = 0; i <= 14; i++)
                    {
                        if (timesheetConfigurationVeiw.TimesheetAlertApprovalFromDayId == 7)
                        {
                            timesheetConfigurationVeiw.TimesheetAlertApprovalFromDayId = 0;
                        }
                        if (timesheetConfigurationVeiw.TimesheetAlertApprovalToDayId == 7)
                        {
                            timesheetConfigurationVeiw.TimesheetAlertApprovalToDayId = 0;
                        }
                        if (timesheetConfigurationVeiw.TimesheetAlertApprovalFromDayId == currentDay)
                        {
                            notification = true;
                        }
                        if (timesheetConfigurationVeiw.TimesheetAlertApprovalFromDayId == timesheetConfigurationVeiw.TimesheetAlertApprovalToDayId)
                        {
                            break;
                        }
                        timesheetConfigurationVeiw.TimesheetAlertApprovalFromDayId += 1;
                    }
                    if (notification)
                        return Tuple.Create(_timesheetLogRepository.GetTimesheetAlertForApproval(weekStartDate), weekStartDate, notification);
                }
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
            }
            return new Tuple<List<TimesheetAlertApproval>, DateTime, bool>(new List<TimesheetAlertApproval>(), DateTime.Now, notification);
        }
        #endregion

        #region Get All Timesheet
        /// <summary>
        /// Get All Timesheet
        /// </summary>
        /// <returns></returns>
        public List<SharedLibraries.Models.Timesheet.Timesheet> GetAllTimesheet()
        {
            try
            {
                return _timesheetRepository.GetAllTimesheet();
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return new List<SharedLibraries.Models.Timesheet.Timesheet>();
            }
        }
        #endregion

            #region Get All Timesheet Log
            /// <summary>
            /// Get All Timesheet Log
            /// </summary>
            /// <returns></returns>
            public List<TimesheetLog> GetAllTimesheetLog()
        {
            try
            {
                return _timesheetLogRepository.GetAllTimesheetLog();
            }
            catch (Exception)
            {
                throw;
                //_logger.Error(ex.Message.ToString());
                //return new List<TimesheetLog>();
            }
        }
        #endregion

        #region Get timesheet home report
        /// <summary>
        /// Get timesheet home report
        /// </summary>
        /// <returns></returns>
        public HomeReportData GetTimesheetHomeReport(ProjectTimesheet projectTimesheet)
        {
            try
            {
                HomeReportData homeReportData = new HomeReportData();
                homeReportData.ReportData = _timesheetLogRepository.GetTimesheetHomeReport(projectTimesheet);
                int currentDay = (int)DateTime.Now.DayOfWeek;
                TimesheetConfigurationView timesheetConfigurationVeiw = new TimesheetConfigurationView();
                DateTime previouWeekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek).AddDays(-7);
                homeReportData.ReportSubTitle = string.Concat(previouWeekStartDate.Date.ToString("dd MMM yy"), " - ", previouWeekStartDate.Date.AddDays(6).ToString("dd MMM yy"));
                homeReportData.ReportTitle = "Previous Week";
                if (homeReportData.ReportData == "Yet to submit")
                {

                    string[] savedTime = timesheetConfigurationVeiw?.TimesheetSubmissionTime?.ToString().Split(':');
                    DateTime timeheetSubmissionDay = new DateTime();
                    if (savedTime?.Count() > 0)
                    {
                        timeheetSubmissionDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(savedTime[0]), savedTime?.Count() > 1 ? Convert.ToInt32(savedTime[1]) : 0, 0);
                    }
                    if (projectTimesheet.WeekStartDate.Value.Date < previouWeekStartDate.Date)
                    {
                        homeReportData.ReportData = "Timesheet Due";
                    }
                    else if (projectTimesheet.WeekStartDate.Value.Date == previouWeekStartDate.Date)
                    {
                        if ((int)DateTime.Now.DayOfWeek > Convert.ToInt32(timesheetConfigurationVeiw?.TimesheetSubmissionDayId))
                        {
                            homeReportData.ReportData = "Timesheet Due";
                        }
                        else if ((int)DateTime.Now.DayOfWeek == Convert.ToInt32(timesheetConfigurationVeiw?.TimesheetSubmissionDayId))
                        {
                            if (TimeSpan.Compare(DateTime.Now.TimeOfDay, timeheetSubmissionDay.TimeOfDay) == 0 || TimeSpan.Compare(DateTime.Now.TimeOfDay, timeheetSubmissionDay.TimeOfDay) == 1)
                            {
                                homeReportData.ReportData = "Timesheet Due";
                            }
                        }
                    }
                }
                return homeReportData;
            }
            catch (Exception)
            {
                //_logger.Error(ex.Message.ToString());
                throw;
            }
        }
        #endregion

        #region Get Timesheet Utilization Details
        /// <summary>
        /// Get Timesheet Utilization Details
        /// </summary>
        /// <param name="pResourceId"></param>
        /// <returns></returns>
        public List<TimesheetUtilizationView> GetTimesheetUtilizationDetails(int pResourceId)
        {
            //try
            //{
                return _timesheetLogRepository.GetTimesheetUtilizationDetails(pResourceId);
            //}
            //catch (Exception)
            //{
            //    _logger.Error(ex.Message.ToString());
            //    return new List<TimesheetUtilizationView>();
            //}
        }
        #endregion

        #region Get Employee Timesheet Utilization Details
        /// <summary>
        /// Get Employee Timesheet Utilization Details
        /// </summary>
        /// <param name="pResourceId"></param>
        /// <returns></returns>
        public List<EmployeeTimeUtilizationView> GetEmployeeTimesheetUtilizationDetails(int pResourceId)
        {
            //try
            //{
                return _timesheetLogRepository.GetEmployeeTimesheetUtilizationDetails(pResourceId);
            //}
            //catch (Exception)
            //{
            //    _logger.Error(ex.Message.ToString());
            //    return new List<EmployeeTimeUtilizationView>();
            //}
        }
        #endregion

        #region Get Employee Timesheet By Employee Id
        /// <summary>
        /// Get Employee Timesheet By Employee Id
        /// </summary>
        /// <param name="pResourceId"></param>
        /// <returns></returns>
        public List<EmployeeTimesheetHourView> GetEmployeeTimesheetByEmployeeId(int pResourceId)
        {
            //try
            //{
                return _timesheetLogRepository.GetEmployeeTimesheetByEmployeeId(pResourceId);
            //}
            //catch (Exception)
            //{
            //    _logger.Error(ex.Message.ToString());
            //    return new List<EmployeeTimesheetHourView>();
            //}
        }
        #endregion

        #region Get Timesheet Grid Detail By Employee Id
        /// <summary>
        /// Get Timesheet Grid Detail By Employee Id
        /// </summary>
        /// <param name="pResourceId"></param>
        /// <returns></returns>
        public List<EmployeeTimesheetGridView> GetTimesheetGridDetailByEmployeeId(int pResourceId)
        {
            //try
            //{
                return _timesheetLogRepository.GetTimesheetGridDetailByEmployeeId(pResourceId);
            //}
            //catch (Exception)
            //{
            //    _logger.Error(ex.Message.ToString());
            //    return new List<EmployeeTimesheetGridView>();
            //}
        }
        #endregion
        #region Get timesheet by timesheet id
        /// <summary>
        /// </summary>
        /// <param name="timesheetIdid"></param>
        /// <returns></returns>
        public List<TimesheetLogView> GetTimesheetLogByTimesheetId(List<int> timesheetId)
        {
            return _timesheetLogRepository.GetTimesheetLogByTimesheetId(timesheetId);
        }
        #endregion

        #region Delete Timesheet Logs
        /// <summary>
        /// Delete Timesheet Logs
        /// </summary>
        /// <param name="lstTimesheetLogIds"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSavedTimesheetLogs(TimesheetLogView timesheetLog)
        {
            try
            {
                TimesheetLogView? timeLog = timesheetLog;
                if (timeLog != null && timeLog.ResourceId != 0)
                {
                    DateTime weekstartDate = timeLog.PeriodSelection.AddDays((double)(DayOfWeek.Sunday - timeLog.PeriodSelection.DayOfWeek));
                    List<TimesheetLog> timesheetsLogList = _timesheetLogRepository.GetTimesheetLogsByWeek(weekstartDate, timeLog.ResourceId == null ? 0 : (int)timeLog.ResourceId);
                    foreach (TimesheetLog item in timesheetsLogList)
                    {
                        _timesheetLogRepository.Delete(item);
                        await _timesheetLogRepository.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/DeleteSavedTimesheetLogs");
                return false;
            }
        }
        #endregion 
    }
}