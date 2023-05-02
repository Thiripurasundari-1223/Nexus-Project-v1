using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace PolicyManagement.DAL.Models
{
    public class PolicyAcknowledged
    {
        [Key]
        public int PolicyAcknowledgedId { get; set; }
        public int? EmployeeId { get; set; }
        public int? PolicyDocumentId { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public int? AcknowledgedBy { get; set; }
        public string? AcknowledgedStatus { get; set; }
    }
    public class PolicyAcknowledgedConfiguration : IEntityTypeConfiguration<PolicyAcknowledged>
    {
        public void Configure(EntityTypeBuilder<PolicyAcknowledged> builder)
        {
            builder.ToTable("PolicyAcknowledged");
            builder.HasKey(o => o.PolicyAcknowledgedId);
        }
    }
}