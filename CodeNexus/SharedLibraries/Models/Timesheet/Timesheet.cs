using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Timesheet
{
    public class Timesheet
    {
        [Key]
        public int TimesheetId { get; set; }
        public int ReportingPersonId { get; set; }
        public string TotalClockedHours { get; set; }
        public string TotalApprovedHours { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsBillable { get; set; }
        public int? RejectionReasonId { get; set; }
        public string OtherReasonForRejection { get; set; }
        public Guid? WeekTimesheetId { get; set; }
        public string TotalRequiredHours { get; set; }
    }
    public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
    {
        public void Configure(EntityTypeBuilder<Timesheet> builder)
        {
            builder.ToTable("Timesheet");
            builder.HasKey(o => o.TimesheetId);
        }
    }
}