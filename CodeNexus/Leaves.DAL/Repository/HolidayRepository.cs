using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using static SharedLibraries.ViewModels.Leaves.WeeklyOverviewReportView;

namespace Leaves.DAL.Repository
{
    public interface IHolidayRepository : IBaseRepository<Holiday>
    {
        Holiday GetByID(int HolidayId);
        List<HolidayView> GetAllHolidays();
        HolidayDetailView GetByHolidayID(int pHolidayID);
        HolidayDetailsView GetUpcomingHolidays(int departmentId, int locationId, DateTime DOJ, DateTime FromDate, DateTime ToDate);
        List<HolidaysData> GetWeeklyReportHolidays(int employeeId);
        List<Holiday> GetHolidayByDate(int departmentId, DateTime fromDate, DateTime toDate, int locationId, int shiftId);
        List<Holiday> GetCurrentFinancialYearHolidayList(int departmentId, DateTime FromDate, DateTime ToDate, int locationId, int shiftId,DateTime? dateOfJoining);
        string HolidayNameDuplication(string HolidayName, int HolidayID, DateTime HolidayDate, bool IsRestricted);
        Holiday CheckHolidayByDate(int departmentId, DateTime Date, int locationId, int shiftId);
        Holiday GetHolidayDetailByDate(List<int?> activeHolidayId, int departmentId, DateTime holidayDate, int locationId, int shiftId);
        HolidayDetailsView GetHolidayDetails(int departmentId, DateTime FromDate, DateTime ToDate, int locationId, DateTime? dateOfJoining);
    }
    public class HolidayRepository : BaseRepository<Holiday>, IHolidayRepository
    {
        private readonly LeaveDBContext dbContext;
        public HolidayRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public Holiday GetByID(int holidayId)
        {
            return dbContext.Holiday.Where(x => x.HolidayID == holidayId).FirstOrDefault();
        }
        public List<HolidayView> GetAllHolidays()
        {
            List<HolidayView> holidayViews = (from holiday in dbContext.Holiday
                                              select new HolidayView
                                              {
                                                  HolidayID = holiday.HolidayID,
                                                  HolidayName = holiday.HolidayName,
                                                  HolidayDate = holiday.HolidayDate,
                                                  Year = holiday.Year,
                                                  EmployeeShiftId = dbContext.HolidayShift.Where(x => x.HolidayId == holiday.HolidayID).Select(x => x.ShiftDetailsId == null ? 0 : (int)x.ShiftDetailsId).ToList(),
                                                  EmployeeDepartmentId = dbContext.HolidayDepartment.Where(x => x.HolidayId == holiday.HolidayID).Select(x => x.DepartmentId == null ? 0 : (int)x.DepartmentId).ToList(),
                                                  EmployeeLocationId = dbContext.HolidayLocation.Where(x => x.HolidayId == holiday.HolidayID).Select(x => x.LocationId == null ? 0 : (int)x.LocationId).ToList(),
                                                  CreatedOn = holiday.CreatedOn,
                                                  IsActive = holiday.IsActive
                                              }).OrderByDescending(x => x.CreatedOn).ToList();
            return holidayViews;
        }
        public HolidayDetailView GetByHolidayID(int pHolidayID)
        {
            Holiday holiday = dbContext.Holiday.Where(x => x.HolidayID == pHolidayID).FirstOrDefault();
            if (holiday != null && holiday?.HolidayID > 0)
            {
                HolidayDetailView holidayDetailView = new();
                {
                    holidayDetailView.HolidayID = holiday.HolidayID;
                    holidayDetailView.Year = holiday.Year;
                    holidayDetailView.HolidayName = holiday.HolidayName;
                    holidayDetailView.HolidayCode = holiday.HolidayCode;
                    holidayDetailView.HolidayDescription = holiday.HolidayDescription;
                    holidayDetailView.IsActive = holiday.IsActive;
                    holidayDetailView.HolidayDate = holiday.HolidayDate;
                    holidayDetailView.IsRestrictHoliday = holiday.IsRestrictHoliday;
                };
                return holidayDetailView;
            }
            return null;
        }
        public HolidayDetailsView GetUpcomingHolidays(int departmentId, int locationId, DateTime DOJ,DateTime FromDate, DateTime ToDate)
        {
            HolidayDetailsView holidayDetails = new();
            DateTime startDate = FromDate.AddYears(-1);
            holidayDetails.HolidayList = (from holiday in dbContext.Holiday
                                         where holiday.HolidayDate.Value.Date >= startDate.Date && holiday.HolidayDate.Value.Date <= ToDate.Date && holiday.IsActive == true
                                         && holiday.HolidayDate.Value.Date>=DOJ.Date
                                          select new Holiday
                                         {
                                             HolidayID = holiday.HolidayID,
                                             HolidayName = holiday.HolidayName,
                                             HolidayDate = holiday.HolidayDate,
                                             HolidayDescription = holiday.HolidayDescription,
                                             Year = holiday.Year,
                                             IsRestrictHoliday = holiday.IsRestrictHoliday
                                          }).OrderBy(x => x.HolidayDate).ToList();
            holidayDetails.HolidayLocation = (from location in dbContext.HolidayLocation
                                              join holiday in dbContext.Holiday on location.HolidayId equals holiday.HolidayID
                                              where holiday.IsActive == true
                                              select new HolidayLocation
                                              {
                                                  HolidayId = location.HolidayId,
                                                  LocationId = location.LocationId,
                                              }).Distinct().ToList();
            holidayDetails.HolidayShift = (from shift in dbContext.HolidayShift
                                           join holiday in dbContext.Holiday on shift.HolidayId equals holiday.HolidayID
                                           where holiday.IsActive == true
                                           select new HolidayShift
                                           {
                                               HolidayId = shift.HolidayId,
                                               ShiftDetailsId = (int)shift.ShiftDetailsId,
                                           }).Distinct().ToList();
            holidayDetails.HolidayDepartment = (from department in dbContext.HolidayDepartment
                                                join holiday in dbContext.Holiday on department.HolidayId equals holiday.HolidayID
                                                where holiday.IsActive == true
                                                select new HolidayDepartment
                                                {
                                                    HolidayId = department.HolidayId,
                                                    DepartmentId = department.DepartmentId,
                                                }).Distinct().ToList();
            return holidayDetails;
        }
        public List<HolidaysData> GetWeeklyReportHolidays(int employeeId)
        {
            return (from holiday in dbContext.Holiday.Where(x => x.IsActive == true)
                    select new HolidaysData
                    {
                        HolidayDate = holiday.HolidayDate,
                        Year = holiday.Year
                    }).ToList();
        }
        public List<Holiday> GetHolidayByDate(int departmentId, DateTime fromDate, DateTime toDate, int locationId, int shiftId)
        {
            List<Holiday> holidayList = (from holiday in dbContext.Holiday
                                         where holiday.HolidayDate.Value.Date >= fromDate && holiday.HolidayDate.Value.Date <= toDate && holiday.IsActive == true
                                         select new Holiday
                                         {
                                             HolidayDate = holiday.HolidayDate,
                                             HolidayID = holiday.HolidayID,
                                             Year = holiday.Year,
                                             HolidayName = holiday.HolidayName,
                                             IsRestrictHoliday = holiday.IsRestrictHoliday
                                         }).Distinct().ToList();
            return getApplicableHolidayList(holidayList, departmentId, locationId, shiftId);
        }
        public List<Holiday> getApplicableHolidayList(List<Holiday> holidayList, int departmentId, int locationId, int shiftId)
        {
            List<Holiday> empHolidayList = new List<Holiday>();
            if (holidayList?.Count > 0)
            {
                foreach (Holiday holiday in holidayList)
                {
                    bool applicable = false;
                    //Check department
                    var depList = dbContext.HolidayDepartment.Where(x => x.HolidayId == holiday.HolidayID).Select(x => x.DepartmentId).ToList();
                    if (depList?.Count > 0)
                    {
                        if (depList.Contains(departmentId))
                        {
                            applicable = true;
                        }
                        else
                        {
                            applicable = false;
                        }
                    }
                    else
                    {
                        applicable = true;
                    }
                    //Check shift
                    if (applicable)
                    {
                        var shiftList = dbContext.HolidayShift.Where(x => x.HolidayId == holiday.HolidayID).Select(x => x.ShiftDetailsId).ToList();
                        if (shiftList?.Count > 0)
                        {
                            if (shiftList.Contains(shiftId))
                            {
                                applicable = true;
                            }
                            else
                            {
                                applicable = false;
                            }
                        }
                        else
                        {
                            applicable = true;
                        }
                        //check location
                        if (applicable)
                        {                            
                            var locationList = dbContext.HolidayLocation.Where(x => x.HolidayId == holiday.HolidayID).Select(x => x.LocationId).ToList();
                            if (locationList?.Count > 0)
                            {
                                if (locationList.Contains(locationId))
                                {
                                    applicable = true;
                                }
                                else
                                {
                                    applicable = false;
                                }
                            }
                            else
                            {
                                applicable = true;
                            }
                        }

                    }


                    if (applicable)
                    {
                        empHolidayList.Add(holiday);
                    }

                }
            }
            return empHolidayList;
        }
        #region Get Current Financial year Holiday List By Holid Id
        public List<Holiday> GetCurrentFinancialYearHolidayList(int departmentId, DateTime FromDate, DateTime ToDate, int locationId, int shiftId, DateTime? dateOfJoining)
        {
            List<Holiday> holidayList = (from holiday in dbContext.Holiday
                                         where holiday.HolidayDate.Value.Date >= FromDate && holiday.HolidayDate.Value.Date <= ToDate && holiday.IsActive == true &&
                                         holiday.HolidayDate.Value.Date>= dateOfJoining.Value.Date
                                         select new Holiday
                                         {
                                             HolidayID = holiday.HolidayID,
                                             Year = holiday.Year,
                                             HolidayName = holiday.HolidayName,
                                             HolidayCode = holiday.HolidayCode,
                                             HolidayDescription = holiday.HolidayDescription,
                                             HolidayDate = holiday.HolidayDate,
                                             IsActive = holiday.IsActive,
                                             IsRestrictHoliday = holiday.IsRestrictHoliday
                                         }).Distinct().ToList();
            return getApplicableHolidayList(holidayList, departmentId, locationId, shiftId);

            //List<HolidayViewDetails> holidayList = dbContext.Holiday.Where(x => x.HolidayDate >= FromDate && x.HolidayDate <= ToDate && x.IsActive == true)
            //    .Select(x => new HolidayViewDetails
            //    {
            //        HolidayID = x.HolidayID,
            //        Year = x.Year,
            //        HolidayName = x.HolidayName,
            //        HolidayCode = x.HolidayCode,
            //        HolidayDescription = x.HolidayDescription,
            //        HolidayDate = x.HolidayDate,
            //        IsActive = x.IsActive,
            //        IsRestrictHoliday = x.IsRestrictHoliday
            //    }).ToList();
            //return holidayList;
        }
        #endregion
        public string HolidayNameDuplication(string HolidayName, int HolidayID, DateTime HolidayDate , bool IsRestricted)
        {
            //bool isDuplicateName = false;
            string isDuplicateName = "nameDuplication";
            string isDuplicateDate = "dateDuplication";
            string existHolidayName = dbContext.Holiday.Where(x => x.HolidayName.ToLower() == HolidayName.ToLower() && (x.HolidayID == HolidayID || HolidayID == 0) && x.Year == HolidayDate.Year && x.IsRestrictHoliday == IsRestricted).Select(x => x.HolidayName).FirstOrDefault();
            if (HolidayID == 0 && existHolidayName != null)
            {
                return isDuplicateName;
            }
            if (HolidayID != 0 && existHolidayName?.ToLower() != HolidayName?.ToLower())
            {
                string newHolidayName = dbContext.Holiday.Where(x => x.HolidayName.ToLower() == HolidayName.ToLower() && x.Year == HolidayDate.Year && x.IsRestrictHoliday == IsRestricted).Select(x => x.HolidayName).FirstOrDefault();
                if (newHolidayName != null)
                {
                    return isDuplicateName;
                }
            }
            DateTime? existHolidayDate = dbContext.Holiday.Where(x => x.HolidayDate.Value.Date == HolidayDate.Date && (x.HolidayID == HolidayID || HolidayID == 0) && x.IsRestrictHoliday == IsRestricted).Select(x => x.HolidayDate).FirstOrDefault();
            if (HolidayID == 0 && existHolidayDate != null)
            {
                return isDuplicateDate;
            }
            if (HolidayID != 0 && existHolidayDate?.Date != HolidayDate.Date)
            {
                DateTime? newHolidaydate = dbContext.Holiday.Where(x => x.HolidayDate.Value.Date == HolidayDate.Date && x.IsRestrictHoliday == IsRestricted).Select(x => x.HolidayDate).FirstOrDefault();
                if (newHolidaydate != null)
                {
                    return isDuplicateDate;
                }
            }
            return string.Empty;
        }
        public Holiday CheckHolidayByDate(int departmentId, DateTime Date,int locationId, int shiftId)
        {
            List<Holiday> holidayList = (from holiday in dbContext.Holiday
                                         where holiday.HolidayDate.Value.Date == Date && holiday.IsActive == true && holiday.IsRestrictHoliday==false
                                         select new Holiday
                                         {
                                             HolidayDate = holiday.HolidayDate,
                                             HolidayID = holiday.HolidayID,
                                             Year = holiday.Year,
                                             HolidayName = holiday.HolidayName
                                         }).Distinct().ToList();
            return getApplicableHolidayList(holidayList, departmentId, locationId, shiftId).FirstOrDefault();
           
            //return (from holiday in dbContext.Holiday
            //        join department in dbContext.HolidayDepartment on holiday.HolidayID equals department.HolidayId into departmentList
            //        from department in departmentList.DefaultIfEmpty()
            //        join location in dbContext.HolidayLocation on holiday.HolidayID equals location.HolidayId into locationList
            //        from location in locationList.DefaultIfEmpty()
            //        join shift in dbContext.HolidayShift on holiday.HolidayID equals shift.HolidayId into shiftList
            //        from shift in shiftList.DefaultIfEmpty()
            //        where ((department.DepartmentId == departmentId || department.DepartmentId == null) && (location.LocationId == locationId || location.LocationId == null) && (shift.ShiftDetailsId == shiftId || shift.ShiftDetailsId == null))
            //        && holiday.HolidayDate.Value.Date == Date && holiday.IsActive == true
            //        select new Holiday
            //        {
            //            HolidayDate = holiday.HolidayDate,
            //            HolidayID=holiday.HolidayID,
            //            Year = holiday.Year,
            //            HolidayName = holiday.HolidayName
            //        }).FirstOrDefault();
        }
        public Holiday GetHolidayDetailByDate(List<int?> activeHolidayId,int departmentId, DateTime holidayDate, int locationId, int shiftId)
        {
            List<Holiday> holidayList = (from holiday in dbContext.Holiday
                                         where holiday.HolidayDate.Value.Date == holidayDate && activeHolidayId.Contains(holiday.HolidayID) && holiday.IsActive == true
                                         select new Holiday
                                         {
                                             HolidayDate = holiday.HolidayDate,
                                             Year = holiday.Year,
                                             HolidayName = holiday.HolidayName
                                         }).Distinct().ToList();
            return getApplicableHolidayList(holidayList, departmentId, locationId, shiftId).FirstOrDefault();

            //return (from holiday in dbContext.Holiday
            //        join department in dbContext.HolidayDepartment on holiday.HolidayID equals department.HolidayId into departmentList
            //        from department in departmentList.DefaultIfEmpty()
            //        join location in dbContext.HolidayLocation on holiday.HolidayID equals location.HolidayId into locationList
            //        from location in locationList.DefaultIfEmpty()
            //        join shift in dbContext.HolidayShift on holiday.HolidayID equals shift.HolidayId into shiftList
            //        from shift in shiftList.DefaultIfEmpty()
            //        where ((department.DepartmentId == departmentId || department.DepartmentId == null) && (location.LocationId == locationId || location.LocationId == null) && (shift.ShiftDetailsId == shiftId || shift.ShiftDetailsId == null))
            //        && holiday.HolidayDate.Value.Date == holidayDate && activeHolidayId.Contains(holiday.HolidayID) && holiday.IsActive == true
            //        select new Holiday
            //        {
            //            HolidayDate = holiday.HolidayDate,
            //            Year = holiday.Year,
            //            HolidayName = holiday.HolidayName
            //        }).Distinct().FirstOrDefault();
        }
        public HolidayDetailsView GetHolidayDetails(int departmentId, DateTime FromDate, DateTime ToDate, int locationId, DateTime? dateOfJoining)
        {
            HolidayDetailsView holidayDetails = new();
            holidayDetails.HolidayList = (from holiday in dbContext.Holiday
                                          where holiday.HolidayDate.Value.Date >= FromDate && holiday.HolidayDate.Value.Date <= ToDate && holiday.IsActive == true &&
                                          holiday.HolidayDate.Value.Date >= dateOfJoining.Value.Date
                                          select new Holiday
                                          {
                                              HolidayID = holiday.HolidayID,
                                              Year = holiday.Year,
                                              HolidayName = holiday.HolidayName,
                                              HolidayCode = holiday.HolidayCode,
                                              HolidayDescription = holiday.HolidayDescription,
                                              HolidayDate = holiday.HolidayDate,
                                              IsActive = holiday.IsActive,
                                              IsRestrictHoliday = holiday.IsRestrictHoliday
                                          }).Distinct().ToList();
            holidayDetails.HolidayLocation = (from location in dbContext.HolidayLocation
                                              join holiday in dbContext.Holiday on location.HolidayId equals holiday.HolidayID
                                              where holiday.IsActive == true
                                              select new HolidayLocation
                                              {
                                                  HolidayId = location.HolidayId,
                                                  LocationId = location.LocationId,
                                              }).Distinct().ToList();
            holidayDetails.HolidayShift = (from shift in dbContext.HolidayShift
                                           join holiday in dbContext.Holiday on shift.HolidayId equals holiday.HolidayID
                                           where holiday.IsActive == true
                                           select new HolidayShift
                                           {
                                               HolidayId = shift.HolidayId,
                                               ShiftDetailsId = (int)shift.ShiftDetailsId,
                                           }).Distinct().ToList();
            holidayDetails.HolidayDepartment = (from department in dbContext.HolidayDepartment
                                                join holiday in dbContext.Holiday on department.HolidayId equals holiday.HolidayID
                                                where holiday.IsActive == true
                                                select new HolidayDepartment
                                                {
                                                    HolidayId = department.HolidayId,
                                                    DepartmentId = department.DepartmentId,
                                                }).Distinct().ToList();
            return holidayDetails;
        }

    }
}