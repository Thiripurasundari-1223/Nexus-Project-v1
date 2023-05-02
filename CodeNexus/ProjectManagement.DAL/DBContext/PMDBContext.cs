using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Projects;

namespace ProjectManagement.DAL.DBContext
{
    public class PMDBContext : DbContext
    {
        public PMDBContext(DbContextOptions<PMDBContext> options) : base(options)
        { }
        public DbSet<ProjectDetails> ProjectDetails { get; set; }
        public DbSet<ChangeRequest> ChangeRequest { get; set; }
        public DbSet<ResourceAllocation> ResourceAllocation { get; set; }
        public DbSet<CurrencyType> CurrencyType { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        public DbSet<RateFrequency> RateFrequency { get; set; }
        public DbSet<Allocation> Allocation { get; set; }
        ///public DbSet<RequiredSkillSet> RequiredSkillSet { get; set; }
        public DbSet<ProjectDetailComments> ProjectDetailComments { get; set; }
        public DbSet<ChangeRequestType> ChangeRequestType { get; set; }

        public DbSet<ProjectAudit> projectAudits { get; set; }

        public DbSet<AppConstants> AppConstants { get; set; }
       
         public DbSet<CustomerSPOC> CustomerSPOC { get; set; }
        public DbSet<CustomerSPOCDetails> CustomerSPOCDetails { get; set; }
        public DbSet<FixedIteration> FixedIteration { get; set; }
        public DbSet<ProjectAudit> ProjectAudit { get; set; }
       public DbSet<ProjectDocument> ProjectDocument { get; set; }
        public DbSet<ProjectRole> ProjectRole { get; set; }
       public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<VersionChangeRequest> VersionChangeRequest { get; set; }
        public DbSet<VersionCustomerSPOCDetails> VersionCustomerSPOCDetails { get; set; }
        public DbSet<VersionFixedIteration> VersionFixedIteration { get; set; }
        public DbSet<VersionProjectDetail> VersionProjectDetail { get; set; }
        public DbSet<VersionProjectDetailComments> VersionProjectDetailComments { get; set; }
        public DbSet<VersionProjectDocument> VersionProjectDocument { get; set; }
        public DbSet<VersionResourceAllocation> VersionResourceAllocation { get; set; }

     

    }
}