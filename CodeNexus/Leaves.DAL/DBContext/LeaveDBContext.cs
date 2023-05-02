using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaves.DAL.DBContext
{
    public class LeaveDBContext : DbContext
    {
        public LeaveDBContext(DbContextOptions<LeaveDBContext> options) : base(options)
        { }
        public DbSet<Holiday> Holiday { get; set; }
        public DbSet<LeaveTypes> LeaveTypes { get; set; }
        public DbSet<LeaveApplicable> LeaveApplicable { get; set; }
        public DbSet<LeaveRestrictions> LeaveRestrictions { get; set; }
        public DbSet<LeaveEntitlement> LeaveEntitlement { get; set; }
        public DbSet<LeaveMaxLimitAction> LeaveMaxLimitAction { get; set; }
        public DbSet<ApplyLeaves> ApplyLeave { get; set; }
        public DbSet<AppliedLeaveDetails> AppliedLeaveDetails { get; set; }
        public DbSet<LeaveRejectionReason> LeaveRejectionReason { get; set; }
        public DbSet<HolidayShift> HolidayShift { get; set; }
        public DbSet<HolidayDepartment> HolidayDepartment { get; set; }
        public DbSet<HolidayLocation> HolidayLocation { get; set; }
        public DbSet<LeaveDepartment> LeaveDepartment { get; set; }
        public DbSet<LeaveDesignation> LeaveDesignation { get; set; }
        public DbSet<LeaveLocation> LeaveLocation { get; set; }
        public DbSet<LeaveRole> LeaveRole { get; set; }
        public DbSet<EmployeeApplicableLeave> EmployeeApplicableLeave { get; set; }
        public DbSet<LeaveDuration> LeaveDuration { get; set; }
        public DbSet<AppConstants> AppConstants { get; set; }
        public DbSet<EmployeeLeaveDetails> EmployeeLeaveDetails { get; set; }
        public DbSet<LeaveTakenTogether> LeaveTakenTogether { get; set; }
        public DbSet<ProRateMonthDetails> ProRateMonthDetails { get; set; }
        public DbSet<SpecificEmployeeDetailLeave> SpecificEmployeeDetailLeave { get; set; }
        public DbSet<LeaveCarryForward> LeaveCarryForward { get; set; }
        public DbSet<LeaveGrantRequestDetails> LeaveGrantRequestDetails { get; set; }
        public DbSet<LeaveGrantDocumentDetails> LeaveGrantDocumentDetails { get; set; }
        public DbSet<GrantLeaveApproval> GrantLeaveApproval { get; set; }
        public DbSet<EmployeeGrantLeaveApproval> EmployeeGrantLeaveApproval { get; set; }
        public DbSet<LeaveAdjustmentDetails> LeaveAdjustmentDetails { get; set; }
        public DbSet<LeaveEmployeeType> LeaveEmployeeType { get; set; }
        public DbSet<LeaveProbationStatus> LeaveProbationStatus { get; set; }
    }
}
