IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Department')
	CREATE TABLE [dbo].[Department](
		[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
		[DepartmentName] [varchar](250) NULL,	
		[DepartmentShortName] [varchar](100) NULL,	
		[DepartmentDescription] [varchar](Max) NULL,	
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,		
	PRIMARY KEY CLUSTERED 
	(
		[DepartmentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

