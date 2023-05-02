using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Attendance.DAL.Repository
{
    public interface IWeekendDefinitionRepository : IBaseRepository<ShiftWeekendDefinition>
    {
        ShiftWeekendDefinition GetByID(int pWeekendDayId);
        List<WeekendDefinitionView> GetByWeekendDefinitionShiftDetailsId(int pShiftDetailsId);
    }
    public class WeekendDefinitionRepository : BaseRepository<ShiftWeekendDefinition>, IWeekendDefinitionRepository
    {
        private readonly AttendanceDBContext dbContext;
        public WeekendDefinitionRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public ShiftWeekendDefinition GetByID(int pWeekendDayId)
        {
            return dbContext.ShiftWeekendDefinition.Where(x => x.WeekendDayId == pWeekendDayId).FirstOrDefault();
        }
        public List<WeekendDefinitionView> GetByWeekendDefinitionShiftDetailsId(int pShiftDetailsId)
        {
            List<WeekendDefinitionView> weekendDefinitionViews = new List<WeekendDefinitionView>();
            List<ShiftWeekendDefinition> weekendDefinitions = dbContext.ShiftWeekendDefinition.Where(x => x.ShiftDetailsId == pShiftDetailsId).ToList();
           foreach(ShiftWeekendDefinition shiftWeekendDefinition in weekendDefinitions)
            {
                WeekendDefinitionView weekendDefinitionView = new WeekendDefinitionView
                {
                    ShiftWeekendDefinitionId = shiftWeekendDefinition.ShiftWeekendDefinitionId,
                    WeekendDayId = shiftWeekendDefinition.WeekendDayId,
                    ShiftDetailsId = shiftWeekendDefinition.ShiftDetailsId,
                    CreatedOn = shiftWeekendDefinition.CreatedOn,
                    ModifiedOn = shiftWeekendDefinition.ModifiedOn,
                    CreatedBy = shiftWeekendDefinition.CreatedBy,
                    ModifiedBy = shiftWeekendDefinition.ModifiedBy
                };
                weekendDefinitionViews.Add(weekendDefinitionView);
            }
            return weekendDefinitionViews;
        }

        
    }
}
