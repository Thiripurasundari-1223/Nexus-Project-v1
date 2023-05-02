using Attendance.DAL.DBContext;
using SharedLibraries.Common;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Attendance.DAL.Repository
{
    public interface IShiftDetailsRepository : IBaseRepository<ShiftDetails>
    {
        ShiftDetails GetByID(int shiftDetailsId);
        List<ShiftView> GetAllShiftDetails();
        AttendanceShiftDetailsView GetByShiftDetailsId(int pShiftDetailsId);
        List<KeyWithValue> GetShiftNameById(List<int> shiftId);
        List<KeyWithValue> GetAllShiftName();
        List<EmployeeShiftMasterData> GetEmployeeShiftMasterData();
        int GetShiftIdByName(string pShiftName);
        EmployeeShiftDetailsView GetDefaultShiftId();
        EmployeeShiftDetailsView GetDefaultShiftDetailsById(int shiftId);
        List<EmployeeShiftDetailsView> GetShiftDetails();
        EmployeeShiftDetailsView GetDefaultShiftDetails();
        List<ShiftViewDetails> GetShiftWeekendDetailsList();
        List<ShiftView> GetAllActiveShift();
    }
    public class ShiftDetailsRepository : BaseRepository<ShiftDetails>, IShiftDetailsRepository
    {
        private readonly AttendanceDBContext dbContext;
        public ShiftDetailsRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public ShiftDetails GetByID(int shiftDetailsId)
        {
            return dbContext.ShiftDetails.Where(x => x.ShiftDetailsId == shiftDetailsId).FirstOrDefault();
        }
        public List<ShiftView> GetAllShiftDetails()
        {
            return (from shiftDetails in dbContext.ShiftDetails
                    select new ShiftView
                    {
                        ShiftDetailsId = shiftDetails.ShiftDetailsId,
                        ShiftName = shiftDetails.ShiftName,
                        ShiftCode = shiftDetails.ShiftCode,
                        TimeFrom =  ConvertingAsString(shiftDetails.TimeFrom),
                        TimeTo = ConvertingAsString(shiftDetails.TimeTo),
                        Status = shiftDetails.IsActive == true ? "Active" : "Inactive",
                        CreatedOn = shiftDetails.CreatedOn,
                        WeekendId= dbContext.ShiftWeekendDefinition.Where(x => x.ShiftDetailsId == shiftDetails.ShiftDetailsId).Select(x => x.WeekendDayId).ToList()
                    }).OrderByDescending(x => x.CreatedOn).ToList();
        }
        private static string ConvertingAsString(TimeSpan? timeSpan)
        {
            string returnValue = "00:00";
            if (timeSpan != null)
                returnValue = timeSpan.Value.Hours.ToString().PadLeft(2, '0') + ":" + timeSpan.Value.Minutes.ToString().PadLeft(2, '0');
            return returnValue;
        }
        public AttendanceShiftDetailsView GetByShiftDetailsId(int pShiftDetailsId)
        {
            ShiftDetails shiftDetails = dbContext.ShiftDetails.Where(x => x.ShiftDetailsId == pShiftDetailsId).FirstOrDefault();
            if (shiftDetails != null)
            {
                AttendanceShiftDetailsView attendanceShiftDetailsView = new AttendanceShiftDetailsView
                {
                    ShiftDetailsId = shiftDetails.ShiftDetailsId,
                    ShiftName = shiftDetails.ShiftName,
                    ShiftCode = shiftDetails.ShiftCode,
                    TimeFrom = shiftDetails.TimeFrom == null ? "00" : shiftDetails.TimeFrom.ToString().PadLeft(2, '0'),
                    TimeTo = shiftDetails.TimeTo == null ? "00" : shiftDetails.TimeTo.ToString().PadLeft(2, '0'),
                    ShiftDescription = shiftDetails.ShiftDescription,
                    //EmployeeGroupId = shiftDetails.EmployeeGroupId,
                    OverTime = shiftDetails.OverTime,
                    IsActive = shiftDetails.IsActive == null ? false : (bool)shiftDetails.IsActive,
                    IsFlexyShift = shiftDetails.IsFlexyShift == null ? false : (bool)shiftDetails.IsFlexyShift,
                };
                return attendanceShiftDetailsView;
            }
            return null;
        }
        public List<KeyWithValue> GetShiftNameById(List<int> shiftId)
        {
            return dbContext.ShiftDetails.Where(x => shiftId.Contains(x.ShiftDetailsId)).Select(y => new
                            KeyWithValue
            { Key = y.ShiftDetailsId, Value = y.ShiftName }).ToList();
        }
        public List<KeyWithValue> GetAllShiftName()
        {
            return dbContext.ShiftDetails.Select(y => new
                            KeyWithValue
            { Key = y.ShiftDetailsId, Value = y.ShiftName }).ToList();
        }
        public List<EmployeeShiftMasterData> GetEmployeeShiftMasterData()
        {
            return dbContext.ShiftDetails.Where(x => x.IsActive == true).Select(y => new EmployeeShiftMasterData { ShiftDetailsId = y.ShiftDetailsId, ShiftName = y.ShiftName }).ToList();
        }
        public int GetShiftIdByName(string pShiftName)
        {
            return dbContext.ShiftDetails.Where(x => x.ShiftName == pShiftName).Select(x => x.ShiftDetailsId).FirstOrDefault();
        }
        public EmployeeShiftDetailsView GetDefaultShiftId()
        {
            EmployeeShiftDetailsView employeeShiftDetails = dbContext.ShiftDetails.Where(x => x.ShiftName.ToLower() == "general shift").Select(y => new EmployeeShiftDetailsView { ShiftDetailsId = y.ShiftDetailsId }).FirstOrDefault();
            if(employeeShiftDetails != null)
            {
                return employeeShiftDetails;
            }
            else
            {
                return employeeShiftDetails = dbContext.ShiftDetails.Select(y => new EmployeeShiftDetailsView { ShiftDetailsId = y.ShiftDetailsId }).FirstOrDefault();
            }
        }
        public EmployeeShiftDetailsView GetDefaultShiftDetailsById(int shiftId)
        {
            EmployeeShiftDetailsView shiftDetails = new();
            if(shiftId > 0)
            {
                shiftDetails= dbContext.ShiftDetails.Where(x=>x.ShiftDetailsId== shiftId).Select(y => new EmployeeShiftDetailsView { ShiftDetailsId = y.ShiftDetailsId,ShiftFromTime=y.TimeFrom.ToString(), ShiftToTime=y.TimeTo.ToString(), IsFlexyShift=y.IsFlexyShift }).FirstOrDefault();
            }
            else
            {
                EmployeeShiftDetailsView employeeShiftDetails = dbContext.ShiftDetails.Where(x => x.ShiftName.ToLower() == "general shift").Select(y => new EmployeeShiftDetailsView { ShiftDetailsId = y.ShiftDetailsId, ShiftFromTime = y.TimeFrom.ToString(), ShiftToTime = y.TimeTo.ToString(), IsFlexyShift = y.IsFlexyShift }).FirstOrDefault();
                if (employeeShiftDetails != null)
                {
                    shiftDetails = employeeShiftDetails;
                }
                else
                {
                    shiftDetails = dbContext.ShiftDetails.Select(y => new EmployeeShiftDetailsView { ShiftDetailsId = y.ShiftDetailsId,ShiftFromTime = y.TimeFrom.ToString(), ShiftToTime = y.TimeTo.ToString(), IsFlexyShift = y.IsFlexyShift }).FirstOrDefault();
                }
            }
            if(shiftDetails != null)
            {
                shiftDetails.WeekendId = dbContext.ShiftWeekendDefinition.Where(x => x.ShiftDetailsId == shiftDetails.ShiftDetailsId).Select(x => new WeekendDetails { WeekendId = x.WeekendDayId }).ToList();
            }
            return shiftDetails;


        }
        public List<EmployeeShiftDetailsView> GetShiftDetails()
        {
            List<EmployeeShiftDetailsView> shiftDetailsList = new();
            EmployeeShiftDetailsView shiftDetails = new();
            List<ShiftDetails> pShiftDetailsId = dbContext.ShiftDetails.Where(x => x.IsActive == true).Select(x => x).ToList();
            foreach (var item in pShiftDetailsId)
            {
                shiftDetails = dbContext.ShiftDetails.Where(x => x.ShiftDetailsId == item.ShiftDetailsId).Select(y => new EmployeeShiftDetailsView { ShiftDetailsId = y.ShiftDetailsId, ShiftFromTime = y.TimeFrom.ToString(), ShiftToTime = y.TimeTo.ToString(),IsFlexyShift=y.IsFlexyShift }).FirstOrDefault();
                if (shiftDetails != null)
                {
                    shiftDetails.WeekendId = dbContext.ShiftWeekendDefinition.Where(x => x.ShiftDetailsId == shiftDetails.ShiftDetailsId).Select(x => new WeekendDetails { WeekendId = x.WeekendDayId }).ToList();
                }
                shiftDetailsList.Add(shiftDetails);
            }
            return shiftDetailsList;
        }
        public EmployeeShiftDetailsView GetDefaultShiftDetails()
        {
            EmployeeShiftDetailsView shiftDetails = new();
            EmployeeShiftDetailsView employeeShiftDetails = dbContext.ShiftDetails.Where(x => x.ShiftName.ToLower() == "general shift").Select(y => new EmployeeShiftDetailsView { ShiftDetailsId = y.ShiftDetailsId, ShiftFromTime = y.TimeFrom.ToString(), ShiftToTime = y.TimeTo.ToString(), IsFlexyShift = y.IsFlexyShift }).FirstOrDefault();
            if (employeeShiftDetails != null)
            {
                shiftDetails = employeeShiftDetails;
            }
            else
            {
                shiftDetails = dbContext.ShiftDetails.Select(y => new EmployeeShiftDetailsView { ShiftDetailsId = y.ShiftDetailsId, ShiftFromTime = y.TimeFrom.ToString(), ShiftToTime = y.TimeTo.ToString(), IsFlexyShift = y.IsFlexyShift }).FirstOrDefault();
            }
            if (shiftDetails != null)
            {
                shiftDetails.WeekendId = dbContext.ShiftWeekendDefinition.Where(x => x.ShiftDetailsId == shiftDetails.ShiftDetailsId).Select(x => new WeekendDetails { WeekendId = x.WeekendDayId }).ToList();
            }
            return shiftDetails;
        }
        public List<ShiftViewDetails> GetShiftWeekendDetailsList()
        {
            List<ShiftViewDetails> shiftViewDetails = new();
            ShiftViewDetails shiftvsl = new();
            List<int> ShiftDetailsId = dbContext.ShiftDetails.Where(x => x.IsActive == true).Select(x => x.ShiftDetailsId).ToList();
            foreach (int item in ShiftDetailsId)
            {
                List<WeekendViewDefinition> listWeekend = new List<WeekendViewDefinition>();
                listWeekend = (from weekend in dbContext.WeekendDefinition
                               join shift in dbContext.ShiftWeekendDefinition on weekend.WeekendDayId equals shift.WeekendDayId
                               where shift.ShiftDetailsId == item
                               select new WeekendViewDefinition
                               {
                                   WeekendDayId = weekend.WeekendDayId,
                                   WeekendDayName = weekend.WeekendDayName,
                                   ShiftDetailsId = shift.ShiftDetailsId,
                               }).ToList();

                shiftvsl = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == item).Select(rs => new ShiftViewDetails
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
                    ShiftTimeTo = rs.sd.TimeTo.ToString(),
                    IsGenralShift = rs.sd.ShiftName.ToLower()=="general shift" ? true : false
                }).FirstOrDefault();
                shiftViewDetails.Add(shiftvsl);
            }
            return shiftViewDetails == null ? new List<ShiftViewDetails>() : shiftViewDetails;
        }
        public List<ShiftView> GetAllActiveShift()
        {
            return dbContext.ShiftDetails.Join(dbContext.TimeDefinition, s => s.ShiftDetailsId, t => t.ShiftDetailsId, (s, t) => new { s, t }).Select(x =>
                       new ShiftView { 
                           ShiftDetailsId = x.s.ShiftDetailsId, 
                           ShiftName = x.s.ShiftName, 
                           TotalHours = x.t.TotalHours.Value.TotalHours.ToString() + ":" + x.t.TotalHours.Value.Minutes.ToString(),
                           WeekEndDays = dbContext.ShiftWeekendDefinition.Where(y=>y.ShiftDetailsId== x.s.ShiftDetailsId).Count()
                       }).ToList();
        }

    }
}
