USE [PMSNexus_Appraisal]
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='WorkDayDetail')
BEGIN
	CREATE TABLE [dbo].[WorkDayDetail](
	[WorkDayDetailId] [int] IDENTITY(1,1) NOT NULL,
	[WorkDayId] [int] NOT NULL,
	[WorkdayKRAId] [int] NOT NULL,
	[WorkDate] [datetime] NULL,
	[WorkHours] Time(0) NULL,
	[ProjectId] [int] NULL,
	[ProjectName] [VARCHAR](500) NULL,
	[EmployeeRemark] [VARCHAR](2000) NULL,	
	[Status] [VARCHAR](500) NULL,
	[ApproverId] [int] NULL default(0),
	[ApproverName] [VARCHAR](500) NULL,
	[ApproverRemark] [VARCHAR](500) NULL,
	[ApprovedDate] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkDayDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO