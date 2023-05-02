USE [PMSNexus_Employees]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeRequest')
	CREATE TABLE [dbo].[EmployeeRequest](
		[EmployeeRequestId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeID] [int]  NULL,
		[RequestCategory] [varchar](50) NULL,
		[ChangeRequestId] [uniqueidentifier]  NULL,
		[Status] [varchar] (20) NULL,
		[ChangeType] [varchar] (20) NULL,
		[Remark] [nvarchar] (max) NULL,
		[ApprovedBy] [int]  NULL,
		[SourceId] [int]  NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[ApprovedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[EmployeeRequestId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
 
