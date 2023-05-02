using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Attendance.DAL.Repository
{
    public interface IAbsentSettingRepository : IBaseRepository<AbsentSetting>
    {
        AbsentSetting GetAbsentSettingById(int absentSettingId);
        AbsentSettingsView GetAbsentSetting();
    }
    public class AbsentSettingRepository : BaseRepository<AbsentSetting>, IAbsentSettingRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentSettingRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public AbsentSetting GetAbsentSettingById(int absentSettingId)
        {
            return dbContext.AbsentSetting.Where(x => x.AbsentSettingId == absentSettingId).FirstOrDefault();
        }
        public AbsentSettingsView GetAbsentSetting()
        {
            return (from AbsentSetting in dbContext.AbsentSetting
                    select new AbsentSettingsView
                    {
                        AbsentSettingId = AbsentSetting.AbsentSettingId,
                        Gender_Male_Applicable = AbsentSetting.Gender_Male_Applicable,
                        Gender_Female_Applicable = AbsentSetting.Gender_Female_Applicable,
                        Gender_Others_Applicable = AbsentSetting.Gender_Others_Applicable,
                        MaritalStatus_Single_Applicable = AbsentSetting.MaritalStatus_Single_Applicable,
                        MaritalStatus_Married_Applicable = AbsentSetting.MaritalStatus_Married_Applicable,
                        Gender_Male_Exception = AbsentSetting.Gender_Male_Exception,
                        Gender_Female_Exception = AbsentSetting.Gender_Female_Exception,
                        Gender_Others_Exception = AbsentSetting.Gender_Others_Exception,
                        MaritalStatus_Single_Exception = AbsentSetting.MaritalStatus_Single_Exception,
                        MaritalStatus_Married_Exception = AbsentSetting.MaritalStatus_Married_Exception
                    }).FirstOrDefault();
        }
    }
}
