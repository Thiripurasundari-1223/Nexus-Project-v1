USE [PMSNexus_Employees]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='WorkHistory')
	CREATE TABLE [dbo].[WorkHistory](
		[WorkHistoryId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeId] [int]  NULL,
		[OrganizationName] [varchar](50) NULL,
		[Designation] [varchar](20) NULL,
		[EmployeeTypeId] [int] NULL,
		[StartDate] [datetime] NULL,
		[EndDate] [datetime] NULL,
		[LastCTC] [DECIMAL](18,2) NULL,
		[LeavingReason] [Nvarchar](max) NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[WorkHistoryId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
 