using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace PolicyManagement.DAL.Models
{
    public class SharePolicyDocuments
    {
        [Key]
        public int SharePolicyDocumentId { get; set; }
        public int? PolicyDocumentId { get; set; }
        public int? DepartmentId { get; set; }
        public int? LocationId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class SharePolicyDocumentConfiguration : IEntityTypeConfiguration<SharePolicyDocuments>
    {
        public void Configure(EntityTypeBuilder<SharePolicyDocuments> builder)
        {
            builder.ToTable("SharePolicyDocuments");
            builder.HasKey(o => o.SharePolicyDocumentId);
        }
    }
}