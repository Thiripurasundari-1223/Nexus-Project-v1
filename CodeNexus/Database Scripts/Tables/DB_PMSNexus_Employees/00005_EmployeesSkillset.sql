IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeesSkillset')
	CREATE TABLE [dbo].[EmployeesDesignationHistory](
		[DesignationHistoryId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeId] [int] NULL,
		[DesignationId] [int] NULL,		
		[DesignationName] [nvarchar](50),
		[EffiectiveFromDate] [datetime] NULL,
		[EffiectiveToDate] [datetime] NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,		
	PRIMARY KEY CLUSTERED 
	(
		[DesignationHistoryId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
