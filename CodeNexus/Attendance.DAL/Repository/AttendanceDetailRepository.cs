using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Attendance.DAL.Repository
{
    public interface IAttendanceDetailRepository : IBaseRepository<AttendanceDetail>
    {
        public AttendanceDetail GetAttendanceDetailById(int attendanceId);
        public WeeklyMonthlyAttendance GetDailyAttendanceDetail(int employyeId,int shiftId, DateTime shiftDate);
        List<WeeklyMonthlyAttendance> GetWeeklyMonthlyAttendanceDetail(WeekMonthAttendanceView weekMonthAttendance);
        public TimeSpan? GetAttendanceHoursById(int attendanceId);
        //public TimeSpan? GetAttendanceBreakHoursById(int attendanceId);
        List<EmployeesAttendanceDetails> GetEmployeeAttendanceDetails(ReportingManagerTeamLeaveView managerTeamLeaveView);
        List<DetailsView> GetAttendanceDetailsById(AttendanceWeekView attendanceWeekView);
        public ShiftViewDetails GetShiftWeekendDetails(int ShiftDetailsId);
        public List<ApplyLeavesView> GetEmployeeAbsentList(EmployeeDepartmentAndLocationView employeeDepartments);
        public List<EmployeesAttendanceDetails> GetEmployeeAttendanceDetailsByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate);
        bool GetAttendanceDetailByAttendanceId(int attendanceId, DateTime fromDate, DateTime toDate, int attendanceDetailId, bool isEdit=false);
        AttendanceDetail GetAttendanceDetailByAttendanceDetailId(int attendanceDetailId);
        //List<AttendanceDetail> GetAttendanceDetailListByAttendanceId(int attendanceId);
        List<TeamLeaveView> GetEmployeeRegularizationList(ReportingManagerTeamLeaveView managerTeamLeaveView);
        List<ApplyLeavesView> GetEmployeeRegularizationById(int employeeId, DateTime FromDate, DateTime ToDate);
        public TimeSpan? GetAttendanceDetailByEmployeeId(int employeeId, DateTime Date, int attendnaceDetailId);
        public EmployeeShiftDetailView GetShiftDetails(int ShiftId);
        RegularizationDetailView GetEmployeeRegularizationByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction);
        List<EmployeeRequestCount> GetAttendanceRequestCount(List<int> employeeIdList);
    }
    public class AttendanceDetailRepository : BaseRepository<AttendanceDetail>, IAttendanceDetailRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AttendanceDetailRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public AttendanceDetail GetAttendanceDetailById(int attendanceId)
        {
            AttendanceDetail attendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceId && x.Isregularize == null).OrderByDescending(x => x.AttendanceDetailId).Select(x => x).FirstOrDefault();
            return attendanceDetail == null ? new AttendanceDetail() : attendanceDetail;
        }

        public TimeSpan? GetAttendanceHoursById(int attendanceId)
        {
            List<TimeSpan?> totalHour = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceId).Select(x => x.TotalHours).ToList();
            return new TimeSpan(totalHour.Sum(r => r == null ? 0 : r.Value.Ticks));
        }
        //public TimeSpan? GetAttendanceBreakHoursById(int attendanceId)
        //{
        //    List<TimeSpan?> breakHour = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceId).Select(x => x.BreakHours).ToList();
        //    return new TimeSpan(breakHour.Sum(r => r == null ? 0 : r.Value.Ticks));
        //}

        public WeeklyMonthlyAttendance GetDailyAttendanceDetail(int employeeId,int shiftId, DateTime shiftDate)
        {
            //DateTime shiftDate = DateTime.Now;
            ShiftDetails shiftDetails = dbContext.ShiftDetails.Where(x => x.ShiftDetailsId == shiftId).FirstOrDefault();
            WeeklyMonthlyAttendance weeklyMonthlyAttendance = new WeeklyMonthlyAttendance();

            if(shiftDetails !=null)
            {
                TimeSpan? totalHours = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == shiftId).Select(rs => rs.td.TotalHours).FirstOrDefault();
                weeklyMonthlyAttendance = (from attend in dbContext.Attendance
                                           where attend.Date == shiftDate.Date && attend.EmployeeId == employeeId
                                           select new WeeklyMonthlyAttendance
                                           {
                                               AttendanceId = attend.AttendanceId,
                                               EmployeeId = attend.EmployeeId,
                                               Date = shiftDate,
                                               TotalHours = attend.TotalHours == null ? "0" : attend.TotalHours.Value.ToString(),
                                               BreakHours = "00", //attend.BreakHours == null ? "0" : attend.BreakHours.Value.ToString(),
                                               IsCheckin = attend.IsCheckin,
                                               ShiftFromTime = shiftDetails.TimeFrom==null?"": shiftDetails.TimeFrom.ToString(),
                                               ShiftToTime = shiftDetails.TimeTo==null?"": shiftDetails.TimeTo.ToString(),
                                               //ShiftHour = "08:00:00"
                                               ShiftHour = totalHours==null?"00": totalHours.ToString(),
                                               IsActiveShift= shiftDetails.IsActive == null ?false : (bool)shiftDetails.IsActive,
                                               IsFlexyShift = shiftDetails.IsFlexyShift == null ? false : (bool)shiftDetails.IsFlexyShift,                                             
                                           }).FirstOrDefault();
            }             
            if (weeklyMonthlyAttendance != null)
            {
                weeklyMonthlyAttendance.WeeklyMonthlyAttendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == weeklyMonthlyAttendance.AttendanceId &&
                ((x.Status == null && x.Isregularize == null) || (x.Status.ToLower() == "approved" && x.Isregularize == true))).Select(
                x => new WeeklyMonthlyAttendanceDetail
                {
                    AttendanceDetailId = x.AttendanceDetailId,
                    AttendanceId = x.AttendanceId,
                    CheckinTime = x.CheckinTime,
                    CheckoutTime = x.CheckoutTime,
                    //BreakinTime = x.CheckoutTime.Value,
                    //BreakoutTime = x.BreakoutTime.Value,
                    TotalHours = x.TotalHours.Value.ToString(),
                    //BreakHours = x.BreakHours.Value.ToString()
                }).ToList();
            }
            else
            {
                weeklyMonthlyAttendance = new WeeklyMonthlyAttendance();
                weeklyMonthlyAttendance.Date = shiftDate;
                weeklyMonthlyAttendance.ShiftFromTime = shiftDetails.TimeFrom.ToString();
                weeklyMonthlyAttendance.ShiftToTime = shiftDetails.TimeTo.ToString();
                weeklyMonthlyAttendance.ShiftHour = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == shiftId).Select(rs => rs.td.TotalHours).FirstOrDefault().ToString();
                weeklyMonthlyAttendance.IsFlexyShift = shiftDetails.IsFlexyShift == null ? false : (bool)shiftDetails.IsFlexyShift;
                weeklyMonthlyAttendance.IsActiveShift = shiftDetails.IsActive == null ? false : (bool)shiftDetails.IsActive;
            }
            return weeklyMonthlyAttendance != null ? weeklyMonthlyAttendance : new WeeklyMonthlyAttendance();
        }

        public List<WeeklyMonthlyAttendance> GetWeeklyMonthlyAttendanceDetail(WeekMonthAttendanceView weekMonthAttendance)
        {
            //DateTime todayDate=DateTime.Now;
            ShiftDetails shiftDetails = dbContext.ShiftDetails.Where(x => x.ShiftDetailsId == weekMonthAttendance.ShiftDetailsId).FirstOrDefault();
            //if (shiftDetails?.TimeFrom <= DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay <= new TimeSpan(24, 0, 0))
            //{
            //    todayDate = DateTime.Now;
            //}
            //else
            //{
            //    todayDate = DateTime.Now.AddDays(-1);
            //}
            List<WeeklyMonthlyAttendance> WeeklyMonthlyMarkedAttendance = (from attend in dbContext.Attendance
                                                                           where attend.Date >= weekMonthAttendance.FromDate && attend.Date <= weekMonthAttendance.ToDate && attend.EmployeeId == weekMonthAttendance.EmployeeId
                                                                           select new WeeklyMonthlyAttendance
                                                                           {
                                                                               AttendanceId = attend.AttendanceId,
                                                                               EmployeeId = attend.EmployeeId,
                                                                               Date = attend.Date.Date,
                                                                               TotalHours = attend.TotalHours == null ? "0" : attend.TotalHours.Value.ToString(),
                                                                               //BreakHours = attend.BreakHours == null ? "0" : attend.BreakHours.Value.ToString(),
                                                                               IsCheckin = attend.IsCheckin,
                                                                               ShiftHour = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == weekMonthAttendance.ShiftDetailsId).Select(rs => rs.td.TotalHours).FirstOrDefault().ToString(),
                                                                               WeeklyMonthlyAttendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attend.AttendanceId && (x.Status == "Approved" || x.Status == null || x.Status=="Pending") && ((x.CheckinTime != null && x.CheckoutTime != null) || attend.Date.Date== weekMonthAttendance.ShiftDate.Date)).Select(x =>
                                                                                    new WeeklyMonthlyAttendanceDetail
                                                                                    {
                                                                                        AttendanceDetailId = x.AttendanceDetailId,
                                                                                        AttendanceId = x.AttendanceId,
                                                                                        CheckinTime = x.CheckinTime,
                                                                                        CheckoutTime = x.CheckoutTime,
                                                                                        IsRegularize = x.Isregularize,
                                                                                        Status=x.Status,
                                                                                       //BreakinTime = x.CheckoutTime.Value,
                                                                                       //BreakoutTime = x.BreakoutTime.Value,
                                                                                        TotalHours = x.TotalHours==null?"0": x.TotalHours.Value.ToString(),
                                                                                        CreatedOn = x.CreatedOn,
                                                                                        //BreakHours = x.BreakHours.Value.ToString()
                                                                                         Reason = x.Reason
                                                                                    }
                                                                               ).ToList(),
                                                                               WeeklyMonthlyRegularizationDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attend.AttendanceId && x.Isregularize==true).Select(x =>
                                                                                 new WeeklyMonthlyAttendanceDetail
                                                                                 {
                                                                                     AttendanceDetailId = x.AttendanceDetailId,
                                                                                     AttendanceId = x.AttendanceId,
                                                                                     CheckinTime = x.CheckinTime,
                                                                                     CheckoutTime = x.CheckoutTime,
                                                                                     TotalHours = x.TotalHours == null ? "0" : x.TotalHours.Value.ToString(),
                                                                                     IsRegularize=x.Isregularize,
                                                                                     Status=x.Status,
                                                                                     CreatedOn=x.CreatedOn,
                                                                                     Reason =x.Reason
                                                                                         
                                                                                     }).OrderByDescending(x=>x.CreatedOn).ToList()
                                                                           }).ToList();


            return WeeklyMonthlyMarkedAttendance;
        }

        public List<EmployeesAttendanceDetails> GetEmployeeAttendanceDetailsByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<EmployeesAttendanceDetails> employeesAttendanceDetails = new();
            var dates = new List<DateTime>();
            for (var dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            foreach (var date in dates)
            {
                SharedLibraries.Models.Attendance.Attendance attendance = dbContext.Attendance.Where(x => x.EmployeeId == employeeId && x.Date == date).Select(x => x).FirstOrDefault();
                List<TimeSpan?> totalBreakHour = new List<TimeSpan?>();
                TimeSpan? totalHour = new TimeSpan(0, 0, 0);
                if (attendance != null)
                {
                    totalHour = attendance?.TotalHours;
                    //TimeSpan? breakHour = dbContext.Attendance.Where(x => x.EmployeeId == employeeId && x.Date == date).Select(x => x.BreakHours).FirstOrDefault();
                    List<AttendanceDetail> attendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendance.AttendanceId && ((x.Status == null && x.Isregularize == null) || (x.Status.ToLower() == "approved" && x.Isregularize == true)) && x.CheckinTime != null && x.CheckoutTime != null).OrderBy(x => x.CheckinTime).ToList();

                    for (int i = 0; i < attendanceDetail?.Count; i++)
                    {
                        if (i < (attendanceDetail?.Count - 1))
                        {
                            //attendanceDetail[i].BreakoutTime = attendanceDetail[i + 1].CheckinTime;
                            TimeSpan? breakHours = attendanceDetail[i + 1].CheckinTime - attendanceDetail[i].CheckoutTime;
                            totalBreakHour.Add(breakHours == null ? new TimeSpan(0) : breakHours);
                        }
                    }
                }

                TimeSpan breakHour = new TimeSpan(totalBreakHour.Sum(r => r == null ? 0 : r.Value.Ticks));

                TimeSpan? TotalHour = new TimeSpan((totalHour == null ? 0 : totalHour.Value.Ticks) + (breakHour.Ticks));
                EmployeesAttendanceDetails employeesAttendance = new();
                employeesAttendance.EmployeeId = employeeId;
                employeesAttendance.Date = date;
                //employeesAttendance.WorkedHours = (totalHour == null) ? "00:00:00" : totalHour.Value.ToString();
                employeesAttendance.WorkedHours = (totalHour == null) ? "00:00:00" : string.Format("{0:hh\\:mm\\:ss}", totalHour);
                //employeesAttendance.TotalHours = breakHour.ToString();
                employeesAttendance.BreakHours = string.Format("{0:hh\\:mm\\:ss}", breakHour);
                //employeesAttendance.TotalHours = TotalHour.ToString();
                employeesAttendance.TotalHours = string.Format("{0:hh\\:mm\\:ss}", TotalHour);
                //employeesAttendance = (from attend in dbContext.Attendance
                //                           // where attend.EmployeeId == employeeId && attend.Date >= fromDate && attend.Date <= toDate
                //                       select new EmployeesAttendanceDetails
                //                       {
                //                           EmployeeId = employeeId,
                //                           Date = date,
                //                           WorkedHours = (totalHour == null) ? "00:00:00" : totalHour.Value.ToString(),
                //                           BreakHours = breakHour.ToString(),
                //                           TotalHours = TotalHour.ToString(),
                //                       }).FirstOrDefault();
                employeesAttendanceDetails?.Add(employeesAttendance);
            }
            return employeesAttendanceDetails;
        }

        public List<DetailsView> GetAttendanceDetailsById(AttendanceWeekView attendanceWeekView)
        {

            List<DetailsView> data=(from attend in dbContext.Attendance
                                      where attend.EmployeeId == attendanceWeekView.EmployeeId && attend.Date >= attendanceWeekView.FromDate && attend.Date <= attendanceWeekView.ToDate
                                      select new DetailsView
                                      {
                                          EmployeeId = attend.EmployeeId,
                                          AttendanceId=attend.AttendanceId,
                                          Date = attend.Date,
                                          TotalHours = attend.TotalHours == null ? "00" : attend.TotalHours.Value.ToString(),
                                          //BreakHours = attend.BreakHours == null ? "00" : attend.BreakHours.Value.ToString(),
                                      }).ToList();
            foreach(DetailsView item in data)
            {
                List<AttendanceDetail> attendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == item.AttendanceId && ((x.Status == null && x.Isregularize == null) || (x.Status.ToLower() == "approved" && x.Isregularize == true)) && x.CheckinTime != null && x.CheckoutTime != null).OrderBy(x => x.CheckinTime).ToList();
                List<TimeSpan?> totalBreakHour = new List<TimeSpan?>();
                for (int i = 0; i < attendanceDetail?.Count; i++)
                {
                    if (i < (attendanceDetail?.Count - 1))
                    {
                        //attendanceDetail[i].BreakoutTime = attendanceDetail[i + 1].CheckinTime;
                        TimeSpan? breakHours = attendanceDetail[i + 1].CheckinTime - attendanceDetail[i].CheckoutTime;
                        totalBreakHour.Add(breakHours == null ? new TimeSpan(0) : breakHours);
                    }
                }
                item.BreakHours= new TimeSpan(totalBreakHour.Sum(r => r == null ? 0 : r.Value.Ticks)).ToString();
            }
            return data;
        }

        public static DateTime FirstDayOfWeek(DateTime date)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var dateDiff = date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (dateDiff < 0)
                dateDiff += 7;
            return date.AddDays(-dateDiff).Date;
        }

        public ShiftViewDetails GetShiftWeekendDetails(int ShiftDetailsId)
        {
            if (ShiftDetailsId == 0)
            {
                int shiftId = dbContext.ShiftDetails.Where(x => x.ShiftName.ToLower() == "general shift").Select(x => x.ShiftDetailsId).FirstOrDefault();
                if (shiftId == 0)
                {
                    shiftId = dbContext.ShiftDetails.Select(x => x.ShiftDetailsId).FirstOrDefault();
                }
                return GetShiftDetailsById(shiftId);
            }
            else
            {
                return GetShiftDetailsById(ShiftDetailsId);
            }
        }
        public ShiftViewDetails GetShiftDetailsById(int shiftId)
        {
            ShiftViewDetails shiftvsl = new ShiftViewDetails();
            List<WeekendViewDefinition> listWeekend = new List<WeekendViewDefinition>();
            listWeekend = (from weekend in dbContext.WeekendDefinition
                           join shift in dbContext.ShiftWeekendDefinition on weekend.WeekendDayId equals shift.WeekendDayId
                           where shift.ShiftDetailsId == shiftId
                           select new WeekendViewDefinition
                           {
                               WeekendDayId = weekend.WeekendDayId,
                               WeekendDayName = weekend.WeekendDayName,
                               ShiftDetailsId = shift.ShiftDetailsId,
                           }).ToList();

            shiftvsl = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == shiftId).Select(rs => new ShiftViewDetails
            {
                ShiftDetailsId = rs.sd.ShiftDetailsId,
                ShiftName = rs.sd.ShiftName,
                ShiftCode = rs.sd.ShiftCode,
                TotalHours = rs.td.TotalHours.ToString(),
                WeekendList = listWeekend,
                IsConsiderAbsent = rs.td.IsConsiderAbsent,
                AbsentFromHour = rs.td.AbsentFromHour.ToString(),
                AbsentFromOperator = rs.td.AbsentFromOperator,
                AbsentToHour = rs.td.AbsentToHour.ToString(),
                AbsentToOperator = rs.td.AbsentToOperator,
                IsConsiderHalfaDay = rs.td.IsConsiderHalfaDay,
                HalfaDayFromHour = rs.td.HalfaDayFromHour.ToString(),
                HalfaDayFromOperator = rs.td.HalfaDayFromOperator,
                HalfaDayToHour = rs.td.HalfaDayToHour.ToString(),
                HalfaDayToOperator = rs.td.HalfaDayToOperator,
                IsConsiderPresent = rs.td.IsConsiderPresent,
                PresentHour = rs.td.PresentHour.ToString(),
                PresentHourOperator = rs.td.PresentOperator,
                TimeFrom = rs.sd.TimeFrom.ToString(),
                TimeTo = rs.sd.TimeTo.ToString(),
                ShiftTimeFrom = rs.sd.TimeFrom.ToString(),
                ShiftTimeTo = rs.sd.TimeTo.ToString()
            }).FirstOrDefault();
            return shiftvsl == null ? new ShiftViewDetails() : shiftvsl;
        }

        public List<ApplyLeavesView> GetEmployeeAbsentList(EmployeeDepartmentAndLocationView employeeDepartments)
        {
            List<ApplyLeavesView> employeeAbsentLists = new List<ApplyLeavesView>();
            var dates = new List<DateTime>();
            if(employeeDepartments.DOJ > employeeDepartments.fromDate) { employeeDepartments.fromDate = employeeDepartments.DOJ; }
            if (employeeDepartments.toDate > DateTime.Now.Date) { employeeDepartments.toDate = DateTime.Now.Date; }
            for (var dt = employeeDepartments.fromDate; dt < employeeDepartments.toDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            {
                dates = dates.OrderByDescending(x => x).Skip(employeeDepartments.NoOfRecord * (employeeDepartments.PageNumber)).Take(employeeDepartments.NoOfRecord).Distinct().ToList();
                var attendanceDates = new List<DateTime>();
                employeeAbsentLists = (from attend in dbContext.Attendance
                                    where attend.EmployeeId == employeeDepartments.employeeId
                                    select new ApplyLeavesView
                                    {
                                        EmployeeId = employeeDepartments.employeeId,
                                        FromDate = attend.Date,
                                        ToDate = attend.Date,
                                        AttendaceId = attend.AttendanceId,
                                        NoOfDays = 1,//(decimal)(attend.Date.Subtract(attend.Date).TotalDays + 1),
                                        Status = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attend.AttendanceId && x.Status == "Pending").Select(x => x.Status).FirstOrDefault(),
                                        TotalHours = attend.TotalHours.ToString(),
                                        WeeklyMonthlyAttendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attend.AttendanceId && (x.Status != "Rejected" || x.Status != "Cancelled") 
                                                                        && x.CheckinTime != null && x.CheckoutTime != null).Select(x =>
                                                                                        new WeeklyMonthlyAttendanceDetail
                                                                                        {
                                                                                            AttendanceDetailId = x.AttendanceDetailId,
                                                                                            AttendanceId = x.AttendanceId,
                                                                                            CheckinTime = x.CheckinTime == null ? new DateTime?() : x.CheckinTime,
                                                                                            CheckoutTime = x.CheckoutTime,
                                                                                                //BreakinTime = x.CheckoutTime.Value,
                                                                                                //BreakoutTime = x.BreakoutTime.Value,
                                                                                                TotalHours = x.TotalHours.Value.ToString(),
                                                                                                //BreakHours = x.BreakHours.Value.ToString(),
                                                                                                Status = x.Status
                                                                                            }
                                                                            ).OrderBy(x => x.CheckinTime).ToList()
                                    }).OrderByDescending(x => x.FromDate).Skip(employeeDepartments.NoOfRecord * (employeeDepartments.PageNumber)).Take(employeeDepartments.NoOfRecord).Distinct().ToList();
                if (employeeAbsentLists != null && employeeAbsentLists.Count > 0)
                    attendanceDates = (from attend in employeeAbsentLists
                                    select (DateTime)attend.FromDate).Distinct().ToList();
                employeeAbsentLists.AddRange((from fullDate in dates
                                       where !attendanceDates.Contains(fullDate)
                                       select new ApplyLeavesView
                                       {
                                           EmployeeId = employeeDepartments.employeeId,
                                            FromDate = fullDate,
                                            ToDate = fullDate,
                                            AttendaceId = 0,
                                            NoOfDays = 1, //(decimal)(fullDate.Subtract(fullDate).TotalDays + 1),
                                            Status = null,
                                            TotalHours = "00:00"
                                        }
                                       ).ToList());
                employeeAbsentLists = employeeAbsentLists.OrderByDescending(x => x.FromDate).Skip(employeeDepartments.NoOfRecord * (employeeDepartments.PageNumber)).Take(employeeDepartments.NoOfRecord).Distinct().ToList();
            }
            /*
            foreach (var date in dates)
            {
                if (DateTime.Now.Date >= date) 
                //if (date == new DateTime(2021, 12, 29))
                {
                    SharedLibraries.Models.Attendance.Attendance attendanceDetail = dbContext.Attendance.Where(r => r.Date == date && r.EmployeeId == employeeDepartments.employeeId).FirstOrDefault();
                    if (attendanceDetail != null)
                    {
                        List<AttendanceDetail> attendanceStatus = new();
                        attendanceStatus = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceDetail.AttendanceId).Select(x => x).ToList();
                        string status = attendanceStatus?.Where(x => x.Status?.ToLower() == "pending")?.Select(x => x?.Status).FirstOrDefault();
                        ApplyLeavesView employeeAbsent = new();
                        employeeAbsent = (from attend in dbContext.Attendance
                                          where attend.EmployeeId == employeeDepartments.employeeId
                                          select new ApplyLeavesView
                                          {
                                              EmployeeId = employeeDepartments.employeeId,
                                              FromDate = date,
                                              ToDate = date,
                                              AttendaceId = attendanceDetail.AttendanceId,
                                              NoOfDays = (decimal)(date.Subtract(date).TotalDays + 1),
                                              Status = status,
                                              //IsRegularize = dbContext.Attendance.Join(dbContext.AttendanceDetail, attend => attend.AttendanceId, ad => ad.AttendanceId, (attend, ad) => new { attend, ad }).Where(r => r.attend.Date == date && r.attend.EmployeeId == employeeDepartments.employeeId && r.ad.Status.ToLower() != "approved").Select(r => r.ad.Isregularize).FirstOrDefault(),
                                              TotalHours = dbContext.Attendance.Where(x => x.Date == date && x.EmployeeId == employeeDepartments.employeeId).Select(x => x.TotalHours.ToString()).FirstOrDefault(),
                                              //TotalHours = dbContext.Attendance.Join(dbContext.AttendanceDetail, attend => attend.AttendanceId, ad => ad.AttendanceId, (attend, ad) => new { attend, ad }).Where(r => r.attend.Date == date && r.attend.EmployeeId == employeeDepartments.employeeId && r.ad.Isregularize == false).Select(r => r.attend.TotalHours.ToString()).FirstOrDefault(),
                                              //ShiftHour = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == employeeDepartments.ShiftId).Select(rs => rs.td.TotalHours).FirstOrDefault().ToString(),
                                              WeeklyMonthlyAttendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceDetail.AttendanceId && (x.Status!="Rejected" || x.Status!="Cancelled" ) && x.CheckinTime != null && x.CheckoutTime != null).Select(x =>
                                                                                             new WeeklyMonthlyAttendanceDetail
                                                                                             {
                                                                                                 AttendanceDetailId = x.AttendanceDetailId,
                                                                                                 AttendanceId = x.AttendanceId,
                                                                                                 CheckinTime = x.CheckinTime == null ? new DateTime?() : x.CheckinTime,
                                                                                                 CheckoutTime = x.CheckoutTime,
                                                                                                 //BreakinTime = x.CheckoutTime.Value,
                                                                                                 //BreakoutTime = x.BreakoutTime.Value,
                                                                                                 TotalHours = x.TotalHours.Value.ToString(),
                                                                                                 //BreakHours = x.BreakHours.Value.ToString()
                                                                                             }
                                                                                   ).OrderBy(x => x.CheckinTime).ToList()
                                          }).FirstOrDefault();
                        employeeAbsentLists?.Add(employeeAbsent);

                    }
                    //if (employeeAbsent == null)
                    else
                    {
                        ApplyLeavesView employeeAbsents = new();

                        employeeAbsents = new ApplyLeavesView();
                        employeeAbsents.EmployeeId = employeeDepartments.employeeId;
                        employeeAbsents.FromDate = date;
                        employeeAbsents.ToDate = date;
                        employeeAbsents.AttendaceId = 0;
                        employeeAbsents.NoOfDays = (decimal)(date.Subtract(date).TotalDays + 1);
                        employeeAbsents.Status = null;
                        employeeAbsents.TotalHours = "00:00";
                        //}
                        employeeAbsentLists?.Add(employeeAbsents);
                    }
                }
            }
            */
            employeeAbsentLists = employeeAbsentLists.Where(x => x?.Status?.ToLower() != "pending" && x!=null)?.Select(x => x).ToList();
            //employeeAbsentLists = employeeAbsentLists.Where(x => x.IsAbsent!=null && x.IsAbsent == true)?.Select(x => x).ToList();
            return employeeAbsentLists;
        }

        public List<EmployeesAttendanceDetails> GetEmployeeAttendanceDetails(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            List<EmployeesAttendanceDetails> employeesAttendanceDetails = new();
            if(managerTeamLeaveView?.ResourceId?.Count>0)
            {
                foreach (var item in managerTeamLeaveView.ResourceId)
                {
                    List<TimeSpan?> totalHour = dbContext.Attendance.Where(x => x.EmployeeId == item && x.Date >= managerTeamLeaveView.FromDate && x.Date <= managerTeamLeaveView.ToDate).Select(x => x.TotalHours).ToList();
                    //List<TimeSpan?> breakHour = dbContext.Attendance.Where(x => x.EmployeeId == item && x.Date >= managerTeamLeaveView.FromDate && x.Date <= managerTeamLeaveView.ToDate).Select(x => x.BreakHours).ToList();
                    List<int> attendanceIdList = dbContext.Attendance.Where(x => x.EmployeeId == item && x.Date >= managerTeamLeaveView.FromDate && x.Date <= managerTeamLeaveView.ToDate).Select(x => x.AttendanceId).ToList();
                    List<TimeSpan?> breakHour = new List<TimeSpan?>();
                    foreach (int attendanceId in attendanceIdList)
                    {
                        List<AttendanceDetail> attendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceId && ((x.Status == null && x.Isregularize == null) || (x.Status.ToLower() == "approved" && x.Isregularize == true)) && x.CheckinTime != null && x.CheckoutTime != null).OrderBy(x => x.CheckinTime).ToList();
                        List<TimeSpan?> totalBreakHour = new List<TimeSpan?>();
                        for (int i = 0; i < attendanceDetail?.Count; i++)
                        {
                            if (i < (attendanceDetail?.Count - 1))
                            {
                                //attendanceDetail[i].BreakoutTime = attendanceDetail[i + 1].CheckinTime;
                                TimeSpan? breakHours = attendanceDetail[i + 1].CheckinTime - attendanceDetail[i].CheckoutTime;
                                totalBreakHour.Add(breakHours == null ? new TimeSpan(0) : breakHours);
                            }
                        }
                        breakHour.Add(new TimeSpan(totalBreakHour.Sum(r => r == null ? 0 : r.Value.Ticks)));
                    }
                    EmployeesAttendanceDetails employeesAttendance = new();
                    employeesAttendance.EmployeeId = item;
                    //employeesAttendance.TotalHours = new TimeSpan(totalHour.Sum(r => r == null ? 0 : r.Value.Ticks)).ToString();
                    //employeesAttendance.BreakHours = new TimeSpan(breakHour.Sum(r => r == null ? 0 : r.Value.Ticks)).ToString();

                    TimeSpan? TotalHours = TimeSpan.FromTicks(totalHour.Sum(r => r == null ? 0 : r.Value.Ticks));
                    employeesAttendance.TotalHours = TotalHours == null ? "00:00:00" : ((int)TotalHours.Value.TotalHours).ToString().PadLeft(2, '0') + ":" + TotalHours.Value.Minutes.ToString().PadLeft(2, '0') + ":" + TotalHours.Value.Seconds.ToString().PadLeft(2, '0');
                    TimeSpan? BreakHours = new TimeSpan(breakHour.Sum(r => r == null ? 0 : r.Value.Ticks));
                    employeesAttendance.BreakHours = BreakHours == null ? "00:00:00" : ((int)BreakHours.Value.TotalHours).ToString().PadLeft(2, '0') + ":" + BreakHours.Value.Minutes.ToString().PadLeft(2, '0') + ":" + BreakHours.Value.Seconds.ToString().PadLeft(2, '0');
                    employeesAttendanceDetails?.Add(employeesAttendance);
                }
            }            
            return employeesAttendanceDetails;
        }
        public bool GetAttendanceDetailByAttendanceId(int attendanceId, DateTime fromDate, DateTime toDate, int attendanceDetailId, bool isEdit)
        {
            bool isExistsAttendance = false;
            List<AttendanceDetail> attendanceDetail = new List<AttendanceDetail>();
            if (isEdit)
            {
                attendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceId && x.AttendanceDetailId!= attendanceDetailId && x.Status.ToLower() != "cancelled" && x.Status.ToLower() != "rejected" && x.CheckinTime != null && x.CheckoutTime != null).ToList();
            }
            else
            {
                attendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceId && x.Status.ToLower() != "cancelled" && x.Status.ToLower() != "rejected" && x.CheckinTime != null && x.CheckoutTime != null).ToList();
            }
            foreach (AttendanceDetail attendance in attendanceDetail)
            {
                if(attendance?.CheckinTime<= fromDate && attendance?.CheckoutTime>= fromDate)
                {
                    isExistsAttendance = true;
                    break;
                }
                if (attendance?.CheckinTime <= toDate && attendance?.CheckoutTime >= toDate)
                {
                    isExistsAttendance = true;
                    break;
                }
                if (attendance?.CheckinTime >= fromDate && attendance?.CheckoutTime <= toDate)
                {
                    isExistsAttendance = true;
                    break;
                }
            }
            return isExistsAttendance;
        }
        public AttendanceDetail GetAttendanceDetailByAttendanceDetailId(int attendanceDetailId)
        {
            return dbContext.AttendanceDetail.Where(x => x.AttendanceDetailId == attendanceDetailId).FirstOrDefault();
            
        }
        //public List<AttendanceDetail> GetAttendanceDetailListByAttendanceId(int attendanceId)
        //{
        //    List<AttendanceDetail> attendanceDetail = dbContext.AttendanceDetail.Where(x => x.AttendanceId == attendanceId&&((x.Status==null&&x.Isregularize==null) ||(x.Status.ToLower() =="approved" &&x.Isregularize==true))).OrderBy(x=>x.CheckinTime).ToList();
        //    return attendanceDetail;
        //}
        public List<TeamLeaveView> GetEmployeeRegularizationList(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            return (from at in dbContext.Attendance
                    join ad in dbContext.AttendanceDetail on at.AttendanceId equals ad.AttendanceId
                    //where managerTeamLeaveView.ResourceId.Contains(at.EmployeeId) && at.Date >= managerTeamLeaveView.FromDate && at.Date <= managerTeamLeaveView.ToDate && ad.Isregularize == true
                    //&& ad.Status != true
                    where ad.ManagerId == (managerTeamLeaveView.ManagerId == 0 ? ad.ManagerId : managerTeamLeaveView.ManagerId) && at.Date >= managerTeamLeaveView.FromDate && at.Date <= managerTeamLeaveView.ToDate && ad.Isregularize == true
                    && (decimal)((at.Date.Subtract(at.Date)).TotalDays + 1) == (managerTeamLeaveView.NoOfDays == 0 ? (decimal)((at.Date.Subtract(at.Date)).TotalDays + 1) : managerTeamLeaveView.NoOfDays)
                    && ad.Status == (string.IsNullOrEmpty(managerTeamLeaveView.LeaveStatus) ? ad.Status : managerTeamLeaveView.LeaveStatus)
                    select new TeamLeaveView
                    {
                        EmployeeId = at.EmployeeId,
                        AttendanceDetailsId = ad.AttendanceDetailId,
                        FromDate = at.Date,
                        ToDate = at.Date,
                        NoOfDays = (decimal)((at.Date.Subtract(at.Date)).TotalDays + 1),
                        Reason = ad.Reason,
                        IsRegularize = ad.Isregularize,
                        Status = ad.Status,
                        FromTime = ad.CheckinTime,
                        ToTime = ad.CheckoutTime,
                        LeaveType = "Regularization",
                        Feedback = ad.RejectReason,
                        ManagerId = ad.ManagerId,
                        CreatedOn = ad.CreatedOn
                    }).OrderByDescending(x => x.CreatedOn).Skip(managerTeamLeaveView.NoOfRecord * (managerTeamLeaveView.PageNumber)).Take(managerTeamLeaveView.NoOfRecord).ToList();
        }
        public List<ApplyLeavesView> GetEmployeeRegularizationById(int employeeId, DateTime FromDate, DateTime ToDate)
        {
            return (from at in dbContext.Attendance
                    join ad in dbContext.AttendanceDetail on at.AttendanceId equals ad.AttendanceId
                    where  at.EmployeeId == employeeId && at.Date >= FromDate && at.Date <= ToDate && ad.Isregularize == true
                    //&& ad.Status != true
                    select new ApplyLeavesView
                    {
                        EmployeeId = at.EmployeeId,
                        AttendaceId = at.AttendanceId,
                        AttendanceDetailsId = ad.AttendanceDetailId,
                        FromDate = at.Date,
                        ToDate = at.Date,
                        NoOfDays = (decimal)((at.Date.Subtract(at.Date)).TotalDays + 1),
                        Reason = ad.Reason,
                        IsRegularize = ad.Isregularize,
                        Status = ad.Status,
                        FromTime = ad.CheckinTime,
                        ToTime = ad.CheckoutTime,
                        LeaveType = "Regularization",
                        Feedback = ad.RejectReason,
                        CreatedOn = ad.CreatedOn
                    }).ToList();
        }
        public TimeSpan? GetAttendanceDetailByEmployeeId(int employeeId, DateTime Date, int attendnaceDetailId)
        {
            List<TimeSpan?> totalHour = new List<TimeSpan?>();
            if(attendnaceDetailId > 0)
            {
                totalHour= dbContext.Attendance.Join(dbContext.AttendanceDetail, attend => attend.AttendanceId, ad => ad.AttendanceId, (attend, ad) => new { attend, ad }).
                Where(r => r.attend.Date.Date == Date.Date && r.attend.EmployeeId == employeeId && r.ad.Status.ToLower() != "rejected" && r.ad.Status.ToLower() != "cancelled"
                && r.ad.AttendanceDetailId != attendnaceDetailId).Select(r => r.ad.TotalHours).ToList();
            }
            else
            {
                totalHour= dbContext.Attendance.Join(dbContext.AttendanceDetail, attend => attend.AttendanceId, ad => ad.AttendanceId, (attend, ad) => new { attend, ad }).
                Where(r => r.attend.Date.Date == Date.Date && r.attend.EmployeeId == employeeId && r.ad.Status.ToLower() != "rejected" && r.ad.Status.ToLower() != "cancelled").Select(r => r.ad.TotalHours).ToList();
            }
            
            return new TimeSpan(totalHour.Sum(r => r == null ? 0 : r.Value.Ticks));
        }
        public EmployeeShiftDetailView GetShiftDetails(int ShiftId)
        {
            EmployeeShiftDetailView shiftvsl = new EmployeeShiftDetailView();

            shiftvsl.HalfDay = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == ShiftId).Select(rs => new HalfDay
            {
                FromValue = rs.td.HalfaDayFromHour.ToString(),
                OperatorOne = rs.td.HalfaDayFromOperator,
                ToValue = rs.td.HalfaDayToHour.ToString(),
                Operatortwo = rs.td.HalfaDayToOperator,
            }).FirstOrDefault();
            shiftvsl.FullDay = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == ShiftId).Select(rs => new HalfDay
            {
                FromValue = rs.td.PresentHour.ToString(),
                OperatorOne = rs.td.PresentOperator
            }).FirstOrDefault();
            shiftvsl.Absent = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == ShiftId).Select(rs => new HalfDay
            {
                FromValue = rs.td.AbsentFromHour.ToString(),
                OperatorOne = rs.td.AbsentFromOperator,
                ToValue = rs.td.AbsentToHour.ToString(),
                Operatortwo = rs.td.AbsentToOperator,
            }).FirstOrDefault();
            shiftvsl.IsActive = dbContext.ShiftDetails.Where(x => x.ShiftDetailsId == ShiftId).Select(x => x.IsActive).FirstOrDefault();
            return shiftvsl == null ? new EmployeeShiftDetailView() : shiftvsl;
        }

        public RegularizationDetailView GetEmployeeRegularizationByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            RegularizationDetailView regularizationDetail = new RegularizationDetailView();
            FilterData filterData = employeeLeaveandRestriction.FilterData;
          if (employeeLeaveandRestriction.PageNumber == 0)
          {
                regularizationDetail.TotalCount = (from at in dbContext.Attendance
                                                   join ad in dbContext.AttendanceDetail on at.AttendanceId equals ad.AttendanceId
                                                   where ad.Isregularize == true && at.EmployeeId == employeeLeaveandRestriction.EmployeeId && (
                                                   employeeLeaveandRestriction.isFiltered == false || (
                                                   (filterData.statusList == null || filterData.statusList.Contains(ad.Status) ) && ((filterData.FromDate == null) || (filterData.DateCondition == ">" ? (at.Date <= filterData.FromDate) : filterData.DateCondition == "<" ? (at.Date >= filterData.FromDate) : (at.Date >= filterData.FromDate && at.Date <= filterData.ToDate)))))
                                                   select new WeeklyMonthlyAttendanceDetail
                                                   {
                                                       EmployeeId = at.EmployeeId,
                                                   }).Count();

            }
            if(employeeLeaveandRestriction.isExport == true)
            {
                regularizationDetail.RegularizationDetailList = (from at in dbContext.Attendance
                                                                 join ad in dbContext.AttendanceDetail on at.AttendanceId equals ad.AttendanceId
                                                                 where ad.Isregularize == true && at.EmployeeId == employeeLeaveandRestriction.EmployeeId
                                                                  && (employeeLeaveandRestriction.isFiltered == false || (
                                                                      (filterData.statusList == null || filterData.statusList.Contains(ad.Status)) && ((filterData.FromDate == null) || (filterData.DateCondition == ">" ? (at.Date <= filterData.FromDate) : filterData.DateCondition == "<" ? (at.Date >= filterData.FromDate) : (at.Date >= filterData.FromDate && at.Date <= filterData.ToDate)))))
                                                                 select new WeeklyMonthlyAttendanceDetail
                                                                 {
                                                                     EmployeeId = at.EmployeeId,
                                                                     AttendanceId = at.AttendanceId,
                                                                     AttendanceDetailId = ad.AttendanceDetailId,
                                                                     RegularizationDate = at.Date,
                                                                     Reason = ad.Reason,
                                                                     IsRegularize = ad.Isregularize,
                                                                     Status = ad.Status,
                                                                     CheckinTime = ad.CheckinTime,
                                                                     CheckoutTime = ad.CheckoutTime,
                                                                     TotalTime = ad.TotalHours,
                                                                     LeaveType = "Regularization",
                                                                     Feedback = ad.RejectReason,
                                                                     //ManagerId = ad.ManagerId,
                                                                     CreatedOn = ad.CreatedOn
                                                                 }).OrderByDescending(x => x.CreatedOn).ToList();

            }
            else
            {
                regularizationDetail.RegularizationDetailList = (from at in dbContext.Attendance
                                                                 join ad in dbContext.AttendanceDetail on at.AttendanceId equals ad.AttendanceId
                                                                 where ad.Isregularize == true && at.EmployeeId == employeeLeaveandRestriction.EmployeeId
                                                                  && (employeeLeaveandRestriction.isFiltered == false || (
                                                                      (filterData.statusList == null || filterData.statusList.Contains(ad.Status)) && ((filterData.FromDate == null) || (filterData.DateCondition == ">" ? (at.Date <= filterData.FromDate) : filterData.DateCondition == "<" ? (at.Date >= filterData.FromDate) : (at.Date >= filterData.FromDate && at.Date <= filterData.ToDate)))))
                                                                 select new WeeklyMonthlyAttendanceDetail
                                                                 {
                                                                     EmployeeId = at.EmployeeId,
                                                                     AttendanceId = at.AttendanceId,
                                                                     AttendanceDetailId = ad.AttendanceDetailId,
                                                                     RegularizationDate = at.Date,
                                                                     Reason = ad.Reason,
                                                                     IsRegularize = ad.Isregularize,
                                                                     Status = ad.Status,
                                                                     CheckinTime = ad.CheckinTime,
                                                                     CheckoutTime = ad.CheckoutTime,
                                                                     TotalTime = ad.TotalHours,
                                                                     LeaveType = "Regularization",
                                                                     Feedback = ad.RejectReason,
                                                                     //ManagerId = ad.ManagerId,
                                                                     CreatedOn = ad.CreatedOn
                                                                 }).OrderByDescending(x => x.CreatedOn).Skip(employeeLeaveandRestriction.NoOfRecord * (employeeLeaveandRestriction.PageNumber)).Take(employeeLeaveandRestriction.NoOfRecord).ToList();

            }

            return regularizationDetail;
        }

        public List<EmployeeRequestCount> GetAttendanceRequestCount(List<int> employeeIdList)
        {

            List<EmployeeRequestCount> employeeRequests = dbContext.Attendance.Join(dbContext.AttendanceDetail, attendance => attendance.AttendanceId, ad => ad.AttendanceId, (attendance, ad) => new { attendance, ad })
                      .Where(rs => employeeIdList.Contains(rs.attendance.EmployeeId) && rs.ad.Status == "Pending" && rs.ad.Isregularize == true).ToList().GroupBy(x => x.attendance.EmployeeId).Select(x => new EmployeeRequestCount
                      {
                          EmployeeId = x.Key,
                          RequestCount = x.Count()
                      }).ToList();
            return employeeRequests;
        }
    }
}