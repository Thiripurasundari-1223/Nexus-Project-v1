USE [PMSNexus_Employees]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeAudit')
	CREATE TABLE [dbo].[EmployeeAudit](
		[EmployeeAuditId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeID] [int]  NULL,
		[ActionType] [varchar](50) NULL,
		[Field] [varchar](50) NULL,
        [OldValue] [varchar](max) NULL,
		[NewValue] [varchar](max) NULL,
		[Status] [varchar](50) NULL,
		[Remark] [varchar] (max) NULL,
		[ApprovedOn] [datetime] NULL,
		[ApprovedById] [int] NULL,
		[ChangeRequestID] [uniqueidentifier]  NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
	
	PRIMARY KEY CLUSTERED 
	(
		[EmployeeAuditId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
 
