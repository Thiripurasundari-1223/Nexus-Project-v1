using Leaves.DAL.Repository;
using Leaves.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leaves.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void LeaveDIResolver(this IServiceCollection services)
        {
            services.AddScoped<LeaveServices>();
            services.AddScoped<IHolidayRepository, HolidayRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<ILeaveEntitlementRepository, LeaveEntitlementRepository>();
            services.AddScoped<ILeaveApplicableRepository, LeaveApplicableRepository>();
            services.AddScoped<ILeaveRestrictionsRepository, LeaveRestrictionsRepository>();
            services.AddScoped<ILeaveApplyRepository, LeaveApplyRepository>();
            services.AddScoped<IAppliedLeaveDetailsRepository, AppliedLeaveDetailsRepository>();
            services.AddScoped<ILeaveRejectionReasonRepository, LeaveRejectionReasonRepository>();
            services.AddScoped<IHolidayDepartmentRepository, HolidayDepartmentRepository>();
            services.AddScoped<IHolidayLocationRepository, HolidayLocationRepository>();
            services.AddScoped<IHolidayShiftRepository, HolidayShiftRepository>();
            services.AddScoped<ILeaveDepartmentRepository, LeaveDepartmentRepository>();
            services.AddScoped<ILeaveDesignationRepository, LeaveDesignationRepository>();
            services.AddScoped<ILeaveLocationRepository, LeaveLocationRepository>();
            services.AddScoped<ILeaveRoleRepository, LeaveRoleRepository>();
            services.AddScoped<IEmployeeApplicableLeaveRepository, EmployeeApplicableLeaveRepository>();
            services.AddScoped<ILeaveDurationRepository, LeaveDurationRepository>();
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<IEmployeeLeaveDetailsRepository, EmployeeLeaveDetailsRepository>();
            services.AddScoped<ILeaveTakenTogetherRepository, LeaveTakenTogetherRepository>();
            services.AddScoped<IProRateMonthDetailsRepository, ProRateMonthDetailsRepository>();
            services.AddScoped<IAppConstantsRepository, AppConstantsRepository>();
            services.AddScoped<ISpecificEmployeeDetailLeaveRepository, SpecificEmployeeDetailLeaveRepository>();
            services.AddScoped<ILeaveCarryForwardRepository, LeaveCarryForwardRepository>();
            services.AddScoped<ILeaveGrantRequestDetailsRepository, LeaveGrantRequestDetailsRepository>();
            services.AddScoped<ILeaveGrantDocumentDetailsRepository, LeaveGrantDocumentDetailsRepository>();
            services.AddScoped<IGrantLeaveApprovalRepository, GrantLeaveApprovalRepository>();
            services.AddScoped<IEmployeeGrantLeaveApprovalRepository, EmployeeGrantLeaveApprovalRepository>();
            services.AddScoped<ILeaveAdjustmentDetailsRepository, LeaveAdjustmentDetailsRepository>();
            services.AddScoped<ILeaveEmployeeTypeRepository, LeaveEmployeeTypeRepository>();
            services.AddScoped<ILeaveProbationStatuRepository, LeaveProbationStatuRepository>();
        }
    }
}
