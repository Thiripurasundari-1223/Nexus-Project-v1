IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='TimesheetLog')
BEGIN
	CREATE TABLE [dbo].[TimesheetLog](
	[TimesheetLogId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[ResourceId] [int] NOT NULL,
	[PeriodSelection] [datetime] NOT NULL,
	[RequiredHours] Time(0) NULL,
	[ClockedHours] Time(0) NULL,
	[Comments] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[TimesheetId] [int] NULL,
	[IsSubmitted] [bit] NULL,
	[IsApproved] [bit] NULL,
	[IsRejected] [bit] NULL,
	[WeekTimesheetId] uniqueidentifier NULL,
PRIMARY KEY CLUSTERED 
(
	[TimesheetLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO