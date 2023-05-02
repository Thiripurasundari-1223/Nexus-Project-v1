USE [PMSNexus_ExitManagement]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ResignationApprovalStatus')
	CREATE TABLE [dbo].[ResignationApprovalStatus](
		[ResignationApprovalStatusId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeResignationDetailsId] [int] NULL,
		[ApproverEmployeeId] [int] NULL,
		[ApprovalType] Nvarchar(100) NULL,
		[ApprovedBy] [int] NULL,
		[LevelId] [int] NULL,
		[FeedBack] Nvarchar (Max) NULL,
		[Status] [nvarchar](500) NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
                [ApproverType] NVarchar(250) NULL,	
	PRIMARY KEY CLUSTERED 
	(
		[ResignationApprovalStatusId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
