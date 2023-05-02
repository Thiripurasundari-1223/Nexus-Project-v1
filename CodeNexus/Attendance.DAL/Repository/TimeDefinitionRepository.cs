using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels.Attendance;
using System.Linq;

namespace Attendance.DAL.Repository
{
    public interface ITimeDefinitionRepository : IBaseRepository<TimeDefinition>
    {
        TimeDefinition GetByID(int pTimeDefinitionId);
        TimeDefinition GetTimeDefinitionByShiftID(int shiftDetailsId);
        TimeDefinitionView GetByTimeDefinitionShiftDetailsId(int pShiftDetailsId);
        ShiftTimeDefinition GetShiftTimeDefinitionByShiftID(int shiftDetailsId);
    }
    public class TimeDefinitionRepository : BaseRepository<TimeDefinition>, ITimeDefinitionRepository
    {
        private readonly AttendanceDBContext dbContext;
        public TimeDefinitionRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public TimeDefinition GetByID(int pTimeDefinitionId)
        {
            return dbContext.TimeDefinition.Where(x => x.TimeDefinitionId == pTimeDefinitionId).FirstOrDefault();
        }
        public TimeDefinition GetTimeDefinitionByShiftID(int shiftDetailsId)
        {
            return dbContext.TimeDefinition.Where(x => x.ShiftDetailsId == shiftDetailsId).FirstOrDefault();
        }
        public TimeDefinitionView GetByTimeDefinitionShiftDetailsId(int pShiftDetailsId)
        {
            TimeDefinitionView timeDefinitionView = new TimeDefinitionView();
            TimeDefinition timeDefinition = dbContext.TimeDefinition.Where(x => x.ShiftDetailsId == pShiftDetailsId).FirstOrDefault();
            if (timeDefinition != null)
            {
                timeDefinitionView.TimeDefinitionId = timeDefinition.TimeDefinitionId;
                timeDefinitionView.TimeFrom = timeDefinition.TimeFrom == null ? "00" : timeDefinition.TimeFrom?.Hours.ToString() + ":" + timeDefinition.TimeFrom?.Minutes.ToString() + ":" + timeDefinition.TimeFrom?.Seconds.ToString();
                timeDefinitionView.TimeTo = timeDefinition.TimeTo == null ? "00" : timeDefinition.TimeTo?.Hours.ToString() + ":" + timeDefinition.TimeTo?.Minutes.ToString() + ":" + timeDefinition.TimeTo?.Seconds.ToString();
                timeDefinitionView.BreakTime = timeDefinition.BreakTime == null ? "00" : timeDefinition.BreakTime?.Hours.ToString() + ":" + timeDefinition.BreakTime?.Minutes.ToString() + ":" + timeDefinition.BreakTime?.Seconds.ToString();
                timeDefinitionView.TotalHours = timeDefinition.TotalHours == null ? "00" : timeDefinition.TotalHours?.Hours.ToString() + ":" + timeDefinition.TotalHours?.Minutes.ToString() + ":" + timeDefinition.TotalHours?.Seconds.ToString();
                timeDefinitionView.AbsentFromHour = timeDefinition.AbsentFromHour == null ? "00" : timeDefinition.AbsentFromHour?.Hours.ToString() + ":" + timeDefinition.AbsentFromHour?.Minutes.ToString() + ":" + timeDefinition.AbsentFromHour?.Seconds.ToString();
                timeDefinitionView.AbsentFromOperator = timeDefinition.AbsentFromOperator;
                timeDefinitionView.AbsentToHour = timeDefinition.AbsentToHour == null ? "00" : timeDefinition.AbsentToHour?.Hours.ToString() + ":" + timeDefinition.AbsentToHour?.Minutes.ToString() + ":" + timeDefinition.AbsentToHour?.Seconds.ToString();
                timeDefinitionView.AbsentToOperator = timeDefinition.AbsentToOperator;
                timeDefinitionView.HalfaDayFromHour = timeDefinition.HalfaDayFromHour == null ? "00" : timeDefinition.HalfaDayFromHour?.Hours.ToString() + ":" + timeDefinition.HalfaDayFromHour?.Minutes.ToString() + ":" + timeDefinition.HalfaDayFromHour?.Seconds.ToString();
                timeDefinitionView.HalfaDayFromOperator = timeDefinition.HalfaDayFromOperator;
                timeDefinitionView.HalfaDayToHour = timeDefinition.HalfaDayToHour == null ? "00" : timeDefinition.HalfaDayToHour?.Hours.ToString() + ":" + timeDefinition.HalfaDayToHour?.Minutes.ToString() + ":" + timeDefinition.HalfaDayToHour?.Seconds.ToString();
                timeDefinitionView.HalfaDayToOperator = timeDefinition.HalfaDayToOperator;
                timeDefinitionView.PresentHour = timeDefinition.PresentHour == null ? "00" : timeDefinition.PresentHour?.Hours.ToString() + ":" + timeDefinition.PresentHour?.Minutes.ToString() + ":" + timeDefinition.PresentHour?.Seconds.ToString();
                timeDefinitionView.PresentOperator = timeDefinition.PresentOperator;
                timeDefinitionView.IsConsiderAbsent = timeDefinition.IsConsiderAbsent;
                timeDefinitionView.IsConsiderPresent = timeDefinition.IsConsiderPresent;
                timeDefinitionView.IsConsiderHalfaDay = timeDefinition.IsConsiderHalfaDay;
            }
            return timeDefinitionView;
        }

        public ShiftTimeDefinition GetShiftTimeDefinitionByShiftID(int shiftDetailsId)
        {
            return dbContext.TimeDefinition.Where(x => x.ShiftDetailsId == shiftDetailsId).Select(x=> new ShiftTimeDefinition
            { 
                TotalHours=x.TotalHours.ToString(),
                PresentHour=x.PresentHour.ToString()
            }).FirstOrDefault();
        }
    }
}
