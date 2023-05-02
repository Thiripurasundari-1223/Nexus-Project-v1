using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Attendance.DAL.Repository
{
    public interface IShiftWeekendDefinitionRepository : IBaseRepository<ShiftWeekendDefinition>
    {
        List<ShiftWeekendDefinition> GetWeekendDefinitionByShiftID(int shiftDetailsId);
        ShiftWeekendDefinition GetByShiftWeekendDefinitonId(int shiftDetailsId);
        List<WeekendShiftName> GetShiftWeekendNameByShiftID(int shiftID);
        List<ShiftTimeandWeekendView> GetEmployeeTimeandWeekendbyShiftID();
    }
    public class ShiftWeekendDefinitionRepository : BaseRepository<ShiftWeekendDefinition>, IShiftWeekendDefinitionRepository
    {
        private readonly AttendanceDBContext dbContext;
        public ShiftWeekendDefinitionRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<ShiftWeekendDefinition> GetWeekendDefinitionByShiftID(int shiftDetailsId)
        {
            return dbContext.ShiftWeekendDefinition.Where(x => x.ShiftDetailsId == shiftDetailsId).ToList();
        }
        public ShiftWeekendDefinition GetByShiftWeekendDefinitonId(int shiftDetailsId)
        {
            return dbContext.ShiftWeekendDefinition.Where(x => x.ShiftDetailsId == shiftDetailsId).FirstOrDefault();
        }
        public List<WeekendShiftName> GetShiftWeekendNameByShiftID(int shiftID)
        {
            return (from weekenddef in dbContext.WeekendDefinition 
                    join shiftweek in dbContext.ShiftWeekendDefinition
                    on weekenddef.WeekendDayId equals shiftweek.WeekendDayId
                    where shiftweek.ShiftDetailsId==shiftID
                    select new WeekendShiftName
                    {
                        
                        WeekEndID = weekenddef.WeekendDayId,
                        WeekEndName = weekenddef.WeekendDayName
                    }
                    ).ToList();
        }
        public List<ShiftTimeandWeekendView> GetEmployeeTimeandWeekendbyShiftID()
        {
            List<ShiftTimeandWeekendView> timeandweek = new ();
            ShiftTimeandWeekendView timeandWeekendView = new();
            List<int> ShiftDetailsId = dbContext.ShiftDetails.Where(x => x.IsActive == true).Select(x => x.ShiftDetailsId).ToList();
            foreach (int item in ShiftDetailsId)
            {
                List<WeekendShiftName> listWeekend = new();
                listWeekend = (from weekend in dbContext.WeekendDefinition
                               join shift in dbContext.ShiftWeekendDefinition on weekend.WeekendDayId equals shift.WeekendDayId
                               where shift.ShiftDetailsId == item
                               select new WeekendShiftName
                               {
                                   WeekEndID = weekend.WeekendDayId,
                                   WeekEndName = weekend.WeekendDayName,
                                   shiftId = shift.ShiftDetailsId,
                               }).ToList();

                timeandWeekendView = dbContext.ShiftDetails.Join(dbContext.TimeDefinition, sd => sd.ShiftDetailsId, td => td.ShiftDetailsId, (sd, td) => new { sd, td }).Where(r => r.sd.ShiftDetailsId == item).Select(rs => new ShiftTimeandWeekendView
                {
                    ShiftDetailsId = rs.sd.ShiftDetailsId,
                    ShiftName = rs.sd.ShiftName,
                    TotalHours = rs.td.TotalHours.ToString(),
                    WeekEndNameList = listWeekend,
                    PresentHour = rs.td.PresentHour.ToString(),
                    IsGenralShift = rs.sd.ShiftName.ToLower() == "general shift" ? true : false
                }).FirstOrDefault();

                timeandweek.Add(timeandWeekendView);
            }
            return timeandweek == null ? new List<ShiftTimeandWeekendView>() : timeandweek;
        }
    }
 }
