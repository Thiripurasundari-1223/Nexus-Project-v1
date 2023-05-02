USE [PMSNexus_Leaves]
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AppliedLeaveDetails')
CREATE TABLE [dbo].[AppliedLeaveDetails](
	[AppliedLeaveDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NULL,
	[IsFullDay] [bit] NULL,
	[IsFirstHalf] [bit] NULL,
	[IsSecondHalf] [bit] NULL,
	[LeaveId] [int] NULL,
	[CompensatoryOffId] [int] NULL,
	[AppliedLeaveStatus] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 PRIMARY KEY CLUSTERED 
	(
		[AppliedLeaveDetailsID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO