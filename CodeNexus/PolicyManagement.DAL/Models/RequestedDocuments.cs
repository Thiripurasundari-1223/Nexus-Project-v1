using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace PolicyManagement.DAL.Models
{
    public class RequestedDocuments
    {
        [Key]
        public int RequestedDocumentId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? OtherDocumentType { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }
        public string? RejectedReason { get; set; }
        public DateTime? ApprovedOrRejectedOn { get; set; }
        public int? ApprovedOrRejectedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? PolicyDocumentId { get; set; }
    }
    public class RequestedDocumentsConfiguration : IEntityTypeConfiguration<RequestedDocuments>
    {
        public void Configure(EntityTypeBuilder<RequestedDocuments> builder)
        {
            builder.ToTable("RequestedDocuments");
            builder.HasKey(o => o.RequestedDocumentId);
        }
    }
}