using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExitManagement.DAL.DBContext
{
    public class ExitManagementDBContext : DbContext
    {
        public ExitManagementDBContext(DbContextOptions<ExitManagementDBContext> options) : base(options)
        { }
        public DbSet<EmployeeResignationDetails> EmployeeResignationDetails { get; set; }

        public DbSet<ResignationReason> ResignationReason { get; set; }
        public DbSet<ResignationApproval> ResignationApproval { get; set; }
        public DbSet<ResignationApprovalStatus> ResignationApprovalStatus { get; set; }
        public DbSet<AppConstants> AppConstants { get; set; }
        public DbSet<ExitManagementEmailTemplate> ExitManagementEmailTemplate { get; set; }
        public DbSet<ResignationInterview> ResignationInterview { get; set; }
        public DbSet<ResignationFeedbackToManagement> ResignationFeedbackToManagement { get; set; }
        public DbSet<ReasonLeavingPosition> ReasonLeavingPosition { get; set; }
        public DbSet<ResignationChecklist> ResignationChecklist { get; set; }
        public DbSet<ManagerCheckList> ManagerCheckList { get; set; }
        public DbSet<PMOCheckList> PMOCheckList { get; set; }
        public DbSet<ITCheckList> ITCheckList { get; set; }
        public DbSet<AdminCheckList> AdminCheckList { get; set; }
        public DbSet<FinanceCheckList> FinanceCheckList { get; set; }
        public DbSet<HRCheckList> HRCheckList { get; set; }
        public DbSet<CheckListApprover> CheckListApprover { get; set; }
        public DbSet<CheckListView> CheckListView { get; set; }

    }
}
