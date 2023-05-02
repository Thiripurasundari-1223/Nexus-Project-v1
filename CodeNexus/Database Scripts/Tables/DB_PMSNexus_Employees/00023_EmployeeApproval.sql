USE [PMSNexus_Employees]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeApproval')
	CREATE TABLE [dbo].[EmployeeApproval](
		[EmployeeApprovalId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeID] [int]  NULL,
		[Field] [varchar](50) NULL,
        [OldValue] [varchar](max) NULL,
		[NewValue] [varchar](max) NULL,
		[CRID] [uniqueidentifier]  NULL,
		[Status] [varchar] (20) NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[EmployeeApprovalId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
 
