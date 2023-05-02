using Attendance.DAL.Repository;
using Attendance.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedLibraries.Authentication.Manager;

namespace Attendance.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void AttendanceDIResolver(this IServiceCollection services)
        {
            services.AddScoped<AttendanceServices>();
            services.AddScoped<IShiftDetailsRepository, ShiftDetailsRepository>();
            services.AddScoped<ITimeDefinitionRepository, TimeDefinitionRepository>();
            services.AddScoped<IWeekendDefinitionRepository, WeekendDefinitionRepository>();
            services.AddScoped<IShiftWeekendDefinitionRepository, ShiftWeekendDefinitionRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IAttendanceDetailRepository, AttendanceDetailRepository>();
            services.AddScoped<IAbsentSettingRepository, AbsentSettingRepository>();
            services.AddScoped<IAbsentDepartmentRepository, AbsentDepartmentRepository>();
            services.AddScoped<IAbsentDesignationRepository, AbsentDesignationRepository>();
            services.AddScoped<IAbsentLocationRepository, AbsentLocationRepository>();
            services.AddScoped<IAbsentRoleRepository, AbsentRoleRepository>();
            services.AddScoped<IAbsentEmployeeTypeRepository, AbsentEmployeeTypeRepository>();
            services.AddScoped<IAbsentProbationStatusRepository, AbsentProbationStatusRepository>();
            services.AddScoped<IAbsentEmployeeRepository, AbsentEmployeeRepository>();
            services.AddScoped<IAbsentRestrictionRepository, AbsentRestrictionRepository>();
        }
    }
}