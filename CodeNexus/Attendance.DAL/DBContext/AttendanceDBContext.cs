using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attendance.DAL.DBContext
{
    public class AttendanceDBContext: DbContext
    {
        public AttendanceDBContext(DbContextOptions<AttendanceDBContext> options) : base(options)
        { }
        public DbSet<WeekendDefinition> WeekendDefinition { get; set; }
        public DbSet<ShiftWeekendDefinition> ShiftWeekendDefinition { get; set; }
        public DbSet<TimeDefinition> TimeDefinition { get; set; }
        public DbSet<ShiftDetails> ShiftDetails { get; set; }
        public DbSet<SharedLibraries.Models.Attendance.Attendance> Attendance { get; set; }
        public DbSet<AttendanceDetail> AttendanceDetail { get; set; }
        public DbSet<SharedLibraries.Models.Employee.EmployeeShiftDetails> EmployeeShiftDetails { get; set; }
        public DbSet<AbsentSetting> AbsentSetting { get; set; }
        public DbSet<AbsentDepartment> AbsentDepartment { get; set; }
        public DbSet<AbsentDesignation> AbsentDesignation { get; set; }
        public DbSet<AbsentLocation> AbsentLocation { get; set; }
        public DbSet<AbsentRole> AbsentRole { get; set; }
        public DbSet<AbsentEmployeeType> AbsentEmployeeType { get; set; }
        public DbSet<AbsentProbationStatus> AbsentProbationStatus { get; set; }
        public DbSet<AbsentEmployee> AbsentEmployee { get; set; }
        public DbSet<AbsentRestrictions> AbsentRestrictions { get; set; }
    }
}
