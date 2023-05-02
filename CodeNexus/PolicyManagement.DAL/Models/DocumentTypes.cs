using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace PolicyManagement.DAL.Models
{
    public class DocumentTypes
    {
        [Key]
        public int DocumentTypeId { get; set; }
        public string? DocumentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string? TemplateFile { get; set; }
        public string? SignatureFile { get; set; }
    }
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentTypes>
    {
        public void Configure(EntityTypeBuilder<DocumentTypes> builder)
        {
            builder.ToTable("DocumentTypes");
            builder.HasKey(o => o.DocumentTypeId);
        }
    }
}