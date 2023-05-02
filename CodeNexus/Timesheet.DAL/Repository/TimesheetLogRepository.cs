using SharedLibraries.Models.Timesheet;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using Timesheet.DAL.DBContext;
using Timesheet.DAL.Models;
using SharedLibraries.Models.Projects;

namespace Timesheet.DAL.Repository
{
    public interface ITimesheetLogRepository : IBaseRepository<TimesheetLog>
    {
        TimesheetLog GetByID(int pTimesheetLogId);
        List<TimesheetLogView> GetTimesheetByTimesheetId(int timesheetId);
        TimesheetLog GetByTimesheetLogId(int timesheetLogId);
        ResourceWeeklyTimesheet GetResourceTimesheetByWeek(ProjectTimesheet projectTimesheet);
        void UpdateTimesheetId(int timesheetId, List<int> timesheetLogId);
        List<TeamTimesheet> GetResourceTeamTimesheet(TeamMember teamMember);
        List<TimesheetLogView> GetTimesheetLogByProjectId(List<int?> lstProjectId);
        int? GetResourceIdByTimesheetId(int? lstProjectId);
        List<int> GetTimesheetAlertForSubmission(DateTime weekStartDate);
        List<TimesheetAlertApproval> GetTimesheetAlertForApproval(DateTime weekStartDate);
        List<TimesheetLog> GetAllTimesheetLog();
        string GetTimesheetHomeReport(ProjectTimesheet projectTimesheet);
        List<TimesheetUtilizationView> GetTimesheetUtilizationDetails(int pResourceId);
        List<EmployeeTimeUtilizationView> GetEmployeeTimesheetUtilizationDetails(int pResourceId);
        List<EmployeeTimesheetHourView> GetEmployeeTimesheetByEmployeeId(int pResourceId);
        List<EmployeeTimesheetGridView> GetTimesheetGridDetailByEmployeeId(int pResourceId);
        List<TimesheetLogView> GetTimesheetLogByTimesheetId(List<int> timesheetId);
        List<TimesheetLog> GetTimesheetLogsByWeek(DateTime weekstartDate, int resourceId);
    }
    public class TimesheetLogRepository : BaseRepository<TimesheetLog>, ITimesheetLogRepository
    {
        private readonly TSDBContext _dbContext;
        public TimesheetLogRepository(TSDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public List<TimesheetLogView> GetTimesheetByTimesheetId(int timesheetId)
        {
            if (timesheetId > 0)
            {
                return _dbContext.TimesheetLog.Where(x => x.TimesheetId == timesheetId).Select(
               x => new TimesheetLogView
               {
                   TimesheetId = x.TimesheetId,
                   ProjectId = x.ProjectId,
                   ClockedHours = x.ClockedHours == null ? "0" : string.Format("{0}:{1}", x.ClockedHours.Value.Days * 24 + x.ClockedHours.Value.Hours, x.ClockedHours.Value.Minutes),
                   RequiredHours = x.RequiredHours == null ? "0" : string.Format("{0}:{1}", x.RequiredHours.Value.Days * 24 + x.RequiredHours.Value.Hours, x.RequiredHours.Value.Minutes),
                   PeriodSelection = x.PeriodSelection,
                   TimesheetLogId = x.TimesheetLogId,
                   WeekTimesheetId = x.WeekTimesheetId,
                   IsApproved = x.IsApproved,
                   IsRejected = x.IsRejected,
                   IsSubmitted = x.IsSubmitted,
                   ResourceId = x.ResourceId,
                   WorkItem=x.WorkItem
               }).ToList();
            }
            return new List<TimesheetLogView>();
        }
        public ResourceWeeklyTimesheet GetResourceTimesheetByWeek(ProjectTimesheet projectTimesheet)
        {
            ResourceWeeklyTimesheet resourceWeeklyTimesheet = new ResourceWeeklyTimesheet();
            List<ResourceTimesheet> lstResourceTimesheet = new List<ResourceTimesheet>();
            List<TimesheetSet> lstTimesheetSets = new List<TimesheetSet>();
            List<ResourceProjectList> resourceProjectLists = new List<ResourceProjectList>();
            List<ResourceTimeList> resourceTimeLists = new List<ResourceTimeList>();
            List<ResourceProjectList> spocProjectLists = new List<ResourceProjectList>();
            List<WeeklyTimesheetComments> weeklyTimesheetComments = new List<WeeklyTimesheetComments>();
            List<string> totalDayHours = new List<string>();
            List<int?> lstTimesheetId = new List<int?>();
            string timesheetStatus = "Yet to submit";
            string timesheetComment = "";
            int? TimesheetId = null;
            List<Guid?> weekTimesheetId = new List<Guid?>();
            if (projectTimesheet?.ResourceId > 0)
            {
                if (projectTimesheet?.TimesheetId > 0)
                {
                    DateTime? weekStartDate = _dbContext.TimesheetLog.Where(x => x.TimesheetId == projectTimesheet.TimesheetId).OrderBy(x => x.PeriodSelection).Select(x => x.PeriodSelection).FirstOrDefault();
                    if (weekStartDate != null)
                    {
                        projectTimesheet.WeekStartDate = weekStartDate?.AddDays((double)(DayOfWeek.Sunday - weekStartDate?.DayOfWeek));
                    }
                }

                var timesheetLogData = _dbContext.TimesheetLog.ToArray();
                var projectList = (from timesheetLog in timesheetLogData
                                   join projectDetails in projectTimesheet?.ProjectDetails?.ToArray() on timesheetLog.ProjectId equals projectDetails.ProjectId
                                   where timesheetLog.ResourceId == projectTimesheet?.ResourceId && projectDetails.ProjectStatus == "Ongoing"
                                   && timesheetLog.PeriodSelection.Date >= projectTimesheet?.WeekStartDate.Value.Date
                                   && timesheetLog.PeriodSelection.Date <= projectTimesheet?.WeekStartDate.Value.Date.AddDays(6)
                                   && (projectDetails.ProjectStartDate != null && projectDetails.ProjectStartDate <= projectTimesheet?.WeekStartDate.Value.Date)
                                   && (projectDetails.ProjectEndDate != null && projectDetails.ProjectEndDate >= projectTimesheet?.WeekStartDate.Value.Date.AddDays(6))
                                   select new
                                   {
                                       timesheetLog.ProjectId,
                                       projectDetails.ProjectName,
                                       timesheetLog.PeriodSelection,
                                       timesheetLog.RequiredHours,
                                       timesheetLog.ClockedHours,
                                       timesheetLog.TimesheetLogId,
                                       timesheetLog.IsSubmitted,
                                       timesheetLog.IsApproved,
                                       timesheetLog.IsRejected,
                                       timesheetLog.TimesheetId,
                                       timesheetLog.Comments,
                                       timesheetLog.CreatedOn,
                                       timesheetLog.WeekTimesheetId,
                                       projectDetails.ProjectSPOC,
                                       timesheetLog.WorkItem
                                   }).ToList();
                if (projectList?.Count > 0)
                {
                    if (projectTimesheet?.SPOCId > 0)
                    {
                        projectList = projectList?.Where(x => x.ProjectSPOC == projectTimesheet?.SPOCId).ToList();
                    }
                    weekTimesheetId = projectList.Select(x => x.WeekTimesheetId).ToList();
                    foreach (var timesheetSet in projectList.GroupBy(x => new { x.WeekTimesheetId }))
                    {
                        lstResourceTimesheet = new List<ResourceTimesheet>();
                        totalDayHours = new List<string>();
                        foreach (var project in timesheetSet.GroupBy(x => new { x.ProjectName, x.ProjectId }).OrderBy(x => x.Key.ProjectName))
                        {
                            //decimal projectTotalHours = 0;
                            //foreach(var item in project)
                            //{
                            //    string[] hours = item.ClockedHours.ToString().Split('.');
                            //    if (hours.Length > 1)
                            //    {
                            //        projectTotalHours += Convert.ToDecimal(hours[0]) * 60;
                            //        projectTotalHours += Convert.ToDecimal(hours[1]);
                            //    }
                            //    else
                            //    {
                            //        projectTotalHours += Convert.ToDecimal(hours[0]) * 60;
                            //    }
                            //}
                            //if (projectTotalHours > 0)
                            //{
                            //    projectTotalHours = Convert.ToDecimal(((int)Math.Floor(projectTotalHours / 60)).ToString() + "." + (projectTotalHours % 60).ToString());
                            //}
                            ResourceTimesheet resourceTimesheet = new ResourceTimesheet();

                            resourceTimesheet.ProjectName = project.Key.ProjectName;
                            resourceTimesheet.ProjectId = project.Key.ProjectId;
                            TimeSpan clockedHour = new TimeSpan(project.Sum(x => x.ClockedHours == null ? 0 : x.ClockedHours.Value.Ticks));
                            resourceTimesheet.TotalProjectClockedHours = string.Format("{0}:{1}", clockedHour.Days * 24 + clockedHour.Hours, clockedHour.Minutes);
                            resourceTimesheet.IsSubmited = project.All(x => x.IsSubmitted == true);
                            resourceTimesheet.IsRejected = project.All(x => x.IsRejected == true);
                            resourceTimesheet.IsApproved = project.All(x => x.IsApproved == true);
                            resourceTimesheet.TimesheetLog = new List<TimesheetLogView>();

                            foreach (var timesheet in project)
                            {
                                TimesheetLogView timesheetLog = new TimesheetLogView
                                {
                                    TimesheetLogId = timesheet.TimesheetLogId,
                                    PeriodSelection = timesheet.PeriodSelection,
                                    RequiredHours = timesheet.RequiredHours.ToString(),
                                    ClockedHours = timesheet.ClockedHours.ToString(),
                                    ProjectId = timesheet.ProjectId,
                                    WorkItem=timesheet.WorkItem
                                };
                                timesheetComment = timesheet.Comments;
                                resourceTimesheet.TimesheetLog.Add(timesheetLog);
                            }
                            TimesheetId = project.Select(x => x.TimesheetId).FirstOrDefault();
                            SharedLibraries.Models.Timesheet.Timesheet timesheet1 = new SharedLibraries.Models.Timesheet.Timesheet();
                            if (TimesheetId != null && TimesheetId > 0)
                                timesheet1 = _dbContext.Timesheet.Where(x => x.TimesheetId == TimesheetId).FirstOrDefault();
                            resourceTimesheet.IsBillable = timesheet1.IsBillable;
                            resourceTimesheet.OtherReasonForRejection = timesheet1.OtherReasonForRejection;
                            resourceTimesheet.RejectionReasonId = timesheet1.RejectionReasonId;
                            resourceTimesheet.RejectionReason = _dbContext.RejectionReason.Where(x => x.RejectionReasonId == timesheet1.RejectionReasonId).
                                Select(x => x.ReasonForRejection).FirstOrDefault();
                            resourceTimesheet.TimesheetId = TimesheetId;
                            lstResourceTimesheet.Add(resourceTimesheet);
                        }
                        long allProjectDayHour = 0;
                        for (int i = 0; i <= 6; i++)
                        {
                            var totalHours = timesheetSet?.GroupBy(x => new { x.PeriodSelection.Date, x.TimesheetLogId }).OrderBy(x => x.Key.Date).
                                Where(x => x.Key.Date.Date == projectTimesheet?.WeekStartDate?.AddDays(i).Date).ToList();
                            long dayHours = 0;
                            if (totalHours?.Count > 0)
                            {
                                foreach (var item in totalHours)
                                {
                                    // string[] hours = item.Select(x=>x.ClockedHours).FirstOrDefault().ToString().Split('.');
                                    //if (hours.Length > 1)
                                    //{
                                    //    dayHours += Convert.ToDecimal(hours[0]) * 60;
                                    //    dayHours += Convert.ToDecimal(hours[1]);
                                    //}
                                    //else
                                    //{
                                    //    dayHours += Convert.ToDecimal(hours[0]) * 60;
                                    //}
                                    dayHours += item.Select(x => x.ClockedHours.Value.Ticks).FirstOrDefault();
                                }
                            }
                            if (dayHours > 0)
                            {
                                allProjectDayHour += dayHours;
                            }
                            totalDayHours.Add(string.Format("{0}:{1}", new TimeSpan(dayHours).Days * 24 + new TimeSpan(dayHours).Hours, new TimeSpan(dayHours).Minutes));


                            //if (projectTotalHours > 0)
                            //{
                            //    projectTotalHours = Convert.ToDecimal(((int)Math.Floor(projectTotalHours / 60)).ToString() + "." + (projectTotalHours % 60).ToString());
                            //}
                        }
                        TimesheetSet newTimesheetSet = new TimesheetSet()
                        {
                            LstResourceTimesheet = lstResourceTimesheet?.OrderBy(x=>x.ProjectId).ToList(),
                            TotalDayHours = totalDayHours,
                            TotalClockedHours = string.Format("{0}:{1}", new TimeSpan(allProjectDayHour).Days * 24 + new TimeSpan(allProjectDayHour).Hours, new TimeSpan(allProjectDayHour).Minutes),
                            IsSubmited = timesheetSet.All(x => x.IsSubmitted == true),
                            IsRejected = timesheetSet.Any(x => x.IsRejected == true),
                            IsApproved = timesheetSet.All(x => x.IsApproved == true)
                        };
                        lstTimesheetSets.Add(newTimesheetSet);
                    }
                    var lastElement = projectList.Where(x=>x.WeekTimesheetId != null).OrderBy(x => x.TimesheetLogId).GroupBy(x => new { x.WeekTimesheetId }).LastOrDefault();
                    resourceWeeklyTimesheet.IsSubmited = (lastElement != null && lastElement.All(x => x.IsSubmitted == true));
                    if (resourceWeeklyTimesheet.IsSubmited)
                        timesheetStatus = "Submitted";
                    resourceWeeklyTimesheet.IsRejected = (lastElement != null && lastElement.Any(x => x.IsRejected == true));
                    if (resourceWeeklyTimesheet.IsRejected)
                        timesheetStatus = "Rejected";
                    resourceWeeklyTimesheet.IsApproved = (lastElement != null && lastElement.All(x => x.IsApproved == true));
                    if (resourceWeeklyTimesheet.IsApproved)
                        timesheetStatus = "Approved";
                }
                resourceWeeklyTimesheet.LstTimesheetSet = lstTimesheetSets;
                resourceWeeklyTimesheet.Status = timesheetStatus;
            }
            resourceProjectLists = (from projectDetails in projectTimesheet?.ProjectDetails
                                    join resourceAllocation in projectTimesheet?.ResourceAllocation on projectDetails.ProjectId equals resourceAllocation.ProjectId
                                    //join allocation in projectTimesheet?.Allocation on resourceAllocation.AllocationId equals allocation.AllocationId
                                    where resourceAllocation.EmployeeId == projectTimesheet?.ResourceId && projectDetails.ProjectStatus == "Ongoing"
                                    && (projectDetails.ProjectStartDate != null && projectDetails.ProjectStartDate <= projectTimesheet?.WeekStartDate.Value.Date)
                                    && (projectDetails.ProjectEndDate != null && projectDetails.ProjectEndDate >= projectTimesheet?.WeekStartDate?.Date.AddDays(6))
                                    && (resourceAllocation?.StartDate?.Date <= projectTimesheet?.WeekStartDate?.Date)
                                    && (resourceAllocation.EndDate == null || resourceAllocation?.EndDate?.Date >= projectTimesheet?.WeekStartDate?.Date)
                                    select new ResourceProjectList { ProjectId = projectDetails.ProjectId, ProjectName = projectDetails.ProjectName }).ToList();

            resourceTimeLists = (from projectDetails in projectTimesheet?.ProjectDetails
                                 join resourceAllocation in projectTimesheet?.ResourceAllocation on projectDetails.ProjectId equals resourceAllocation.ProjectId
                                 join Allocation in projectTimesheet?.Allocation on resourceAllocation.AllocationId equals Allocation.AllocationId
                                 where resourceAllocation.EmployeeId == projectTimesheet?.ResourceId && projectDetails.ProjectStatus == "Ongoing"
                                 && (projectDetails.ProjectStartDate != null && projectDetails.ProjectStartDate <= projectTimesheet?.WeekStartDate?.Date)
                                 && (projectDetails.ProjectEndDate != null && projectDetails.ProjectEndDate >= projectTimesheet?.WeekStartDate?.Date.AddDays(6))
                                 && (resourceAllocation.StartDate?.Date <= projectTimesheet?.WeekStartDate?.Date)
                                 && (resourceAllocation?.EndDate == null || resourceAllocation?.EndDate?.Date >= projectTimesheet?.WeekStartDate?.Date)
                                 group new { resourceAllocation, Allocation } by new { resourceAllocation.EmployeeId, resourceAllocation.ProjectId } into value

                                 select new ResourceTimeList
                                 {
                                     ProjectId = value.Key.ProjectId == null ? 0 : (int)value.Key.ProjectId,
                                     AllocationPercent =value.Sum(x => string.IsNullOrEmpty(x.Allocation.AllocationDescription) ? 0 : Convert.ToDecimal(x.Allocation.AllocationDescription.Substring(0, x.Allocation.AllocationDescription.Length - 1)) )

                                 }).ToList();

            //spocProjectLists = (from projectDetails in projectTimesheet?.ProjectDetails
            //                    where projectDetails.ProjectSPOC == projectTimesheet?.ResourceId && projectDetails.ProjectStatus == "Ongoing"
            //                    && (projectDetails.ProjectStartDate != null && projectDetails.ProjectStartDate <= projectTimesheet?.WeekStartDate.Value.Date)
            //                    && (projectDetails.ProjectEndDate != null && projectDetails.ProjectEndDate >= projectTimesheet?.WeekStartDate?.Date.AddDays(6))
            //                    select new ResourceProjectList { ProjectId = projectDetails.ProjectId, ProjectName = projectDetails.ProjectName }).ToList();
            //if (spocProjectLists != null && spocProjectLists.Count > 0)
            //{
            //    foreach (ResourceProjectList spocProjectList in spocProjectLists)
            //    {
            //        resourceProjectLists.Add(spocProjectList);
            //    }
            //}

            if (weekTimesheetId.Count > 0)
                weeklyTimesheetComments = (from timesheetComments in _dbContext.TimesheetComments
                                           where weekTimesheetId.Contains(timesheetComments.WeekTimesheetId)
                                           select new WeeklyTimesheetComments
                                           {
                                               Comments = timesheetComments.Comments,
                                               CreatedById = timesheetComments.CreatedBy == null ? 0 : (int)timesheetComments.CreatedBy,
                                               CreatedBy = "",
                                               CreatedOn = timesheetComments.CreatedOn,
                                               TimesheetCommentsId = timesheetComments.TimesheetCommentsId,
                                               TimesheetId = timesheetComments.TimesheetId
                                           }).OrderByDescending(x => x.TimesheetCommentsId).ToList();
            resourceWeeklyTimesheet.WeeklyTimesheetComments = weeklyTimesheetComments;
            resourceWeeklyTimesheet.ListOfProject = resourceProjectLists;
            resourceWeeklyTimesheet.ResourceTimeList = resourceTimeLists;
            resourceWeeklyTimesheet.Comments = timesheetComment;
            return resourceWeeklyTimesheet;
        }
        public TimesheetLog GetByID(int pTimesheetLogId)
        {
            return _dbContext.TimesheetLog.Where(x => x.TimesheetLogId == pTimesheetLogId).FirstOrDefault();
        }
        public TimesheetLog GetByTimesheetLogId(int timesheetLogId)
        {
            return _dbContext.TimesheetLog.Where(x => x.TimesheetLogId == timesheetLogId).FirstOrDefault();
        }
        public void UpdateTimesheetId(int timesheetId, List<int> timesheetLogId)
        {
            _dbContext.TimesheetLog.Where(x => timesheetLogId.Contains(x.TimesheetLogId)).ToList().ForEach((x =>
            { x.TimesheetId = timesheetId; x.IsSubmitted = true; }));
        }
        public List<TeamTimesheet> GetResourceTeamTimesheet(TeamMember teamMembers)
        {
            List<TeamTimesheet> lstResourceTeamTimesheet = new List<TeamTimesheet>();
            if (teamMembers?.ResourceId > 0)
            {
                foreach (var teamMember in teamMembers?.ListOfTeamMember?.GroupBy(x => new { x.UserId, x.ReportingPersonId }).OrderBy(x => x.Key.UserId))
                {
                    List<int> projectId = teamMembers?.ListOfTeamMember.Select(x => x.ProjectId).Distinct().ToList();
                    var teamTimesheetLog = (from timesheetLog in _dbContext.TimesheetLog
                                            where timesheetLog.ResourceId == teamMember.Key.UserId
                                            && timesheetLog.PeriodSelection.Date >= teamMembers.WeekStartDate.Value.Date
                                            && timesheetLog.PeriodSelection.Date <= teamMembers.WeekStartDate.Value.Date.AddDays(6)
                                            && timesheetLog.IsSubmitted == true
                                            && projectId.Contains(timesheetLog.ProjectId==null?0:(int)timesheetLog.ProjectId)
                                            select new
                                            {
                                                timesheetLog.RequiredHours,
                                                timesheetLog.ClockedHours,
                                                timesheetLog.IsSubmitted,
                                                timesheetLog.IsApproved,
                                                timesheetLog.IsRejected,
                                                timesheetLog.TimesheetId,
                                                timesheetLog.PeriodSelection.Date,
                                                timesheetLog.WorkItem,
                                                timesheetLog.CreatedOn,
                                                timesheetLog.WeekTimesheetId
                                            }).ToList();
                    //var lastElement = teamTimesheetLog.LastOrDefault();
                    //DateTime? firstDay = teamTimesheetLog.Select(x => x.Date).FirstOrDefault();
                    //if(firstDay !=null)
                    //{
                    //    TimeSpan shift = new TimeSpan(teamTimesheetLog.Where(x => x.Date == firstDay?.Date).Sum(x => x.RequiredHours == null ? 0 : x.RequiredHours.Value.Ticks));
                    //    teamTimesheet.shiftHours = string.Format("{0}:{1}", shift.TotalHours, shift.Minutes);
                    //}
                    //else
                    //{
                    //    teamTimesheet.shiftHours = "0:0";
                    //}
                    TeamTimesheet teamTimesheet = new TeamTimesheet();
                    teamTimesheet.UserId = teamMember.Key.UserId;
                    teamTimesheet.UserName = teamMember.Select(x => x.UserName).FirstOrDefault();
                    teamTimesheet.Destination = ""; // teamMember.Select(x => x.Destination).FirstOrDefault(),
                    TimeSpan clockedHour = new TimeSpan(teamTimesheetLog.Where(x=>x.IsRejected !=true).Sum(x => x.ClockedHours == null ? 0 : x.ClockedHours.Value.Ticks));
                    teamTimesheet.ClockedHours = string.Format("{0}:{1}", (int)clockedHour.TotalHours, clockedHour.Minutes);
                    teamTimesheet.RequiredHours = "0:0";
                    //TimeSpan requiredHours = new TimeSpan(teamTimesheetLog.Sum(x => x.RequiredHours == null ? 0 : x.RequiredHours.Value.Ticks));
                    //teamTimesheet.RequiredHours = string.Format("{0}:{1}", requiredHours.Days * 24 + requiredHours.Hours, requiredHours.Minutes % 60);
                    //teamTimesheet.RequiredHours = teamTimesheetLog?.Count > 0 ? _dbContext.Timesheet.Where(x => x.TimesheetId == teamTimesheetLog.FirstOrDefault().TimesheetId).Select(x => x.TotalRequiredHours).FirstOrDefault() : "0:0";
                    //if (teamTimesheet.RequiredHours == null)
                    //{
                    //    teamTimesheet.RequiredHours = "0:0";
                    //}
                    //if(!string.IsNullOrEmpty(teamTimesheet.ClockedHours)&& teamTimesheet.RequiredHours== "0:0")
                    //{
                    //    teamTimesheet.RequiredHours = teamTimesheet.ClockedHours;
                    //}
                    teamTimesheet.ReportingPersonId = teamMember.Key.ReportingPersonId;
                    teamTimesheet.Status = "Yet to Submit";
                    if (teamTimesheetLog.Count > 0)
                    {
                        var lastElement = teamTimesheetLog.GroupBy(x => new { x.WeekTimesheetId }).LastOrDefault();
                        if ((lastElement != null && lastElement.All(x => x.IsSubmitted == true)))
                            teamTimesheet.Status = "Submitted";
                        if ((lastElement != null && lastElement.Any(x => x.IsRejected == true)))
                            teamTimesheet.Status = "Rejected";
                        if ((lastElement != null && lastElement.All(x => x.IsApproved == true)))
                            teamTimesheet.Status = "Approved";                            
                    }
                    //teamTimesheet.Status = ((teamTimesheetLog.Count > 0 && teamTimesheetLog.All(x => x.IsApproved == true)) ? "Approved" :
                    //               teamTimesheetLog.Any(x => x.IsRejected == true) ? "Rejected" :
                    //               (teamTimesheetLog.Count > 0 && teamTimesheetLog.All(x => x.IsSubmitted == true)) ? "Submitted" :
                    //               "Yet to Submit");
                    teamTimesheet.TimesheetId = teamTimesheetLog.Select(x => x.TimesheetId).FirstOrDefault();
                    lstResourceTeamTimesheet.Add(teamTimesheet);
                }
            }
            return lstResourceTeamTimesheet;
        }
        public List<TimesheetLogView> GetTimesheetLogByProjectId(List<int?> lstProjectId)
        {
            return _dbContext.TimesheetLog.Where(x => lstProjectId.Contains(x.ProjectId) && x.IsSubmitted==true && x.IsRejected != true).Select(
               x => new TimesheetLogView
               {
                   TimesheetId = x.TimesheetId,
                   ProjectId = x.ProjectId,
                   ClockedHours = x.ClockedHours == null ? "0" : string.Format("{0}:{1}", (int)x.ClockedHours.Value.TotalHours, x.ClockedHours.Value.Minutes),
                   RequiredHours = x.RequiredHours == null ? "0" : string.Format("{0}:{1}", (int)x.RequiredHours.Value.TotalHours, x.RequiredHours.Value.Minutes),
                   PeriodSelection = x.PeriodSelection,
                   TimesheetLogId = x.TimesheetLogId,
                   WeekTimesheetId = x.WeekTimesheetId,
                   IsApproved = x.IsApproved,
                   IsRejected = x.IsRejected,
                   IsSubmitted = x.IsSubmitted,
                   ResourceId = x.ResourceId,
                   WorkItem=x.WorkItem
               }).ToList();
        }
        public int? GetResourceIdByTimesheetId(int? pTimesheetId)
        {
            return _dbContext.TimesheetLog.Where(x => x.TimesheetId == pTimesheetId).Select(x => x.ResourceId).FirstOrDefault();
        }
        public List<int> GetTimesheetAlertForSubmission(DateTime weekStartDate)
        {
            return (from timesheetLog in _dbContext.TimesheetLog
                    where timesheetLog.PeriodSelection.Date >= weekStartDate.Date
                   && timesheetLog.PeriodSelection.Date <= weekStartDate.Date.AddDays(6)
                   && timesheetLog.IsSubmitted == true
                    select timesheetLog.ResourceId == null ? 0 : (int)timesheetLog.ResourceId).Distinct().ToList();
        }
        public List<TimesheetAlertApproval> GetTimesheetAlertForApproval(DateTime weekStartDate)
        {
            return (from timesheetLog in _dbContext.TimesheetLog
                    where timesheetLog.PeriodSelection.Date >= weekStartDate.Date
                   && timesheetLog.PeriodSelection.Date <= weekStartDate.Date.AddDays(6)
                   && timesheetLog.IsSubmitted == true
                   && timesheetLog.IsRejected == false
                   && timesheetLog.IsApproved == false
                    select new TimesheetAlertApproval { ResourceId = timesheetLog.ResourceId == null ? 0 : (int)timesheetLog.ResourceId, ProjectId = timesheetLog.ProjectId == null ? 0 : (int)timesheetLog.ProjectId }).Distinct().ToList();
        }
        public List<TimesheetLog> GetAllTimesheetLog()
        {
            return _dbContext.TimesheetLog.ToList();
        }
        public string GetTimesheetHomeReport(ProjectTimesheet projectTimesheet)
        {
            string timesheetStatus = "Yet to submit";
            List<Guid?> weekTimesheetId = new List<Guid?>();
            if (projectTimesheet?.ResourceId > 0)
            {

                var timesheetLogData = _dbContext.TimesheetLog.ToArray();
                var projectList = (from timesheetLog in timesheetLogData
                                   join projectDetails in projectTimesheet?.ProjectDetails?.ToArray() on timesheetLog.ProjectId equals projectDetails.ProjectId
                                   where timesheetLog.ResourceId == projectTimesheet?.ResourceId && projectDetails.ProjectStatus == "Ongoing"
                                   && timesheetLog.PeriodSelection.Date >= projectTimesheet?.WeekStartDate.Value.Date
                                   && timesheetLog.PeriodSelection.Date <= projectTimesheet?.WeekStartDate.Value.Date.AddDays(6)
                                   && (projectDetails.ProjectStartDate != null && projectDetails.ProjectStartDate <= projectTimesheet?.WeekStartDate.Value.Date)
                                   && (projectDetails.ProjectEndDate != null && projectDetails.ProjectEndDate >= projectTimesheet?.WeekStartDate.Value.Date.AddDays(6))
                                   select new
                                   {
                                       timesheetLog.ProjectId,
                                       projectDetails.ProjectName,
                                       timesheetLog.PeriodSelection,
                                       timesheetLog.RequiredHours,
                                       timesheetLog.ClockedHours,
                                       timesheetLog.TimesheetLogId,
                                       timesheetLog.IsSubmitted,
                                       timesheetLog.IsApproved,
                                       timesheetLog.IsRejected,
                                       timesheetLog.TimesheetId,
                                       timesheetLog.Comments,
                                       timesheetLog.CreatedOn,
                                       timesheetLog.WeekTimesheetId,
                                       projectDetails.ProjectSPOC,
                                       timesheetLog.WorkItem
                                   }).ToList();
                if (projectList?.Count > 0)
                {

                    var lastElement = projectList.GroupBy(x => new { x.WeekTimesheetId}).LastOrDefault();
                    if ((lastElement != null && lastElement.All(x => x.IsSubmitted == true)))
                        timesheetStatus = "Submitted";
                    if ((lastElement != null && lastElement.Any(x => x.IsRejected == true)))
                        timesheetStatus = "Rejected";
                    if ((lastElement != null && lastElement.All(x => x.IsApproved == true)))
                        timesheetStatus = "Approved";
                }

            }

            return timesheetStatus;
        }
        public List<TimesheetUtilizationView> GetTimesheetUtilizationDetails(int pResourceId)
        {
            List<TimesheetUtilizationView> TimesheetUtilization = new List<TimesheetUtilizationView>();
            if (pResourceId > 0)
            {
                TimesheetUtilization = (from timesheet in _dbContext.Timesheet
                                        join timesheetLog in _dbContext.TimesheetLog on timesheet.WeekTimesheetId equals timesheetLog.WeekTimesheetId
                                        where timesheet.ReportingPersonId == pResourceId
                                        && timesheetLog.IsRejected != true && timesheetLog.IsSubmitted == true
                                        && timesheetLog.IsApproved == true
                                        select new TimesheetUtilizationView
                                        {
                                            ProjectId = (int)timesheetLog.ProjectId,
                                            ResourceId = (int)timesheet.ReportingPersonId,
                                            PlannedHour = timesheetLog.RequiredHours,
                                            ActualHour = timesheetLog.ClockedHours,
                                            Date = timesheetLog.PeriodSelection.Date,
                                            WorkItem= timesheetLog.WorkItem
                                        }).ToList();
            }
            return TimesheetUtilization;
        }
        public List<EmployeeTimeUtilizationView> GetEmployeeTimesheetUtilizationDetails(int pResourceId)
        {
            List<EmployeeTimeUtilizationView> TimesheetUtilization = new List<EmployeeTimeUtilizationView>();
            if (pResourceId > 0)
            {
                TimesheetUtilization = (from timesheet in _dbContext.Timesheet
                                        join timesheetLog in _dbContext.TimesheetLog on timesheet.WeekTimesheetId equals timesheetLog.WeekTimesheetId
                                        where timesheet.ReportingPersonId == pResourceId
                                        && timesheetLog.IsRejected != true && timesheetLog.IsSubmitted == true
                                        && timesheetLog.IsApproved == true
                                        select new EmployeeTimeUtilizationView
                                        {
                                            EmployeeId = (int)timesheetLog.ResourceId,
                                            PlannedHour = timesheetLog.RequiredHours,
                                            ActualHour = timesheetLog.ClockedHours,
                                            ProjectId = (int)timesheetLog.ProjectId,
                                            ResourceId = (int)timesheet.ReportingPersonId,
                                            Date = timesheetLog.PeriodSelection.Date,
                                            WorkItem= timesheetLog.WorkItem,
                                            Status = (timesheetLog.IsApproved == true ? "Approved" :
                                            timesheetLog.IsRejected == true ? "Rejected" :
                                            timesheetLog.IsSubmitted == true ? "Submitted" :
                                            "Yet to Submit")
                                        }).ToList();
            }
            return TimesheetUtilization;
        }

        public List<EmployeeTimesheetHourView> GetEmployeeTimesheetByEmployeeId(int pResourceId)//, DateTime? pWeekStartDate
        {
            List<EmployeeTimesheetHourView> timesheetHour = new List<EmployeeTimesheetHourView>();
            if (pResourceId > 0)
            {
                timesheetHour = (from timesheetLog in _dbContext.TimesheetLog
                                 where timesheetLog.ResourceId == pResourceId
                                 && timesheetLog.IsRejected != true && timesheetLog.IsSubmitted == true //&& timesheetLog.IsApproved == true
                                 select new EmployeeTimesheetHourView
                                 {
                                     employeeId = (int)timesheetLog.ResourceId,
                                     projectId = (int)timesheetLog.ProjectId,
                                     date = timesheetLog.PeriodSelection.Date,
                                     RequiredHours = timesheetLog.RequiredHours,
                                     clockedHours = timesheetLog.ClockedHours,
                                     WorkItem=timesheetLog.WorkItem
                                 }).ToList();
            }
            return timesheetHour;
        }
        public List<EmployeeTimesheetGridView> GetTimesheetGridDetailByEmployeeId(int pResourceId)
        {
            List<EmployeeTimesheetGridView> timesheetHour = new List<EmployeeTimesheetGridView>();
            if (pResourceId > 0)
            {
                timesheetHour = (from timesheetLog in _dbContext.TimesheetLog
                                 where timesheetLog.ResourceId == pResourceId
                                 select new EmployeeTimesheetGridView
                                 {
                                     EmployeeId = pResourceId,
                                     projectId = (int)timesheetLog.ProjectId,
                                     Date = timesheetLog.PeriodSelection,
                                     RequiredHour = timesheetLog.RequiredHours,
                                     clockedHour = timesheetLog.ClockedHours,
                                     WorkItem=timesheetLog.WorkItem,
                                     status = (timesheetLog.IsApproved == true ? "Approved" :
                                     timesheetLog.IsRejected == true ? "Rejected" :
                                     timesheetLog.IsSubmitted == true ? "Submitted" :
                                     "Yet to Submit")
                                 }).ToList();
            }
            return timesheetHour;
        }
        public List<TimesheetLogView> GetTimesheetLogByTimesheetId(List<int> timesheetId)
        {
            if (timesheetId?.Count>0)
            {
                return _dbContext.TimesheetLog.Where(x => timesheetId.Contains(x.TimesheetId==null?0:(int)x.TimesheetId)).Select(
               x => new TimesheetLogView
               {
                   TimesheetId = x.TimesheetId,
                   ProjectId = x.ProjectId,
                   ClockedHours = x.ClockedHours == null ? "0" : string.Format("{0}:{1}", x.ClockedHours.Value.Days * 24 + x.ClockedHours.Value.Hours, x.ClockedHours.Value.Minutes),
                   RequiredHours = x.RequiredHours == null ? "0" : string.Format("{0}:{1}", x.RequiredHours.Value.Days * 24 + x.RequiredHours.Value.Hours, x.RequiredHours.Value.Minutes),
                   PeriodSelection = x.PeriodSelection,
                   TimesheetLogId = x.TimesheetLogId,
                   WeekTimesheetId = x.WeekTimesheetId,
                   IsApproved = x.IsApproved,
                   IsRejected = x.IsRejected,
                   IsSubmitted = x.IsSubmitted,
                   ResourceId = x.ResourceId,
                   WorkItem=x.WorkItem
               }).ToList();
            }
            return new List<TimesheetLogView>();
        }

        public List<TimesheetLog> GetTimesheetLogsByWeek(DateTime weekstartDate , int resourceId)
        {
           return _dbContext.TimesheetLog.Where(x => x.PeriodSelection.Date >= weekstartDate.Date && x.PeriodSelection.Date <= weekstartDate.AddDays(6).Date && x.ResourceId == resourceId && x.IsSubmitted == false).ToList();
        }
    }
}