using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace PolicyManagement.DAL.Models
{
    public class DocumentTag
    {
        [Key]
        public int DocumentTagId { get; set; }
        public string? TagName { get; set; }
        public string? PlaceHolderName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class DocumentTagConfiguration : IEntityTypeConfiguration<DocumentTag>
    {
        public void Configure(EntityTypeBuilder<DocumentTag> builder)
        {
            builder.ToTable("DocumentTag");
            builder.HasKey(o => o.DocumentTagId);
        }
    }
}