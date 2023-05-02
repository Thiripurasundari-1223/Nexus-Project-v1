using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.PolicyManagement
{
    public class PolicyDocuments
    {
        [Key]
        public int PolicyDocumentId { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public int? FolderId { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool? Acknowledgement { get; set; }
        public bool? IsEmployeeAbleToDownload { get; set; }
        public bool? IsNotifyViaEmail { get; set; }
        public bool? IsNotifyViaFeeds { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string FilePath { get; set; }
    }
    public class PolicyDocumentsConfiguration : IEntityTypeConfiguration<PolicyDocuments>
    {
        public void Configure(EntityTypeBuilder<PolicyDocuments> builder)
        {
            builder.ToTable("PolicyDocuments");
            builder.HasKey(o => o.PolicyDocumentId);
        }
    }
}