IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='TimesheetConfigurationDetails')
CREATE TABLE [dbo].[TimesheetConfigurationDetails](
[TimesheetConfigurationId] [int] IDENTITY(1,1) NOT NULL,
[TimesheetSubmissionDayId] [int] NULL,
[TimesheetSubmissionTime] [Time](0) NULL,
[TimesheetAlertSubmissionFromDayID] [int] NULL,
[TimesheetAlertSubmissionToDayID] [int] NULL,
[TimesheetApprovalFromDayID] [int] NULL,
[TimesheetApprovalToDayID] [int] NULL,
[TimesheetAlertApprovalFromDayID] [int] NULL,
[TimesheetAlertApprovalToDayID] [int] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
	(
		[TimesheetConfigurationId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO