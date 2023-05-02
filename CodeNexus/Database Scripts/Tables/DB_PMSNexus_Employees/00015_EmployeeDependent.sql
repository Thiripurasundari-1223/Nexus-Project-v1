IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeDependent')
	CREATE TABLE [dbo].[EmployeeDependent] (
		[EmployeeDependentId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeRelationName] [varchar](250) NULL,
		[EmployeeRelationshipId] [int] NULL,
		[EmployeeRelationDateOfBirth] [datetime] NULL,
		[EmployeeID] [int] NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[EmployeeDependentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO