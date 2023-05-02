using Attendance.DAL.DBContext;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Attendance.DAL.Repository
{
    public interface IAttendanceRepository : IBaseRepository<SharedLibraries.Models.Attendance.Attendance>
    {
        SharedLibraries.Models.Attendance.Attendance GetAttendanceById(int EmployeeId, DateTime attendanceDate);
        HomeReportData GetAttendanceHomeReport();
        SharedLibraries.Models.Attendance.Attendance GetAttendanceDetailById(DateTime FromDate, int EmployeeId);
        List<SharedLibraries.Models.Attendance.Attendance> GetAttendanceDetailByIdAndDate(int employeeId, DateTime fromDate, DateTime toDate);
        List<AttendaceDetails> GetAttendanceDetailByDate(int employeeId, DateTime fromDate, DateTime toDate);
        SharedLibraries.Models.Attendance.Attendance GetAttendanceDetailByAttendaceId(int AttendaceId, int EmployeeId);
        SharedLibraries.Models.Attendance.Attendance GetAttendanceDetailById(int AttendaceId);
        AttendaceDetails GetAttendanceHoursByDate(int employeeId, DateTime fromDate);
    }
    public class AttendanceRepository : BaseRepository<SharedLibraries.Models.Attendance.Attendance>, IAttendanceRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AttendanceRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public SharedLibraries.Models.Attendance.Attendance GetAttendanceById(int EmployeeId, DateTime attendanceDate)
        {
            SharedLibraries.Models.Attendance.Attendance employeeAttendance = dbContext.Attendance.Where(x => x.EmployeeId == EmployeeId && x.Date == attendanceDate.Date).FirstOrDefault();
            if (employeeAttendance != null && employeeAttendance?.AttendanceId > 0)
            {
                return employeeAttendance;
            }
            return null;
        }
        public HomeReportData GetAttendanceHomeReport()
        {
            HomeReportData attendanceReport = new();
            DateTime todayDate = DateTime.Now.Date;
            try
            {
                attendanceReport.ReportTitle = "Checked in";
                attendanceReport.ReportData = (dbContext.Attendance.Where(x => x.Date == todayDate  && x.IsCheckin == true).Select(x => x.EmployeeId).ToList() ?? new List<int>()).Distinct().Count().ToString();
                return attendanceReport;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                attendanceReport = null;
            }
        }
        public SharedLibraries.Models.Attendance.Attendance GetAttendanceDetailById(DateTime FromDate, int EmployeeId)
        {
            SharedLibraries.Models.Attendance.Attendance employeeAttendance = dbContext.Attendance.Where(x =>x.EmployeeId == EmployeeId && x.Date == FromDate).FirstOrDefault();
            if (employeeAttendance != null && employeeAttendance?.AttendanceId > 0)
            {
                return employeeAttendance;
            }
            return null;
        }
        public List<SharedLibraries.Models.Attendance.Attendance> GetAttendanceDetailByIdAndDate(int employeeId, DateTime fromDate,DateTime toDate)
        {
            List<SharedLibraries.Models.Attendance.Attendance> employeeAttendanceList = dbContext.Attendance.Where(x => x.EmployeeId == employeeId && x.Date >= fromDate && x.Date<=toDate).ToList();
            return employeeAttendanceList;
        }
        public List<AttendaceDetails> GetAttendanceDetailByDate(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<AttendaceDetails> WeeklyMonthlyMarkedAttendance = new();
            var dates = new List<DateTime>();
            for (var dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            foreach (var date in dates)
            {
                List<TimeSpan?> totalHour = dbContext.Attendance.Join(dbContext.AttendanceDetail, attend => attend.AttendanceId, ad => ad.AttendanceId, (attend, ad) => new { attend, ad }).Where(r => r.attend.Date == date && r.attend.EmployeeId == employeeId && r.ad.Status.ToLower() != "rejected" && r.ad.Status.ToLower() != "cancelled").Select(r => r.ad.TotalHours).ToList();
                TimeSpan Hour = new TimeSpan(totalHour.Sum(r => r == null ? 0 : r.Value.Ticks));
                DateTime? Checkin = dbContext.Attendance.Join(dbContext.AttendanceDetail, attend => attend.AttendanceId, ad => ad.AttendanceId, (attend, ad) => new { attend, ad }).Where(r => r.attend.Date == date && r.attend.EmployeeId == employeeId && r.ad.Status.ToLower() != "rejected" && r.ad.Status.ToLower() != "cancelled" && r.ad.Status.ToLower() != "pending").OrderBy(r => r.ad.CheckinTime).OrderBy(r => r.ad.CheckinTime).Select(r => r.ad.CheckinTime).FirstOrDefault();
                //DateTime? Checkout = dbContext.Attendance.Join(dbContext.AttendanceDetail, attend => attend.AttendanceId, ad => ad.AttendanceId, (attend, ad) => new { attend, ad }).Where(r => r.attend.Date == date && r.attend.EmployeeId == employeeId && r.ad.Status.ToLower() != "rejected" && r.ad.Status.ToLower() != "cancelled").OrderBy(r => r.ad.CheckoutTime).Select(r => r.ad.CheckoutTime).FirstOrDefault();
                AttendaceDetails Attendance = (from attend in dbContext.Attendance
                                               where attend.EmployeeId == employeeId && attend.Date == date
                                               select new AttendaceDetails
                                               {
                                                   AttendanceId = attend.AttendanceId,
                                                   EmployeeId = attend.EmployeeId,
                                                   Date = attend.Date.Date,
                                                   TotalHours = Hour.ToString(),
                                                   IsCheckin = attend.IsCheckin,
                                                   CheckinTime = Checkin == null ? new DateTime() : Checkin
                                               }).FirstOrDefault();
                WeeklyMonthlyMarkedAttendance?.Add(Attendance);
            }
            return WeeklyMonthlyMarkedAttendance;
        }
        public SharedLibraries.Models.Attendance.Attendance GetAttendanceDetailByAttendaceId(int AttendaceId, int EmployeeId)
        {
            SharedLibraries.Models.Attendance.Attendance employeeAttendance = dbContext.Attendance.Where(x => x.AttendanceId == AttendaceId && x.EmployeeId == EmployeeId).FirstOrDefault();
            if (employeeAttendance != null && employeeAttendance?.AttendanceId > 0)
            {
                return employeeAttendance;
            }
            return null;
        }
        public SharedLibraries.Models.Attendance.Attendance GetAttendanceDetailById(int AttendaceId)
        {
            SharedLibraries.Models.Attendance.Attendance employeeAttendance = dbContext.Attendance.Where(x => x.AttendanceId == AttendaceId).FirstOrDefault();
            if (employeeAttendance != null && employeeAttendance?.AttendanceId > 0)
            {
                return employeeAttendance;
            }
            return null;
        }
        public AttendaceDetails GetAttendanceHoursByDate(int employeeId, DateTime fromDate)
        {
            AttendaceDetails Attendance = new();
            List<TimeSpan?> totalHour = dbContext.Attendance.Join(dbContext.AttendanceDetail, attend => attend.AttendanceId, ad => ad.AttendanceId, (attend, ad) => new { attend, ad }).Where(r => r.attend.Date == fromDate && r.attend.EmployeeId == employeeId && r.ad.Status.ToLower() != "rejected" && r.ad.Status.ToLower() != "cancelled").Select(r => r.ad.TotalHours).ToList();
            TimeSpan Hour = new TimeSpan(totalHour.Sum(r => r == null ? 0 : r.Value.Ticks));
            return Attendance = (from attend in dbContext.Attendance
                                 where attend.EmployeeId == employeeId && attend.Date == fromDate
                                 select new AttendaceDetails
                                 {
                                     AttendanceId = attend.AttendanceId,
                                     EmployeeId = attend.EmployeeId,
                                     Date = attend.Date.Date,
                                     TotalHours = Hour.ToString(),
                                     IsCheckin = attend.IsCheckin,
                                 }).FirstOrDefault();
        }
    }
}