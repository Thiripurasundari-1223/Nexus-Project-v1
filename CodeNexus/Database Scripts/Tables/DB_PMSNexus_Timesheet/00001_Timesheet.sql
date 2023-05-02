IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Timesheet')
BEGIN
	CREATE TABLE [dbo].[Timesheet](
	[TimesheetId] [int] IDENTITY(1,1) NOT NULL,
	[ReportingPersonId] [int] NOT NULL,
	[TotalClockedHours] Time(0) NULL,
	[TotalApprovedHours] Time(0) NULL,
	[IsBillable] [BIT] NULL,
	[RejectionReasonId] [INT] NULL,
	[OtherReasonForRejection] [VARCHAR](2000) NULL,
	[WeekTimesheetId] uniqueidentifier NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TimesheetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO