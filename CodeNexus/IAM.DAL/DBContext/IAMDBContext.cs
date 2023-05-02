using IAM.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System.Collections.Generic;

namespace IAM.DAL.DBContext
{
    public class IAMDBContext : DbContext
    {
        public IAMDBContext(DbContextOptions<IAMDBContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<RoleSetup> RoleSetup { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserAccessBag> UserAccessesbag { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        //Employees
        public DbSet<Employees> Employees { get; set; }
        public DbSet<EmployeesTypes> EmployeesType { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Skillsets> Skillset { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<EmployeesSkillset> EmployeesSkillset { get; set; }
        //Roles related
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<ModuleFeatureMapping> ModuleFeatureMapping { get; set; }
        public DbSet<RoleReports> RoleReports { get; set; }
        public DbSet<DepartmentReportMapping> DepartmentReportMapping { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<EmployeeLocation> EmployeeLocation { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<EmployeeCategory> EmployeeCategory { get; set; }
        public DbSet<EmployeeRelationship> EmployeeRelationship { get; set; }
        public DbSet<EmployeeDependent> EmployeeDependent { get; set; }
        public DbSet<EmployeeShiftDetails> EmployeeShiftDetails { get; set; }
        public DbSet<ProbationStatus> ProbationStatus { get; set; }
        public DbSet<SystemRoles> SystemRoles { get; set; }
        public DbSet<WorkHistory> WorkHistory { get; set; }
        public DbSet<EducationDetail> EducationDetail { get; set; }
        public DbSet<CompensationDetail> CompensationDetail { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocument { get; set; }
        public DbSet<EmployeeAudit> EmployeeAudit { get; set; }
        public DbSet<EmployeeAppConstants> EmployeeAppConstants { get; set; }
        public DbSet<EmployeesPersonalInfo> EmployeesPersonalInfo { get; set; }
        public DbSet<EmployeeCountry> EmployeeCountry { get; set; }
        public DbSet<EmployeeState> EmployeeState { get; set; }
        public DbSet<SkillsetCategory> SkillsetCategory { get; set; }
        public DbSet<SkillsetHistory> SkillsetHistory { get; set; }
        public DbSet<EmployeeRequest> EmployeeRequest { get; set; }
        public DbSet<PreventAbsentNotification> PreventAbsentNotification { get; set; }
        public DbSet<EmployeesDesignationHistory> EmployeesDesignationHistory { get; set; }
        public DbSet<EmployeeNationality> EmployeeNationality { get; set; }
        public DbSet<EmployeeSpecialAbility> EmployeeSpecialAbility { get; set; }
        public DbSet<EmployeeRequestDetail> EmployeeRequestDetails { get; set; }
        public DbSet<EmployeeRequestDocument> EmployeeRequestDocument { get; set; }

        //emailrelated
        public DbSet<EmployeeMasterEmailTemplate> EmployeeMasterEmailTemplate { get; set; }
        public DbSet<NoticePeriodCategory> NoticePeriodCategory { get; set; }
    }
}