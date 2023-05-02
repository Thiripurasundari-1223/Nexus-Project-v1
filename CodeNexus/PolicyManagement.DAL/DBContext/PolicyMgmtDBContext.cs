using Microsoft.EntityFrameworkCore;
using PolicyManagement.DAL.Models;

namespace PolicyManagement.DAL.DBContext
{
    public class PolicyMgmtDBContext : DbContext
    {
        public PolicyMgmtDBContext(DbContextOptions<PolicyMgmtDBContext> options) : base(options) { }
        public DbSet<PolicyDocuments> PolicyDocuments { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<SharePolicyDocuments> SharePolicyDocuments { get; set; }
        public DbSet<DocumentTag> DocumentTag { get; set; }
        public DbSet<DocumentTypes> DocumentTypes { get; set; }
        public DbSet<RequestedDocuments> RequestedDocuments { get; set; }
        public DbSet<PolicyAcknowledged> PolicyAcknowledged { get; set; }
        public DbSet<Announcement> Announcement { get; set; }
    }
}