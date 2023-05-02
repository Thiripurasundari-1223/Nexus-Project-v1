USE [PMSNexus_Leaves]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeGrantLeaveApproval')
	CREATE TABLE [dbo].[EmployeeGrantLeaveApproval](
		[EmployeeGrantLeaveApprovalId] [int] IDENTITY(1,1) NOT NULL,
		[LeaveGrantDetailId] [int] NULL,
		[ApproverEmployeeId] [int] NULL,
		[LevelId] [int] NULL,
		[Comments] Nvarchar (Max) NULL,
		[Status] [nvarchar](500) NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,		
	PRIMARY KEY CLUSTERED 
	(
		[EmployeeGrantLeaveApprovalId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
